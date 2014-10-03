using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eZet.AStar.Algorithms {
    public class Dfs : ISearchAlgorithm {
        public Dfs(int throttle) {
            Throttle = throttle;
        }

        /// <summary>
        /// A stack containing all open nodes
        /// </summary>
        private Stack<Path> _open;

        /// <summary>
        /// A hash set containing all closed nodes
        /// </summary>
        private HashSet<ISearchNode> _closed;

        public int Throttle { get; set; }

        public IList<ISearchNode> ExpandedNodes {
            get { return _closed.ToList(); }
        }

        public int SearchNodeCount() {
            return _closed.Count + _open.Count;
        }

        public CancellationToken CancellationToken { get; set; }

        public Path Run(ISearchSolvable solvable) {
            _open = new Stack<Path>();
            _closed = new HashSet<ISearchNode>();
            _open.Push(new Path(solvable.StartNode));
            while (_open.Any()) {
                if (CancellationToken.IsCancellationRequested) {
                    return null;
                }
                var path = _open.Pop();
                if (_closed.Contains(path.Node)) {
                    continue;
                }
                if (solvable.IsSolution(path.Node)) {
                    return path;
                }
                path.Node.State = NodeState.Processing;
                foreach (ISearchNode node in solvable.Expand(path.Node)) {
                    if (node.State == NodeState.Closed) {
                        continue;
                    }
                    node.State = NodeState.Open;
                    var g = solvable.Cost(path.Node, node);
                    Path newPath = path.AddNode(node, g);
                    _open.Push(newPath);
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