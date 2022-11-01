using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using WinUI3Sample.Activation;
using WinUI3Sample.Contracts.Services;
using WinUI3Sample.Core.Contracts.Services;
using WinUI3Sample.Core.Services;
using WinUI3Sample.Helpers;
using WinUI3Sample.Services;
using WinUI3Sample.ViewModels;
using WinUI3Sample.Views;
using Microsoft.Windows.AppNotifications;

namespace WinUI3Sample;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<SpiralViewModel>();
            services.AddTransient<SpiralPage>();

            // Configuration
        }).
        Build();

        UnhandledException += App_UnhandledException;

        // Register notification manager
        AppNotificationManager.Default.Register();
        AppDomain.CurrentDomain.ProcessExit += new EventHandler((object? o, EventArgs args) =>
        {
            AppNotificationManager.Default.Unregister();
        });

        // Display startup toast notification
        // see: https://learn.microsoft.com/en-us/windows/apps/design/shell/tiles-and-notifications/adaptive-interactive-toasts?tabs=builder-syntax
        var notificationPayload = new string(
                "<toast>"+
                    "<visual>"+
                        "<binding template = \"ToastGeneric\">"+
                            "<text>Starting...</text>"+
                        "</binding>"+
                    "</visual>"+
                "</toast>"
            );
        var toast = new AppNotification(notificationPayload);
        AppNotificationManager.Default.Show(toast);
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
