using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Correspondence;

namespace CardBoard.Projects.ViewModels
{
    public class ProjectListViewModel
    {
        private readonly Community _community;
        private readonly Individual _individual;

        public ProjectListViewModel(Community community, Individual individual)
        {
            _community = community;
            _individual = individual;
        }
    }
}
