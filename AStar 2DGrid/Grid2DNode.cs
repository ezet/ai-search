using System.ComponentModel;
using System.Runtime.CompilerServices;
using eZet.AStar.Grid.Properties;

namespace eZet.AStar.Grid {

    /// <summary>
    /// Represents a bindable search node
    /// </summary>
    public class Grid2DNode : ISearchNode, INotifyPropertyChanged {

        /// <summary>
        /// The state of the node
        /// </summary>
        private NodeState _state;

        /// <summary>
        /// Returns a new grid node with given coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isBlocked"></param>
        public Grid2DNode(int x, int y, bool isBlocked = false) {
            X = x;
            Y = y;
            IsBlocked = isBlocked;
        }

        /// <summary>
        /// Gets or sets the state of the node
        /// </summary>
        public NodeState State {
            get { return _state; }
            set {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets whether the node is a start node
        /// </summary>
        public bool IsStartNode { get; set; }

        /// <summary>
        /// Gets or sets whether the node is a goal node
        /// </summary>
        public bool IsGoalNode { get; set; }

        /// <summary>
        /// Gets or sets whether the node is blocked
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Gets the X coordiante
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the Y coordinate
        /// </summary>
        public int Y { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}