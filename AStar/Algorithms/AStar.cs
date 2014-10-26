using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eZet.AStar.Algorithms {
    /// <summary>
    /// This class implements the A-Star algorithm
    /// </summary>
    public class AStar : ISearchAlgorithm {
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="throttle"></param>
        public AStar(int throttle) {
            Throttle = throttle;
            _closed = new HashSet<ISearchNode>();
            _open = new PriorityQueue<double, Path>();
        }

        /// <summary>
        /// A hash set containing all closed nodes
        /// </summary>
        private HashSet<ISearchNode> _closed;

        /// <summary>
        /// A priority queue containing all open nodes
        /// </summary>
        private PriorityQueue<double, Path> _open;

        /// <summary>
        /// Get or set a delay in milliseconds used to throttle the speed of the algorithm
        /// </summary>
        public int Throttle { get; set; }

        /// <summary>
        /// Returns a list of all nodes that have been processed and expanded
        /// </summary>
        public IList<ISearchNode> ExpandedNodes {
            get { return _closed.ToList(); }
        }

        public int SearchNodeCount() {
            return ExpandedNodes.Count + _open.Count;
        }

        /// <summary>
        /// Get or set a cancellation token used to halt the algorithm
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Runs the A star algorithm on a ISearchSolvable object
        /// Returns a solution path if a solution was found, otherwise null
        /// </summary>
        /// <param name="solvable">A ISearchSolvable object</param>
        /// <returns>A solution path or null</returns>
        public Path Run(ISearchSolvable solvable) {
            _open = new PriorityQueue<double, Path>();
            _closed.Clear();
            _open.Enqueue(0, new Path(solvable.StartNode));
            solvable.StartNode.State = NodeState.Open;
            while (!_open.IsEmpty) {
                if (CancellationToken.IsCancellationRequested) {
                    return null;
                }
                var path = _open.Dequeue();
                if (path.Node.State == NodeState.Closed) {
                    continue;
                }
                if (solvable.IsSolution(path.Node)) {
                    return path;
                }
                path.Node.State = NodeState.Processing;

                // expand the node
                foreach (ISearchNode node in solvable.Expand(path.Node)) {
                    if (node.State == NodeState.Closed) {
                        continue;
                    }
                    node.State = NodeState.Open;
                    double g = solvable.Cost(path.Node, node);
                    var newPath = path.AddNode(node, g);
                    _open.Enqueue(newPath.TotalCost + solvable.Estimate(node), newPath);
                }
                if (Throttle > 0)
                    Thread.Sleep(Throttle);
                _closed.Add(path.Node);
                path.Node.State = NodeState.Closed;
            }
            return null;
        }
    }
}