using System.Collections.Generic;
using System.Threading;

namespace eZet.AStar {
    public class AStar : IAlgorithm {

        public AStar() {
            
        }

        public AStar(int throttle) {
            Throttle = throttle;
        }

        private HashSet<INode> _closed;

        private PriorityQueue<double, Path> _open;

        public int Throttle { get; set; }

        public Path Run(ISolvable solvable) {
            _closed = new HashSet<INode>();
            _open = new PriorityQueue<double, Path>();
            _open.Enqueue(0, new Path(solvable.GetStartNode));
            solvable.GetStartNode.State = NodeState.Open;
            while (!_open.IsEmpty) {
                var path = _open.Dequeue();
                if (path.CurrentNode.State == NodeState.Closed) {
                    continue;
                }
                if (solvable.IsSolution(path.CurrentNode)) {
                    return path;
                }
                path.CurrentNode.State = NodeState.Processing;

                foreach (INode node in solvable.GetNeighbours(path.CurrentNode)) {
                    if (node.State == NodeState.Closed) {
                        continue;
                    }
                    node.State = NodeState.Open;
                    double g = solvable.Cost(path.CurrentNode, node);
                    var newPath = path.AddNode(node, g);
                    _open.Enqueue(newPath.TotalCost + solvable.Estimate(node), newPath);
                }
                Thread.Sleep(Throttle);
                _closed.Add(path.CurrentNode);
                path.CurrentNode.State = NodeState.Closed;

            }
            return null;
        }
    }
}
