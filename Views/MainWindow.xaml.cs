using System.Windows;
using System.Windows.Threading;
using WordNotes.Models;
using WordNotes.Services;
using System.Diagnostics;
using WordNotes.Views.Base;
using System.Windows.Controls;

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
    public DictionaryLoaderService dictionaryLoaderService = new DictionaryLoaderService();

    // 单词队列
    public WordQueueService wordQueueService;
    private int currentWordIndex;
    public Word currentWord;

    // 随机数生成器
    private Random random = new Random();

    // 定时器
    private DispatcherTimer timer = new DispatcherTimer();
    private int interval;

    // 收藏队列
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
        // 初始化定时器
        
        timer.Tick += Timer_Tick;
        timer.Stop();
        timer.Start();
    }

    public void InitSettings()
    {

        appSettings = settingsService.LoadSettings();
        // 从 CSV 或 Excel 文件加载单词列表
        words = dictionaryLoaderService.LoadWords(appSettings);

        //// 设置窗口位置（任务栏左侧）
        this.Left = appSettings.MainWindowLeft == 0 ? 0 : appSettings.MainWindowLeft;
        this.Top = appSettings.MainWindowTop == 0 ? SystemParameters.WorkArea.Height - this.Height : appSettings.MainWindowTop;

        // 可拖动范围
        _dragArea = new Rect(0, 0, 200, 100);

        // 加载收藏单词表
        favoriteQueue = appSettings.FavoriteIndices;

        // 时间间隔
        interval = appSettings.TimerIntervalSeconds;
        // 单词生成器
        wordQueueService = new WordQueueService(appSettings, words, settingsService);

        timer.Interval = TimeSpan.FromSeconds(interval); // 每n秒更换一次单词

    }

    // 计时器
    private void Timer_Tick(object sender, EventArgs e)
    {
        ShowRandomWord();
    }

    private void ShowRandomWord()
    {
        
        currentWordIndex = wordQueueService.NextWord();
        currentWord = words[currentWordIndex];

        DataContext = currentWord;
        //EnglishWordTextBlock.Text = nextWord.English;
        //ExplanationTextBlock.Text = nextWord.Chinese;
        //StarButton.IsChecked = nextWord.IsFavorite;
    }


    // 点击英语单词搜索该单词
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        // 获取当前显示的英文单词

        if (currentWord != null)
        {

            // 调用浏览器打开有道词典搜索页面
            if (!string.IsNullOrEmpty(currentWord.English))
            {
                string url = $"https://dict.youdao.com/result?word={currentWord.English}&lang=en";
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("当前没有显示的单词。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    private void PausePlayButton_Checked(object sender, RoutedEventArgs e)
    {
        timer.Stop();
    }
    private void PausePlayButton_Unchecked(object sender, RoutedEventArgs e)
    {
        timer.Start();
    }



    // 实心五角星按钮点击事件
    private void StarButton_Click(object sender, RoutedEventArgs e)
    {
        // 获取当前单词的索引
        int currentIndex = currentWordIndex;

        // 如果按钮被选中（实心五角星），将索引添加到记忆队列
        if (StarButton.IsChecked == true && currentIndex != -1 && !favoriteQueue.Contains(currentIndex))
        {
            // 更新收藏属性
            words[currentIndex].IsFavorite = true;
            favoriteQueue.Add(currentIndex);
            // 持久化
            settingsService.UpdateSetting("FavoriteIndices", favoriteQueue);
        }
        // 如果按钮取消选中（空心五角星），从记忆队列中移除索引
        else if (StarButton.IsChecked == false && currentIndex != -1)
        {
            words[currentIndex].IsFavorite = false;

            favoriteQueue.Remove(currentIndex);
            settingsService.UpdateSetting("FavoriteIndices", favoriteQueue);
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
        _noteWindow = new NoteWindow(this.currentWord,this.Left,this.Top);
        _noteWindow.Owner = this; // 设置主窗口为菜单窗口的父窗口
        _noteWindow.Closed += (s, args) => _noteWindow = null; // 窗口关闭时清空引用
        _noteWindow.Show();
    }

    // 存储窗口位置
    public override void SaveWindowPosition()
    {
        appSettings.MainWindowLeft = this.Left;
        appSettings.MainWindowTop = this.Top;
        settingsService.UpdateSetting("MainWindowLeft", appSettings.MainWindowLeft);
        settingsService.UpdateSetting("MainWindowTop", appSettings.MainWindowTop);

    }



}