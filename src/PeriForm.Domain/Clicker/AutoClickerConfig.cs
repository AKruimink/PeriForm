using PeriForm.Domain.Automation;
using PeriForm.Domain.Infrastructure.Enum;

namespace PeriForm.Domain.Clicker;

public class AutoClickerConfig : IAutomationConfig
{
    /// <summary>
    /// When <c>true</c>, this auto-clicker configuration is disabled.
    /// Start/stop hotkeys will have no effect while disabled.
    /// </summary>
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// User-defined unique name for this auto-clicker configuration.
    /// </summary>
    public string Name { get; set; } = "Default";

    /// <summary>
    /// Mouse button to press when the automation runs.
    /// </summary>
    public MouseButton Button { get; set; } = MouseButton.Left;

    /// <summary>
    /// Target time between the start of one click cycle and the start of the next.
    /// This is the fastest intended cadence. The effective interval may be longer
    /// if <see cref="HoldDuration"/> + <see cref="PauseDuration"/> exceeds this value.
    /// </summary>
    /// <remarks>
    /// Overrun rule: effective interval = <c>max(ClickInterval, HoldDuration + PauseDuration)</c>.
    /// For example, with a 1s interval, 2s hold, and 1s pause, the cycle takes 3s,
    /// so the effective cadence is 3s even though <c>ClickInterval</c> is 1s.
    /// </remarks>
    public TimeSpan ClickInterval { get; set; } = TimeSpan.FromMicroseconds(1);

    /// <summary>
    /// How long to hold the mouse button down during each click cycle.
    /// Use <c>TimeSpan.Zero</c> for a simple press-and-release.
    /// </summary>
    public TimeSpan? HoldDuration { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Additional delay after releasing the mouse button and before the next cycle begins.
    /// </summary>
    public TimeSpan? PauseDuration { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Controls how long the auto-clicker runs:
    /// <list type="bullet">
    /// <item><description><c>Indefinite</c>: runs until explicitly stopped.</description></item>
    /// <item><description>ExecutionCount: runs until <see cref="ExecutionCount"/> cycles complete.</description></item>
    /// <item><description>Duration: runs until <see cref="RunDuration"/> elapses.</description></item>
    /// </list>
    /// </summary>
    public ConfigRunMode RunMode { get; set; } = ConfigRunMode.Indefinite;

    /// <summary>
    /// Maximum number of click cycles to execute before stopping when
    /// <see cref="RunMode"/> is set to the ExecutionCount mode. Ignored otherwise.
    /// </summary>
    public int? ExecutionCount { get; set; }

    /// <summary>
    /// Maximum wall-clock time to run before stopping when
    /// <see cref="RunMode"/> is set to the Duration mode. Ignored otherwise.
    /// </summary>
    public TimeSpan? RunDuration { get; set; }

    /// <summary>
    /// Hotkey used to start the auto-clicker. If <see cref="StartHotKey"/> and
    /// <see cref="StopHotKey"/> are the same key, the hotkey acts as a toggle.
    /// </summary>
    public string StartHotKey { get; set; } = "F6";

    /// <summary>
    /// Hotkey used to stop the auto-clicker. If identical to <see cref="StartHotKey"/>,
    /// the hotkey acts as a toggle.
    /// </summary>
    public string StopHotKey { get; set; } = "F7";
}
