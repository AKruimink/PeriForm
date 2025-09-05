using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using PeriForm.Domain.WinApi;
using PeriForm.Domain.WinApi.Types;

namespace PeriForm.Domain.Hotkey;

/// <summary>
/// Registers global hot‑keys and raises events when triggered. Uses a message‑only window.
/// </summary>
public sealed class HotKeyService : IHotKeyService
{
    private const int WM_HOTKEY = 0x0312;
    private readonly IWinApiService _winApi;

    private readonly ConcurrentDictionary<int, (string Id, bool IsStart)> _registrations = new();
    private readonly ConcurrentDictionary<string, (int StartId, int? StopId)> _keysByName = new();
    private readonly ConcurrentQueue<Action> _pendingOps = new();
    private readonly AutoResetEvent _opSignal = new(false);

    private IntPtr _hwnd;
    private Thread? _messageLoopThread;
    private int _nextId;
    private bool _disposed;

    public event EventHandler<HotKeyEventArgs>? HotKeyPressed;

    public HotKeyService(IWinApiService winApi)
    {
        _winApi = winApi ?? throw new ArgumentNullException(nameof(winApi));
        StartMessageLoop();
    }

    public void RegisterHotKey(string id, string hotKeyDefinition)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(hotKeyDefinition))
            throw new ArgumentException("id and hotKeyDefinition must be non‑empty.");

        var (modifiers, vk) = ParseHotKeyDefinition(hotKeyDefinition);

        // Enqueue the registration to be performed on the message loop thread
        _pendingOps.Enqueue(() =>
        {
            var newId = Interlocked.Increment(ref _nextId);
            if (!_winApi.RegisterHotKey(_hwnd, newId, modifiers, vk))
            {
                int error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException(
                    $"Could not register hot key '{hotKeyDefinition}'. Win32 error {error}");
            }
            _registrations[newId] = (id, true);
            _keysByName[id] = (newId, null);
        });
        _opSignal.Set();
    }

    public void UnregisterHotKey(string id)
    {
        _pendingOps.Enqueue(() =>
        {
            if (_keysByName.TryRemove(id, out var tuple))
            {
                _winApi.UnregisterHotKey(_hwnd, tuple.StartId);
                _registrations.TryRemove(tuple.StartId, out _);
                if (tuple.StopId.HasValue)
                {
                    _winApi.UnregisterHotKey(_hwnd, tuple.StopId.Value);
                    _registrations.TryRemove(tuple.StopId.Value, out _);
                }
            }
        });
        _opSignal.Set();
    }

    private void StartMessageLoop()
    {
        _messageLoopThread = new Thread(() =>
        {
            // Create the message‑only window on this thread
            _hwnd = _winApi.CreateMessageOnlyWindow("PeriFormHotKeyWindow");

            NativeTypes.MSG msg;
            while (!_disposed)
            {
                // First, drain any pending registration/unregistration operations
                while (_pendingOps.TryDequeue(out var op))
                {
                    op();
                }

                // Peek for messages; if none, wait for either a message or a pending operation
                if (NativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, NativeMethods.PM_REMOVE))
                {
                    if (msg.message == WM_HOTKEY)
                    {
                        int id = msg.wParam.ToInt32();
                        if (_registrations.TryGetValue(id, out var tuple))
                        {
                            HotKeyPressed?.Invoke(this, new HotKeyEventArgs(tuple.Id, tuple.IsStart));
                        }
                    }
                    _winApi.TranslateMessage(ref msg);
                    _winApi.DispatchMessage(ref msg);
                }
                else
                {
                    // No messages; wait until there is a pending operation or a message
                    _opSignal.WaitOne(50);
                }
            }
        })
        {
            IsBackground = true,
            Name = "HotKeyServiceMessageLoop"
        };
        _messageLoopThread.Start();
    }

    private (uint Modifiers, uint Vk) ParseHotKeyDefinition(string definition)
    {
        uint modifiers = 0;
        uint vk = 0;
        var parts = definition.Split('+', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var token = part.Trim().ToUpperInvariant();
            switch (token)
            {
                case "CTRL":
                case "CONTROL":
                    modifiers |= _winApi.ModControl;
                    break;

                case "ALT":
                    modifiers |= _winApi.ModAlt;
                    break;

                case "SHIFT":
                    modifiers |= _winApi.ModShift;
                    break;

                case "WIN":
                case "WINDOWS":
                    modifiers |= _winApi.ModWin;
                    break;

                default:
                    // Handle F1–F24
                    if (token.StartsWith("F") &&
                        int.TryParse(token.Substring(1), out var fn) &&
                        fn >= 1 && fn <= 24)
                    {
                        vk = (uint)(0x70 + (fn - 1));
                    }
                    else if (token.Length == 1)
                    {
                        vk = (uint)(_winApi.VkKeyScan(token[0]) & 0xff);
                    }
                    break;
            }
        }
        return (modifiers, vk);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        // Signal the loop to exit
        _opSignal.Set();

        if (_messageLoopThread != null && _messageLoopThread.IsAlive)
        {
            _messageLoopThread.Join();
        }

        foreach (var kvp in _registrations.Keys)
        {
            _winApi.UnregisterHotKey(_hwnd, kvp);
        }
        _registrations.Clear();
        _keysByName.Clear();
    }

    private static class NativeMethods
    {
        public const uint PM_NOREMOVE = 0x0000;
        public const uint PM_REMOVE = 0x0001;

        [DllImport("user32.dll")]
        public static extern bool PeekMessage(out NativeTypes.MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
    }
}
