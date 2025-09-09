using System.Runtime.InteropServices;
using PeriForm.Domain.Inputs.Native.Enums;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Describes a simulated mouse input event for <see cref="Input"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct MouseInput
{
    /// <summary>
    /// Specifies the absolute position or relative motion in the X direction.
    /// </summary>
    public int Dx;

    /// <summary>
    /// Specifies the absolute position or relative motion in the Y direction.
    /// </summary>
    public int Dy;

    /// <summary>
    /// Specifies additional data depending on the event type.
    /// For mouse wheel events this is the wheel delta, for X button events it identifies which button (1 or 2).
    /// </summary>
    public uint MouseData;

    /// <summary>
    /// Specifies event flags.
    /// Combination of <see cref="MouseEventFlags"/> values.
    /// </summary>
    public MouseEventFlags Flags;

    /// <summary>
    /// Specifies the timestamp for the event.
    /// If zero, the system will provide one.
    /// </summary>
    public uint Time;

    /// <summary>
    /// Additional information associated with the message.
    /// Typically zero.
    /// </summary>
    public IntPtr ExtraInfo;
}
