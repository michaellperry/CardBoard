using CardBoard.Model;
using UpdateControls.Correspondence;

namespace CardBoard
{
    public interface ISynchronizationService
    {
        Project Project { get; set; }
        Community Community { get; }
        Individual Individual { get; }
    }
}
