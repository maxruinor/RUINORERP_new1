using System;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 插件基类，提供插件通用功能
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
        private PluginState _state = PluginState.Uninitialized;
        private readonly Dictionary<string, object> _pluginData = new Dictionary<string, object>();
        private Func<string, bool> _permissionChecker;
        
        /// <summary>
        /// 插件名称
        /// </summary>
        public abstract string Name { get; }
        
        /// <summary>
        /// 插件描述
        /// </summary>
        public abstract string Description { get; }
        
        /// <summary>
        /// 插件版本
        /// </summary>
        public abstract Version Version { get; }
        
        /// <summary>
        /// 插件状态
        /// </summary>
        public PluginState State => _state;
        
        /// <summary>
        /// 插件权限标识
        /// </summary>
        public virtual string PermissionKey => $"PLUGIN_{this.Name.ToUpper()}";
        
        /// <summary>
        /// 插件目录
        /// </summary>
        protected string PluginDirectory { get; private set; }
        
        /// <summary>
        /// 插件数据目录
        /// </summary>
        protected string PluginDataDirectory { get; private set; }
        
        /// <summary>
        /// 插件菜单项目
        /// </summary>
        protected ToolStripMenuItem PluginMenuItem { get; private set; }
        
        /// <summary>
        /// 初始化插件
        /// </summary>
        public virtual bool Initialize()
        {
            try
            {
                // 设置插件目录
                PluginDirectory = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);
                PluginDataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PluginData", Name);
                
                // 创建插件数据目录
                if (!Directory.Exists(PluginDataDirectory))
                {
                    Directory.CreateDirectory(PluginDataDirectory);
                }
                
                // 创建插件菜单项
                PluginMenuItem = new ToolStripMenuItem(Name)
                {
                    Tag = this, // 存储插件引用
                    ToolTipText = Description
                };
                
                // 添加点击事件处理
                PluginMenuItem.Click += OnPluginMenuItemClick;
                
                // 初始化插件时的其他操作
                OnInitialize();
                
                _state = PluginState.Initialized;
                return true;
            }
            catch (Exception ex)
            {
                _state = PluginState.Error;
                ShowError($"初始化插件 {Name} 时发生错误: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 启动插件
        /// </summary>
        public virtual bool Start()
        {
            if (_state != PluginState.Initialized && _state != PluginState.Stopped)
            {
                ShowError($"插件 {Name} 状态不正确，无法启动");
                return false;
            }
            
            try
            {
                OnStart();
                _state = PluginState.Running;
                return true;
            }
            catch (Exception ex)
            {
                _state = PluginState.Error;
                ShowError($"启动插件 {Name} 时发生错误: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 停止插件
        /// </summary>
        public virtual void Stop()
        {
            if (_state != PluginState.Running)
            {
                return;
            }
            
            try
            {
                OnStop();
                _state = PluginState.Stopped;
            }
            catch (Exception ex)
            {
                _state = PluginState.Error;
                ShowError($"停止插件 {Name} 时发生错误: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 执行插件功能
        /// </summary>
        public abstract void Execute();
        
        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <returns>插件菜单项</returns>
        public virtual ToolStripMenuItem GetMenuItem()
        {
            return PluginMenuItem;
        }
        
        /// <summary>
        /// 检查当前用户是否有权限使用此插件
        /// </summary>
        /// <returns>是否有权限</returns>
        public virtual bool HasPermission()
        {
            // 如果没有设置权限检查器，默认允许访问
            if (_permissionChecker == null)
                return true;
            
            // 使用权限检查器检查权限
            return _permissionChecker(this.PermissionKey);
        }
        
        /// <summary>
        /// 设置权限检查委托
        /// </summary>
        /// <param name="permissionChecker">权限检查委托</param>
        public virtual void SetPermissionChecker(Func<string, bool> permissionChecker)
        {
            _permissionChecker = permissionChecker;
        }
        
        /// <summary>
        /// 菜单项点击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPluginMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                if (_state != PluginState.Running)
                {
                    ShowWarning($"插件 {Name} 尚未运行，请先启动插件");
                    return;
                }
                
                Execute();
            }
            catch (Exception ex)
            {
                ShowError($"执行插件 {Name} 时发生错误: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 初始化插件时的自定义逻辑
        /// </summary>
        protected virtual void OnInitialize()
        {
            // 派生类可以重写此方法以添加自定义初始化逻辑
        }
        
        /// <summary>
        /// 启动插件时的自定义逻辑
        /// </summary>
        protected virtual void OnStart()
        {
            // 派生类可以重写此方法以添加自定义启动逻辑
        }
        
        /// <summary>
        /// 停止插件时的自定义逻辑
        /// </summary>
        protected virtual void OnStop()
        {
            // 派生类可以重写此方法以添加自定义停止逻辑
        }
        
        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="message">错误消息</param>
        protected void ShowError(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        
        /// <summary>
        /// 显示信息消息
        /// </summary>
        /// <param name="message">信息消息</param>
        protected void ShowInfo(string message)
        {
            MessageBox.Show(message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// 显示警告消息
        /// </summary>
        /// <param name="message">警告消息</param>
        protected void ShowWarning(string message)
        {
            MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        
        ///// <summary>
        ///// 获取Autofac容器中的服务
        ///// </summary>
        ///// <typeparam name="T">服务类型</typeparam>
        ///// <returns>服务实例</returns>
        //protected T GetService<T>()
        //{
        //    try
        //    {
        //        return Startup.AutoFacContainer.Resolve<T>();
        //    }
        //    catch
        //    {
        //        return default(T);
        //    }
        //}
        
        /// <summary>
        /// 保存插件数据
        /// </summary>
        /// <param name="key">数据键</param>
        /// <param name="value">数据值</param>
        protected void SaveData(string key, object value)
        {
            _pluginData[key] = value;
        }
        
        /// <summary>
        /// 获取插件数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">数据键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>数据值</returns>
        protected T GetData<T>(string key, T defaultValue = default(T))
        {
            if (_pluginData.ContainsKey(key) && _pluginData[key] is T)
            {
                return (T)_pluginData[key];
            }
            return defaultValue;
        }
        
        /// <summary>
        /// 保存插件设置到文件
        /// </summary>
        /// <param name="settings">设置字典</param>
        protected void SaveSettings(Dictionary<string, string> settings)
        {
            try
            {
                string settingsFile = Path.Combine(PluginDataDirectory, "settings.json");
                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(settingsFile, json);
            }
            catch (Exception ex)
            {
                ShowError($"保存插件设置时发生错误: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 从文件加载插件设置
        /// </summary>
        /// <returns>设置字典</returns>
        protected Dictionary<string, string> LoadSettings()
        {
            try
            {
                string settingsFile = Path.Combine(PluginDataDirectory, "settings.json");
                if (File.Exists(settingsFile))
                {
                    string json = File.ReadAllText(settingsFile);
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }
            }
            catch (Exception ex)
            {
                ShowError($"加载插件设置时发生错误: {ex.Message}");
            }
            return new Dictionary<string, string>();
        }
    }
}