/**
 * 文件: SimpleCacheManager.cs
 * 说明: 简化版缓存管理器 - 提供基本的缓存功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 简化版缓存管理器 - 提供基本的缓存功能
    /// 替代原来复杂的缓存机制，保留必要的性能优化
    /// </summary>
    public class SimpleCacheManager
    {
        #region 私有字段

        /// <summary>
        /// 状态转换规则缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _transitionRuleCache;

        /// <summary>
        /// UI控件状态缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, (bool Enabled, bool Visible)> _uiControlStateCache;

        /// <summary>
        /// 操作权限缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, bool> _actionPermissionCache;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<SimpleCacheManager> _logger;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化简化版缓存管理器
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public SimpleCacheManager(ILogger<SimpleCacheManager> logger = null)
        {
            _logger = logger;
            _transitionRuleCache = new ConcurrentDictionary<string, bool>();
            _uiControlStateCache = new ConcurrentDictionary<string, (bool, bool)>();
            _actionPermissionCache = new ConcurrentDictionary<string, bool>();
            _statusTypeCache = new ConcurrentDictionary<string, string>();
        }

        #endregion

        #region 状态转换规则缓存

        /// <summary>
        /// 获取状态转换规则缓存
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>是否允许转换，如果缓存中不存在则返回null</returns>
        public bool? GetTransitionRuleCache(Enum fromStatus, Enum toStatus, Type statusType)
        {
            string cacheKey = $"{statusType.Name}_{fromStatus}_{toStatus}";
            
            if (_transitionRuleCache.TryGetValue(cacheKey, out bool cachedValue))
            {
                _logger?.LogDebug("从缓存中获取状态转换规则: {CacheKey} = {Value}", cacheKey, cachedValue);
                return cachedValue;
            }

            return null;
        }

        /// <summary>
        /// 设置状态转换规则缓存
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="isAllowed">是否允许转换</param>
        public void SetTransitionRuleCache(Enum fromStatus, Enum toStatus, Type statusType, bool isAllowed)
        {
            string cacheKey = $"{statusType.Name}_{fromStatus}_{toStatus}";
            _transitionRuleCache[cacheKey] = isAllowed;
            _logger?.LogDebug("设置状态转换规则缓存: {CacheKey} = {Value}", cacheKey, isAllowed);
        }

        #endregion

        #region UI控件状态缓存

        /// <summary>
        /// 获取UI控件状态缓存
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controlName">控件名称</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>控件状态，如果缓存中不存在则返回null</returns>
        public (bool Enabled, bool Visible)? GetUIControlStateCache(Enum status, string controlName, Type statusType)
        {
            string cacheKey = $"{statusType.Name}_{status}_{controlName}";
            
            if (_uiControlStateCache.TryGetValue(cacheKey, out (bool, bool) cachedValue))
            {
                _logger?.LogDebug("从缓存中获取UI控件状态: {CacheKey} = {Value}", cacheKey, cachedValue);
                return cachedValue;
            }

            return null;
        }

        /// <summary>
        /// 设置UI控件状态缓存
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controlName">控件名称</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        public void SetUIControlStateCache(Enum status, string controlName, Type statusType, bool enabled, bool visible)
        {
            string cacheKey = $"{statusType.Name}_{status}_{controlName}";
            _uiControlStateCache[cacheKey] = (enabled, visible);
            _logger?.LogDebug("设置UI控件状态缓存: {CacheKey} = ({Enabled}, {Visible})", cacheKey, enabled, visible);
        }

        #endregion

        #region 操作权限缓存

        /// <summary>
        /// 获取操作权限缓存
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>是否允许操作，如果缓存中不存在则返回null</returns>
        public bool? GetActionPermissionCache(Enum status, string actionName, Type statusType)
        {
            string cacheKey = $"{statusType.Name}_{status}_{actionName}";
            
            if (_actionPermissionCache.TryGetValue(cacheKey, out bool cachedValue))
            {
                _logger?.LogDebug("从缓存中获取操作权限: {CacheKey} = {Value}", cacheKey, cachedValue);
                return cachedValue;
            }

            return null;
        }

        /// <summary>
        /// 设置操作权限缓存
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="isAllowed">是否允许操作</param>
        public void SetActionPermissionCache(Enum status, string actionName, Type statusType, bool isAllowed)
        {
            string cacheKey = $"{statusType.Name}_{status}_{actionName}";
            _actionPermissionCache[cacheKey] = isAllowed;
            _logger?.LogDebug("设置操作权限缓存: {CacheKey} = {Value}", cacheKey, isAllowed);
        }

        #endregion

        #region 状态类型缓存

        /// <summary>
        /// 状态类型缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _statusTypeCache;

        /// <summary>
        /// 获取状态类型名称
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态类型名称，如果缓存中不存在则返回null</returns>
        public string GetStatusType(Type statusType)
        {
            if (statusType == null) return null;
            
            string cacheKey = statusType.FullName;
            
            if (_statusTypeCache.TryGetValue(cacheKey, out string cachedValue))
            {
                _logger?.LogDebug("从缓存中获取状态类型名称: {CacheKey} = {Value}", cacheKey, cachedValue);
                return cachedValue;
            }

            return null;
        }

        /// <summary>
        /// 设置状态类型名称
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="typeName">状态类型名称</param>
        public void SetStatusType(Type statusType, string typeName)
        {
            if (statusType == null) return;
            
            string cacheKey = statusType.FullName;
            _statusTypeCache[cacheKey] = typeName;
            _logger?.LogDebug("设置状态类型名称缓存: {CacheKey} = {Value}", cacheKey, typeName);
        }

        #endregion

        #region 缓存管理

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            _transitionRuleCache.Clear();
            _uiControlStateCache.Clear();
            _actionPermissionCache.Clear();
            _statusTypeCache.Clear();
            _logger?.LogDebug("已清除所有缓存");
        }

        /// <summary>
        /// 清除指定状态类型的所有缓存
        /// </summary>
        /// <param name="statusType">状态类型</param>
        public void ClearCacheByStatusType(Type statusType)
        {
            string prefix = $"{statusType.Name}_";
            
            // 清除状态转换规则缓存
            var keysToRemove = new List<string>();
            foreach (var key in _transitionRuleCache.Keys)
            {
                if (key.StartsWith(prefix))
                {
                    keysToRemove.Add(key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                _transitionRuleCache.TryRemove(key, out _);
            }
            
            // 清除UI控件状态缓存
            keysToRemove.Clear();
            foreach (var key in _uiControlStateCache.Keys)
            {
                if (key.StartsWith(prefix))
                {
                    keysToRemove.Add(key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                _uiControlStateCache.TryRemove(key, out _);
            }
            
            // 清除操作权限缓存
            keysToRemove.Clear();
            foreach (var key in _actionPermissionCache.Keys)
            {
                if (key.StartsWith(prefix))
                {
                    keysToRemove.Add(key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                _actionPermissionCache.TryRemove(key, out _);
            }
            
            // 清除状态类型缓存
            keysToRemove.Clear();
            foreach (var key in _statusTypeCache.Keys)
            {
                if (key.StartsWith(prefix))
                {
                    keysToRemove.Add(key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                _statusTypeCache.TryRemove(key, out _);
            }
            
            _logger?.LogDebug("已清除状态类型 {StatusType} 的所有缓存", statusType.Name);
        }

        #endregion
    }
}