using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using System;

namespace RUINORERP.UI.Network
{
/// <summary>
    /// 命令接收事件参数 - 网络事件数据载体
    /// 
    /// 事件数据流：
    /// 1. BizPipelineFilter 接收并解析服务器数据
    /// 2. 解析命令ID和业务数据
    /// 3. 创建 CommandReceivedEventArgs
    /// 4. 通过 ClientEventManager 触发事件
    /// 5. ClientCommunicationService 接收事件并处理
    /// 
    /// 📋 核心职责：
    /// - 封装命令ID
    /// - 传输业务数据
    /// - 提供事件参数验证
    /// - 支持事件数据序列化
    /// 
    /// 🔗 与架构集成：
    /// - 由 BizPipelineFilter 创建
    /// - 通过 ClientEventManager.CommandReceived 事件传递
    /// - 被 ClientCommandProcessor 消费
    /// - 支持事件订阅者的数据访问
    /// 
    /// 📊 数据内容：
    /// - CommandId: 命令标识符
    /// - Data: 业务数据字节数组
    /// - Timestamp: 事件时间戳（可选）
    /// </summary>
    public class CommandReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 命令ID
        /// </summary>
        public CommandId CommandId { get; }

        /// <summary>
        /// 命令数据
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">命令数据</param>
        public CommandReceivedEventArgs(CommandId commandId, object data)
        {
            CommandId = commandId;
            Data = data;
        }