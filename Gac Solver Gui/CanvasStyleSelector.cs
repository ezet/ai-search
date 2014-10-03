using System.Windows;
using System.Windows.Controls;
using eZet.Csp.GraphColouring;

namespace eZet.Gac.Gui {
    public class CanvasStyleSelector : StyleSelector {
        public Style GraphNodeStyle { get; set; }

        public Style GraphEdgeStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container) {
            if (item.GetType() == typeof (SimpleVariable))
                return GraphNodeStyle;
            return GraphEdgeStyle;
        }
    }
}