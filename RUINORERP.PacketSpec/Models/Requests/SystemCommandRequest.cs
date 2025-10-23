using System;

using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 系统指令类型枚举
    /// </summary>
    public enum SystemCommandType
    {
        /// <summary>
        /// 电脑状态查询
        /// </summary>
        ComputerStatus = 1,
        
        /// <summary>
        /// 关闭电脑
        /// </summary>
        ShutdownComputer = 2,
        
        /// <summary>
        /// 退出系统
        /// </summary>
        ExitSystem = 3,
        
        /// <summary>
        /// 强制用户下线
        /// </summary>
        ForceLogout = 4
    }

    /// <summary>
    /// 统一系统指令请求
    /// </summary>
    [Serializable]
    public class SystemCommandRequest : RequestBase
    {
        /// <summary>
        /// 系统指令类型
        /// </summary>
        public SystemCommandType CommandType { get; set; }

        /// <summary>
        /// 目标用户ID
        /// </summary>
        public string TargetUserId { get; set; }

        /// <summary>
        /// 关闭类型（关机、重启等）
        /// </summary>
        public string ShutdownType { get; set; } = "Shutdown";

        /// <summary>
        /// 延迟时间（秒）
        /// </summary>
        public int DelaySeconds { get; set; } = 0;

        /// <summary>
        /// 管理员ID
        /// </summary>
        public string AdminUserId { get; set; }

        /// <summary>
        /// 强制下线原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 管理员备注
        /// </summary>
        public string AdminRemark { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// 创建电脑状态查询请求
        /// </summary>
        public static SystemCommandRequest CreateComputerStatusRequest(string targetUserId, string requestType = "Status")
        {
            return new SystemCommandRequest
            {
                CommandType = SystemCommandType.ComputerStatus,
                TargetUserId = targetUserId,
                RequestType = requestType,
                RequestId = IdGenerator.GenerateRequestId(SystemCommands.ComputerStatus)
            };
        }

        /// <summary>
        /// 创建关闭电脑请求
        /// </summary>
        public static SystemCommandRequest CreateShutdownRequest(string targetUserId, string shutdownType = "Shutdown", int delaySeconds = 0, string adminRemark = "")
        {
            return new SystemCommandRequest
            {
                CommandType = SystemCommandType.ShutdownComputer,
                TargetUserId = targetUserId,
                ShutdownType = shutdownType,
                DelaySeconds = delaySeconds,
                AdminRemark = adminRemark,
                RequestId = IdGenerator.GenerateRequestId(SystemCommands.ShutdownComputer)
            };
        }

        /// <summary>
        /// 创建退出系统请求
        /// </summary>
        public static SystemCommandRequest CreateExitSystemRequest(string targetUserId, int delaySeconds = 0, string adminRemark = "")
        {
            return new SystemCommandRequest
            {
                CommandType = SystemCommandType.ExitSystem,
                TargetUserId = targetUserId,
                ShutdownType = "Logoff",
                DelaySeconds = delaySeconds,
                AdminRemark = adminRemark,
                RequestId = IdGenerator.GenerateRequestId(SystemCommands.ExitSystem)
            };
        }

        /// <summary>
        /// 创建强制用户下线请求
        /// </summary>
        public static SystemCommandRequest CreateForceLogoutRequest(string targetUserId, string adminUserId, string reason = "管理员强制下线", string adminRemark = "")
        {
            return new SystemCommandRequest
            {
                CommandType = SystemCommandType.ForceLogout,
                TargetUserId = targetUserId,
                AdminUserId = adminUserId,
                Reason = reason,
                AdminRemark = adminRemark,
                RequestId = IdGenerator.GenerateRequestId(AuthenticationCommands.ForceLogout)
            };
        }

        /// <summary>
        /// 验证请求有效性
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(TargetUserId))
                return false;

            switch (CommandType)
            {
                case SystemCommandType.ComputerStatus:
                    return true;
                    
                case SystemCommandType.ShutdownComputer:
                case SystemCommandType.ExitSystem:
                    return ShutdownType == "Shutdown" || ShutdownType == "Restart" || ShutdownType == "Logoff";
                    
                case SystemCommandType.ForceLogout:
                    return !string.IsNullOrEmpty(AdminUserId);
                    
                default:
                    return false;
            }
        }
    }
}
