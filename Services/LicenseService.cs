using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IsTakipWpf.Repositories;
using System.Globalization;

namespace IsTakipWpf.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly ISettingsRepository _settingsRepository;
        private const string LicenseKeySetting = "LicenseKey";
        private const string TrialStartSetting = "TrialStartDate";
        private const string LastKnownDateSetting = "LastKnownDate";
        private const string SecretSalt = "IsTakip_2026_Secure_Salt_!@#"; 

        public LicenseService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        private async Task<DateTime> GetBestAvailableDateAsync()
        {
            DateTime currentDate = DateTime.Now;
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    var response = await client.GetAsync("https://www.google.com", System.Net.Http.HttpCompletionOption.ResponseHeadersRead);
                    if (response.Headers.Date.HasValue)
                    {
                        currentDate = response.Headers.Date.Value.LocalDateTime;
                    }
                }
            }
            catch { }

            var lastKnownStr = await _settingsRepository.GetValueAsync(LastKnownDateSetting);
            if (long.TryParse(lastKnownStr, out long lastTicks))
            {
                DateTime lastKnownDate = new DateTime(lastTicks);
                if (currentDate < lastKnownDate) currentDate = lastKnownDate;
            }

            await _settingsRepository.SetValueAsync(LastKnownDateSetting, currentDate.Ticks.ToString());
            return currentDate;
        }

        public async Task<DateTime?> GetTrialStartDateAsync()
        {
            var dateStr = await _settingsRepository.GetValueAsync(TrialStartSetting);
            if (string.IsNullOrEmpty(dateStr))
            {
                var now = await GetBestAvailableDateAsync();
                await _settingsRepository.SetValueAsync(TrialStartSetting, now.Ticks.ToString());
                return now;
            }
            return long.TryParse(dateStr, out long ticks) ? new DateTime(ticks) : (DateTime?)null;
        }

        public async Task<int> GetRemainingTrialDaysAsync()
        {
            if (await IsLicensedAsync()) return 9999;
            var startDate = await GetTrialStartDateAsync();
            if (startDate == null) return 0;
            var currentDate = await GetBestAvailableDateAsync();
            int remaining = 30 - (int)(currentDate - startDate.Value).TotalDays;
            return Math.Max(0, remaining);
        }

        public async Task<bool> IsTrialActiveAsync()
        {
            if (await IsLicensedAsync()) return true;
            return await GetRemainingTrialDaysAsync() > 0;
        }

        public string GetHardwareId()
        {
            try
            {
                string cpuId = "", mbId = "";
                using (var cpuSearcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                    foreach (var obj in cpuSearcher.Get()) { cpuId = obj["ProcessorId"]?.ToString(); break; }
                using (var mbSearcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                    foreach (var obj in mbSearcher.Get()) { mbId = obj["SerialNumber"]?.ToString(); break; }
                return HashString($"{cpuId}-{mbId}").Substring(0, 8);
            }
            catch { return HashString(Environment.MachineName).Substring(0, 8); }
        }

        public bool ValidateLicense(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            try
            {
                var parts = key.Split('-');
                if (parts.Length != 3) return false;

                string hwidPart = parts[0];
                string datePart = parts[1];
                string signPart = parts[2];

                // 1. HWID Kontrolü
                if (!hwidPart.Equals(GetHardwareId(), StringComparison.OrdinalIgnoreCase)) return false;

                // 2. İmza Kontrolü (Tarih manipülasyonunu engeller)
                string expectedSign = HashString($"{hwidPart}-{datePart}-{SecretSalt}").Substring(0, 6);
                if (!signPart.Equals(expectedSign, StringComparison.OrdinalIgnoreCase)) return false;

                // 3. Süre Kontrolü
                if (DateTime.TryParseExact(datePart, "yyyyMMdd", null, DateTimeStyles.None, out DateTime expiryDate))
                {
                    // Şimdilik sadece formatı doğrula, süre dolumunu IsLicensedAsync içinde yapıyoruz
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        public async Task<bool> IsLicensedAsync()
        {
            var savedKey = await _settingsRepository.GetValueAsync(LicenseKeySetting);
            if (!ValidateLicense(savedKey)) return false;

            var parts = savedKey.Split('-');
            if (DateTime.TryParseExact(parts[1], "yyyyMMdd", null, DateTimeStyles.None, out DateTime expiryDate))
            {
                var currentDate = await GetBestAvailableDateAsync();
                return currentDate.Date <= expiryDate.Date;
            }
            return false;
        }

        public async Task<bool> ActivateLicenseAsync(string key)
        {
            if (ValidateLicense(key))
            {
                await _settingsRepository.SetValueAsync(LicenseKeySetting, key);
                return true;
            }
            return false;
        }

        private string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes) builder.Append(b.ToString("X2"));
                return builder.ToString();
            }
        }
    }
}
