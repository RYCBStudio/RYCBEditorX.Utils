using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RYCBEditorX.Utils
{
    public static class ConfigManager
    {
        private static readonly string ConfigDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles", "Config");
        private static readonly string ConfigPath = Path.Combine(ConfigDirectory, "Settings.json");

        private static AppConfig _currentConfig;
        private static readonly object _lock = new();
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// 初始化配置管理器
        /// </summary>
        public static void Initialize()
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            if (!File.Exists(ConfigPath))
            {
                _currentConfig = CreateDefaultConfig();
                SaveConfig();
            }
            else
            {
                LoadConfig();
            }
        }

        /// <summary>
        /// 获取当前配置（只读）
        /// </summary>
        public static AppConfig CurrentConfig
        {
            get
            {
                lock (_lock)
                {
                    return _currentConfig;
                }
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public static void LoadConfig()
        {
            try
            {
                lock (_lock)
                {
                    string json = File.ReadAllText(ConfigPath);
                    _currentConfig = JsonSerializer.Deserialize<AppConfig>(json, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                // 如果加载失败，使用默认配置
                _currentConfig = CreateDefaultConfig();
                GlobalConfig.CurrentLogger?.Log($"加载配置文件失败，使用默认配置: {ex.Message}",
                    module: EnumLogModule.CUSTOM, customModuleName: "配置管理");
            }
        }

        /// <summary>
        /// 保存当前配置到文件
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                lock (_lock)
                {
                    string json = JsonSerializer.Serialize(_currentConfig, _jsonOptions);
                    File.WriteAllText(ConfigPath, json);
                }
            }
            catch (Exception ex)
            {
                GlobalConfig.CurrentLogger?.Log($"保存配置文件失败: {ex.Message}",
                    module: EnumLogModule.CUSTOM, customModuleName: "配置管理");
            }
        }

        /// <summary>
        /// 异步保存当前配置到文件
        /// </summary>
        public static async Task SaveConfigAsync()
        {
            try
            {
                string json;
                lock (_lock)
                {
                    json = JsonSerializer.Serialize(_currentConfig, _jsonOptions);
                }

                await File.WriteAllTextAsync(ConfigPath, json);
            }
            catch (Exception ex)
            {
                GlobalConfig.CurrentLogger?.Log($"异步保存配置文件失败: {ex.Message}",
                    module: EnumLogModule.CUSTOM, customModuleName: "配置管理");
            }
        }

        /// <summary>
        /// 更新配置并保存
        /// </summary>
        /// <param name="updateAction">更新配置的回调函数</param>
        public static void UpdateConfig(Action<AppConfig> updateAction)
        {
            lock (_lock)
            {
                updateAction?.Invoke(_currentConfig);
                SaveConfig();
            }
        }

        /// <summary>
        /// 异步更新配置并保存
        /// </summary>
        /// <param name="updateAction">更新配置的回调函数</param>
        public static async Task UpdateConfigAsync(Action<AppConfig> updateAction)
        {
            lock (_lock)
            {
                updateAction?.Invoke(_currentConfig);
            }

            await SaveConfigAsync();
        }

        /// <summary>
        /// 重置为默认配置
        /// </summary>
        public static void ResetToDefault()
        {
            lock (_lock)
            {
                _currentConfig = CreateDefaultConfig();
                SaveConfig();
            }
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        private static AppConfig CreateDefaultConfig()
        {
            return new AppConfig
            {
                Skin = "dark",
                MaximumFileSize = 307200,
                Language = "zh-CN",
                PythonPath = "python",
                AutoSave = new AutoSaveConfig { Enabled = true, Interval = 5 },
                AutoBackup = new AutoBackupConfig { Enabled = true, Interval = 1, Path = "Backup\\" },
                Font = "Microsoft YaHei UI",
                XshdFilePath = "Highlightings\\",
                Editor = new EditorConfig
                {
                    Theme = "IDEA",
                    ShowLineNumber = true,
                    FontName = "Jetbrains Mono",
                    FontSize = 12
                },
                Downloading = new DownloadingConfig
                {
                    ParallelDownload = true,
                    ParallelCount = 8
                }
            };
        }
    }
}
