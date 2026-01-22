using System.Threading.Tasks;
using IsTakipWpf.Services;
using IsTakipWpf.ViewModels;
using Moq;
using Xunit;

namespace IsTakipWpf.Tests.ViewModels
{
    public class PasswordChangeViewModelTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly PasswordChangeViewModel _viewModel;

        public PasswordChangeViewModelTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _viewModel = new PasswordChangeViewModel(_authServiceMock.Object);
        }

        [Fact]
        public async Task ChangePasswordCommand_WithEmptyFields_ShouldShowErrorMessage()
        {
            // Act
            _viewModel.ChangePasswordCommand.Execute(null);

            // Assert
            Assert.False(string.IsNullOrEmpty(_viewModel.ErrorMessage));
        }

        [Fact]
        public async Task ChangePasswordCommand_WithMismatchedNewPasswords_ShouldShowErrorMessage()
        {
            // Arrange
            _viewModel.CurrentPassword = "admin";
            _viewModel.NewPassword = "newpassword";
            _viewModel.ConfirmPassword = "different";

            // Act
            _viewModel.ChangePasswordCommand.Execute(null);

            // Assert
            Assert.Equal("Yeni şifreler eşleşmiyor.", _viewModel.ErrorMessage);
        }

        [Fact]
        public async Task ChangePasswordCommand_WithCorrectData_ShouldCallAuthService()
        {
            // Arrange
            _viewModel.CurrentPassword = "admin";
            _viewModel.NewPassword = "newpassword";
            _viewModel.ConfirmPassword = "newpassword";
            
            _authServiceMock.Setup(s => s.ChangePasswordAsync("admin", "newpassword")).ReturnsAsync(true);

            // Act
            _viewModel.ChangePasswordCommand.Execute(null);

            // Assert
            _authServiceMock.Verify(s => s.ChangePasswordAsync("admin", "newpassword"), Times.Once);
            Assert.True(_viewModel.IsSuccess);
        }

        [Fact]
        public async Task ChangePasswordCommand_WhenAuthFails_ShouldShowErrorMessage()
        {
            // Arrange
            _viewModel.CurrentPassword = "wrong";
            _viewModel.NewPassword = "new";
            _viewModel.ConfirmPassword = "new";
            
            _authServiceMock.Setup(s => s.ChangePasswordAsync("wrong", "new")).ReturnsAsync(false);

            // Act
            _viewModel.ChangePasswordCommand.Execute(null);

            // Assert
            Assert.Equal("Mevcut şifre hatalı veya işlem başarısız.", _viewModel.ErrorMessage);
            Assert.False(_viewModel.IsSuccess);
        }
    }
}
