namespace eZet.Csp.GraphColouring {
    public class VertexColorResult {
        public int UnsatisfiedConstraints { get; set; }

        public int UnassignedVariables { get; set; }

        public int SearchNodeCount { get; set; }

        public int ExpandedNodeCount { get; set; }

        public int SolutionSearchLength { get; set; }
    }
}