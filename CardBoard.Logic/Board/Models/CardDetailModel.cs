using CardBoard.Model;
using System;

namespace CardBoard.Board.Models
{
    public class CardDetailModel
    {
        public string Text { get; set; }

        public static CardDetailModel FromCard(Card card)
        {
            return new CardDetailModel
            {
                Text = card.Text
            };
        }

        public void ToCard(Card card)
        {
            card.Text = Text;
        }
    }
}
