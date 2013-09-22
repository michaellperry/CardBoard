using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CardBoard.Projects.Views
{
    public sealed partial class ProjectDetailControl : UserControl
    {
        public event RoutedEventHandler Ok;
        public event RoutedEventHandler Cancel;

        public ProjectDetailControl()
        {
            this.InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Ok != null)
                Ok(sender, e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel != null)
                Cancel(sender, e);
        }

        private void ProjectDetail_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            NameTextBox.SelectAll();
        }
    }
}
