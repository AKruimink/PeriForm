namespace PeriForm.Domain.Automation;

/// <summary>
/// Starts and stops an automation.
/// </summary>
public interface IAutomationRunner
{
    string Name { get; }

    bool IsRunning { get; }

    void Start();

    void Stop();

    void Toggle();
}
