using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Model.Base.StatusManager.PerformanceMonitoring
{
    /// <summary>
    /// 性能监控总开关
    /// 类似日志系统的开关机制，控制整个性能监控体系的启用/禁用
    /// </summary>
    public static class PerformanceMonitorSwitch
    {
        private static volatile bool _isEnabled = false;
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private static ConfigModel.PerformanceMonitorConfig _config = new ConfigModel.PerformanceMonitorConfig();
        private static readonly string _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "performance_monitor.json");

        /// <summary>
        /// 性能监控是否启用
        /// </summary>
        public static bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _lock.EnterWriteLock();
                try
                {
                    _isEnabled = value;
                    OnSwitchChanged?.Invoke(null, new PerformanceSwitchEventArgs(value));
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// 性能监控配置
        /// </summary>
        public static ConfigModel.PerformanceMonitorConfig Config
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _config;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            set
            {
                _lock.EnterWriteLock();
                try
                {
                    _config = value ?? new ConfigModel.PerformanceMonitorConfig();
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// 开关状态变化事件
        /// </summary>
        public static event EventHandler<PerformanceSwitchEventArgs> OnSwitchChanged;

        /// <summary>
        /// 初始化性能监控开关
        /// </summary>
        public static void Initialize()
        {
            LoadConfig();
            _isEnabled = _config.IsEnabled;
        }

        /// <summary>
        /// 启用性能监控
        /// </summary>
        public static void Enable()
        {
            IsEnabled = true;
            _config.IsEnabled = true;
            SaveConfig();
        }

        /// <summary>
        /// 禁用性能监控
        /// </summary>
        public static void Disable()
        {
            IsEnabled = false;
            _config.IsEnabled = false;
            SaveConfig();
        }

        /// <summary>
        /// 切换性能监控状态
        /// </summary>
        public static bool Toggle()
        {
            IsEnabled = !IsEnabled;
            _config.IsEnabled = _isEnabled;
            SaveConfig();
            return IsEnabled;
        }

        /// <summary>
        /// 更新配置并保存
        /// </summary>
        /// <param name="config">新的配置</param>
        public static void UpdateConfig(ConfigModel.PerformanceMonitorConfig config)
        {
            _lock.EnterWriteLock();
            try
            {
                _config = config ?? new ConfigModel.PerformanceMonitorConfig();
                _isEnabled = _config.IsEnabled;
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            SaveConfig();
        }

        /// <summary>
        /// 保存配置（仅内存保存，不持久化）
        /// </summary>
        private static void SaveConfigToMemory()
        {
            _lock.EnterReadLock();
            try
            {
                _config.IsEnabled = _isEnabled;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 检查特定监控项是否启用
        /// </summary>
        public static bool IsMonitorEnabled(PerformanceMonitorType monitorType)
        {
            if (!IsEnabled) return false;

            _lock.EnterReadLock();
            try
            {
                return _config.EnabledMonitors.Contains(monitorType);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 启用特定监控项
        /// </summary>
        public static void EnableMonitor(PerformanceMonitorType monitorType)
        {
            _lock.EnterWriteLock();
            try
            {
                if (!_config.EnabledMonitors.Contains(monitorType))
                {
                    _config.EnabledMonitors.Add(monitorType);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 禁用特定监控项
        /// </summary>
        public static void DisableMonitor(PerformanceMonitorType monitorType)
        {
            _lock.EnterWriteLock();
            try
            {
                _config.EnabledMonitors.Remove(monitorType);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private static void LoadConfig()
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    var json = File.ReadAllText(_configFilePath);
                    _config = JsonConvert.DeserializeObject<ConfigModel.PerformanceMonitorConfig>(json) ?? new ConfigModel.PerformanceMonitorConfig();
                    _isEnabled = _config.IsEnabled;
                }
                else
                {
                    _config = new ConfigModel.PerformanceMonitorConfig();
                    _config.InitDefault();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载性能监控配置失败: {ex.Message}");
                _config = new ConfigModel.PerformanceMonitorConfig();
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private static void SaveConfig()
        {
            try
            {
                var directory = Path.GetDirectoryName(_configFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                _config.IsEnabled = _isEnabled;
                var json = JsonConvert.SerializeObject(_config, Formatting.Indented);
                File.WriteAllText(_configFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存性能监控配置失败: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 性能监控类型
    /// </summary>
    public enum PerformanceMonitorType
    {
        /// <summary>
        /// 方法执行性能
        /// </summary>
        MethodExecution = 1,

        /// <summary>
        /// 数据库性能
        /// </summary>
        Database = 2,

        /// <summary>
        /// 网络性能
        /// </summary>
        Network = 3,

        /// <summary>
        /// 内存使用
        /// </summary>
        Memory = 4,

        /// <summary>
        /// 缓存性能
        /// </summary>
        Cache = 5,

        /// <summary>
        /// UI响应性能
        /// </summary>
        UIResponse = 6,

        /// <summary>
        /// 事务性能
        /// </summary>
        Transaction = 7,

        /// <summary>
        /// 死锁检测
        /// </summary>
        Deadlock = 8
    }

    /// <summary>
    /// 性能监控开关事件参数
    /// </summary>
    public class PerformanceSwitchEventArgs : EventArgs
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceSwitchEventArgs(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }

}
