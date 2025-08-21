using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PeriForm.Domain.Infrastructure.Messenger;
using PeriForm.Domain.Settings;
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
        // Events
        services.AddSingleton<IEventAggregator, EventAggregator>();

        // Settings
        services.AddTransient<ISettingStore, SettingStore>();
        services.AddSingleton<ISettingFactory, SettingFactory>();
        services.AddSingleton(sp => sp.GetRequiredService<ISettingFactory>().Create<ApplicationSettings>());
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
