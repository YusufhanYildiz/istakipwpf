using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Services;

namespace IsTakipWpf.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private string _username;
        private string _password;
        private bool _isRememberMe;
        private bool _isPasswordVisible;
        private string _errorMessage;
        private bool? _loginResult;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(async _ => await ExecuteLogin());
            LoadRememberedSettings();
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRememberMe
        {
            get => _isRememberMe;
            set => SetProperty(ref _isRememberMe, value);
        }

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool? LoginResult
        {
            get => _loginResult;
            private set => SetProperty(ref _loginResult, value);
        }

        public ICommand LoginCommand { get; }

        private async void LoadRememberedSettings()
        {
            IsRememberMe = await _authService.GetRememberMeAsync();
            if (IsRememberMe)
            {
                var credentials = await _authService.GetSavedCredentialsAsync();
                if (!string.IsNullOrEmpty(credentials.Username))
                {
                    Username = credentials.Username;
                }
                if (!string.IsNullOrEmpty(credentials.Password))
                {
                    Password = credentials.Password;
                }
            }
            
            if (string.IsNullOrEmpty(Username))
            {
                Username = "admin";
            }
        }

        private async Task ExecuteLogin()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Lütfen şifrenizi girin.";
                return;
            }

            // Simple validation for demo/admin only
            if (Username?.ToLower() != "admin")
            {
                ErrorMessage = "Geçersiz kullanıcı adı.";
                return;
            }

            try 
            {
                bool isAuthenticated = await _authService.AuthenticateAsync(Password);
                
                if (isAuthenticated)
                {
                    await _authService.SetRememberMeAsync(IsRememberMe);
                    if (IsRememberMe)
                    {
                        await _authService.SaveCredentialsAsync(Username, Password);
                    }
                    LoginResult = true;
                }
                else
                {
                    ErrorMessage = "Hatalı şifre!";
                    LoginResult = false;
                }
            }
            catch (System.Exception ex)
            {
                 ErrorMessage = $"Hata: {ex.Message}";
            }
        }
    }
}