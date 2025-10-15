using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.StateManagement
{
    /// <summary>
    /// 增强的状态管理器
    /// 提供更完善的状态跟踪和管理功能
    /// </summary>
    public class EnhancedStateManager
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ConcurrentDictionary<string, HandlerStateInfo> _handlerStates;
        private readonly ConcurrentDictionary<string, SessionStateInfo> _sessionStates;
        private readonly Timer _stateCleanupTimer;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _stateExpiration = TimeSpan.FromHours(1);

        public EnhancedStateManager(CommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _handlerStates = new ConcurrentDictionary<string, HandlerStateInfo>();
            _sessionStates = new ConcurrentDictionary<string, SessionStateInfo>();
            
            // 启动状态清理定时器
            _stateCleanupTimer = new Timer(CleanupExpiredStates, null, _cleanupInterval, _cleanupInterval);
        }

        /// <summary>
        /// 更新处理器状态
        /// </summary>
        /// <param name="handler">命令处理器</param>
        /// <param name="status">状态</param>
        /// <param name="additionalInfo">附加信息</param>
        public void UpdateHandlerState(ICommandHandler handler, HandlerStatus status, string additionalInfo = null)
        {
            var stateInfo = new HandlerStateInfo
            {
                HandlerId = handler.HandlerId,
                HandlerName = handler.Name,
                Status = status,
                LastUpdated = DateTime.Now,
                AdditionalInfo = additionalInfo,
                Statistics = handler.GetStatistics()
            };

            _handlerStates[handler.HandlerId] = stateInfo;
        }

        /// <summary>
        /// 获取处理器状态
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <returns>处理器状态信息</returns>
        public HandlerStateInfo GetHandlerState(string handlerId)
        {
            return _handlerStates.TryGetValue(handlerId, out var state) ? state : null;
        }

        /// <summary>
        /// 获取所有处理器状态
        /// </summary>
        /// <returns>处理器状态信息列表</returns>
        public List<HandlerStateInfo> GetAllHandlerStates()
        {
            return _handlerStates.Values.ToList();
        }

        /// <summary>
        /// 更新会话状态
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="state">会话状态</param>
        /// <param name="userInfo">用户信息</param>
        public void UpdateSessionState(string sessionId, SessionState state, SessionUserInfo userInfo = null)
        {
            var stateInfo = new SessionStateInfo
            {
                SessionId = sessionId,
                State = state,
                LastUpdated = DateTime.Now,
                UserInfo = userInfo
            };

            _sessionStates[sessionId] = stateInfo;
        }

        /// <summary>
        /// 获取会话状态
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话状态信息</returns>
        public SessionStateInfo GetSessionState(string sessionId)
        {
            return _sessionStates.TryGetValue(sessionId, out var state) ? state : null;
        }

        /// <summary>
        /// 获取所有会话状态
        /// </summary>
        /// <returns>会话状态信息列表</returns>
        public List<SessionStateInfo> GetAllSessionStates()
        {
            return _sessionStates.Values.ToList();
        }

        /// <summary>
        /// 获取活跃会话数
        /// </summary>
        /// <returns>活跃会话数</returns>
        public int GetActiveSessionCount()
        {
            return _sessionStates.Values.Count(s => s.State == SessionState.Active);
        }

        /// <summary>
        /// 获取会话统计信息
        /// </summary>
        /// <returns>会话统计信息</returns>
        public SessionStatistics GetSessionStatistics()
        {
            var allSessions = _sessionStates.Values.ToList();
            
            return new SessionStatistics
            {
                TotalSessions = allSessions.Count,
                ActiveSessions = allSessions.Count(s => s.State == SessionState.Active),
                InactiveSessions = allSessions.Count(s => s.State == SessionState.Inactive),
                ExpiredSessions = allSessions.Count(s => s.State == SessionState.Expired),
                AverageSessionDuration = CalculateAverageSessionDuration(allSessions),
                PeakSessionCount = allSessions.Count, // 简化处理，实际应该跟踪历史峰值
                SessionStateDistribution = allSessions.GroupBy(s => s.State)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }

        /// <summary>
        /// 计算平均会话持续时间
        /// </summary>
        /// <param name="sessions">会话列表</param>
        /// <returns>平均会话持续时间（分钟）</returns>
        private double CalculateAverageSessionDuration(List<SessionStateInfo> sessions)
        {
            if (!sessions.Any()) return 0;
            
            var activeSessions = sessions.Where(s => s.State == SessionState.Active).ToList();
            if (!activeSessions.Any()) return 0;
            
            // 简化处理，实际应该跟踪会话开始时间
            return activeSessions.Average(s => (DateTime.Now - s.LastUpdated).TotalMinutes);
        }

        /// <summary>
        /// 清理过期状态
        /// </summary>
        /// <param name="state">状态对象</param>
        private void CleanupExpiredStates(object state)
        {
            var now = DateTime.Now;
            
            // 清理会话状态
            var expiredSessions = _sessionStates.Where(kvp => 
                now - kvp.Value.LastUpdated > _stateExpiration).ToList();
            
            foreach (var expired in expiredSessions)
            {
                _sessionStates.TryRemove(expired.Key, out _);
            }
            
            // 清理处理器状态（如果需要）
            // 处理器状态通常不需要清理，因为它们与处理器生命周期绑定
        }

        /// <summary>
        /// 获取系统状态报告
        /// </summary>
        /// <returns>系统状态报告</returns>
        public string GetSystemStateReport()
        {
            var report = $"=== 系统状态报告 ===\n";
            report += $"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n";
            
            // 处理器状态
            report += "== 处理器状态 ==\n";
            foreach (var handlerState in _handlerStates.Values)
            {
                report += $"  {handlerState.HandlerName}: {handlerState.Status}\n";
                if (!string.IsNullOrEmpty(handlerState.AdditionalInfo))
                {
                    report += $"    附加信息: {handlerState.AdditionalInfo}\n";
                }
            }
            
            report += "\n";
            
            // 会话状态统计
            var sessionStats = GetSessionStatistics();
            report += "== 会话状态统计 ==\n";
            report += $"  总会话数: {sessionStats.TotalSessions}\n";
            report += $"  活跃会话数: {sessionStats.ActiveSessions}\n";
            report += $"  非活跃会话数: {sessionStats.InactiveSessions}\n";
            report += $"  过期会话数: {sessionStats.ExpiredSessions}\n";
            report += $"  平均会话持续时间: {sessionStats.AverageSessionDuration:F2}分钟\n";
            
            return report;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _stateCleanupTimer?.Dispose();
        }
    }

    /// <summary>
    /// 处理器状态信息
    /// </summary>
    public class HandlerStateInfo
    {
        /// <summary>
        /// 处理器ID
        /// </summary>
        public string HandlerId { get; set; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 处理器状态
        /// </summary>
        public HandlerStatus Status { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public HandlerStatistics Statistics { get; set; }
    }

    /// <summary>
    /// 会话状态信息
    /// </summary>
    public class SessionStateInfo
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 会话状态
        /// </summary>
        public SessionState State { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public SessionUserInfo UserInfo { get; set; }
    }

    /// <summary>
    /// 会话状态
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// 活跃
        /// </summary>
        Active = 0,

        /// <summary>
        /// 非活跃
        /// </summary>
        Inactive = 1,

        /// <summary>
        /// 过期
        /// </summary>
        Expired = 2,

        /// <summary>
        /// 已断开
        /// </summary>
        Disconnected = 3
    }

    /// <summary>
    /// 会话用户信息
    /// </summary>
    public class SessionUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivityTime { get; set; }
    }

    /// <summary>
    /// 会话统计信息
    /// </summary>
    public class SessionStatistics
    {
        /// <summary>
        /// 总会话数
        /// </summary>
        public int TotalSessions { get; set; }

        /// <summary>
        /// 活跃会话数
        /// </summary>
        public int ActiveSessions { get; set; }

        /// <summary>
        /// 非活跃会话数
        /// </summary>
        public int InactiveSessions { get; set; }

        /// <summary>
        /// 过期会话数
        /// </summary>
        public int ExpiredSessions { get; set; }

        /// <summary>
        /// 平均会话持续时间（分钟）
        /// </summary>
        public double AverageSessionDuration { get; set; }

        /// <summary>
        /// 峰值会话数
        /// </summary>
        public int PeakSessionCount { get; set; }

        /// <summary>
        /// 会话状态分布
        /// </summary>
        public Dictionary<SessionState, int> SessionStateDistribution { get; set; }
    }
}