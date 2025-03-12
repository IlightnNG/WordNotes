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
using System.Windows.Shapes;
using WordNotes.Models;
using WordNotes.Services;
using WordNotes.Views.Base;


namespace WordNotes.Views
{
    /// <summary>
    /// MenuWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MenuWindow : BaseWindow
    {
        private MainWindow _mainWindow;


        public MenuWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            InitSettings();

            // 默认显示已显示单词页面
            ShowHistoryWords(null, null);

        }

        private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void InitSettings()
        {
            // 窗口位置
            this.Left = _mainWindow.appSettings.MenuWindowLeft == 0 ? 0 : _mainWindow.appSettings.MenuWindowLeft;
            this.Top = _mainWindow.appSettings.MenuWindowTop == 0 ? SystemParameters.WorkArea.Height - this.Height - 100 : _mainWindow.appSettings.MenuWindowTop;

            // 可拖动范围
            _dragArea = new Rect(0, 30, 180, 30);
        }

        // 显示已显示单词页面
        private void ShowHistoryWords(object sender, RoutedEventArgs e)
        {
            // 将按钮置于true
            ShowHistoryWordsButton.IsChecked = true;
            // 将其他按钮置于false
            ShowFavoriteWordsButton.IsChecked = false;
            ShowSettingsButton.IsChecked = false;

            PageName.Text = "历史单词";
            ContentFrame.Navigate(new HistoryWordsPage(_mainWindow));
        }

        

        private void ShowFavoriteWords(object sender, RoutedEventArgs e)
        {
            ShowFavoriteWordsButton.IsChecked = true;
            ShowHistoryWordsButton.IsChecked = false;
            ShowSettingsButton.IsChecked = false;

            PageName.Text = "已收藏单词";
            ContentFrame.Navigate(new FavoriteWordsPage(_mainWindow));
        }


        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            ShowSettingsButton.IsChecked = true;
            ShowFavoriteWordsButton.IsChecked = false;
            ShowHistoryWordsButton.IsChecked = false;
            PageName.Text = "设置";
            ContentFrame.Navigate(new SettingsPage(_mainWindow));
        }



        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowHistoryWordsButton.IsChecked == true) ShowHistoryWords(null, null);
            else if (ShowFavoriteWordsButton.IsChecked == true) ShowFavoriteWords(null, null);
        }

        // 存储窗口位置
        public override void SaveWindowPosition()
        {
            _mainWindow.appSettings.MenuWindowLeft = this.Left;
            _mainWindow.appSettings.MenuWindowTop = this.Top;
            _mainWindow.settingsService.UpdateSetting("MenuWindowLeft", _mainWindow.appSettings.MenuWindowLeft);
            _mainWindow.settingsService.UpdateSetting("MenuWindowTop", _mainWindow.appSettings.MenuWindowTop);

        }

        private void ShowSettingsButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
