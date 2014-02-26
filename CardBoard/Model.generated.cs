using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Mementos;
using UpdateControls.Correspondence.Strategy;
using System;
using System.IO;

/**
/ For use with http://graphviz.org/
digraph "CardBoard"
{
    rankdir=BT
    Project__name -> Project [color="red"]
    Project__name -> Project__name [label="  *"]
    Member -> Individual [color="red"]
    Member -> Project
    MemberDelete -> Member
    Column -> Project [color="red"]
    Column__name -> Column
    Column__name -> Column__name [label="  *"]
    Column__ordinal -> Column
    Column__ordinal -> Column__ordinal [label="  *"]
    Card -> Project [color="red"]
    Card__text -> Card
    Card__text -> Card__text [label="  *"]
    CardDelete -> Card
    CardColumn -> Card
    CardColumn -> Column [color="red"]
    CardColumn -> CardColumn [label="  *"]
}
**/

namespace CardBoard
{
    public partial class Individual : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Individual newFact = new Individual(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._created = (DateTime)_fieldSerializerByType[typeof(DateTime)].ReadData(output);
						newFact._identifier = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Individual fact = (Individual)obj;
				_fieldSerializerByType[typeof(DateTime)].WriteData(output, fact._created);
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._identifier);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Individual.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Individual.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Individual", 2524);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Individual GetUnloadedInstance()
        {
            return new Individual((FactMemento)null) { IsLoaded = false };
        }

        public static Individual GetNullInstance()
        {
            return new Individual((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Individual> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Individual)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles

        // Queries
        private static Query _cacheQueryMemberships;

        public static Query GetQueryMemberships()
		{
            if (_cacheQueryMemberships == null)
            {
			    _cacheQueryMemberships = new Query()
    				.JoinSuccessors(Member.GetRoleIndividual(), Condition.WhereIsEmpty(Member.GetQueryIsDeleted())
				)
                ;
            }
            return _cacheQueryMemberships;
		}
        private static Query _cacheQueryProjects;

        public static Query GetQueryProjects()
		{
            if (_cacheQueryProjects == null)
            {
			    _cacheQueryProjects = new Query()
    				.JoinSuccessors(Member.GetRoleIndividual(), Condition.WhereIsEmpty(Member.GetQueryIsDeleted())
				)
		    		.JoinPredecessors(Member.GetRoleProject())
                ;
            }
            return _cacheQueryProjects;
		}

        // Predicates

        // Predecessors

        // Fields
        private DateTime _created;
        private string _identifier;

        // Results
        private Result<Member> _memberships;
        private Result<Project> _projects;

        // Business constructor
        public Individual(
            DateTime created
            ,string identifier
            )
        {
            InitializeResults();
            _created = created;
            _identifier = identifier;
        }

        // Hydration constructor
        private Individual(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _memberships = new Result<Member>(this, GetQueryMemberships(), Member.GetUnloadedInstance, Member.GetNullInstance);
            _projects = new Result<Project>(this, GetQueryProjects(), Project.GetUnloadedInstance, Project.GetNullInstance);
        }

        // Predecessor access

        // Field access
        public DateTime Created
        {
            get { return _created; }
        }
        public string Identifier
        {
            get { return _identifier; }
        }

        // Query result access
        public Result<Member> Memberships
        {
            get { return _memberships; }
        }
        public Result<Project> Projects
        {
            get { return _projects; }
        }

        // Mutable property access

    }
    
    public partial class Project : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Project newFact = new Project(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._created = (DateTime)_fieldSerializerByType[typeof(DateTime)].ReadData(output);
						newFact._identifier = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Project fact = (Project)obj;
				_fieldSerializerByType[typeof(DateTime)].WriteData(output, fact._created);
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._identifier);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Project.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Project.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Project", 2524);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Project GetUnloadedInstance()
        {
            return new Project((FactMemento)null) { IsLoaded = false };
        }

        public static Project GetNullInstance()
        {
            return new Project((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Project> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Project)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles

        // Queries
        private static Query _cacheQueryName;

        public static Query GetQueryName()
		{
            if (_cacheQueryName == null)
            {
			    _cacheQueryName = new Query()
    				.JoinSuccessors(Project__name.GetRoleProject(), Condition.WhereIsEmpty(Project__name.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryName;
		}
        private static Query _cacheQueryColumns;

        public static Query GetQueryColumns()
		{
            if (_cacheQueryColumns == null)
            {
			    _cacheQueryColumns = new Query()
		    		.JoinSuccessors(Column.GetRoleProject())
                ;
            }
            return _cacheQueryColumns;
		}
        private static Query _cacheQueryCards;

        public static Query GetQueryCards()
		{
            if (_cacheQueryCards == null)
            {
			    _cacheQueryCards = new Query()
		    		.JoinSuccessors(Column.GetRoleProject())
    				.JoinSuccessors(CardColumn.GetRoleColumn(), Condition.WhereIsEmpty(CardColumn.GetQueryIsCurrent())
				)
    				.JoinPredecessors(CardColumn.GetRoleCard(), Condition.WhereIsEmpty(Card.GetQueryIsDeleted())
				)
                ;
            }
            return _cacheQueryCards;
		}

        // Predicates

        // Predecessors

        // Fields
        private DateTime _created;
        private string _identifier;

        // Results
        private Result<Project__name> _name;
        private Result<Column> _columns;
        private Result<Card> _cards;

        // Business constructor
        public Project(
            DateTime created
            ,string identifier
            )
        {
            InitializeResults();
            _created = created;
            _identifier = identifier;
        }

        // Hydration constructor
        private Project(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _name = new Result<Project__name>(this, GetQueryName(), Project__name.GetUnloadedInstance, Project__name.GetNullInstance);
            _columns = new Result<Column>(this, GetQueryColumns(), Column.GetUnloadedInstance, Column.GetNullInstance);
            _cards = new Result<Card>(this, GetQueryCards(), Card.GetUnloadedInstance, Card.GetNullInstance);
        }

        // Predecessor access

        // Field access
        public DateTime Created
        {
            get { return _created; }
        }
        public string Identifier
        {
            get { return _identifier; }
        }

        // Query result access
        public Result<Column> Columns
        {
            get { return _columns; }
        }
        public Result<Card> Cards
        {
            get { return _cards; }
        }

        // Mutable property access
        public TransientDisputable<Project__name, string> Name
        {
            get { return _name.AsTransientDisputable(fact => fact.Value); }
			set
			{
                Community.Perform(async delegate()
                {
                    var current = (await _name.EnsureAsync()).ToList();
                    if (current.Count != 1 || !object.Equals(current[0].Value, value.Value))
                    {
                        await Community.AddFactAsync(new Project__name(this, _name, value.Value));
                    }
                });
			}
        }

    }
    
    public partial class Project__name : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Project__name newFact = new Project__name(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._value = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Project__name fact = (Project__name)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._value);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Project__name.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Project__name.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Project__name", 83639396);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Project__name GetUnloadedInstance()
        {
            return new Project__name((FactMemento)null) { IsLoaded = false };
        }

        public static Project__name GetNullInstance()
        {
            return new Project__name((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Project__name> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Project__name)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleProject;
        public static Role GetRoleProject()
        {
            if (_cacheRoleProject == null)
            {
                _cacheRoleProject = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "project",
			        Project._correspondenceFactType,
			        true));
            }
            return _cacheRoleProject;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        Project__name._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(Project__name.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<Project> _project;
        private PredecessorList<Project__name> _prior;

        // Fields
        private string _value;

        // Results

        // Business constructor
        public Project__name(
            Project project
            ,IEnumerable<Project__name> prior
            ,string value
            )
        {
            InitializeResults();
            _project = new PredecessorObj<Project>(this, GetRoleProject(), project);
            _prior = new PredecessorList<Project__name>(this, GetRolePrior(), prior);
            _value = value;
        }

        // Hydration constructor
        private Project__name(FactMemento memento)
        {
            InitializeResults();
            _project = new PredecessorObj<Project>(this, GetRoleProject(), memento, Project.GetUnloadedInstance, Project.GetNullInstance);
            _prior = new PredecessorList<Project__name>(this, GetRolePrior(), memento, Project__name.GetUnloadedInstance, Project__name.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Project Project
        {
            get { return IsNull ? Project.GetNullInstance() : _project.Fact; }
        }
        public PredecessorList<Project__name> Prior
        {
            get { return _prior; }
        }

        // Field access
        public string Value
        {
            get { return _value; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Member : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Member newFact = new Member(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Member fact = (Member)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Member.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Member.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Member", -1037170418);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Member GetUnloadedInstance()
        {
            return new Member((FactMemento)null) { IsLoaded = false };
        }

        public static Member GetNullInstance()
        {
            return new Member((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Member> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Member)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleIndividual;
        public static Role GetRoleIndividual()
        {
            if (_cacheRoleIndividual == null)
            {
                _cacheRoleIndividual = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "individual",
			        Individual._correspondenceFactType,
			        true));
            }
            return _cacheRoleIndividual;
        }
        private static Role _cacheRoleProject;
        public static Role GetRoleProject()
        {
            if (_cacheRoleProject == null)
            {
                _cacheRoleProject = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "project",
			        Project._correspondenceFactType,
			        false));
            }
            return _cacheRoleProject;
        }

        // Queries
        private static Query _cacheQueryIsDeleted;

        public static Query GetQueryIsDeleted()
		{
            if (_cacheQueryIsDeleted == null)
            {
			    _cacheQueryIsDeleted = new Query()
		    		.JoinSuccessors(MemberDelete.GetRoleMember())
                ;
            }
            return _cacheQueryIsDeleted;
		}

        // Predicates
        public static Condition IsDeleted = Condition.WhereIsNotEmpty(GetQueryIsDeleted());

        // Predecessors
        private PredecessorObj<Individual> _individual;
        private PredecessorObj<Project> _project;

        // Unique
        private Guid _unique;

        // Fields

        // Results

        // Business constructor
        public Member(
            Individual individual
            ,Project project
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, GetRoleIndividual(), individual);
            _project = new PredecessorObj<Project>(this, GetRoleProject(), project);
        }

        // Hydration constructor
        private Member(FactMemento memento)
        {
            InitializeResults();
            _individual = new PredecessorObj<Individual>(this, GetRoleIndividual(), memento, Individual.GetUnloadedInstance, Individual.GetNullInstance);
            _project = new PredecessorObj<Project>(this, GetRoleProject(), memento, Project.GetUnloadedInstance, Project.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Individual Individual
        {
            get { return IsNull ? Individual.GetNullInstance() : _individual.Fact; }
        }
        public Project Project
        {
            get { return IsNull ? Project.GetNullInstance() : _project.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access

        // Mutable property access

    }
    
    public partial class MemberDelete : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				MemberDelete newFact = new MemberDelete(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				MemberDelete fact = (MemberDelete)obj;
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return MemberDelete.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return MemberDelete.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.MemberDelete", -1787331760);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static MemberDelete GetUnloadedInstance()
        {
            return new MemberDelete((FactMemento)null) { IsLoaded = false };
        }

        public static MemberDelete GetNullInstance()
        {
            return new MemberDelete((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<MemberDelete> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (MemberDelete)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleMember;
        public static Role GetRoleMember()
        {
            if (_cacheRoleMember == null)
            {
                _cacheRoleMember = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "member",
			        Member._correspondenceFactType,
			        false));
            }
            return _cacheRoleMember;
        }

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Member> _member;

        // Fields

        // Results

        // Business constructor
        public MemberDelete(
            Member member
            )
        {
            InitializeResults();
            _member = new PredecessorObj<Member>(this, GetRoleMember(), member);
        }

        // Hydration constructor
        private MemberDelete(FactMemento memento)
        {
            InitializeResults();
            _member = new PredecessorObj<Member>(this, GetRoleMember(), memento, Member.GetUnloadedInstance, Member.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Member Member
        {
            get { return IsNull ? Member.GetNullInstance() : _member.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class Column : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Column newFact = new Column(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Column fact = (Column)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Column.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Column.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Column", 2027656806);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Column GetUnloadedInstance()
        {
            return new Column((FactMemento)null) { IsLoaded = false };
        }

        public static Column GetNullInstance()
        {
            return new Column((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Column> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Column)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleProject;
        public static Role GetRoleProject()
        {
            if (_cacheRoleProject == null)
            {
                _cacheRoleProject = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "project",
			        Project._correspondenceFactType,
			        true));
            }
            return _cacheRoleProject;
        }

        // Queries
        private static Query _cacheQueryName;

        public static Query GetQueryName()
		{
            if (_cacheQueryName == null)
            {
			    _cacheQueryName = new Query()
    				.JoinSuccessors(Column__name.GetRoleColumn(), Condition.WhereIsEmpty(Column__name.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryName;
		}
        private static Query _cacheQueryOrdinal;

        public static Query GetQueryOrdinal()
		{
            if (_cacheQueryOrdinal == null)
            {
			    _cacheQueryOrdinal = new Query()
    				.JoinSuccessors(Column__ordinal.GetRoleColumn(), Condition.WhereIsEmpty(Column__ordinal.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryOrdinal;
		}
        private static Query _cacheQueryCards;

        public static Query GetQueryCards()
		{
            if (_cacheQueryCards == null)
            {
			    _cacheQueryCards = new Query()
    				.JoinSuccessors(CardColumn.GetRoleColumn(), Condition.WhereIsEmpty(CardColumn.GetQueryIsCurrent())
				)
    				.JoinPredecessors(CardColumn.GetRoleCard(), Condition.WhereIsEmpty(Card.GetQueryIsDeleted())
				)
                ;
            }
            return _cacheQueryCards;
		}

        // Predicates

        // Predecessors
        private PredecessorObj<Project> _project;

        // Unique
        private Guid _unique;

        // Fields

        // Results
        private Result<Column__name> _name;
        private Result<Column__ordinal> _ordinal;
        private Result<Card> _cards;

        // Business constructor
        public Column(
            Project project
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _project = new PredecessorObj<Project>(this, GetRoleProject(), project);
        }

        // Hydration constructor
        private Column(FactMemento memento)
        {
            InitializeResults();
            _project = new PredecessorObj<Project>(this, GetRoleProject(), memento, Project.GetUnloadedInstance, Project.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
            _name = new Result<Column__name>(this, GetQueryName(), Column__name.GetUnloadedInstance, Column__name.GetNullInstance);
            _ordinal = new Result<Column__ordinal>(this, GetQueryOrdinal(), Column__ordinal.GetUnloadedInstance, Column__ordinal.GetNullInstance);
            _cards = new Result<Card>(this, GetQueryCards(), Card.GetUnloadedInstance, Card.GetNullInstance);
        }

        // Predecessor access
        public Project Project
        {
            get { return IsNull ? Project.GetNullInstance() : _project.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access
        public Result<Card> Cards
        {
            get { return _cards; }
        }

        // Mutable property access
        public TransientDisputable<Column__name, string> Name
        {
            get { return _name.AsTransientDisputable(fact => fact.Value); }
			set
			{
                Community.Perform(async delegate()
                {
                    var current = (await _name.EnsureAsync()).ToList();
                    if (current.Count != 1 || !object.Equals(current[0].Value, value.Value))
                    {
                        await Community.AddFactAsync(new Column__name(this, _name, value.Value));
                    }
                });
			}
        }
        public TransientDisputable<Column__ordinal, int> Ordinal
        {
            get { return _ordinal.AsTransientDisputable(fact => fact.Value); }
			set
			{
                Community.Perform(async delegate()
                {
                    var current = (await _ordinal.EnsureAsync()).ToList();
                    if (current.Count != 1 || !object.Equals(current[0].Value, value.Value))
                    {
                        await Community.AddFactAsync(new Column__ordinal(this, _ordinal, value.Value));
                    }
                });
			}
        }

    }
    
    public partial class Column__name : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Column__name newFact = new Column__name(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._value = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Column__name fact = (Column__name)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._value);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Column__name.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Column__name.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Column__name", -1893013376);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Column__name GetUnloadedInstance()
        {
            return new Column__name((FactMemento)null) { IsLoaded = false };
        }

        public static Column__name GetNullInstance()
        {
            return new Column__name((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Column__name> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Column__name)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleColumn;
        public static Role GetRoleColumn()
        {
            if (_cacheRoleColumn == null)
            {
                _cacheRoleColumn = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "column",
			        Column._correspondenceFactType,
			        false));
            }
            return _cacheRoleColumn;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        Column__name._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(Column__name.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<Column> _column;
        private PredecessorList<Column__name> _prior;

        // Fields
        private string _value;

        // Results

        // Business constructor
        public Column__name(
            Column column
            ,IEnumerable<Column__name> prior
            ,string value
            )
        {
            InitializeResults();
            _column = new PredecessorObj<Column>(this, GetRoleColumn(), column);
            _prior = new PredecessorList<Column__name>(this, GetRolePrior(), prior);
            _value = value;
        }

        // Hydration constructor
        private Column__name(FactMemento memento)
        {
            InitializeResults();
            _column = new PredecessorObj<Column>(this, GetRoleColumn(), memento, Column.GetUnloadedInstance, Column.GetNullInstance);
            _prior = new PredecessorList<Column__name>(this, GetRolePrior(), memento, Column__name.GetUnloadedInstance, Column__name.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Column Column
        {
            get { return IsNull ? Column.GetNullInstance() : _column.Fact; }
        }
        public PredecessorList<Column__name> Prior
        {
            get { return _prior; }
        }

        // Field access
        public string Value
        {
            get { return _value; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Column__ordinal : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Column__ordinal newFact = new Column__ordinal(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._value = (int)_fieldSerializerByType[typeof(int)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Column__ordinal fact = (Column__ordinal)obj;
				_fieldSerializerByType[typeof(int)].WriteData(output, fact._value);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Column__ordinal.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Column__ordinal.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Column__ordinal", -1893013364);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Column__ordinal GetUnloadedInstance()
        {
            return new Column__ordinal((FactMemento)null) { IsLoaded = false };
        }

        public static Column__ordinal GetNullInstance()
        {
            return new Column__ordinal((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Column__ordinal> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Column__ordinal)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleColumn;
        public static Role GetRoleColumn()
        {
            if (_cacheRoleColumn == null)
            {
                _cacheRoleColumn = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "column",
			        Column._correspondenceFactType,
			        false));
            }
            return _cacheRoleColumn;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        Column__ordinal._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(Column__ordinal.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<Column> _column;
        private PredecessorList<Column__ordinal> _prior;

        // Fields
        private int _value;

        // Results

        // Business constructor
        public Column__ordinal(
            Column column
            ,IEnumerable<Column__ordinal> prior
            ,int value
            )
        {
            InitializeResults();
            _column = new PredecessorObj<Column>(this, GetRoleColumn(), column);
            _prior = new PredecessorList<Column__ordinal>(this, GetRolePrior(), prior);
            _value = value;
        }

        // Hydration constructor
        private Column__ordinal(FactMemento memento)
        {
            InitializeResults();
            _column = new PredecessorObj<Column>(this, GetRoleColumn(), memento, Column.GetUnloadedInstance, Column.GetNullInstance);
            _prior = new PredecessorList<Column__ordinal>(this, GetRolePrior(), memento, Column__ordinal.GetUnloadedInstance, Column__ordinal.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Column Column
        {
            get { return IsNull ? Column.GetNullInstance() : _column.Fact; }
        }
        public PredecessorList<Column__ordinal> Prior
        {
            get { return _prior; }
        }

        // Field access
        public int Value
        {
            get { return _value; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Card : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Card newFact = new Card(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
						newFact._created = (DateTime)_fieldSerializerByType[typeof(DateTime)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Card fact = (Card)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
				_fieldSerializerByType[typeof(DateTime)].WriteData(output, fact._created);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Card.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Card.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Card", 2008857786);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Card GetUnloadedInstance()
        {
            return new Card((FactMemento)null) { IsLoaded = false };
        }

        public static Card GetNullInstance()
        {
            return new Card((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Card> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Card)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleProject;
        public static Role GetRoleProject()
        {
            if (_cacheRoleProject == null)
            {
                _cacheRoleProject = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "project",
			        Project._correspondenceFactType,
			        true));
            }
            return _cacheRoleProject;
        }

        // Queries
        private static Query _cacheQueryText;

        public static Query GetQueryText()
		{
            if (_cacheQueryText == null)
            {
			    _cacheQueryText = new Query()
    				.JoinSuccessors(Card__text.GetRoleCard(), Condition.WhereIsEmpty(Card__text.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryText;
		}
        private static Query _cacheQueryCardColumns;

        public static Query GetQueryCardColumns()
		{
            if (_cacheQueryCardColumns == null)
            {
			    _cacheQueryCardColumns = new Query()
    				.JoinSuccessors(CardColumn.GetRoleCard(), Condition.WhereIsEmpty(CardColumn.GetQueryIsCurrent())
				)
                ;
            }
            return _cacheQueryCardColumns;
		}
        private static Query _cacheQueryIsDeleted;

        public static Query GetQueryIsDeleted()
		{
            if (_cacheQueryIsDeleted == null)
            {
			    _cacheQueryIsDeleted = new Query()
		    		.JoinSuccessors(CardDelete.GetRoleCard())
                ;
            }
            return _cacheQueryIsDeleted;
		}

        // Predicates
        public static Condition IsDeleted = Condition.WhereIsNotEmpty(GetQueryIsDeleted());

        // Predecessors
        private PredecessorObj<Project> _project;

        // Unique
        private Guid _unique;

        // Fields
        private DateTime _created;

        // Results
        private Result<Card__text> _text;
        private Result<CardColumn> _cardColumns;

        // Business constructor
        public Card(
            Project project
            ,DateTime created
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _project = new PredecessorObj<Project>(this, GetRoleProject(), project);
            _created = created;
        }

        // Hydration constructor
        private Card(FactMemento memento)
        {
            InitializeResults();
            _project = new PredecessorObj<Project>(this, GetRoleProject(), memento, Project.GetUnloadedInstance, Project.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
            _text = new Result<Card__text>(this, GetQueryText(), Card__text.GetUnloadedInstance, Card__text.GetNullInstance);
            _cardColumns = new Result<CardColumn>(this, GetQueryCardColumns(), CardColumn.GetUnloadedInstance, CardColumn.GetNullInstance);
        }

        // Predecessor access
        public Project Project
        {
            get { return IsNull ? Project.GetNullInstance() : _project.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }

        public DateTime Created
        {
            get { return _created; }
        }

        // Query result access
        public Result<CardColumn> CardColumns
        {
            get { return _cardColumns; }
        }

        // Mutable property access
        public TransientDisputable<Card__text, string> Text
        {
            get { return _text.AsTransientDisputable(fact => fact.Value); }
			set
			{
                Community.Perform(async delegate()
                {
                    var current = (await _text.EnsureAsync()).ToList();
                    if (current.Count != 1 || !object.Equals(current[0].Value, value.Value))
                    {
                        await Community.AddFactAsync(new Card__text(this, _text, value.Value));
                    }
                });
			}
        }

    }
    
    public partial class Card__text : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Card__text newFact = new Card__text(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._value = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Card__text fact = (Card__text)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._value);
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return Card__text.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return Card__text.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.Card__text", -1707909704);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static Card__text GetUnloadedInstance()
        {
            return new Card__text((FactMemento)null) { IsLoaded = false };
        }

        public static Card__text GetNullInstance()
        {
            return new Card__text((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<Card__text> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (Card__text)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleCard;
        public static Role GetRoleCard()
        {
            if (_cacheRoleCard == null)
            {
                _cacheRoleCard = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "card",
			        Card._correspondenceFactType,
			        false));
            }
            return _cacheRoleCard;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        Card__text._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(Card__text.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());

        // Predecessors
        private PredecessorObj<Card> _card;
        private PredecessorList<Card__text> _prior;

        // Fields
        private string _value;

        // Results

        // Business constructor
        public Card__text(
            Card card
            ,IEnumerable<Card__text> prior
            ,string value
            )
        {
            InitializeResults();
            _card = new PredecessorObj<Card>(this, GetRoleCard(), card);
            _prior = new PredecessorList<Card__text>(this, GetRolePrior(), prior);
            _value = value;
        }

        // Hydration constructor
        private Card__text(FactMemento memento)
        {
            InitializeResults();
            _card = new PredecessorObj<Card>(this, GetRoleCard(), memento, Card.GetUnloadedInstance, Card.GetNullInstance);
            _prior = new PredecessorList<Card__text>(this, GetRolePrior(), memento, Card__text.GetUnloadedInstance, Card__text.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Card Card
        {
            get { return IsNull ? Card.GetNullInstance() : _card.Fact; }
        }
        public PredecessorList<Card__text> Prior
        {
            get { return _prior; }
        }

        // Field access
        public string Value
        {
            get { return _value; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class CardDelete : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				CardDelete newFact = new CardDelete(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				CardDelete fact = (CardDelete)obj;
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return CardDelete.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return CardDelete.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.CardDelete", 300831704);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static CardDelete GetUnloadedInstance()
        {
            return new CardDelete((FactMemento)null) { IsLoaded = false };
        }

        public static CardDelete GetNullInstance()
        {
            return new CardDelete((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<CardDelete> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (CardDelete)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleCard;
        public static Role GetRoleCard()
        {
            if (_cacheRoleCard == null)
            {
                _cacheRoleCard = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "card",
			        Card._correspondenceFactType,
			        false));
            }
            return _cacheRoleCard;
        }

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Card> _card;

        // Fields

        // Results

        // Business constructor
        public CardDelete(
            Card card
            )
        {
            InitializeResults();
            _card = new PredecessorObj<Card>(this, GetRoleCard(), card);
        }

        // Hydration constructor
        private CardDelete(FactMemento memento)
        {
            InitializeResults();
            _card = new PredecessorObj<Card>(this, GetRoleCard(), memento, Card.GetUnloadedInstance, Card.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Card Card
        {
            get { return IsNull ? Card.GetNullInstance() : _card.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class CardColumn : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				CardColumn newFact = new CardColumn(memento);


				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				CardColumn fact = (CardColumn)obj;
			}

            public CorrespondenceFact GetUnloadedInstance()
            {
                return CardColumn.GetUnloadedInstance();
            }

            public CorrespondenceFact GetNullInstance()
            {
                return CardColumn.GetNullInstance();
            }
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"CardBoard.CardColumn", 631381956);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Null and unloaded instances
        public static CardColumn GetUnloadedInstance()
        {
            return new CardColumn((FactMemento)null) { IsLoaded = false };
        }

        public static CardColumn GetNullInstance()
        {
            return new CardColumn((FactMemento)null) { IsNull = true };
        }

        // Ensure
        public Task<CardColumn> EnsureAsync()
        {
            if (_loadedTask != null)
                return _loadedTask.ContinueWith(t => (CardColumn)t.Result);
            else
                return Task.FromResult(this);
        }

        // Roles
        private static Role _cacheRoleCard;
        public static Role GetRoleCard()
        {
            if (_cacheRoleCard == null)
            {
                _cacheRoleCard = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "card",
			        Card._correspondenceFactType,
			        false));
            }
            return _cacheRoleCard;
        }
        private static Role _cacheRoleColumn;
        public static Role GetRoleColumn()
        {
            if (_cacheRoleColumn == null)
            {
                _cacheRoleColumn = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "column",
			        Column._correspondenceFactType,
			        true));
            }
            return _cacheRoleColumn;
        }
        private static Role _cacheRolePrior;
        public static Role GetRolePrior()
        {
            if (_cacheRolePrior == null)
            {
                _cacheRolePrior = new Role(new RoleMemento(
			        _correspondenceFactType,
			        "prior",
			        CardColumn._correspondenceFactType,
			        false));
            }
            return _cacheRolePrior;
        }

        // Queries
        private static Query _cacheQueryIsCurrent;

        public static Query GetQueryIsCurrent()
		{
            if (_cacheQueryIsCurrent == null)
            {
			    _cacheQueryIsCurrent = new Query()
		    		.JoinSuccessors(CardColumn.GetRolePrior())
                ;
            }
            return _cacheQueryIsCurrent;
		}
        private static Query _cacheQueryIsDeleted;

        public static Query GetQueryIsDeleted()
		{
            if (_cacheQueryIsDeleted == null)
            {
			    _cacheQueryIsDeleted = new Query()
    				.JoinPredecessors(CardColumn.GetRoleCard(), Condition.WhereIsNotEmpty(Card.GetQueryIsDeleted())
				)
                ;
            }
            return _cacheQueryIsDeleted;
		}

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(GetQueryIsCurrent());
        public static Condition IsDeleted = Condition.WhereIsNotEmpty(GetQueryIsDeleted());

        // Predecessors
        private PredecessorObj<Card> _card;
        private PredecessorObj<Column> _column;
        private PredecessorList<CardColumn> _prior;

        // Fields

        // Results

        // Business constructor
        public CardColumn(
            Card card
            ,Column column
            ,IEnumerable<CardColumn> prior
            )
        {
            InitializeResults();
            _card = new PredecessorObj<Card>(this, GetRoleCard(), card);
            _column = new PredecessorObj<Column>(this, GetRoleColumn(), column);
            _prior = new PredecessorList<CardColumn>(this, GetRolePrior(), prior);
        }

        // Hydration constructor
        private CardColumn(FactMemento memento)
        {
            InitializeResults();
            _card = new PredecessorObj<Card>(this, GetRoleCard(), memento, Card.GetUnloadedInstance, Card.GetNullInstance);
            _column = new PredecessorObj<Column>(this, GetRoleColumn(), memento, Column.GetUnloadedInstance, Column.GetNullInstance);
            _prior = new PredecessorList<CardColumn>(this, GetRolePrior(), memento, CardColumn.GetUnloadedInstance, CardColumn.GetNullInstance);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Card Card
        {
            get { return IsNull ? Card.GetNullInstance() : _card.Fact; }
        }
        public Column Column
        {
            get { return IsNull ? Column.GetNullInstance() : _column.Fact; }
        }
        public PredecessorList<CardColumn> Prior
        {
            get { return _prior; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    

	public class CorrespondenceModel : ICorrespondenceModel
	{
		public void RegisterAllFactTypes(Community community, IDictionary<Type, IFieldSerializer> fieldSerializerByType)
		{
			community.AddType(
				Individual._correspondenceFactType,
				new Individual.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Individual._correspondenceFactType }));
			community.AddQuery(
				Individual._correspondenceFactType,
				Individual.GetQueryMemberships().QueryDefinition);
			community.AddQuery(
				Individual._correspondenceFactType,
				Individual.GetQueryProjects().QueryDefinition);
			community.AddType(
				Project._correspondenceFactType,
				new Project.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Project._correspondenceFactType }));
			community.AddQuery(
				Project._correspondenceFactType,
				Project.GetQueryName().QueryDefinition);
			community.AddQuery(
				Project._correspondenceFactType,
				Project.GetQueryColumns().QueryDefinition);
			community.AddQuery(
				Project._correspondenceFactType,
				Project.GetQueryCards().QueryDefinition);
			community.AddType(
				Project__name._correspondenceFactType,
				new Project__name.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Project__name._correspondenceFactType }));
			community.AddQuery(
				Project__name._correspondenceFactType,
				Project__name.GetQueryIsCurrent().QueryDefinition);
			community.AddType(
				Member._correspondenceFactType,
				new Member.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Member._correspondenceFactType }));
			community.AddQuery(
				Member._correspondenceFactType,
				Member.GetQueryIsDeleted().QueryDefinition);
			community.AddType(
				MemberDelete._correspondenceFactType,
				new MemberDelete.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { MemberDelete._correspondenceFactType }));
			community.AddType(
				Column._correspondenceFactType,
				new Column.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Column._correspondenceFactType }));
			community.AddQuery(
				Column._correspondenceFactType,
				Column.GetQueryName().QueryDefinition);
			community.AddQuery(
				Column._correspondenceFactType,
				Column.GetQueryOrdinal().QueryDefinition);
			community.AddQuery(
				Column._correspondenceFactType,
				Column.GetQueryCards().QueryDefinition);
			community.AddType(
				Column__name._correspondenceFactType,
				new Column__name.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Column__name._correspondenceFactType }));
			community.AddQuery(
				Column__name._correspondenceFactType,
				Column__name.GetQueryIsCurrent().QueryDefinition);
			community.AddType(
				Column__ordinal._correspondenceFactType,
				new Column__ordinal.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Column__ordinal._correspondenceFactType }));
			community.AddQuery(
				Column__ordinal._correspondenceFactType,
				Column__ordinal.GetQueryIsCurrent().QueryDefinition);
			community.AddType(
				Card._correspondenceFactType,
				new Card.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Card._correspondenceFactType }));
			community.AddQuery(
				Card._correspondenceFactType,
				Card.GetQueryText().QueryDefinition);
			community.AddQuery(
				Card._correspondenceFactType,
				Card.GetQueryCardColumns().QueryDefinition);
			community.AddQuery(
				Card._correspondenceFactType,
				Card.GetQueryIsDeleted().QueryDefinition);
			community.AddType(
				Card__text._correspondenceFactType,
				new Card__text.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Card__text._correspondenceFactType }));
			community.AddQuery(
				Card__text._correspondenceFactType,
				Card__text.GetQueryIsCurrent().QueryDefinition);
			community.AddType(
				CardDelete._correspondenceFactType,
				new CardDelete.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { CardDelete._correspondenceFactType }));
			community.AddType(
				CardColumn._correspondenceFactType,
				new CardColumn.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { CardColumn._correspondenceFactType }));
			community.AddQuery(
				CardColumn._correspondenceFactType,
				CardColumn.GetQueryIsCurrent().QueryDefinition);
			community.AddQuery(
				CardColumn._correspondenceFactType,
				CardColumn.GetQueryIsDeleted().QueryDefinition);
		}
	}
}
