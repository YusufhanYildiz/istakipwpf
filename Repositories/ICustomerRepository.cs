using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync(bool includeDeleted = false);
        Task<int> AddAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> SoftDeleteAsync(int id);
        Task<IEnumerable<Customer>> SearchAsync(string searchTerm, string city = null, string district = null);
        Task<int> AddMultipleAsync(IEnumerable<Customer> customers);
    }
}