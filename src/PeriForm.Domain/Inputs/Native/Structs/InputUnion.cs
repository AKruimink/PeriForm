using System.Runtime.InteropServices;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Represents the union portion of the <see cref="Input"/> structure.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct InputUnion
{
    [FieldOffset(0)]
    public MouseInput Mouse;

    [FieldOffset(0)]
    public KeyboardInput Keyboard;

    [FieldOffset(0)]
    public HardwareInput Hardware;
}
