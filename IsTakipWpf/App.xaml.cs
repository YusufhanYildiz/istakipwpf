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

            // Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IReportingService, ReportingService>();
            services.AddScoped<IAuthService, AuthService>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<CustomerListViewModel>();
            services.AddTransient<JobListViewModel>();
            services.AddTransient<SettingsViewModel>();

            // Views
            services.AddTransient<DashboardView>();
            services.AddTransient<CustomerListView>();
            services.AddTransient<JobListView>();
            services.AddTransient<SettingsView>();
        }
    }
}