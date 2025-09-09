using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.Inputs.Models;

namespace PeriForm.Domain.Inputs.Interfaces;

/// <summary>
/// Manages system‑wide hotkeys.
/// Hotkeys registered with this service will generate events whenever the specified key combination is pressed, even
/// when the application is not in the foreground.
/// Each hotkey is identified internally by a unique integer id.
/// Hotkey events are published via the application's <see cref="IEventAggregator"/>.
/// </summary>
public interface IHotkeyService : IDisposable
{
    /// <summary>
    /// Registers a new system hotkey.
    /// If a conflicting hotkey already exists, the call will fail and return <c>false</c>.
    /// </summary>
    /// <param name="hotkey">The hotkey to register.</param>
    /// <returns><c>true</c> if the hotkey was registered; otherwise <c>false</c>.</returns>
    bool RegisterHotkey(Hotkey hotkey);

    /// <summary>
    /// Unregisters a previously registered system hotkey.
    /// </summary>
    /// <param name="hotkey">The hotkey to unregister.</param>
    /// <returns><c>true</c> if the hotkey was unregistered; otherwise <c>false</c>.</returns>
    bool UnregisterHotkey(Hotkey hotkey);
}
