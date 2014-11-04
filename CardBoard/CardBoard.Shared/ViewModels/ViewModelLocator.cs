using UpdateControls.XAML;

namespace CardBoard.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly SynchronizationService _synchronizationService;

        private Board.Models.CardSelectionModel _cardSelectionModel =
            new Board.Models.CardSelectionModel();
        private Projects.Models.ProjectSelectionModel _projectSelectionModel =
            new Projects.Models.ProjectSelectionModel();
        private CardBoard.Board.Models.CardDetailModel _cardDetail =
            new Board.Models.CardDetailModel();

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _synchronizationService.Initialize();
            else
                _synchronizationService.InitializeDesignMode();
        }

        public object Board
        {
            get
            {
                return ViewModel(() => new Board.ViewModels.BoardViewModel(
                    _synchronizationService,
                    _cardSelectionModel,
                    _cardDetail));
            }
        }

        public object CardDetail
        {
            get
            {
                return ViewModel(() => new CardBoard.Board.ViewModels.CardDetailViewModel(
                    _cardDetail,
                    _synchronizationService.Project,
                    _cardSelectionModel.SelectedCard));
            }
        }

        public object ProjectList
        {
            get
            {
                return ViewModel(() => _synchronizationService.Individual == null
                    ? null :
                    new Projects.ViewModels.ProjectListViewModel(
                        _synchronizationService,
                        _synchronizationService.Individual,
                        _projectSelectionModel));
            }
        }
    }
}
