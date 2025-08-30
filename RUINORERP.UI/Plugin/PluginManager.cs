using Autofac;
using RUINORERP.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 插件管理器，负责扫描、加载和管理所有插件
    /// </summary>
    public class PluginManager
    {
        private readonly string _pluginDirectory;
        private readonly List<IPlugin> _plugins = new List<IPlugin>();
        
        /// <summary>
        /// 获取已加载的所有插件
        /// </summary>
        public IReadOnlyCollection<IPlugin> Plugins => _plugins.AsReadOnly();
        
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
                        // 加载插件程序集
                        var assembly = Assembly.LoadFrom(pluginFile);
                        
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
                                
                                // 初始化插件
                                plugin.Initialize();
                                
                                // 添加到插件列表
                                _plugins.Add(plugin);
                            }
                            catch (Exception ex)
                            {
                                // 记录插件实例化错误
                                Console.WriteLine($"创建插件实例失败: {pluginType.Name}, 错误: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录插件加载错误
                        Console.WriteLine($"加载插件失败: {pluginFile}, 错误: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录整体扫描错误
                Console.WriteLine($"扫描插件目录失败: {ex.Message}");
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
                    Console.WriteLine($"执行插件失败: {plugin.Name}, 错误: {ex.Message}");
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
                    var pluginMenuItem = plugin.GetMenuItem();
                    if (pluginMenuItem != null)
                    {
                        menu.DropDownItems.Add(pluginMenuItem);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"注册插件菜单项失败: {plugin.Name}, 错误: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 重新加载所有插件
        /// </summary>
        public void ReloadPlugins()
        {
            _plugins.Clear();
            LoadPlugins();
        }
    }
}