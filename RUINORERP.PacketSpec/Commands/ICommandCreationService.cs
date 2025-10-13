using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 优化后的统一命令创建服务接口 - 提供标准化的命令创建功能
    /// 精简了方法数量，保留核心功能，提高接口的简洁性和可维护性
    /// </summary>
    public interface ICommandCreationService 
    {
        /// <summary>
        /// 从数据包创建命令 - 主入口方法
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>创建的命令对象，失败时返回null</returns>
        ICommand CreateCommand(PacketModel packet);

        /// <summary>
        /// 从字节数组和类型名称创建命令
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>创建的命令对象</returns>
        ICommand CreateCommandFromBytes(byte[] data, string typeName);

        /// <summary>
        /// 从字节数组和类型对象创建命令
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="type">类型对象</param>
        /// <returns>创建的命令对象</returns>
        ICommand CreateCommandFromBytes(byte[] data, Type type);

        /// <summary>
        /// 根据命令ID创建空命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>空命令实例</returns>
        ICommand CreateEmptyCommand(CommandId commandId);

        /// <summary>
        /// 将命令对象转换为数据包模型
        /// 用于发送命令时构建网络传输数据包
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>数据包模型</returns>
        PacketModel CreatePacket(ICommand command);

        /// <summary>
        /// 注册自定义命令创建器
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="creator">创建器函数</param>
        void RegisterCommandCreator(CommandId commandCode, Func<PacketModel, ICommand> creator);
    }
}
