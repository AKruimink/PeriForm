using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PeriForm.Infrastructure.Extensions;
using PeriForm.Infrastructure.ViewModel;
using PeriForm.Window;

namespace PeriForm;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    /// <summary>
    /// Creates all the required objects on startup
    /// </summary>
    /// <param name="e"></param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                // Register domain + GUI services.
                services.AddPeriFormServices();

                // Register the main window so DI can construct it (and its dependencies).
                services.AddSingleton<MainWindow>();
            })
            .Build();

        // Initialize the ViewModelLocator with a scope factory (per-view lifetime).
        var scopeFactory = _host.Services.GetRequiredService<IServiceScopeFactory>();
        ViewModelLocator.SetServiceScopeFactory(scopeFactory);

        // Resolve and display the main window.
        var window = _host.Services.GetRequiredService<MainWindow>();
        window.Show();
    }

    /// <summary>
    /// Disposes of all unmanaged objects on exit
    /// </summary>
    /// <param name="e"></param>
    protected override void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            try
            {
                // Give hosted services a chance to shut down gracefully.
                _host.StopAsync().GetAwaiter().GetResult();
            }
            finally
            {
                _host.Dispose();
            }
        }

        base.OnExit(e);
    }
}
