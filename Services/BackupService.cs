using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using IsTakipWpf.Infrastructure;
using IsTakipWpf.Repositories;

namespace IsTakipWpf.Services
{
    public class BackupService : IBackupService
    {
        private readonly ISettingsRepository _settingsRepository;
        private const string BackupHistoryKey = "BackupHistory";

        public BackupService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<(bool Success, string Message)> CreateBackupAsync(string targetFolder)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    if (!Directory.Exists(targetFolder))
                    {
                        Directory.CreateDirectory(targetFolder);
                    }

                    string fileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    string targetPath = Path.Combine(targetFolder, fileName);

                    // Ensure data is flushed to disk
                    File.Copy(DatabaseBootstrap.DbPath, targetPath, true);

                    await UpdateBackupHistoryAsync(targetPath);

                    return (true, "Yedekleme başarıyla tamamlandı.");
                }
                catch (Exception ex)
                {
                    return (false, $"Yedekleme sırasında hata oluştu: {ex.Message}");
                }
            });
        }

        public async Task<(bool Success, string Message)> RestoreBackupAsync(string backupPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!File.Exists(backupPath))
                    {
                        return (false, "Yedek dosyası bulunamadı.");
                    }

                    // SQLite bağlantısı dosyayı kilitleyebilir. 
                    // Bu yüzden dosyayı .restore uzantısıyla yanına kopyalıyoruz.
                    // DatabaseBootstrap.Initialize() tarafında bu dosya varsa asıl dosyanın üzerine yazılacak.
                    string restorePath = DatabaseBootstrap.DbPath + ".restore";
                    File.Copy(backupPath, restorePath, true);

                    return (true, "Geri yükleme hazırlandı. Değişikliklerin uygulanması için lütfen uygulamayı kapatıp yeniden açın.");
                }
                catch (Exception ex)
                {
                    return (false, $"Geri yükleme sırasında hata oluştu: {ex.Message}");
                }
            });
        }

        public async Task<IEnumerable<BackupInfo>> GetBackupHistoryAsync()
        {
            var json = await _settingsRepository.GetValueAsync(BackupHistoryKey);
            if (string.IsNullOrEmpty(json))
            {
                return Enumerable.Empty<BackupInfo>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<BackupInfo>>(json);
            }
            catch
            {
                return Enumerable.Empty<BackupInfo>();
            }
        }

        private async Task UpdateBackupHistoryAsync(string newBackupPath)
        {
            var history = (await GetBackupHistoryAsync()).ToList();
            history.Add(new BackupInfo 
            { 
                FilePath = newBackupPath, 
                BackupDate = DateTime.Now 
            });

            // Keep only last 10 backups in history
            if (history.Count > 10)
            {
                history = history.OrderByDescending(x => x.BackupDate).Take(10).ToList();
            }

            var json = JsonSerializer.Serialize(history);
            await _settingsRepository.SetValueAsync(BackupHistoryKey, json);
        }
    }
}