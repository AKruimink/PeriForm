using System.ComponentModel.DataAnnotations;

namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Contains constants representing low-level Windows hook types (see WinUser.h).
/// </summary>
internal enum HookType : int
{
    [Display(Name = "Low-Level Keyboard Hook")]
    WH_KEYBOARD_LL = 13,

    [Display(Name = "Low-Level Mouse Hook")]
    WH_MOUSE_LL = 14
}
