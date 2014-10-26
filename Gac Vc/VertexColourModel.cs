using System.Collections.Generic;
using eZet.Csp.Constraints;

namespace eZet.Csp.VertexColouring {
    /// <summary>
    /// Represents a graph
    /// </summary>
    public class VertexColourModel : CspModel {


        /// <summary>
        /// Gets all nodes the nodes in the graph
        /// </summary>
        public IEnumerable<IVariable> Nodes { get; private set; }

        public IEnumerable<IDomainValue> DomainValues { get; private set; }

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
        public VertexColourModel(IEnumerable<IVariable> nodes, IEnumerable<IEdge> edges) {
            Nodes = nodes;
            Edges = edges;
            Constraints = new List<IConstraint>();
            foreach (var edge in Edges) {
                Constraints.Add(new BinaryConstraint(new[] { edge.Node1, edge.Node2 },
                    "@" + edge.Node1.Identifier + " != " + "@" + edge.Node2.Identifier));
            }
  
        }

        public void Initialize(int domainValueCount) {
            var list = new List<IDomainValue>();
            for (int i = 0; i < domainValueCount; ++i) {
                list.Add(new DomainValue(i));
            }
            DomainValues = list;
            foreach (var node in Nodes) {
                node.SetValues(list);
            }
        }
    }
}