using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using eZet.Csp;
using eZet.Csp.GraphColouring;

namespace eZet.Gac.Gui.Converters {
    public class DomainValuesToColorConverter : MarkupExtension, IValueConverter {
        public DomainValuesToColorConverter() {
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                var domains = (IEnumerable<IDomainValue>)value;
                if (!domains.Any())
                    return Brushes.Black;
                if (domains.Count() > 1)
                    return Brushes.LightGray;
                var domain = (DomainValue)domains.Single();
                if (domain.Value == 0) return Brushes.Red;
                if (domain.Value == 1) return Brushes.Green;
                if (domain.Value == 2) return Brushes.Blue;
                if (domain.Value == 3) return Brushes.Yellow;
                if (domain.Value == 4) return Brushes.Turquoise;
                if (domain.Value == 5) return Brushes.Tan;
                if (domain.Value == 6) return Brushes.Teal;
                if (domain.Value == 7) return Brushes.SpringGreen;
            } catch (Exception) {
                return Brushes.LightGray;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}