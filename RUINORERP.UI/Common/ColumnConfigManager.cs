using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Model;
using Newtonsoft.Json;
using RUINORERP.UI.UControls;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 列配置统一管理器
    /// 提供配置加载、保存、缓存和批量延时提交功能
    /// </summary>
    public class ColumnConfigManager
    {
        private static ColumnConfigManager _instance;
        private static readonly object _lock = new object();

        public static ColumnConfigManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            try
                            {
                                _instance = RUINORERP.UI.Startup.GetFromFac<ColumnConfigManager>();
                            }
                            catch
                            {
                                _instance = new ColumnConfigManager();
                            }
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly ConcurrentDictionary<string, List<ColDisplayController>> _configCache = new();
        private readonly ConcurrentQueue<ConfigChangeItem> _pendingChanges = new();
        private readonly Timer _flushTimer;
        private readonly SemaphoreSlim _flushLock = new(1, 1);

        private const int FlushIntervalMs = 5000; // 5秒自动刷新
        private const int MaxPendingChanges = 10; // 达到10个变更立即刷新

        private ColumnConfigManager()
        {
            _flushTimer = new Timer(async _ => await FlushChangesAsync(), null, FlushIntervalMs, FlushIntervalMs);
        }

        /// <summary>
        /// 无参构造函数，用于DI容器实例化
        /// </summary>
        public ColumnConfigManager(ILogger<ColumnConfigManager> logger = null)
            : this()
        {
        }

        /// <summary>
        /// 加载列配置（优先缓存）
        /// </summary>
        /// <param name="gridKeyName">表格键名</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>列配置列表</returns>
        public async Task<List<ColDisplayController>> LoadColumnConfigAsync(string gridKeyName, long menuId)
        {
            string cacheKey = GetCacheKey(gridKeyName, menuId);

            if (_configCache.TryGetValue(cacheKey, out var cachedConfig))
            {
                return cachedConfig;
            }

            var config = await LoadFromDatabaseAsync(gridKeyName, menuId);
            _configCache.TryAdd(cacheKey, config);
            return config;
        }

        /// <summary>
        /// 保存列配置（加入延时队列）
        /// </summary>
        /// <param name="gridKeyName">表格键名</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="config">列配置</param>
        public void SaveColumnConfig(string gridKeyName, long menuId, List<ColDisplayController> config)
        {
            string cacheKey = GetCacheKey(gridKeyName, menuId);

            if (_configCache.TryGetValue(cacheKey, out var existingConfig))
            {
                _configCache[cacheKey] = config;
            }
            else
            {
                _configCache.TryAdd(cacheKey, config);
            }

            _pendingChanges.Enqueue(new ConfigChangeItem
            {
                GridKeyName = gridKeyName,
                MenuId = menuId,
                Config = config,
                Timestamp = DateTime.Now
            });

            if (_pendingChanges.Count >= MaxPendingChanges)
            {
                Task.Run(async () => await FlushChangesAsync());
            }
        }

        /// <summary>
        /// 立即刷新保存所有待提交的变更
        /// </summary>
        public async Task FlushChangesAsync()
        {
            if (!_flushLock.Wait(0))
            {
                return;
            }

            try
            {
                var changesToFlush = new List<ConfigChangeItem>();
                while (_pendingChanges.TryDequeue(out var change))
                {
                    changesToFlush.Add(change);
                }

                if (changesToFlush.Count == 0)
                {
                    return;
                }

                var groupedChanges = changesToFlush
                    .GroupBy(c => GetCacheKey(c.GridKeyName, c.MenuId))
                    .ToList();

                foreach (var group in groupedChanges)
                {
                    var latestChange = group.OrderByDescending(c => c.Timestamp).First();
                    await SaveToDatabaseAsync(latestChange.GridKeyName, latestChange.MenuId, latestChange.Config);
                }
            }
            finally
            {
                _flushLock.Release();
            }
        }

        /// <summary>
        /// 恢复默认配置
        /// </summary>
        /// <param name="gridKeyName">表格键名</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="defaultConfig">默认配置</param>
        public async Task ResetToDefaultAsync(string gridKeyName, long menuId, List<ColDisplayController> defaultConfig)
        {
            string cacheKey = GetCacheKey(gridKeyName, menuId);

            if (_configCache.ContainsKey(cacheKey))
            {
                _configCache[cacheKey] = defaultConfig;
            }
            else
            {
                _configCache.TryAdd(cacheKey, defaultConfig);
            }

            await SaveToDatabaseAsync(gridKeyName, menuId, defaultConfig);
        }

        /// <summary>
        /// 清除指定菜单的配置缓存
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        public void InvalidateCache(long menuId)
        {
            var keysToRemove = _configCache.Keys.Where(k => k.EndsWith($"_{menuId}")).ToList();
            foreach (var key in keysToRemove)
            {
                _configCache.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// 从数据库加载配置
        /// </summary>
        private async Task<List<ColDisplayController>> LoadFromDatabaseAsync(string gridKeyName, long menuId)
        {
            try
            {
                var db = MainForm.Instance.AppContext.Db;

                var gridSetting = await db.Queryable<tb_UIGridSetting>()
                    .Where(c => c.GridKeyName == gridKeyName && c.UIMenuPID == menuId)
                    .FirstAsync();

                if (gridSetting == null || string.IsNullOrEmpty(gridSetting.ColsSetting))
                {
                    return new List<ColDisplayController>();
                }

                var config = JsonConvert.DeserializeObject<List<ColDisplayController>>(gridSetting.ColsSetting);
                return config ?? new List<ColDisplayController>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ColumnConfigManager] 加载列配置失败: {ex.Message}");
                return new List<ColDisplayController>();
            }
        }

        /// <summary>
        /// 保存配置到数据库
        /// </summary>
        private async Task SaveToDatabaseAsync(string gridKeyName, long menuId, List<ColDisplayController> config)
        {
            try
            {
                var db = MainForm.Instance.AppContext.Db;

                var json = JsonConvert.SerializeObject(config, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var existingSetting = await db.Queryable<tb_UIGridSetting>()
                    .Where(c => c.GridKeyName == gridKeyName && c.UIMenuPID == menuId)
                    .FirstAsync();

                if (existingSetting == null)
                {
                    var newSetting = new tb_UIGridSetting
                    {
                        GridKeyName = gridKeyName,
                        UIMenuPID = menuId,
                        ColsSetting = json,
                        GridType = "NewSumDataGridView",
                        ColumnsMode = 0
                    };
                    await db.Insertable(newSetting).ExecuteReturnSnowflakeIdAsync();
                    System.Diagnostics.Debug.WriteLine($"[ColumnConfigManager] 新建配置: {gridKeyName}_{menuId}");
                }
                else
                {
                    if (existingSetting.ColsSetting != json)
                    {
                        existingSetting.ColsSetting = json;
                        await db.Updateable(existingSetting).ExecuteCommandAsync();
                        System.Diagnostics.Debug.WriteLine($"[ColumnConfigManager] 更新配置: {gridKeyName}_{menuId}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[ColumnConfigManager] 配置未变化，跳过保存: {gridKeyName}_{menuId}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ColumnConfigManager] 保存列配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取缓存键
        /// </summary>
        private string GetCacheKey(string gridKeyName, long menuId)
        {
            return $"{gridKeyName}_{menuId}";
        }

        /// <summary>
        /// 配置变更项
        /// </summary>
        private class ConfigChangeItem
        {
            public string GridKeyName { get; set; }
            public long MenuId { get; set; }
            public List<ColDisplayController> Config { get; set; }
            public DateTime Timestamp { get; set; }
        }

        /// <summary>
        /// 从XML文件迁移配置到数据库
        /// </summary>
        /// <param name="xmlFilePath">XML文件完整路径</param>
        /// <param name="gridKeyName">表格键名</param>
        /// <param name="menuId">菜单ID</param>
        /// <returns>迁移是否成功</returns>
        public async Task<bool> MigrateFromXmlAsync(string xmlFilePath, string gridKeyName, long menuId)
        {
            if (!System.IO.File.Exists(xmlFilePath))
            {
                return false;
            }

            try
            {
                var xmlHelper = new RUINORERP.Common.Helper.XmlHelper();
                var config = xmlHelper.deserialize_from_xml(xmlFilePath, typeof(List<ColDisplayController>)) as List<ColDisplayController>;

                if (config == null || config.Count == 0)
                {
                    return false;
                }

                await SaveToDatabaseAsync(gridKeyName, menuId, config);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"XML迁移失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检查数据库中是否存在配置
        /// </summary>
        public async Task<bool> HasConfigAsync(string gridKeyName, long menuId)
        {
            try
            {
                var db = MainForm.Instance.AppContext.Db;
                var count = await db.Queryable<tb_UIGridSetting>()
                    .Where(c => c.GridKeyName == gridKeyName && c.UIMenuPID == menuId)
                    .Where(c => !string.IsNullOrEmpty(c.ColsSetting))
                    .CountAsync();
                return count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
