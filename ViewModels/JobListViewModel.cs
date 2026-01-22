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
    public class JobListViewModel : ViewModelBase, IRefreshable
    {
        private readonly IJobService _jobService;
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;
        private readonly IExcelService _excelService;
        private readonly IReportingService _reportingService;
        private readonly MaterialDesignThemes.Wpf.ISnackbarMessageQueue _messageQueue;
        private string _searchTerm;
        private JobStatus? _selectedStatus;
        private Customer _selectedCustomer;
        private bool _isLoading;
        private City _selectedCityFilter;
        private string _selectedDistrictFilter;

        public ObservableCollection<Job> Jobs { get; } = new ObservableCollection<Job>();
        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();
        public ObservableCollection<City> Cities { get; } = new ObservableCollection<City>();
        public ObservableCollection<string> Districts { get; } = new ObservableCollection<string>();
        public Array Statuses => Enum.GetValues(typeof(JobStatus));

        public string SearchTerm
        {
            get => _searchTerm;
            set { if (SetProperty(ref _searchTerm, value)) _ = FilterAsync(); }
        }

        public JobStatus? SelectedStatus
        {
            get => _selectedStatus;
            set { if (SetProperty(ref _selectedStatus, value)) _ = FilterAsync(); }
        }

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set { if (SetProperty(ref _selectedCustomer, value)) _ = FilterAsync(); }
        }

        public City SelectedCityFilter
        {
            get => _selectedCityFilter;
            set
            {
                if (SetProperty(ref _selectedCityFilter, value))
                {
                    _ = LoadFilterDistrictsAsync();
                    _ = FilterAsync();
                }
            }
        }

        public string SelectedDistrictFilter
        {
            get => _selectedDistrictFilter;
            set { if (SetProperty(ref _selectedDistrictFilter, value)) _ = FilterAsync(); }
        }

        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        public ICommand AddJobCommand { get; }
        public ICommand EditJobCommand { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ImportExcelCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand ExportPdfCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public JobListViewModel(
            IJobService jobService, 
            ICustomerService customerService,
            ILocationService locationService,
            IExcelService excelService,
            IReportingService reportingService,
            MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _jobService = jobService;
            _customerService = customerService;
            _locationService = locationService;
            _excelService = excelService;
            _reportingService = reportingService;
            _messageQueue = messageQueue;

            AddJobCommand = new RelayCommand(async _ => await AddJobAsync());
            EditJobCommand = new RelayCommand(async job => await EditJobAsync(job as Job));
            DeleteJobCommand = new RelayCommand(async job => await DeleteJobAsync(job as Job));
            RefreshCommand = new RelayCommand(async _ => await InitializeAsync());
            ImportExcelCommand = new RelayCommand(async _ => await ImportExcelAsync());
            ExportExcelCommand = new RelayCommand(async _ => await ExportExcelAsync());
            ExportPdfCommand = new RelayCommand(async _ => await ExportPdfAsync());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

            _ = InitializeAsync();
        }

        public async Task RefreshAsync()
        {
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            IsLoading = true;
            try
            {
                var customers = await _customerService.GetActiveCustomersAsync();
                Customers.Clear();
                foreach (var c in customers) Customers.Add(c);

                var cities = await _locationService.GetCitiesAsync();
                Cities.Clear();
                foreach (var city in cities) Cities.Add(city);

                await FilterAsync();
            }
            finally { IsLoading = false; }
        }

        private async Task FilterAsync()
        {
            var results = await _jobService.SearchJobsAsync(SearchTerm, SelectedCustomer?.Id, SelectedStatus, SelectedCityFilter?.Name, SelectedDistrictFilter);
            Jobs.Clear();
            foreach (var job in results) Jobs.Add(job);
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
            SelectedCustomer = null;
            SelectedStatus = null;
            SelectedCityFilter = null;
            SelectedDistrictFilter = null;
        }

        private async Task AddJobAsync()
        {
            var viewModel = new AddEditJobViewModel(_jobService, _customerService);
            var view = new Views.AddEditJobDialog { DataContext = viewModel };
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            if (result is bool b && b) await FilterAsync();
        }

        private async Task EditJobAsync(Job job)
        {
            if (job == null) return;
            var viewModel = new AddEditJobViewModel(_jobService, _customerService, job);
            var view = new Views.AddEditJobDialog { DataContext = viewModel };
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            if (result is bool b && b) await FilterAsync();
        }

        private async Task DeleteJobAsync(Job job)
        {
            if (job == null) return;
            if (await _jobService.DeleteJobAsync(job.Id))
            {
                _messageQueue.Enqueue("İş başarıyla silindi.");
                await FilterAsync();
            }
        }

        private async Task ImportExcelAsync()
        {
            var openFileDialog = new OpenFileDialog { Filter = "Excel Dosyası (*.xlsx)|*.xlsx" };
            if (openFileDialog.ShowDialog() == true)
            {
                var result = await _excelService.ImportJobsAsync(openFileDialog.FileName);
                _messageQueue.Enqueue(result.Message);
                if (result.Success) await FilterAsync();
            }
        }

        private async Task ExportExcelAsync()
        {
            var saveFileDialog = new SaveFileDialog { Filter = "Excel Dosyası (*.xlsx)|*.xlsx", FileName = "Isler.xlsx" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var result = await _excelService.ExportJobsAsync(saveFileDialog.FileName, Jobs);
                _messageQueue.Enqueue(result.Message);
            }
        }

        private async Task ExportPdfAsync()
        {
            var saveFileDialog = new SaveFileDialog { Filter = "PDF Dosyası (*.pdf)|*.pdf", FileName = "Isler.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var result = await _reportingService.ExportJobsToPdfAsync(saveFileDialog.FileName, Jobs);
                _messageQueue.Enqueue(result.Message);
            }
        }
    }
}
