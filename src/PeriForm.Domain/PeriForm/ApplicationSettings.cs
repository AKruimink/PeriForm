namespace PeriForm.Domain.PeriForm;

/// <summary>
/// Defines a class that acts as model for the application settings
/// </summary>
public class ApplicationSettings
{
    /// <summary>
    /// Gets or Sets an idicator that defines if dark theme should be used
    /// <remarks>Defaults to <see langword="false"/></remarks>
    /// </summary>
    public bool UseDarkTheme { get; set; } = true;

    /// <summary>
    /// Gets or Sets an idicator that defines if the window should be minimized when closed
    /// <remarks>Defaults to <see langword="false"/></remarks>
    /// </summary>
    public bool MinimizeOnClose { get; set; } = false;
}
