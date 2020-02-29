using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Pomshell.Services;
using Pomshell.ViewModels;
using Pomshell.Views;

namespace Pomshell
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var net = new GameNetworkService();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(net),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
