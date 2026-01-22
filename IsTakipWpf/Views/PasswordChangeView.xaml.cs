using System.Windows;
using System.Windows.Controls;
using IsTakipWpf.ViewModels;

namespace IsTakipWpf.Views
{
    public partial class PasswordChangeView : UserControl
    {
        public PasswordChangeView(PasswordChangeViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void CurrentPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PasswordChangeViewModel vm)
            {
                vm.CurrentPassword = ((PasswordBox)sender).Password;
            }
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PasswordChangeViewModel vm)
            {
                vm.NewPassword = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PasswordChangeViewModel vm)
            {
                vm.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
