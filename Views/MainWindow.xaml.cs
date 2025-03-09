using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using WordNotes.Models;
using WordNotes.Services;
using System.Diagnostics;
using WordNotes.Views.Base;

namespace WordNotes.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : BaseWindow
{

    // 用户设置
    public SettingsService settingsService = new SettingsService("Config/settings.json");
    public AppSettings appSettings;
    // 单词库
    public List<Word> words;
    WordLoaderService wordLoaderService = new WordLoaderService();

    // 随机数生成器
    private Random random = new Random();

    // 定时器
    private DispatcherTimer timer = new DispatcherTimer();
    private int interval;

    // 展示队列
    public List<int> historyQueue = new List<int>();
    // 记忆队列
    public List<int> favoriteQueue = new List<int>();

    // 菜单窗口
    private MenuWindow _menuWindow;
    // 便签窗口
    private NoteWindow _noteWindow;


    public MainWindow()
    {

        InitializeComponent();

        InitSettings();
        // 显示第一个单词
        ShowRandomWord();

        

    }

    public void InitSettings()
    {

        appSettings = settingsService.LoadSettings();

        //// 设置窗口位置（任务栏左侧）
        this.Left = appSettings.MainWindowLeft == 0 ? 0 : appSettings.MainWindowLeft;
        this.Top = appSettings.MainWindowTop == 0 ? SystemParameters.WorkArea.Height - this.Height : appSettings.MainWindowTop;

        // 可拖动范围
        _dragArea = new Rect(0, 0, 200, 100);

        // 从 CSV 或 Excel 文件加载单词列表
        words = wordLoaderService.LoadWords(appSettings);

        // 加载收藏单词表
        favoriteQueue = appSettings.FavoriteIndices;

        // 时间间隔
        interval = appSettings.TimerIntervalSeconds;

        // 初始化定时器
        
        timer.Interval = TimeSpan.FromSeconds(interval); // 每5秒更换一次单词
        timer.Tick += Timer_Tick;
        timer.Stop();
        timer.Start();

    }

    // 计时器
    private void Timer_Tick(object sender, EventArgs e)
    {
        ShowRandomWord();
    }

    private void ShowRandomWord()
    {
        int index = 0;
        if (favoriteQueue.Count > 0 && random.Next(4) == 0) // 进入记忆队列概率
        {
            // 从队列中随机获取一个单词
            index = favoriteQueue[random.Next(favoriteQueue.Count)];

            // 将星确认
            StarButton.IsChecked = true;
        }
        else
        {    // 随机选择一个单词
            index = random.Next(words.Count);

            // 将星取消
            StarButton.IsChecked = false;
        }    
        var CurrentWord = words[index];


        EnglishWordTextBlock.Text = CurrentWord.English;
        ExplanationTextBlock.Text = CurrentWord.Chinese;

        // 展示队列调整
        historyQueue.Add(index);
        if (historyQueue.Count>10)
        {
            historyQueue.RemoveAt(0);
        }

        
    }
    

    // 搜索按钮
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        // 获取当前显示的英文单词
        string word = EnglishWordTextBlock.Text;

        // 调用浏览器打开有道词典搜索页面
        if (!string.IsNullOrEmpty(word))
        {
            string url = $"https://dict.youdao.com/result?word={word}&lang=en";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        else
        {
            MessageBox.Show("当前没有显示的单词。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // 实心五角星按钮点击事件
    private void StarButton_Click(object sender, RoutedEventArgs e)
    {
        // 获取当前单词的索引
        int currentIndex = words.FindIndex(w => w.English == EnglishWordTextBlock.Text);

        // 如果按钮被选中（实心五角星），将索引添加到记忆队列
        if (StarButton.IsChecked == true && currentIndex != -1 && !favoriteQueue.Contains(currentIndex))
        {
            // 更新收藏属性
            words[currentIndex].IsFavorite = true;
            favoriteQueue.Add(currentIndex);
            // 持久化
            settingsService.SaveSettings(appSettings);


        }
        // 如果按钮取消选中（空心五角星），从记忆队列中移除索引
        else if (StarButton.IsChecked == false && currentIndex != -1)
        {
            words[currentIndex].IsFavorite = false;

            favoriteQueue.Remove(currentIndex);
            settingsService.SaveSettings(appSettings);
        }
    }

    // 下一个单词按钮
    private void NextWordButton_Click(object sender, RoutedEventArgs e)
    {
        // 重置计时器
        timer.Stop();
        timer.Start();

        ShowRandomWord();
    }

    // 菜单窗口按钮
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        // 如果菜单窗口未打开或已关闭
        if (_menuWindow == null || !_menuWindow.IsLoaded)
        {
            // 创建新的菜单窗口
            _menuWindow = new MenuWindow(this);
            _menuWindow.Owner = this; // 设置主窗口为菜单窗口的父窗口
            _menuWindow.Closed += (s, args) => _menuWindow = null; // 窗口关闭时清空引用
            _menuWindow.Show();
        }
        else
        {
            // 如果菜单窗口已打开，则关闭它
            _menuWindow.Close();
            _menuWindow = null;
        }
    }

    private void NoteButton_Click(object sender, RoutedEventArgs e)
    {
        _noteWindow = new NoteWindow(this);
        _noteWindow.Owner = this; // 设置主窗口为菜单窗口的父窗口
        _noteWindow.Closed += (s, args) => _noteWindow = null; // 窗口关闭时清空引用
        _noteWindow.Show();
    }

    // 存储窗口位置
    public override void SaveWindowPosition()
    {
        appSettings.MainWindowLeft = this.Left;
        appSettings.MainWindowTop = this.Top;
        settingsService.SaveSettings(appSettings);

    }



}