using System.Threading;

namespace eZet.AStar {
    /// <summary>
    /// A helper class that can can solve an ISearchSolvable using an ISearchAlgorithm
    /// </summary>
    public class SearchSolver {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="algorithm"></param>
        public SearchSolver(ISearchAlgorithm algorithm) {
            Algorithm = algorithm;
        }

        /// <summary>
        /// Gets or sets the ISearchAlgorithm
        /// </summary>
        public ISearchAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Solves an ISearchSolvable.
        /// 
        /// </summary>
        /// <param name="solvable">An ISearchSolvable to solve</param>
        /// <returns></returns>
        public Path Solve(ISearchSolvable solvable) {
            return Solve(solvable, new CancellationToken());
        }

        /// <summary>
        /// Solves an ISearchSolvable using a haltable algorithm
        /// </summary>
        /// <param name="solvable">An ISearchSolvable to solve</param>
        /// <param name="token">A token used to halt the algorithm</param>
        /// <returns></returns>
        public Path Solve(ISearchSolvable solvable, CancellationToken token) {
            Algorithm.CancellationToken = token;
            var path = Algorithm.Run(solvable);
            if (path == null) return null;
            foreach (var node in path) {
                node.State = NodeState.Solution;
            }
            return path;
        }
    }
}