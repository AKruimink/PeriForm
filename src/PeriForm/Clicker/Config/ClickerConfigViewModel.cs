using PeriForm.Domain.Clicker;
using PeriForm.Domain.Infrastructure.Enum;
using PeriForm.Infrastructure.ViewModel;

namespace PeriForm.Clicker.Config;

public class ClickerConfigViewModel : ViewModelBase, IClickerConfigViewModel
{
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private MouseButton _mouseButton;

    public MouseButton MouseButton
    {
        get => _mouseButton;
        set => SetProperty(ref _mouseButton, value);
    }

    private TimeSpan _clickInterval;

    public TimeSpan ClickInterval
    {
        get => _clickInterval;
        set => SetProperty(ref _clickInterval, value);
    }

    private TimeSpan _holdDuration;

    public TimeSpan HoldDuration
    {
        get => _holdDuration;
        set => SetProperty(ref _holdDuration, value);
    }

    private TimeSpan _pauseDuration;

    public TimeSpan PauseDuration
    {
        get => _pauseDuration;
        set => SetProperty(ref _pauseDuration, value);
    }

    private ConfigRunMode _runMode;

    public ConfigRunMode RunMode
    {
        get => _runMode;
        set => SetProperty(ref _runMode, value);
    }

    private int? _executionCount;

    public int? ExecutionCount
    {
        get => _executionCount;
        set => SetProperty(ref _executionCount, value);
    }

    private TimeSpan? _duration;

    public TimeSpan? Duration
    {
        get => _duration;
        set => SetProperty(ref _duration, value);
    }

    private string _startHotKey = string.Empty;

    public string StartHotKey
    {
        get => _startHotKey;
        set => SetProperty(ref _startHotKey, value);
    }

    private string _stopHotKey = string.Empty;

    public string StopHotKey
    {
        get => _stopHotKey;
        set => SetProperty(ref _stopHotKey, value);
    }
}
