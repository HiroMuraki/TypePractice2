using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using TypePractice2.View;
using TypePractice2.ViewModel;

namespace TypePractice2 {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {
        private readonly string WordPackIdentifier = "[WordPack]";
        private readonly string WordPackPath = Directory.GetCurrentDirectory();

        private void Application_Startup(object sender, StartupEventArgs e) {
            List<WordViewModel> wordList = null;
            if (e.Args.Length > 0) {
                foreach (var arg in e.Args) {
                    if (arg.StartsWith("-")) {
                        wordList = GetWordList(arg.Substring(1));
                    }
                }
            }
            if (wordList == null) {
                wordList = GetAWordList();
            }

            MainWindow window = new MainWindow(wordList);
            window.Show();
            window.TypePractice.ResetPractice();
        }

        /// <summary>
        /// 获取一个单词包
        /// </summary>
        /// <returns></returns>
        public List<WordViewModel> GetAWordList() {
            List<string> wordPackList = new List<string>();
            foreach (var file in Directory.GetFiles(WordPackPath)) {
                string filename = Path.GetFileName(file);
                if (filename.StartsWith(WordPackIdentifier)) {
                    wordPackList.Add(file);
                }
            }

            List<WordViewModel> wordList;
            if (wordPackList.Count > 0) {
                string currentFile = wordPackList[new Random().Next(0, wordPackList.Count)];
                wordList = new List<WordViewModel>(TypeViewModel.ReadFromFile(currentFile));
            } else {
                wordList = new List<WordViewModel>() { new WordViewModel(new Model.WordModel() { Source = "YZTXDY", Meaning = "The first Motto" }) };
            }
            return wordList;
        }

        /// <summary>
        /// 获取指定单词包
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public List<WordViewModel> GetWordList(string filepath) {
            return new List<WordViewModel>(TypeViewModel.ReadFromFile(filepath));
        }
    }
}
