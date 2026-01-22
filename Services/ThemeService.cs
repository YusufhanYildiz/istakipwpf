using System;
using System.Threading.Tasks;
using IsTakipWpf.Repositories;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace IsTakipWpf.Services
{
    public class ThemeService : IThemeService
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private AppTheme _currentTheme = AppTheme.System;
        private const string ThemeKey = "AppTheme";

        public ThemeService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public AppTheme GetCurrentTheme() => _currentTheme;

        public async Task LoadThemeAsync()
        {
            var storedValue = await _settingsRepository.GetValueAsync(ThemeKey);
            if (Enum.TryParse(storedValue, out AppTheme theme))
            {
                _currentTheme = theme;
            }
            else
            {
                _currentTheme = AppTheme.System;
            }
            ApplyTheme(_currentTheme);
        }

        public async void SetTheme(AppTheme theme)
        {
            _currentTheme = theme;
            ApplyTheme(theme);
            await _settingsRepository.SetValueAsync(ThemeKey, theme.ToString());
        }

        private void ApplyTheme(AppTheme theme)
        {
            var currentPalette = _paletteHelper.GetTheme();
            bool isDark = false;

            if (theme == AppTheme.System)
            {
                isDark = IsSystemInDarkMode();
            }
            else
            {
                isDark = theme == AppTheme.Dark;
            }

            currentPalette.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
            _paletteHelper.SetTheme(currentPalette);
        }

        private bool IsSystemInDarkMode()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    object registryValueObject = key?.GetValue("AppsUseLightTheme");
                    if (registryValueObject == null) return false;
                    
                    int registryValue = (int)registryValueObject;
                    return registryValue == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
