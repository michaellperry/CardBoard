using System;

namespace CardBoard.Board.Models
{
    public class CardEditedEventArgs : EventArgs
    {
        public CardDetailModel CardDetail { get; set; }
        public Action<CardDetailModel> Completed { get; set; }
    }
}
