using System.Runtime.InteropServices;
using PeriForm.Domain.Inputs.Native.Enums;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Describes a simulated keyboard input event for <see cref="Input"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct KeyboardInput
{
    /// <summary>
    /// Specifies the virtual key code.
    /// Set to zero when using the scan code and setting the <see cref="KeyboardEventFlags.Unicode"/> flag.
    /// </summary>
    public ushort Vk;

    /// <summary>
    /// Specifies a hardware scan code for the key.
    /// </summary>
    public ushort Scan;

    /// <summary>
    /// Specifies event flags.
    /// Combination of <see cref="KeyboardEventFlags"/> values.
    /// </summary>
    public KeyboardEventFlags Flags;

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
