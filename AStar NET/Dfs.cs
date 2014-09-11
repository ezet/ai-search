using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace eZet.AStar {
    public class Dfs : IAlgorithm {

        public Dfs() {
            
        }

        public Dfs(int throttle) {
            Throttle = throttle;
        }

        private Stack<Path> _open;

        private HashSet<INode> _closed;

        public Path Run(ISolvable solvable) {
            _open = new Stack<Path>();
            _closed = new HashSet<INode>();
            _open.Push(new Path(solvable.GetStartNode));
            while (_open.Any()) {
                var path = _open.Pop();
                if (_closed.Contains(path.CurrentNode)) {
                    continue;}
                if (solvable.IsSolution(path.CurrentNode)) {
                    return path;
                }
                path.CurrentNode.State = NodeState.Processing;
                foreach (INode node in solvable.GetNeighbours(path.CurrentNode)) {
                    if (node.State != NodeState.Closed) {
                        node.State = NodeState.Open;
                    }var g = solvable.Cost(path.CurrentNode, node);
                    Path newPath = path.AddNode(node, g);
                    _open.Push(newPath);
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