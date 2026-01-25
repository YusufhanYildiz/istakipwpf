using System.Windows.Controls;
using IsTakipWpf.ViewModels;

namespace IsTakipWpf.Views
{
    public partial class NotesView : UserControl
    {
        public NotesView(NotesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
