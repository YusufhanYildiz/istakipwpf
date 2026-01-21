using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;
using IsTakipWpf.Infrastructure;
using Xunit;

namespace IsTakipWpf.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        public CustomerRepositoryTests()
        {
            DatabaseBootstrap.Initialize();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNewIdAndPersistData()
        {
            // Arrange
            var repo = new CustomerRepository();
            var customer = new Customer 
            { 
                FirstName = "Test", 
                LastName = "User",
                PhoneNumber = "1234567890",
                Address = "Test Address"
            };

            // Act
            var id = await repo.AddAsync(customer);
            var retrieved = await repo.GetByIdAsync(id);

            // Assert
            Assert.True(id > 0);
            Assert.NotNull(retrieved);
            Assert.Equal(customer.FirstName, retrieved.FirstName);
            Assert.Equal(customer.LastName, retrieved.LastName);
        }

        [Fact]
        public async Task SoftDeleteAsync_ShouldMarkAsDeleted()
        {
            // Arrange
            var repo = new CustomerRepository();
            var customer = new Customer { FirstName = "Delete", LastName = "Me" };
            var id = await repo.AddAsync(customer);

            // Act
            var result = await repo.SoftDeleteAsync(id);
            var retrieved = await repo.GetByIdAsync(id);

            // Assert
            Assert.True(result);
            Assert.True(retrieved.IsDeleted);
        }
    }
}
