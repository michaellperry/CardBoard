using System;

namespace CardBoard.Board.ViewModels
{
    public class CardViewModel
    {
        private readonly Card _card;

        public CardViewModel(Card card)
        {
            _card = card;            
        }

        public string Text
        {
            get { return _card.Text; }
        }
    }
}
