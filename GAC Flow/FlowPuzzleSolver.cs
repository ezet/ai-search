using System;
using System.IO;
using System.Linq;
using eZet.AStar;

namespace eZet.Csp.Flow {

    public class FlowPuzzleSolver {

        public FlowGridModel Load(string path) {
            return Parse(File.ReadAllText(path));
        }

        public FlowGridModel Parse(string data) {
            string[] lines = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var spec = lines[0].Split(' ');
            var model = new FlowGridModel(int.Parse(spec[0]), int.Parse(spec[1]));
            foreach (var line in lines.Skip(1)) {
                var values = line.Trim().Split(' ');
                model.AddStartPoint(int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[0]));
                model.AddEndPoint(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[0]));
            }
            model.Initialize();
            return model;
        }

        public GacSolvable.Result Solve(FlowGridModel model, ISearchAlgorithm algorithm) {
            Solvable = new GacSolvable(model);
            Solvable.Algorithm = algorithm;
            return Solvable.Solve();
        }

        public AlgorithmResult GetStatistics() {
            var solutionLength = Solvable.SearchPath != null ? Solvable.SearchPath.Count() : 0;
            var expandedNodes = Solvable.Algorithm.ExpandedNodes.Count;
            var generatedNodes = Solvable.Algorithm.SearchNodeCount();
            return new AlgorithmResult(solutionLength, expandedNodes, generatedNodes);
        }

        public GacSolvable Solvable { get; private set; }
    }
}
