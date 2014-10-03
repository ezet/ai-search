namespace eZet.AStar {
    /// <summary>
    /// Represents a node in a ISearchSolvable object
    /// </summary>
    public interface ISearchNode {
        /// <summary>
        /// Gets or sets the node state
        /// </summary>
        NodeState State { get; set; }
    }
}