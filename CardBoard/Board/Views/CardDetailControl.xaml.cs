using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CardBoard.Board.Views
{
    public sealed partial class CardDetailControl : UserControl
    {
        public event RoutedEventHandler Ok;
        public event RoutedEventHandler Cancel;

        public CardDetailControl()
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

        private void CardDetail_Loaded(object sender, RoutedEventArgs e)
        {
            CardTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            CardTextBox.SelectAll();
        }
    }
}
