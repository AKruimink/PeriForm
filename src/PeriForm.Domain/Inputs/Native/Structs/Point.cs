using System.Runtime.InteropServices;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Represents a point (x,y) coordinate in screen space.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct Point(int x, int y)
{
    /// <summary>
    /// The x‑coordinate.
    /// </summary>
    public int X = x;

    /// <summary>
    /// The y‑coordinate.
    /// </summary>
    public int Y = y;
}
