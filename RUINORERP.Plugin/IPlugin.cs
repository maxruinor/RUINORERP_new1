using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 插件接口，所有插件必须实现此接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 插件描述
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// 插件版本
        /// </summary>
        Version Version { get; }
        
        /// <summary>
        /// 插件状态
        /// </summary>
        PluginState State { get; }
        
        /// <summary>
        /// 插件权限标识
        /// </summary>
        string PermissionKey { get; }
        
        /// <summary>
        /// 初始化插件
        /// </summary>
        /// <returns>是否初始化成功</returns>
        bool Initialize();
        
        /// <summary>
        /// 启动插件
        /// </summary>
        /// <returns>是否启动成功</returns>
        bool Start();
        
        /// <summary>
        /// 停止插件
        /// </summary>
        void Stop();
        
        /// <summary>
        /// 执行插件功能
        /// </summary>
        void Execute();
        
        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <returns>插件菜单项</returns>
        ToolStripMenuItem GetMenuItem();
        
        /// <summary>
        /// 检查当前用户是否有权限使用此插件
        /// </summary>
        /// <returns>是否有权限</returns>
        bool HasPermission();
        
        /// <summary>
        /// 设置权限检查委托
        /// </summary>
        /// <param name="permissionChecker">权限检查委托</param>
        void SetPermissionChecker(Func<string, bool> permissionChecker);
        
        /// <summary>
        /// 设置插件与主程序的通信通道
        /// </summary>
        /// <param name="communicationChannel">通信通道实例</param>
        void SetCommunicationChannel(IPluginCommunicationChannel communicationChannel);
    }
    
    /// <summary>
    /// 插件与主程序之间的通信通道接口
    /// </summary>
    public interface IPluginCommunicationChannel
    {
        /// <summary>
        /// 发送数据到主程序
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="data">要发送的数据</param>
        /// <returns>是否发送成功</returns>
        bool SendDataToHost(string pluginName, object data);
        
        /// <summary>
        /// 从主程序请求数据
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="request">请求参数</param>
        /// <returns>返回的数据</returns>
        object RequestDataFromHost(string pluginName, object request);
        
        /// <summary>
        /// 调用主程序服务
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="service">服务名称</param>
        /// <param name="parameters">服务参数</param>
        /// <returns>服务调用结果</returns>
        object InvokeHostService(string pluginName, string service, object parameters);
    }
    
    /// <summary>
    /// 插件状态枚举
    /// </summary>
    public enum PluginState
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        Uninitialized,
        
        /// <summary>
        /// 已初始化
        /// </summary>
        Initialized,
        
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        
        /// <summary>
        /// 已停止
        /// </summary>
        Stopped,
        
        /// <summary>
        /// 错误状态
        /// </summary>
        Error
    }
}