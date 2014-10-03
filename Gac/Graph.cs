using System.Collections.Generic;
using eZet.Csp.Constraints;

namespace eZet.Csp {
    /// <summary>
    /// Represents a graph
    /// </summary>
    public class Graph {

        /// <summary>
        /// Gets all nodes the nodes in the graph
        /// </summary>
        public IEnumerable<IVariable> Nodes { get; private set; }

        /// <summary>
        /// Gets all the edges in the graph
        /// </summary>
        public IEnumerable<IEdge> Edges { get; private set; }

        public List<IConstraint> Constraints { get; private set; }

        /// <summary>
        /// Creates a new graph
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="edges"></param>
        public Graph(IEnumerable<IVariable> nodes, IEnumerable<IEdge> edges) {
            Nodes = nodes;
            Edges = edges;
        }
    }
}