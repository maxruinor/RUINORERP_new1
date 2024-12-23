using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TransInstruction.CommandService
{
    /// <summary>
    /// 命令和处理器是一对。一个定义 。一个处理。
    /// </summary>
    public interface IServerCommand
    {
        void Execute();
        Task ExecuteAsync(CancellationToken cancellationToken);
        void Undo();
    }
    //public abstract class Command
    //{
    //    public abstract void Execute();
    //}





}
