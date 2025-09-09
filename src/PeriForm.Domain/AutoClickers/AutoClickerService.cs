using System.Collections.Concurrent;
using PeriForm.Domain.AutoClickers.Interfaces;
using PeriForm.Domain.AutoClickers.Models;
using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.Inputs.Events;
using PeriForm.Domain.Inputs.Interfaces;
using PeriForm.Domain.Inputs.Models;

namespace PeriForm.Domain.AutoClickers;

/// <summary>
/// Service responsible for managing and running multiple auto‑clicker configurations concurrently.
/// Each configuration can be started and stopped independently via its hotkey.
/// Configurations may be added or updated at runtime without affecting other running clickers.
/// When a configuration is removed, any running instance is stopped and the associated hotkey is unregistered.
/// </summary>
public class AutoClickerService : IAutoClickerService
{
    private readonly IMouseService _mouseService;
    private readonly IHotkeyService _hotkeyService;
    private readonly IEventAggregator _eventAggregator;

    private class AutoClickerState
    {
        public AutoClickerConfig Config { get; set; } = null!;

        public CancellationTokenSource? Cancellation { get; set; } = null;

        public bool IsRunning { get; set; } = false;
    }

    private readonly ConcurrentDictionary<Guid, AutoClickerState> _clickers = new();

    public AutoClickerService(IMouseService mouseService, IHotkeyService hotkeyService, IEventAggregator eventAggregator)
    {
        _mouseService = mouseService ?? throw new ArgumentNullException(nameof(mouseService));
        _hotkeyService = hotkeyService ?? throw new ArgumentNullException(nameof(hotkeyService));
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));

        _eventAggregator.GetEvent<HotkeyTriggeredEvent>().Subscribe(OnHotkeyTriggered, ThreadOption.BackgroundThread, false);
    }

    /// <inheritdoc />
    public void AddOrUpdate(AutoClickerConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        // Remove existing auto clicker if present
        if (_clickers.TryGetValue(config.AutoClickerID, out var existing))
        {
            // Stop existing auto clicker and unregister its hotkeys
            StopClicker(config.AutoClickerID);

            // Unregister the old hotkeys
            _hotkeyService.UnregisterHotkey(existing.Config.StartHotkey);
            _hotkeyService.UnregisterHotkey(existing.Config.StopHotkey);
        }

        // Register the new hotkeys
        _hotkeyService.RegisterHotkey(config.StartHotkey);
        _hotkeyService.RegisterHotkey(config.StopHotkey);

        // Add or replace the existing config
        _clickers[config.AutoClickerID] = new AutoClickerState
        {
            Config = config,
            Cancellation = null,
            IsRunning = false
        };
    }

    /// <inheritdoc />
    public void Remove(Guid autoClickerID)
    {
        if (_clickers.TryRemove(autoClickerID, out var existing))
        {
            // Stop the config it it's currently running
            if (existing.IsRunning)
            {
                StopClicker(autoClickerID);
            }

            // Unregister the old hotkeys
            _hotkeyService.UnregisterHotkey(existing.Config.StartHotkey);
            _hotkeyService.UnregisterHotkey(existing.Config.StopHotkey);
        }
    }

    private void OnHotkeyTriggered(Hotkey hotkey)
    {
        // Find all configs that use this hotkey
        foreach (var autoClicker in _clickers.Values)
        {
            var config = autoClicker.Config;
            var startMatch = config.StartHotkey != null && config.StartHotkey.Equals(hotkey);
            var stopMatch = config.StopHotkey != null && config.StopHotkey.Equals(hotkey);

            // Same hotkey for start and stop acts as a toggle
            if (startMatch && stopMatch)
            {
                if (!autoClicker.IsRunning)
                {
                    StartClicker(autoClicker);
                }
                else
                {
                    StopClicker(config.AutoClickerID);
                }
            }
            else if (startMatch)
            {
                if (!autoClicker.IsRunning)
                {
                    StartClicker(autoClicker);
                }
            }
            else if (stopMatch)
            {
                if (autoClicker.IsRunning)
                {
                    StopClicker(config.AutoClickerID);
                }
            }
        }
    }

    private void StartClicker(AutoClickerState autoClickerState)
    {
        if (autoClickerState.IsRunning)
        {
            return;
        }

        autoClickerState.IsRunning = true;
        autoClickerState.Cancellation = new CancellationTokenSource();

        var config = autoClickerState.Config;
        var cancellationToken = autoClickerState.Cancellation.Token;

        _ = Task.Run(async () =>
        {
            try
            {
                var remainingClicks = config.MaxClicks;
                var endTime = config.MaxDuration.HasValue ? DateTime.Now + config.MaxDuration : null;

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (remainingClicks.HasValue && remainingClicks.Value <= 0)
                    {
                        break;
                    }

                    if (endTime.HasValue && DateTime.Now >= endTime.Value)
                    {
                        break;
                    }

                    if (config.UseFixedPosition)
                    {
                        await _mouseService.MoveTo(config.PositionX ?? 0, config.PositionY ?? 0);
                    }

                    await _mouseService.Click(config.Button, config.HoldDuration);

                    if (remainingClicks.HasValue)
                    {
                        remainingClicks--;
                    }

                    if (config.Interval > TimeSpan.Zero)
                    {
                        try
                        {
                            await Task.Delay(config.Interval, cancellationToken);
                        }
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                // Auto stop when the loop exists
                StopClicker(config.AutoClickerID);
            }
        }, cancellationToken);
    }

    private void StopClicker(Guid autoClickerID)
    {
        if (_clickers.TryGetValue(autoClickerID, out var autoClickerState) && autoClickerState.IsRunning)
        {
            autoClickerState.Cancellation?.Cancel();
            autoClickerState.IsRunning = false;
            autoClickerState.Cancellation = null;
        }
    }
}
