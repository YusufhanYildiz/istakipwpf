using System;
using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Models;
using IsTakipWpf.Services;

namespace IsTakipWpf.ViewModels
{
    public class AddEditCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _address;
        private string _errorMessage;
        private int _customerId;

        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
        public string Address { get => _address; set => SetProperty(ref _address, value); }
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public bool IsEditMode => _customerId > 0;
        public string Title => IsEditMode ? "Müşteri Düzenle" : "Yeni Müşteri Ekle";

        public ICommand SaveCommand { get; }

        public AddEditCustomerViewModel(ICustomerService customerService, Customer customer = null)
        {
            _customerService = customerService;

            SaveCommand = new RelayCommand(async _ => await ExecuteSaveAsync());

            if (customer != null)
            {
                _customerId = customer.Id;
                FirstName = customer.FirstName;
                LastName = customer.LastName;
                PhoneNumber = customer.PhoneNumber;
                Address = customer.Address;
            }
        }

        private async Task ExecuteSaveAsync()
        {
            var (success, message) = await SaveAsync();
            if (success)
            {
                MaterialDesignThemes.Wpf.DialogHost.Close("RootDialog", true);
            }
            else
            {
                ErrorMessage = message;
            }
        }

        public async Task<(bool Success, string Message)> SaveAsync()
        {
            var customer = new Customer
            {
                Id = _customerId,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Address = Address
            };

            if (IsEditMode)
            {
                return await _customerService.UpdateCustomerAsync(customer);
            }
            else
            {
                var result = await _customerService.CreateCustomerAsync(customer);
                return (result.Success, result.Message);
            }
        }
    }
}