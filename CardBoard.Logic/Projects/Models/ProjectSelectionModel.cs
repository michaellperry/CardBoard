using CardBoard.Model;
using UpdateControls.Fields;

namespace CardBoard.Projects.Models
{
    public class ProjectSelectionModel
    {
        private Independent<Project> _selectedProject = new Independent<Project>();

        public Project SelectedProject
        {
            get { return _selectedProject.Value; }
            set { _selectedProject.Value = value; }
        }
    }
}
