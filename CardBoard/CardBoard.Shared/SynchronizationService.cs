using CardBoard.Common;
using CardBoard.Model;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.BinaryHTTPClient;
using UpdateControls.Correspondence.FileStream;
using UpdateControls.Correspondence.Memory;
using UpdateControls.Fields;
using Windows.UI.Xaml;

namespace CardBoard
{
    public class SynchronizationService : ISynchronizationService
    {
        private const string ThisIndividual = "CardBoard.Individual.this";
        private const string CurrentProject = "CardBoard.Project.current";

        private Community _community;
        private Independent<Individual> _individual = new Independent<Individual>(
            Individual.GetNullInstance());
        private Independent<Project> _project = new Independent<Project>(
            Project.GetNullInstance());

        public void Initialize()
        {
            var storage = new FileStreamStorageStrategy();
            var http = new HTTPConfigurationProvider();
            var communication = new BinaryHTTPAsynchronousCommunicationStrategy(http);

            _community = new Community(storage);
            _community.AddAsynchronousCommunicationStrategy(communication);
            _community.Register<CorrespondenceModel>();
            _community.Subscribe(() => Individual);
            _community.Subscribe(() => Individual.Projects);

            ScheduleSynchronization(http);

            LoadInitialFacts(http);
        }

        public void InitializeDesignMode()
        {
            var storage = new MemoryStorageStrategy();
            var http = new HTTPConfigurationProvider();

            _community = new Community(storage);
            _community.Register<CorrespondenceModel>();

            LoadInitialFacts(http);
        }

        public Community Community
        {
            get { return _community; }
        }

        public Individual Individual
        {
            get
            {
                lock (this)
                {
                    return _individual;
                }
            }
        }

        public Project Project
        {
            get
            {
                lock (this)
                {
                    return _project;
                }
            }
            set
            {
                lock (this)
                {
                    _project.Value = value;
                    SetCurrentProject(value);
                }
            }
        }

        public void Synchronize()
        {
            _community.BeginSending();
            _community.BeginReceiving();
        }

        private void ScheduleSynchronization(HTTPConfigurationProvider http)
        {
            // Synchronize periodically.
            DispatcherTimer timer = new DispatcherTimer();
            int timeoutSeconds = Math.Min(http.Configuration.TimeoutSeconds, 30);
            timer.Interval = TimeSpan.FromSeconds(5 * timeoutSeconds);
            timer.Tick += delegate(object sender, object e)
            {
                Synchronize();
            };
            timer.Start();

            // Synchronize whenever the user has something to send.
            _community.FactAdded += delegate
            {
                Synchronize();
            };

            // Synchronize on startup and resume.
            if (NetworkInterface.GetIsNetworkAvailable())
                Synchronize();

            // Synchronize when the network becomes available.
            System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += (sender, e) =>
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    Synchronize();
            };
        }

        private void LoadInitialFacts(HTTPConfigurationProvider http)
        {
            _community.Perform(async delegate
            {
                var individual = await InitializeIndividual(http);
                var project = await InitializeProject(individual);
            });
        }

        private void SetCurrentProject(Project project)
        {
            _community.Perform(async delegate
            {
                await _community.SetFactAsync(CurrentProject, project);
            });
        }

        private async Task<Individual> InitializeIndividual(HTTPConfigurationProvider http)
        {
            Individual individual = await _community.LoadFactAsync<Individual>(ThisIndividual);
            if (individual == null)
            {
                individual = await _community.AddFactAsync(
                    new Individual(DateTime.Now, Utilities.GenerateRandomId()));
                await _community.SetFactAsync(ThisIndividual, individual);
            }
            lock (this)
            {
                _individual.Value = individual;
            }
            http.Individual = individual;

            return individual;
        }

        private async Task<Project> InitializeProject(Individual individual)
        {
            Project project = await _community.LoadFactAsync<Project>(CurrentProject);
            var projects = await individual.Projects.EnsureAsync();
            if (project == null || !projects.Contains(project))
            {
                project = await _community.AddFactAsync(
                    new Project(DateTime.Now));
                project.Name = "My Project";
                await _community.AddFactAsync(new Member(individual, project));
                await _community.SetFactAsync(CurrentProject, project);
                (await project.MakeColumnAsync("To Do")).Ordinal = 1;
                (await project.MakeColumnAsync("Doing")).Ordinal = 2;
                (await project.MakeColumnAsync("Done")).Ordinal = 3;
            }

            lock (this)
            {
                _project.Value = project;
            }
            return project;
        }
    }
}
