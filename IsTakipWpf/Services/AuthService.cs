using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IsTakipWpf.Repositories;

namespace IsTakipWpf.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISettingsRepository _settingsRepository;
        private const string PasswordKey = "AdminPasswordHash";
        private const string RememberMeKey = "RememberMe";

        public AuthService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<bool> AuthenticateAsync(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            var storedHash = await _settingsRepository.GetValueAsync(PasswordKey);
            
            // Initial migration from plain text to hash
            if (storedHash == "admin")
            {
                if (password == "admin")
                {
                    var newHash = HashPassword("admin");
                    await _settingsRepository.SetValueAsync(PasswordKey, newHash);
                    return true;
                }
                return false;
            }

            var inputHash = HashPassword(password);
            return storedHash == inputHash;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            if (await AuthenticateAsync(currentPassword))
            {
                var newHash = HashPassword(newPassword);
                return await _settingsRepository.SetValueAsync(PasswordKey, newHash);
            }
            return false;
        }

        public async Task<bool> GetRememberMeAsync()
        {
            var val = await _settingsRepository.GetValueAsync(RememberMeKey);
            return val == "True";
        }

        public async Task SetRememberMeAsync(bool enabled)
        {
            await _settingsRepository.SetValueAsync(RememberMeKey, enabled.ToString());
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}