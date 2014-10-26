using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace eZet.Csp.Flow.Converters {
    public class DomainValuesToColorConverter : MarkupExtension, IValueConverter {
        public DomainValuesToColorConverter() {
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                var domains = ((IEnumerable<IDomainValue>)value).Cast<FlowGridDomainValue>();
                if (!domains.Any())
                    return Brushes.Black;
                var domain = domains.First();
                if (!domains.All(d => d.Value == domain.Value))
                    return Brushes.White;
                if (domain.Value == 0) return Brushes.Green;
                if (domain.Value == 1) return Brushes.Yellow;
                if (domain.Value == 2) return Brushes.Red;
                if (domain.Value == 3) return Brushes.Blue;
                if (domain.Value == 4) return Brushes.DarkOrange;
                if (domain.Value == 5) return Brushes.Purple;
                if (domain.Value == 6) return Brushes.Turquoise;
                if (domain.Value == 7) return Brushes.DarkSlateGray;
                if (domain.Value == 8) return Brushes.DarkKhaki;
                if (domain.Value == 9) return Brushes.DarkGray;
                if (domain.Value == 10) return Brushes.CornflowerBlue;
            } catch (Exception) {
                return Brushes.White;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}