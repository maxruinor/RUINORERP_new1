using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 插件与主程序之间的默认通信通道实现
    /// </summary>
    public class PluginCommunicationChannel : IPluginCommunicationChannel
    {
        // 定义事件，让主程序可以订阅
        public event EventHandler<PluginDataEventArgs> DataReceivedFromPlugin;
        public event EventHandler<PluginDataRequestEventArgs> DataRequestedFromHost;
        public event EventHandler<PluginServiceInvokeEventArgs> ServiceInvokedByPlugin;

        /// <summary>
        /// 发送数据到主程序
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="data">要发送的数据</param>
        /// <returns>是否发送成功</returns>
        public bool SendDataToHost(string pluginName, object data)
        {
            try
            {
                DataReceivedFromPlugin?.Invoke(this, new PluginDataEventArgs(pluginName, data));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"插件 {pluginName} 发送数据到主程序时发生错误: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从主程序请求数据
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="request">请求参数</param>
        /// <returns>返回的数据</returns>
        public object RequestDataFromHost(string pluginName, object request)
        {
            try
            {
                var args = new PluginDataRequestEventArgs(pluginName, request);
                DataRequestedFromHost?.Invoke(this, args);
                return args.Response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"插件 {pluginName} 从主程序请求数据时发生错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 调用主程序服务
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="service">服务名称</param>
        /// <param name="parameters">服务参数</param>
        /// <returns>服务调用结果</returns>
        public object InvokeHostService(string pluginName, string service, object parameters)
        {
            try
            {
                var args = new PluginServiceInvokeEventArgs(pluginName, service, parameters);
                ServiceInvokedByPlugin?.Invoke(this, args);
                return args.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"插件 {pluginName} 调用主程序服务 {service} 时发生错误: {ex.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// 插件发送数据事件参数
    /// </summary>
    public class PluginDataEventArgs : EventArgs
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="data">数据内容</param>
        public PluginDataEventArgs(string pluginName, object data)
        {
            PluginName = pluginName;
            Data = data;
        }
    }

    /// <summary>
    /// 插件请求数据事件参数
    /// </summary>
    public class PluginDataRequestEventArgs : EventArgs
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public object Request { get; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public object Response { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="request">请求参数</param>
        public PluginDataRequestEventArgs(string pluginName, object request)
        {
            PluginName = pluginName;
            Request = request;
        }
    }

    /// <summary>
    /// 插件调用服务事件参数
    /// </summary>
    public class PluginServiceInvokeEventArgs : EventArgs
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { get; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Service { get; }

        /// <summary>
        /// 服务参数
        /// </summary>
        public object Parameters { get; }

        /// <summary>
        /// 服务调用结果
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pluginName">插件名称</param>
        /// <param name="service">服务名称</param>
        /// <param name="parameters">服务参数</param>
        public PluginServiceInvokeEventArgs(string pluginName, string service, object parameters)
        {
            PluginName = pluginName;
            Service = service;
            Parameters = parameters;
        }
    }
}