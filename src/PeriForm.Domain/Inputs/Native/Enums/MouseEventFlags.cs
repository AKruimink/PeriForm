using PeriForm.Domain.Inputs.Native.Structs;

namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Flags for mouse events used with <see cref="MouseInput.dwFlags"/>.
/// Values correspond to the MOUSEEVENTF_* constants in WinUser.h.
/// </summary>
[Flags]
internal enum MouseEventFlags : uint
{
    /// <summary>
    /// Movement occurred.
    /// Combine with absolute flags to specify absolute coordinates.
    /// </summary>
    Move = 0x0001,

    /// <summary>
    /// Left button down.
    /// </summary>
    LeftDown = 0x0002,

    /// <summary>
    /// Left button up.
    /// </summary>
    LeftUp = 0x0004,

    /// <summary>
    /// Right button down.
    /// </summary>
    RightDown = 0x0008,

    /// <summary>
    /// Right button up.
    /// </summary>
    RightUp = 0x0010,

    /// <summary>
    /// Middle button down.
    /// </summary>
    MiddleDown = 0x0020,

    /// <summary>
    /// Middle button up.
    /// </summary>
    MiddleUp = 0x0040,

    /// <summary>
    /// X button down.
    /// When used, set <see cref="MouseInput.mouseData"/> to 1 for XButton1 or 2 for XButton2.
    /// </summary>
    XDown = 0x0080,

    /// <summary>
    /// X button up.
    /// When used, set <see cref="MouseInput.mouseData"/> to 1 for XButton1 or 2 for XButton2.
    /// </summary>
    XUp = 0x0100,

    /// <summary>
    /// The wheel was moved.
    /// The amount of movement is specified in <see cref="MouseInput.mouseData"/>.
    /// </summary>
    Wheel = 0x0800,

    /// <summary>
    /// Specifies that the dx and dy members contain normalized absolute coordinates.
    /// </summary>
    Absolute = 0x8000,

    /// <summary>
    /// Specifies that movement is relative to the virtual desktop (entire desktop).
    /// </summary>
    VirtualDesk = 0x4000
}
