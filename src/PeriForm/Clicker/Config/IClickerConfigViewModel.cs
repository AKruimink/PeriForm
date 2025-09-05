using PeriForm.Domain.Clicker;
using PeriForm.Domain.Infrastructure.Enum;

namespace PeriForm.Clicker.Config;

public interface IClickerConfigViewModel
{
    string Name { get; set; }

    MouseButton MouseButton { get; set; }

    TimeSpan ClickInterval { get; set; }

    TimeSpan HoldDuration { get; set; }

    TimeSpan PauseDuration { get; set; }

    ConfigRunMode RunMode { get; set; }

    int? ExecutionCount { get; set; }

    TimeSpan? Duration { get; set; }

    string StartHotKey { get; set; }

    string StopHotKey { get; set; }
}
