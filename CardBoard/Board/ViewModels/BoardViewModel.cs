using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CardBoard.Board.Models;
using UpdateControls.XAML;

namespace CardBoard.Board.ViewModels
{
    public class BoardViewModel
    {
        private readonly SynchronizationService _synchronizationService;
        private readonly CardSelectionModel _cardSelectionModel;

        public delegate void CardEditedHandler(object sender, CardEditedEventArgs args);
        public event CardEditedHandler CardEdited;
        
        public BoardViewModel(
            SynchronizationService synchronizationService,
            CardSelectionModel cardSelectionModel)
        {
            _synchronizationService = synchronizationService;
            _cardSelectionModel = cardSelectionModel;
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
                    _synchronizationService.Project);
            }
        }

        public ICommand DeleteCard
        {
            get
            {
                return MakeCommand
                    .When(() => SelectedCard != null)
                    .Do(delegate
                    {
                        _synchronizationService.Community.AddFactAsync(
                            new CardDelete(SelectedCard));
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
                            CardEdited(this, new CardEditedEventArgs
                            {
                                CardDetail = CardDetailModel.FromCard(selectedCard),
                                Completed = cardDetail =>
                                    cardDetail.ToCard(selectedCard)
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
                            CardEdited(this, new CardEditedEventArgs
                            {
                                CardDetail = new CardDetailModel(),
                                Completed = cardDetail =>
                                    AddCardCompleted(project, cardDetail)
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

                Column column = null;
                var columns = await _synchronizationService.Project.Columns.EnsureAsync();
                foreach (var c in columns)
                {
                    if ((await c.Name.EnsureAsync()).Value == "To Do")
                    {
                        column = c;
                        break;
                    }
                }
                if (column == null)
                {
                    column = await _synchronizationService.Community
                        .AddFactAsync(new Column(project));
                    column.Name = "To Do";
                }
                await _synchronizationService.Community.AddFactAsync(
                    new CardColumn(card, column, Enumerable.Empty<CardColumn>()));

                SelectedCard = card;
            }
            catch (Exception x)
            {
                // TODO: Report error.
            }
        }
    }
}
