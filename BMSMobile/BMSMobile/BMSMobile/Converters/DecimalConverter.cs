using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BMSMobile.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Decimal.Parse(value.ToString()).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueFromString = value.ToString().Replace(" ", "");

            if (valueFromString.Length <= 0)
                return 0m;

            decimal valueDec;
            if (!decimal.TryParse(valueFromString, out valueDec))
                return 0m;

            if (valueDec <= 0)
                return 0m;

            return valueDec;
        }
    }
}
