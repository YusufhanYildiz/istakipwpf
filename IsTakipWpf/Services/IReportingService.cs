using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public interface IReportingService
    {
        Task<(bool Success, string Message)> GenerateCustomerHistoryPdfAsync(string filePath, Customer customer, IEnumerable<Job> jobs);
    }
}
