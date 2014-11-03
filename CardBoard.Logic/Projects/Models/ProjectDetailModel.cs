using CardBoard.Model;
using System;

namespace CardBoard.Projects.Models
{
    public class ProjectDetailModel
    {
        public string Name { get; set; }

        public static ProjectDetailModel FromProject(Project project)
        {
            return new ProjectDetailModel
            {
                Name = project.Name
            };
        }

        public void ToProject(Project project)
        {
            project.Name = Name;
        }
    }
}
