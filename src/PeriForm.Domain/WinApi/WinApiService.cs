using System.Runtime.InteropServices;
using PeriForm.Domain.WinApi.Types;

namespace PeriForm.Domain.WinApi;

/// <summary>
/// Concrete implementation of <see cref="IWinApiService"/> using P/Invoke.
/// </summary>
public sealed class WinApiService : IWinApiService
{
    // Constants for modifier keys
    public uint ModAlt => 0x0001;

    public uint ModControl => 0x0002;
    public uint ModShift => 0x0004;
    public uint ModWin => 0x0008;

    public bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk)
        => NativeMethods.RegisterHotKey(hWnd, id, fsModifiers, vk);

    public bool UnregisterHotKey(IntPtr hWnd, int id)
        => NativeMethods.UnregisterHotKey(hWnd, id);

    public sbyte VkKeyScan(char ch) => NativeMethods.VkKeyScan(ch);

    public bool GetMessage(out NativeTypes.MSG msg, IntPtr hWnd, uint filterMin, uint filterMax)
        => NativeMethods.GetMessage(out msg, hWnd, filterMin, filterMax);

    public bool TranslateMessage(ref NativeTypes.MSG msg)
        => NativeMethods.TranslateMessage(ref msg);

    public IntPtr DispatchMessage(ref NativeTypes.MSG msg)
        => NativeMethods.DispatchMessage(ref msg);

    public void PostQuitMessage(int exitCode)
        => NativeMethods.PostQuitMessage(exitCode);

    public IntPtr CreateMessageOnlyWindow(string className)
        => NativeMethods.CreateMessageOnlyWindow(className);

    // P/Invoke declarations
    private static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern sbyte VkKeyScan(char ch);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref NativeTypes.MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(ref NativeTypes.MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern bool GetMessage(out NativeTypes.MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);

        // Create a message‑only window as in the previous example
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateWindowEx(
            int dwExStyle, string lpClassName, string lpWindowName,
            int dwStyle, int X, int Y, int nWidth, int nHeight,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        // WNDCLASSEX structure used for registering a window class
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct WNDCLASSEX
        {
            public uint cbSize;
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string? lpszMenuName;
            public string? lpszClassName;
            public IntPtr hIconSm;
        }

        private static IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
            => DefWindowProc(hWnd, msg, wParam, lParam);

        private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static IntPtr CreateMessageOnlyWindow(string className)
        {
            var wndProcDelegate = new WndProc(WindowProc);
            var wndClass = new WNDCLASSEX
            {
                cbSize = (uint)Marshal.SizeOf<WNDCLASSEX>(),
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProcDelegate),
                lpszClassName = className,
            };
            RegisterClassEx(ref wndClass);

            const int WS_EX_NOACTIVATE = 0x08000000;
            const int WS_OVERLAPPED = 0x00000000;

            return CreateWindowEx(
                WS_EX_NOACTIVATE, className, string.Empty,
                WS_OVERLAPPED, 0, 0, 0, 0,
                IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
