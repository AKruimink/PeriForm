using System.ComponentModel.DataAnnotations;

namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Identifiers for Windows mouse message types (see WinUser.h).
/// </summary>
internal enum MouseMessage : uint
{
    [Display(Name = "Mouse Move")]
    WM_MOUSEMOVE = 0x0200,

    [Display(Name = "Left Button Down")]
    WM_LBUTTONDOWN = 0x0201,

    [Display(Name = "Left Button Up")]
    WM_LBUTTONUP = 0x0202,

    [Display(Name = "Right Button Down")]
    WM_RBUTTONDOWN = 0x0204,

    [Display(Name = "Right Button Up")]
    WM_RBUTTONUP = 0x0205,

    [Display(Name = "Middle Button Down")]
    WM_MBUTTONDOWN = 0x0207,

    [Display(Name = "Middle Button Up")]
    WM_MBUTTONUP = 0x0208,

    [Display(Name = "Mouse Wheel")]
    WM_MOUSEWHEEL = 0x020A,

    [Display(Name = "X Button Down")]
    WM_XBUTTONDOWN = 0x020B,

    [Display(Name = "X Button Up")]
    WM_XBUTTONUP = 0x020C
}
