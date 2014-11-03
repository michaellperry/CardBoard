using CardBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Board.ViewModels
{
    public class ProjectHeaderViewModel
    {
        private readonly Project _project;

        public ProjectHeaderViewModel(Project project)
        {
            _project = project;            
        }

        public Project Project
        {
            get { return _project; }
        }

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_project.Name))
                    return "<<New project>>";

                return _project.Name;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            ProjectHeaderViewModel that = obj as ProjectHeaderViewModel;
            if (that == null)
                return false;
            return Object.Equals(this._project, that._project);
        }

        public override int GetHashCode()
        {
            return _project.GetHashCode();
        }
    }
}
