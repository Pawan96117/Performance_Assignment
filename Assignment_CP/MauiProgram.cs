using Assignment_CP.Interfaces;
using Assignment_CP.Services;
using Microsoft.Extensions.Logging;

namespace Assignment_CP;

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
            });

        builder.Services.AddMauiBlazorWebView();

        builder.Configuration["Monitoring:IntervalSeconds"] = "5";
        builder.Configuration["Monitoring:ApiUrl"] = "https://httpbin.org/post"; //dummy api

#if WINDOWS
    builder.Services.AddSingleton<ISystemInfoService, SystemInfoService>();
    builder.Services.AddSingleton<SystemMonitorService>();

    // Register plugins
    builder.Services.AddSingleton<IMonitorPlugin, FileLoggerPlugin>();
    builder.Services.AddSingleton<IMonitorPlugin, ApiPostPlugin>();
#endif

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

#if WINDOWS
        var monitor = app.Services.GetService<SystemMonitorService>();
        monitor?.Start();
#endif

        return app;
    }
}
