using System;
using System.Collections.ObjectModel;
using System.Linq;
using CardBoard.Board.ViewModels;
using UpdateControls.XAML;
using UpdateControls.XAML.Wrapper;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using CardBoard.Model;

namespace CardBoard.Board.Views
{
    public sealed partial class FullScreenBoardControl : UserControl
    {
        public FullScreenBoardControl()
        {
            this.InitializeComponent();
        }

        private Card _draggingCard;

        private void CardList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var cardViewModel = ForView.Unwrap<CardViewModel>(e.Items.FirstOrDefault());
            if (cardViewModel != null)
            {
                var card = cardViewModel.Card;
                _draggingCard = card;
                e.Data.SetApplicationLink(ProjectDetailViewModel.UriOfCard(card));
            }
        }

        private async void CardList_Drop(object sender, DragEventArgs e)
        {
            var viewModel = ForView.Unwrap<ProjectDetailViewModel>(DataContext);
            if (viewModel == null)
                return;

            int columnIndex =
                sender == ToDo ? 0 :
                sender == Doing ? 1 :
                sender == Done ? 2 :
                    -1;
            if (columnIndex == -1)
                return;

            e.Handled = true;

            RemoveFromLists();

            var uri = await e.Data.GetView().GetApplicationLinkAsync();

            await viewModel.MoveCard(uri, columnIndex);
        }

        private void RemoveFromLists()
        {
            RemoveFromList(ToDo);
            RemoveFromList(Doing);
            RemoveFromList(Done);
        }

        private void RemoveFromList(ListView listView)
        {
            TransitionCollection saveTransitions = listView.ItemContainerTransitions;
            listView.ItemContainerTransitions = new TransitionCollection();

            var observableCollection = listView.ItemsSource as ObservableCollection<object>;
            if (observableCollection == null)
                return;

            var removed = observableCollection
                .Where(wrapper =>
                {
                    CardViewModel viewModel = ForView.Unwrap<CardViewModel>(wrapper);
                    if (viewModel == null)
                        return false;
                    return viewModel.Card == _draggingCard;
                })
                .ToList();
            foreach (var obj in removed)
                observableCollection.Remove(obj);

            listView.ItemContainerTransitions = saveTransitions;
        }
    }
}
