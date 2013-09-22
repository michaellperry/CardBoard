using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CardBoard.Common;
using CardBoard.Projects.Models;
using UpdateControls.XAML;

namespace CardBoard.Projects.ViewModels
{
    public class ProjectListViewModel
    {
        private readonly SynchronizationService _synchronizationService;
        private readonly Individual _individual;
        private readonly ProjectSelectionModel _projectSelectionModel;
        
        public delegate void ProjectEditedHandler(object sender, ProjectEditedEventArgs args);
        public event ProjectEditedHandler ProjectEdited;
        
        public ProjectListViewModel(
            SynchronizationService synchronizationService,
            Individual individual,
            ProjectSelectionModel projectSelectionModel)
        {
            _synchronizationService = synchronizationService;
            _individual = individual;
            _projectSelectionModel = projectSelectionModel;
        }

        public IEnumerable<ProjectHeaderViewModel> Projects
        {
            get
            {
                return
                    from project in _individual.Projects
                    orderby project.Created
                    select new ProjectHeaderViewModel(project);
            }
        }

        public ProjectHeaderViewModel SelectedProject
        {
            get
            {
                if (_projectSelectionModel.SelectedProject == null)
                    return null;
                return new ProjectHeaderViewModel(_projectSelectionModel.SelectedProject);
            }
            set
            {
                if (value == null)
                    _projectSelectionModel.SelectedProject = null;
                _projectSelectionModel.SelectedProject = value.Project;
            }
        }

        public ICommand DeleteProject
        {
            get
            {
                return MakeCommand
                    .When(() => _projectSelectionModel.SelectedProject != null)
                    .Do(delegate
                    {
                        DeleteProjectInternal(_projectSelectionModel.SelectedProject);
                    });
            }
        }

        public ICommand EditProject
        {
            get
            {
                return MakeCommand
                    .When(() => _projectSelectionModel.SelectedProject != null)
                    .Do(delegate
                    {
                        var project = _projectSelectionModel.SelectedProject;
                        if (ProjectEdited != null)
                        {
                            ProjectEdited(this, new ProjectEditedEventArgs
                            {
                                ProjectDetail = ProjectDetailModel.FromProject(project),
                                Completed = projectDetail =>
                                    projectDetail.ToProject(project)
                            });
                        }
                    });
            }
        }

        public ICommand AddProject
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        if (ProjectEdited != null)
                        {
                            ProjectEdited(this, new ProjectEditedEventArgs
                            {
                                ProjectDetail = new ProjectDetailModel(),
                                Completed = projectDetail =>
                                    AddProjectInternal(projectDetail)
                            });
                        }
                    });
            }
        }

        private async void DeleteProjectInternal(Project project)
        {
            try
            {
                var members = (await _individual.Memberships.EnsureAsync())
                    .Where(m => m.Project == project);
                foreach (var member in members)
                    await member.Community.AddFactAsync(new MemberDelete(member));
            }
            catch (Exception x)
            {
                // TODO: Report the exception.
            }
        }

        private async void AddProjectInternal(ProjectDetailModel projectDetail)
        {
            try
            {
                var project = await _individual.Community.AddFactAsync(new Project(
                    DateTime.Now,
                    Utilities.GenerateRandomId()));
                projectDetail.ToProject(project);
                var member = await _individual.Community.AddFactAsync(new Member(
                    _individual,
                    project));

                _synchronizationService.Project = project;
            }
            catch (Exception x)
            {
                // TODO: Report the exception.
            }
        }
    }
}
