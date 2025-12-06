using System;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Models.Requests
{


    /// <summary>
    /// 统一系统指令请求
    /// </summary>
    [Serializable]
    public class SystemCommandRequest : RequestBase
    {
        /// <summary>
        /// 系统指令类型
        /// </summary>
        public SystemManagementType CommandType { get; set; }
        /// <summary>
        /// 目标用户ID
        /// </summary>
        public string TargetUserId { get; set; }


        /// <summary>
        /// 延迟时间（秒）
        /// </summary>
        public int DelaySeconds { get; set; } = 0;

        /// <summary>
        /// 管理员ID
        /// </summary>
        public string AdminUserId { get; set; }

        /// <summary>
        /// 创建电脑状态查询请求
        /// </summary>
        public static SystemCommandRequest CreateComputerStatusRequest(string targetUserId, string requestType = "Status")
        {
            return new SystemCommandRequest
            {
                CommandType = SystemManagementType.ComputerStatus,
                TargetUserId = targetUserId,
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
                CommandType = SystemManagementType.ShutdownComputer,
                TargetUserId = targetUserId,
                DelaySeconds = delaySeconds,
                RequestId = IdGenerator.GenerateRequestId(SystemCommands.SystemManagement)
            };
        }

        /// <summary>
        /// 创建退出系统请求
        /// </summary>
        public static SystemCommandRequest CreateExitSystemRequest(string targetUserId, int delaySeconds = 0, string adminRemark = "")
        {
            return new SystemCommandRequest
            {
                CommandType = SystemManagementType.ExitERPSystem,
                TargetUserId = targetUserId,
                DelaySeconds = delaySeconds,
                RequestId = IdGenerator.GenerateRequestId(SystemCommands.SystemManagement)
            };
        }



    }
}
