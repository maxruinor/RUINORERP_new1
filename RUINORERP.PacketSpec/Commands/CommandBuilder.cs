using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Protocol;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令构建器 - 提供流畅的API构建命令对象
    /// 使用建造者模式简化命令创建过程
    /// </summary>
    public class CommandBuilder<TCommand> where TCommand : BaseCommand, new()
    {
        private readonly TCommand _command;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private CommandBuilder()
        {
            _command = new TCommand();
        }

        /// <summary>
        /// 创建新的构建器实例
        /// </summary>
        /// <returns>构建器实例</returns>
        public static CommandBuilder<TCommand> Create()
        {
            return new CommandBuilder<TCommand>();
        }

        /// <summary>
        /// 设置命令方向
        /// </summary>
        /// <param name="direction">方向</param>
        /// <returns>当前构建器实例</returns>
        public CommandBuilder<TCommand> WithDirection(PacketDirection direction)
        {
            _command.Direction = direction;
            return this;
        }

        /// <summary>
        /// 设置命令优先级
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <returns>当前构建器实例</returns>
        public CommandBuilder<TCommand> WithPriority(CommandPriority priority)
        {
            _command.Priority = priority;
            return this;
        }

  

        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>当前构建器实例</returns>
        public CommandBuilder<TCommand> WithTimeout(int timeoutMs)
        {
            _command.TimeoutMs = timeoutMs;
            return this;
        }

 
 

        ///// <summary>
        ///// 设置JSON数据
        ///// </summary>
        ///// <typeparam name="T">数据类型</typeparam>
        ///// <param name="data">数据对象</param>
        ///// <returns>当前构建器实例</returns>
        //public CommandBuilder<TCommand> WithJsonData<T>(T data)
        //{
        //    if (_command.Packet == null)
        //    {
        //        _command.Packet = new PacketModel();
        //    }
            
        //    _command.Packet.SetJsonData(data);
        //    return this;
        //}

        /// <summary>
        /// 从数据包构建命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>当前构建器实例</returns>
        public CommandBuilder<TCommand> FromPacket(PacketModel packet)
        {
            //_command.Packet = packet;
            
            //// 从数据包中提取通用属性
            //if (packet.Extensions.TryGetValue("RequestId", out var requestId))
            //{
            //    _command.RequestId = requestId?.ToString();
            //}
            
            //if (packet.Extensions.TryGetValue("Timeout", out var timeout))
            //{
            //    _command.TimeoutMs = Convert.ToInt32(timeout);
            //}
            
            //_command.SessionId = packet.SessionId;
            
            //if (packet.Extensions.TryGetValue("ClientId", out var clientId))
            //{
            //    _command.ClientId = clientId?.ToString();
            //}
            
            return this;
        }

        /// <summary>
        /// 构建最终的命令对象
        /// </summary>
        /// <returns>构建完成的命令实例</returns>
        public TCommand Build()
        {
            return _command;
        }

        /// <summary>
        /// 构建并执行命令
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        public async Task<ResponseBase> BuildAndExecuteAsync(CancellationToken cancellationToken = default)
        {
            var command = Build();
            return await command.ExecuteAsync(cancellationToken);
        }
    }

    /// <summary>
    /// 命令构建器扩展方法
    /// </summary>
    public static class CommandBuilderExtensions
    {
        /// <summary>
        /// 设置高优先级
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <returns>构建器实例</returns>
        public static CommandBuilder<TCommand> WithHighPriority<TCommand>(this CommandBuilder<TCommand> builder) 
            where TCommand : BaseCommand, new()
        {
            return builder.WithPriority(CommandPriority.High);
        }

        /// <summary>
        /// 设置正常优先级
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <returns>构建器实例</returns>
        public static CommandBuilder<TCommand> WithNormalPriority<TCommand>(this CommandBuilder<TCommand> builder) 
            where TCommand : BaseCommand, new()
        {
            return builder.WithPriority(CommandPriority.Normal);
        }

        /// <summary>
        /// 设置低优先级
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <returns>构建器实例</returns>
        public static CommandBuilder<TCommand> WithLowPriority<TCommand>(this CommandBuilder<TCommand> builder) 
            where TCommand : BaseCommand, new()
        {
            return builder.WithPriority(CommandPriority.Low);
        }

        /// <summary>
        /// 设置客户端到服务器方向
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <returns>构建器实例</returns>
        public static CommandBuilder<TCommand> FromClientToServer<TCommand>(this CommandBuilder<TCommand> builder) 
            where TCommand : BaseCommand, new()
        {
            return builder.WithDirection(PacketDirection.ClientToServer);
        }

        /// <summary>
        /// 设置服务器到客户端方向
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <param name="builder">构建器实例</param>
        /// <returns>构建器实例</returns>
        public static CommandBuilder<TCommand> FromServerToClient<TCommand>(this CommandBuilder<TCommand> builder) 
            where TCommand : BaseCommand, new()
        {
            return builder.WithDirection(PacketDirection.ServerToClient);
        }
    }
}
