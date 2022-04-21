using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MiniProjekatHCI
{
    class NameToBrushConverter:IValueConverter
    {
        public static double minValue;
        public static double maxValue;

        public NameToBrushConverter() { }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double input = (double)value;
            if (input == maxValue)
            {
                return Brushes.LightBlue;
            }
            else if(input == minValue)
            {
                return Brushes.LightGreen;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
