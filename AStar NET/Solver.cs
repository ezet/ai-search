namespace eZet.AStar {
    public class Solver {

        public Solver(IAlgorithm algorithm) {
            Algorithm = algorithm;
        }

        public IAlgorithm Algorithm { get; set; }

        public Path Solve(ISolvable solvable) {

            var path = Algorithm.Run(solvable);
            foreach (var node in path) {
                node.State = NodeState.Solution;
            }
            return path;
        }
    }
}
