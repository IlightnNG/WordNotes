using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WordNotes.Models;
using WordNotes.Services;

namespace WordNotes.Views
{
    /// <summary>
    /// FavoritWordsPage.xaml 的交互逻辑
    /// </summary>
    public partial class FavoriteWordsPage : Page
    {
        private MainWindow _mainWindow;
        // 便签窗口
        private NoteWindow _noteWindow;

        public FavoriteWordsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            // 加载已显示单词列表
            LoadFavoriteWords();

        }

        private void LoadFavoriteWords()
        {
            // 根据索引获取已显示单词
            var favoriteWords = _mainWindow.wordQueueService.favoriteQueue
                .Select(index => _mainWindow.words[index])
                .ToList();

            // 倒序单词列表
            favoriteWords.Reverse();

            // 绑定到界面
            FavoriteWordsList.ItemsSource = favoriteWords;
        }

        // 搜索按钮
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取当前显示的英文单词
            Button button = sender as Button;
            var dataContext = button.DataContext;
            if (dataContext != null && dataContext is Word word && word.English != null)
            {

                // 调用浏览器打开有道词典搜索页面
                if (!string.IsNullOrEmpty(word.English))
                {
                    string url = $"https://dict.youdao.com/result?word={word.English}&lang=en";
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("当前没有显示的单词。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // 实心五角星按钮点击事件
        private void StarButton_Click(object sender, RoutedEventArgs e)
        {
            // 将 sender 转换为 ToggleButton
            ToggleButton toggleButton = sender as ToggleButton;


            if (toggleButton != null)
            {
                // 获取与 ToggleButton 关联的数据上下文
                var dataContext = toggleButton.DataContext;

                if (dataContext != null && dataContext is Word word && word.English != null)
                {
                    // 获取当前单词的索引
                    int currentIndex = _mainWindow.words.FindIndex(w => w.English == word.English);

                    // 如果按钮被选中（实心五角星），将索引添加到记忆队列
                    if (toggleButton.IsChecked == true && currentIndex != -1 && !_mainWindow.favoriteQueue.Contains(currentIndex))
                    {
                        _mainWindow.favoriteQueue.Add(currentIndex);
                        // 持久化
                        _mainWindow.settingsService.SaveSettings(_mainWindow.appSettings);
                    }
                    // 如果按钮被取消（空心五角星），从记忆队列中移除索引
                    else if (toggleButton.IsChecked == false && currentIndex != -1)
                    {
                        _mainWindow.favoriteQueue.Remove(currentIndex);
                        _mainWindow.settingsService.SaveSettings(_mainWindow.appSettings);
                    }
                }
            }
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取当前显示的英文单词
            Button button = sender as Button;
            var dataContext = button.DataContext;
            if (dataContext != null && dataContext is Word word && word.English != null)
            {
                _noteWindow = new NoteWindow(word, _mainWindow.Left, _mainWindow.Top);
                _noteWindow.Owner = _mainWindow; // 设置主窗口为菜单窗口的父窗口
                _noteWindow.Closed += (s, args) => _noteWindow = null; // 窗口关闭时清空引用
                _noteWindow.Show();
            }
                
        }

    }
}
