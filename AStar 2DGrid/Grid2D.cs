using System;
using System.Collections.Generic;
using System.Linq;

namespace eZet.AStar.Grid {
    /// <summary>
    /// Represents a solvable 2D Grid
    /// </summary>
    public class Grid2D : ISearchSolvable {

        /// <summary>
        /// Creates a new Grid
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public Grid2D(int height, int width) {
            Nodes = new List<Grid2DNode>();
            Goal = new List<Grid2DNode>();
            Height = height;
            Width = width;
            initialize();
        }

        /// <summary>
        /// Initializes the grid
        /// </summary>
        private void initialize() {
            Nodes.Clear();
            Grid = new Grid2DNode[Width, Height];
            for (int i = 0; i < Width; ++i) {
                for (int j = 0; j < Height; ++j) {
                    Grid[i, j] = new Grid2DNode(i, j);
                    Nodes.Add(Grid[i, j]);
                }
            }
        }

        /// <summary>
        /// Gets a list of all the grid nodes
        /// </summary>
        public List<Grid2DNode> Nodes { get; private set; }

        /// <summary>
        /// Gets the height of the grid
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the width of the grid
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the full grid
        /// </summary>
        public Grid2DNode[,] Grid { get; private set; }

        /// <summary>
        /// The start node
        /// </summary>
        private Grid2DNode Start { get; set; }

        /// <summary>
        /// The goal nodes
        /// </summary>
        private List<Grid2DNode> Goal { get; set; }


        /// <summary>
        /// Returns true if the parameter node is a solution node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsSolution(ISearchNode node) {
            return Goal.Contains(node);
        }

        /// <summary>
        /// Returns the cost of moving from lastNode to node
        /// </summary>
        /// <param name="lastNode"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public double Cost(ISearchNode lastNode, ISearchNode node) {
            return 1;
        }

        /// <summary>
        /// Returns the estimated cost of moving from node to the goal
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public double Estimate(ISearchNode node) {
            var n = (Grid2DNode) node;
            return ManhattenDistance(n.X, n.Y, Goal.First().X, Goal.First().Y);
        }

        /// <summary>
        /// Adds a goal 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddGoal(int x, int y) {
            Goal.Add(Grid[x, y]);
            Grid[x, y].IsGoalNode = true;
        }

        /// <summary>
        /// Sets the start node
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetStart(int x, int y) {
            Start = Grid[x, y];
            Grid[x, y].IsStartNode = true;
        }


        /// <summary>
        /// Adds a barrier to the grid
        /// </summary>
        /// <param name="barriers"></param>
        public void AddBarriers(List<Grid2DNode> barriers) {
            foreach (var barrier in barriers) {
                Grid[barrier.X, barrier.Y].IsBlocked = true;
            }
        }

        /// <summary>
        /// Returns true if the node at given coordinates is blocked
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsBlocked(int x, int y) {
            return Grid[x, y].IsBlocked;
        }

        /// <summary>
        /// Returns true if the node is on the grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsOnGrid(int x, int y) {
            return x < Width && x >= 0 && y < Height && y >= 0;
        }

        /// <summary>
        /// Gets the start node
        /// </summary>
        public ISearchNode StartNode {
            get { return Start; }
        }

        /// <summary>
        /// Expands the node, returnings its neighbours
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<ISearchNode> Expand(ISearchNode node) {
            var n = (Grid2DNode) node;
            var neighbors = new List<ISearchNode> {
                getNode(n.X + 1, n.Y),
                getNode(n.X - 1, n.Y),
                getNode(n.X, n.Y + 1),
                getNode(n.X, n.Y - 1)
            };
            return neighbors.Where(neighbor => neighbor != null);
        }

        /// <summary>
        /// Resets the grid
        /// </summary>
        public void Reset() {
            initialize();
        }

        /// <summary>
        /// Returns a node at the given coordiantes
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private ISearchNode getNode(int x, int y) {
            if (IsOnGrid(x, y) && !IsBlocked(x, y)) {
                return Grid[x, y];
            }
            return null;
        }

        /// <summary>
        /// Returns the manhatten distance between coordinates
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        private static int ManhattenDistance(int x1, int y1, int x2, int y2) {
            return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }

        private static int EuclidianDistance(int x1, int y1, int x2, int y2) {
            int result = 0;
            double part1 = Math.Pow((x2 - x1), 2);
            double part2 = Math.Pow((y2 - y1), 2);
            double underRadical = part1 + part2;
            result = (int) Math.Sqrt(underRadical);
            return result;
        }
    }
}