using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
#if __ANDROID__
                    // https://stackoverflow.com/questions/49867588/accessing-files-through-a-physical-path-in-xamarin-android
                    //var documentsFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    var configFile = "appsettings.json";

                    //https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/file-system
                    //var directories = Directory.EnumerateDirectories("./");
                    //foreach (var directory in directories)
                    //{
                    //    Console.WriteLine(directory);
                    //}
                    //var destinationPath = Path.Combine(documentsFolderPath, configFile);
                    //string[] files = Directory.GetFiles(documentsFolderPath);
                    config.AddJsonFile(configFile, optional: false, reloadOnChange: true);
#endif

#if WINDOWS10_0_17763_0_OR_GREATER
                    //https://stackoverflow.com/questions/69000474/how-to-load-app-configuration-from-appsettings-json-in-maui-startup
                    Assembly callingAssembly = Assembly.GetEntryAssembly();
                    Version versionRuntime = callingAssembly.GetName().Version;
                    string assemblyLocation = Path.GetDirectoryName(System.AppContext.BaseDirectory); //CallingAssembly.Location
                    var configFile = Path.Combine(assemblyLocation, "appsettings.json");
                    config.AddJsonFile(configFile, optional: false, reloadOnChange: true);
#endif
                });

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