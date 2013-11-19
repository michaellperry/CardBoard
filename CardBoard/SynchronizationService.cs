using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CardBoard.Common;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.BinaryHTTPClient;
using UpdateControls.Correspondence.FileStream;
using UpdateControls.Fields;
using Windows.UI.Xaml;

namespace CardBoard
{
    public class SynchronizationService
    {
        private const string ThisIndividual = "CardBoard.Individual.this";
        private const string CurrentProject = "CardBoard.Project.current";

        private Community _community;
        private Independent<Individual> _individual = new Independent<Individual>();
        private Independent<Project> _project = new Independent<Project>();

        public void Initialize()
        {
            var storage = new FileStreamStorageStrategy();
            var http = new HTTPConfigurationProvider();
            var communication = new BinaryHTTPAsynchronousCommunicationStrategy(http);

            _community = new Community(storage);
            _community.AddAsynchronousCommunicationStrategy(communication);
            _community.Register<CorrespondenceModel>();
            _community.Subscribe(() => _individual.Value);
            _community.Subscribe(() => _individual.Value == null ? Enumerable.Empty<Project>() :
                _individual.Value.Projects);

            ScheduleSynchronization(http);

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

            // Synchronize when the network becomes available.
            System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += (sender, e) =>
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    Synchronize();
            };

            // And synchronize on startup or resume.
            Synchronize();
        }

        private async void LoadInitialFacts(HTTPConfigurationProvider http)
        {
            try
            {
                var individual = await InitializeIndividual(http);
                var project = await InitializeProject(individual);
            }
            catch (Exception x)
            {
                // TODO: Report the exception.
            }
        }

        private async void SetCurrentProject(Project project)
        {
            try
            {
                await _community.SetFactAsync(CurrentProject, project);
            }
            catch (Exception x)
            {
                // TODO: Report the exception.
            }
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
                //project = await _community.AddFactAsync(
                //    new Project(DateTime.Now, Utilities.GenerateRandomId()));
                project = await _community.AddFactAsync(
                    new Project(new DateTime(2013, 9, 24, 10, 13, 0, DateTimeKind.Utc), "MVP Summit 2"));
                project.Name = "My Project";
                await _community.AddFactAsync(new Member(individual, project));
                await _community.SetFactAsync(CurrentProject, project);
            }

            lock (this)
            {
                _project.Value = project;
            }
            return project;
        }
    }
}
