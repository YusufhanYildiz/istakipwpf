using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Repositories
{
    public interface IJobRepository
    {
        Task<Job> GetByIdAsync(int id);
        Task<IEnumerable<Job>> GetAllAsync(bool includeDeleted = false);
        Task<IEnumerable<Job>> GetByCustomerIdAsync(int customerId);
        Task<int> AddAsync(Job job);
        Task<bool> UpdateAsync(Job job);
        Task<bool> SoftDeleteAsync(int id);
        Task<IEnumerable<Job>> SearchAsync(string searchTerm, int? customerId = null, JobStatus? status = null, string city = null, string district = null);
        Task<int> AddMultipleAsync(IEnumerable<Job> jobs);
    }
}