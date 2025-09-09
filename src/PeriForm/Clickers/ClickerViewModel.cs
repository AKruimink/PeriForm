using System.Collections.ObjectModel;
using System.Windows.Input;
using PeriForm.Domain.Infrastructure.Enums;
using PeriForm.Domain.Inputs.Enums;
using PeriForm.Infrastructure.ViewModel;

namespace PeriForm.Clickers;

public class ClickerViewModel : ViewModelBase,  IClickerViewModel
{
    /// <inheritdoc />
    public ObservableCollection<ClickerItemViewModel> Clickers { get; } = new();

    /// <inheritdoc />
    public ICommand AddNewCommand { get; }

    /// <inheritdoc />
    public ICommand DeleteCommand { get; }

    public ClickerViewModel()
    {
        if (Clickers.Count == 0)
        {
            var sample1 = new ClickerItemViewModel
            {
                Name = "Rapid",
                Interval = TimeSpan.FromMilliseconds(100),
                Button = Domain.Inputs.Enums.MouseButton.Left,
                RunMode = RunMode.ExecutionCount,
                MaxClicks = 100
            };
            Clickers.Add(sample1);
            var sample2 = new ClickerItemViewModel
            {
                Name = "Timed",
                Interval = TimeSpan.FromSeconds(1),
                Button = Domain.Inputs.Enums.MouseButton.Right,
                RunMode = RunMode.Duration,
                MaxDuration = TimeSpan.FromMinutes(2)
            };
            Clickers.Add(sample2);
        }
    }
}
