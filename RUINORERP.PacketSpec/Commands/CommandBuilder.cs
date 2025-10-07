using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Serialization;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令构建器 - 此类型已过时，请使用 <see cref="CommandDataBuilder" /> 替代
    /// </summary>
    [Obsolete("This type is obsolete. Use CommandDataBuilder instead.")]
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
        /// 从数据包构建命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>当前构建器实例</returns>
        public CommandBuilder<TCommand> FromPacket(PacketModel packet)
        {
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


    }

    /// <summary>
    /// 命令构建器扩展方法 - 此类型已过时，请使用 <see cref="CommandDataBuilder" /> 替代
    /// </summary>
    [Obsolete("This type is obsolete. Use CommandDataBuilder instead.")]
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

    /// <summary>
    /// 静态命令构建器 - 此类型已过时，请使用 <see cref="CommandDataBuilder" /> 替代
    /// </summary>
    [Obsolete("This type is obsolete. Use CommandDataBuilder instead.")]
    public static class CommandBuilder
    {
        /// <summary>
        /// 创建基础命令对象
        /// </summary>
        /// <param name="id">命令标识符</param>
        /// <param name="payload">命令载荷</param>
        /// <returns>基础命令对象</returns>
        public static BaseCommand BuildBase(CommandId id, object payload = null)
        {
            return CommandDataBuilder.BuildBaseCommand(id, payload);
        }

        /// <summary>
        /// 创建强类型命令对象
        /// </summary>
        /// <typeparam name="TReq">请求类型，必须实现IRequest接口</typeparam>
        /// <typeparam name="TResp">响应类型，必须实现IResponse接口</typeparam>
        /// <param name="id">命令标识符</param>
        /// <param name="req">请求对象</param>
        /// <returns>强类型命令对象</returns>
        public static BaseCommand<TReq, TResp> BuildTyped<TReq, TResp>(CommandId id, TReq req)
            where TReq : class, IRequest
            where TResp : class, IResponse
        {
            return CommandDataBuilder.BuildCommand<TReq, TResp>(id, req);
        }

        /// <summary>
        /// 创建泛型命令对象
        /// </summary>
        /// <typeparam name="TPayload">载荷类型</typeparam>
        /// <param name="id">命令标识符</param>
        /// <param name="payload">命令载荷</param>
        /// <returns>泛型命令对象</returns>
        public static GenericCommand<TPayload> BuildGeneric<TPayload>(CommandId id, TPayload payload)
        {
            return CommandDataBuilder.BuildGenericCommand(id, payload);
        }
    }
}
