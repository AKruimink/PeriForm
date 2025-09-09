namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Flags for keyboard events used with the native <c>KeyboardInput.Flags</c> field.
/// These values correspond to the KEYEVENTF_* constants defined in WinUser.h.
/// </summary>
[Flags]
internal enum KeyboardEventFlags : uint
{
    /// <summary>
    /// If specified, the scan code was preceded by a prefix byte (0xE0 or 0xE1).
    /// </summary>
    ExtendedKey = 0x0001,

    /// <summary>
    /// If specified, the key is being released.
    /// If not specified, the key is being pressed.
    /// </summary>
    KeyUp = 0x0002,

    /// <summary>
    /// Uses the scan code.
    /// If not set, the virtual key code is used.
    /// </summary>
    Scancode = 0x0008,

    /// <summary>
    /// Specifies a Unicode character, the scan code member identifies the Unicode character.
    /// When this flag is set, the virtual key code should be zero.
    /// </summary>
    Unicode = 0x0004
}
