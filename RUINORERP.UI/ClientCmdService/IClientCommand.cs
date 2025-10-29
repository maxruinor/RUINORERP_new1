/*****************************************************************************************
 * 【过时作废文件】OBSOLETE - DEPRECATED - DO NOT USE
 * 此文件已被废弃，不再维护和使用
 * 原因：ClientCmdService目录下的所有文件都已过时，实际已排除在项目外
 * 替代方案：请使用新的命令处理机制
 * 创建日期：系统自动标识
 *****************************************************************************************/

using RUINORERP.Global;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.ClientCmdService
{

    /// <summary>
    /// 客户端的指令 可能是发送功能，也有可能是接收功能
    /// </summary>
    [Obsolete("此接口已过时，不再使用")]
    public interface IClientCommand : IExcludeFromRegistration
    {
        CommandDirection OperationType { get; set; }
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
