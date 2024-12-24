using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;

namespace RUINORERP.UI.ClientCmdService
{
    public interface IClientCommand
    {
        Task ExecuteAsync(CancellationToken cancellationToken, object parameters = null);
        //构建请求的数据包
        OriginalData BuildDataPacket(object request = null);
        //解析的数据包后再响应回应
        bool AnalyzeDataPacket(OriginalData gd);
    }


     
}
