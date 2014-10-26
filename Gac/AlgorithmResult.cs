namespace eZet.Csp {
    public class AlgorithmResult {
        public AlgorithmResult(int solutionLength, int nodesExpanded, int nodesGenerated) {
            NodesExpanded = nodesExpanded;
            NodesGenerated = nodesGenerated;
            SolutionLength = solutionLength;
        }

        public int SolutionLength { get; private set; }

        public int NodesExpanded { get; private set; }
        public int NodesGenerated { get; private set; }


    }
}