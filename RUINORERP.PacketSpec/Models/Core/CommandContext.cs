using RUINORERP.PacketSpec.Commands.Authentication;
using System;
using System.Collections.Generic;
using MessagePack;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 命令处理上下文 - 存储命令处理过程中的基础设施信息
    /// 包含会话、认证、追踪等元数据，用于命令处理过程中的上下文共享
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class CommandContext 
    {
        /// <summary>
        /// 会话标识符
        /// </summary>
        [Key(0)]
        public string SessionId { get; set; }
        
        /// <summary>
        /// 认证令牌信息
        /// </summary>
        [Key(1)]
        public TokenInfo Token { get; set; }
        
        /// <summary>
        /// 命令接收时间
        /// </summary>
        [Key(2)]
        public DateTime ReceivedTime { get; set; }

        /// <summary>
        /// 处理开始时间（用于性能统计）
        /// </summary>
        [Key(3)]
        public DateTime ProcessingStartTime { get; set; }

        /// <summary>
        /// 处理结束时间（用于性能统计）
        /// </summary>
        [Key(4)]
        public DateTime ProcessingEndTime { get; set; }

        /// <summary>
        /// 用户认证信息
        /// </summary>
        [Key(5)]
        public long UserId { get; set; }
        
        [Key(6)]
        public string UserName { get; set; }
        
        [Key(7)]
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 初始化新的CommandContext实例
        /// </summary>
        public CommandContext()
        {
            ReceivedTime = DateTime.Now;
            IsAuthenticated = false;
        }

        /// <summary>
        /// 创建当前CommandContext的深拷贝
        /// </summary>
        /// <returns>CommandContext的深拷贝实例</returns>
        public CommandContext Clone()
        {
            return new CommandContext
            {
                SessionId = this.SessionId,
                Token = this.Token != null ? new TokenInfo
                {
                    AccessToken = this.Token.AccessToken,
                    RefreshToken = this.Token.RefreshToken,
                    ExpiresAt = this.Token.ExpiresAt,
                    TokenType = this.Token.TokenType
                } : null,
                ReceivedTime = this.ReceivedTime,
                ProcessingStartTime = this.ProcessingStartTime,
                ProcessingEndTime = this.ProcessingEndTime,
                UserId = this.UserId,
                UserName = this.UserName,
                IsAuthenticated = this.IsAuthenticated
            };
        }

        /// <summary>
        /// 标记处理开始时间
        /// </summary>
        public void MarkProcessingStart()
        {
            ProcessingStartTime = DateTime.Now;
        }

        /// <summary>
        /// 标记处理结束时间
        /// </summary>
        public void MarkProcessingEnd()
        {
            ProcessingEndTime = DateTime.Now;
        }

        /// <summary>
        /// 获取处理耗时（毫秒）
        /// </summary>
        /// <returns>处理耗时（毫秒）</returns>
        public double GetProcessingDurationMs()
        {
            if (ProcessingStartTime == default || ProcessingEndTime == default)
                return 0;

            return (ProcessingEndTime - ProcessingStartTime).TotalMilliseconds;
        }
    }
}
