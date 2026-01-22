using System.Threading.Tasks;

namespace IsTakipWpf.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates the user with the provided password.
        /// </summary>
        Task<bool> AuthenticateAsync(string password);

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
    }
}
