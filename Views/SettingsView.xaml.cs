using System.Windows;
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

        private void CopyHwid_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm && !string.IsNullOrEmpty(vm.HardwareId))
            {
                Clipboard.SetText(vm.HardwareId);
            }
        }
    }
}