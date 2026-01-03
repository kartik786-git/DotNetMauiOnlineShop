using Microsoft.Extensions.Logging;
using ShopApp.Services;
using ShopApp.ViewModels;
using ShopApp.Views;

namespace ShopApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<IApiService, ApiService>();

        // ViewModels
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddTransient<ProductDetailsViewModel>();
        builder.Services.AddSingleton<CartViewModel>();
        builder.Services.AddSingleton<OrdersViewModel>();

        // Views
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddTransient<ProductDetailsPage>();
        builder.Services.AddSingleton<CartPage>();
        builder.Services.AddSingleton<OrdersPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            System.Diagnostics.Debug.WriteLine($"[CRASH] Unhandled Exception: {error.ExceptionObject}");
        };

        TaskScheduler.UnobservedTaskException += (sender, error) =>
        {
            System.Diagnostics.Debug.WriteLine($"[CRASH] Unobserved Task Exception: {error.Exception}");
        };

        return builder.Build();
    }
}
