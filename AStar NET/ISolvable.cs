using System.Security.Cryptography.X509Certificates;

namespace eZet.AStar {
    public interface ISolvable {

        INode GetStartNode { get; }


        bool IsSolution(INode node);
        double Cost(INode lastNode, INode node);
        double Estimate(INode node);
    }
}
