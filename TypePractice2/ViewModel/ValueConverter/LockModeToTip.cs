using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TypePractice2.ViewModel.ValueConverter {
    [ValueConversion(typeof(bool), typeof(string))]
    public class LockModeToTip : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                switch ((bool)value) {
                    case true:
                        return "锁定";
                    case false:
                        return "";
                    default:
                        return "未知";
                }
            }
            catch {
                return "未知";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
