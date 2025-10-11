using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Plugin;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 插件权限服务，用于检查用户对插件的访问权限
    /// </summary>
    public class PluginPermissionService
    {
        // 模拟用户权限数据存储
        // 在实际应用中，这应该从数据库或缓存中获取
        private static readonly Dictionary<string, HashSet<string>> UserPermissions = new Dictionary<string, HashSet<string>>
        {
            // 管理员用户拥有所有插件权限
            { "admin", new HashSet<string> { "PLUGIN_*" } },
            
            // 普通用户只拥有特定插件权限
            { "user1", new HashSet<string> { "PLUGIN_SALES", "PLUGIN_INVENTORY" } },
            
            // 另一个普通用户拥有不同的插件权限
            { "user2", new HashSet<string> { "PLUGIN_REPORTS" } }
        };
        
        /// <summary>
        /// 检查指定用户是否有权访问指定插件
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pluginPermissionKey">插件权限键</param>
        /// <returns>是否有权限</returns>
        public bool CheckPluginPermission(string username, string pluginPermissionKey)
        {
            // 参数验证
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pluginPermissionKey))
                return false;
            
            // 检查用户是否存在
            if (!UserPermissions.ContainsKey(username))
                return false;
            
            var userPerms = UserPermissions[username];
            
            // 检查是否有通配符权限
            if (userPerms.Contains("PLUGIN_*"))
                return true;
            
            // 检查具体权限
            return userPerms.Contains(pluginPermissionKey);
        }
        
        /// <summary>
        /// 获取用户所有插件权限
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>权限列表</returns>
        public IEnumerable<string> GetUserPluginPermissions(string username)
        {
            if (string.IsNullOrEmpty(username) || !UserPermissions.ContainsKey(username))
                return Enumerable.Empty<string>();
            
            return UserPermissions[username];
        }
        
        /// <summary>
        /// 为插件管理器创建权限检查委托
        /// </summary>
        /// <param name="currentUsername">当前用户名</param>
        /// <returns>权限检查委托</returns>
        public Func<string, bool> CreatePermissionChecker(string currentUsername)
        {
            return (pluginPermissionKey) => CheckPluginPermission(currentUsername, pluginPermissionKey);
        }
    }
}