using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Model
{
    public partial class Project
    {
        public async Task<Column> MakeColumnAsync(string name)
        {
            var columns = await Columns.EnsureAsync();
            Column column = await columns.FirstOrDefaultAsync(async delegate(Column c)
            {
                return (await c.Name.EnsureAsync()).Value == name;
            });
            if (column == null)
            {
                column = await Community.AddFactAsync(new Column(this));
                column.Name = name;
            }
            return column;
        }
    }
}
