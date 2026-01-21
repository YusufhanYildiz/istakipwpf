using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Services;
using Xunit;

namespace IsTakipWpf.Tests.Services
{
    public class IntegrationServicesTests
    {
        [Fact]
        public async Task GeneratePdf_ShouldCreateFile()
        {
            // Arrange
            var service = new ReportingService();
            string filePath = Path.Combine(Path.GetTempPath(), "test_report.pdf");
            if (File.Exists(filePath)) File.Delete(filePath);

            var customer = new Customer { FirstName = "John", LastName = "Doe" };
            var jobs = new List<Job> 
            { 
                new Job { JobTitle = "Task 1", Status = JobStatus.Tamamlandi },
                new Job { JobTitle = "Task 2", Status = JobStatus.Bekliyor }
            };

            // Act
            var result = await service.GenerateCustomerHistoryPdfAsync(filePath, customer, jobs);

            // Assert
            Assert.True(result.Success);
            Assert.True(File.Exists(filePath));
            Assert.True(new FileInfo(filePath).Length > 0);
        }
    }
}
