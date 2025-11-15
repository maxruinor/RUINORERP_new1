using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Common
{
    /// <summary>
    /// 命令处理上下文 - 存储命令处理过程中的基础设施信息
    /// 包含会话、认证、追踪等元数据，用于命令处理过程中的上下文共享
    /// </summary>
    [Serializable]
    public class CommandContext 
    {
        /// <summary>
        /// 会话标识符
        /// </summary>
      
        public string SessionId { get; set; }
        
        /// <summary>
        /// 认证令牌信息
        /// </summary>
    
        public TokenInfo Token { get; set; }


        public string RequestId { get; set; }

        /// <summary>
        /// 命令接收时间
        /// </summary>
        public DateTime ReceivedTime { get; set; }

        /// <summary>
        /// 处理开始时间（用于性能统计）
        /// </summary>
        public DateTime ProcessingStartTime { get; set; }

        /// <summary>
        /// 处理结束时间（用于性能统计）
        /// </summary>
        public DateTime ProcessingEndTime { get; set; }

        /// <summary>
        /// 用户认证信息
        /// </summary>
        public long UserId { get; set; }
        
        public string UserName { get; set; }
        
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// 是否需要响应
        /// </summary>
        public bool NeedResponse { get; set; } = true;

        /// <summary>
        /// 期望的响应类型名称 - 用于异常处理时通过反射创建特定类型的响应
        /// 客户端在请求时知道需要什么类型的响应结果，将这个类型名称保存起来
        /// </summary>
        public string ExpectedResponseTypeName { get; set; }
         
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
        {            return new CommandContext
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
                IsAuthenticated = this.IsAuthenticated,
                ExpectedResponseTypeName = this.ExpectedResponseTypeName
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
