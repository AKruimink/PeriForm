using PeriForm.Domain.Settings;

namespace PeriForm.Domain.Infrastructure.Messenger.Events;

/// <summary>
/// Defines a class that acts as a contract for when a settings have changed
/// </summary>
public class SettingChangedEvent<TSetting> : PubSubEvent<ISetting<TSetting>>
{
}
