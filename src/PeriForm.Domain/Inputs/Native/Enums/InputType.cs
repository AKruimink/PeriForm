namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Specifies the type of input in the <see cref="Input"/> structure.
/// </summary>
internal enum InputType : uint
{
    /// <summary>
    /// Specifies mouse input.
    /// </summary>
    Mouse = 0,

    /// <summary>
    /// Specifies keyboard input.
    /// </summary>
    Keyboard = 1,

    /// <summary>
    /// Specifies hardware input.
    /// </summary>
    Hardware = 2
}
