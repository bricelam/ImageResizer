using System.Windows;
using ImageResizer.ViewModels;

namespace ImageResizer.Views
{
    public partial class AdvancedWindow : Window
    {
        public AdvancedWindow(AdvancedViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        void HandleAcceptClick(object sender, RoutedEventArgs e)
            => DialogResult = true;
    }
}
