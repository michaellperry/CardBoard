using System;
using System.ComponentModel;
using System.Linq;
using UpdateControls.XAML;

namespace CardBoard.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly SynchronizationService _synchronizationService;

        private Board.Models.CardSelectionModel _cardSelectionModel =
            new Board.Models.CardSelectionModel();

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _synchronizationService.Initialize();
        }

        public object Board
        {
            get
            {
                return ViewModel(() => new Board.ViewModels.BoardViewModel(
                    _synchronizationService,
                    _cardSelectionModel));
            }
        }

        public object ProjectList
        {
            get
            {
                return ViewModel(() => _synchronizationService.Individual == null
                    ? null :
                    new Projects.ViewModels.ProjectListViewModel(
                        _synchronizationService.Community,
                        _synchronizationService.Individual));
            }
        }
    }
}
