using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 配置框架使用示例
    /// 演示如何扩展和复用配置框架
    /// </summary>
    public class ConfigurationFrameworkExample
    {
        /// <summary>
        /// 示例1：创建新的配置模型
        /// </summary>
        public class NotificationConfig
        {
            [Display(Name = "启用桌面通知", Description = "是否在桌面显示通知")]
            public bool DesktopNotificationEnabled { get; set; } = true;

            [Display(Name = "显示通知时长", Description = "通知显示的时间（秒）")]
            [Range(1, 60, ErrorMessage = "通知时长必须在1-60秒之间")]
            public int NotificationDuration { get; set; } = 5;

            [Display(Name = "通知位置", Description = "通知显示的位置")]
            public NotificationPosition Position { get; set; } = NotificationPosition.BottomRight;
        }

        /// <summary>
        /// 示例2：创建对应的配置服务
        /// </summary>
        public static class NotificationConfigService
        {
            private const string CONFIG_FIELD_NAME = "NotificationConfig";

            public static NotificationConfig GetConfig()
            {
                return ConfigurationService.GetConfig<NotificationConfig>(CONFIG_FIELD_NAME);
            }

            public static bool SaveConfig(NotificationConfig config)
            {
                return ConfigurationService.SaveConfig(config, CONFIG_FIELD_NAME);
            }
        }

        /// <summary>
        /// 示例3：枚举类型定义
        /// </summary>
        public enum NotificationPosition
        {
            [Display(Name = "左上角")]
            TopLeft,
            
            [Display(Name = "右上角")]
            TopRight,
            
            [Display(Name = "左下角")]
            BottomLeft,
            
            [Display(Name = "右下角")]
            BottomRight
        }

        /// <summary>
        /// 示例4：复杂配置模型（包含嵌套对象）
        /// </summary>
        public class UserPreferencesConfig
        {
            [Display(Name = "界面主题", Description = "应用程序的界面主题")]
            public ThemeConfig Theme { get; set; } = new ThemeConfig();

            [Display(Name = "快捷键设置", Description = "自定义快捷键")]
            public ShortcutConfig Shortcuts { get; set; } = new ShortcutConfig();

            [Display(Name = "自动保存", Description = "自动保存相关设置")]
            public AutoSaveConfig AutoSave { get; set; } = new AutoSaveConfig();
        }

        public class ThemeConfig
        {
            [Display(Name = "主题颜色", Description = "界面主题颜色")]
            public string PrimaryColor { get; set; } = "#007ACC";

            [Display(Name = "暗色模式", Description = "是否启用暗色模式")]
            public bool DarkMode { get; set; } = false;

            [Display(Name = "字体大小", Description = "界面字体大小")]
            [Range(8, 24, ErrorMessage = "字体大小必须在8-24之间")]
            public int FontSize { get; set; } = 12;
        }

        public class ShortcutConfig
        {
            [Display(Name = "保存快捷键", Description = "保存操作的快捷键")]
            public string SaveShortcut { get; set; } = "Ctrl+S";

            [Display(Name = "新建快捷键", Description = "新建操作的快捷键")]
            public string NewShortcut { get; set; } = "Ctrl+N";

            [Display(Name = "查找快捷键", Description = "查找操作的快捷键")]
            public string FindShortcut { get; set; } = "Ctrl+F";
        }

        public class AutoSaveConfig
        {
            [Display(Name = "启用自动保存", Description = "是否启用自动保存功能")]
            public bool Enabled { get; set; } = true;

            [Display(Name = "自动保存间隔", Description = "自动保存的时间间隔（分钟）")]
            [Range(1, 60, ErrorMessage = "自动保存间隔必须在1-60分钟之间")]
            public int Interval { get; set; } = 5;
        }

        /// <summary>
        /// 示例5：使用配置框架的最佳实践
        /// </summary>
        public class ConfigurationUsageExample
        {
            /// <summary>
            /// 获取配置的最佳实践
            /// </summary>
            public void GetConfigurationBestPractice()
            {
                // 1. 获取配置（带默认值）
                var config = MessageReminderConfigService.GetConfig();

                // 2. 使用配置
                if (MessageReminderConfigService.CanPlayVoiceReminder(config))
                {
                    // 播放语音提醒
                    PlayVoiceReminder(config.Volume);
                }

                // 3. 检查免打扰时段
                if (!MessageReminderConfigService.IsInQuietTime(config))
                {
                    // 发送通知
                    SendNotification();
                }
            }

            /// <summary>
            /// 保存配置的最佳实践
            /// </summary>
            public void SaveConfigurationBestPractice()
            {
                // 1. 获取当前配置
                var config = MessageReminderConfigService.GetConfig();

                // 2. 修改配置
                config.VoiceReminderEnabled = true;
                config.Volume = 80;

                // 3. 验证配置
                var validationResults = new System.ComponentModel.DataAnnotations.ValidationResultCollection();
                if (!ConfigurationService.ValidateConfig(config, validationResults))
                {
                    // 处理验证错误
                    foreach (var result in validationResults)
                    {
                        Console.WriteLine($"验证错误: {result.ErrorMessage}");
                    }
                    return;
                }

                // 4. 保存配置
                if (MessageReminderConfigService.SaveConfig(config))
                {
                    Console.WriteLine("配置保存成功");
                }
                else
                {
                    Console.WriteLine("配置保存失败");
                }
            }

            private void PlayVoiceReminder(int volume)
            {
                // 实现语音播放逻辑
                Console.WriteLine($"播放语音提醒，音量: {volume}%");
            }

            private void SendNotification()
            {
                // 实现通知发送逻辑
                Console.WriteLine("发送通知");
            }
        }

        /// <summary>
        /// 示例6：如何扩展系统配置表
        /// </summary>
        public class SystemConfigExtensionExample
        {
            /// <summary>
            /// 扩展系统配置表的字段定义（需要在tb_SystemConfig中添加对应字段）
            /// </summary>
            public class ExtendedSystemConfig
            {
                /*
                 * 在 tb_SystemConfig 类中添加以下字段：
                 * 
                 * private string _NotificationConfig;
                 * [SugarColumn(ColumnDataType = "text", ColumnName = "NotificationConfig", IsNullable = true)]
                 * public string NotificationConfig { get; set; }
                 * 
                 * private string _UserPreferencesConfig;
                 * [SugarColumn(ColumnDataType = "text", ColumnName = "UserPreferencesConfig", IsNullable = true)]
                 * public string UserPreferencesConfig { get; set; }
                 */
            }
        }
    }

    /// <summary>
    /// 配置框架核心特性总结
    /// </summary>
    public static class ConfigurationFrameworkFeatures
    {
        /*
         * 框架核心特性：
         * 
         * 1. 通用性：支持任意类型的配置对象
         * 2. 扩展性：新增配置项无需修改数据库结构
         * 3. 默认值：确保首次使用时正常初始化
         * 4. 验证机制：支持数据验证和错误提示
         * 5. 类型安全：强类型配置，避免字符串操作错误
         * 6. 错误处理：完善的异常处理和日志记录
         * 7. 性能优化：缓存机制和批量操作支持
         * 8. UI集成：支持数据绑定和控件状态管理
         */

        /// <summary>
        /// 配置项扩展指南
        /// </summary>
        public static class ExtensionGuidelines
        {
            /*
             * 扩展配置项的步骤：
             * 
             * 1. 定义配置模型类
             *    - 包含所有配置属性
             *    - 设置合理的默认值
             *    - 添加数据验证特性
             * 
             * 2. 创建配置服务类
             *    - 定义配置字段名称常量
             *    - 实现获取和保存方法
             *    - 添加业务逻辑方法
             * 
             * 3. 创建配置界面
             *    - 使用Krypton UI控件
             *    - 实现数据绑定
             *    - 添加控件状态管理
             * 
             * 4. 集成到系统
             *    - 在系统配置表中添加字段
             *    - 更新数据库迁移脚本
             *    - 测试配置功能
             */
        }
    }
}