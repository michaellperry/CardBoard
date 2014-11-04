using CardBoard.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using UpdateControls.Collections;
using UpdateControls.Fields;

namespace CardBoard.Board.Models
{
    public class CardDetailModel
    {
        private Independent<string> _text = new Independent<string>();
        private Independent<Column> _selectedColumn = new Independent<Column>();

        public string Text
        {
            get { return _text.Value; }
            set { _text.Value = value; }
        }

        public async Task Clear(Project project)
        {
            _text.Value = String.Empty;
            var columns = await project.Columns.EnsureAsync();
            var column = columns.FirstOrDefault();
            _selectedColumn.Value = column;
        }

        public async Task FromCard(Card card)
        {
            Text = card.Text;
            _selectedColumn.Value = await GetColumn(card);
        }

        public async Task ToCard(Card card)
        {
            card.Text = Text;
            var column = await GetColumn(card);
            if (_selectedColumn.Value != column)
            {
                var prior = await card.CardColumns.EnsureAsync();
                await card.Community.AddFactAsync(new CardColumn(card, _selectedColumn.Value, prior));
            }
        }

        private static async Task<Column> GetColumn(Card card)
        {
            var columns = await card.CardColumns.EnsureAsync();
            var cardColumn = columns.FirstOrDefault();
            var column = await cardColumn.Column.EnsureAsync();
            return column;
        }
    }
}
