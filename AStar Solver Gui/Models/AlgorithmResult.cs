using System;

namespace eZet.AStar.Gui.Models {
    public class AlgorithmResult {
        public AlgorithmResult(int solutionLength, int nodesExpanded, TimeSpan timeSpent) {
            TimeSpent = timeSpent;
            NodesExpanded = nodesExpanded;
            SolutionLength = solutionLength;
        }

        public int SolutionLength { get; private set; }

        public int NodesExpanded { get; private set; }

        public TimeSpan TimeSpent { get; private set; }
    }
}