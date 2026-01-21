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

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            }
        }
    }
}
