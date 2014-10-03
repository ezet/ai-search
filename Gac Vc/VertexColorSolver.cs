using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using eZet.AStar;
using eZet.Csp.Constraints;

namespace eZet.Csp.GraphColouring {
    /// <summary>
    /// A helper class for sovling Vertex Coloring problems
    /// </summary>
    public class VertexColorSolver {
        /// <summary>
        /// Gets the current instance of GacSolvable 
        /// </summary>
        public GacSolvable GacSolvable { get; private set; }

        /// <summary>
        /// Loads a Graph from a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Graph Load(string file) {
            var data = File.ReadAllLines(file);
            return Parse(data);
        }

        /// <summary>
        /// Solves a vertex coloring problem
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="numDomainValues"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public GacSolvable.Result Solve(Graph graph, int numDomainValues, ISearchAlgorithm algorithm) {
            var constraints = createConstraints(graph.Edges);
            var domain = new List<IDomainValue>();
            for (int i = 0; i < numDomainValues; ++i) {
                domain.Add(new DomainValue(i));
            }
            GacSolvable = new GacSolvable(graph, domain);
            GacSolvable.Algorithm = algorithm;
            GacSolvable.AddConstraints(constraints);
            return GacSolvable.Solve();
        }

        /// <summary>
        /// Gets the statistics for the last completed run
        /// </summary>
        /// <returns></returns>
        public VertexColorResult GetStatistics() {
            var result = new VertexColorResult();
            result.UnsatisfiedConstraints = GacSolvable.AppliedStates.Sum(s => s.UnsatisfiedConstraints.Count);
            result.UnassignedVariables = GacSolvable.Graph.Nodes.Count(n => !n.DomainValues.Any());
            result.SearchNodeCount = GacSolvable.Algorithm.SearchNodeCount();
            result.ExpandedNodeCount = GacSolvable.Algorithm.ExpandedNodes.Count;
            result.SolutionSearchLength = GacSolvable.SearchPath.Count() - 1;
            return result;
        }

        /// <summary>
        /// Parses a data string to a Graph
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Graph Parse(IList<string> data) {
            var nv = int.Parse(data[0].Split(' ')[0]);
            var ne = int.Parse(data[0].Split(' ')[1]);

            var nodes = new List<IVariable>(nv);
            foreach (var line in data.Skip(1).Take(nv)) {
                var node = line.Trim().Split(' ').ToList();
                nodes.Add(new SimpleVariable(node[0], double.Parse(node[1], CultureInfo.InvariantCulture),
                    double.Parse(node[2], CultureInfo.InvariantCulture)));
            }
            var edges = new List<IEdge>(ne);
            foreach (var line in data.Skip(nv + 1).Take(ne)) {
                var edge = line.Trim().Split(' ').Select(int.Parse).ToList();
                edges.Add(new GraphEdge(nodes[edge[0]], nodes[edge[1]]));
            }
            return new Graph(nodes, edges);
        }

        /// <summary>
        /// Creates vertex coloring constraints from a list of edges
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        private IEnumerable<IConstraint> createConstraints(IEnumerable<IEdge> edges) {
            var constraints = new List<IConstraint>();
            foreach (var edge in edges) {
                constraints.Add(new BinaryConstraint(new[] {edge.Node1, edge.Node2},
                    "@" + edge.Node1.Identifier + " != " + "@" + edge.Node2.Identifier));
            }
            return constraints;
        }
    }
}