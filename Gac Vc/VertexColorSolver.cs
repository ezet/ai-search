using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using eZet.AStar;
using VertexColouring;

namespace eZet.Csp.VertexColouring {
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
        public static VertexColourModel Load(string file) {
            var data = File.ReadAllLines(file);
            return Parse(data);
        }

        /// <summary>
        /// Solves a vertex coloring problem
        /// </summary>
        /// <param name="model"></param>
        /// <param name="numDomainValues"></param>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public GacSolvable.Result Solve(VertexColourModel model, int numDomainValues, ISearchAlgorithm algorithm) {
            model.Initialize(numDomainValues);
            GacSolvable = new GacSolvable(model);
            GacSolvable.Algorithm = algorithm;
            return GacSolvable.Solve();
        }

        /// <summary>
        /// Gets the statistics for the last completed run
        /// </summary>
        /// <returns></returns>
        public VertexColorResult GetStatistics() {
            var result = new VertexColorResult();
            result.UnsatisfiedConstraints = GacSolvable.AppliedStates.Sum(s => s.UnsatisfiedConstraints.Count);
            result.UnassignedVariables = GacSolvable.Model.Nodes.Count(n => !n.DomainValues.Any());
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
        private static VertexColourModel Parse(IList<string> data) {
            var nv = int.Parse(data[0].Split(' ')[0]);
            var ne = int.Parse(data[0].Split(' ')[1]);

            var nodes = new List<IVariable>(nv);
            foreach (var line in data.Skip(1).Take(nv)) {
                var node = line.Trim().Split(' ').ToList();
                nodes.Add(new GridVariable(node[0], double.Parse(node[1], CultureInfo.InvariantCulture),
                    double.Parse(node[2], CultureInfo.InvariantCulture)));
            }
            var edges = new List<IEdge>(ne);
            foreach (var line in data.Skip(nv + 1).Take(ne)) {
                var edge = line.Trim().Split(' ').Select(int.Parse).ToList();
                edges.Add(new GraphEdge(nodes[edge[0]], nodes[edge[1]]));
            }


            return new VertexColourModel(nodes, edges);
        }


    }
}