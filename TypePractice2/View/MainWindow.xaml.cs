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
using TypePractice2.Model;
using TypePractice2.ViewModel;

namespace TypePractice2.View {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public TypeViewModel TypePractice { get; set; }

        private MainWindow() {
            this.TypePractice = new TypeViewModel(
                new List<WordModel>() {
                    new WordModel() { Source = "YZTXDY", Meaning = "The first motto" }
                });
            InitializeComponent();
        }
        public MainWindow(IEnumerable<WordModel> wordList) {
            this.TypePractice = new TypeViewModel(wordList);
            this.InitializeComponent();
        }
        public MainWindow(IEnumerable<WordViewModel> wordList) {
            this.TypePractice = new TypeViewModel(wordList);
            this.InitializeComponent();
        }

        #region 控件操作
        private void UserInput_TextChanged(object sender, TextChangedEventArgs e) {
            TypePractice.Compare();
        }
        #endregion

        #region 窗口操作与快捷键
        private void Window_Move(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }
        private void Window_Close(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.LeftCtrl:
                    this.TypePractice.ToggleShuffleMode();
                    break;
                case Key.RightCtrl:
                    this.TypePractice.ToggleLockMode();
                    break;
                case Key.Enter:
                    if (this.TypePractice.PracticeStatus == PracticeStatus.Finished) {
                        this.TypePractice.ResetPractice();
                        this.InputBox.Focus();
                    } else {
                        this.TypePractice.ToggleImmerseMode();
                    }
                    break;
            }
        }
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                this.TypePractice.Previous();
            } else if (e.Delta < 0) {
                this.TypePractice.Next();
            }
        }
        #endregion
    }
}
