using System;
using System.Windows.Controls;
using IsTakipWpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace IsTakipWpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MaterialDesignThemes.Wpf.ISnackbarMessageQueue MessageQueue { get; }

        public MainWindowViewModel(IServiceProvider serviceProvider, MaterialDesignThemes.Wpf.ISnackbarMessageQueue messageQueue)
        {
            _serviceProvider = serviceProvider;
            MessageQueue = messageQueue;
            // Initial view
            CurrentView = _serviceProvider.GetRequiredService<DashboardView>();
        }

        public void Navigate(string target)
        {
            switch (target)
            {
                case "Dashboard":
                    CurrentView = _serviceProvider.GetRequiredService<DashboardView>();
                    break;
                case "Customers":
                    CurrentView = _serviceProvider.GetRequiredService<CustomerListView>();
                    break;
                case "Jobs":
                    CurrentView = _serviceProvider.GetRequiredService<JobListView>();
                    break;
                case "Settings":
                    CurrentView = _serviceProvider.GetRequiredService<SettingsView>();
                    break;
            }
        }
    }
}
