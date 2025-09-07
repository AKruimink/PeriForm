using System.Runtime.InteropServices;
using PeriForm.Domain.Inputs.Native.Enums;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Defines the <c>INPUT</c> structure used by the <see cref="NativeMethods.SendInput"/> function.
/// This is a tagged union that can represent either a mouse, keyboard or hardware input event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct Input
{
    /// <summary>
    /// Specifies the type of input.
    /// </summary>
    public InputType type;

    /// <summary>
    /// Holds the event data for this input.  Only one of the fields will be valid
    /// depending on the value of <see cref="type"/>.
    /// </summary>
    public InputUnion U;

    /// <summary>
    /// Helper property to access the <see cref="MouseInput"/> view of the union.
    /// </summary>
    public MouseInput mi => U.mi;

    /// <summary>
    /// Helper property to access the <see cref="KeyboardInput"/> view of the union.
    /// </summary>
    public KeyboardInput ki => U.ki;

    /// <summary>
    /// Helper property to access the <see cref="HardwareInput"/> view of the union.
    /// </summary>
    public HardwareInput hi => U.hi;
}
