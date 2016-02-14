using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace EnglishDX {
    public class ValueToColorConverter : MarkupExtension, IValueConverter {
        public ValueToColorConverter() { }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            Color cl;
            if (!(bool)value) {
                cl = (Color)ColorConverter.ConvertFromString("#FFF7C9C9");
                return new SolidColorBrush(cl);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }


    public class BoolToFontSizeConverter : MarkupExtension, IValueConverter {
        public BoolToFontSizeConverter() { }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            //Color cl;
            if ((bool)value) {
                //  cl = (Color)ColorConverter.ConvertFromString("#FFF7C9C9");
                return 50;
            }
            return 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }


    public class IntToColorConverter : MarkupExtension, IValueConverter {
        public IntToColorConverter() { }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value == null)
                return null;
            int v = (int)value;
            if (v < 4)
                return new SolidColorBrush(Colors.Red);
            if (v < 8)
                return new SolidColorBrush(Colors.Yellow);
            return new SolidColorBrush(Colors.Green);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }

    public class custConverter : MarkupExtension, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            ContentPresenter cp = value as ContentPresenter;
            return "tx";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }

   
}
