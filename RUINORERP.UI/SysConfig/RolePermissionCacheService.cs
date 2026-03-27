using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Model;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.CustomAttribute;

namespace RUINORERP.UI.SysConfig
{
    /// <summary>
    /// 角色权限缓存服务
    /// 提供本地缓存管理、增量同步、变更追踪功能
    /// 【优化】解决频繁数据库查询问题，提升角色切换性能
    /// </summary>
    [NoWantIOC]
    public class RolePermissionCacheService
    {
        #region 单例模式

        private static readonly Lazy<RolePermissionCacheService> _instance =
            new Lazy<RolePermissionCacheService>(() => new RolePermissionCacheService());

        public static RolePermissionCacheService Instance => _instance.Value;

        /// <summary>
        /// 公共构造函数（用于 Autofac 兼容）
        /// 注意：请使用 Instance 属性获取单例实例
        /// </summary>
        public RolePermissionCacheService()
        {
            _cache = new ConcurrentDictionary<long, RolePermissionCacheItem>();
            _changeTracker = new ConcurrentDictionary<long, List<PermissionChange>>();
        }

        #endregion

        #region 缓存数据结构

        /// <summary>
        /// 权限缓存字典
        /// Key: RoleId, Value: 角色权限缓存项
        /// </summary>
        private readonly ConcurrentDictionary<long, RolePermissionCacheItem> _cache;

        /// <summary>
        /// 变更追踪字典
        /// Key: RoleId, Value: 变更列表
        /// </summary>
        private readonly ConcurrentDictionary<long, List<PermissionChange>> _changeTracker;

        /// <summary>
        /// 角色权限缓存项
        /// </summary>
        public class RolePermissionCacheItem
        {
            /// <summary>
            /// 角色ID
            /// </summary>
            public long RoleId { get; set; }

            /// <summary>
            /// 菜单权限列表
            /// </summary>
            public List<MenuPermissionCache> MenuPermissions { get; set; }

            /// <summary>
            /// 按钮权限列表
            /// </summary>
            public List<ButtonPermissionCache> ButtonPermissions { get; set; }

            /// <summary>
            /// 字段权限列表
            /// </summary>
            public List<FieldPermissionCache> FieldPermissions { get; set; }

            /// <summary>
            /// 行级权限列表
            /// </summary>
            public List<RowAuthPermissionCache> RowAuthPermissions { get; set; }

            /// <summary>
            /// 最后同步时间
            /// </summary>
            public DateTime LastSyncTime { get; set; }

            /// <summary>
            /// 数据版本号
            /// </summary>
            public long Version { get; set; }

            /// <summary>
            /// 是否有未保存的变更
            /// </summary>
            public bool IsDirty { get; set; }

            /// <summary>
            /// 缓存创建时间
            /// </summary>
            public DateTime CacheCreatedTime { get; set; }

            public RolePermissionCacheItem()
            {
                MenuPermissions = new List<MenuPermissionCache>();
                ButtonPermissions = new List<ButtonPermissionCache>();
                FieldPermissions = new List<FieldPermissionCache>();
                RowAuthPermissions = new List<RowAuthPermissionCache>();
                LastSyncTime = DateTime.MinValue;
                Version = 0;
                IsDirty = false;
                CacheCreatedTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 菜单权限缓存
        /// </summary>
        public class MenuPermissionCache
        {
            public long MenuId { get; set; }
            public bool IsVisible { get; set; }
            public PermissionStatus Status { get; set; }
            public long? P4MenuId { get; set; }
            public string MenuName { get; set; }
        }

        /// <summary>
        /// 按钮权限缓存
        /// </summary>
        public class ButtonPermissionCache
        {
            public long ButtonInfoId { get; set; }
            public long MenuId { get; set; }
            public bool IsVisible { get; set; }
            public bool IsEnabled { get; set; }
            public PermissionStatus Status { get; set; }
            public long? P4BtnId { get; set; }
            public string ButtonName { get; set; }
        }

        /// <summary>
        /// 字段权限缓存
        /// </summary>
        public class FieldPermissionCache
        {
            public long FieldInfoId { get; set; }
            public long MenuId { get; set; }
            public bool IsVisible { get; set; }
            public bool? IsChild { get; set; }
            public PermissionStatus Status { get; set; }
            public long? P4FieldId { get; set; }
            public string FieldName { get; set; }
        }

        /// <summary>
        /// 行级权限缓存
        /// </summary>
        public class RowAuthPermissionCache
        {
            public long PolicyId { get; set; }
            public long MenuId { get; set; }
            public bool IsEnabled { get; set; }
            public PermissionStatus Status { get; set; }
            public long? PolicyRoleRId { get; set; }
            public string PolicyName { get; set; }
            public string BizType { get; set; }
            public string BizName { get; set; }
            public string Condition { get; set; }
        }

        /// <summary>
        /// 权限状态枚举
        /// </summary>
        public enum PermissionStatus
        {
            Unchanged,  // 未变更
            Added,      // 新增
            Modified,   // 修改
            Deleted     // 删除
        }

        /// <summary>
        /// 权限变更记录
        /// </summary>
        public class PermissionChange
        {
            public long ChangeId { get; set; }
            public long RoleId { get; set; }
            public PermissionType Type { get; set; }
            public long ItemId { get; set; }
            public string ItemName { get; set; }
            public object OldValue { get; set; }
            public object NewValue { get; set; }
            public DateTime ChangeTime { get; set; }
        }

        /// <summary>
        /// 权限类型枚举
        /// </summary>
        public enum PermissionType
        {
            Menu,
            Button,
            Field,
            RowAuth
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 获取角色权限（优先从缓存）
        /// 【优化】实现缓存优先策略，减少数据库查询
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <returns>角色权限缓存项</returns>
        public async Task<RolePermissionCacheItem> GetRolePermissionAsync(long roleId, bool forceRefresh = false)
        {
            // 检查缓存是否存在且有效
            if (!forceRefresh && _cache.TryGetValue(roleId, out var cachedItem))
            {
                // 检查缓存是否过期（默认30分钟）
                if (DateTime.Now - cachedItem.LastSyncTime < TimeSpan.FromMinutes(30))
                {
                    MainForm.Instance.logger?.LogDebug($"角色[{roleId}]权限数据从缓存获取");
                    return cachedItem;
                }
            }

            // 缓存不存在或已过期，从数据库加载
            return await LoadFromDatabaseAsync(roleId);
        }

        /// <summary>
        /// 从数据库加载角色权限数据
        /// </summary>
        private async Task<RolePermissionCacheItem> LoadFromDatabaseAsync(long roleId)
        {
            try
            {
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                db.Ado.CommandTimeOut = 30;

                var cacheItem = new RolePermissionCacheItem { RoleId = roleId };

                // 并行加载所有权限数据
                var tasks = new List<Task>
                {
                    // 加载菜单权限
                    LoadMenuPermissionsAsync(db, roleId, cacheItem),

                    // 加载按钮权限
                    LoadButtonPermissionsAsync(db, roleId, cacheItem),

                    // 加载字段权限
                    LoadFieldPermissionsAsync(db, roleId, cacheItem),

                    // 加载行级权限
                    LoadRowAuthPermissionsAsync(db, roleId, cacheItem)
                };

                await Task.WhenAll(tasks);

                cacheItem.LastSyncTime = DateTime.Now;
                cacheItem.Version = 1;
                cacheItem.IsDirty = false;

                // 更新缓存
                _cache[roleId] = cacheItem;

                MainForm.Instance.logger?.LogInformation($"角色[{roleId}]权限数据从数据库加载完成：" +
                    $"菜单{cacheItem.MenuPermissions.Count}个，" +
                    $"按钮{cacheItem.ButtonPermissions.Count}个，" +
                    $"字段{cacheItem.FieldPermissions.Count}个");

                return cacheItem;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"加载角色[{roleId}]权限数据失败");
                throw;
            }
        }

        /// <summary>
        /// 加载菜单权限
        /// </summary>
        private async Task LoadMenuPermissionsAsync(SqlSugarClient db, long roleId, RolePermissionCacheItem cacheItem)
        {
            var menus = await db.Queryable<tb_P4Menu>()
                .Where(r => r.RoleID == roleId)
                .Includes(c => c.tb_menuinfo)
                .ToListAsync();

            cacheItem.MenuPermissions = menus.Select(m => new MenuPermissionCache
            {
                MenuId = m.MenuID ?? 0,
                IsVisible = m.IsVisble,
                Status = PermissionStatus.Unchanged,
                P4MenuId = m.P4Menu_ID,
                MenuName = m.tb_menuinfo?.CaptionCN ?? m.tb_menuinfo?.MenuName ?? ""
            }).ToList();
        }

        /// <summary>
        /// 加载按钮权限
        /// </summary>
        private async Task LoadButtonPermissionsAsync(SqlSugarClient db, long roleId, RolePermissionCacheItem cacheItem)
        {
            var buttons = await db.Queryable<tb_P4Button>()
                .Where(r => r.RoleID == roleId)
                .Includes(c => c.tb_buttoninfo)
                .ToListAsync();

            cacheItem.ButtonPermissions = buttons.Select(b => new ButtonPermissionCache
            {
                ButtonInfoId = b.ButtonInfo_ID ?? 0,
                MenuId = b.MenuID ?? 0,
                IsVisible = b.IsVisble,
                IsEnabled = b.IsEnabled,
                Status = PermissionStatus.Unchanged,
                P4BtnId = b.P4Btn_ID,
                ButtonName = b.tb_buttoninfo?.BtnText ?? b.Notes
            }).ToList();
        }

        /// <summary>
        /// 加载字段权限
        /// </summary>
        private async Task LoadFieldPermissionsAsync(SqlSugarClient db, long roleId, RolePermissionCacheItem cacheItem)
        {
            var fields = await db.Queryable<tb_P4Field>()
                .Where(r => r.RoleID == roleId)
                .Includes(c => c.tb_fieldinfo)
                .ToListAsync();

            cacheItem.FieldPermissions = fields.Select(f => new FieldPermissionCache
            {
                FieldInfoId = f.FieldInfo_ID,
                MenuId = f.MenuID ?? 0,
                IsVisible = f.IsVisble,
                IsChild = f.IsChild,
                Status = PermissionStatus.Unchanged,
                P4FieldId = f.P4Field_ID,
                FieldName = f.tb_fieldinfo?.FieldText ?? f.Notes
            }).ToList();
        }

        /// <summary>
        /// 加载行级权限
        /// </summary>
        private async Task LoadRowAuthPermissionsAsync(SqlSugarClient db, long roleId, RolePermissionCacheItem cacheItem)
        {
            var rowAuths = await db.Queryable<tb_P4RowAuthPolicyByRole>()
                .Where(r => r.RoleID == roleId)
                .Includes(c => c.tb_rowauthpolicy)
                .ToListAsync();

            cacheItem.RowAuthPermissions = rowAuths.Select(r => new RowAuthPermissionCache
            {
                PolicyId = r.PolicyId,
                MenuId = r.MenuID,
                IsEnabled = r.IsEnabled,
                Status = PermissionStatus.Unchanged,
                PolicyRoleRId = r.Policy_Role_RID,
                PolicyName = r.tb_rowauthpolicy?.PolicyName,
               // BizType = r.tb_rowauthpolicy?.BizType.ToString() ?? "",
                BizName = r.tb_rowauthpolicy?.PolicyName ?? "",
               // Condition = r.tb_rowauthpolicy?.Condition ?? ""
            }).ToList();
        }

        /// <summary>
        /// 增量同步权限数据
        /// 【优化】只拉取变更数据，减少数据传输量
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="lastSyncTime">最后同步时间</param>
        /// <returns>角色权限缓存项</returns>
        public async Task<RolePermissionCacheItem> IncrementalSyncAsync(long roleId, DateTime lastSyncTime)
        {
            try
            {
                var db = MainForm.Instance.AppContext.Db.CopyNew();

                // 获取缓存项
                if (!_cache.TryGetValue(roleId, out var cacheItem))
                {
                    return await LoadFromDatabaseAsync(roleId);
                }

                // 查询变更的菜单权限
                var changedMenus = await db.Queryable<tb_P4Menu>()
                    .Where(r => r.RoleID == roleId && r.Modified_at > lastSyncTime)
                    .ToListAsync();

                // 查询变更的按钮权限
                var changedButtons = await db.Queryable<tb_P4Button>()
                    .Where(r => r.RoleID == roleId && r.Modified_at > lastSyncTime)
                    .Includes(c => c.tb_buttoninfo)
                    .ToListAsync();

                // 查询变更的字段权限
                var changedFields = await db.Queryable<tb_P4Field>()
                    .Where(r => r.RoleID == roleId && r.Modified_at > lastSyncTime)
                    .Includes(c => c.tb_fieldinfo)
                    .ToListAsync();

                // 更新缓存中的变更项
                UpdateChangedPermissions(cacheItem, changedMenus, changedButtons, changedFields);

                cacheItem.LastSyncTime = DateTime.Now;
                cacheItem.Version++;

                MainForm.Instance.logger?.LogInformation($"角色[{roleId}]权限数据增量同步完成");

                return cacheItem;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"增量同步角色[{roleId}]权限数据失败");
                throw;
            }
        }

        /// <summary>
        /// 更新变更的权限到缓存
        /// </summary>
        private void UpdateChangedPermissions(RolePermissionCacheItem cacheItem,
            List<tb_P4Menu> changedMenus,
            List<tb_P4Button> changedButtons,
            List<tb_P4Field> changedFields)
        {
            // 更新菜单权限
            foreach (var menu in changedMenus)
            {
                var existing = cacheItem.MenuPermissions.FirstOrDefault(m => m.MenuId == menu.MenuID);
                if (existing != null)
                {
                    existing.IsVisible = menu.IsVisble;
                    existing.Status = PermissionStatus.Modified;
                }
                else
                {
                    cacheItem.MenuPermissions.Add(new MenuPermissionCache
                    {
                        MenuId = menu.MenuID ?? 0,
                        IsVisible = menu.IsVisble,
                        Status = PermissionStatus.Added,
                        P4MenuId = menu.P4Menu_ID
                    });
                }
            }

            // 更新按钮权限
            foreach (var button in changedButtons)
            {
                var existing = cacheItem.ButtonPermissions.FirstOrDefault(b => b.ButtonInfoId == button.ButtonInfo_ID);
                if (existing != null)
                {
                    existing.IsVisible = button.IsVisble;
                    existing.IsEnabled = button.IsEnabled;
                    existing.Status = PermissionStatus.Modified;
                }
                else
                {
                    cacheItem.ButtonPermissions.Add(new ButtonPermissionCache
                    {
                        ButtonInfoId = button.ButtonInfo_ID ?? 0,
                        MenuId = button.MenuID ?? 0,
                        IsVisible = button.IsVisble,
                        IsEnabled = button.IsEnabled,
                        Status = PermissionStatus.Added,
                        P4BtnId = button.P4Btn_ID,
                        ButtonName = button.tb_buttoninfo?.BtnText ?? button.Notes
                    });
                }
            }

            // 更新字段权限
            foreach (var field in changedFields)
            {
                var existing = cacheItem.FieldPermissions.FirstOrDefault(f => f.FieldInfoId == field.FieldInfo_ID);
                if (existing != null)
                {
                    existing.IsVisible = field.IsVisble;
                    existing.Status = PermissionStatus.Modified;
                }
                else
                {
                    cacheItem.FieldPermissions.Add(new FieldPermissionCache
                    {
                        FieldInfoId = field.FieldInfo_ID,
                        MenuId = field.MenuID ?? 0,
                        IsVisible = field.IsVisble,
                        IsChild = field.IsChild,
                        Status = PermissionStatus.Added,
                        P4FieldId = field.P4Field_ID,
                        FieldName = field.tb_fieldinfo?.FieldText ?? field.Notes
                    });
                }
            }
        }

        /// <summary>
        /// 标记权限变更
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="change">变更记录</param>
        public void MarkPermissionChanged(long roleId, PermissionChange change)
        {
            change.ChangeId = DateTime.Now.Ticks;
            change.ChangeTime = DateTime.Now;
            change.RoleId = roleId;

            var changes = _changeTracker.GetOrAdd(roleId, new List<PermissionChange>());
            lock (changes)
            {
                changes.Add(change);
            }

            // 标记缓存项为脏数据
            if (_cache.TryGetValue(roleId, out var cacheItem))
            {
                cacheItem.IsDirty = true;
            }

            MainForm.Instance.logger?.LogDebug($"标记角色[{roleId}]权限变更：{change.Type} - {change.ItemName}");
        }

        /// <summary>
        /// 获取变更列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>变更列表</returns>
        public List<PermissionChange> GetChanges(long roleId)
        {
            if (_changeTracker.TryGetValue(roleId, out var changes))
            {
                lock (changes)
                {
                    return changes.ToList();
                }
            }
            return new List<PermissionChange>();
        }

        /// <summary>
        /// 清除变更记录
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public void ClearChanges(long roleId)
        {
            _changeTracker.TryRemove(roleId, out _);

            if (_cache.TryGetValue(roleId, out var cacheItem))
            {
                cacheItem.IsDirty = false;
            }
        }

        /// <summary>
        /// 保存变更到数据库
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveChangesAsync(long roleId)
        {
            try
            {
                var changes = GetChanges(roleId);
                if (!changes.Any())
                {
                    return true;
                }

                var db = MainForm.Instance.AppContext.Db.CopyNew();

                // 按类型分组处理变更
                var menuChanges = changes.Where(c => c.Type == PermissionType.Menu).ToList();
                var buttonChanges = changes.Where(c => c.Type == PermissionType.Button).ToList();
                var fieldChanges = changes.Where(c => c.Type == PermissionType.Field).ToList();

                // 批量保存变更
                await SaveMenuChangesAsync(db, roleId, menuChanges);
                await SaveButtonChangesAsync(db, roleId, buttonChanges);
                await SaveFieldChangesAsync(db, roleId, fieldChanges);

                // 清除变更记录
                ClearChanges(roleId);

                // 更新缓存版本
                if (_cache.TryGetValue(roleId, out var cacheItem))
                {
                    cacheItem.Version++;
                    cacheItem.LastSyncTime = DateTime.Now;
                }

                MainForm.Instance.logger.LogInformation($"角色[{roleId}]权限变更保存成功，共{changes.Count}条");

                return true;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger?.LogError(ex, $"保存角色[{roleId}]权限变更失败");
                return false;
            }
        }

        /// <summary>
        /// 保存菜单权限变更
        /// </summary>
        private async Task SaveMenuChangesAsync(SqlSugarClient db, long roleId, List<PermissionChange> changes)
        {
            if (!changes.Any()) return;

            var toAdd = new List<tb_P4Menu>();
            var toUpdate = new List<tb_P4Menu>();
            var toDelete = new List<long>();

            foreach (var change in changes)
            {
                if (change.Type == PermissionType.Menu)
                {
                    var menuChange = change.NewValue as MenuPermissionCache;
                    if (menuChange == null) continue;

                    if (change.OldValue == null)
                    {
                        // 新增
                        toAdd.Add(new tb_P4Menu
                        {
                            RoleID = roleId,
                            MenuID = menuChange.MenuId,
                            IsVisble = menuChange.IsVisible
                        });
                    }
                    else if (menuChange.Status == PermissionStatus.Deleted)
                    {
                        // 删除
                        if (menuChange.P4MenuId.HasValue)
                        {
                            toDelete.Add(menuChange.P4MenuId.Value);
                        }
                    }
                    else
                    {
                        // 修改
                        toUpdate.Add(new tb_P4Menu
                        {
                            P4Menu_ID = menuChange.P4MenuId ?? 0,
                            RoleID = roleId,
                            MenuID = menuChange.MenuId,
                            IsVisble = menuChange.IsVisible
                        });
                    }
                }
            }

            // 批量执行数据库操作
            if (toAdd.Any())
            {
                await db.Insertable(toAdd).ExecuteReturnSnowflakeIdListAsync();
            }

            if (toUpdate.Any())
            {
                await db.Updateable(toUpdate).ExecuteCommandAsync();
            }

            if (toDelete.Any())
            {
                await db.Deleteable<tb_P4Menu>().Where(p => toDelete.Contains(p.P4Menu_ID)).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 保存按钮权限变更
        /// </summary>
        private async Task SaveButtonChangesAsync(SqlSugarClient db, long roleId, List<PermissionChange> changes)
        {
            if (!changes.Any()) return;

            var toAdd = new List<tb_P4Button>();
            var toUpdate = new List<tb_P4Button>();
            var toDelete = new List<long>();

            foreach (var change in changes)
            {
                if (change.Type == PermissionType.Button)
                {
                    var buttonChange = change.NewValue as ButtonPermissionCache;
                    if (buttonChange == null) continue;

                    if (change.OldValue == null)
                    {
                        // 新增
                        toAdd.Add(new tb_P4Button
                        {
                            RoleID = roleId,
                            ButtonInfo_ID = buttonChange.ButtonInfoId,
                            MenuID = buttonChange.MenuId,
                            IsVisble = buttonChange.IsVisible,
                            IsEnabled = buttonChange.IsEnabled
                        });
                    }
                    else if (buttonChange.Status == PermissionStatus.Deleted)
                    {
                        // 删除
                        if (buttonChange.P4BtnId.HasValue)
                        {
                            toDelete.Add(buttonChange.P4BtnId.Value);
                        }
                    }
                    else
                    {
                        // 修改
                        toUpdate.Add(new tb_P4Button
                        {
                            P4Btn_ID = buttonChange.P4BtnId ?? 0,
                            RoleID = (long)roleId,
                            ButtonInfo_ID = buttonChange.ButtonInfoId,
                            MenuID = buttonChange.MenuId,
                            IsVisble = buttonChange.IsVisible,
                            IsEnabled = buttonChange.IsEnabled
                        });
                    }
                }
            }

            // 批量执行数据库操作
            if (toAdd.Any())
            {
                await db.Insertable(toAdd).ExecuteReturnSnowflakeIdListAsync();
            }

            if (toUpdate.Any())
            {
                await db.Updateable(toUpdate).ExecuteCommandAsync();
            }

            if (toDelete.Any())
            {
                await db.Deleteable<tb_P4Button>().Where(p => toDelete.Contains(p.P4Btn_ID)).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 保存字段权限变更
        /// </summary>
        private async Task SaveFieldChangesAsync(SqlSugarClient db, long roleId, List<PermissionChange> changes)
        {
            if (!changes.Any()) return;

            var toAdd = new List<tb_P4Field>();
            var toUpdate = new List<tb_P4Field>();
            var toDelete = new List<long>();

            foreach (var change in changes)
            {
                if (change.Type == PermissionType.Field)
                {
                    var fieldChange = change.NewValue as FieldPermissionCache;
                    if (fieldChange == null) continue;

                    if (change.OldValue == null)
                    {
                        // 新增
                        toAdd.Add(new tb_P4Field
                        {
                            RoleID = roleId,
                            FieldInfo_ID = fieldChange.FieldInfoId,
                            MenuID = fieldChange.MenuId,
                            IsVisble = fieldChange.IsVisible,
                            IsChild = fieldChange.IsChild
                        });
                    }
                    else if (fieldChange.Status == PermissionStatus.Deleted)
                    {
                        // 删除
                        if (fieldChange.P4FieldId.HasValue)
                        {
                            toDelete.Add(fieldChange.P4FieldId.Value);
                        }
                    }
                    else
                    {
                        // 修改
                        toUpdate.Add(new tb_P4Field
                        {
                            P4Field_ID = fieldChange.P4FieldId ?? 0,
                            RoleID = roleId,
                            FieldInfo_ID = fieldChange.FieldInfoId,
                            MenuID = fieldChange.MenuId,
                            IsVisble = fieldChange.IsVisible,
                            IsChild = fieldChange.IsChild
                        });
                    }
                }
            }

            // 批量执行数据库操作
            if (toAdd.Any())
            {
                await db.Insertable(toAdd).ExecuteReturnSnowflakeIdListAsync();
            }

            if (toUpdate.Any())
            {
                await db.Updateable(toUpdate).ExecuteCommandAsync();
            }

            if (toDelete.Any())
            {
                await db.Deleteable<tb_P4Field>().Where(p => toDelete.Contains(p.P4Field_ID)).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public void ClearCache(long roleId)
        {
            _cache.TryRemove(roleId, out _);
            _changeTracker.TryRemove(roleId, out _);

            MainForm.Instance.logger?.LogDebug($"清除角色[{roleId}]权限缓存");
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            _cache.Clear();
            _changeTracker.Clear();

            MainForm.Instance.logger?.LogInformation("清除所有角色权限缓存");
        }

        /// <summary>
        /// 检查是否有未保存的变更
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>是否有未保存的变更</returns>
        public bool HasUnsavedChanges(long roleId)
        {
            if (_changeTracker.TryGetValue(roleId, out var changes))
            {
                lock (changes)
                {
                    return changes.Any();
                }
            }
            return false;
        }

        /// <summary>
        /// 获取未保存变更数量
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>变更数量</returns>
        public int GetUnsavedChangesCount(long roleId)
        {
            if (_changeTracker.TryGetValue(roleId, out var changes))
            {
                lock (changes)
                {
                    return changes.Count;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public CacheStatistics GetCacheStatistics()
        {
            return new CacheStatistics
            {
                CachedRoleCount = _cache.Count,
                TotalMenuPermissions = _cache.Values.Sum(c => c.MenuPermissions.Count),
                TotalButtonPermissions = _cache.Values.Sum(c => c.ButtonPermissions.Count),
                TotalFieldPermissions = _cache.Values.Sum(c => c.FieldPermissions.Count),
                TotalRowAuthPermissions = _cache.Values.Sum(c => c.RowAuthPermissions.Count),
                TotalTrackedChanges = _changeTracker.Values.Sum(c => c.Count),
                CacheMemorySize = EstimateCacheMemorySize()
            };
        }

        /// <summary>
        /// 估算缓存内存大小（粗略估计）
        /// </summary>
        private long EstimateCacheMemorySize()
        {
            // 粗略估算每个缓存项的大小
            long sizePerRole = 1024; // 基础开销 1KB
            foreach (var cacheItem in _cache.Values)
            {
                sizePerRole += cacheItem.MenuPermissions.Count * 32;
                sizePerRole += cacheItem.ButtonPermissions.Count * 64;
                sizePerRole += cacheItem.FieldPermissions.Count * 64;
                sizePerRole += cacheItem.RowAuthPermissions.Count * 48;
            }
            return sizePerRole * _cache.Count;
        }

        #endregion
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        public int CachedRoleCount { get; set; }
        public int TotalMenuPermissions { get; set; }
        public int TotalButtonPermissions { get; set; }
        public int TotalFieldPermissions { get; set; }
        public int TotalRowAuthPermissions { get; set; }
        public int TotalTrackedChanges { get; set; }
        public long CacheMemorySize { get; set; }

        public override string ToString()
        {
            return $"缓存角色数: {CachedRoleCount}, " +
                   $"菜单权限: {TotalMenuPermissions}, " +
                   $"按钮权限: {TotalButtonPermissions}, " +
                   $"字段权限: {TotalFieldPermissions}, " +
                   $"行级权限: {TotalRowAuthPermissions}, " +
                   $"待保存变更: {TotalTrackedChanges}, " +
                   $"内存占用: {CacheMemorySize / 1024}KB";
        }
    }
}