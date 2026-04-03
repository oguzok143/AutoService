using Avalonia;
using System;
using AutoService.DB;
using AutoService.Models;
using AutoService.ViewModels;
using AutoService.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoService;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder().
            ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
            }).
            ConfigureServices((c,s) =>
            {
                s.Configure<DatabaseConnection>(c.Configuration.
                    GetSection("DatabaseConnection"));
                s.AddTransient<MainWindow>();
                s.AddTransient<MainWindowViewModel>();
                s.AddTransient<WorksWindow>();
                s.AddTransient<WorksWindowViewModel>();
                s.AddTransient<ReceiptWindow>();
                s.AddTransient<OrderWindowViewModel>();
                s.AddSingleton<ServiceRepository>();
                s.AddSingleton<WorkRepository>();
                s.AddSingleton<OrderRepository>();
            }).
            Build();
        BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure(()=> new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}