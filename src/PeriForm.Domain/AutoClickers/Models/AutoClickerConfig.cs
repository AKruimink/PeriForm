using PeriForm.Domain.Inputs.Enums;
using PeriForm.Domain.Inputs.Models;

namespace PeriForm.Domain.AutoClickers.Models;

/// <summary>
/// Represents the configuration for an auto‑clicker.  Each configuration
/// defines how frequently clicks occur, which mouse button is used, where
/// clicks occur, and optional limits on the number of clicks or duration.
/// </summary>
public class AutoClickerConfig
{
    /// <summary>
    /// Unique identifier for this auto clicker configuration.
    /// Used to manage running instances and update/delete configurations without affecting others.
    /// </summary>
    public Guid AutoClickerID { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Human readable name for this clicker.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The mouse button to click (left, right, middle, XButton1, XButton2).
    /// </summary>
    public MouseButton Button { get; set; } = MouseButton.Left;

    /// <summary>
    /// The interval between clicks.
    /// </summary>
    public TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// The duration to hold the mouse button down for each click.
    /// </summary>
    public TimeSpan HoldDuration { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// If true, the cursor is moved to <see cref="PositionX"/> and <see cref="PositionY"/> before each click.
    /// If false, clicks occur wherever the cursor currently is.
    /// </summary>
    public bool UseFixedPosition { get; set; } = false;

    /// <summary>
    /// The x coordinate used when <see cref="UseFixedPosition"/> is true.
    /// </summary>
    public int? PositionX { get; set; }

    /// <summary>
    /// The y coordinate used when <see cref="UseFixedPosition"/> is true.
    /// </summary>
    public int? PositionY { get; set; }

    /// <summary>
    /// Optional maximum number of clicks to perform.
    /// If null, the clicker runs indefinitely or until stopped by the user.
    /// </summary>
    public int? MaxClicks { get; set; }

    /// <summary>
    /// Optional maximum duration to run.
    /// If null, the clicker runs indefinitely or until stopped.
    /// </summary>
    public TimeSpan? MaxDuration { get; set; }

    /// <summary>
    /// Hotkey used to start this clicker.
    /// </summary>
    public Hotkey StartHotkey { get; set; } = new Hotkey(KeyCode.F6);

    /// <summary>
    /// Hotkey used to stop this clicker.
    /// If this value is the same as <see cref="StartHotkey"/>, the hotkey acts as a toggle
    /// depending on whether the clicker is currently running.
    /// </summary>
    public Hotkey StopHotkey { get; set; } = new Hotkey(KeyCode.F7);
}
