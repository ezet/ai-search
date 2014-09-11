using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eZet.AStar {
    public class Bfs : IAlgorithm {

        public Bfs() {
            
        }

        public Bfs(int throttle) {
            Throttle = throttle;
        }

        private Queue<Path> _open;

        private HashSet<INode> _closed;

        public Path Run(ISolvable solvable) {
            _open = new Queue<Path>();
            _closed = new HashSet<INode>();
            _open.Enqueue(new Path(solvable.GetStartNode));
            while (_open.Any()) {
                var path = _open.Dequeue();
                if (_closed.Contains(path.CurrentNode)) {
                    continue;}
                if (solvable.IsSolution(path.CurrentNode)) {
                    return path;
                }
                path.CurrentNode.State = NodeState.Processing;
                foreach (INode node in solvable.GetNeighbours(path.CurrentNode)) {
                    node.State = NodeState.Open;
                    var g = solvable.Cost(path.CurrentNode, node);
                    Path newPath = path.AddNode(node, g);
                    _open.Enqueue(newPath);
                }
                Thread.Sleep(Throttle);
                _closed.Add(path.CurrentNode);
                path.CurrentNode.State = NodeState.Closed;
            }
            return null;
        }

        public int Throttle { get; set; }
    }
}