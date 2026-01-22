using System.Threading.Tasks;
using IsTakipWpf.Repositories;
using IsTakipWpf.Services;
using Moq;
using Xunit;

namespace IsTakipWpf.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<ISettingsRepository> _settingsRepoMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _settingsRepoMock = new Mock<ISettingsRepository>();
            _authService = new AuthService(_settingsRepoMock.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_WithCorrectPlainInitialPassword_ShouldSucceed()
        {
            // Arrange
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordHash")).ReturnsAsync("admin");
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordSalt")).ReturnsAsync("somesalt");
            _settingsRepoMock.Setup(r => r.SetValueAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.AuthenticateAsync("admin");

            // Assert
            Assert.True(result);
            _settingsRepoMock.Verify(r => r.SetValueAsync("AdminPasswordHash", It.Is<string>(s => s != "admin")), Times.Once);
            _settingsRepoMock.Verify(r => r.SetValueAsync("AdminPasswordSalt", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_WithCorrectHashedPassword_ShouldSucceed()
        {
            // Arrange
            string password = "mypassword";
            string salt = "randomsalt";
            string expectedHash = HashPassword(password, salt);
            
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordHash")).ReturnsAsync(expectedHash);
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordSalt")).ReturnsAsync(salt);

            // Act
            var result = await _authService.AuthenticateAsync(password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AuthenticateAsync_WithIncorrectPassword_ShouldFail()
        {
            // Arrange
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordHash")).ReturnsAsync("some_hash");
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordSalt")).ReturnsAsync("some_salt");

            // Act
            var result = await _authService.AuthenticateAsync("wrong_password");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ChangePasswordAsync_WithCorrectOldPassword_ShouldUpdateHashAndSalt()
        {
            // Arrange
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordHash")).ReturnsAsync("admin");
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordSalt")).ReturnsAsync("somesalt");
            _settingsRepoMock.Setup(r => r.SetValueAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.ChangePasswordAsync("admin", "new_secure_password");

            // Assert
            Assert.True(result);
            _settingsRepoMock.Verify(r => r.SetValueAsync("AdminPasswordHash", It.Is<string>(s => s != "admin")), Times.AtLeastOnce);
            _settingsRepoMock.Verify(r => r.SetValueAsync("AdminPasswordSalt", It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task SetRememberMe_ShouldSaveToSettings()
        {
            // Arrange
            _settingsRepoMock.Setup(r => r.SetValueAsync("RememberMe", "True")).ReturnsAsync(true);

            // Act
            await _authService.SetRememberMeAsync(true);

            // Assert
            _settingsRepoMock.Verify(r => r.SetValueAsync("RememberMe", "True"), Times.Once);
        }

        [Fact]
        public async Task SaveCredentialsAsync_ShouldEncryptAndSave()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";

            _settingsRepoMock.Setup(r => r.SetValueAsync("SavedUsername", username)).ReturnsAsync(true);
            _settingsRepoMock.Setup(r => r.SetValueAsync("SavedPassword", It.IsAny<string>())).ReturnsAsync(true);

            // Act
            await _authService.SaveCredentialsAsync(username, password);

            // Assert
            _settingsRepoMock.Verify(r => r.SetValueAsync("SavedUsername", username), Times.Once);
            _settingsRepoMock.Verify(r => r.SetValueAsync("SavedPassword", It.Is<string>(s => !string.IsNullOrEmpty(s) && s != password)), Times.Once);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var combined = password + salt;
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined));
                var builder = new System.Text.StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
