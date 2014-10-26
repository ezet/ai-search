using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace eZet.Csp.Flow.Converters {
    public class PositionConverter : MarkupExtension, IValueConverter {
        public PositionConverter() {
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var offset = 20;
            return (int) value*30 + offset;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}