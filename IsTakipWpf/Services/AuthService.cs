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
        private const string SaltKey = "AdminPasswordSalt";
        private const string RememberMeKey = "RememberMe";

        public AuthService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<bool> AuthenticateAsync(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            var storedHash = await _settingsRepository.GetValueAsync(PasswordKey);
            var storedSalt = await _settingsRepository.GetValueAsync(SaltKey);
            
            // Initial migration from plain text to hash+salt
            if (storedHash == "admin")
            {
                if (password == "admin")
                {
                    var newSalt = Guid.NewGuid().ToString("N");
                    var newHash = HashPassword("admin", newSalt);
                    await _settingsRepository.SetValueAsync(PasswordKey, newHash);
                    await _settingsRepository.SetValueAsync(SaltKey, newSalt);
                    return true;
                }
                return false;
            }

            if (string.IsNullOrEmpty(storedSalt)) return false;

            var inputHash = HashPassword(password, storedSalt);
            return storedHash == inputHash;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            if (await AuthenticateAsync(currentPassword))
            {
                var newSalt = Guid.NewGuid().ToString("N");
                var newHash = HashPassword(newPassword, newSalt);
                
                var r1 = await _settingsRepository.SetValueAsync(PasswordKey, newHash);
                var r2 = await _settingsRepository.SetValueAsync(SaltKey, newSalt);
                
                return r1 && r2;
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

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = password + salt;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
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