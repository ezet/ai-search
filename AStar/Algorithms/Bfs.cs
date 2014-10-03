using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eZet.AStar.Algorithms {
    public class Bfs : ISearchAlgorithm {
        public Bfs() {
        }

        public Bfs(int throttle) {
            Throttle = throttle;
        }

        private Queue<Path> _open;

        private HashSet<ISearchNode> _closed;

        public IList<ISearchNode> ExpandedNodes {
            get { return _closed.ToList(); }
        }

        public int SearchNodeCount() {
            return _closed.Count + _open.Count;
        }

        public CancellationToken CancellationToken { get; set; }

        public Path Run(ISearchSolvable solvable) {
            _open = new Queue<Path>();
            _closed = new HashSet<ISearchNode>();
            _open.Enqueue(new Path(solvable.StartNode));
            while (_open.Any()) {
                if (CancellationToken.IsCancellationRequested) {
                    return null;
                }
                var path = _open.Dequeue();
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
                    _open.Enqueue(newPath);
                }

                if (Throttle > 0)
                    Thread.Sleep(Throttle);
                _closed.Add(path.Node);
                path.Node.State = NodeState.Closed;
            }
            return null;
        }

        public int Throttle { get; set; }
    }
}