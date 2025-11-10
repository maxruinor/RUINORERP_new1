
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 数据库配置管理器
    /// 负责管理tb_SysGlobalDynamicConfig相关的数据库配置，包括加载、缓存和变更通知
    /// </summary>
    [Obsolete("")]
    public class UIConfigManager : IDisposable
    {
        #region 单例模式实现
        private static readonly Lazy<UIConfigManager> _instance = new Lazy<UIConfigManager>(() => new UIConfigManager());

        /// <summary>
        /// 获取配置管理器实例
        /// </summary>
        public static UIConfigManager Instance => _instance.Value;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public UIConfigManager() { }
        #endregion

        #region 数据库配置属性
        // 数据库配置缓存
        private readonly Dictionary<string, string> _dbConfigCache = new Dictionary<string, string>();

        /// <summary>
        /// 配置变更事件
        /// </summary>
        public event EventHandler<UIConfigChangedEventArgs> ConfigChanged;
        #endregion

        #region 数据库配置加载与管理
        /// <summary>
        /// 初始化配置管理器
        /// </summary>
        public void Initialize()
        {
            // 异步加载数据库配置
            Task.Run(() => LoadConfigValues());
        }

        /// <summary>
        /// 从数据库加载配置值
        /// </summary>
        public async Task LoadConfigValues()
        {
            List<tb_SysGlobalDynamicConfig> configEntries = new List<tb_SysGlobalDynamicConfig>();
            try
            {
                // 从数据库加载配置项
                tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig> ctr = Startup.GetFromFac<tb_SysGlobalDynamicConfigController<tb_SysGlobalDynamicConfig>>();
                configEntries = await ctr.QueryAsync();
                if (configEntries == null)
                {
                    return;
                }

                // 清空并重新加载缓存
                lock (_dbConfigCache)
                {
                    _dbConfigCache.Clear();
                    foreach (var entry in configEntries)
                    {
                        if (!_dbConfigCache.ContainsKey(entry.ConfigKey))
                        {
                            _dbConfigCache.Add(entry.ConfigKey, entry.ConfigValue);
                        }
                    }
                }

                // 触发配置变更事件
                ConfigChanged?.Invoke(this, new UIConfigChangedEventArgs { ConfigType = ConfigType.Database });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从数据库加载配置失败: {ex.Message}");
            }
        }
        #endregion

        #region 配置值获取
        /// <summary>
        /// 获取数据库配置值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns>配置值字符串</returns>
        public string GetValue(string key)
        {
            if (_dbConfigCache.TryGetValue(key, out var value))
            {
                return value;
            }

            System.Diagnostics.Debug.WriteLine($"数据库配置键 '{key}' 未找到");
            return string.Empty;
        }

        /// <summary>
        /// 获取数据库配置值（泛型版本）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">配置键</param>
        /// <returns>配置值</returns>
        public T GetValue<T>(string key)
        {
            try
            {
                if (_dbConfigCache.TryGetValue(key, out var value))
                {
                    if (typeof(T).IsEnum)
                    {
                        return (T)Enum.Parse(typeof(T), value);
                    }
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                return default;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取配置值失败: {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// 检查配置键是否存在
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns>是否存在</returns>
        public bool ContainsKey(string key)
        {
            return _dbConfigCache.ContainsKey(key);
        }

        /// <summary>
        /// 处理配置同步
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON字符串</param>
        public void HandleConfigSync(string configType, string configDataJson)
        {
            HandleConfigSync(configType, configDataJson, false);
        }

        /// <summary>
        /// 处理配置同步
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON字符串</param>
        /// <param name="forceApply">是否强制应用配置</param>
        public void HandleConfigSync(string configType, string configDataJson, bool forceApply)
        {
            try
            {
                // 确保配置数据不为空
                if (string.IsNullOrEmpty(configDataJson))
                {
                    throw new ArgumentNullException(nameof(configDataJson), "配置数据不能为空");
                }

                // 根据配置类型处理不同的配置同步
                switch (configType)
                {                    
                    case "Database":
                        // 刷新数据库配置
                        Task.Run(() => LoadConfigValues());
                        break;
                    default:
                        // 尝试作为特定配置项进行处理
                        try
                        {
                            // 尝试解析为键值对
                            var configEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(configDataJson);
                            if (configEntry != null && configEntry.Count > 0)
                            {
                                // 更新数据库配置缓存
                                lock (_dbConfigCache)
                                {
                                    foreach (var kvp in configEntry)
                                    {
                                        _dbConfigCache[kvp.Key] = kvp.Value;
                                    }
                                }
                                // 触发配置变更事件
                                ConfigChanged?.Invoke(this, new UIConfigChangedEventArgs { ConfigType = ConfigType.Database });

                                // 如果需要强制应用，执行额外的应用逻辑
                                if (forceApply)
                                {
                                    OnCustomConfigForceApplied(configType, configDataJson);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"处理特定配置项失败: {ex.Message}");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理配置同步失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 当自定义配置被强制应用时触发
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="configJson">配置JSON数据</param>
        private void OnCustomConfigForceApplied(string configType, string configJson)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"强制应用自定义配置: {configType}");
                // 这里可以添加具体的应用逻辑
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"强制应用自定义配置 {configType} 失败: {ex.Message}");
            }
        }
        #endregion

        #region IDisposable实现
        private bool _disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 清空配置缓存
                    _dbConfigCache.Clear();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~UIConfigManager()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// 配置类型枚举
    /// </summary>
    public enum ConfigType
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        Database
    }

    /// <summary>
    /// 配置变更事件参数
    /// </summary>
    public class UIConfigChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public ConfigType ConfigType { get; set; }

        /// <summary>
        /// 变更后的配置对象
        /// </summary>
        public object Config { get; set; }

        /// <summary>
        /// 是否强制应用配置
        /// </summary>
        public bool ForceApply { get; set; }
    }

    /// <summary>
    /// 用于动态参数表中的值的数据类型
    /// </summary>
    public enum ConfigValueType
    {
        [DescriptionAttribute("文本")]
        String,

        [DescriptionAttribute("整数")]
        Integer,

        [DescriptionAttribute("布尔值")]
        Boolean,

        [DescriptionAttribute("小数")]
        Decimal,

        [DescriptionAttribute("日期")]
        DateTime,

        [DescriptionAttribute("双精度浮点数")]
        Double,

        [DescriptionAttribute("单精度浮点数")]
        Float,

        [DescriptionAttribute("长整数")]
        Long,

        [DescriptionAttribute("短整数")]
        Short,

        [DescriptionAttribute("字节数组")]
        Byte,

        [DescriptionAttribute("时间跨度")]
        TimeSpan,

        [DescriptionAttribute("GUID")]
        Guid,

        [DescriptionAttribute("URI")]
        Uri,

        [DescriptionAttribute("版本号")]
        Version,

        [DescriptionAttribute("数组")]
        Array,

        [DescriptionAttribute("列表")]
        List,

        [DescriptionAttribute("字典")]
        Dictionary,

        [DescriptionAttribute("集合")]
        Collection,

        [DescriptionAttribute("对象")]
        Object
    }
}
