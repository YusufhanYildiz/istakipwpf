using System.Windows;
using System.Windows.Controls;
using IsTakipWpf.ViewModels;

namespace IsTakipWpf
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<IsTakipWpf.Repositories.ISettingsRepository>(App.ServiceProvider);
            var autoBackup = await settings.GetValueAsync("AutoBackupOnExit");

            if (autoBackup == "True")
            {
                var result = MessageBox.Show("Uygulamadan çıkmadan önce yedekleme yapmak istiyor musunuz?", "Yedekleme", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    var backupService = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<IsTakipWpf.Services.IBackupService>(App.ServiceProvider);
                    
                    // DatabaseBootstrap içindeki ana klasörü baz alarak varsayılan yedekleme yolunu bul
                    string baseFolder = System.IO.Path.GetDirectoryName(Infrastructure.DatabaseBootstrap.DbPath);
                    string defaultBackupFolder = System.IO.Path.Combine(baseFolder, "Yedekler");

                    var folder = await settings.GetValueAsync("BackupFolder") ?? defaultBackupFolder;
                    await backupService.CreateBackupAsync(folder);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void RootNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is ListBoxItem selectedItem)
            {
                _viewModel.Navigate(selectedItem.Tag?.ToString());
                if (MenuToggleButton != null)
                {
                    MenuToggleButton.IsChecked = false;
                }
            }
        }

        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
