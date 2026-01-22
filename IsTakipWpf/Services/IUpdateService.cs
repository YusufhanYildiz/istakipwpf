using System.Threading.Tasks;
using Velopack;

namespace IsTakipWpf.Services
{
    public interface IUpdateService
    {
        string CurrentVersion { get; }
        Task<UpdateInfo> CheckForUpdatesAsync();
        Task DownloadUpdatesAsync(UpdateInfo updateInfo);
        void ApplyUpdatesAndRestart(UpdateInfo updateInfo);
    }
}
