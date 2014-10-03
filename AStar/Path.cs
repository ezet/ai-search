using System.Collections;
using System.Collections.Generic;

namespace eZet.AStar {
    /// <summary>
    /// Represents a path between two nodes
    /// </summary>
    public class Path : IEnumerable<ISearchNode> {
        /// <summary>
        /// Gets the node this edge leads to
        /// </summary>
        public ISearchNode Node { get; private set; }

        /// <summary>
        /// Gets the node this path came from
        /// </summary>
        public Path PreviousStep { get; private set; }

        /// <summary>
        /// Gets the total cost of transversing to this node, from the start
        /// </summary>
        public double TotalCost { get; private set; }

        /// <summary>
        /// Constructor for adding another node to the path
        /// </summary>
        /// <param name="node"></param>
        /// <param name="previousStep"></param>
        /// <param name="totalCost"></param>
        private Path(ISearchNode node, Path previousStep, double totalCost) {
            Node = node;
            PreviousStep = previousStep;
            TotalCost = totalCost;
        }

        /// <summary>
        /// Creates a new path
        /// </summary>
        /// <param name="start"></param>
        public Path(ISearchNode start)
            : this(start, null, 0) {
        }

        /// <summary>
        /// Adds a new node to the end of the path
        /// </summary>
        /// <param name="node">The ISearchNode to add</param>
        /// <param name="edgeCost">The cost of moving from the previous node to the input node</param>
        /// <returns></returns>
        public Path AddNode(ISearchNode node, double edgeCost) {
            return new Path(node, this, TotalCost + edgeCost);
        }

        /// <summary>
        /// Returns an enumerator for enumerating all nodes in the path
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ISearchNode> GetEnumerator() {
            for (Path p = this; p != null; p = p.PreviousStep) {
                yield return p.Node;
            }
        }

        /// <summary>
        /// Returns an enumerator for enumerating all nodes in the path
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}