namespace PeriForm.Domain.Automation;

/// <summary>
/// Represents the current status of an automation runner.
/// </summary>
public sealed class AutomationStatus
{
    public string Name { get; init; } = string.Empty;

    public bool IsRunning { get; init; }

    public int ExecutedCycles { get; init; }
}
