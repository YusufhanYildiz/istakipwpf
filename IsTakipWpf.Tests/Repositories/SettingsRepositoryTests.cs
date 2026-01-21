using System.Threading.Tasks;
using IsTakipWpf.Infrastructure;
using IsTakipWpf.Repositories;
using Xunit;

namespace IsTakipWpf.Tests.Repositories
{
    public class SettingsRepositoryTests
    {
        public SettingsRepositoryTests()
        {
            DatabaseBootstrap.Initialize();
        }

        [Fact]
        public async Task SetAndGetValue_ShouldWork()
        {
            // Arrange
            var repo = new SettingsRepository();
            string key = "TestKey";
            string value = "TestValue";

            // Act
            await repo.SetValueAsync(key, value);
            var retrieved = await repo.GetValueAsync(key);

            // Assert
            Assert.Equal(value, retrieved);
        }

        [Fact]
        public async Task UpdateValue_ShouldWork()
        {
            // Arrange
            var repo = new SettingsRepository();
            string key = "UpdateKey";
            await repo.SetValueAsync(key, "OldValue");

            // Act
            await repo.SetValueAsync(key, "NewValue");
            var retrieved = await repo.GetValueAsync(key);

            // Assert
            Assert.Equal("NewValue", retrieved);
        }
    }
}
