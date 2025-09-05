using System.Runtime.InteropServices;

namespace PeriForm.Domain.WinApi.Types;

/// <summary>
/// Structures used by WinApi message functions.
/// </summary>
public static class NativeTypes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;

        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public System.IntPtr hwnd;

        public uint message;

        public System.IntPtr wParam;

        public System.IntPtr lParam;

        public uint time;

        public POINT pt;
    }
}
