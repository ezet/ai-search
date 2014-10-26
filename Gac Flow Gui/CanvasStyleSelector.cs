using System.Windows;
using System.Windows.Controls;
using eZet.Csp.VertexColouring;

namespace eZet.Csp.Flow {
    public class CanvasStyleSelector : StyleSelector {
        public Style GraphNodeStyle { get; set; }

        public Style GraphEdgeStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container) {
            if (item.GetType() == typeof (GridVariable))
                return GraphNodeStyle;
            return GraphEdgeStyle;
        }
    }
}