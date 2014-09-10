using System.Collections.Generic;

namespace eZet.AStar {
    public class AStar {

        public HashSet<INode> Closed;

        public PriorityQueue<double, Path> Open;

        public Path Solve(ISolvable solvable) {
            Closed = new HashSet<INode>();
            Open = new PriorityQueue<double, Path>();
            Open.Enqueue(0, new Path(solvable.GetStartNode));
            while (!Open.IsEmpty) {
                var path = Open.Dequeue();
                if (solvable.IsSolution(path.CurrentNode)) {
                    return path;
                }
                if (Closed.Contains(path.CurrentNode)) {
                    continue;
                }
                Closed.Add(path.CurrentNode);
                foreach (INode node in path.CurrentNode.Neighbours) {
                    double g = solvable.Cost(path.CurrentNode, node);
                    var newPath = path.AddNode(node, g);
                    Open.Enqueue(newPath.TotalCost + solvable.Estimate(node), newPath);
                }

            }
            return null;
        }
    }
}
