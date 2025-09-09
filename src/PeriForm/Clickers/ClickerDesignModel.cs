using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PeriForm.Clickers;

public class ClickerDesignModel : IClickerViewModel
{
    /// <inheritdoc />
    public ObservableCollection<ClickerItemViewModel> Clickers { get; } = new();

    /// <inheritdoc />
    public ICommand AddNewCommand { get; }

    /// <inheritdoc />
    public ICommand DeleteCommand { get; }
}
