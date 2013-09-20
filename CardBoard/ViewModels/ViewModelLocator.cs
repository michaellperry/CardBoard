using System;
using System.ComponentModel;
using System.Linq;
using UpdateControls.XAML;

namespace CardBoard.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly SynchronizationService _synchronizationService;

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _synchronizationService.Initialize();
        }

        public object Main
        {
            get
            {
                return ViewModel(() => _synchronizationService.Individual == null
                    ? null :
                    new MainViewModel(
                        _synchronizationService.Community,
                        _synchronizationService.Individual));
            }
        }
    }
}
