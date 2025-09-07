using System.ComponentModel.DataAnnotations;

namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Identifiers for Windows keyboard message types (see WinUser.h).
/// </summary>
internal enum KeyboardMessage : uint
{
    [Display(Name = "Key Down")]
    WM_KEYDOWN = 0x0100,

    [Display(Name = "Key Up")]
    WM_KEYUP = 0x0101,

    [Display(Name = "System Key Down")]
    WM_SYSKEYDOWN = 0x0104,

    [Display(Name = "System Key Up")]
    WM_SYSKEYUP = 0x0105
}
