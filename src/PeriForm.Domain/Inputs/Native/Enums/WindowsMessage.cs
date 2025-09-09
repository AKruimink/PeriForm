namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Defines common window messages used for keyboard and mouse input and hotkeys.
/// Values correspond to the WM_* constants defined in WinUser.h.
/// </summary>
internal enum WindowsMessage
{
    /// <summary>
    /// Keyboard key down.
    /// </summary>
    KeyDown = 0x0100,

    /// <summary>
    /// Keyboard key up.
    /// </summary>
    KeyUp = 0x0101,

    /// <summary>
    /// System key down (e.g. Alt key combos).
    /// </summary>
    SysKeyDown = 0x0104,

    /// <summary>
    /// System key up.
    /// </summary>
    SysKeyUp = 0x0105,

    /// <summary>
    /// Mouse moved.
    /// </summary>
    MouseMove = 0x0200,

    /// <summary>
    /// Left mouse button down.
    /// </summary>
    LeftButtonDown = 0x0201,

    /// <summary>
    /// Left mouse button up.
    /// </summary>
    LeftButtonUp = 0x0202,

    /// <summary>
    /// Right mouse button down.
    /// </summary>
    RightButtonDown = 0x0204,

    /// <summary>
    /// Right mouse button up.
    /// </summary>
    RightButtonUp = 0x0205,

    /// <summary>
    /// Middle mouse button down.
    /// </summary>
    MiddleButtonDown = 0x0207,

    /// <summary>
    /// Middle mouse button up.
    /// </summary>
    MiddleButtonUp = 0x0208,

    /// <summary>
    /// X button down.
    /// Use mouseData to determine which button (1 or 2).
    /// </summary>
    XButtonDown = 0x020B,

    /// <summary>
    /// X button up.
    /// </summary>
    XButtonUp = 0x020C,

    /// <summary>
    /// Mouse wheel.
    /// </summary>
    MouseWheel = 0x020A,

    /// <summary>
    /// Hotkey message.
    /// </summary>
    HotKey = 0x0312
}
