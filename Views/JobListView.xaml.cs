using System.Windows.Controls;
using IsTakipWpf.ViewModels;

namespace IsTakipWpf.Views
{
    public partial class JobListView : UserControl
    {
        public JobListView(JobListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}