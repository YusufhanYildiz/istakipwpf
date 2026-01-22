using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Services;

namespace IsTakipWpf.ViewModels
{
    public class PasswordChangeViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private string _currentPassword;
        private string _newPassword;
        private string _confirmPassword;
        private string _errorMessage;
        private bool _isSuccess;

        public PasswordChangeViewModel(IAuthService authService)
        {
            _authService = authService;
            ChangePasswordCommand = new RelayCommand(async _ => await ExecuteChangePassword());
        }

        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsSuccess
        {
            get => _isSuccess;
            set => SetProperty(ref _isSuccess, value);
        }

        public ICommand ChangePasswordCommand { get; }

        private async Task ExecuteChangePassword()
        {
            ErrorMessage = string.Empty;
            IsSuccess = false;

            if (string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
            {
                ErrorMessage = "Lütfen tüm alanları doldurun.";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Yeni şifreler eşleşmiyor.";
                return;
            }

            bool result = await _authService.ChangePasswordAsync(CurrentPassword, NewPassword);
            if (result)
            {
                IsSuccess = true;
                CurrentPassword = string.Empty;
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
            }
            else
            {
                ErrorMessage = "Mevcut şifre hatalı veya işlem başarısız.";
            }
        }
    }
}
