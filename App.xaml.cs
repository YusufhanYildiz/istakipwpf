using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using IsTakipWpf.Infrastructure;
using IsTakipWpf.Repositories;
using IsTakipWpf.Services;
using IsTakipWpf.ViewModels;
using IsTakipWpf.Views;

namespace IsTakipWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the application's service provider.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Set global culture to Turkish for proper Currency and Date formatting
            var culture = new System.Globalization.CultureInfo("tr-TR");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(culture.IetfLanguageTag)));

            base.OnStartup(e);

            // Prevent app from closing when LoginWindow (the only open window) closes
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            try
            {
                // Initialize Database
                DatabaseBootstrap.Initialize();

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                // Apply Theme
                var themeService = ServiceProvider.GetRequiredService<IThemeService>();
                themeService.LoadThemeAsync().GetAwaiter().GetResult();

                // Show Login Window First
                var loginViewModel = ServiceProvider.GetRequiredService<LoginViewModel>();
                var loginWindow = new LoginWindow(loginViewModel);
                
                if (loginWindow.ShowDialog() == true)
                {
                    var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                    MainWindow = mainWindow;
                    mainWindow.Show();
                    ShutdownMode = ShutdownMode.OnMainWindowClose;
                }
                else
                {
                    Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Uygulama başlatılırken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // UI
            services.AddSingleton<MaterialDesignThemes.Wpf.ISnackbarMessageQueue>(new MaterialDesignThemes.Wpf.SnackbarMessageQueue());

            // Windows
            services.AddSingleton<MainWindow>();
            services.AddTransient<LoginWindow>();

            // Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();

            // Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IReportingService, ReportingService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<ILocationService, LocationService>();
            services.AddSingleton<IUpdateService, UpdateService>();
            services.AddSingleton<ILicenseService, LicenseService>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<CustomerListViewModel>();
            services.AddSingleton<JobListViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<NotesViewModel>();

            // Views
            services.AddSingleton<DashboardView>();
            services.AddSingleton<CustomerListView>();
            services.AddSingleton<JobListView>();
            services.AddSingleton<SettingsView>();
            services.AddSingleton<NotesView>();
        }
    }
}