using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeriForm.Domain.Clicker;
using PeriForm.Domain.Hotkey;
using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.Infrastructure.Messenger.Events;
using PeriForm.Domain.Settings;

namespace PeriForm.Domain.Automation;

public sealed class AutomationManager
{
    private readonly IHotKeyService _hotKeyService;
    private readonly IEventAggregator _eventAggregator;

    private readonly ISetting<AutoClickerSettings> _clickerSetting;
    //private readonly ISetting<AutoMoverSettings> _moverSetting;
    //private readonly ISetting<AutoTyperSettings> _typerSetting;

    private readonly ConcurrentDictionary<string, IAutomationRunner> _runners = new();

    public AutomationManager(
        ISettingFactory settingFactory,
        IHotKeyService hotKeyService,
        IEventAggregator eventAggregator)
    {
        _hotKeyService = hotKeyService ?? throw new ArgumentNullException(nameof(hotKeyService));
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

        _clickerSetting = settingFactory.Create<AutoClickerSettings>();
        //_moverSetting = settingFactory.Create<AutoMoverSettings>();
        //_typerSetting = settingFactory.Create<AutoTyperSettings>();

        // Subscribe to setting changes
        _eventAggregator.GetEvent<SettingChangedEvent<AutoClickerSettings>>()
            .Subscribe(OnClickerSettingsChanged);

        //_eventAggregator.GetEvent<SettingChangedEvent<AutoMoverSettings>>()
        //    .Subscribe(OnMoverSettingsChanged);

        //_eventAggregator.GetEvent<SettingChangedEvent<AutoTyperSettings>>()
        //    .Subscribe(OnTyperSettingsChanged);

        _hotKeyService.HotKeyPressed += OnHotKeyPressed;

        // Build initial runners
        LoadInitialRunners();
    }

    private void LoadInitialRunners()
    {
        foreach (var cfg in _clickerSetting.CurrentSetting.AutoClickers)
            AddOrUpdateRunner(cfg);

        //foreach (var cfg in _moverSetting.CurrentSetting.Configs)
        //    AddOrUpdateRunner(cfg);

        //foreach (var cfg in _typerSetting.CurrentSetting.Configs)
        //    AddOrUpdateRunner(cfg);
    }

    // Event handlers now receive Setting<T> instead of T
    private void OnClickerSettingsChanged(ISetting<AutoClickerSettings> changedSetting)
    {
        SyncRunners(changedSetting.CurrentSetting.AutoClickers, typeof(AutoClickerConfig));
    }

    //private void OnMoverSettingsChanged(Setting<AutoMoverSettings> changedSetting)
    //{
    //    SyncRunners(changedSetting.CurrentSetting.Configs, typeof(AutoMoverConfig));
    //}

    //private void OnTyperSettingsChanged(Setting<AutoTyperSettings> changedSetting)
    //{
    //    SyncRunners(changedSetting.CurrentSetting.Configs, typeof(AutoTyperConfig));
    //}

    /// <summary>
    /// Aligns the current runners with the given set of configs for one specific type.
    /// Runners of other types are left untouched.
    /// </summary>
    private void SyncRunners(IEnumerable<IAutomationConfig> configs, Type configType)
    {
        var newNames = new HashSet<string>(configs.Select(c => c.Name), StringComparer.OrdinalIgnoreCase);

        // Remove runners whose config type matches and whose name is no longer present
        foreach (var existing in _runners.Keys.ToList())
        {
            var runner = _runners[existing];
            var cfg = GetAutomationConfig(existing);
            if (cfg != null && cfg.GetType() == configType && !newNames.Contains(existing))
            {
                RemoveRunner(existing);
            }
        }

        // Add or update each config in the new set
        foreach (var cfg in configs)
        {
            AddOrUpdateRunner(cfg);
        }
    }

    private void OnHotKeyPressed(object? sender, HotKeyEventArgs e)
    {
        var parts = e.ID.Split('/');
        if (parts.Length != 2) return;
        var name = parts[0];
        var action = parts[1];

        if (!_runners.TryGetValue(name, out var runner)) return;

        var cfg = GetAutomationConfig(name);
        if (cfg != null && string.Equals(cfg.StartHotKey, cfg.StopHotKey, StringComparison.OrdinalIgnoreCase))
        {
            runner.Toggle();
        }
        else if (action == "start" && !runner.IsRunning)
        {
            runner.Start();
        }
        else if (action == "stop" && runner.IsRunning)
        {
            runner.Stop();
        }
    }

    private IAutomationConfig? GetAutomationConfig(string name)
    {
        return _clickerSetting.CurrentSetting.AutoClickers
                              .Cast<IAutomationConfig>()
                              //.Concat(_moverSetting.CurrentSetting.Configs)
                              //.Concat(_typerSetting.CurrentSetting.Configs)
                              .FirstOrDefault(c => c.Name == name);
    }

    private void AddOrUpdateRunner(IAutomationConfig config)
    {
        RemoveRunner(config.Name);

        IAutomationRunner runner = config switch
        {
            AutoClickerConfig c => new AutoClickerRunner(c, _eventAggregator),
            //AutoMoverConfig m => new AutoMoverRunner(m, _eventAggregator),
            //AutoTyperConfig t => new AutoTyperRunner(t, _eventAggregator),
            _ => throw new InvalidOperationException($"Unsupported config type {config.GetType()}")
        };

        _runners[config.Name] = runner;

        _hotKeyService.RegisterHotKey($"{config.Name}/start", config.StartHotKey);
        if (!string.Equals(config.StartHotKey, config.StopHotKey, StringComparison.OrdinalIgnoreCase))
        {
            _hotKeyService.RegisterHotKey($"{config.Name}/stop", config.StopHotKey);
        }
    }

    private void RemoveRunner(string name)
    {
        if (_runners.TryRemove(name, out var runner))
        {
            runner.Stop();
            _hotKeyService.UnregisterHotKey($"{name}/start");
            _hotKeyService.UnregisterHotKey($"{name}/stop");
        }
    }
}
