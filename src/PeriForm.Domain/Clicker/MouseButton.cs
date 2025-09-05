using System.ComponentModel.DataAnnotations;

namespace PeriForm.Domain.Clicker;

public enum MouseButton
{
    [Display(Name = "Left Mouse")]
    Left = 0,

    [Display(Name = "Right Mouse")]
    Right = 1,

    [Display(Name = "Middle Mouse")]
    Middle = 2,

    XButton1 = 3,

    XButton2 = 4,

    XButton3 = 5,

    XButton4 = 6,

    XButton5 = 7,

    XButton6 = 8,

    XButton7 = 9,

    XButton8 = 10,

    XButton9 = 11,

    XButton10 = 12,

    XButton11 = 13,

    XButton12 = 14
}
