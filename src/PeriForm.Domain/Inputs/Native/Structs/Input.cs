using System.Runtime.InteropServices;
using PeriForm.Domain.Inputs.Native.Enums;

namespace PeriForm.Domain.Inputs.Native.Structs;

/// <summary>
/// Defines the structure used to send mouse, keyboard or hardware input via <see cref="NativeMethods.SendInput"/>.
/// Only one of the union fields is valid depending on <see cref="Type"/>.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct Input
{
    /// <summary>
    /// Specifies the type of input.
    /// </summary>
    public InputType Type;

    /// <summary>
    /// Contains the event data for the input.
    /// </summary>
    public InputUnion Data;

    /// <summary>
    /// Shortcut to the <see cref="MouseInput"/> view of the union.
    /// </summary>
    public MouseInput Mouse
    {
        get => Data.Mouse;
        set => Data.Mouse = value;
    }

    /// <summary>
    /// Shortcut to the <see cref="KeyboardInput"/> view of the union.
    /// </summary>
    public KeyboardInput Keyboard
    {
        get => Data.Keyboard;
        set => Data.Keyboard = value;
    }

    /// <summary>
    /// Shortcut to the <see cref="HardwareInput"/> view of the union.
    /// </summary>
    public HardwareInput Hardware
    {
        get => Data.Hardware;
        set => Data.Hardware = value;
    }
}
