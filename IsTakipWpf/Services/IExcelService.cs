using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public interface IExcelService
    {
        Task<(bool Success, string Message, List<Customer> Data)> ImportCustomersAsync(string filePath);
        Task<(bool Success, string Message)> ExportCustomersAsync(string filePath, IEnumerable<Customer> customers);
        Task<(bool Success, string Message, List<Job> Data)> ImportJobsAsync(string filePath);
        Task<(bool Success, string Message)> ExportJobsAsync(string filePath, IEnumerable<Job> jobs);
    }
}
