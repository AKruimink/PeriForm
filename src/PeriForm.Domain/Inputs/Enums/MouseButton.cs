using PeriForm.Domain.Inputs.Native.Enums;

namespace PeriForm.Domain.Inputs.Enums;

/// <summary>
/// Represents the buttons available on a standard mouse.
/// These values are used by the native input layer when translating to <see cref="MouseEventFlags"/>.
/// </summary>
public enum MouseButton
{
    /// <summary>
    /// The primary (left) mouse button.
    /// </summary>
    Left,

    /// <summary>
    /// The secondary (right) mouse button.
    /// </summary>
    Right,

    /// <summary>
    /// The middle mouse button (often used for scrolling).
    /// </summary>
    Middle,

    /// <summary>
    /// The first extra mouse button (commonly labelled as Mouse4 or Back on modern mice).
    /// </summary>
    XButton1,

    /// <summary>
    /// The second extra mouse button (commonly labelled as Mouse5 or Forward on modern mice).
    /// </summary>
    XButton2
}
