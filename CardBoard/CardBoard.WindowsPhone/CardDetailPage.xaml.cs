﻿using CardBoard.Board.ViewModels;
using UpdateControls.XAML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CardBoard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CardDetailPage : Page
    {
        public CardDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = ForView.Unwrap<CardDetailViewModel>(DataContext);
            if (viewModel != null)
            {
                viewModel.Ok();
                Frame.GoBack();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CardText.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            CardText.SelectAll();
        }

        private void Move_Click(object sender, RoutedEventArgs e)
        {
            ColumnSelector.IsDropDownOpen = true;
        }
    }
}
