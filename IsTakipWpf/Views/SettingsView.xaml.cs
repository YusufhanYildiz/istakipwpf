using System.Windows.Controls;
using IsTakipWpf.ViewModels;

namespace IsTakipWpf.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}