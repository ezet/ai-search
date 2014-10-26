using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace eZet.Csp.Nonogram.Gui.Converters {
    public class LineConverter : MarkupExtension, IValueConverter {
        public LineConverter() {
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {

                var line = ((IList<IDomainValue>) value).Cast<LinePattern>();
                var index = int.Parse(parameter.ToString());
                if (line.Count() != 1 || line.Single().BlockArray.Length <= index) return Visibility.Hidden;
                return line.Single().BlockArray[index]
                    ? Visibility.Visible
                    : Visibility.Hidden;
            }
            catch (Exception) {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}