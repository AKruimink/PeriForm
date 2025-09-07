using PeriForm.Domain.Inputs.Native.Structs;

namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Flags for keyboard events used with <see cref="KeyboardInput.dwFlags"/>.
/// Values correspond to the KEYEVENTF_* constants in WinUser.h.
/// </summary>
[Flags]
internal enum KeyboardEventFlags : uint
{
    /// <summary>
    /// If specified, scan code was preceded by a prefix byte.
    /// </summary>
    ExtendedKey = 0x0001,

    /// <summary>
    /// Key up event.  If not specified, the event is key down.
    /// </summary>
    KeyUp = 0x0002,

    /// <summary>
    /// System key event (used for Alt keys).
    /// </summary>
    Alt = 0x0004,

    /// <summary>
    /// Unicode key.
    /// If specified, wScan identifies the Unicode character and wVk is ignored.
    /// </summary>
    Unicode = 0x0004,

    /// <summary>
    /// Use the scan code.
    /// If not set, wVk is used.
    /// </summary>
    Scancode = 0x0008
}
