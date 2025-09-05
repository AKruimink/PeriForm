using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using ControlzEx.Theming;
using PeriForm.Domain.Automation;
using PeriForm.Domain.Clicker;
using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.Infrastructure.Messenger.Events;
using PeriForm.Domain.PeriForm;
using PeriForm.Domain.Settings;
using PeriForm.Infrastructure.Command;
using PeriForm.Infrastructure.ViewModel;

namespace PeriForm.Window;

/// <summary>
/// Defines a class that provides and handles application information
/// </summary>
public class WindowViewModel : ViewModelBase, IWindowViewModel
{
    public string Version { get; }

    public bool MenuIsOpen
    {
        get => _menuIsOpen;
        set => SetProperty(ref _menuIsOpen, value);
    }

    private bool _menuIsOpen;
    public bool MinimizeOnClose { get; set; }

    // Status properties for the demo clicker
    private bool _clickerIsRunning;

    public bool ClickerIsRunning
    {
        get => _clickerIsRunning;
        set => SetProperty(ref _clickerIsRunning, value);
    }

    private int _clickerExecutedCycles;

    public int ClickerExecutedCycles
    {
        get => _clickerExecutedCycles;
        set => SetProperty(ref _clickerExecutedCycles, value);
    }

    private readonly IEventAggregator _eventAggregator;

    public ICommand ShowSourceOnGithubCommand { get; }
    public ICommand ShowVersionsOnGithubCommand { get; }

    public WindowViewModel(
        IEventAggregator eventAggregator,
        ISetting<ApplicationSettings> applicationSettings,
        ISetting<AutoClickerSettings> clickerSettings,
        AutomationManager automationManager)
    {
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

        // Subscribe to application settings changes to update theme etc.
        _eventAggregator.GetEvent<SettingChangedEvent<ApplicationSettings>>()
                        .Subscribe(ApplicationSettingsChanged, ThreadOption.UIThread, false);
        if (applicationSettings == null)
            throw new ArgumentNullException(nameof(applicationSettings));
        ApplicationSettingsChanged(applicationSettings);

        // Ensure there is at least one clicker config for demonstration
        if (!clickerSettings.CurrentSetting.AutoClickers.Any())
        {
            clickerSettings.CurrentSetting.AutoClickers.Add(new AutoClickerConfig
            {
                Name = "DemoClicker",
                StartHotKey = "F6",
                StopHotKey = "F7",
                ClickInterval = TimeSpan.FromSeconds(1)
            });
            clickerSettings.Save();
        }

        // Subscribe to automation status updates
        _eventAggregator.GetEvent<AutomationStatusChangedEvent>()
                        .Subscribe(status =>
                        {
                            if (status.Name == "DemoClicker")
                            {
                                ClickerIsRunning = status.IsRunning;
                                ClickerExecutedCycles = status.ExecutedCycles;
                            }
                        }, ThreadOption.UIThread);

        // Setup commands
        Version = $"v:{Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString(3)}";
        ShowSourceOnGithubCommand = new DelegateCommand(() =>
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/AKruimink/PeriForm",
                UseShellExecute = true
            };
            Process.Start(psi);
        });
        ShowVersionsOnGithubCommand = new DelegateCommand(() =>
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/AKruimink/PeriForm/releases",
                UseShellExecute = true
            };
            Process.Start(psi);
        });
    }

    private void ApplicationSettingsChanged(ISetting<ApplicationSettings> settings)
    {
        if (settings.CurrentSetting != null)
        {
            UpdateTheme(settings.CurrentSetting.UseDarkTheme);
            MinimizeOnClose = settings.CurrentSetting.MinimizeOnClose;
        }
    }

    private void UpdateTheme(bool useDarkTheme)
    {
        var themeName = useDarkTheme ? ThemeManager.BaseColorDark : ThemeManager.BaseColorLight;
        if (ThemeManager.Current.DetectTheme()?.BaseColorScheme != themeName && System.Windows.Application.Current != null)
        {
            ThemeManager.Current.ChangeThemeBaseColor(System.Windows.Application.Current, themeName);
        }
    }
}
