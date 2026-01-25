using System.Threading.Tasks;
using IsTakipWpf.Services;
using IsTakipWpf.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;

namespace IsTakipWpf.ViewModels
{
    public class DashboardViewModel : ViewModelBase, IRefreshable
    {
        private readonly ICustomerService _customerService;
        private readonly IJobService _jobService;
        private readonly INoteService _noteService;
        private int _totalCustomers;
        private int _activeJobs;
        private int _waitingJobs;
        private decimal _totalBalance;
        private string _quickNoteTitle;
        private string _quickNoteContent;
        private ObservableCollection<Note> _latestNotes;

        public int TotalCustomers { get => _totalCustomers; set => SetProperty(ref _totalCustomers, value); }
        public int ActiveJobs { get => _activeJobs; set => SetProperty(ref _activeJobs, value); }
        public int WaitingJobs { get => _waitingJobs; set => SetProperty(ref _waitingJobs, value); }
        public decimal TotalBalance { get => _totalBalance; set => SetProperty(ref _totalBalance, value); }
        public string QuickNoteTitle { get => _quickNoteTitle; set => SetProperty(ref _quickNoteTitle, value); }
        public string QuickNoteContent { get => _quickNoteContent; set => SetProperty(ref _quickNoteContent, value); }
        public ObservableCollection<Note> LatestNotes { get => _latestNotes; set => SetProperty(ref _latestNotes, value); }

        public ICommand AddQuickNoteCommand { get; }

        public DashboardViewModel(ICustomerService customerService, IJobService jobService, INoteService noteService)
        {
            _customerService = customerService;
            _jobService = jobService;
            _noteService = noteService;
            LatestNotes = new ObservableCollection<Note>();
            AddQuickNoteCommand = new RelayCommand(async _ => await AddQuickNoteAsync(), _ => !string.IsNullOrWhiteSpace(QuickNoteTitle));
            _ = RefreshAsync();
        }

        private async Task AddQuickNoteAsync()
        {
            var note = new Note
            {
                Title = QuickNoteTitle,
                Content = QuickNoteContent,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Color = "#FFFFFF"
            };

            await _noteService.AddNoteAsync(note);
            QuickNoteTitle = string.Empty;
            QuickNoteContent = string.Empty;
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            var customers = await _customerService.GetActiveCustomersAsync();
            TotalCustomers = customers.Count();

            var jobs = (await _jobService.GetAllJobsAsync()).ToList();
            ActiveJobs = jobs.Count(j => j.Status == JobStatus.DevamEdiyor);
            WaitingJobs = jobs.Count(j => j.Status == JobStatus.Bekliyor);
            TotalBalance = jobs.Sum(j => j.Balance);

            var notes = await _noteService.GetAllNotesAsync();
            LatestNotes = new ObservableCollection<Note>(notes.OrderByDescending(n => n.CreatedDate).Take(5));
        }
    }
}