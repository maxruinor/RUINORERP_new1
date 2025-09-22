//------------------------------------------------------------------------------
// <copyright file="ERPServerOptions.cs" company="RUINORERP">
//     Copyright (c) RUINORERP.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
// 服务器配置选项类文件
// 定义SuperSocket服务器的配置结构和验证逻辑
// 支持从appsettings.json配置文件中读取配置信息
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;

namespace RUINORERP.Server.Network.Core
{
    /// <summary>
    /// 监听器选项类，对应appsettings.json中的serverOptions.listeners配置
    /// 定义SuperSocket服务器监听器的基本配置参数
    /// </summary>
    public class ListenerOptions
    {
        /// <summary>
        /// 监听IP地址，Any表示所有IPv4地址
        /// </summary>
        public string Ip { get; set; } = "Any";

        /// <summary>
        /// 监听端口号
        /// </summary>
        public int Port { get; set; } = 3003;

        /// <summary>
        /// 连接等待队列的最大长度
        /// </summary>
        public int Backlog { get; set; } = 100;

        /// <summary>
        /// IP地址属性（用于JSON序列化）
        /// 注意：与appsettings.json中的小写"ip"命名保持一致
        /// </summary>
        [JsonProperty(PropertyName = "ip")]
        public string IpAddress
        {
            get { return Ip; }
            set { Ip = value; }
        }

        /// <summary>
        /// 端口号属性（用于JSON序列化）
        /// 注意：与appsettings.json中的小写"port"命名保持一致
        /// </summary>
        [JsonProperty(PropertyName = "port")]
        public int PortNumber
        {
            get { return Port; }
            set { Port = value; }
        }
    }

    /// <summary>
    /// ERP服务器配置选项类
    /// 用于从appsettings.json配置文件中读取SuperSocket服务器的完整配置
    /// 对应配置节点："serverOptions"或"ERPServer"
    /// </summary>
    public class ERPServerOptions
    {
        /// <summary>
        /// 服务器监听器列表
        /// 包含服务器需要监听的所有端点配置
        /// 默认包含一个监听所有IPv4地址的监听器
        /// </summary>
        public List<ListenerOptions> Listeners { get; set; } = new List<ListenerOptions>
        {
            new ListenerOptions() // 确保至少有一个默认监听器
        };

        /// <summary>
        /// 验证服务器配置的有效性
        /// 确保监听器列表不为空且最大连接数为正数
        /// </summary>
        public void Validate()
        {
            if (Listeners.Count == 0)
                Listeners.Add(new ListenerOptions());

            if (MaxConnectionCount <= 0)
                MaxConnectionCount = 1000;
        }

        /// <summary>
        /// 服务器名称
        /// 标识服务器实例的唯一名称
        /// 默认值为"RUINORERP.Network.Server"
        /// </summary>
        public string Name { get; set; } = "RUINORERP.Network.Server";

        /// <summary>
        /// 最大连接数
        /// 限制服务器同时可以处理的最大连接数量
        /// 默认值为500
        /// </summary>
        public int MaxConnectionCount { get; set; } = 500;

        /// <summary>
        /// 最大包长度
        /// 单个数据包的最大允许长度
        /// 默认值为1MB
        /// </summary>
        public int MaxPackageLength { get; set; } = 1024 * 1024; // 1MB

        /// <summary>
        /// 接收缓冲区大小
        /// 服务器接收数据的缓冲区大小
        /// 默认值为4096字节
        /// </summary>
        public int ReceiveBufferSize { get; set; } = 4096;

        /// <summary>
        /// 发送缓冲区大小
        /// 服务器发送数据的缓冲区大小
        /// 默认值为4096字节
        /// </summary>
        public int SendBufferSize { get; set; } = 4096;

        /// <summary>
        /// 接收超时时间
        /// 接收数据的最大等待时间
        /// 默认值为120000毫秒（2分钟）
        /// </summary>
        public int ReceiveTimeout { get; set; } = 120000; // 2分钟

        /// <summary>
        /// 发送超时时间
        /// 发送数据的最大等待时间
        /// 默认值为60000毫秒（1分钟）
        /// </summary>
        public int SendTimeout { get; set; } = 60000; // 1分钟

        /// <summary>
        /// 安全模式
        /// 指定服务器使用的安全协议
        /// 默认值为"Tls12"
        /// </summary>
        public string SecurityMode { get; set; } = "Tls12";
    }
}