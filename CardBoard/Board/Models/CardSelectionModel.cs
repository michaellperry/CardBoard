using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Fields;

namespace CardBoard.Board.Models
{
    public class CardSelectionModel
    {
        private Independent<Card> _selectedCard = new Independent<Card>();

        public Card SelectedCard
        {
            get { return _selectedCard.Value; }
            set { _selectedCard.Value = value; }
        }
    }
}
