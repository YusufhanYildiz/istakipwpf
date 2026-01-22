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
                    else if (viewModel.LoginResult == false)
                    {
                        // Optional: Shake animation or focus reset
                    }
                }
                else if (e.PropertyName == nameof(LoginViewModel.IsPasswordVisible))
                {
                    if (!viewModel.IsPasswordVisible)
                    {
                        // Sync back to PasswordBox when switching to hidden mode
                        if (PasswordBox.Password != viewModel.Password)
                        {
                            PasswordBox.Password = viewModel.Password;
                        }
                        PasswordBox.Focus(); // Optional: move focus back
                    }
                    else
                    {
                         // Switching to visible mode - TextBox binds automatically
                         VisiblePasswordBox.Focus();
                    }
                }
                else if (e.PropertyName == nameof(LoginViewModel.Password))
                {
                    // If updated from outside (e.g. Remember Me loaded it), sync PasswordBox
                    if (!viewModel.IsPasswordVisible && PasswordBox.Password != viewModel.Password)
                    {
                        PasswordBox.Password = viewModel.Password;
                    }
                }
            };
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                // Only update if PasswordBox is actually visible/active source of truth
                if (!vm.IsPasswordVisible)
                {
                    vm.Password = ((PasswordBox)sender).Password;
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
