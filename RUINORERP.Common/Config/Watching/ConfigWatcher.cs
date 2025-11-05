/*************************************************************
 * 文件名：ConfigWatcher.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置文件监视器
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RUINORERP.Common.Config.Notifications;

namespace RUINORERP.Common.Config.Watching
{
    /// <summary>
    /// 配置文件变更事件参数
    /// </summary>
    public class ConfigFileChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 变更的文件路径
        /// </summary>
        public string FilePath { get; set; }
        
        /// <summary>
        /// 变更类型
        /// </summary>
        public WatcherChangeTypes ChangeType { get; set; }
        
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// 关联的配置类型（如果已知）
        /// </summary>
        public Type ConfigType { get; set; }
    }

    /// <summary>
    /// 配置监视器选项
    /// </summary>
    public class ConfigWatcherOptions
    {
        /// <summary>
        /// 是否启用配置监视
        /// 默认true
        /// </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// 监视延迟（毫秒）
        /// 用于防抖，避免短时间内多次触发变更事件
        /// 默认500毫秒
        /// </summary>
        public int WatchDelay { get; set; } = 500;
        
        /// <summary>
        /// 监视的文件过滤器
        /// 默认*.json
        /// </summary>
        public string FileFilter { get; set; } = "*.json";
        
        /// <summary>
        /// 是否监视子目录
        /// 默认true
        /// </summary>
        public bool IncludeSubdirectories { get; set; } = true;
        
        /// <summary>
        /// 监视的变更类型
        /// 默认Changed | Created | Deleted
        /// </summary>
        public WatcherChangeTypes NotifyFilter { get; set; } = WatcherChangeTypes.Changed | WatcherChangeTypes.Created | WatcherChangeTypes.Deleted;
    }

    /// <summary>
    /// 配置文件监视器接口
    /// 定义配置文件监视的基本操作
    /// </summary>
    public interface IConfigWatcher : IDisposable
    {
        /// <summary>
        /// 监视配置文件
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="filePath">文件路径</param>
        void WatchFile<TConfig>(string filePath);
        
        /// <summary>
        /// 监视配置目录
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="directoryPath">目录路径</param>
        void WatchDirectory<TConfig>(string directoryPath);
        
        /// <summary>
        /// 停止监视文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        void UnwatchFile(string filePath);
        
        /// <summary>
        /// 停止监视目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        void UnwatchDirectory(string directoryPath);
        
        /// <summary>
        /// 重新加载配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>异步任务</returns>
        Task ReloadConfigAsync<TConfig>(string filePath);
        
        /// <summary>
        /// 配置文件变更事件
        /// </summary>
        event EventHandler<ConfigFileChangedEventArgs> ConfigFileChanged;
    }

    /// <summary>
    /// 默认配置文件监视器
    /// 使用FileSystemWatcher监视配置文件的变化
    /// </summary>
    public class DefaultConfigWatcher : IConfigWatcher
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogger<DefaultConfigWatcher> _logger;
        
        /// <summary>
        /// 配置选项
        /// </summary>
        protected readonly ConfigWatcherOptions _options;
        
        /// <summary>
        /// 配置变更通知器
        /// </summary>
        protected readonly IConfigChangeNotifier _changeNotifier;
        
        /// <summary>
        /// 文件系统监视器字典
        /// 键：监视路径，值：文件系统监视器
        /// </summary>
        protected readonly ConcurrentDictionary<string, FileSystemWatcher> _fileWatchers;
        
        /// <summary>
        /// 目录监视器字典
        /// 键：监视目录，值：文件系统监视器
        /// </summary>
        protected readonly ConcurrentDictionary<string, FileSystemWatcher> _directoryWatchers;
        
        /// <summary>
        /// 文件路径到配置类型的映射
        /// </summary>
        protected readonly ConcurrentDictionary<string, Type> _fileToConfigTypeMap;
        
        /// <summary>
        /// 用于防抖的计时器字典
        /// 键：文件路径，值：最后变更时间
        /// </summary>
        protected readonly ConcurrentDictionary<string, DateTime> _debounceTimers;
        
        /// <summary>
        /// 是否已释放资源
        /// </summary>
        protected bool _disposed = false;

        /// <summary>
        /// 配置文件变更事件
        /// </summary>
        public event EventHandler<ConfigFileChangedEventArgs> ConfigFileChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="options">配置选项</param>
        /// <param name="changeNotifier">配置变更通知器</param>
        public DefaultConfigWatcher(
            ILogger<DefaultConfigWatcher> logger,
            IOptions<ConfigWatcherOptions> options = null,
            IConfigChangeNotifier changeNotifier = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new ConfigWatcherOptions();
            _changeNotifier = changeNotifier;
            
            _fileWatchers = new ConcurrentDictionary<string, FileSystemWatcher>();
            _directoryWatchers = new ConcurrentDictionary<string, FileSystemWatcher>();
            _fileToConfigTypeMap = new ConcurrentDictionary<string, Type>();
            _debounceTimers = new ConcurrentDictionary<string, DateTime>();
            
            _logger.LogInformation("配置文件监视器初始化完成，启用状态: {Enabled}, 监视延迟: {Delay}ms", 
                _options.Enabled, _options.WatchDelay);
        }

        /// <summary>
        /// 监视配置文件
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="filePath">文件路径</param>
        public void WatchFile<TConfig>(string filePath)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置监视已禁用，跳过监视文件: {FilePath}", filePath);
                return;
            }
            
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("要监视的配置文件不存在: {FilePath}", filePath);
                return;
            }
            
            _logger.LogInformation("开始监视配置文件: {FilePath}, 配置类型: {ConfigType}", 
                filePath, typeof(TConfig).FullName);
            
            // 记录文件到配置类型的映射
            _fileToConfigTypeMap[filePath] = typeof(TConfig);
            
            // 获取文件所在目录
            string directoryPath = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            
            // 创建或获取目录监视器
            var watcher = _directoryWatchers.GetOrAdd(directoryPath, path =>
            {
                var newWatcher = CreateWatcher(path, fileName);
                newWatcher.Filter = fileName; // 只监视特定文件
                newWatcher.Changed += OnFileChanged;
                newWatcher.Created += OnFileChanged;
                newWatcher.Deleted += OnFileChanged;
                newWatcher.Error += OnWatcherError;
                newWatcher.EnableRaisingEvents = true;
                
                _logger.LogInformation("创建文件监视器: {Path}, 过滤器: {Filter}", path, fileName);
                return newWatcher;
            });
            
            // 如果已经存在监视器，确保启用事件
            if (!watcher.EnableRaisingEvents)
            {
                watcher.EnableRaisingEvents = true;
                _logger.LogDebug("启用文件监视器: {Path}", directoryPath);
            }
        }

        /// <summary>
        /// 监视配置目录
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="directoryPath">目录路径</param>
        public void WatchDirectory<TConfig>(string directoryPath)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置监视已禁用，跳过监视目录: {DirectoryPath}", directoryPath);
                return;
            }
            
            if (string.IsNullOrEmpty(directoryPath))
                throw new ArgumentNullException(nameof(directoryPath));
            
            if (!Directory.Exists(directoryPath))
            {
                _logger.LogWarning("要监视的配置目录不存在: {DirectoryPath}", directoryPath);
                return;
            }
            
            _logger.LogInformation("开始监视配置目录: {DirectoryPath}, 配置类型: {ConfigType}", 
                directoryPath, typeof(TConfig).FullName);
            
            // 创建或获取目录监视器
            var watcher = _directoryWatchers.GetOrAdd(directoryPath, path =>
            {
                var newWatcher = CreateWatcher(path, _options.FileFilter);
                newWatcher.Filter = _options.FileFilter;
                newWatcher.IncludeSubdirectories = _options.IncludeSubdirectories;
                newWatcher.Changed += OnFileChanged;
                newWatcher.Created += OnFileChanged;
                newWatcher.Deleted += OnFileChanged;
                newWatcher.Error += OnWatcherError;
                newWatcher.EnableRaisingEvents = true;
                
                _logger.LogInformation("创建目录监视器: {Path}, 过滤器: {Filter}, 包含子目录: {IncludeSubdirs}", 
                    path, _options.FileFilter, _options.IncludeSubdirectories);
                return newWatcher;
            });
            
            // 如果已经存在监视器，确保启用事件
            if (!watcher.EnableRaisingEvents)
            {
                watcher.EnableRaisingEvents = true;
                _logger.LogDebug("启用目录监视器: {Path}", directoryPath);
            }
            
            // 为目录中的所有匹配文件记录配置类型
            foreach (var file in Directory.EnumerateFiles(directoryPath, _options.FileFilter, 
                _options.IncludeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                _fileToConfigTypeMap[file] = typeof(TConfig);
            }
        }

        /// <summary>
        /// 停止监视文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void UnwatchFile(string filePath)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置监视已禁用，跳过取消监视文件: {FilePath}", filePath);
                return;
            }
            
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            
            _logger.LogInformation("停止监视配置文件: {FilePath}", filePath);
            
            // 移除文件到配置类型的映射
            _fileToConfigTypeMap.TryRemove(filePath, out _);
            
            // 获取文件所在目录
            string directoryPath = Path.GetDirectoryName(filePath);
            
            // 检查是否有该目录的监视器
            if (_directoryWatchers.TryGetValue(directoryPath, out var watcher))
            {
                // 检查该目录下是否还有其他被监视的文件
                bool hasOtherWatchedFiles = _fileToConfigTypeMap.Keys.Any(
                    file => Path.GetDirectoryName(file) == directoryPath && file != filePath);
                
                // 如果只有这一个文件被监视，则移除监视器
                if (!hasOtherWatchedFiles && watcher.Filter != _options.FileFilter) // 确保是文件特定监视器
                {
                    DisposeWatcher(watcher);
                    _directoryWatchers.TryRemove(directoryPath, out _);
                    _logger.LogInformation("移除文件监视器: {Path}", directoryPath);
                }
            }
        }

        /// <summary>
        /// 停止监视目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        public void UnwatchDirectory(string directoryPath)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置监视已禁用，跳过取消监视目录: {DirectoryPath}", directoryPath);
                return;
            }
            
            if (string.IsNullOrEmpty(directoryPath))
                throw new ArgumentNullException(nameof(directoryPath));
            
            _logger.LogInformation("停止监视配置目录: {DirectoryPath}", directoryPath);
            
            // 移除该目录下所有文件的映射
            var filesToRemove = _fileToConfigTypeMap.Keys.Where(
                file => Path.GetDirectoryName(file) == directoryPath).ToList();
            
            foreach (var file in filesToRemove)
            {
                _fileToConfigTypeMap.TryRemove(file, out _);
            }
            
            // 移除目录监视器
            if (_directoryWatchers.TryGetValue(directoryPath, out var watcher))
            {
                DisposeWatcher(watcher);
                _directoryWatchers.TryRemove(directoryPath, out _);
                _logger.LogInformation("移除目录监视器: {Path}", directoryPath);
            }
        }

        /// <summary>
        /// 重新加载配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>异步任务</returns>
        public async Task ReloadConfigAsync<TConfig>(string filePath)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置监视已禁用，跳过重新加载配置: {FilePath}", filePath);
                return;
            }
            
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            
            _logger.LogInformation("重新加载配置文件: {FilePath}, 配置类型: {ConfigType}", 
                filePath, typeof(TConfig).FullName);
            
            try
            {
                // 这里实际应用中需要从配置文件加载配置
                // 并通过变更通知器通知配置变更
                
                // 触发配置文件变更事件
                OnConfigFileChanged(new ConfigFileChangedEventArgs
                {
                    FilePath = filePath,
                    ChangeType = WatcherChangeTypes.Changed,
                    ConfigType = typeof(TConfig)
                });
                
                _logger.LogInformation("配置文件重新加载完成: {FilePath}", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重新加载配置文件失败: {FilePath}", filePath);
                throw;
            }
        }

        /// <summary>
        /// 文件变更事件处理
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">文件系统事件参数</param>
        protected virtual async void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                _logger.LogDebug("检测到文件变更: {Path}, 类型: {ChangeType}", e.FullPath, e.ChangeType);
                
                // 应用防抖逻辑
                if (!ApplyDebounce(e.FullPath))
                {
                    _logger.LogDebug("文件变更防抖，跳过处理: {Path}", e.FullPath);
                    return;
                }
                
                // 获取关联的配置类型
                Type configType = null;
                _fileToConfigTypeMap.TryGetValue(e.FullPath, out configType);
                
                // 触发配置文件变更事件
                var args = new ConfigFileChangedEventArgs
                {
                    FilePath = e.FullPath,
                    ChangeType = e.ChangeType,
                    ConfigType = configType
                };
                
                OnConfigFileChanged(args);
                
                _logger.LogInformation("处理文件变更完成: {Path}", e.FullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理文件变更事件失败: {Path}", e.FullPath);
            }
        }

        /// <summary>
        /// 监视器错误事件处理
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">错误事件参数</param>
        protected virtual void OnWatcherError(object sender, ErrorEventArgs e)
        {
            _logger.LogError(e.GetException(), "配置文件监视器错误");
            
            // 尝试重新启动监视器
            if (sender is FileSystemWatcher watcher)
            {
                try
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.EnableRaisingEvents = true;
                    _logger.LogInformation("已重新启动文件监视器: {Path}", watcher.Path);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "重新启动文件监视器失败: {Path}", watcher.Path);
                }
            }
        }

        /// <summary>
        /// 触发配置文件变更事件
        /// </summary>
        /// <param name="args">配置文件变更事件参数</param>
        protected virtual void OnConfigFileChanged(ConfigFileChangedEventArgs args)
        {
            _logger.LogInformation("触发配置文件变更事件: {Path}, 类型: {ChangeType}", 
                args.FilePath, args.ChangeType);
            
            // 触发事件
            ConfigFileChanged?.Invoke(this, args);
            
            // 如果有配置变更通知器且知道配置类型，通知配置变更
            if (_changeNotifier != null && args.ConfigType != null)
            {
                // 注意：这里简化处理，实际应用中需要加载新旧配置值
                // 可以通过反射调用泛型方法
                _logger.LogDebug("通过配置变更通知器广播配置变更: {ConfigType}", args.ConfigType.FullName);
            }
        }

        /// <summary>
        /// 创建文件系统监视器
        /// </summary>
        /// <param name="path">监视路径</param>
        /// <param name="filter">文件过滤器</param>
        /// <returns>文件系统监视器</returns>
        protected virtual FileSystemWatcher CreateWatcher(string path, string filter)
        {
            return new FileSystemWatcher
            {
                Path = path,
                Filter = filter,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                EnableRaisingEvents = false
            };
        }

        /// <summary>
        /// 应用防抖逻辑
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否应该处理变更事件</returns>
        protected virtual bool ApplyDebounce(string filePath)
        {
            var now = DateTime.UtcNow;
            
            // 检查是否存在最近的变更记录
            if (_debounceTimers.TryGetValue(filePath, out var lastChangeTime))
            {
                // 如果距离上次变更时间小于延迟时间，更新时间并返回false
                if ((now - lastChangeTime).TotalMilliseconds < _options.WatchDelay)
                {
                    _debounceTimers[filePath] = now;
                    return false;
                }
            }
            
            // 更新最后变更时间并返回true
            _debounceTimers[filePath] = now;
            return true;
        }

        /// <summary>
        /// 释放文件系统监视器
        /// </summary>
        /// <param name="watcher">文件系统监视器</param>
        protected virtual void DisposeWatcher(FileSystemWatcher watcher)
        {
            if (watcher == null)
                return;
            
            try
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                _logger.LogDebug("已释放文件系统监视器: {Path}", watcher.Path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放文件系统监视器失败: {Path}", watcher.Path);
            }
        }

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
                    // 释放托管资源
                    foreach (var watcher in _fileWatchers.Values)
                    {
                        DisposeWatcher(watcher);
                    }
                    
                    foreach (var watcher in _directoryWatchers.Values)
                    {
                        DisposeWatcher(watcher);
                    }
                    
                    _fileWatchers.Clear();
                    _directoryWatchers.Clear();
                    _fileToConfigTypeMap.Clear();
                    _debounceTimers.Clear();
                    
                    _logger.LogInformation("配置文件监视器资源已释放");
                }
                
                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~DefaultConfigWatcher()
        {
            Dispose(false);
        }
    }

    /// <summary>
    /// 无操作配置文件监视器
    /// 用于禁用配置监视的场景
    /// </summary>
    public class NoOpConfigWatcher : IConfigWatcher
    {
        /// <summary>
        /// 配置文件变更事件
        /// </summary>
        public event EventHandler<ConfigFileChangedEventArgs> ConfigFileChanged;

        /// <summary>
        /// 监视配置文件
        /// 无操作实现
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="filePath">文件路径</param>
        public void WatchFile<TConfig>(string filePath)
        {
            // 无操作
        }

        /// <summary>
        /// 监视配置目录
        /// 无操作实现
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="directoryPath">目录路径</param>
        public void WatchDirectory<TConfig>(string directoryPath)
        {
            // 无操作
        }

        /// <summary>
        /// 停止监视文件
        /// 无操作实现
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void UnwatchFile(string filePath)
        {
            // 无操作
        }

        /// <summary>
        /// 停止监视目录
        /// 无操作实现
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        public void UnwatchDirectory(string directoryPath)
        {
            // 无操作
        }

        /// <summary>
        /// 重新加载配置
        /// 无操作实现
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>完成的任务</returns>
        public Task ReloadConfigAsync<TConfig>(string filePath)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 释放资源
        /// 无操作实现
        /// </summary>
        public void Dispose()
        {
            // 无操作
        }
    }

    
}