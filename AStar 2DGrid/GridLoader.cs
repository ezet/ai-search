using System.Collections.Generic;

namespace eZet.AStar.Grid {
    public static class GridLoader {

        public static Grid2D Load(string path) {
            //return parse(path);
            return parse("10,10 0,0 9,9");
        }

        static Grid2D parse(string data) {
            var tokens = data.Split(' ');
            var dimensions = tokens[0].Split(',');
            var grid = new Grid2D(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
            grid.Start = parseNode(tokens[1]);
            grid.Goal.Add(parseNode(tokens[2]));
            for (var i = 3; i < tokens.Length; ++i) {
                grid.AddBarriers(parseBarrier(tokens[i]));
            }
            return grid;

        }

        static Grid2DNode parseNode(string node) {
            var nodeData = node.Split(',');
            return new Grid2DNode(int.Parse(nodeData[0]), int.Parse(nodeData[1]));
        }

        static List<Grid2DNode> parseBarrier(string barrier) {
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
