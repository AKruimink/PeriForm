using PeriForm.Domain.Infrastructure.Enum;

namespace PeriForm.Domain.Automation;

/// <summary>
/// Common properties for all automation configurations.
/// </summary>
public interface IAutomationConfig
{
    bool Disabled { get; }

    string Name { get; }

    string StartHotKey { get; }

    string StopHotKey { get; }

    ConfigRunMode RunMode { get; }

    int? ExecutionCount { get; }

    TimeSpan? RunDuration { get; }
}
