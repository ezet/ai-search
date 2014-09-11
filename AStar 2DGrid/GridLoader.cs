using System.Collections.Generic;

namespace eZet.AStar.Grid {
    public static class GridLoader {

        static GridLoader() {
            GridBank = new List<string>();
            GridBank.Add("10,10 0,0 9,9 2,3,5,5 8,8,2,1");
            GridBank.Add("20,20 19,3 2,18 5,5,10,10 1,2,4,1");
            GridBank.Add("20,20 0,0 19,19 17,10,2,1 14,4,5,2 3,16,10,2 13,7,5,3 15,15,3,3");
            GridBank.Add("10,10 0,0 9,5 3,0,2,7 6,0,4,4 6,6,2,4");
            GridBank.Add("10,10 0,0 9,9 3,0,2,7 6,0,4,4 6,6,2,4");
            GridBank.Add("20,20 0,0 19,13 4,0,4,16 12,4,2,16 16,8,4,4");
        }

        public static List<string> GridBank { get; set; }

        public static Grid2D Load(string data) {
            return parse(data);
        }

        private static Grid2D parse(string data) {
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
