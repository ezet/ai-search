using System.Collections.Generic;

namespace eZet.AStar {
    /// <summary>
    /// Represents a interface for classes that are solvable by an ISearchAlgorithm
    /// </summary>
    public interface ISearchSolvable {
        /// <summary>
        /// Gets the start node
        /// </summary>
        ISearchNode StartNode { get; }

        /// <summary>
        /// Returns true if the parameter node is a solution node, otherwise false
        /// </summary>
        /// <param name="node">An ISearchNode</param>
        /// <returns>True if the ISearchNode is a solution, otherwise false</returns>
        bool IsSolution(ISearchNode node);

        /// <summary>
        /// Returns the cost of traversing the edge from the lastNode to node;
        /// </summary>
        /// <param name="lastNode">An ISearchNode to move from</param>
        /// <param name="node">An ISearchNode to move to</param>
        /// <returns>The cost of moving from lastNode to node</returns>
        double Cost(ISearchNode lastNode, ISearchNode node);

        /// <summary>
        /// Returns the estimated minimal cost of moving to a solution node from the input node
        /// </summary>
        /// <param name="node">Node to move from</param>
        /// <returns></returns>
        double Estimate(ISearchNode node);

        /// <summary>
        /// Returns the neighbours of the input node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IEnumerable<ISearchNode> Expand(ISearchNode node);

        /// <summary>
        /// Resets all node states of the grid
        /// </summary>
        void Reset();
    }
}