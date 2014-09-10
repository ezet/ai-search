using System.ComponentModel;
using System.Runtime.CompilerServices;
using eZet.AStar.Grid.Annotations;
using eZet.AStar.Gui;

namespace eZet.AStar.Grid {

    public class Grid2DNode : INode, INotifyPropertyChanged {
        private NodeState _state;

        public Grid2DNode(int x, int y, bool isBlocked = false) {
            X = x;
            Y = y;
            IsBlocked = isBlocked;
        }

 

        public NodeState State {
            get { return _state; }
            set {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public bool IsBlocked { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
