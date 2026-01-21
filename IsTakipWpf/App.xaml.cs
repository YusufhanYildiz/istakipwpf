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

            try
            {
                // Initialize Database
                DatabaseBootstrap.Initialize();

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Uygulama baÅŸlatÄ±lÄ±rken bir hata oluÅŸtu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Main Window
            services.AddSingleton<MainWindow>();

            // Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<IJobRepository, JobRepository>();

            // Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IJobService, JobService>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<CustomerListViewModel>();

            // Views
            services.AddTransient<DashboardView>();
            services.AddTransient<CustomerListView>();
        }
    }
}