using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IsTakipWpf.Models;
using IsTakipWpf.Repositories;

namespace IsTakipWpf.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILicenseService _licenseService;

        public CustomerService(ICustomerRepository customerRepository, ILicenseService licenseService)
        {
            _customerRepository = customerRepository;
            _licenseService = licenseService;
        }

        public async Task<(bool Success, string Message, int CustomerId)> CreateCustomerAsync(Customer customer)
        {
            if (!await _licenseService.IsTrialActiveAsync())
            {
                return (false, "Deneme süreniz dolmuştur. Lütfen devam etmek için lisans satın alınız.", 0);
            }

            var (isValid, message) = ValidateCustomer(customer);
            if (!isValid)
            {
                return (false, message, 0);
            }

            try
            {
                var id = await _customerRepository.AddAsync(customer);
                return (true, "Müşteri başarıyla oluşturuldu.", id);
            }
            catch (Exception ex)
            {
                return (false, $"Müşteri oluşturulurken bir hata oluştu: {ex.Message}", 0);
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            return await _customerRepository.SoftDeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            return await _customerRepository.GetAllAsync(includeDeleted: false);
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, string city = null, string district = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(district))
            {
                return await GetActiveCustomersAsync();
            }
            return await _customerRepository.SearchAsync(searchTerm, city, district);
        }

        public async Task<(bool Success, string Message)> UpdateCustomerAsync(Customer customer)
        {
            if (!await _licenseService.IsTrialActiveAsync())
            {
                return (false, "Deneme süreniz dolmuştur. Lütfen devam etmek için lisans satın alınız.");
            }

            var (isValid, message) = ValidateCustomer(customer);
            if (!isValid)
            {
                return (false, message);
            }

            try
            {
                var result = await _customerRepository.UpdateAsync(customer);
                return result ? (true, "Müşteri güncellendi.") : (false, "Müşteri bulunamadı veya güncellenemedi.");
            }
            catch (Exception ex)
            {
                return (false, $"Müşteri güncellenirken bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<int> AddMultipleAsync(IEnumerable<Customer> customers)
        {
            return await _customerRepository.AddMultipleAsync(customers);
        }

        private (bool IsValid, string Message) ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                return (false, "Ad alanı boş bırakılamaz.");
            }

            if (string.IsNullOrWhiteSpace(customer.LastName))
            {
                return (false, "Soyad alanı boş bırakılamaz.");
            }

            if (!string.IsNullOrWhiteSpace(customer.PhoneNumber) && customer.PhoneNumber.Length < 10)
            {
                return (false, "Telefon numarası geçersiz.");
            }

            return (true, string.Empty);
        }
    }
}