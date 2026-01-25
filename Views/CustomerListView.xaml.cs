using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IsTakipWpf.ViewModels;

namespace IsTakipWpf.Views
{
    public partial class CustomerListView : UserControl
    {
        public CustomerListView(CustomerListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid == null) return;

            var hit = VisualTreeHelper.HitTest(grid, e.GetPosition(grid));
            if (hit == null) return;

            var row = FindVisualParent<DataGridRow>(hit.VisualHit);
            if (row != null && row.IsSelected)
            {
                grid.SelectedItem = null;
                e.Handled = true;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem != null)
            {
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.SelectedItem);
            }
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            return parent ?? FindVisualParent<T>(parentObject);
        }
    }
}