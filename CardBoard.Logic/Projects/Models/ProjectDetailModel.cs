using CardBoard.Model;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace CardBoard.Projects.Models
{
    public class ProjectDetailModel
    {
        public string Identifier { get; set; }
        public string Name { get; set; }

        public void FromProject(Project project)
        {
            Identifier = project.Identifier;
            Name = project.Name;
        }

        public void ToProject(Project project)
        {
            project.Name = Name;
        }
    }
}
