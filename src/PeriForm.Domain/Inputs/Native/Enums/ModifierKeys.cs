namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Flags for modifier keys used when registering a system hotkey.
/// These correspond to the MOD_* constants in WinUser.h.
/// </summary>
[Flags]
internal enum ModifierKeys : uint
{
    /// <summary>
    /// No modifier.
    /// </summary>
    None = 0x0000,

    /// <summary>
    /// Alt key.
    /// </summary>
    Alt = 0x0001,

    /// <summary>
    /// Control key.
    /// </summary>
    Control = 0x0002,

    /// <summary>
    /// Shift key.
    /// </summary>
    Shift = 0x0004,

    /// <summary>
    /// Windows (logo) key.
    /// </summary>
    Windows = 0x0008
}
