using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public interface IJobService
    {
        Task<Job> GetJobAsync(int id);
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<(bool Success, string Message, int JobId)> CreateJobAsync(Job job);
        Task<(bool Success, string Message)> UpdateJobAsync(Job job);
        Task<bool> DeleteJobAsync(int id);
        Task<IEnumerable<Job>> SearchJobsAsync(string searchTerm, int? customerId = null, JobStatus? status = null, string city = null, string district = null);
        Task<bool> UpdateJobStatusAsync(int id, JobStatus status);
        Task<int> AddMultipleAsync(IEnumerable<Job> jobs);
    }
}