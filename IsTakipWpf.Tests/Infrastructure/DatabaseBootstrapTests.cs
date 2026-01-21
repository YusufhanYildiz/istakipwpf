using IsTakipWpf.Infrastructure;
using Xunit;

namespace IsTakipWpf.Tests.Infrastructure
{
    public class DatabaseBootstrapTests
    {
        [Fact]
        public void ConnectionString_ShouldContainDatabaseName()
        {
            // Act
            var connectionString = DatabaseBootstrap.ConnectionString;

            // Assert
            Assert.Contains("database.db", connectionString);
            Assert.Contains("Data Source=", connectionString);
        }
    }
}
