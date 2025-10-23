using RUINORERP.IServices;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Common.Helper.Security;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Services
{
    /// <summary>
    /// 配置加密服务实现
    /// 提供配置对象中敏感字段的自动加密和解密功能
    /// </summary>
    public class ConfigEncryptionService : IConfigEncryptionService
    {
        private readonly ILogger<ConfigEncryptionService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ConfigEncryptionService(ILogger<ConfigEncryptionService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 加密配置对象中的敏感字段
        /// </summary>
        public T EncryptConfig<T>(T config) where T : BaseConfig
        {
            if (config == null)
                return null;

            try
            {
                // 获取配置对象的所有属性
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    // 检查是否标记了SensitiveConfig特性
                    SensitiveConfigAttribute attribute = property.GetCustomAttribute<SensitiveConfigAttribute>();
                    if (attribute != null && property.CanRead && property.CanWrite)
                    {
                        // 获取属性值
                        string value = property.GetValue(config) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            // 加密并设置回属性
                            string encryptedValue = EncryptValue(value);
                            property.SetValue(config, encryptedValue);
                        }
                    }
                }

                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置加密失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 异步加密配置对象中的敏感字段
        /// </summary>
        public async Task<T> EncryptConfigAsync<T>(T config) where T : BaseConfig
        {
            return await Task.Run(() => EncryptConfig(config));
        }

        /// <summary>
        /// 解密配置对象中的敏感字段
        /// </summary>
        public T DecryptConfig<T>(T config) where T : BaseConfig
        {
            if (config == null)
                return null;

            try
            {
                // 获取配置对象的所有属性
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    // 检查是否标记了SensitiveConfig特性
                    SensitiveConfigAttribute attribute = property.GetCustomAttribute<SensitiveConfigAttribute>();
                    if (attribute != null && property.CanRead && property.CanWrite)
                    {
                        // 获取属性值
                        string value = property.GetValue(config) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            // 解密并设置回属性
                            string decryptedValue = DecryptValue(value);
                            property.SetValue(config, decryptedValue);
                        }
                    }
                }

                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置解密失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 异步解密配置对象中的敏感字段
        /// </summary>
        public async Task<T> DecryptConfigAsync<T>(T config) where T : BaseConfig
        {
            return await Task.Run(() => DecryptConfig(config));
        }

        /// <summary>
        /// 对单个字符串进行加密
        /// </summary>
        public string EncryptValue(string value)
        {
            // 检查是否已经加密
            if (ConfigEncryptionHelper.IsEncrypted(value))
                return value;

            return ConfigEncryptionHelper.EncryptConfigValue(value);
        }

        /// <summary>
        /// 对单个字符串进行解密
        /// </summary>
        public string DecryptValue(string value)
        {
            return ConfigEncryptionHelper.DecryptConfigValue(value);
        }

        /// <summary>
        /// 检查字符串是否已加密
        /// </summary>
        public bool IsValueEncrypted(string value)
        {
            return ConfigEncryptionHelper.IsEncrypted(value);
        }

        /// <summary>
        /// 生成字段掩码
        /// </summary>
        public string GenerateMask(string value, int showFirst = 2, int showLast = 2, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            // 如果字符串长度小于显示的字符总和，直接返回掩码
            if (value.Length <= showFirst + showLast)
            {
                return new string(maskChar, value.Length);
            }

            // 构建掩码字符串
            string masked = value.Substring(0, showFirst);
            masked += new string(maskChar, value.Length - showFirst - showLast);
            masked += value.Substring(value.Length - showLast);

            return masked;
        }
    }
}