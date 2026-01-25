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
        private readonly IUpdateService _updateService;
        private readonly ILicenseService _licenseService;
        private readonly MaterialDesignThemes.Wpf.ISnackbarMessageQueue _messageQueue;
        
        private string _backupFolder;
        private bool _autoBackupOnExit;
        private AppTheme _selectedTheme;
        
        private string _updateStatus = "Güncel";
        private bool _isUpdateChecking;
        private bool _isUpdateDownloading;
        private bool _isUpdateDownloaded;
        private Velopack.UpdateInfo _availableUpdate;

        private string _licenseKey;
        private string _hardwareId;
        private string _licenseStatus;
        private int _remainingTrialDays;

        public string CurrentVersion => _updateService.CurrentVersion;

        public string UpdateStatus
        {
            get => _updateStatus;
            set => SetProperty(ref _updateStatus, value);
        }

        public bool IsUpdateChecking
        {
            get => _isUpdateChecking;
            set => SetProperty(ref _isUpdateChecking, value);
        }

        public bool IsUpdateDownloading
        {
            get => _isUpdateDownloading;
            set => SetProperty(ref _isUpdateDownloading, value);
        }

        public bool IsUpdateDownloaded
        {
            get => _isUpdateDownloaded;
            set
            {
                if (SetProperty(ref _isUpdateDownloaded, value))
                {
                    OnPropertyChanged(nameof(CanDownloadUpdate));
                    OnPropertyChanged(nameof(CanRestartToUpdate));
                }
            }
        }

        public bool CanDownloadUpdate => _availableUpdate != null && !IsUpdateDownloading && !IsUpdateDownloaded;
        public bool CanRestartToUpdate => IsUpdateDownloaded;

        public string LicenseKey
        {
            get => _licenseKey;
            set => SetProperty(ref _licenseKey, value);
        }

        public string HardwareId
        {
            get => _hardwareId;
            set => SetProperty(ref _hardwareId, value);
        }

        public string LicenseStatus
        {
            get => _licenseStatus;
            set => SetProperty(ref _licenseStatus, value);
        }

        public int RemainingTrialDays
        {
            get => _remainingTrialDays;
            set => SetProperty(ref _remainingTrialDays, value);
        }

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
        public ICommand CheckForUpdatesCommand { get; }
        public ICommand DownloadUpdateCommand { get; }
        public ICommand ApplyUpdateCommand { get; }
        public ICommand ActivateLicenseCommand { get; }

        public SettingsViewModel(IBackupService backupService, ISettingsRepository settingsRepository, IThemeService themeService, IUpdateService updateService, ILicenseService licenseService, MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _backupService = backupService;
            _settingsRepository = settingsRepository;
            _themeService = themeService;
            _updateService = updateService;
            _licenseService = licenseService;
            _messageQueue = messageQueue;

            SelectFolderCommand = new RelayCommand(_ => SelectFolder());
            CreateManualBackupCommand = new RelayCommand(async _ => await CreateBackupAsync());
            RestoreBackupCommand = new RelayCommand(async backup => await RestoreAsync(backup as BackupInfo));
            
            CheckForUpdatesCommand = new RelayCommand(async _ => await CheckForUpdatesAsync());
            DownloadUpdateCommand = new RelayCommand(async _ => await DownloadUpdateAsync());
            ApplyUpdateCommand = new RelayCommand(_ => ApplyUpdate());
            ActivateLicenseCommand = new RelayCommand(async _ => await ActivateLicenseAsync());

            _ = InitializeAsync();
        }

        private async Task DownloadUpdateAsync()
        {
            if (_availableUpdate == null) return;
            
            IsUpdateDownloading = true;
            UpdateStatus = "Güncelleme indiriliyor...";
            
            try
            {
                await _updateService.DownloadUpdatesAsync(_availableUpdate);
                IsUpdateDownloaded = true;
                UpdateStatus = "İndirme tamamlandı. Yeniden başlatmaya hazır.";
            }
            catch (Exception ex)
            {
                UpdateStatus = "İndirme hatası.";
                _messageQueue.Enqueue($"İndirme hatası: {ex.Message}");
            }
            finally
            {
                IsUpdateDownloading = false;
            }
        }

        private void ApplyUpdate()
        {
            if (_availableUpdate == null) return;
            _updateService.ApplyUpdatesAndRestart(_availableUpdate);
        }

        private async Task ActivateLicenseAsync()
        {
            if (string.IsNullOrWhiteSpace(LicenseKey))
            {
                _messageQueue.Enqueue("Lütfen bir lisans anahtarı giriniz.");
                return;
            }

            var success = await _licenseService.ActivateLicenseAsync(LicenseKey);
            if (success)
            {
                _messageQueue.Enqueue("Lisans başarıyla aktifleştirildi!");
                await UpdateLicenseInfoAsync();
            }
            else
            {
                _messageQueue.Enqueue("Geçersiz lisans anahtarı. Lütfen HWID kodunuzu kontrol edin.");
            }
        }

        private async Task UpdateLicenseInfoAsync()
        {
            HardwareId = _licenseService.GetHardwareId();
            var isLicensed = await _licenseService.IsLicensedAsync();
            
            if (isLicensed)
            {
                var savedKey = await _settingsRepository.GetValueAsync("LicenseKey");
                var parts = savedKey.Split('-');
                if (parts.Length > 1 && DateTime.TryParseExact(parts[1], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime expiryDate))
                {
                    LicenseStatus = $"Full Sürüm (Bitiş: {expiryDate.ToShortDateString()})";
                }
                else
                {
                    LicenseStatus = "Full Sürüm Aktif";
                }
                RemainingTrialDays = 9999;
            }
            else
            {
                RemainingTrialDays = await _licenseService.GetRemainingTrialDaysAsync();
                LicenseStatus = RemainingTrialDays > 0 ? $"Deneme Sürümü ({RemainingTrialDays} gün kaldı)" : "Deneme Süresi Doldu";
            }
        }

        private async Task CheckForUpdatesAsync()
        {
            if (IsUpdateChecking) return;
            
            IsUpdateChecking = true;
            UpdateStatus = "Güncellemeler denetleniyor...";
            
            try
            {
                _availableUpdate = await Task.Run(async () => await _updateService.CheckForUpdatesAsync());
                
                if (_availableUpdate != null)
                {
                    UpdateStatus = $"Yeni sürüm mevcut: {_availableUpdate.TargetFullRelease.Version}";
                    OnPropertyChanged(nameof(CanDownloadUpdate));
                }
                else
                {
                    UpdateStatus = "Uygulama güncel.";
                }
            }
            catch (Exception ex)
            {
                UpdateStatus = "Hata oluştu.";
                _messageQueue.Enqueue($"Güncelleme hatası: {ex.Message}");
            }
            finally
            {
                IsUpdateChecking = false;
            }
        }

        private async Task InitializeAsync()
        {
            // Load Theme
            SelectedTheme = _themeService.GetCurrentTheme();

            var savedFolder = await _settingsRepository.GetValueAsync("BackupFolder");
            if (string.IsNullOrEmpty(savedFolder))
            {
                // DatabaseBootstrap içindeki ana klasörü baz alarak bir Backups alt klasörü oluşturuyoruz
                string baseFolder = System.IO.Path.GetDirectoryName(Infrastructure.DatabaseBootstrap.DbPath);
                savedFolder = System.IO.Path.Combine(baseFolder, "Yedekler");
                
                if (!System.IO.Directory.Exists(savedFolder))
                {
                    System.IO.Directory.CreateDirectory(savedFolder);
                }
            }
            BackupFolder = savedFolder;

            var autoBackupStr = await _settingsRepository.GetValueAsync("AutoBackupOnExit");
            AutoBackupOnExit = autoBackupStr == "True";

            await LoadHistoryAsync();
            await UpdateLicenseInfoAsync();
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