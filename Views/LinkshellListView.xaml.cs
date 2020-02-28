using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pomshell.Views
{
    public class LinkshellListView : UserControl
    {
        public LinkshellListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
