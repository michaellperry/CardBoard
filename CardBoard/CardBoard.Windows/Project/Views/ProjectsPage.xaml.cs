using CardBoard.Project.Views;
using CardBoard.Projects.ViewModels;
using UpdateControls.XAML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace CardBoard.Projects.Views
{
    public sealed partial class ProjectsPage : Page
    {
        public ProjectsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = ForView.Unwrap<ProjectListViewModel>(DataContext);
            if (viewModel != null)
            {
                viewModel.ProjectEdited += ProjectEdited;
                viewModel.ProjectAdded += ProjectAdded;
                viewModel.ProjectJoined += ProjectJoined;
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = ForView.Unwrap<ProjectListViewModel>(DataContext);
            if (viewModel != null)
            {
                viewModel.ProjectEdited -= ProjectEdited;
                viewModel.ProjectAdded -= ProjectAdded;
                viewModel.ProjectJoined -= ProjectJoined;
            }

            base.OnNavigatedFrom(e);
        }

        void ProjectEdited(object sender, Models.ProjectEditedEventArgs args)
        {
            DisplayDialog(args, new ProjectDetailControl());
        }

        private void ProjectAdded(object sender, CardBoard.Projects.Models.ProjectEditedEventArgs args)
        {
            DisplayDialog(args, new NewProjectControl());
        }

        private void ProjectJoined(object sender, CardBoard.Projects.Models.ProjectEditedEventArgs args)
        {
            DisplayDialog(args, new JoinProjectControl());
        }

        private static void DisplayDialog(Models.ProjectEditedEventArgs args, IDialogControl detail)
        {
            Popup popup = new Popup()
            {
                ChildTransitions = new TransitionCollection { new PopupThemeTransition() }
            };
            detail.Width = Window.Current.Bounds.Width;
            detail.Height = Window.Current.Bounds.Height;
            detail.DataContext = args.ProjectDetail;
            detail.Ok += delegate
            {
                popup.IsOpen = false;
                if (args.Completed != null)
                    args.Completed(args.ProjectDetail);
            };
            detail.Cancel += delegate
            {
                popup.IsOpen = false;
            };
            popup.Child = (UIElement)detail;
            popup.IsOpen = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
