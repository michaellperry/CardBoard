using System.Collections.Generic;
using System.Linq;
using UpdateControls.Correspondence;

namespace CardBoard.Board.ViewModels
{
    public class ProjectDetailViewModel
    {
        private readonly Community _community;
        private readonly Individual _individual;
        private readonly Project _project;

        public ProjectDetailViewModel(Community community, Individual individual, Project project)
        {
            _community = community;
            _individual = individual;
            _project = project;
        }

        public IEnumerable<CardViewModel> ToDoCards
        {
            get
            {
                var column = _project.Columns.FirstOrDefault(c => c.Name.Value == "To Do");
                if (column == null)
                    return Enumerable.Empty<CardViewModel>();

                return
                    from card in column.Cards
                    orderby card.Created
                    select new CardViewModel(card);
            }
        }

        public IEnumerable<CardViewModel> DoingCards
        {
            get
            {
                var column = _project.Columns.FirstOrDefault(c => c.Name.Value == "Doing");
                if (column == null)
                    return Enumerable.Empty<CardViewModel>();

                return
                    from card in column.Cards
                    orderby card.Created
                    select new CardViewModel(card);
            }
        }

        public IEnumerable<CardViewModel> DoneCards
        {
            get
            {
                var column = _project.Columns.FirstOrDefault(c => c.Name.Value == "Done");
                if (column == null)
                    return Enumerable.Empty<CardViewModel>();

                return
                    from card in column.Cards
                    orderby card.Created
                    select new CardViewModel(card);
            }
        }
    }
}
