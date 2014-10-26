using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace eZet.Csp.Nonogram.Gui.Converters {
    public class PositionConverter : MarkupExtension, IValueConverter {
        public PositionConverter() {
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int index = (int) value;
            var max = 20;
            var offset = 20;
            return (max - index)*10 + offset;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}