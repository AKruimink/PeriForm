using System.Runtime.InteropServices;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Represents a point (x,y) coordinate.  Used for retrieving the current cursor position.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct Point
{
    public int x;

    public int y;
}
