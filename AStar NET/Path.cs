using System.Collections;
using System.Collections.Generic;

namespace eZet.AStar {
    public class Path : IEnumerable<INode> {

        public INode CurrentNode { get; private set; }

        public Path PreviousNodes { get; private set; }

        public double TotalCost { get; private set; }

        private Path(INode currentNode, Path previousNodes, double totalCost) {
            CurrentNode = currentNode;
            PreviousNodes = previousNodes;
            TotalCost = totalCost;
        }

        public Path(INode start)
            : this(start, null, 0) {
        }

        public Path AddNode(INode node, double edgeCost) {
            return new Path(node, this, TotalCost + edgeCost);
        }

        public IEnumerator<INode> GetEnumerator() {
            for (Path p = this; p != null; p = p.PreviousNodes) {
                yield return p.CurrentNode;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
