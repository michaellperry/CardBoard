using CardBoard.Board.Models;
using CardBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UpdateControls;
using UpdateControls.Fields;
using UpdateControls.XAML;

namespace CardBoard.Board.ViewModels
{
    public class BoardViewModel : IUpdatable
    {
        private readonly ISynchronizationService _synchronizationService;
        private readonly CardSelectionModel _cardSelectionModel;
        private readonly CardDetailModel _cardDetail;
        
        public delegate void CardEditedHandler(object sender, CardEditedEventArgs args);
        public event CardEditedHandler CardEdited;

        public delegate void CardSelectedHandler(Card card);
        public event CardSelectedHandler CardSelected;

        private Dependent<Card> _selectedCard;
        
        public BoardViewModel(
            ISynchronizationService synchronizationService,
            CardSelectionModel cardSelectionModel,
            CardDetailModel cardDetail)
        {
            _synchronizationService = synchronizationService;
            _cardSelectionModel = cardSelectionModel;
            _cardDetail = cardDetail;

            _selectedCard = new Dependent<Card>(() => _cardSelectionModel.SelectedCard);
            _selectedCard.Invalidated += ScheduleUpdateSelectedCard;
            ScheduleUpdateSelectedCard();
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
                            _synchronizationService.Community
                                .Perform(async delegate
                                {
                                    await _cardDetail.FromCard(selectedCard);
                                    CardEdited(this, new CardEditedEventArgs
                                    {
                                        CardDetail = _cardDetail,
                                        Completed = c => _synchronizationService.Community
                                            .Perform(async delegate
                                            {
                                                await c.ToCard(selectedCard);
                                            })
                                    });
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
                            _synchronizationService.Community.Perform(async delegate
                            {
                                await _cardDetail.Clear(_synchronizationService.Project);
                                CardEdited(this, new CardEditedEventArgs
                                {
                                    CardDetail = _cardDetail,
                                    Completed = c =>
                                        AddCardCompleted(project, c)
                                });
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

        private void AddCardCompleted(Project project, CardDetailModel cardDetail)
        {
            _synchronizationService.Community.Perform(async delegate
            {
                var card = await _synchronizationService.Community.AddFactAsync(
                    new Card(project, DateTime.Now));
                await cardDetail.ToCard(card);

                Column column = await _synchronizationService.Project.MakeColumnAsync("To Do");
                await _synchronizationService.Community.AddFactAsync(
                    new CardColumn(card, column, Enumerable.Empty<CardColumn>()));

                SelectedCard = card;
            });
        }

        public void PrepareNewCard()
        {
            _synchronizationService.Community.Perform(async delegate
            {
                await _cardDetail.Clear(_synchronizationService.Project);
            });
        }

        public void PrepareEditCard(Card card)
        {
            _synchronizationService.Community.Perform(async delegate
            {
                await _cardDetail.FromCard(card);
            });
        }

        public void ClearSelection()
        {
            _cardSelectionModel.SelectedCard = null;
        }

        private void ScheduleUpdateSelectedCard()
        {
            UpdateScheduler.ScheduleUpdate(this);
        }

        public void UpdateNow()
        {
            var selectedCard = _selectedCard.Value;
            if (CardSelected != null)
                CardSelected(selectedCard);
        }
    }
}
