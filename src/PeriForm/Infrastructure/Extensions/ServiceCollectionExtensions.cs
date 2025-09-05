using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PeriForm.Domain.Automation;
using PeriForm.Domain.Clicker;
using PeriForm.Domain.Hotkey;
using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.PeriForm;
using PeriForm.Domain.Settings;
using PeriForm.Domain.WinApi;
using PeriForm.Infrastructure.ViewModel;

namespace PeriForm.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering PeriForm services with Microsoft.Extensions.DependencyInjection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all PeriForm services (domain + GUI) into the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    public static IServiceCollection AddPeriFormServices(this IServiceCollection services)
    {
        RegisterDomainDependencies(services);
        RegisterGuiDependencies(services);

        return services;
    }

    /// <summary>
    /// Registers domain-layer services and settings.
    /// </summary>
    private static void RegisterDomainDependencies(IServiceCollection services)
    {
        // Event aggregator
        services.AddSingleton<IEventAggregator, EventAggregator>();

        // WinAPI abstraction and hot‑key service
        services.AddSingleton<IWinApiService, WinApiService>();
        services.AddSingleton<IHotKeyService, HotKeyService>();

        // Settings infrastructure
        services.AddTransient<ISettingStore, SettingStore>();
        services.AddSingleton<ISettingFactory, SettingFactory>();

        // Application-wide settings
        services.AddSingleton(sp => sp.GetRequiredService<ISettingFactory>().Create<ApplicationSettings>());
        // Automation settings for each type (clicker, mover, typer)
        services.AddSingleton(sp => sp.GetRequiredService<ISettingFactory>().Create<AutoClickerSettings>());
        //services.AddSingleton(sp => sp.GetRequiredService<ISettingFactory>().Create<AutoMoverSettings>());
        //services.AddSingleton(sp => sp.GetRequiredService<ISettingFactory>().Create<AutoTyperSettings>());

        // Automation manager
        services.AddSingleton<AutomationManager>();
    }

    /// <summary>
    /// Registers GUI-layer services (e.g., ViewModels).
    /// </summary>
    private static void RegisterGuiDependencies(IServiceCollection services)
    {
        var viewModelTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ViewModelBase).IsAssignableFrom(t));

        foreach (var t in viewModelTypes)
        {
            services.AddTransient(t);
        }
    }
}
