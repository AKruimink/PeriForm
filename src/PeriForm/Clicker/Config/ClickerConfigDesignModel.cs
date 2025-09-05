using PeriForm.Domain.Clicker;
using PeriForm.Domain.Infrastructure.Enum;

namespace PeriForm.Clicker.Config;

public class ClickerConfigDesignModel : IClickerConfigViewModel
{
    public string Name { get; set; }
    public MouseButton MouseButton { get; set; }
    public TimeSpan ClickInterval { get; set; }
    public TimeSpan HoldDuration { get; set; }
    public TimeSpan PauseDuration { get; set; }
    public ConfigRunMode RunMode { get; set; }
    public int? ExecutionCount { get; set; }
    public TimeSpan? Duration { get; set; }
    public string StartHotKey { get; set; }
    public string StopHotKey { get; set; }

    public ClickerConfigDesignModel()
    {
        Name = "Test Config";
        MouseButton = MouseButton.XButton2;
        ClickInterval = TimeSpan.FromSeconds(1);
        HoldDuration = TimeSpan.Zero;
        PauseDuration = TimeSpan.Zero;
        RunMode = ConfigRunMode.ExecutionCount;
        ExecutionCount = 100;
        StartHotKey = "F6";
        StopHotKey = "F7";
    }
}
