using CardBoard.Model;
using System;

namespace CardBoard.Board.ViewModels
{
    public class ColumnViewModel
    {
        private readonly Column _column;

        public ColumnViewModel(Column column)
        {
            _column = column;            
        }

        public Column Column
        {
            get { return _column; }
        }

        public string Name
        {
            get { return _column.Name; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Object.Equals(this._column, ((ColumnViewModel)obj)._column);
        }

        public override int GetHashCode()
        {
            return _column.GetHashCode();
        }
    }
}
