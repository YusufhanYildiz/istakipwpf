using System;
using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;
using IsTakipWpf.Services;
using Moq;
using Xunit;

namespace IsTakipWpf.Tests.Services
{
    public class JobServiceTests
    {
        private readonly Mock<IJobRepository> _mockRepo;
        private readonly JobService _service;

        public JobServiceTests()
        {
            _mockRepo = new Mock<IJobRepository>();
            _service = new JobService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateJobAsync_ShouldFail_WhenTitleIsEmpty()
        {
            // Arrange
            var job = new Job { CustomerId = 1, JobTitle = "" };

            // Act
            var result = await _service.CreateJobAsync(job);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("başlığı", result.Message);
        }

        [Fact]
        public async Task CreateJobAsync_ShouldFail_WhenDatesAreInvalid()
        {
            // Arrange
            var job = new Job 
            { 
                CustomerId = 1, 
                JobTitle = "Test", 
                StartDate = DateTime.Now.AddDays(1), 
                EndDate = DateTime.Now 
            };

            // Act
            var result = await _service.CreateJobAsync(job);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("tarih", result.Message);
        }

        [Fact]
        public async Task CreateJobAsync_ShouldSucceed_WhenValid()
        {
            // Arrange
            var job = new Job { CustomerId = 1, JobTitle = "Valid Job" };
            _mockRepo.Setup(r => r.AddAsync(job)).ReturnsAsync(1);

            // Act
            var result = await _service.CreateJobAsync(job);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(1, result.JobId);
        }
    }
}
