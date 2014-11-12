using CardBoard.Board.Models;
using CardBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UpdateControls.XAML;

namespace CardBoard.Board.ViewModels
{
    public class CardDetailViewModel
    {
        private readonly CardDetailModel _cardDetail;
        private readonly Project _project;
        private readonly Card _card;
        
        public CardDetailViewModel(CardDetailModel cardDetail, Project project, Card card)
        {
            _cardDetail = cardDetail;
            _project = project;
            _card = card;
        }

        public string Text
        {
            get { return _cardDetail.Text; }
            set { _cardDetail.Text = value; }
        }

        public IEnumerable<ColumnViewModel> Columns
        {
            get
            {
                return
                    from column in _project.Columns
                    orderby column.Ordinal.Value
                    select new ColumnViewModel(column);
            }
        }

        public ColumnViewModel SelectedColumn
        {
            get
            {
                return _cardDetail.SelectedColumn == null ? null
                    : new ColumnViewModel(_cardDetail.SelectedColumn);
            }
            set
            {
                if (value != null)
                    _cardDetail.SelectedColumn = value.Column;
            }
        }

        public void Ok()
        {
            _project.Community.Perform(async delegate
            {
                var card = _card;
                if (card == null)
                {
                    card = await _project.Community.AddFactAsync(new Card(
                        _project, DateTime.Now.ToUniversalTime()));
                }
                await _cardDetail.ToCard(card);
            });
        }
    }
}
