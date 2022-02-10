using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System;
using System.IO;
using System.Reflection;
using WebViewWithSendingPrint.Maui.Services;

namespace WebViewWithSendingPrint.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .RegisterBlazorMauiWebView()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .Host
                .ConfigureAppConfiguration((app, config) =>
                {
                    //https://stackoverflow.com/questions/70280264/maui-what-build-action-for-appsettings-json-and-how-to-access-the-file-on-andro
                    var assembly = typeof(App).GetTypeInfo().Assembly;
                    config.AddJsonFile(new EmbeddedFileProvider(assembly), "appsettings.json", optional: false, false);
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<OnScreenLogs>();

            //https://docs.microsoft.com/en-us/dotnet/maui/fundamentals/app-startup
#if __ANDROID__
            builder.Services.AddSingleton<IPrintingService, AndroidPrintingService>();
#endif

#if WINDOWS10_0_17763_0_OR_GREATER
            builder.Services.AddSingleton<IPrintingService, WindowsPrintingService>();
#endif
            builder.Services.AddBlazorWebView();

            return builder.Build();
        }
    }
}