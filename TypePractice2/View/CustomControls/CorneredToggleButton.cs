using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TypePractice2.View {
    public class CorneredToggleButton : ToggleButton {
        /// <summary>
        /// 设置控件的圆角
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
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CorneredToggleButton), new PropertyMetadata(new CornerRadius(0)));

        static CorneredToggleButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CorneredToggleButton), new FrameworkPropertyMetadata(typeof(CorneredToggleButton)));
        }
    }
}
