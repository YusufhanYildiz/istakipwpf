using System;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;
using System.Reflection;

namespace IsTakipWpf.Services
{
    public class UpdateService : IUpdateService
    {
        // TODO: Burayı GitHub Repo URL'niz ile değiştirin
        private const string UpdateUrl = "https://github.com/YusufhanYildiz/IsTakipUygulamasi"; 

        public string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public async Task<UpdateInfo> CheckForUpdatesAsync()
        {
            try
            {
                // Velopack henüz konfigüre edilmemişse veya dev modundaysa hata verebilir.
                // Bu yüzden try-catch bloğunda tutuyoruz.
                var mgr = new UpdateManager(new GithubSource(UpdateUrl, null, false));
                return await mgr.CheckForUpdatesAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task DownloadUpdatesAsync(UpdateInfo updateInfo)
        {
            if (updateInfo == null) return;
            var mgr = new UpdateManager(new GithubSource(UpdateUrl, null, false));
            await mgr.DownloadUpdatesAsync(updateInfo);
        }

        public void ApplyUpdatesAndRestart(UpdateInfo updateInfo)
        {
            if (updateInfo == null) return;
            var mgr = new UpdateManager(new GithubSource(UpdateUrl, null, false));
            mgr.ApplyUpdatesAndRestart(updateInfo);
        }
    }
}
