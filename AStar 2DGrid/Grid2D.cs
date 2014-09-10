using System;
using System.Collections.Generic;

namespace eZet.AStar.Grid {
    public class Grid2D : ISolvable {

        public Grid2D(int height, int width) {
            Height = height;
            Width = width;
            Grid = new Grid2DNode[Height][];
            for (int i = 0; i < Grid.Length; ++i) {
                Grid[i] = new Grid2DNode[Width];
            }
        }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public Grid2DNode Start { get; set; }
        public bool IsSolution(INode node) {
            throw new System.NotImplementedException();
        }

        public double Cost(INode lastNode, INode node) {
            throw new System.NotImplementedException();
        }

        public double Estimate(INode node) {
            throw new System.NotImplementedException();
        }

        public Grid2DNode Goal { get; set; }

        public Grid2DNode[][] Grid { get; set; }


        public void AddBarriers(List<Grid2DNode> barriers) {
            foreach (var barrier in barriers) {
                Grid[barrier.X][barrier.Y] = barrier;
            }
        }

        public bool IsBlocked(Grid2DNode node) {
            return Grid[node.X][node.Y].IsBlocked;
        }

        public bool IsOnGrid(Grid2DNode node) {
            return node.X < Width && node.X >= 0 && node.Y < Height && node.Y >= 0;
        }

        public INode GetStartNode {
            get { return Start; }
        }

        /// <summary>
        /// Finds the distance between two points on a 2D surface.
        /// </summary>
        /// <param name="x1">The IntPoint on the x-axis of the first IntPoint</param>
        /// <param name="x2">The IntPoint on the x-axis of the second IntPoint</param>
        /// <param name="y1">The IntPoint on the y-axis of the first IntPoint</param>
        /// <param name="y2">The IntPoint on the y-axis of the second IntPoint</param>
        /// <returns></returns>
        public static long Distance2D(long x1, long y1, long x2, long y2) {
            //     ______________________
            //d = &#8730; (x2-x1)^2 + (y2-y1)^2
            //

            //Our end result
            long result = 0;
            //Take x2-x1, then square it
            double part1 = Math.Pow((x2 - x1), 2);
            //Take y2-y1, then sqaure it
            double part2 = Math.Pow((y2 - y1), 2);
            //Add both of the parts together
            double underRadical = part1 + part2;
            //Get the square root of the parts
            result = (long)Math.Sqrt(underRadical);
            //Return our result
            return result;
        }

        /// <summary>
        /// Finds the distance between two points on a 2D surface.
        /// </summary>
        /// <param name="x1">The IntPoint on the x-axis of the first IntPoint</param>
        /// <param name="x2">The IntPoint on the x-axis of the second IntPoint</param>
        /// <param name="y1">The IntPoint on the y-axis of the first IntPoint</param>
        /// <param name="y2">The IntPoint on the y-axis of the second IntPoint</param>
        /// <returns></returns>
        public static int Distance2D(int x1, int y1, int x2, int y2) {
            //     ______________________
            //d = &#8730; (x2-x1)^2 + (y2-y1)^2
            //

            //Our end result
            int result = 0;
            //Take x2-x1, then square it
            double part1 = Math.Pow((x2 - x1), 2);
            //Take y2-y1, then sqaure it
            double part2 = Math.Pow((y2 - y1), 2);
            //Add both of the parts together
            double underRadical = part1 + part2;
            //Get the square root of the parts
            result = (int)Math.Sqrt(underRadical);
            //Return our result
            return result;
        }
    }
}
