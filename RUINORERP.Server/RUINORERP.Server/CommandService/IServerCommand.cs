using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;
using RUINORERP.Server.ServerSession;
using TransInstruction;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// 命令和处理器是一对。一个定义 。一个处理。
    /// 接口定义了服务器指令的基本结构和行为，它包含了 Execute 和 ExecuteAsync 方法，用于执行指令
    /// 这个接口的主要目的是提供一个统一的方式来定义和执行不同类型的指令。
    /// 用于定义指令的基本结构和行为
    /// 实体参数
    /// </summary>
    public interface IServerCommand
    {
        // void Execute();


        /// <summary>
        /// cancellationToken.IsCancellationRequested时不会执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken cancellationToken);
        bool AnalyzeDataPacket(OriginalData gd, SessionforBiz FromSession);
    }
    //public abstract class Command
    //{
    //    public abstract void Execute();
    //}





}
