using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.PacketSpec.Protocol
{
    /// <summary>
    /// 指令协议定义和工具方法
    /// </summary>
    public static class CommandProtocol
    {
        /// <summary>
        /// 协议头大小（字节）
        /// </summary>
        public const int HeaderSize = 12;

        /// <summary>
        /// 最大数据包大小（64KB）
        /// </summary>
        public const int MaxPacketSize = 1024 * 64;

        /// <summary>
        /// 心跳包间隔（毫秒）
        /// </summary>
        public const int HeartbeatInterval = 30000;

        /// <summary>
        /// 编码指令（主指令 + 子指令）
        /// </summary>
        public static uint EncodeCommand(ClientCommand mainCommand, ushort subCommand = 0)
        {
            return ((uint)mainCommand << 16) | subCommand;
        }

        /// <summary>
        /// 编码指令（主指令 + 子指令）
        /// </summary>
        public static uint EncodeCommand(ServerCommand mainCommand, ushort subCommand = 0)
        {
            return ((uint)mainCommand << 16) | subCommand;
        }

        /// <summary>
        /// 解码指令
        /// </summary>
        public static (ClientCommand mainCommand, ushort subCommand) DecodeClientCommand(uint encodedCommand)
        {
            return ((ClientCommand)(encodedCommand >> 16), (ushort)(encodedCommand & 0xFFFF));
        }

        /// <summary>
        /// 解码指令
        /// </summary>
        public static (ServerCommand mainCommand, ushort subCommand) DecodeServerCommand(uint encodedCommand)
        {
            return ((ServerCommand)(encodedCommand >> 16), (ushort)(encodedCommand & 0xFFFF));
        }

        /// <summary>
        /// 检查是否为心跳指令
        /// </summary>
        public static bool IsHeartbeatCommand(uint command)
        {
            var (mainCommand, _) = DecodeClientCommand(command);
            return mainCommand == ClientCommand.Heartbeat;
        }

        /// <summary>
        /// 获取指令类型（客户端/服务端）
        /// </summary>
        public static PackageSourceType GetCommandSource(uint command)
        {
            var mainPart = command >> 16;
            
            // 根据数值范围判断指令来源
            if (mainPart >= 0x90000)
            {
                return PackageSourceType.Client;
            }
            else if (mainPart < 0x100)
            {
                return PackageSourceType.Server;
            }

            // 默认根据数值大小判断
            return mainPart > 0x8000 ? PackageSourceType.Client : PackageSourceType.Server;
        }

        /// <summary>
        /// 验证指令有效性
        /// </summary>
        public static bool IsValidCommand(uint command, PackageSourceType expectedSource)
        {
            if (command == 0) return false;

            var actualSource = GetCommandSource(command);
            return actualSource == expectedSource;
        }

        /// <summary>
        /// 生成心跳指令
        /// </summary>
        public static uint GenerateHeartbeatCommand()
        {
            return EncodeCommand(ClientCommand.Heartbeat);
        }

        /// <summary>
        /// 生成心跳响应指令
        /// </summary>
        public static uint GenerateHeartbeatResponseCommand()
        {
            return EncodeCommand(ServerCommand.HeartbeatResponse);
        }
    }
}