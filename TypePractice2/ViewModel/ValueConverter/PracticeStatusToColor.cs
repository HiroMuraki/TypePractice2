using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using TypePractice2.ViewModel;

namespace TypePractice2.ViewModel.ValueConverter {
    [ValueConversion(typeof(PracticeStatus), typeof(SolidColorBrush))]
    public class PracticeStatusToColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                switch ((PracticeStatus)value) {
                    case PracticeStatus.Ready:
                        return new SolidColorBrush(Color.FromRgb(0xf9, 0xc5, 0x3a));
                    case PracticeStatus.Prcessing:
                        return new SolidColorBrush(Color.FromRgb(0xbd, 0x2f, 0x54));
                    case PracticeStatus.Finished:
                        return new SolidColorBrush(Color.FromRgb(0x15, 0xb9, 0x79));
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
