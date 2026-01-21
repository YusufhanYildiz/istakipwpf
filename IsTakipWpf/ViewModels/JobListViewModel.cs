using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Models;
using IsTakipWpf.Services;

namespace IsTakipWpf.ViewModels
{
    public class JobListViewModel : ViewModelBase
    {
        private readonly IJobService _jobService;
        private readonly ICustomerService _customerService;
        private readonly MaterialDesignThemes.Wpf.ISnackbarMessageQueue _messageQueue;
        private string _searchTerm;
        private JobStatus? _selectedStatus;
        private Customer _selectedCustomer;
        private bool _isLoading;

        public ObservableCollection<Job> Jobs { get; } = new ObservableCollection<Job>();
        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();
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

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand AddJobCommand { get; }
        public ICommand EditJobCommand { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand RefreshCommand { get; }

        public JobListViewModel(IJobService jobService, ICustomerService customerService, MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _jobService = jobService;
            _customerService = customerService;
            _messageQueue = messageQueue;

            AddJobCommand = new RelayCommand(async _ => await AddJobAsync());
            EditJobCommand = new RelayCommand(async job => await EditJobAsync(job as Job));
            DeleteJobCommand = new RelayCommand(async job => await DeleteJobAsync(job as Job));
            RefreshCommand = new RelayCommand(async _ => await InitializeAsync());

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            IsLoading = true;
            try
            {
                var customers = await _customerService.GetActiveCustomersAsync();
                Customers.Clear();
                foreach (var c in customers) Customers.Add(c);

                await FilterAsync();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task FilterAsync()
        {
            var results = await _jobService.SearchJobsAsync(SearchTerm, SelectedCustomer?.Id, SelectedStatus);
            Jobs.Clear();
            foreach (var job in results) Jobs.Add(job);
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
    }
}