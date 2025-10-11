using System;
using System.Windows.Forms;
using RUINORERP.Plugin;
using RUINORERP.Server.Services;

namespace RUINORERP.Server.Examples
{
    /// <summary>
    /// 插件权限集成示例
    /// 展示如何在主程序中集成插件权限控制系统
    /// </summary>
    public class PluginPermissionIntegrationExample
    {
        private PluginManager _pluginManager;
        private PluginPermissionService _permissionService;
        
        /// <summary>
        /// 初始化插件权限集成示例
        /// </summary>
        public void Initialize()
        {
            // 创建插件管理器
            _pluginManager = new PluginManager();
            
            // 创建权限服务
            _permissionService = new PluginPermissionService();
            
            // 设置当前用户（在实际应用中，这应该从登录会话中获取）
            string currentUser = "user1"; // 示例用户
            
            // 为插件管理器设置权限检查器
            _pluginManager.SetPermissionChecker(
                _permissionService.CreatePermissionChecker(currentUser)
            );
            
            // 初始化插件管理器
            _pluginManager.Initialize();
        }
        
        /// <summary>
        /// 注册插件菜单项到主程序菜单
        /// </summary>
        /// <param name="mainMenu">主程序菜单</param>
        public void RegisterPluginMenus(ToolStripMenuItem mainMenu)
        {
            // 这将自动根据用户权限过滤插件菜单项
            _pluginManager.RegisterPluginMenuItems(mainMenu);
        }
        
        /// <summary>
        /// 切换当前用户并重新加载插件权限
        /// </summary>
        /// <param name="newUsername">新用户名</param>
        public void SwitchUser(string newUsername)
        {
            // 更新权限检查器
            _pluginManager.SetPermissionChecker(
                _permissionService.CreatePermissionChecker(newUsername)
            );
            
            // 注意：在实际应用中，可能需要重新加载插件或刷新界面
            // 以反映新的权限设置
        }
        
        /// <summary>
        /// 检查当前用户是否有权访问指定插件
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <returns>是否有权限</returns>
        public bool CheckCurrentUserPluginAccess(string pluginName)
        {
            var plugin = _pluginManager.GetPluginByName(pluginName);
            return plugin?.HasPermission() ?? false;
        }
    }
}