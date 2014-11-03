using CardBoard.Model;
using System;
using System.Linq;

namespace CardBoard.Board.ViewModels
{
    public class CardViewModel
    {
        private readonly Card _card;

        public CardViewModel(Card card)
        {
            _card = card;            
        }

        public Card Card
        {
            get { return _card; }
        }

        public string Text
        {
            get { return _card.Text; }
        }

        public bool Conflict
        {
            get { return _card.CardColumns.Count() > 1; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            CardViewModel that = obj as CardViewModel;
            if (that == null)
                return false;
            return Object.Equals(this._card, that._card);
        }

        public override int GetHashCode()
        {
            return _card.GetHashCode();
        }
    }
}
