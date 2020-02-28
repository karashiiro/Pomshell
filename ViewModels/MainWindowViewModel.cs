using Pomshell.Services;

namespace Pomshell.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public LinkshellListViewModel List { get; }

        public MainWindowViewModel(GameNetworkService net)
        {
            List = new LinkshellListViewModel(net.GetLinkshells());
        }
    }
}
