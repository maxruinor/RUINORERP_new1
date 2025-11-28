using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 队列命令模型，支持单向命令和带响应命令
    /// </summary>
    public class ClientQueuedCommand
    {
        public CommandId CommandId { get; set; }
        public object Data { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public Type DataType { get; set; }
        public TaskCompletionSource<bool> CompletionSource { get; set; } // 用于单向命令
        public TaskCompletionSource<PacketModel> ResponseCompletionSource { get; set; } // 用于带响应命令
        public int TimeoutMs { get; set; } = 30000;
        public bool IsResponseCommand { get; set; } // 标识是否是带响应的命令
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 创建时间，用于超时清理
    }
}
