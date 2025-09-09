using PeriForm.Domain.Inputs.Enums;
using PeriForm.Domain.Inputs.Interfaces;

namespace PeriForm.Domain.Inputs.Models;

/// <summary>
/// Represents a hotkey consisting of a trigger key and optional modifier keys.
/// The modifiers correspond to the Control, Alt, Shift and Windows keys.
/// When registered through <see cref="IHotkeyService"/> the operating system will notify the application whenever the specified combination is pressed.
/// </summary>
/// <param name="Key">The primary key to trigger the hotkey.</param>
/// <param name="Ctrl">Specifies whether the Control modifier is part of the combination.</param>
/// <param name="Alt">Specifies whether the Alt modifier is part of the combination.</param>
/// <param name="Shift">Specifies whether the Shift modifier is part of the combination.</param>
/// <param name="Win">Specifies whether the Windows modifier is part of the combination.</param>
public record Hotkey(KeyCode Key, bool Ctrl = false, bool Alt = false, bool Shift = false, bool Win = false);
