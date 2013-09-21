using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Board.Models
{
    public class CardEditedEventArgs
    {
        public CardDetailModel CardDetail { get; set; }
        public Action<CardDetailModel> Completed { get; set; }
    }
}
