using System;
using System.Windows;
using System.Windows.Controls;
using eZet.Csp.VertexColouring;

namespace eZet.Gac.Gui {
    public class CanvasTemplateSelector : DataTemplateSelector {
        public DataTemplate GraphNodeTemplate { get; set; }

        public DataTemplate GraphEdgeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            if (item.GetType() == typeof (GridVariable))
                return GraphNodeTemplate;
            if (item.GetType() == typeof (GraphEdge))
                return GraphEdgeTemplate;
            throw new NotImplementedException();
        }
    }
}