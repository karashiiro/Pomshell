using Microsoft.Extensions.DependencyInjection;
using Pomshell.Services;

namespace Pomshell.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public LinkshellListViewModel List { get; }

        public MainWindowViewModel(ServiceProvider services)
        {
            var net = services.GetRequiredService<GameNetworkService>();
            List = new LinkshellListViewModel(net.GetLinkshells());
        }
    }
}
