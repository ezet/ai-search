using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace eZet.AStar.Grid {
    public static class GridLoader {

        /// <summary>
        /// Loads the gridbank from disk
        /// </summary>
        /// <returns></returns>
        public static List<string> LoadBank() {
            return File.ReadAllLines("../../grids.txt").ToList();
        }

        /// <summary>
        /// Parses a grid string and returns a grid
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Grid2D Parse(string data) {
            data = data.Trim();
            if (data.Contains("(")) {
                data = data.Replace(" ", "").Replace(")(", " ").Replace("(", "").Replace(")", "");
            }
            var tokens = data.Split(' ');
            var dimensions = tokens[0].Split(',');
            var grid = new Grid2D(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
            var start = tokens[1].Split(',');
            grid.SetStart(int.Parse(start[0]), int.Parse(start[1]));
            var goal = tokens[2].Split(',');
            grid.AddGoal(int.Parse(goal[0]), int.Parse(goal[1]));
            for (var i = 3; i < tokens.Length; ++i) {
                grid.AddBarriers(parseBarrier(tokens[i]));
            }
            return grid;
        }

        /// <summary>
        /// Parses barrier coordinates
        /// </summary>
        /// <param name="barrier"></param>
        /// <returns></returns>
        private static List<Grid2DNode> parseBarrier(string barrier) {
            var barrierData = barrier.Split(',');
            var list = new List<Grid2DNode>();
            var x = int.Parse(barrierData[0]);
            var y = int.Parse(barrierData[1]);
            for (var xi = 0; xi < int.Parse(barrierData[2]); ++xi) {
                for (var yi = 0; yi < int.Parse(barrierData[3]); ++yi) {
                    list.Add(new Grid2DNode(x + xi, y + yi, true));
                }
            }
            return list;
        }
    }
}