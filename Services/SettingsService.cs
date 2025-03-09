using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using WordNotes.Models;
namespace WordNotes.Services
{
    public class SettingsService
    {
        private readonly string _filePath;

        public SettingsService(string filePath)
        {
            if (IsDevelopmentEnvironment())
            {
                // 开发环境：使用项目目录
                var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));
                _filePath = Path.Combine(projectRoot, filePath);
            }
            else
            {
                // 生产环境：使用用户配置文件目录，实测无权限
                //var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //var appName = "WordNotes"; // 你的应用程序名称
                //var appDirectory = Path.Combine(appDataPath, appName);

                var appDirectory = GetApplicationRootDirectory();

                Directory.CreateDirectory(appDirectory);
                _filePath = Path.Combine(appDirectory, filePath);
            }
            EnsureFileExists();

        }

        private bool IsDevelopmentEnvironment()
        {
        #if DEBUG
            return true;
        #else
            return false;
        #endif
        }

        /// <summary>
        /// 获取应用程序的根目录
        /// </summary>
        private string GetApplicationRootDirectory()
        {
            // 使用 AppContext.BaseDirectory 获取应用程序基目录
            var baseDirectory = AppContext.BaseDirectory;

            // 如果是单文件发布，BaseDirectory 可能是临时目录，需要进一步处理
            if (baseDirectory.Contains("App"))
            {
                // 获取当前进程的可执行文件路径
                var processPath = Process.GetCurrentProcess().MainModule.FileName;
                return Path.GetDirectoryName(processPath);
            }

            return baseDirectory;
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                var defaultSettings = new AppSettings();
                SaveSettings(defaultSettings);
            }
        }
        /// <summary>
        /// 从 JSON 文件读取设置
        /// </summary>
        public AppSettings LoadSettings()
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载 JSON 文件失败: {ex.Message}");
                return new AppSettings();
            }
        }

        /// <summary>
        /// 将设置保存到 JSON 文件
        /// </summary>
        public void SaveSettings(AppSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true // 使 JSON 文件格式化（易于阅读）
                });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存 JSON 文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 修改 JSON 文件中的特定属性
        /// </summary>
        public void UpdateSetting(string propertyName, object value)
        {
            try
            {
                // 读取当前设置
                var settings = LoadSettings();

                // 使用反射找到属性并更新值
                var property = typeof(AppSettings).GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    // 转换值的类型以匹配属性类型
                    var convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(settings, convertedValue);

                    // 保存更新后的设置
                    SaveSettings(settings);
                }
                else
                {
                    Console.WriteLine($"未找到可写的属性: {propertyName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新 JSON 属性失败: {ex.Message}");
            }
        }
    }
}
