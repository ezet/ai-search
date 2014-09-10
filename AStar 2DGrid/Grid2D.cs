using System;
using System.Collections.Generic;
using System.Linq;

namespace eZet.AStar.Grid {
    public class Grid2D : ISolvable {

        public Grid2D(int height, int width) {
            Height = height;
            Width = width;
            Grid = new Grid2DNode[Width, Height];
            Goal = new List<Grid2DNode>();
            Nodes = new List<Grid2DNode>();
            for (int i = 0; i < Width; ++i) {
                for (int j = 0; j < Height; ++j) {
                    Grid[i, j] = new Grid2DNode(i, j);
                    Nodes.Add(Grid[i, j]);
                }
            }
        }

        public List<Grid2DNode> Nodes { get; private set; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public Grid2DNode Start { get; set; }

        public bool IsSolution(INode node) {
            return node.Equals(Goal.First());
        }

        public double Cost(INode lastNode, INode node) {
            return 1;
        }

        public double Estimate(INode node) {
            var n = (Grid2DNode)node;
            return Distance2D(n.X, n.Y, Goal.First().X, Goal.First().Y);
        }

        public List<Grid2DNode> Goal { get; private set; }

        public Grid2DNode[,] Grid { get; set; }


        public void AddBarriers(List<Grid2DNode> barriers) {
            foreach (var barrier in barriers) {
                Grid[barrier.X, barrier.Y].IsBlocked = true;
            }
        }

        public bool IsBlocked(int x, int y) {
            return Grid[x, y].IsBlocked;
        }

        public bool IsOnGrid(int x, int y) {
            return x < Width && x >= 0 && y < Height && y >= 0;
        }

        public INode GetStartNode {
            get { return Start; }
        }

        public IEnumerable<INode> GetNeighbours(INode node) {
            var n = (Grid2DNode)node;
            var neighbors = new List<INode> {
                getNode(n.X + 1, n.Y),
                getNode(n.X - 1, n.Y),
                getNode(n.X, n.Y + 1),
                getNode(n.X, n.Y - 1)
            };
            return neighbors.Where(neighbor => neighbor != null);
        }

        private INode getNode(int x, int y) {
            if (IsOnGrid(x, y) && !IsBlocked(x, y)) {
                return Grid[x, y];
            }
            return null;
        }

        public static int Distance2D(int x1, int y1, int x2, int y2) {
            int result = 0;
            double part1 = Math.Pow((x2 - x1), 2);
            double part2 = Math.Pow((y2 - y1), 2);
            double underRadical = part1 + part2;
            result = (int)Math.Sqrt(underRadical);
            return result;
        }
    }
}
