using System.Threading.Tasks;
using IsTakipWpf.Services;
using IsTakipWpf.Models;
using System.Linq;

namespace IsTakipWpf.ViewModels
{
        public class DashboardViewModel : ViewModelBase, IRefreshable
        {
            private readonly ICustomerService _customerService;
            private readonly IJobService _jobService;
            private int _totalCustomers;
            private int _activeJobs;
            private int _waitingJobs;
    
            public int TotalCustomers { get => _totalCustomers; set => SetProperty(ref _totalCustomers, value); }
            public int ActiveJobs { get => _activeJobs; set => SetProperty(ref _activeJobs, value); }
            public int WaitingJobs { get => _waitingJobs; set => SetProperty(ref _waitingJobs, value); }
    
            public DashboardViewModel(ICustomerService customerService, IJobService jobService)
            {
                _customerService = customerService;
                _jobService = jobService;
                _ = RefreshAsync();
            }
    
            public async Task RefreshAsync()
            {
                var customers = await _customerService.GetActiveCustomersAsync();
                TotalCustomers = customers.Count();
    
                var jobs = await _jobService.GetAllJobsAsync();
                ActiveJobs = jobs.Count(j => j.Status == JobStatus.DevamEdiyor);
                WaitingJobs = jobs.Count(j => j.Status == JobStatus.Bekliyor);
            }
        }
    }
    