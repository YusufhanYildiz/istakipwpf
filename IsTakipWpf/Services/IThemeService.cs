using System.ComponentModel;
using System.Threading.Tasks;

namespace IsTakipWpf.Services
{
    public enum AppTheme
    {
        [Description("Açık")]
        Light,
        [Description("Koyu")]
        Dark,
        [Description("Sistem Varsayılanı")]
        System
    }

    public interface IThemeService
    {
        /// <summary>
        /// Sets the application theme.
        /// </summary>
        void SetTheme(AppTheme theme);

        /// <summary>
        /// Loads the saved theme from settings.
        /// </summary>
        Task LoadThemeAsync();

        /// <summary>
        /// Gets the current active theme setting.
        /// </summary>
        AppTheme GetCurrentTheme();
    }
}
