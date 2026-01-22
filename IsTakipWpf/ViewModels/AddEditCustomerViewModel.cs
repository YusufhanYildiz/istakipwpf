using System;
﻿using System.Collections.Generic;
﻿using System.Collections.ObjectModel;
﻿using System.Linq;
﻿using System.Threading.Tasks;
﻿using System.Windows.Input;
﻿using IsTakipWpf.Models;
﻿using IsTakipWpf.Services;
﻿
﻿namespace IsTakipWpf.ViewModels
﻿{
﻿    public class AddEditCustomerViewModel : ViewModelBase
﻿    {
﻿        private readonly ICustomerService _customerService;
﻿        private readonly ILocationService _locationService;
﻿        private string _firstName;
﻿        private string _lastName;
﻿        private string _phoneNumber;
﻿        private string _address;
﻿        private string _errorMessage;
﻿        private int _customerId;
﻿
﻿        private ObservableCollection<City> _cities = new ObservableCollection<City>();
﻿        private ObservableCollection<string> _districts = new ObservableCollection<string>();
﻿        private City _selectedCity;
﻿        private string _selectedDistrict;
﻿
﻿        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
﻿        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
﻿        public string PhoneNumber { get => _phoneNumber; set => SetProperty(ref _phoneNumber, value); }
﻿        public string Address { get => _address; set => SetProperty(ref _address, value); }
﻿        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }
﻿
﻿        public ObservableCollection<City> Cities => _cities;
﻿        public ObservableCollection<string> Districts => _districts;
﻿
﻿        public City SelectedCity
﻿        {
﻿            get => _selectedCity;
﻿            set
﻿            {
﻿                if (SetProperty(ref _selectedCity, value))
﻿                {
﻿                    _ = LoadDistrictsAsync();
﻿                }
﻿            }
﻿        }
﻿
﻿        public string SelectedDistrict
﻿        {
﻿            get => _selectedDistrict;
﻿            set => SetProperty(ref _selectedDistrict, value);
﻿        }
﻿
﻿        public bool IsEditMode => _customerId > 0;
﻿        public string Title => IsEditMode ? "Müşteri Düzenle" : "Yeni Müşteri Ekle";
﻿
﻿        public ICommand SaveCommand { get; }
﻿
﻿        public AddEditCustomerViewModel(ICustomerService customerService, ILocationService locationService, Customer customer = null)
﻿        {
﻿            _customerService = customerService;
﻿            _locationService = locationService;
﻿
﻿            SaveCommand = new RelayCommand(async _ => await ExecuteSaveAsync());
﻿
﻿            _ = InitializeAsync(customer);
﻿        }
﻿
﻿        private async Task InitializeAsync(Customer customer)
﻿        {
﻿            var cities = await _locationService.GetCitiesAsync();
﻿            Cities.Clear();
﻿            foreach (var city in cities) Cities.Add(city);
﻿
﻿            if (customer != null)
﻿            {
﻿                _customerId = customer.Id;
﻿                FirstName = customer.FirstName;
﻿                LastName = customer.LastName;
﻿                PhoneNumber = customer.PhoneNumber;
﻿                Address = customer.Address;
﻿
﻿                if (!string.IsNullOrEmpty(customer.City))
﻿                {
﻿                    SelectedCity = Cities.FirstOrDefault(c => c.Name == customer.City);
﻿                    if (SelectedCity != null && !string.IsNullOrEmpty(customer.District))
﻿                    {
﻿                        // Districts are loaded by SelectedCity setter
﻿                        SelectedDistrict = customer.District;
﻿                    }
﻿                }
﻿            }
﻿        }
﻿
﻿        private async Task LoadDistrictsAsync()
﻿        {
﻿            Districts.Clear();
﻿            if (SelectedCity != null)
﻿            {
﻿                var districts = await _locationService.GetDistrictsAsync(SelectedCity.Name);
﻿                foreach (var d in districts) Districts.Add(d);
﻿            }
﻿        }
﻿
﻿        private async Task ExecuteSaveAsync()
﻿        {
﻿            var (success, message) = await SaveAsync();
﻿            if (success)
﻿            {
﻿                MaterialDesignThemes.Wpf.DialogHost.Close("RootDialog", true);
﻿            }
﻿            else
﻿            {
﻿                ErrorMessage = message;
﻿            }
﻿        }
﻿
﻿        public async Task<(bool Success, string Message)> SaveAsync()
﻿        {
﻿            var customer = new Customer
﻿            {
﻿                Id = _customerId,
﻿                FirstName = FirstName,
﻿                LastName = LastName,
﻿                PhoneNumber = PhoneNumber,
﻿                City = SelectedCity?.Name,
﻿                District = SelectedDistrict,
﻿                Address = Address
﻿            };
﻿
﻿            if (IsEditMode)
﻿            {
﻿                return await _customerService.UpdateCustomerAsync(customer);
﻿            }
﻿            else
﻿            {
﻿                var result = await _customerService.CreateCustomerAsync(customer);
﻿                return (result.Success, result.Message);
﻿            }
﻿        }
﻿    }
﻿}
﻿