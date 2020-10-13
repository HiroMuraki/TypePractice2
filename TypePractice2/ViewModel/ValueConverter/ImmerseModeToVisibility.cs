using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TypePractice2.ViewModel.ValueConverter {
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class ImmerseModeToVisibility : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                switch ((bool)value) {
                    case true:
                        return Visibility.Collapsed;
                    case false:
                        return Visibility.Visible;
                    default:
                        return Visibility.Visible;
                }
            }
            catch {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
