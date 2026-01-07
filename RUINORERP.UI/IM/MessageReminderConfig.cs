using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 消息提醒配置模型
    /// </summary>
    public class MessageReminderConfig
    {
        /// <summary>
        /// 语音提醒开关
        /// </summary>
        public bool VoiceReminderEnabled { get; set; } = true;

        /// <summary>
        /// 提醒音量 (0-100)
        /// </summary>
        public int Volume { get; set; } = 80;

        /// <summary>
        /// 双击业务消息是否自动打开对应单据
        /// </summary>
        public bool AutoOpenDocumentOnDoubleClick { get; set; } = true;

        /// <summary>
        /// 提醒频率（分钟）
        /// </summary>
        public int ReminderFrequency { get; set; } = 5;

        /// <summary>
        /// 免打扰开始时间
        /// </summary>
        public TimeSpan QuietStartTime { get; set; } = new TimeSpan(22, 0, 0);

        /// <summary>
        /// 免打扰结束时间
        /// </summary>
        public TimeSpan QuietEndTime { get; set; } = new TimeSpan(8, 0, 0);

        /// <summary>
        /// 免打扰时段是否启用
        /// </summary>
        public bool QuietTimeEnabled { get; set; } = false;
    }

    // 通用配置管理框架已移至 ConfigurationService.cs 中，使用统一的配置服务

    /// <summary>
    /// 配置项属性特性，用于UI绑定和验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigPropertyAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
        public string Category { get; set; } = "常规";

        public ConfigPropertyAttribute(string displayName, string description = "")
        {
            DisplayName = displayName;
            Description = description;
        }
    }
}