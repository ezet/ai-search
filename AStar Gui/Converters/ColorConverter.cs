using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using eZet.AStar.Grid;

namespace eZet.AStar.Gui.Converters {
    public class ColorConverter :  MarkupExtension, IValueConverter {

        public ColorConverter() {
            
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var state = (NodeState) value;
            if (state == NodeState.Closed)
                return Brushes.Red;
            if (state == NodeState.Open)
                return Brushes.Green;
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

    }
}