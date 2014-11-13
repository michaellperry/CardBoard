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

        public void Clear(Project project)
        {
            _text.Value = String.Empty;
            var columns = project.Columns;
            var columnOrdinals = columns.Select(c => new
            {
                Ordinal = c.Ordinal.Value,
                Column = c
            });
            var awaitedColumnOrdinals = columnOrdinals;
            var column = awaitedColumnOrdinals
                .OrderBy(c => c.Ordinal)
                .Select(c => c.Column)
                .FirstOrDefault();
            _selectedColumn.Value = column;
        }

        public void FromCard(Card card)
        {
            _text.Value = card.Text;
        }

        public async Task ToCard(Card card)
        {
            card.Text = _text.Value;
            //var column = await GetColumn(card);
            //if (_selectedColumn.Value != column)
            //{
            //    var prior = await card.CardColumns.EnsureAsync();
            //    await card.Community.AddFactAsync(new CardColumn(
            //        card, _selectedColumn.Value, prior));
            //}
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
