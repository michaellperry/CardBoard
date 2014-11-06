using CardBoard.Board.ViewModels;
using CardBoard.Projects.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UpdateControls.XAML;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CardBoard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WithViewModel(vm =>
            {
                vm.ClearSelection();
            });
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            WithViewModel(vm =>
            {
                vm.PrepareNewCard();
                Frame.Navigate(typeof(CardDetailPage));
            });
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WithViewModel(vm =>
            {
                vm.CardSelected += CardSelected;
            });
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            WithViewModel(vm =>
            {
                vm.CardSelected -= CardSelected;
            });
        }

        private void CardSelected(CardBoard.Model.Card card)
        {
            if (card != null)
            {
                WithViewModel(vm =>
                    vm.PrepareEditCard(card));
                Frame.Navigate(typeof(CardDetailPage));
            }
        }

        private void WithViewModel(Action<BoardViewModel> action)
        {
            var viewModel = ForView.Unwrap<BoardViewModel>(DataContext);
            if (viewModel != null)
            {
                action(viewModel);
            }
        }

        private void ManageProjects_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProjectsPage));
        }
    }
}
