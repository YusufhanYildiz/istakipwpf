using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;

namespace IsTakipWpf.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool Success, string Message, int CustomerId)> CreateCustomerAsync(Customer customer)
        {
            var (isValid, message) = ValidateCustomer(customer);
            if (!isValid)
            {
                return (false, message, 0);
            }

            try
            {
                var id = await _repository.AddAsync(customer);
                return (true, "MÃ¼ÅŸteri baÅŸarÄ±yla oluÅŸturuldu.", id);
            }
            catch (Exception ex)
            {
                return (false, $"MÃ¼ÅŸteri oluÅŸturulurken bir hata oluÅŸtu: {ex.Message}", 0);
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            return await _repository.SoftDeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            return await _repository.GetAllAsync(includeDeleted: false);
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetActiveCustomersAsync();
            }
            return await _repository.SearchAsync(searchTerm);
        }

        public async Task<(bool Success, string Message)> UpdateCustomerAsync(Customer customer)
        {
            var (isValid, message) = ValidateCustomer(customer);
            if (!isValid)
            {
                return (false, message);
            }

            try
            {
                var result = await _repository.UpdateAsync(customer);
                return result ? (true, "MÃ¼ÅŸteri gÃ¼ncellendi.") : (false, "MÃ¼ÅŸteri bulunamadÄ± veya gÃ¼ncellenemedi.");
            }
            catch (Exception ex)
            {
                return (false, $"MÃ¼ÅŸteri gÃ¼ncellenirken bir hata oluÅŸtu: {ex.Message}");
            }
        }

        private (bool IsValid, string Message) ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                return (false, "Ad alanÄ± boÅŸ bÄ±rakÄ±lamaz.");
            }

            if (string.IsNullOrWhiteSpace(customer.LastName))
            {
                return (false, "Soyad alanÄ± boÅŸ bÄ±rakÄ±lamaz.");
            }

            // Simple phone validation - can be improved later
            if (!string.IsNullOrWhiteSpace(customer.PhoneNumber) && customer.PhoneNumber.Length < 10)
            {
                return (false, "Telefon numarasÄ± geÃ§ersiz.");
            }

            return (true, string.Empty);
        }
    }
}
