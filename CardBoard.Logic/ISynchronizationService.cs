using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateControls.Correspondence;
using CardBoard.Model;

namespace CardBoard
{
    public interface ISynchronizationService
    {
        Project Project { get; set; }
        void Initialize();
        Community Community { get; }
        Individual Individual { get; }
        void Synchronize();
    }
}
