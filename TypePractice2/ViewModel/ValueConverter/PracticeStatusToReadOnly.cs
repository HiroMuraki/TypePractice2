using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TypePractice2.ViewModel.ValueConverter {
    [ValueConversion(typeof(PracticeStatus), typeof(bool))]
    public class PracticeStatusToReadOnly : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                switch ((PracticeStatus)value) {
                    case PracticeStatus.Finished:
                        return true;
                    case PracticeStatus.Prcessing:
                        return false;
                    case PracticeStatus.Ready:
                        return false;
                    default:
                        return false;
                }
            }
            catch {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
