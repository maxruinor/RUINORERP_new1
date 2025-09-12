using System;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.PacketSpec.Protocol
{
    /// <summary>
    /// 协议处理器 - 负责指令的解析和验证
    /// </summary>
    public class ProtocolProcessor
    {
        /// <summary>
        /// 检查指令类型并返回来源类型
        /// </summary>
        public static (PackageSourceType SourceType, string Message) CheckCommandType(uint command)
        {
            string message = string.Empty;
            PackageSourceType sourceType;

            // 尝试转换为服务器指令枚举
            if (Enum.IsDefined(typeof(ServerCommand), command))
            {
                ServerCommand serverCmd = (ServerCommand)command;
                sourceType = PackageSourceType.Server;
                message = $"Server:{serverCmd}|{command:X} ";
            }
            // 尝试转换为客户端指令枚举
            else if (Enum.IsDefined(typeof(ClientCommand), command))
            {
                ClientCommand clientCmd = (ClientCommand)command;
                sourceType = PackageSourceType.Client;
                message = $"Client:{clientCmd}|{command:X} ";
            }
            else
            {
                sourceType = PackageSourceType.Server;
                message = $"Unknown:{command:X} ";
            }

            return (sourceType, message);
        }

        /// <summary>
        /// 从原始数据中组合完整指令
        /// </summary>
        private static uint CombineCommand(OriginalData data)
        {
            uint command = data.Cmd;

            // 如果有Two数据，组合成完整指令
            if (data.Two != null && data.Two.Length >= 2)
            {
                command <<= 8;
                command |= data.Two[1];
                command <<= 8;
                command |= data.Two[0];
            }

            return command;
        }

        /// <summary>
        /// 从原始数据中提取客户端指令
        /// </summary>
        public static ClientCommand GetClientCommand(OriginalData data)
        {
            uint command = CombineCommand(data);
            return Enum.IsDefined(typeof(ClientCommand), command) 
                ? (ClientCommand)command 
                : ClientCommand.None;
        }

        /// <summary>
        /// 从原始数据中提取服务器指令
        /// </summary>
        public static ServerCommand GetServerCommand(OriginalData data)
        {
            uint command = CombineCommand(data);
            return Enum.IsDefined(typeof(ServerCommand), command) 
                ? (ServerCommand)command 
                : ServerCommand.Unknown;
        }

        /// <summary>
        /// 获取服务器主指令
        /// </summary>
        public static ServerMainCommand GetServerMainCommand(OriginalData data)
        {
            return Enum.IsDefined(typeof(ServerMainCommand), data.Cmd)
                ? (ServerMainCommand)data.Cmd
                : ServerMainCommand.Special3;
        }

        /// <summary>
        /// 获取服务器子指令
        /// </summary>
        public static uint GetServerSubCommand(OriginalData data)
        {
            uint subCommand = 0;

            if (data.Two != null && data.Two.Length >= 2)
            {
                subCommand <<= 8;
                subCommand |= data.Two[1];
                subCommand <<= 8;
                subCommand |= data.Two[0];
            }

            return subCommand;
        }

        /// <summary>
        /// 验证指令是否有效
        /// </summary>
        public static bool IsValidCommand(uint command)
        {
            return Enum.IsDefined(typeof(ClientCommand), command) ||
                   Enum.IsDefined(typeof(ServerCommand), command);
        }

        /// <summary>
        /// 获取指令描述信息
        /// </summary>
        public static string GetCommandDescription(uint command)
        {
            if (Enum.IsDefined(typeof(ClientCommand), command))
            {
                return $"Client: {(ClientCommand)command} (0x{command:X})";
            }
            else if (Enum.IsDefined(typeof(ServerCommand), command))
            {
                return $"Server: {(ServerCommand)command} (0x{command:X})";
            }
            else
            {
                return $"Unknown: 0x{command:X}";
            }
        }
    }
}