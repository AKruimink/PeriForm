using System.ComponentModel.DataAnnotations;

namespace PeriForm.Domain.Infrastructure.Enum;

public enum ConfigRunMode
{
    /// <summary>
    /// The task continues execution until explicitly stopped.
    /// </summary>
    [Display(Name = "Indefinite")]
    Indefinite = 0,

    /// <summary>
    /// The task runs a predetermined number of times.
    /// </summary>
    [Display(Name = "Execution Count")]
    ExecutionCount = 1,

    /// <summary>
    /// The task runs for a specific duration.
    /// </summary>
    [Display(Name = "Duration")]
    Duration = 2
}
