using PeriForm.Domain.Inputs.Native.Structs;

namespace PeriForm.Domain.Inputs.Native.Enums;

/// <summary>
/// Specifies the type of input in the <see cref="Input"/> structure.
/// The values correspond to the values passed to the native SendInput API.
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
