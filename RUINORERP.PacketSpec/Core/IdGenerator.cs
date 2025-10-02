using RUINORERP.Model;
using System;


namespace RUINORERP.PacketSpec.Core
{
    /// <summary>
    /// ID生成器 - 提供统一的ID生成服务
    /// </summary>
    public static class IdGenerator
    {
        /// <summary>
        /// 生成命令ID
        /// </summary>
        /// <param name="commandTypeName">命令类型名称</param>
        /// <returns>命令ID</returns>
        public static string GenerateCommandId(string commandTypeName)
        {
            return $"{commandTypeName}_{Ulid.NewUlid()}";
        }

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
