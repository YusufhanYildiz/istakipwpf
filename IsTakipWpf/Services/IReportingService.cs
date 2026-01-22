using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public interface IReportingService
    {
        Task<(bool Success, string Message)> GenerateCustomerHistoryPdfAsync(string filePath, Customer customer, IEnumerable<Job> jobs);
        Task<(bool Success, string Message)> ExportCustomersToPdfAsync(string filePath, IEnumerable<Customer> customers);
        Task<(bool Success, string Message)> ExportJobsToPdfAsync(string filePath, IEnumerable<Job> jobs);
    }
}
