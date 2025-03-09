using Microsoft.Win32;
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
using WordNotes.Models;

namespace WordNotes.Views
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        private MainWindow _mainWindow;
        private AppSettings _appSettings;

        public SettingsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _appSettings = _mainWindow.appSettings;

            // 绑定设置数据
            DataContext = _appSettings;
        }

        // 增加单词切换时间
        private void IncreaseInterval_Click(object sender, RoutedEventArgs e)
        {
            _appSettings.TimerIntervalSeconds++;
            IntervalTextBox.Text = _appSettings.TimerIntervalSeconds.ToString();
        }

        // 减少单词切换时间
        private void DecreaseInterval_Click(object sender, RoutedEventArgs e)
        {
            if (_appSettings.TimerIntervalSeconds > 1)
            {
                _appSettings.TimerIntervalSeconds--;
                IntervalTextBox.Text = _appSettings.TimerIntervalSeconds.ToString();
            }
        }

        // 选择词典文件
        private void SelectDictionaryFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "excel文件 (*.xlsx)|*.xlsx|所有文件 (*.*)|*.*",
                Title = "选择词典文件"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _appSettings.DictionaryPath = openFileDialog.FileName;
                DictionaryPathTextBox.Text = openFileDialog.FileName;
            }
        }

        // 保存设置
        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.settingsService.SaveSettings(_appSettings);
            _mainWindow.InitSettings();
        }


        // 恢复默认设置
        private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            // 弹出确认对话框
            var result = MessageBox.Show(
                "确定要恢复默认设置吗？收藏列表也将被清除",
                "恢复默认设置",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // 如果用户选择“是”，则恢复默认设置
            if (result == MessageBoxResult.Yes)
            {
                _appSettings = new AppSettings(); // 重置为默认配置
                DataContext = _appSettings;    // 更新绑定
                _mainWindow.settingsService.SaveSettings(_appSettings);
                _mainWindow.InitSettings();
            }
            
        }

        // 页面离开时保存设置
        private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content != this) // 确保只在离开页面时保存
            {
                _mainWindow.settingsService.SaveSettings(_appSettings);
            }
        }
    }
}
