using System.Collections.Generic;
using System.Threading.Tasks;

namespace IsTakipWpf.Services
{
    public class BackupInfo
    {
        public string FilePath { get; set; }
        public System.DateTime BackupDate { get; set; }
    }

    public interface IBackupService
    {
        Task<(bool Success, string Message)> CreateBackupAsync(string targetPath);
        Task<(bool Success, string Message)> RestoreBackupAsync(string backupPath);
        Task<IEnumerable<BackupInfo>> GetBackupHistoryAsync();
    }
}
