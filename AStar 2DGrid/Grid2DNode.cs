using System.Collections.Generic;

namespace eZet.AStar.Grid {
    public class Grid2DNode : INode {
        public Grid2DNode(int x, int y, bool isBlocked = false) {
            X = x;
            Y = y;
            IsBlocked = isBlocked;
        }

        public bool IsBlocked { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public IEnumerable<INode> Neighbours { get; private set; }
    }
}
