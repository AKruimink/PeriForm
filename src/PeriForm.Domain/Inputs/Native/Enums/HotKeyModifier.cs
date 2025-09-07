using System.ComponentModel.DataAnnotations;

namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Modifier key flags used when registering system-wide hotkeys.
/// Supports bitwise combination of values.
/// </summary>
[Flags]
internal enum HotKeyModifier : uint
{
    [Display(Name = "None")]
    None = 0x0000,

    [Display(Name = "Alt")]
    Alt = 0x0001,

    [Display(Name = "Control")]
    Control = 0x0002,

    [Display(Name = "Shift")]
    Shift = 0x0004,

    [Display(Name = "Windows Key")]
    Win = 0x0008
}
