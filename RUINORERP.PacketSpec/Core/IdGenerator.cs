using RUINORERP.Model;
using RUINORERP.PacketSpec.Commands;
using System;


namespace RUINORERP.PacketSpec.Core
{
    /// <summary>
    /// ID生成器 - 提供统一的ID生成服务
    /// </summary>
    public static class IdGenerator
    {

        /// <summary>
        /// 生成新的请求ID
        /// 格式: {命令名称}_{时间戳}_{ULID}
        /// 确保请求ID全局唯一、可追踪、可读
        /// </summary>
        /// <param name="cmd">命令ID</param>
        /// <returns>格式化的请求ID</returns>
        public static string GenerateRequestId(CommandId cmd) => 
            $"{cmd.Name}_{DateTime.Now:HHmmssfff}_{Ulid.NewUlid()}";

        /// <summary>
        /// 生成新的请求ID
        /// 格式: {命令名称}_{时间戳}_{ULID}
        /// 确保请求ID全局唯一、可追踪、可读
        /// </summary>
        /// <param name="cmd">命令ID</param>
        /// <returns>格式化的请求ID</returns>
        public static string GenerateRequestId() =>
            $"{DateTime.Now:HHmmssfff}_{Ulid.NewUlid()}";

        /// <summary>
        /// 生成处理器ID
        /// </summary>
        /// <param name="handlerTypeName">处理器类型名称</param>
        /// <returns>处理器ID</returns>
        public static string GenerateHandlerId(string handlerTypeName)
        {
            return $"{handlerTypeName}_{Ulid.NewUlid()}";
        }

        /// <summary>
        /// 生成请求ID
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>请求ID</returns>
        public static string GenerateRequestId(string commandName)
        {
            return $"{commandName}_{DateTime.Now:HHmmssfff}";
        }
        /// <summary>
        /// 生成响应ID
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>响应ID</returns>
        public static string GenerateResponseId(string requestId)
        {
            return $"RESP_{requestId}_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
        public static string GenerateRequestId(Type type )
        {
            return $"{type.Name}_{DateTime.Now:HHmmssfff}";
        }

        /// <summary>
        /// 生成数据包ID
        /// </summary>
        /// <param name="category">命令类别</param>
        /// <returns>数据包ID</returns>
        public static string GeneratePacketId(string category)
        {
            return $"PKT_{category}_{DateTime.Now:yyyyMMddHHmmssfff}_{Ulid.NewUlid()}";
        }

        /// <summary>
        /// 生成通用ID
        /// </summary>
        /// <returns>通用ID</returns>
        public static string GenerateId()
        {
            return Ulid.NewUlid().ToString();
        }
    }
}
