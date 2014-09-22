﻿using System.Collections.Generic;
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

        public IList<INode> ExpandedNodes {
            get { return _closed.ToList(); }
        }

        public CancellationToken CancellationToken { get; set; }

        public Path Run(ISolvable solvable) {
            _open = new Queue<Path>();
            _closed = new HashSet<INode>();
            _open.Enqueue(new Path(solvable.GetStartNode));
            while (_open.Any()) {
                if (CancellationToken.IsCancellationRequested) {
                    return null;
                }
                var path = _open.Dequeue();
                if (_closed.Contains(path.Node)) {
                    continue;}
                if (solvable.IsSolution(path.Node)) {
                    return path;
                }
                path.Node.State = NodeState.Processing;
                foreach (INode node in solvable.GetNeighbours(path.Node)) {
                    if (node.State == NodeState.Closed) {continue;
                    }
                    node.State = NodeState.Open;
                    var g = solvable.Cost(path.Node, node);
                    Path newPath = path.AddNode(node, g);
                    _open.Enqueue(newPath);
                }

                Thread.Sleep(Throttle);
                _closed.Add(path.Node);
                path.Node.State = NodeState.Closed;
            }
            return null;
        }

        public int Throttle { get; set; }
    }
}