using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerAsync(int id);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync();
        Task<(bool Success, string Message, int CustomerId)> CreateCustomerAsync(Customer customer);
        Task<(bool Success, string Message)> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, string city = null, string district = null);
        Task<int> AddMultipleAsync(IEnumerable<Customer> customers);
    }
}