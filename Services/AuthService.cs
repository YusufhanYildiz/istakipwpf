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
        private const string UsernameKey = "AdminUsername";
        private const string RememberMeKey = "RememberMe";

        public AuthService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            // Check username first
            var storedUsername = await GetUsernameAsync();
            if (!string.Equals(storedUsername, username, StringComparison.OrdinalIgnoreCase))
            {
                // Fallback for initial migration if no username stored yet but default is admin
                if (storedUsername == "admin" && username.ToLower() == "admin")
                {
                    // Proceed to password check
                }
                else
                {
                    return false;
                }
            }

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
                    await _settingsRepository.SetValueAsync(UsernameKey, "admin");
                    return true;
                }
                return false;
            }

            if (string.IsNullOrEmpty(storedSalt)) return false;

            var inputHash = HashPassword(password, storedSalt);
            return storedHash == inputHash;
        }

        public async Task<string> GetUsernameAsync()
        {
            var val = await _settingsRepository.GetValueAsync(UsernameKey);
            return string.IsNullOrEmpty(val) ? "admin" : val;
        }

        public async Task<bool> ChangeUsernameAsync(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername)) return false;
            return await _settingsRepository.SetValueAsync(UsernameKey, newUsername.Trim());
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            // We need the current username to authenticate. 
            // Since this method is usually called when logged in, we can get it from storage.
            var currentUsername = await GetUsernameAsync();
            if (await AuthenticateAsync(currentUsername, currentPassword))
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
            if (!enabled)
            {
                await ClearSavedCredentialsAsync();
            }
        }

        public async Task SaveCredentialsAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;

            await _settingsRepository.SetValueAsync("SavedUsername", username);

            try
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var encryptedBytes = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);
                var encryptedBase64 = Convert.ToBase64String(encryptedBytes);
                await _settingsRepository.SetValueAsync("SavedPassword", encryptedBase64);
            }
            catch
            {
                // Handle encryption error or log it
            }
        }

        public async Task<(string Username, string Password)> GetSavedCredentialsAsync()
        {
            var username = await _settingsRepository.GetValueAsync("SavedUsername");
            var encryptedPassword = await _settingsRepository.GetValueAsync("SavedPassword");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(encryptedPassword))
            {
                return (null, null);
            }

            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedPassword);
                var passwordBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
                var password = Encoding.UTF8.GetString(passwordBytes);
                return (username, password);
            }
            catch
            {
                return (username, null);
            }
        }

        public async Task ClearSavedCredentialsAsync()
        {
            await _settingsRepository.SetValueAsync("SavedUsername", null);
            await _settingsRepository.SetValueAsync("SavedPassword", null);
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