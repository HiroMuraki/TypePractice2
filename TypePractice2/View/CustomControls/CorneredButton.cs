using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TypePractice2.View {
    public class CorneredButton : Button {
        /// <summary>
        /// 用于表示按钮的圆角
        /// </summary>
        public CornerRadius CornerRadius {
            get {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set {
                SetValue(CornerRadiusProperty, value);
            }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CorneredButton), new PropertyMetadata(new CornerRadius(0)));

        static CorneredButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CorneredButton), new FrameworkPropertyMetadata(typeof(CorneredButton)));
        }
    }
}
