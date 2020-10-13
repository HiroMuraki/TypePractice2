using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TypePractice2.ViewModel.ValueConverter {
    [ValueConversion(typeof(bool),typeof(SolidColorBrush))]
    public class ShuffleModeToColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                switch ((bool)value) {
                    case true:
                        return new SolidColorBrush(Color.FromRgb(0xbd, 0x2f, 0x54));
                    case false:
                        return new SolidColorBrush(Color.FromRgb(0x15, 0x81, 0xb9));
                    default:
                        return new SolidColorBrush(Colors.White);
                }
            }
            catch {
                return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
