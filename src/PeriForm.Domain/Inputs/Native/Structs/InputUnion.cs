using System.Runtime.InteropServices;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Represents the union portion of the <see cref="INPUT"/> structure.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct InputUnion
{
    [FieldOffset(0)]
    public MouseInput mi;

    [FieldOffset(0)]
    public KeyboardInput ki;

    [FieldOffset(0)]
    public HardwareInput hi;
}
