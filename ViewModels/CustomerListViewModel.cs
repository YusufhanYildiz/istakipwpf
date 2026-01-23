using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Models;
using IsTakipWpf.Services;
using Microsoft.Win32;

namespace IsTakipWpf.ViewModels
{
    public class CustomerListViewModel : ViewModelBase, IRefreshable
    {
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;
        private readonly IExcelService _excelService;
        private readonly IReportingService _reportingService;
        private readonly MaterialDesignThemes.Wpf.ISnackbarMessageQueue _messageQueue;
        private string _searchTerm;
        private bool _isLoading;
        private City _selectedCityFilter;
        private string _selectedDistrictFilter;

        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();
        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();
        public ObservableCollection<string> Districts { get; } = new ObservableCollection<string>();

        public string SearchTerm
        {
            get => _searchTerm;
            set { if (SetProperty(ref _searchTerm, value)) _ = SearchAsync(); }
        }

        public City SelectedCityFilter
        {
            get => _selectedCityFilter;
            set
            {
                if (SetProperty(ref _selectedCityFilter, value))
                {
                    _ = LoadFilterDistrictsAsync();
                    _ = SearchAsync();
                }
            }
        }

        public string SelectedDistrictFilter
        {
            get => _selectedDistrictFilter;
            set { if (SetProperty(ref _selectedDistrictFilter, value)) _ = SearchAsync(); }
        }

        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public ICommand LoadCustomersCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }
        public ICommand ImportExcelCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand ExportPdfCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public CustomerListViewModel(
            ICustomerService customerService, 
            ILocationService locationService,
            IExcelService excelService,
            IReportingService reportingService,
            MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _customerService = customerService;
            _locationService = locationService;
            _excelService = excelService;
            _reportingService = reportingService;
            _messageQueue = messageQueue;

            LoadCustomersCommand = new RelayCommand(async _ => await LoadCustomersAsync());
            SearchCommand = new RelayCommand(async _ => await SearchAsync());
            AddCustomerCommand = new RelayCommand(async _ => await AddCustomerAsync());
            EditCustomerCommand = new RelayCommand(async customer => await EditCustomerAsync(customer as Customer));
            DeleteCustomerCommand = new RelayCommand(async customer => await DeleteCustomerAsync(customer as Customer));
            ImportExcelCommand = new RelayCommand(async _ => await ImportExcelAsync());
            ExportExcelCommand = new RelayCommand(async _ => await ExportExcelAsync());
            ExportPdfCommand = new RelayCommand(async _ => await ExportPdfAsync());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());
            
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var cities = await _locationService.GetCitiesAsync();
            Cities.Clear();
            foreach (var city in cities) Cities.Add(city);
            await LoadCustomersAsync();
        }

        public async Task RefreshAsync()
        {
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            IsLoading = true;
            try
            {
                var customers = await _customerService.SearchCustomersAsync(SearchTerm, SelectedCityFilter?.Name, SelectedDistrictFilter);
                Customers.Clear();
                foreach (var customer in customers) Customers.Add(customer);
            }
            finally { IsLoading = false; }
        }

        private async Task SearchAsync()
        {
            await LoadCustomersAsync();
        }

        private async Task LoadFilterDistrictsAsync()
        {
            Districts.Clear();
            if (SelectedCityFilter != null)
            {
                var districts = await _locationService.GetDistrictsAsync(SelectedCityFilter.Name);
                foreach (var d in districts) Districts.Add(d);
            }
        }

        private void ClearFilters()
        {
            SearchTerm = string.Empty;
            SelectedCityFilter = null;
            SelectedDistrictFilter = null;
        }

        private async Task AddCustomerAsync()
        {
            var viewModel = new AddEditCustomerViewModel(_customerService, _locationService);
            var view = new Views.AddEditCustomerDialog { DataContext = viewModel };
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            if (result is bool b && b) await LoadCustomersAsync();
        }

        private async Task EditCustomerAsync(Customer customer)
        {
            if (customer == null) return;
            var viewModel = new AddEditCustomerViewModel(_customerService, _locationService, customer);
            var view = new Views.AddEditCustomerDialog { DataContext = viewModel };
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            if (result is bool b && b) await LoadCustomersAsync();
        }

        private async Task DeleteCustomerAsync(Customer customer)
        {
            if (customer == null) return;
            var result = await _customerService.DeleteCustomerAsync(customer.Id);
            if (result)
            {
                _messageQueue.Enqueue("Müşteri başarıyla silindi.");
                await LoadCustomersAsync();
            }
        }

        private async Task ImportExcelAsync()
        {
            var openFileDialog = new OpenFileDialog { Filter = "Excel Dosyası (*.xlsx)|*.xlsx" };
            if (openFileDialog.ShowDialog() == true)
            {
                var result = await _excelService.ImportCustomersAsync(openFileDialog.FileName);
                if (result.Success && result.Data != null && result.Data.Any())
                {
                    var count = await _customerService.AddMultipleAsync(result.Data);
                    await LoadCustomersAsync();
                    _messageQueue.Enqueue($"{count} müşteri başarıyla aktarıldı.");
                }
                else
                {
                    _messageQueue.Enqueue(result.Message);
                }
            }
        }

        private async Task ExportExcelAsync()
        {
            var saveFileDialog = new SaveFileDialog { Filter = "Excel Dosyası (*.xlsx)|*.xlsx", FileName = "Musteriler.xlsx" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var result = await _excelService.ExportCustomersAsync(saveFileDialog.FileName, Customers);
                _messageQueue.Enqueue(result.Message);
            }
        }

        private async Task ExportPdfAsync()
        {
            var saveFileDialog = new SaveFileDialog { Filter = "PDF Dosyası (*.pdf)|*.pdf", FileName = "Musteriler.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var result = await _reportingService.ExportCustomersToPdfAsync(saveFileDialog.FileName, Customers);
                _messageQueue.Enqueue(result.Message);
            }
        }
    }
}
