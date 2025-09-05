namespace PeriForm.Domain.Clicker;

/// <summary>
/// Represents the top-level settings object that is saved and loaded to persist auto-clicker configurations.
/// </summary>
public class AutoClickerSettings
{
    /// <summary>
    /// The collection of auto-clicker configurations to persist.
    /// </summary>
    public List<AutoClickerConfig> AutoClickers { get; set; } = [];
}
