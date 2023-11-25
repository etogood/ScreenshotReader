using CoinsCounter.Commands;
using CoinsCounter.Stores;
using CoinsCounter.ViewModels;
using CoinsCounter.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace CoinsCounter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IHost Host = CreateHostBuilder().Build();

        private static IHostBuilder CreateHostBuilder(string[]? args = null) => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            services =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<UpdateImageCommand>();
                services.AddSingleton<SaveCommand>();
                services.AddSingleton<ChangeFileCommand>();

                services.AddSingleton<IEnvironmentStore, EnvironmentStore>();
                services.AddSingleton<ICoinsStore, CoinsStore>();
            });

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host.StartAsync();

            MainWindow = Host.Services.GetRequiredService<MainWindow>();
            MainWindow.DataContext = Host.Services.GetRequiredService<MainWindowViewModel>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await Host.StopAsync();
            Host.Dispose();

            base.OnExit(e);
        }
    }
}
