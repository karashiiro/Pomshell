using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Pomshell.Services;
using Pomshell.ViewModels;
using Pomshell.Views;

namespace Pomshell
{
    public class App : Application
    {
        private ServiceProvider _services;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            _services = ConfigureServices();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(_services),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private ServiceProvider ConfigureServices()
        {
            var sc = new ServiceCollection()
                .AddSingleton<GameNetworkService>();
            return sc.BuildServiceProvider();
        }
    }
}
