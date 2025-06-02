using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using WordNotes.Models;
using WordNotes.Views.Base;

namespace WordNotes.Views
{
    /// <summary>
    /// NoteWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NoteWindow : BaseWindow
    {
        //private MainWindow _mainWindow;


        //public NoteWindow(MainWindow mainWindow)
        //{
        //    InitializeComponent();
        //    _mainWindow = mainWindow;

        //    LoadCurrentWords();
        //    SetRandomPositon();
        //    SetRandomColor();
            

        //    // 可拖动范围
        //    _dragArea = new Rect(0, 0, this.Width, this.Height);

        //}

        public NoteWindow(Word word,double left,double top)
        {
            InitializeComponent();
            LoadCurrentWords(word);
            SetRandomPositon(left,top);
            SetRandomColor();


            // 可拖动范围
            _dragArea = new Rect(0, 0, this.Width, this.Height);
        }



        private void LoadCurrentWords(Word word)
        {
            // 获取单词
            EnglishNote.Text = word.English;
            ChineseNote.Text = word.Chinese;
            AdjustGridColumnWidth(EnglishNote.Text);
        }

        private void SetRandomPositon(double left, double top)
        {
            // 生成随机位置
            Random random = new Random();
            this.Left = left + random.Next(0, 100);
            this.Top = top + random.Next(0, 100);
        }

        private void SetRandomColor()
        {
            // 生成随机颜色
            Random random = new Random();
            byte r = (byte)random.Next(0, 256);
            byte g = (byte)random.Next(0, 256);
            byte b = (byte)random.Next(0, 256);
            Color randomColor = Color.FromArgb(255, r, g, b); // 透明度为255（不透明）
            RandomColor.Background = new SolidColorBrush(randomColor);
        }

        // 动态调整 Grid 列宽
        private void AdjustGridColumnWidth(string englishWord)
        {
            // 计算英文单词的宽度
            var formattedText = new FormattedText(
                englishWord,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(EnglishNote.FontFamily, EnglishNote.FontStyle, EnglishNote.FontWeight, EnglishNote.FontStretch),
                EnglishNote.FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            // 设置左边列的宽度
            this.Width = (formattedText.Width + 50)>120? (formattedText.Width + 50) : 120; // 增加 50 像素的边距
        }


        public override void SaveWindowPosition() {}

    }
}
