using CardBoard.Board.Models;
using CardBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UpdateControls.XAML;

namespace CardBoard.Board.ViewModels
{
    public class BoardViewModel
    {
        private readonly ISynchronizationService _synchronizationService;
        private readonly CardSelectionModel _cardSelectionModel;
        private readonly CardDetailModel _cardDetail;
        
        public delegate void CardEditedHandler(object sender, CardEditedEventArgs args);
        public event CardEditedHandler CardEdited;
        
        public BoardViewModel(
            ISynchronizationService synchronizationService,
            CardSelectionModel cardSelectionModel,
            CardDetailModel cardDetail)
        {
            _synchronizationService = synchronizationService;
            _cardSelectionModel = cardSelectionModel;
            _cardDetail = cardDetail;
        }

        public string LastError
        {
            get
            {
                if (_synchronizationService.Community.LastException == null)
                    return null;

                return _synchronizationService.Community.LastException.Message;
            }
        }

        public bool Busy
        {
            get { return _synchronizationService.Community.Synchronizing; }
        }

        public ICommand Refresh
        {
            get
            {
                return MakeCommand
                    .When(() => !_synchronizationService.Community.Synchronizing)
                    .Do(() => _synchronizationService.Community.SynchronizeAsync());
            }
        }

        public string ProjectName
        {
            get
            {
                if (_synchronizationService.Project == null)
                    return null;

                return _synchronizationService.Project.Name;
            }
        }

        public IEnumerable<ProjectHeaderViewModel> Projects
        {
            get
            {
                if (_synchronizationService.Individual == null)
                    return Enumerable.Empty<ProjectHeaderViewModel>();

                return
                    from project in _synchronizationService.Individual.Projects
                    orderby project.Created
                    select new ProjectHeaderViewModel(project);
            }
        }

        public ProjectHeaderViewModel SelectedProject
        {
            get
            {
                if (_synchronizationService.Project == null)
                    return null;

                return new ProjectHeaderViewModel(_synchronizationService.Project);
            }
            set
            {
                if (value != null)
                {
                    _synchronizationService.Project = value.Project;
                }
            }
        }

        public ProjectDetailViewModel ProjectDetail
        {
            get
            {
                if (_synchronizationService.Individual == null)
                    return null;
                if (_synchronizationService.Project == null)
                    return null;

                return new ProjectDetailViewModel(
                    _synchronizationService.Community,
                    _synchronizationService.Individual,
                    _synchronizationService.Project,
                    _cardSelectionModel);
            }
        }

        public ICommand DeleteCard
        {
            get
            {
                return MakeCommand
                    .When(() => _cardSelectionModel.SelectedCard != null)
                    .Do(() =>
                    {
                        var card = _cardSelectionModel.SelectedCard;
                        _synchronizationService.Community.AddFactAsync(
                            new CardDelete(card));
                    });
            }
        }

        public ICommand EditCard
        {
            get
            {
                return MakeCommand
                    .When(() => SelectedCard != null)
                    .Do(delegate
                    {
                        var selectedCard = SelectedCard;
                        if (CardEdited != null)
                        {
                            _cardDetail.Text = selectedCard.Text;
                            CardEdited(this, new CardEditedEventArgs
                            {
                                CardDetail = _cardDetail,
                                Completed = c =>
                                    c.ToCard(selectedCard)
                            });
                        }
                    });
            }
        }

        public ICommand NewCard
        {
            get
            {
                return MakeCommand
                    .When(() => _synchronizationService.Project != null)
                    .Do(delegate
                    {
                        var project = _synchronizationService.Project;
                        if (CardEdited != null)
                        {
                            _cardDetail.Text = String.Empty;
                            CardEdited(this, new CardEditedEventArgs
                            {
                                CardDetail = _cardDetail,
                                Completed = c =>
                                    AddCardCompleted(project, c)
                            });
                        }
                    });
            }
        }

        private Card SelectedCard
        {
            get
            {
                if (_cardSelectionModel.SelectedCard != null &&
                    _cardSelectionModel.SelectedCard.Project == _synchronizationService.Project)
                    return _cardSelectionModel.SelectedCard;

                return null;
            }
            set { _cardSelectionModel.SelectedCard = value; }
        }

        private async void AddCardCompleted(Project project, CardDetailModel cardDetail)
        {
            try
            {
                var card = await _synchronizationService.Community.AddFactAsync(
                    new Card(project, DateTime.Now));
                cardDetail.ToCard(card);

                Column column = await _synchronizationService.Project.MakeColumnAsync("To Do");
                await _synchronizationService.Community.AddFactAsync(
                    new CardColumn(card, column, Enumerable.Empty<CardColumn>()));

                SelectedCard = card;
            }
            catch (Exception x)
            {
                // TODO: Report error.
            }
        }

        public void PrepareNewCard()
        {
            _cardDetail.Text = String.Empty;
        }
    }
}
