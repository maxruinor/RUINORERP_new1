using RUINORERP.PacketSpec.Models.Core;
using System;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 命令接收事件参数
    /// 包含接收到的命令ID和数据
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
    }
}