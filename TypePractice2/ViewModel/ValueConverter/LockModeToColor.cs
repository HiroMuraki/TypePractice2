using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TypePractice2.ViewModel.ValueConverter {
    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class LockModeToColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                switch ((bool)value) {
                    case true:
                        return new SolidColorBrush(Color.FromRgb(0xf9, 0xc5, 0x3a));
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
