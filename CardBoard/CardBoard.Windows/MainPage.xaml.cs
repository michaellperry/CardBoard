using CardBoard.Board.Models;
using CardBoard.Board.ViewModels;
using CardBoard.Board.Views;
using UpdateControls.XAML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = ForView.Unwrap<BoardViewModel>(DataContext);

            if (viewModel != null)
            {
                viewModel.CardEdited += CardEdited;
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = ForView.Unwrap<BoardViewModel>(DataContext);

            if (viewModel != null)
            {
                viewModel.CardEdited -= CardEdited;
            }

            base.OnNavigatedFrom(e);
        }

        void CardEdited(object sender, CardEditedEventArgs args)
        {
            Popup popup = new Popup()
            {
                ChildTransitions = new TransitionCollection { new PopupThemeTransition() }
            };
            var detail = new CardDetailControl()
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
                DataContext = args.CardDetail
            };
            detail.Ok += delegate
            {
                popup.IsOpen = false;
                if (args.Completed != null)
                    args.Completed(args.CardDetail);
            };
            detail.Cancel += delegate
            {
                popup.IsOpen = false;
            };
            popup.Child = detail;
            popup.IsOpen = true;
        }

        private void ManageProjects_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Projects.Views.ProjectsPage));
        }
    }
}
