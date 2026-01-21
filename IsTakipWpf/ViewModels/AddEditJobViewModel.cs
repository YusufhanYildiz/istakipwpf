using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Models;
using IsTakipWpf.Services;

namespace IsTakipWpf.ViewModels
{
    public class AddEditJobViewModel : ViewModelBase
    {
        private readonly IJobService _jobService;
        private readonly ICustomerService _customerService;
        private int _jobId;
        private int _customerId;
        private string _jobTitle;
        private string _description;
        private JobStatus _status;
        private DateTime? _startDate = DateTime.Now;
        private DateTime? _endDate;
        private string _errorMessage;
        private Customer _selectedCustomer;

        public string JobTitle { get => _jobTitle; set => SetProperty(ref _jobTitle, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public JobStatus Status { get => _status; set => SetProperty(ref _status, value); }
        public DateTime? StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
        public DateTime? EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }
        public Customer SelectedCustomer 
        { 
            get => _selectedCustomer; 
            set { if (SetProperty(ref _selectedCustomer, value)) _customerId = value?.Id ?? 0; }
        }

        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();
        public Array Statuses => Enum.GetValues(typeof(JobStatus));

        public bool IsEditMode => _jobId > 0;
        public string Title => IsEditMode ? "İşi Düzenle" : "Yeni İş Ekle";

        public ICommand SaveCommand { get; }

        public AddEditJobViewModel(IJobService jobService, ICustomerService customerService, Job job = null)
        {
            _jobService = jobService;
            _customerService = customerService;
            SaveCommand = new RelayCommand(async _ => await ExecuteSaveAsync());

            _ = LoadCustomersAsync(job);
        }

        private async Task LoadCustomersAsync(Job job)
        {
            var customers = await _customerService.GetActiveCustomersAsync();
            foreach (var c in customers)
            {
                Customers.Add(c);
                if (job != null && c.Id == job.CustomerId) SelectedCustomer = c;
            }

            if (job != null)
            {
                _jobId = job.Id;
                JobTitle = job.JobTitle;
                Description = job.Description;
                Status = job.Status;
                StartDate = job.StartDate;
                EndDate = job.EndDate;
            }
        }

        private async Task ExecuteSaveAsync()
        {
            var job = new Job
            {
                Id = _jobId,
                CustomerId = _customerId,
                JobTitle = JobTitle,
                Description = Description,
                Status = Status,
                StartDate = StartDate,
                EndDate = EndDate
            };

            var result = IsEditMode 
                ? await _jobService.UpdateJobAsync(job) 
                : (await _jobService.CreateJobAsync(job)).Success ? (true, "") : (false, "Hata oluştu.");

            if (result.Item1)
            {
                MaterialDesignThemes.Wpf.DialogHost.Close("RootDialog", true);
            }
            else
            {
                ErrorMessage = result.Item2;
            }
        }
    }
}