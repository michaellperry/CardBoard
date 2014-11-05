using CardBoard.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        public Column SelectedColumn
        {
            get { return _selectedColumn.Value; }
            set { _selectedColumn.Value = value; }
        }

        public async Task Clear(Project project)
        {
            _text.Value = String.Empty;
            var columns = await project.Columns.EnsureAsync();
            var columnOrdinals = columns.Select(async c => new
            {
                Ordinal = (await c.Ordinal.EnsureAsync()).Value,
                Column = c
            });
            var awaitedColumnOrdinals = await Task.WhenAll(columnOrdinals.ToArray());
            var column = awaitedColumnOrdinals
                .OrderBy(c => c.Ordinal)
                .Select(c => c.Column)
                .FirstOrDefault();
            _selectedColumn.Value = column;
        }

        public async Task FromCard(Card card)
        {
            _text.Value = card.Text;
            _selectedColumn.Value = await GetColumn(card);
        }

        public async Task ToCard(Card card)
        {
            card.Text = _text.Value;
            var column = await GetColumn(card);
            if (_selectedColumn.Value != column)
            {
                var prior = await card.CardColumns.EnsureAsync();
                await card.Community.AddFactAsync(new CardColumn(
                    card, _selectedColumn.Value, prior));
            }
        }

        private static async Task<Column> GetColumn(Card card)
        {
            var columns = await card.CardColumns.EnsureAsync();
            var cardColumn = columns.FirstOrDefault() ?? CardColumn.GetNullInstance();
            var column = await cardColumn.Column.EnsureAsync();
            return column;
        }
    }
}
