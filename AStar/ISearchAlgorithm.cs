using System.Collections.Generic;
using System.Threading;

namespace eZet.AStar {
    /// <summary>
    /// Interface for an algorithm that can be used on an ISearchSolvable object
    /// </summary>
    public interface ISearchAlgorithm {
        /// <summary>
        /// Runs the algorithm on a ISearchSolvable object
        /// </summary>
        /// <param name="solvable"></param>
        /// <returns></returns>
        Path Run(ISearchSolvable solvable);

        /// <summary>
        /// Get or set a delay in MS used to throttle the algorithm
        /// </summary>
        int Throttle { get; set; }

        /// <summary>
        /// Returns all nodes that have been expanded
        /// </summary>
        IList<ISearchNode> ExpandedNodes { get; }

        int SearchNodeCount();

        /// <summary>
        /// Gets or sets a cancellation token used to halt the algorithm
        /// </summary>
        CancellationToken CancellationToken { get; set; }
    }
}