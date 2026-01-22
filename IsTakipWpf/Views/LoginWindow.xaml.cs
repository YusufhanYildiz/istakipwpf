using System.Windows;
using System.Windows.Controls;
using IsTakipWpf.ViewModels;

namespace IsTakipWpf.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LoginViewModel.LoginResult))
                {
                    if (viewModel.LoginResult == true)
                    {
                        DialogResult = true;
                        Close();
                    }
                }
            };
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
