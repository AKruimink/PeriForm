using PeriForm.Domain.AutoClickers.Models;

namespace PeriForm.Domain.AutoClickers.Interfaces;

/// <summary>
/// Contract for managing multiple auto‑clicker instances concurrently.
/// The service maintains a collection of configurations and ensures
/// each configuration can run independently without interfering with
/// others.
/// </summary>
public interface IAutoClickerService
{
    /// <summary>
    /// Adds a new auto‑clicker configuration or updates an existing one.
    /// If a configuration with the same <see cref="AutoClickerConfig.Identifier"/> exists, it will be stopped and replaced.
    /// If the configuration  defines start and stop hotkeys they are registered with the system.
    /// When start and stop hotkeys are identical the associated clicker toggles on and off with a single key.
    /// </summary>
    /// <param name="config">The configuration to add or update.</param>
    void AddOrUpdate(AutoClickerConfig config);

    /// <summary>
    /// Removes a configuration.
    /// If a clicker is currently running for this configuration, it will be stopped and its start and stop hotkeys unregistered.
    /// </summary>
    /// <param name="identifier">The identifier of the configuration to remove.</param>
    void Remove(Guid autoClickerID);
}
