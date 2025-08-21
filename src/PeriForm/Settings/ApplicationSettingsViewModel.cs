using PeriForm.Domain.Settings;
using PeriForm.Infrastructure.ViewModel;

namespace PeriForm.Settings;

/// <summary>
/// Defines a class that provides general application settings data
/// </summary>
public class ApplicationSettingsViewModel : ViewModelBase, IApplicationSettingsViewModel
{
    ///<inheritdoc/>
    public bool UseDarkTheme
    {
        get => _useDarkTheme;
        set
        {
            SetProperty(ref _useDarkTheme, value);
            _settings.CurrentSetting.UseDarkTheme = value;
            SaveSettings();
        }
    }

    private bool _useDarkTheme = true;

    ///<inheritdoc/>
    public bool MinimizeOnClose
    {
        get => _minimizeOnClose;
        set
        {
            SetProperty(ref _minimizeOnClose, value);
            _settings.CurrentSetting.MinimizeOnClose = value;
            SaveSettings();
        }
    }

    private bool _minimizeOnClose;

    /// <summary>
    /// <see cref="ISetting{TSetting}"/> containing the <see cref="ApplicationSettings"/>
    /// </summary>
    private readonly ISetting<ApplicationSettings> _settings;

    /// <summary>
    /// Create a new instance of the <see cref="ApplicationSettingsViewModel"/>
    /// <param name="applicationSettings"><see cref="ISetting{ApplicationSettings}"/> of the current app settings</param>
    /// </summary>
    public ApplicationSettingsViewModel(ISetting<ApplicationSettings> applicationSettings)
    {
        _settings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));

        // Setup view
        UseDarkTheme = _settings.CurrentSetting.UseDarkTheme;
        MinimizeOnClose = _settings.CurrentSetting.MinimizeOnClose;
    }

    /// <summary>
    /// Save the settings
    /// </summary>
    private void SaveSettings()
    {
        _settings.Save();
    }
}
