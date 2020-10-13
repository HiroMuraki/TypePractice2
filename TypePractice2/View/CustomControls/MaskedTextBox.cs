using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [TemplatePart(Name = "PART_BackgroundDisplay", Type = typeof(Label))]
    public class MaskedTextBox : TextBox {
        /// <summary>
        /// 输入框背景文本
        /// </summary>
        public string BackgroundDisplay {
            get {
                return (string)GetValue(BackgroundDisplayProperty);
            }
            set {
                SetValue(BackgroundDisplayProperty, value);
            }
        }
        public static readonly DependencyProperty BackgroundDisplayProperty =
            DependencyProperty.Register("BackgroundDisplay", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(""));

        /// <summary>
        /// 是否启用背景文本
        /// </summary>
        public Visibility BackgroundDisplayVisibility {
            get {
                return (Visibility)GetValue(BackgroundDisplayVisibilityProperty);
            }
            set {
                SetValue(BackgroundDisplayVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty BackgroundDisplayVisibilityProperty =
            DependencyProperty.Register("BackgroundDisplayVisibility", typeof(Visibility), typeof(MaskedTextBox), new PropertyMetadata(Visibility.Visible));

        static MaskedTextBox() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaskedTextBox), new FrameworkPropertyMetadata(typeof(MaskedTextBox)));
        }
    }
}
