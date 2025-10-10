using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 统一命令创建服务接口 - 提供标准化的命令创建功能
    /// 集中处理所有命令创建逻辑，避免代码重复
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
        /// 从数据包中提取类型化命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>提取的命令对象，不是类型化命令时返回null</returns>
        ICommand ExtractTypedCommand(PacketModel packet);

        /// <summary>
        /// 根据命令ID创建空命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>空命令实例</returns>
        ICommand CreateEmptyCommand(CommandId commandId);

        /// <summary>
        /// 创建泛型命令作为后备方案
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>泛型命令对象</returns>
        ICommand CreateGenericCommand(PacketModel packet);

        /// <summary>
        /// 根据类型创建命令实例
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="packet">数据包</param>
        /// <returns>创建的命令对象</returns>
        ICommand CreateCommandByType(Type commandType, PacketModel packet);

        /// <summary>
        /// 从JSON字符串创建命令
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>创建的命令对象</returns>
        ICommand CreateCommandFromJson(string json, string typeName);

        /// <summary>
        /// 初始化命令对象的基本属性
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="packet">源数据包</param>
        void InitializeCommandProperties(ICommand command, PacketModel packet);
    }
}