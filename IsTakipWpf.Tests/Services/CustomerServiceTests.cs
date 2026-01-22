using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;
using IsTakipWpf.Services;
using Moq;
using Xunit;

namespace IsTakipWpf.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _service = new CustomerService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldReturnFail_WhenFirstNameIsEmpty()
        {
            // Arrange
            var customer = new Customer { FirstName = "", LastName = "User" };

            // Act
            var result = await _service.CreateCustomerAsync(customer);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Ad", result.Message);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldReturnSuccess_WhenDataIsValid()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe" };
            _mockRepo.Setup(r => r.AddAsync(customer)).ReturnsAsync(1);

            // Act
            var result = await _service.CreateCustomerAsync(customer);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(1, result.CustomerId);
            _mockRepo.Verify(r => r.AddAsync(customer), Times.Once);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldReturnFail_WhenPhoneIsInvalid()
        {
            // Arrange
            var customer = new Customer { FirstName = "John", LastName = "Doe", PhoneNumber = "123" };

            // Act
            var result = await _service.CreateCustomerAsync(customer);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Telefon", result.Message);
        }
    }
}
