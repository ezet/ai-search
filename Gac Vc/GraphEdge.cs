using System.ComponentModel;
using System.Runtime.CompilerServices;
using eZet.Csp.Properties;

namespace eZet.Csp.VertexColouring {

    /// <summary>
    /// Represents a simple graph edge, that can be drawn on a canvas
    /// </summary>
    public class GraphEdge : IEdge, ICanvasObject {

        /// <summary>
        /// Gets the first node in the edge
        /// </summary>
        public IVariable Node1 { get; private set; }

        /// <summary>
        /// Gets the second node on the edge
        /// </summary>
        public IVariable Node2 { get; private set; }

        /// <summary>
        /// Creates a new edge
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        public GraphEdge(IVariable node1, IVariable node2) {
            Node1 = node1;
            Node2 = node2;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}