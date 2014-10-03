﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace eZet.Gac.Gui.Converters {
    public class ObjectToIntConverter : MarkupExtension, IValueConverter {
        public ObjectToIntConverter() {
            // to stop WPF no default constructor warning
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return System.Convert.ToInt32(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return System.Convert.ToDouble(value);
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }
}