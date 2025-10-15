/*****************************************************************************************
 * 【过时作废文件】OBSOLETE - DEPRECATED - DO NOT USE
 * 此文件已被废弃，不再维护和使用
 * 原因：ClientCmdService目录下的所有文件都已过时，实际已排除在项目外
 * 替代方案：请使用新的命令处理机制
 * 创建日期：系统自动标识
 *****************************************************************************************/

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
    public class ReceiveCacheCommand : IClientCommand
    {
        public string productName { get; set; }
        public CmdOperation OperationType { get ; set; }
        public OriginalData DataPacket { get; set; }

        public bool AnalyzeDataPacket(OriginalData gd)
        {
            throw new NotImplementedException();
        }

        public void BuildDataPacket(object request = null)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters)
        {
            if (parameters == null)
            {
                await Task.Run(
                   () =>
                   //只是一行做对。为了编译通过
                   productName == "1"
                   //ProductService.AddProduct(productName, price)
                   ,

                   cancellationToken
                   );
            }
            else
            {
                throw new ArgumentException("parameters is required.");
            }

        }

  
    }

}
