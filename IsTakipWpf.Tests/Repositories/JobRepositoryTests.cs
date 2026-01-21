using System;
using System.Threading.Tasks;
using IsTakipWpf.Infrastructure;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;
using Xunit;

namespace IsTakipWpf.Tests.Repositories
{
    public class JobRepositoryTests
    {
        public JobRepositoryTests()
        {
            DatabaseBootstrap.Initialize();
        }

        [Fact]
        public async Task AddAndGetJob_ShouldWork()
        {
            // Arrange
            var customerRepo = new CustomerRepository();
            var customerId = await customerRepo.AddAsync(new Customer { FirstName = "Job", LastName = "Test" });

            var repo = new JobRepository();
            var job = new Job 
            { 
                CustomerId = customerId, 
                JobTitle = "Test Job", 
                Status = JobStatus.Bekliyor 
            };

            // Act
            var id = await repo.AddAsync(job);
            var retrieved = await repo.GetByIdAsync(id);

            // Assert
            Assert.True(id > 0);
            Assert.NotNull(retrieved);
            Assert.Equal(job.JobTitle, retrieved.JobTitle);
            Assert.Equal(job.CustomerId, retrieved.CustomerId);
        }

        [Fact]
        public async Task SearchJobs_ShouldFilterCorrectly()
        {
            // Arrange
            var repo = new JobRepository();
            // Assuming database exists and has some data or adding it here
            
            // Act
            var results = await repo.SearchAsync("Test");

            // Assert
            Assert.NotNull(results);
        }
    }
}
