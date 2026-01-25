using System;
using System.Threading.Tasks;

namespace IsTakipWpf.Services
{
    public interface ILicenseService
    {
        string GetHardwareId();
        bool ValidateLicense(string key);
        Task<bool> IsLicensedAsync();
        Task<bool> ActivateLicenseAsync(string key);
        Task<bool> IsTrialActiveAsync();
        Task<int> GetRemainingTrialDaysAsync();
        Task<DateTime?> GetTrialStartDateAsync();
    }
}