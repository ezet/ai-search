using System.Collections.Generic;

namespace eZet.AStar {
    public interface INode {

        IEnumerable<INode> Neighbours { get; }

    }
}
