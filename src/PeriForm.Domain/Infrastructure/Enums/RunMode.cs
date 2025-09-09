using System.ComponentModel;

namespace PeriForm.Domain.Infrastructure.Enums;

/// <summary>
/// Defines how a config runs: indefinitely, for a fixed number of time or for a fixed duration.
/// </summary>
public enum RunMode
{
    /// <summary>
    /// Runs forever until manually stopped.
    /// </summary>
    [Description("Indefinite")]
    Indefinite,

    /// <summary>
    /// Runs for a fixed number of cycles.
    /// </summary>
    [Description("Execution Count")]
    ExecutionCount,

    /// <summary>
    /// Runs for a fixed time period.
    /// </summary>
    [Description("Duration")]
    Duration
}
