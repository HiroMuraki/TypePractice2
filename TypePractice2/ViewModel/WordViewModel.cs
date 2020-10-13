using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TypePractice2.Model;

namespace TypePractice2.ViewModel {
    public struct WordViewModel {
        private string _source;
        private string _meaning;
        public string Source {
            get {
                return this._source;
            }
        }
        public string Meaning {
            get {
                return this._meaning;
            }
        }

        /// <summary>
        /// 构造函数，传入一个单词模型
        /// </summary>
        /// <param name="word"></param>
        public WordViewModel(WordModel word) {
            this._source = word.Source;
            this._meaning = word.Meaning;
        }
        /// <summary>
        /// 构造函数，传入一个单词源和其含义
        /// </summary>
        /// <param name="source">单词</param>
        /// <param name="meaning">含义</param>
        public WordViewModel(string source,string meaning) {
            this._source = source;
            this._meaning = meaning;
        }
        /// <summary>
        /// 构造函数，传入一个以#分割源和其含义的字符串
        /// </summary>
        /// <param name="wordWithMeaning">以#分割源和其含义的字符串</param>
        public WordViewModel(string wordWithMeaning) {
            var T = wordWithMeaning.Split('#');
            if (T.Length >= 2) {
                this._source = T[0].Trim();
                this._meaning = T[1].Trim();
            } else if(T.Length>=1){
                this._source = T[0].Trim();
                this._meaning = "";
            } else {
                throw new ArgumentException("单词解析出错");
            }
        }

        /// <summary>
        /// </summary>
        /// 运算符重载，用于比较单词是否相同
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(WordViewModel a, WordViewModel b) {
            return a.Source == b.Source;
        }
        public static bool operator !=(WordViewModel a, WordViewModel b) {
            return !(a == b);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
    }
}
