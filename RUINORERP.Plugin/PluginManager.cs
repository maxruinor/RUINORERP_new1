using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
// 添加System.Drawing引用以解决CS7069错误
using System.Drawing;
using System.Security.Principal;
using RUINORERP.Common.Helper;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 插件管理器，负责扫描、加载和管理所有插件
    /// </summary>
    public class PluginManager
    {
        private readonly string _pluginDirectory;
        private readonly List<IPlugin> _plugins = new List<IPlugin>();
        private readonly Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();
        private Func<string, bool> _permissionChecker;
        private IPluginCommunicationChannel _communicationChannel;
        
        /// <summary>
        /// 获取已加载的所有插件
        /// </summary>
        public IReadOnlyCollection<IPlugin> Plugins => _plugins.AsReadOnly();
        
        /// <summary>
        /// 插件状态变更事件
        /// </summary>
        public event EventHandler<PluginStateChangedEventArgs> PluginStateChanged;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public PluginManager()
        {
            // 插件目录默认为应用程序目录下的Plugins文件夹
            _pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pluginDirectory">插件目录路径</param>
        public PluginManager(string pluginDirectory)
        {
            _pluginDirectory = pluginDirectory;
        }
        
        /// <summary>
        /// 设置权限检查委托
        /// </summary>
        /// <param name="permissionChecker">权限检查委托</param>
        public void SetPermissionChecker(Func<string, bool> permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }
        
        /// <summary>
        /// 设置插件与主程序的通信通道
        /// </summary>
        /// <param name="communicationChannel">通信通道实例</param>
        public void SetCommunicationChannel(IPluginCommunicationChannel communicationChannel)
        {
            _communicationChannel = communicationChannel;
        }
        
        /// <summary>
        /// 初始化插件管理器
        /// </summary>
        public void Initialize()
        {
            LoadPlugins();
        }
        
        /// <summary>
        /// 扫描并加载插件
        /// </summary>
        private void LoadPlugins()
        {
            try
            {
                // 创建插件目录（如果不存在）
                if (!Directory.Exists(_pluginDirectory))
                {
                    Directory.CreateDirectory(_pluginDirectory);
                }
                
                // 扫描插件目录下的所有DLL文件
                var pluginFiles = Directory.GetFiles(_pluginDirectory, "*.dll", SearchOption.AllDirectories);
                
                foreach (var pluginFile in pluginFiles)
                {
                    try
                    {
                        // 检查程序集是否已加载
                        string assemblyName = Path.GetFileNameWithoutExtension(pluginFile);
                        if (_loadedAssemblies.ContainsKey(assemblyName))
                        {
                            continue; // 已加载，跳过
                        }
                        
                        // 加载插件程序集，使用AssemblyLoader避免重复加载
                        var assembly = AssemblyLoader.LoadFromPath(pluginFile);
                        if (assembly == null)
                        {
                            continue;
                        }
                        _loadedAssemblies[assemblyName] = assembly;
                        
                        // 获取所有实现了IPlugin接口的非抽象类
                        var pluginTypes = assembly.GetTypes()
                            .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                            .ToList();
                        
                        foreach (var pluginType in pluginTypes)
                        {
                            try
                            {
                                // 创建插件实例
                                var plugin = (IPlugin)Activator.CreateInstance(pluginType);
                                
                                // 设置权限检查器
                                plugin.SetPermissionChecker(_permissionChecker);
                                
                                // 设置通信通道
                                if (_communicationChannel != null)
                                {
                                    plugin.SetCommunicationChannel(_communicationChannel);
                                }
                                
                                // 初始化插件
                                if (plugin.Initialize())
                                {
                                    // 启动插件
                                    if (plugin.Start())
                                    {
                                        // 添加到插件列表
                                        _plugins.Add(plugin);
                                        OnPluginStateChanged(new PluginStateChangedEventArgs(plugin, PluginState.Uninitialized, PluginState.Running));
                                    }
                                    else
                                    {
                                        OnPluginStateChanged(new PluginStateChangedEventArgs(plugin, PluginState.Initialized, PluginState.Error));
                                    }
                                }
                                else
                                {
                                    OnPluginStateChanged(new PluginStateChangedEventArgs(plugin, PluginState.Uninitialized, PluginState.Error));
                                }
                            }
                            catch (Exception ex)
                            {
                                // 记录插件实例化错误
                                System.Diagnostics.Debug.WriteLine($"创建插件实例失败: {pluginType.Name}, 错误: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录插件加载错误
                        System.Diagnostics.Debug.WriteLine($"加载插件失败: {pluginFile}, 错误: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录整体扫描错误
                System.Diagnostics.Debug.WriteLine($"扫描插件目录失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 执行所有插件
        /// </summary>
        public void ExecuteAllPlugins()
        {
            foreach (var plugin in _plugins)
            {
                try
                {
                    plugin.Execute();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"执行插件失败: {plugin.Name}, 错误: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 获取指定名称的插件
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <returns>插件实例</returns>
        public IPlugin GetPluginByName(string pluginName)
        {
            return _plugins.FirstOrDefault(p => p.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// 为所有插件注册菜单项
        /// </summary>
        /// <param name="menu">目标菜单</param>
        public void RegisterPluginMenuItems(ToolStripMenuItem menu)
        {
            if (menu == null) return;
            
            // 清除现有菜单项
            menu.DropDownItems.Clear();
            
            // 为每个插件添加菜单项
            foreach (var plugin in _plugins)
            {
                try
                {
                    // 检查用户是否有权限访问此插件
                    if (!plugin.HasPermission())
                    {
                        // 用户没有权限，跳过此插件
                        continue;
                    }
                    
                    var pluginMenuItem = plugin.GetMenuItem();
                    if (pluginMenuItem != null)
                    {
                        menu.DropDownItems.Add(pluginMenuItem);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"注册插件菜单项失败: {plugin.Name}, 错误: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 重新加载所有插件
        /// </summary>
        public void ReloadPlugins()
        {
            // 停止所有插件
            foreach (var plugin in _plugins)
            {
                try
                {
                    plugin.Stop();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"停止插件失败: {plugin.Name}, 错误: {ex.Message}");
                }
            }
            
            _plugins.Clear();
            _loadedAssemblies.Clear();
            LoadPlugins();
        }
        
        /// <summary>
        /// 启动指定插件
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <returns>是否启动成功</returns>
        public bool StartPlugin(string pluginName)
        {
            var plugin = GetPluginByName(pluginName);
            if (plugin != null && plugin.State != PluginState.Running)
            {
                PluginState oldState = plugin.State;
                bool result = plugin.Start();
                if (result)
                {
                    OnPluginStateChanged(new PluginStateChangedEventArgs(plugin, oldState, PluginState.Running));
                }
                return result;
            }
            return false;
        }
        
        /// <summary>
        /// 停止指定插件
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        public void StopPlugin(string pluginName)
        {
            var plugin = GetPluginByName(pluginName);
            if (plugin != null && plugin.State == PluginState.Running)
            {
                PluginState oldState = plugin.State;
                plugin.Stop();
                OnPluginStateChanged(new PluginStateChangedEventArgs(plugin, oldState, plugin.State));
            }
        }
        
        /// <summary>
        /// 触发插件状态变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnPluginStateChanged(PluginStateChangedEventArgs e)
        {
            PluginStateChanged?.Invoke(this, e);
        }
    }
    
    /// <summary>
    /// 插件状态变更事件参数
    /// </summary>
    public class PluginStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 插件实例
        /// </summary>
        public IPlugin Plugin { get; }
        
        /// <summary>
        /// 旧状态
        /// </summary>
        public PluginState OldState { get; }
        
        /// <summary>
        /// 新状态
        /// </summary>
        public PluginState NewState { get; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="plugin">插件实例</param>
        /// <param name="oldState">旧状态</param>
        /// <param name="newState">新状态</param>
        public PluginStateChangedEventArgs(IPlugin plugin, PluginState oldState, PluginState newState)
        {
            Plugin = plugin;
            OldState = oldState;
            NewState = newState;
        }
    }
}