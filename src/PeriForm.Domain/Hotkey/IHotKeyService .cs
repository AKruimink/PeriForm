namespace PeriForm.Domain.Hotkey;

public interface IHotKeyService : IDisposable
{
    void RegisterHotKey(string id, string hotKeyDefinition);

    void UnregisterHotKey(string id);

    event EventHandler<HotKeyEventArgs> HotKeyPressed;
}
