using System;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;
using System.Reflection;

namespace IsTakipWpf.Services
{
    public class UpdateService : IUpdateService
    {
        private const string UpdateUrl = "https://github.com/YusufhanYildiz/istakipwpf"; 

        public string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            try
            {
                var mgr = new UpdateManager(new GithubSource(UpdateUrl, null, false));
                return await mgr.CheckForUpdatesAsync();
            }
            catch { return null; }
        }

        public async Task DownloadUpdatesAsync(UpdateInfo updateInfo)
        {
            var mgr = new UpdateManager(new GithubSource(UpdateUrl, null, false));
            await mgr.DownloadUpdatesAsync(updateInfo);
        }

        public void ApplyUpdatesAndRestart(UpdateInfo updateInfo)
        {
            var mgr = new UpdateManager(new GithubSource(UpdateUrl, null, false));
            mgr.ApplyUpdatesAndRestart(updateInfo);
        }
    }
}
