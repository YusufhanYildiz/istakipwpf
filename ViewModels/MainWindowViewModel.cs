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
            UserControl view = null;
            switch (target)
            {
                case "Dashboard":
                    view = _serviceProvider.GetRequiredService<DashboardView>();
                    break;
                case "Customers":
                    view = _serviceProvider.GetRequiredService<CustomerListView>();
                    break;
                case "Jobs":
                    view = _serviceProvider.GetRequiredService<JobListView>();
                    break;
                case "Settings":
                    view = _serviceProvider.GetRequiredService<SettingsView>();
                    break;
                case "Security":
                    view = _serviceProvider.GetRequiredService<PasswordChangeView>();
                    break;
            }

            if (view != null)
            {
                CurrentView = view;
                if (view.DataContext is IRefreshable refreshable)
                {
                    _ = refreshable.RefreshAsync();
                }
            }
        }
    }
}
