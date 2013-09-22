using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardBoard.Board.ViewModels;
using UpdateControls.XAML;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CardBoard.Board.Views
{
    public sealed partial class FullScreenBoardControl : UserControl
    {
        public FullScreenBoardControl()
        {
            this.InitializeComponent();
        }

        private void ToDo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Any())
            //{
            //    Doing.SelectedItem = null;
            //    Done.SelectedItem = null;
            //}
        }

        private void Doing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Any())
            //{
            //    ToDo.SelectedItem = null;
            //    Done.SelectedItem = null;
            //}
        }

        private void Done_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Any())
            //{
            //    ToDo.SelectedItem = null;
            //    Doing.SelectedItem = null;
            //}
        }

        private void CardList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var cardViewModel = ForView.Unwrap<CardViewModel>(e.Items.FirstOrDefault());
            if (cardViewModel != null)
            {
                var card = cardViewModel.Card;
                e.Data.SetUri(ProjectDetailViewModel.UriOfCard(card));
            }
        }

        private async void CardList_Drop(object sender, DragEventArgs e)
        {
            var viewModel = ForView.Unwrap<ProjectDetailViewModel>(DataContext);
            if (viewModel == null)
                return;

            var url = await e.Data.GetView().GetUriAsync();

            int columnIndex =
                sender == ToDo ? 0 :
                sender == Doing ? 1 :
                sender == Done ? 2 :
                    -1;
            if (columnIndex == -1)
                return;

            await viewModel.MoveCard(url, columnIndex);
        }
    }
}
