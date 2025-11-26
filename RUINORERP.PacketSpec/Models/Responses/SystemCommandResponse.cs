using System;
using System.Collections.Generic;

using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands;
namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 统一系统指令响应
    /// </summary>
    [Serializable]
    public class SystemCommandResponse : ResponseBase
    {
        /// <summary>
        /// 系统指令类型
        /// </summary>
        public SystemManagementType CommandType { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 电脑名称
        /// </summary>
        public string ComputerName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// CPU使用率
        /// </summary>
        public float CpuUsage { get; set; }

        /// <summary>
        /// 内存使用率
        /// </summary>
        public float MemoryUsage { get; set; }

        /// <summary>
        /// 磁盘使用情况
        /// </summary>
        public Dictionary<string, DiskInfo> DiskUsage { get; set; } = new Dictionary<string, DiskInfo>();

        /// <summary>
        /// 系统运行时间（秒）
        /// </summary>
        public long SystemUptime { get; set; }

        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public string ConnectionStatus { get; set; }

        /// <summary>
        /// 关闭类型
        /// </summary>
        public string ShutdownType { get; set; }

        /// <summary>
        /// 目标用户ID
        /// </summary>
        public string TargetUserId { get; set; }

        /// <summary>
        /// 目标用户名
        /// </summary>
        public string TargetUserName { get; set; }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public string AdminUserId { get; set; }

        /// <summary>
        /// 是否已执行
        /// </summary>
        public bool IsExecuted { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 创建成功的电脑状态响应
        /// </summary>
        public static SystemCommandResponse CreateComputerStatusSuccess(
            string userId, 
            string userName, 
            string computerName, 
            string ipAddress,
            float cpuUsage = 0,
            float memoryUsage = 0,
            Dictionary<string, DiskInfo> diskUsage = null,
            long systemUptime = 0,
            string clientVersion = "",
            string connectionStatus = "Connected")
        {
            return new SystemCommandResponse
            {
                CommandType = SystemManagementType.ComputerStatus,
                IsSuccess = true,
                Message = "电脑状态查询成功",
                UserId = userId,
                UserName = userName,
                ComputerName = computerName,
                IpAddress = ipAddress,
                CpuUsage = cpuUsage,
                MemoryUsage = memoryUsage,
                DiskUsage = diskUsage ?? new Dictionary<string, DiskInfo>(),
                SystemUptime = systemUptime,
                ClientVersion = clientVersion,
                ConnectionStatus = connectionStatus
            };
        }

        /// <summary>
        /// 创建失败的电脑状态响应
        /// </summary>
        public static SystemCommandResponse CreateComputerStatusFailure(string errorMessage, string errorCode = null)
        {
            return new SystemCommandResponse
            {
                CommandType = SystemManagementType.ComputerStatus,
                IsSuccess = false,
                Message = errorMessage,
                Metadata = new Dictionary<string, object>
                {
                    ["ErrorCode"] = errorCode ?? "COMPUTER_STATUS_ERROR"
                }
            };
        }

        /// <summary>
        /// 创建成功的关闭响应
        /// </summary>
        public static SystemCommandResponse CreateShutdownSuccess(
            string userId, 
            string userName, 
            string computerName,
            string shutdownType = "Shutdown",
            bool isExecuted = true)
        {
            return new SystemCommandResponse
            {
                CommandType = shutdownType == "Logoff" ? SystemManagementType.ExitSystem : SystemManagementType.ShutdownComputer,
                IsSuccess = true,
                Message = $"电脑{shutdownType}指令已发送",
                UserId = userId,
                UserName = userName,
                ComputerName = computerName,
                ShutdownType = shutdownType,
                IsExecuted = isExecuted,
                ExecutionTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建失败的关闭响应
        /// </summary>
        public static SystemCommandResponse CreateShutdownFailure(string errorMessage, string errorCode = null)
        {
            return new SystemCommandResponse
            {
                IsSuccess = false,
                Message = errorMessage,
                Metadata = new Dictionary<string, object>
                {
                    ["ErrorCode"] = errorCode ?? "SHUTDOWN_ERROR"
                }
            };
        }

        /// <summary>
        /// 创建成功的强制下线响应
        /// </summary>
        public static SystemCommandResponse CreateForceLogoutSuccess(
            string targetUserId, 
            string targetUserName,
            string adminUserId,
            bool isExecuted = true)
        {
            return new SystemCommandResponse
            {
               // CommandType = SystemManagementType.ForceLogout,
                IsSuccess = true,
                Message = "用户已成功强制下线",
                TargetUserId = targetUserId,
                TargetUserName = targetUserName,
                AdminUserId = adminUserId,
                IsExecuted = isExecuted,
                ExecutionTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建失败的强制下线响应
        /// </summary>
        public static SystemCommandResponse CreateForceLogoutFailure(string errorMessage, string errorCode = null)
        {
            return new SystemCommandResponse
            {
                //CommandType = SystemManagementType.ForceLogout,
                IsSuccess = false,
                Message = errorMessage,
                Metadata = new Dictionary<string, object>
                {
                    ["ErrorCode"] = errorCode ?? "FORCE_LOGOUT_ERROR"
                }
            };
        }
    }
}
