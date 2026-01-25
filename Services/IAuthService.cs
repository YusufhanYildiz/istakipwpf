using System.Threading.Tasks;

namespace IsTakipWpf.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates the user with the provided username and password.
        /// </summary>
        Task<bool> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Changes the current admin username.
        /// </summary>
        Task<bool> ChangeUsernameAsync(string newUsername);

        /// <summary>
        /// Gets the current admin username.
        /// </summary>
        Task<string> GetUsernameAsync();

        /// <summary>
        /// Changes the current admin password.
        /// </summary>
        Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);

        /// <summary>
        /// Gets whether the "Remember Me" option is enabled.
        /// </summary>
        Task<bool> GetRememberMeAsync();

        /// <summary>
        /// Sets the "Remember Me" option.
        /// </summary>
        Task SetRememberMeAsync(bool enabled);

        /// <summary>
        /// Saves the encrypted credentials.
        /// </summary>
        Task SaveCredentialsAsync(string username, string password);

        /// <summary>
        /// Retrieves the saved credentials (username, password).
        /// Returns nulls if not found or decryption fails.
        /// </summary>
        Task<(string Username, string Password)> GetSavedCredentialsAsync();

        /// <summary>
        /// Clears any saved credentials.
        /// </summary>
        Task ClearSavedCredentialsAsync();
    }
}