using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Markup;
using System.Xml.Schema;
using TypePractice2.Model;

namespace TypePractice2.ViewModel {
    public enum PracticeStatus {
        Ready,
        Prcessing,
        Finished
    }
    public class TypeViewModel : ViewModelBase {
        #region 后备字段
        private List<WordViewModel> _wordList;
        private List<WordViewModel> _practiceWords;
        private DateTime startTime;
        private string _stars;
        private bool _isImmerseMode;
        private bool _isLockMode;
        private bool _isShuffleMode;
        private int _correctCount;
        private int _currentIndex;
        private string _currentInput;
        private PracticeStatus _practiceStatus;
        #endregion

        #region 属性
        /// <summary>
        /// 储存的单词列表
        /// </summary>
        public List<WordViewModel> WordList {
            get {
                return this._wordList;
            }
            set {
                this._wordList = value;
            }
        }

        /// <summary>
        /// 当前练习单词列表
        /// </summary>
        public List<WordViewModel> PracticeWords {
            get {
                return this._practiceWords;
            }
            set {
                this._practiceWords = value;
            }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public PracticeStatus PracticeStatus {
            get {
                return this._practiceStatus;
            }
            private set {
                this._practiceStatus = value;
                this.OnPropertyChanged(nameof(PracticeStatus));
            }
        }

        /// <summary>
        /// 是否启用沉浸模式
        /// </summary>
        public bool IsImmerseMode {
            get {
                return this._isImmerseMode;
            }
            set {
                this._isImmerseMode = value;
                this.OnPropertyChanged(nameof(IsImmerseMode));
            }
        }

        /// <summary>
        /// 是否启用锁定模式
        /// </summary>
        public bool IsLockMode {
            get {
                return this._isLockMode;
            }
            set {
                this._isLockMode = value;
                this.OnPropertyChanged(nameof(IsLockMode));
                this.OnPropertyChanged(nameof(Process));
            }
        }

        /// <summary>
        /// 是否启用随机模式，当设置值时，会同时更新PracticeWords
        /// </summary>
        public bool IsShuffleMode {
            get {
                return this._isShuffleMode;
            }
            set {
                this._isShuffleMode = value;
                if (this.IsShuffleMode == true) {
                    this.ToShuffle();
                } else {
                    this.ToOrder();
                }
                this.OnPropertyChanged(nameof(IsShuffleMode));
                this.ResetPractice();
            }
        }

        /// <summary>
        /// 获取当前大小
        /// </summary>
        public int Size {
            get {
                return this.WordList.Count;
            }
        }

        /// <summary>
        /// 正确计数器
        /// </summary>
        public int CorrectCount {
            get {
                return this._correctCount;
            }
            private set {
                this._correctCount = value;
                this.OnPropertyChanged(nameof(CorrectCount));
            }
        }

        /// <summary>
        /// 当前输入
        /// </summary>
        public string CurrentInput {
            get {
                return this._currentInput;
            }
            set {
                this._currentInput = value;
                this.OnPropertyChanged(nameof(CurrentInput));
            }
        }

        /// <summary>
        /// 当前单词位置
        /// </summary>
        public int CurrentIndex {
            get {
                return this._currentIndex;
            }
            set {
                if (value < 0 || value >= this.Size) {
                    return;
                }
                this._currentIndex = value;
                this.ResetInput();
                this.OnPropertyChanged(nameof(CurrentIndex));
                this.OnPropertyChanged(nameof(Process));
                this.OnPropertyChanged(nameof(PreviousWord));
                this.OnPropertyChanged(nameof(NextWord));
                this.OnPropertyChanged(nameof(CurrentWord));
            }
        }

        /// <summary>
        /// 当前单词
        /// </summary>
        public WordViewModel CurrentWord {
            get {
                return this.PracticeWords[CurrentIndex];
            }
        }

        /// <summary>
        /// 上一个单词
        /// </summary>
        public WordViewModel PreviousWord {
            get {
                if (this.CurrentIndex == 0) {
                    return new WordViewModel("", "");
                }
                return this.PracticeWords[this.CurrentIndex - 1];
            }
        }

        /// <summary>
        /// 下一个单词
        /// </summary>
        public WordViewModel NextWord {
            get {
                if (this.CurrentIndex + 1 == this.Size) {
                    return new WordViewModel("", "");
                }
                return this.PracticeWords[this.CurrentIndex + 1];
            }
        }

        /// <summary>
        /// 当前进度
        /// </summary>
        public string Process {
            get {
                if (this.IsLockMode) {
                    return "锁定";
                }
                return $"{this.CurrentIndex + 1}/{this.Size}";
            }
        }

        /// <summary>
        /// 评价
        /// </summary>
        public string Stars {
            get {
                return this._stars;
            }
            private set {
                this._stars = value;
                this.OnPropertyChanged(nameof(Stars));
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数，不可调用
        /// </summary>
        private TypeViewModel() {
            this.WordList = new List<WordViewModel>();
            this.PracticeWords = new List<WordViewModel>();
        }
        /// <summary>
        /// 构造函数，传入一个可遍历的的单词列表
        /// </summary>
        /// <param name="wordList">单词列表</param>
        public TypeViewModel(IEnumerable<WordModel> wordList) : this() {
            foreach (var word in wordList) {
                this.WordList.Add(new WordViewModel(word));
            }
            this.IsLockMode = false;
            this.IsShuffleMode = false;
            this.ResetPractice();
        }
        public TypeViewModel(IEnumerable<WordViewModel> wordList) : this() {
            foreach (var word in wordList) {
                this.WordList.Add(word);
            }
            this.IsLockMode = false;
            this.IsShuffleMode = false;
            this.ResetPractice();
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 重置练习
        /// </summary>
        public void ResetPractice() {
            if (WordList.Count == 0) {
                throw new InvalidOperationException("无法开始练习，因为练习列表为空");
            }
            this.CurrentIndex = 0;
            this.CorrectCount = 0;
            this.Stars = "";
            this.ResetInput();
            this.PracticeStatus = PracticeStatus.Ready;
        }

        /// <summary>
        /// 移动至上一个单词，如果该单词已经为第一个单词，则跳转到最后一个单词
        /// </summary>
        public void Previous() {
            if (this.CurrentIndex <= 0) {
                this.CurrentIndex = this.Size - 1;
            } else {
                this.CurrentIndex -= 1;
            }
        }

        /// <summary>
        /// 移动至下一个单词，如果该单词已经为最后一个单词，则跳转至第一个单词
        /// </summary>
        public void Next() {
            if (this.CurrentIndex + 1 >= this.Size) {
                this.CurrentIndex = 0;
            } else {
                this.CurrentIndex += 1;
            }
        }

        /// <summary>
        /// 执行比较，如果当前输入与当前单词源相同，则增加一个正确单词计数
        /// </summary>
        public void Compare() {
            if (this.PracticeStatus == PracticeStatus.Finished) {
                return;
            }

            if (this.PracticeStatus != PracticeStatus.Prcessing) {
                this.PracticeStatus = PracticeStatus.Prcessing;
                startTime = DateTime.Now;
            }

            if (this.CurrentInput.Length != this.CurrentWord.Source.Length) {
                return;
            }

            if (IsLockMode) {
                this.ResetInput();
                return;
            }

            if (this.CurrentWord.Source == this.CurrentInput) {
                this.CorrectCount += 1;
            }
            if (this.CurrentIndex + 1 == this.Size) {
                this.Calculate();
            } else {
                this.Next();
                this.ResetInput();
            }
        }

        /// <summary>
        /// 切换沉浸模式
        /// </summary>
        public void ToggleImmerseMode() {
            this.IsImmerseMode = !this.IsImmerseMode;
        }

        /// <summary>
        /// 切换随机模式
        /// </summary>
        public void ToggleShuffleMode() {
            this.IsShuffleMode = !this.IsShuffleMode;
        }

        /// <summary>
        /// 切换锁定模式
        /// </summary>
        public void ToggleLockMode() {
            this.IsLockMode = !this.IsLockMode;
        }
        #endregion

        #region 私有辅助方法
        /// <summary>
        /// 打乱单词顺序
        /// </summary>
        private void ToShuffle() {
            this.ToOrder();
            Random rnd = new Random();
            for (int i = 0; i < this.Size; i++) {
                int indexA = i;
                int indexB = rnd.Next(i, this.Size);
                WordViewModel T = this.PracticeWords[indexA];
                this.PracticeWords[indexA] = this.PracticeWords[indexB];
                this.PracticeWords[indexB] = T;
            }
            this.CurrentIndex = 0;
        }

        /// <summary>
        /// 重置单词顺序
        /// </summary>
        private void ToOrder() {
            this.PracticeWords.Clear();
            for (int i = 0; i < this.Size; i++) {
                PracticeWords.Add(this.WordList[i]);
            }
            this.CurrentIndex = 0;
        }

        /// <summary>
        /// 结算练习，获取星级评级
        /// </summary>
        private void Calculate() {
            this.PracticeStatus = PracticeStatus.Finished;
            double maxPracticeLevel = 4;
            double maxCorrectRate = 1;
            double maxPlusCorrectRate = 0.25;
            double maxTimeRateMultiplier = 3;
            double typeSpeed = 2.5;

            //词量评价
            int practiceLevel = 1;
            for (int i = 0; i < (this.Size / 15); i++) {
                practiceLevel += 1;
            }

            //计算准确率评价
            //直接以正确单词数除以总单词数得到准确率，根据词量进行额外准确率加成
            double correctRate = this.CorrectCount / (double)this.Size;
            double plusCorrectRate = (practiceLevel * 0.1 < maxPlusCorrectRate ? practiceLevel * 0.1 : maxPlusCorrectRate);
            correctRate += plusCorrectRate;
            correctRate = correctRate < maxCorrectRate ? correctRate : maxCorrectRate;


            //用时评价
            //如果词量评级大于最大评级，则将额外的级别作为加成
            double usingTime = (DateTime.Now - this.startTime).TotalSeconds;
            double timeRate = usingTime / (typeSpeed * this.Size);
            if (practiceLevel > maxPracticeLevel) {
                double plusMultiplier = 1 + (practiceLevel - maxPracticeLevel) * 0.1;
                timeRate *= (plusMultiplier < maxTimeRateMultiplier ? plusMultiplier : maxTimeRateMultiplier);
            }


            //计算评分
            int scores = (int)(100 * correctRate * (practiceLevel / maxPracticeLevel) * timeRate);

            StringBuilder sb = new StringBuilder(5);
            for (int i = 0; i < (scores / 20); i++) {
                sb.Append("\u272f");
            }
            if (scores % 20 > 0) {
                sb.Append("\u2730");
            }

            Stars = sb.ToString();
        }

        /// <summary>
        /// 重置用户当前输入
        /// </summary>
        private void ResetInput() {
            this.CurrentInput = "";
        }
        #endregion

        #region 公共静态方法
        /// <summary>
        /// 读取文件，并按行读取生成单词
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IEnumerable<WordModel> ReadFromFile(string filePath) {
            string fileExtension = Path.GetExtension(filePath).ToLower();
            IEnumerable<WordModel> wordList;
            switch (fileExtension) {
                case ".json":
                    wordList = ReadJSON(filePath);
                    break;
                case ".txt":
                    wordList = ReadText(filePath);
                    break;
                default:
                    wordList = ReadText(filePath);
                    break;
            }
            foreach (var word in wordList) {
                yield return word;
            }
        }

        public static IEnumerable<WordModel> ReadJSON(string filePath) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException(filePath);
            }
            List<WordModel> wordList;
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<WordModel>));
                try {
                    wordList = serializer.ReadObject(file) as List<WordModel>;
                }
                catch {
                    wordList = new List<WordModel>() { new WordModel() { Source = "YZTXDY", Meaning = "The First Motto" } };
                }

            }
            foreach (var word in wordList) {
                yield return word;
            }
        }

        public static IEnumerable<WordModel> ReadText(string filePath) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException($"未找到文件{filePath}");
            }
            using (StreamReader reader = new StreamReader(filePath)) {
                while (!reader.EndOfStream) {
                    var wordData = reader.ReadLine().Trim().Split('#');
                    if (wordData.Length >= 2) {
                        yield return new WordModel() { Source = wordData[0], Meaning = wordData[1] };
                    } else if (wordData.Length >= 1) {
                        yield return new WordModel() { Source = wordData[0], Meaning = "" };
                    } else {
                        continue;
                    }
                }
            }
        }
        #endregion
    }
}
