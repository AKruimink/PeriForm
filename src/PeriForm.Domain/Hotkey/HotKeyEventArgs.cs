namespace PeriForm.Domain.Hotkey;

public class HotKeyEventArgs(string id, bool isStartKey) : EventArgs
{
    public string ID { get; } = id;

    public bool IsStartKey { get; } = isStartKey;
}
