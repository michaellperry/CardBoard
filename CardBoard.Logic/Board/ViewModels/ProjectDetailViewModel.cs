using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardBoard.Board.Models;
using UpdateControls.Correspondence;
using UpdateControls.Fields;
using CardBoard.Model;

namespace CardBoard.Board.ViewModels
{
    public class ProjectDetailViewModel
    {
        private readonly Community _community;
        private readonly Individual _individual;
        private readonly Project _project;
        private readonly CardSelectionModel _cardSelectionModel;

        private readonly Dependent<Column> _toDoColumn;
        private readonly Dependent<Column> _doingColumn;
        private readonly Dependent<Column> _doneColumn;
        
        public ProjectDetailViewModel(
            Community community, 
            Individual individual, 
            Project project,
            CardSelectionModel cardSelectionModel)
        {
            _community = community;
            _individual = individual;
            _project = project;
            _cardSelectionModel = cardSelectionModel;

            _toDoColumn = new Dependent<Column>(() => GetColumn(project, "To Do"));
            _doingColumn = new Dependent<Column>(() => GetColumn(project, "Doing"));
            _doneColumn = new Dependent<Column>(() => GetColumn(project, "Done"));
        }

        public IEnumerable<CardViewModel> ToDoCards
        {
            get { return CardsInColumn(_toDoColumn); }
        }

        public CardViewModel SelectedToDoCard
        {
            get { return SelectedCardInColumn(_toDoColumn); }
            set { if (value != null) _cardSelectionModel.SelectedCard = value.Card; }
        }

        public IEnumerable<CardViewModel> DoingCards
        {
            get { return CardsInColumn(_doingColumn); }
        }

        public CardViewModel SelectedDoingCard
        {
            get { return SelectedCardInColumn(_doingColumn); }
            set { if (value != null) _cardSelectionModel.SelectedCard = value.Card; }
        }

        public IEnumerable<CardViewModel> DoneCards
        {
            get { return CardsInColumn(_doneColumn); }
        }

        public CardViewModel SelectedDoneCard
        {
            get { return SelectedCardInColumn(_doneColumn); }
            set { if (value != null) _cardSelectionModel.SelectedCard = value.Card; }
        }

        public async Task MoveCard(Uri uri, int columnIndex)
        {
            var cards = await _project.Cards.EnsureAsync();
            var card = cards.FirstOrDefault(c => UriOfCard(c) == uri);
            if (card == null)
                return;

            var column =
                columnIndex == 0 ? await _project.MakeColumnAsync("To Do") :
                columnIndex == 1 ? await _project.MakeColumnAsync("Doing") :
                columnIndex == 2 ? await _project.MakeColumnAsync("Done") :
                    null;
            if (column == null)
                return;

            var cardColumns = await card.CardColumns.EnsureAsync();
            if (cardColumns.Count() == 1 &&
                cardColumns.Single().Column == column)
                return;

            await _community.AddFactAsync(new CardColumn(card, column, cardColumns));
        }

        private IEnumerable<CardViewModel> CardsInColumn(Column column)
        {
            if (column == null)
                return Enumerable.Empty<CardViewModel>();

            return
                from card in column.Cards
                orderby card.Created
                select new CardViewModel(card);
        }

        private CardViewModel SelectedCardInColumn(Column column)
        {
            if (_cardSelectionModel.SelectedCard == null ||
               !_cardSelectionModel.SelectedCard.CardColumns.Any(cc => cc.Column == column))
            {
                return null;
            }

            return new CardViewModel(_cardSelectionModel.SelectedCard);
        }

        private Column GetColumn(Project project, string name)
        {
            return project.Columns.FirstOrDefault(c => c.Name.Value == name);
        }

        public static Uri UriOfCard(Card card)
        {
            return new Uri(
                String.Format("cardboard://card/{0}/{1}",
                    card.Created.Ticks,
                    card.Unique),
                UriKind.Absolute);
        }
    }
}
