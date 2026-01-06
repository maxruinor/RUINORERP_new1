using Newtonsoft.Json;
using RUINORERP.Business;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 通用配置服务 - 提供统一的配置管理功能
    /// </summary>
    public static class ConfigurationService
    {
        /// <summary>
        /// 获取指定类型的配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configFieldName">配置字段名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置对象</returns>
        public static T GetConfig<T>(string configFieldName, T defaultValue = null) where T : class, new()
        {
            try
            {
                tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;

                // 通过反射获取配置字段的值
                var configJson = GetConfigJsonFromUserPersonalized(userPersonalized, configFieldName);

                if (string.IsNullOrEmpty(configJson))
                {
                    return defaultValue ?? new T();
                }

                return JsonConvert.DeserializeObject<T>(configJson) ?? new T();
            }
            catch (Exception ex)
            {
                // 记录日志
                return defaultValue ?? new T();
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="configFieldName">配置字段名</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveConfig<T>(T config, string configFieldName) where T : class
        {
            try
            {
                if (config == null)
                    throw new ArgumentNullException(nameof(config));

                tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;

                // 序列化配置
                string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);

                // 更新用户个性化配置
                SetConfigJsonToUserPersonalized(userPersonalized, configFieldName, configJson);

                // 保存到数据库
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                if (userPersonalized.UserPersonalizedID > 0)
                {
                    return db.Updateable(userPersonalized).ExecuteCommand() > 0;
                }
                else
                {
                    var id = db.Insertable(userPersonalized).ExecuteReturnSnowflakeId();
                    return id > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 验证配置对象
        /// </summary>
        public static bool ValidateConfig<T>(T config, System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults = null)
        {
            if (config == null)
                return false;

            var context = new System.ComponentModel.DataAnnotations.ValidationContext(config, null, null);
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(config, context, results, true);

            validationResults?.AddRange(results);

            return isValid;
        }

   
        /// <summary>
        /// 从用户个性化配置对象中获取配置JSON
        /// </summary>
        private static string GetConfigJsonFromUserPersonalized(tb_UserPersonalized userPersonalized, string configFieldName)
        {
            var property = typeof(tb_UserPersonalized).GetProperty(configFieldName);
            if (property == null)
                throw new ArgumentException($"配置字段 '{configFieldName}' 不存在");

            return property.GetValue(userPersonalized) as string;
        }

        /// <summary>
        /// 设置配置JSON到用户个性化配置对象
        /// </summary>
        private static void SetConfigJsonToUserPersonalized(tb_UserPersonalized userPersonalized, string configFieldName, string configJson)
        {
            var property = typeof(tb_UserPersonalized).GetProperty(configFieldName);
            if (property == null)
                throw new ArgumentException($"配置字段 '{configFieldName}' 不存在");

            property.SetValue(userPersonalized, configJson);
        }
    }

    /// <summary>
    /// 消息提醒配置服务 - 专门处理消息提醒配置
    /// </summary>
    public static class MessageReminderConfigService
    {
        private const string CONFIG_FIELD_NAME = "IMConfig";

        /// <summary>
        /// 获取消息提醒配置
        /// </summary>
        public static MessageReminderConfig GetConfig()
        {
            return ConfigurationService.GetConfig<MessageReminderConfig>(CONFIG_FIELD_NAME);
        }

        /// <summary>
        /// 保存消息提醒配置
        /// </summary>
        public static bool SaveConfig(MessageReminderConfig config)
        {
            return ConfigurationService.SaveConfig(config, CONFIG_FIELD_NAME);
        }

        /// <summary>
        /// 检查当前是否处于免打扰时段
        /// </summary>
        public static bool IsInQuietTime(MessageReminderConfig config = null)
        {
            config ??= GetConfig();

            if (!config.QuietTimeEnabled)
                return false;

            var currentTime = DateTime.Now.TimeOfDay;

            // 处理跨天的免打扰时段（如22:00-08:00）
            if (config.QuietStartTime > config.QuietEndTime)
            {
                return currentTime >= config.QuietStartTime || currentTime <= config.QuietEndTime;
            }
            else
            {
                return currentTime >= config.QuietStartTime && currentTime <= config.QuietEndTime;
            }
        }

        /// <summary>
        /// 检查是否可以播放语音提醒
        /// </summary>
        public static bool CanPlayVoiceReminder(MessageReminderConfig config = null)
        {
            config ??= GetConfig();

            return config.VoiceReminderEnabled && !IsInQuietTime(config);
        }
    }
}