using CardBoard.Model;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UpdateControls.Fields;
using System.Threading.Tasks;

namespace CardBoard.Projects.Models
{
    public class ProjectDetailModel
    {
        private Independent<string> _identifier = new Independent<string>(string.Empty);
        private Independent<string> _name = new Independent<string>(string.Empty);

        private Project _project = Project.GetNullInstance();
        
        public string Identifier
        {
            get { return _identifier; }
            set { _identifier.Value = value; }
        }

        public string ExistingIdentifier
        {
            get { return _project.Identifiers.Select(i => i.Token).FirstOrDefault(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name.Value = value; }
        }

        public void Clear()
        {
            Identifier = string.Empty;
            Name = string.Empty;
        }

        public void FromProject(Project project)
        {
            _project = project;
            Name = project.Name ?? string.Empty;
        }

        public void ToProject(Project project)
        {
            project.Name = Name;
        }

        public bool CanAdd
        {
            get { return Identifier.Length > 3 && Name.Length > 3; }
        }

        public bool CanJoin
        {
            get { return Identifier.Length > 3; }
        }
    }
}
