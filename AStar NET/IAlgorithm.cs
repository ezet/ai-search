using System.Collections.Generic;
using System.Threading;

namespace eZet.AStar {
    public interface IAlgorithm {

        Path Run(ISolvable solvable);

        int Throttle { get; set; }

        IList<INode> ExpandedNodes { get; }

        CancellationToken CancellationToken { get; set; }

    }
}