using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using eZet.AStar;

namespace eZet.Csp.Nonogram {
    public class NonogramSolver {

        public GacSolvable Solvable { get; private set; }

        public NonogramModel Load(string path) {
            return Parse(File.ReadAllText(path));
        }

        public GacSolvable.Result Solve(NonogramModel model, ISearchAlgorithm algorithm) {
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


        public NonogramModel Parse(string data) {
            string[] lineData = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var dim = lineData[0].Split(' ');
            var width = int.Parse(dim[0]);
            var height = int.Parse(dim[1]);
            var rows = new List<NonogramLine>();
            for (int i = 1; i < height + 1; ++i) {
                var blockData = lineData[i].Split(' ');
                var blocks = new List<Block>();
                for (int j = 0; j < blockData.Length; ++j) {
                    if (j == blockData.Length - 1)
                        blocks.Add(new Block(int.Parse(blockData[j])));
                    else
                        blocks.Add(new Block(int.Parse(blockData[j]), true));
                }
                NonogramLine nonogramLine = new NonogramLine(i - 1, width, NonogramLine.LineType.Row, blocks);
                rows.Add(nonogramLine);
            }
            var cols = new List<NonogramLine>();
            for (int i = 1 + height; i < height + width + 1; ++i) {
                var blockData = lineData[i].Split(' ');
                var blocks = new List<Block>();
                for (int j = 0; j < blockData.Length; ++j) {
                    if (j == 0)
                        blocks.Add(new Block(int.Parse(blockData[j])));
                    else
                        blocks.Add(new Block(int.Parse(blockData[j]), true));
                }
                blocks.Reverse();
                NonogramLine nonogramLine = new NonogramLine(i - 1 - height, height, NonogramLine.LineType.Column, blocks);
                cols.Add(nonogramLine);
            }

            var nanogram = new NonogramModel(rows, cols);

            return nanogram;
        }



    }
}
