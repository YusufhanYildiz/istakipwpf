using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Models;
using IsTakipWpf.Services;

namespace IsTakipWpf.ViewModels
{
    public class CustomerListViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly MaterialDesignThemes.Wpf.ISnackbarMessageQueue _messageQueue;
        private string _searchTerm;
        private bool _isLoading;

        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value))
                {
                    _ = SearchAsync();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadCustomersCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }

        public CustomerListViewModel(ICustomerService customerService, MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _customerService = customerService;
            _messageQueue = messageQueue;

            LoadCustomersCommand = new RelayCommand(async _ => await LoadCustomersAsync());
            SearchCommand = new RelayCommand(async _ => await SearchAsync());
            AddCustomerCommand = new RelayCommand(async _ => await AddCustomerAsync());
            EditCustomerCommand = new RelayCommand(async customer => await EditCustomerAsync(customer as Customer));
            DeleteCustomerCommand = new RelayCommand(async customer => await DeleteCustomerAsync(customer as Customer));
            
            _ = LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            IsLoading = true;
            try
            {
                var customers = await _customerService.GetActiveCustomersAsync();
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchAsync()
        {
            var results = await _customerService.SearchCustomersAsync(SearchTerm);
            Customers.Clear();
            foreach (var customer in results)
            {
                Customers.Add(customer);
            }
        }

        private async Task AddCustomerAsync()
        {
            var viewModel = new AddEditCustomerViewModel(_customerService);
            var view = new Views.AddEditCustomerDialog { DataContext = viewModel };

            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            if (result is bool b && b)
            {
                await LoadCustomersAsync();
            }
        }

        private async Task EditCustomerAsync(Customer customer)
        {
            if (customer == null) return;

            var viewModel = new AddEditCustomerViewModel(_customerService, customer);
            var view = new Views.AddEditCustomerDialog { DataContext = viewModel };

            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            if (result is bool b && b)
            {
                await LoadCustomersAsync();
            }
        }

        private async Task DeleteCustomerAsync(Customer customer)
        {
            if (customer == null) return;

            // Simple delete for now, can add confirmation dialog later
            var result = await _customerService.DeleteCustomerAsync(customer.Id);
            if (result)
            {
                _messageQueue.Enqueue("Müşteri başarıyla silindi.");
                await LoadCustomersAsync();
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
