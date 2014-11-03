using System;

namespace CardBoard.Projects.Models
{
    public class ProjectEditedEventArgs : EventArgs
    {
        public ProjectDetailModel ProjectDetail { get; set; }
        public Action<ProjectDetailModel> Completed { get; set; }
    }
}
