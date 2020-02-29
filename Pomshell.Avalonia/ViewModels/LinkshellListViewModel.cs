using Pomshell.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pomshell.ViewModels
{
    public class LinkshellListViewModel : ViewModelBase
    {
        public ObservableCollection<LinkshellItem> Items { get; }

        public LinkshellListViewModel(IEnumerable<LinkshellItem> items)
        {
            Items = new ObservableCollection<LinkshellItem>(items);
        }
    }
}
