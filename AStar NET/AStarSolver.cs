using System.Collections.Generic;
using System.Threading;
using eZet.AStar.Gui;

namespace eZet.AStar {
    public class AStarSolver {

        public AStarSolver() {
            Throttle = 100;}

        public HashSet<INode> Closed;

        public PriorityQueue<double, Path> Open;

        public int Throttle { get; set; }

        public Path Solve(ISolvable solvable) {
            Closed = new HashSet<INode>();
            Open = new PriorityQueue<double, Path>();
            Open.Enqueue(0, new Path(solvable.GetStartNode));
            solvable.GetStartNode.State = NodeState.Open;
            while (!Open.IsEmpty) {
                Thread.Sleep(Throttle);
                var path = Open.Dequeue();
                if (solvable.IsSolution(path.CurrentNode)) {
                    return path;
                }
                if (Closed.Contains(path.CurrentNode)) {
                    continue;
                }
                Closed.Add(path.CurrentNode);
                path.CurrentNode.State = NodeState.Closed;
                foreach (INode node in solvable.GetNeighbours(path.CurrentNode)) {
                    node.State = NodeState.Open;
                    double g = solvable.Cost(path.CurrentNode, node);
                    var newPath = path.AddNode(node, g);
                    Open.Enqueue(newPath.TotalCost + solvable.Estimate(node), newPath);
                }

            }
            return null;
        }
    }
}
