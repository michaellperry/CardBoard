using CardBoard.Model;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace CardBoard.Projects.Models
{
    public class ProjectDetailModel
    {
        private string _identifier;
        private string _name;

        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void Clear()
        {
            Name = string.Empty;
        }

        public void FromProject(Project project)
        {
            Name = project.Name;
        }

        public void ToProject(Project project)
        {
            project.Name = Name;
        }
    }
}
