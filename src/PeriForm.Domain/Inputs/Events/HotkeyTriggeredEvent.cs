using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.Inputs.Models;

namespace PeriForm.Domain.Inputs.Events;

/// <summary>
/// Event published when a registered system hotkey is pressed.
/// The payload is the <see cref="Hotkey"/> that was triggered.
/// </summary>
internal class HotkeyTriggeredEvent : PubSubEvent<Hotkey>
{
}
