using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using IsTakipWpf.Services;
using IsTakipWpf.Repositories;
using Microsoft.Win32;

namespace IsTakipWpf.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IBackupService _backupService;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IThemeService _themeService;
        private readonly MaterialDesignThemes.Wpf.ISnackbarMessageQueue _messageQueue;
        private string _backupFolder;
        private bool _autoBackupOnExit;
        private AppTheme _selectedTheme;

        public string BackupFolder
        {
            get => _backupFolder;
            set { if (SetProperty(ref _backupFolder, value)) _ = SaveSettingsAsync(); }
        }

        public bool AutoBackupOnExit
        {
            get => _autoBackupOnExit;
            set { if (SetProperty(ref _autoBackupOnExit, value)) _ = SaveSettingsAsync(); }
        }

        public AppTheme SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value))
                {
                    _themeService.SetTheme(value);
                }
            }
        }

        public System.Collections.Generic.IEnumerable<AppTheme> Themes => (AppTheme[])Enum.GetValues(typeof(AppTheme));

        public ObservableCollection<BackupInfo> BackupHistory { get; } = new ObservableCollection<BackupInfo>();

        public ICommand SelectFolderCommand { get; }
        public ICommand CreateManualBackupCommand { get; }
        public ICommand RestoreBackupCommand { get; }

        public SettingsViewModel(IBackupService backupService, ISettingsRepository settingsRepository, IThemeService themeService, MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _backupService = backupService;
            _settingsRepository = settingsRepository;
            _themeService = themeService;
            _messageQueue = messageQueue;

            SelectFolderCommand = new RelayCommand(_ => SelectFolder());
            CreateManualBackupCommand = new RelayCommand(async _ => await CreateBackupAsync());
            RestoreBackupCommand = new RelayCommand(async backup => await RestoreAsync(backup as BackupInfo));

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // Load Theme
            SelectedTheme = _themeService.GetCurrentTheme();

            var savedFolder = await _settingsRepository.GetValueAsync("BackupFolder");
            if (string.IsNullOrEmpty(savedFolder))
            {
                savedFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup");
                if (!System.IO.Directory.Exists(savedFolder))
                {
                    System.IO.Directory.CreateDirectory(savedFolder);
                }
            }
            BackupFolder = savedFolder;

            var autoBackupStr = await _settingsRepository.GetValueAsync("AutoBackupOnExit");
            AutoBackupOnExit = autoBackupStr == "True";

            await LoadHistoryAsync();
        }

        private async Task LoadHistoryAsync()
        {
            var history = await _backupService.GetBackupHistoryAsync();
            BackupHistory.Clear();
            foreach (var h in history) BackupHistory.Add(h);
        }

        private void SelectFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BackupFolder = dialog.SelectedPath;
            }
        }

        private async Task CreateBackupAsync()
        {
            var result = await _backupService.CreateBackupAsync(BackupFolder);
            _messageQueue.Enqueue(result.Message);
            if (result.Success) await LoadHistoryAsync();
        }

        private async Task RestoreAsync(BackupInfo backup)
        {
            if (backup == null) return;
            var result = await _backupService.RestoreBackupAsync(backup.FilePath);
            _messageQueue.Enqueue(result.Message);
        }

        private async Task SaveSettingsAsync()
        {
            await _settingsRepository.SetValueAsync("BackupFolder", BackupFolder);
            await _settingsRepository.SetValueAsync("AutoBackupOnExit", AutoBackupOnExit.ToString());
        }
    }
}