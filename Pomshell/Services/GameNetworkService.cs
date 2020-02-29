using Pomshell.Models;
using System.Collections.Generic;

namespace Pomshell.Services
{
    public class GameNetworkService
    {
        public IEnumerable<LinkshellItem> GetLinkshells() => new[]
        {
            new LinkshellItem { Text = "Linkshell 1" },
            new LinkshellItem { Text = "Linkshell 2", IsAdmin = true },
            new LinkshellItem { Text = "CWLS 1", IsChecked = true },
        };
    }
}
