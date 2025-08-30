using Autofac;
using RUINORERP.UI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 插件基类，提供插件通用功能
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
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
        /// 插件菜单项目
        /// </summary>
        protected ToolStripMenuItem PluginMenuItem { get; private set; }
        
        /// <summary>
        /// 初始化插件
        /// </summary>
        public virtual void Initialize()
        {
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
        /// 菜单项点击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPluginMenuItemClick(object sender, EventArgs e)
        {
            try
            {
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
        
        /// <summary>
        /// 获取Autofac容器中的服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        protected T GetService<T>()
        {
            try
            {
                return Startup.AutoFacContainer.Resolve<T>();
            }
            catch
            {
                return default;
            }
        }
    }
}