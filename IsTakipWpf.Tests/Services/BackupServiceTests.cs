using System;
using System.IO;
using System.Threading.Tasks;
using IsTakipWpf.Infrastructure;
using IsTakipWpf.Repositories;
using IsTakipWpf.Services;
using Moq;
using Xunit;

namespace IsTakipWpf.Tests.Services
{
    public class BackupServiceTests
    {
        private readonly Mock<ISettingsRepository> _mockSettingsRepo;
        private readonly BackupService _service;

        public BackupServiceTests()
        {
            _mockSettingsRepo = new Mock<ISettingsRepository>();
            _service = new BackupService(_mockSettingsRepo.Object);
            DatabaseBootstrap.Initialize();
        }

        [Fact]
        public async Task CreateBackup_ShouldCopyFileAndSaveHistory()
        {
            // Arrange
            string tempFolder = Path.Combine(Path.GetTempPath(), "IsTakipBackups");
            if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);

            // Act
            var result = await _service.CreateBackupAsync(tempFolder);

            // Assert
            Assert.True(result.Success);
            Assert.True(Directory.Exists(tempFolder));
            Assert.NotEmpty(Directory.GetFiles(tempFolder, "*.db"));
            _mockSettingsRepo.Verify(r => r.SetValueAsync("BackupHistory", It.IsAny<string>()), Times.Once);
        }
    }
}
