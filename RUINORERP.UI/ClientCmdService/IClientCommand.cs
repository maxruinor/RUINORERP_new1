using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.Enums;

namespace RUINORERP.UI.ClientCmdService
{

    /// <summary>
    /// 客户端的指令 可能是发送功能，也有可能是接收功能
    /// </summary>
    public interface IClientCommand : IExcludeFromRegistration
    {
        CmdOperation OperationType { get; set; }
        //执行要操作的指令
        Task ExecuteAsync(CancellationToken cancellationToken, object parameters = null);

        //构建请求的数据包
        void BuildDataPacket(object request = null);

        //解析的数据包后再响应回应
        bool AnalyzeDataPacket(OriginalData gd);

        /// <summary>
        /// 不管是发送还是接收都有对应要操作的对象：数据包
        /// </summary>
        OriginalData DataPacket { get; set; }
    }



}
