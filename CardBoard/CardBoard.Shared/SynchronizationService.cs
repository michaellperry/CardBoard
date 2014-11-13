using CardBoard.Common;
using CardBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UpdateControls;
using UpdateControls.Collections;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.BinaryHTTPClient;
using UpdateControls.Correspondence.FileStream;
using UpdateControls.Correspondence.Memory;
using UpdateControls.Fields;
using Windows.UI.Xaml;

namespace CardBoard
{
    public class SynchronizationService : ISynchronizationService, IUpdatable
    {
        private const string ThisIndividual = "CardBoard.Individual.this";
        private const string CurrentProject = "CardBoard.Project.current";

        private Community _community;
        private Independent<Individual> _individual = new Independent<Individual>(
            Individual.GetNullInstance());
        private Independent<CardBoard.Model.Project> _project = new Independent<CardBoard.Model.Project>(
            CardBoard.Model.Project.GetNullInstance());
        private IndependentList<Identifier> _identifiers = new IndependentList<Identifier>();

        private Dependent<IEnumerable<Model.Project>> _pendingProjects;

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
            _community.Subscribe(() => (IEnumerable<Identifier>)_identifiers);

            ScheduleSynchronization(http);

            LoadInitialFacts(http);

            _pendingProjects = new Dependent<IEnumerable<Model.Project>>(FindPendingProjects);
            _pendingProjects.Invalidated += JoinPendingProjects;
            JoinPendingProjects();
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

        public CardBoard.Model.Project Project
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

        public void SubscribeTo(Identifier identifier)
        {
            _identifiers.Add(identifier);
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

        private void SetCurrentProject(CardBoard.Model.Project project)
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

        private async Task<CardBoard.Model.Project> InitializeProject(Individual individual)
        {
            CardBoard.Model.Project project = await _community.LoadFactAsync<CardBoard.Model.Project>(CurrentProject);
            var projects = await individual.Projects.EnsureAsync();
            if (project == null && projects.Any())
            {
                project = projects.First();
                await _community.SetFactAsync(CurrentProject, project);
            }

            lock (this)
            {
                _project.Value = project ?? CardBoard.Model.Project.GetNullInstance();
            }
            return project;
        }

        private IEnumerable<Model.Project> FindPendingProjects()
        {
            Individual individual = Individual;
            if (individual.IsNull)
                return Enumerable.Empty<Model.Project>();
            else
                return
                    from identifier in _identifiers
                    from project in identifier.Projects
                    where !individual.Projects.Contains(project)
                    select project;
        }

        private void JoinPendingProjects()
        {
            UpdateScheduler.ScheduleUpdate(this);
        }

        public void UpdateNow()
        {
            var pendingProjects = _pendingProjects.Value.ToList();
            _community.Perform(async delegate
            {
                foreach (var project in pendingProjects)
                {
                    await _community.AddFactAsync(new Member(Individual, project));
                }
            });
        }
    }
}
