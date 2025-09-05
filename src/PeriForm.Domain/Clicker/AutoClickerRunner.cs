using PeriForm.Domain.Automation;
using PeriForm.Domain.Infrastructure.Enum;
using PeriForm.Domain.Infrastructure.Messenger;

namespace PeriForm.Domain.Clicker;

/// <summary>
/// Runs an auto‑clicker loop according to its configuration.
/// </summary>
public sealed class AutoClickerRunner : IAutomationRunner
{
    private readonly AutoClickerConfig _config;
    private readonly IEventAggregator _eventAggregator;
    private CancellationTokenSource? _cts;
    private int _executedCycles;

    public AutoClickerRunner(AutoClickerConfig config, IEventAggregator eventAggregator)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
    }

    public string Name => _config.Name;
    public bool IsRunning => _cts != null;

    public void Start()
    {
        if (IsRunning || _config.Disabled) return;
        _cts = new CancellationTokenSource();
        _ = Task.Run(() => RunLoopAsync(_cts.Token));
    }

    public void Stop()
    {
        _cts?.Cancel();
        _cts = null;
        PublishStatus();
    }

    public void Toggle()
    {
        if (IsRunning)
        {
            Stop();
        }
        else
        {
            Start();
        }
    }

    private async Task RunLoopAsync(CancellationToken token)
    {
        var startTime = DateTime.UtcNow;
        _executedCycles = 0;
        PublishStatus();

        while (!token.IsCancellationRequested)
        {
            if (_config.RunMode == ConfigRunMode.ExecutionCount &&
                _config.ExecutionCount.HasValue &&
                _executedCycles >= _config.ExecutionCount.Value)
                break;

            if (_config.RunMode == ConfigRunMode.Duration &&
                _config.RunDuration.HasValue &&
                (DateTime.UtcNow - startTime) >= _config.RunDuration.Value)
                break;

            MouseDown(_config.Button);
            await Task.Delay(_config.HoldDuration ?? TimeSpan.Zero, token);
            MouseUp(_config.Button);
            await Task.Delay(_config.PauseDuration ?? TimeSpan.Zero, token);

            // Overrun rule: interval must cover hold + pause
            var effectiveInterval = Math.Max(
                _config.ClickInterval.Ticks,
                (_config.HoldDuration ?? TimeSpan.Zero).Ticks +
                (_config.PauseDuration ?? TimeSpan.Zero).Ticks);
            await Task.Delay(TimeSpan.FromTicks(effectiveInterval), token);

            _executedCycles++;
            PublishStatus();
        }

        _cts = null;
        PublishStatus();
    }

    private void PublishStatus()
    {
        var evt = _eventAggregator.GetEvent<AutomationStatusChangedEvent>();
        evt.Publish(new AutomationStatus
        {
            Name = _config.Name,
            IsRunning = IsRunning,
            ExecutedCycles = _executedCycles
        });
    }

    private static void MouseDown(MouseButton button)
    {
        /* call SendInput */
    }

    private static void MouseUp(MouseButton button)
    {
        /* call SendInput */
    }
}
