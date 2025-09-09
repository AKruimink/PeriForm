using System.Collections.Concurrent;
using System.Globalization;
using System.Windows.Interop;
using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.Inputs.Events;
using PeriForm.Domain.Inputs.Interfaces;
using PeriForm.Domain.Inputs.Models;
using PeriForm.Domain.Inputs.Native;
using PeriForm.Domain.Inputs.Native.Enums;

namespace PeriForm.Domain.Inputs.Services;

/// <summary>
/// Service that registers and manages system‑wide hotkeys and publishes them via the application's event aggregator.
/// Hotkeys are registered against a hidden message‑only window created via <see cref="HwndSource"/>.
/// When a hotkey is pressed, a <see cref="WindowsMessage.HotKey"/> message is received and translated into a <see cref="Models.Hotkey"/> instance which is published via a <see cref="HotkeyTriggeredEvent"/>.
/// </summary>
public class HotkeyService(IEventAggregator eventAggregator) : IHotkeyService
{
    private readonly IEventAggregator _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
    private readonly HotkeyTriggeredEvent _hotkeyEvent = eventAggregator.GetEvent<HotkeyTriggeredEvent>();

    // Track how many times a particular hotkey has been registered.
    // When reference counting a hotkey, the native registration is only created on the first registration and removed on the final unregistration.
    private readonly ConcurrentDictionary<Hotkey, HotkeyRegistration> _registrations = new();

    private readonly ConcurrentDictionary<int, Hotkey> _hotkeys = new();

    private HwndSource? _hotkeyWindow;
    private int _hotkeyIDCounter = 0;
    private bool _disposed;

    private class HotkeyRegistration()
    {
        public int HotkeyID { get; set; }

        public int Count { get; set; }
    }

    /// <inheritdoc />
    public bool RegisterHotkey(Hotkey hotkey)
    {
        ArgumentNullException.ThrowIfNull(hotkey);

        EnsureHotKeyWindowCreated();

        var registration = _registrations.GetOrAdd(hotkey, _ => new HotkeyRegistration());
        lock (registration)
        {
            // If the hotkey is already registered, we just increment the reference count and return
            if (registration.Count > 0)
            {
                registration.Count++;
                return true;
            }

            var hotkeyID = Interlocked.Increment(ref _hotkeyIDCounter);
            var modifiers = ModifierKeys.None;

            if (hotkey.Ctrl)
            {
                modifiers |= ModifierKeys.Control;
            }

            if (hotkey.Alt)
            {
                modifiers |= ModifierKeys.Alt;
            }

            if (hotkey.Shift)
            {
                modifiers |= ModifierKeys.Shift;
            }

            if (hotkey.Win)
            {
                modifiers |= ModifierKeys.Windows;
            }

            var success = NativeMethods.RegisterHotKey(_hotkeyWindow!.Handle, hotkeyID, modifiers, (ushort)hotkey.Key);
            if (success)
            {
                registration.HotkeyID = hotkeyID;
                registration.Count = 1;
                _hotkeys[hotkeyID] = hotkey;

                return true;
            }

            return false;
        }
    }

    /// <inheritdoc />
    public bool UnregisterHotkey(Hotkey hotkey)
    {
        ArgumentNullException.ThrowIfNull(hotkey);
        ArgumentNullException.ThrowIfNull(_hotkeyWindow);

        if (!_registrations.TryGetValue(hotkey, out var registration))
        {
            return false;
        }

        lock (registration)
        {
            if (registration.Count == 0)
            {
                return false;
            }

            registration.Count--;
            if (registration.Count > 0)
            {
                return true;
            }

            // This was the last registration, remove it from internal maps
            bool success = NativeMethods.UnregisterHotKey(_hotkeyWindow.Handle, registration.HotkeyID);
            if (success)
            {
                return false;
            }

            // Remove the from our own maps
            _hotkeys.TryRemove(registration.HotkeyID, out _);
            _registrations.TryRemove(hotkey, out _);

            return true;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (_hotkeyWindow != null)
        {
            foreach (var hotkey in _hotkeys)
            {
                NativeMethods.UnregisterHotKey(_hotkeyWindow.Handle, hotkey.Key);
            }

            _hotkeys.Clear();
            _hotkeyWindow.Dispose();
            _hotkeyWindow = null;
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Ensures a hidden window and hook are created that listens for hotkey triggers.
    /// Must be called before registering any hotkeys.
    /// </summary>
    private void EnsureHotKeyWindowCreated()
    {
        if (_hotkeyWindow != null)
        {
            return;
        }

        var paraemters = new HwndSourceParameters("PeriFormHotkeyWindow")
        {
            WindowStyle = unchecked((int)0x80000000), // WS_POPUP
            Width = 0,
            Height = 0,
            ParentWindow = IntPtr.Zero
        };

        _hotkeyWindow = new HwndSource(paraemters);
        _hotkeyWindow.AddHook(ProcessHotKeyMessage);
    }

    /// <summary>
    /// Processes window messages for the hidden hotkey window.
    /// When a <see cref="WindowsMessage.HotKey"/> message is received, the corresponding hotkey is looked up and published via the <see cref="HotkeyTriggeredEvent"/>.
    /// </summary>
    private IntPtr ProcessHotKeyMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        // Check if the message is a hotkey message
        if ((WindowsMessage)msg == WindowsMessage.HotKey)
        {
            var hotkeyID = wParam.ToInt32();
            if (_hotkeys.TryGetValue(hotkeyID, out var hotkey))
            {
                // Publish via the event aggregator
                _hotkeyEvent.Publish(hotkey);
                handled = true;
            }
        }

        return IntPtr.Zero;
    }
}
