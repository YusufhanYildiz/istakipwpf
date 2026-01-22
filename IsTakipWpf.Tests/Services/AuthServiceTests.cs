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
            _settingsRepoMock.Setup(r => r.SetValueAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.AuthenticateAsync("admin");

            // Assert
            Assert.True(result);
            _settingsRepoMock.Verify(r => r.SetValueAsync("AdminPasswordHash", It.Is<string>(s => s != "admin")), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_WithIncorrectPassword_ShouldFail()
        {
            // Arrange
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordHash")).ReturnsAsync("some_hash");

            // Act
            var result = await _authService.AuthenticateAsync("wrong_password");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ChangePasswordAsync_WithCorrectOldPassword_ShouldUpdateHash()
        {
            // Arrange
            _settingsRepoMock.Setup(r => r.GetValueAsync("AdminPasswordHash")).ReturnsAsync("admin");
            _settingsRepoMock.Setup(r => r.SetValueAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _authService.ChangePasswordAsync("admin", "new_secure_password");

            // Assert
            Assert.True(result);
            _settingsRepoMock.Verify(r => r.SetValueAsync("AdminPasswordHash", It.Is<string>(s => s != "admin")), Times.AtLeastOnce);
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
    }
}
