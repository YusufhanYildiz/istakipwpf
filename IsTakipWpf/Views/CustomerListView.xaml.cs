using System.Windows.Controls;
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
    }
}