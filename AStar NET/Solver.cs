using System.Threading;

namespace eZet.AStar {
    public class Solver {

        public Solver(IAlgorithm algorithm) {
            Algorithm = algorithm;
        }

        public IAlgorithm Algorithm { get; set; }

        public Path Solve(ISolvable solvable) {
            return Solve(solvable, new CancellationToken());
        }

        public Path Solve(ISolvable solvable, CancellationToken token) {
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
