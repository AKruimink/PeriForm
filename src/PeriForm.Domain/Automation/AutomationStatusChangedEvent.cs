using PeriForm.Domain.Infrastructure.Messenger;

namespace PeriForm.Domain.Automation;

/// <summary>
/// Publishes updates when an automation's status changes.
/// </summary>
public sealed class AutomationStatusChangedEvent : PubSubEvent<AutomationStatus>
{
}
