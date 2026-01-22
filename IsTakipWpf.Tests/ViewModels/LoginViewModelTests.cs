using System.Threading.Tasks;
using IsTakipWpf.Services;
using IsTakipWpf.ViewModels;
using Moq;
using Xunit;

namespace IsTakipWpf.Tests.ViewModels
{
    public class LoginViewModelTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly LoginViewModel _viewModel;

        public LoginViewModelTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _viewModel = new LoginViewModel(_authServiceMock.Object);
        }

        [Fact]
        public async Task LoginCommand_WithEmptyPassword_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.Password = string.Empty;

            // Act
            _viewModel.LoginCommand.Execute(null);

            // Assert
            Assert.Equal("Lütfen şifrenizi girin.", _viewModel.ErrorMessage);
        }

        [Fact]
        public async Task LoginCommand_WithCorrectPassword_ShouldSetLoginResultTrue()
        {
            // Arrange
            _viewModel.Username = "admin";
            _viewModel.Password = "correct_password";
            _authServiceMock.Setup(s => s.AuthenticateAsync("correct_password")).ReturnsAsync(true);

            // Act
            // Since it's async void in the command for simplicity in WPF, we might need a small delay or use a task-based approach.
            // But here ExecuteLogin is private and called via RelayCommand.
            // I'll make ExecuteLogin internal or just wait a bit.
            // Actually, I can use a TaskCompletionSource if I want to be fancy, but let's just wait.
            _viewModel.LoginCommand.Execute(null);
            
            // Wait for async task
            await Task.Delay(100);

            // Assert
            Assert.True(_viewModel.LoginResult);
            Assert.Empty(_viewModel.ErrorMessage);
        }

        [Fact]
        public async Task LoginCommand_WithWrongPassword_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.Username = "admin";
            _viewModel.Password = "wrong_password";
            _authServiceMock.Setup(s => s.AuthenticateAsync("wrong_password")).ReturnsAsync(false);

            // Act
            _viewModel.LoginCommand.Execute(null);
            await Task.Delay(100);

            // Assert
            Assert.False(_viewModel.LoginResult);
            Assert.Equal("Hatalı şifre!", _viewModel.ErrorMessage);
        }
    }
}
