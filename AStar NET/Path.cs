using System.Collections;
using System.Collections.Generic;

namespace eZet.AStar {
    public class Path : IEnumerable<INode> {

        public INode Node { get; private set; }

        public Path PreviousStep { get; private set; }

        public double TotalCost { get; private set; }

        private Path(INode node, Path previousStep, double totalCost) {
            Node = node;
            PreviousStep = previousStep;
            TotalCost = totalCost;
        }

        public Path(INode start)
            : this(start, null, 0) {
        }

        public Path AddNode(INode node, double edgeCost) {
            return new Path(node, this, TotalCost + edgeCost);
        }

        public IEnumerator<INode> GetEnumerator() {
            for (Path p = this; p != null; p = p.PreviousStep) {
                yield return p.Node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
