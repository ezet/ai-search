using System.Collections.Generic;

namespace eZet.AStar {
    public interface ISolvable {

        INode GetStartNode { get; }

        bool IsSolution(INode node);

        double Cost(INode lastNode, INode node);

        double Estimate(INode node);

        IEnumerable<INode> GetNeighbours(INode node);
    }
}
