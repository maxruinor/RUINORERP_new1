using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Model.ReminderModel.ReminderRules;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.Server.SmartReminder.InvReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{

    public interface IReminderStrategy
    {
        Task<bool> CheckAsync(IReminderRule rule, IReminderContext context);
        int Priority { get; }
        bool CanHandle(ReminderBizType reminderType);
    }
    public interface ISmartReminderMonitor
    {
        bool IsRunning { get; }
        bool PerformQuickCheck();
        Task CheckRemindersAsync();
        void StartMonitoring(TimeSpan interval);

        void AddStrategy(IReminderStrategy strategy);
        Task<List<IReminderRule>> GetActiveRulesAsync();
    }
    // 4. 通知服务实现
    public interface INotificationService
    {
        Task SendNotificationAsync(IReminderRule rule, string message, object contextData);
    }

    /// <summary>
    /// 健康检查接口，用于监控智能提醒系统各组件的健康状态
    /// </summary>
    public interface IHealthCheck
    {
        /// <summary>
        /// 执行完整的健康检查，返回详细的健康状态报告
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>健康检查结果</returns>
        Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 执行快速健康检查，用于频繁监控
        /// </summary>
        /// <returns>系统是否健康</returns>
        bool PerformQuickCheck();
        
        /// <summary>
        /// 获取组件健康状态
        /// </summary>
        /// <returns>组件健康状态字典</returns>
        Dictionary<string, bool> GetComponentHealthStatus();
        
        /// <summary>
        /// 获取系统性能指标
        /// </summary>
        /// <returns>性能指标字典</returns>
        Dictionary<string, object> GetPerformanceMetrics();
    }
    
    /// <summary>
    /// 健康检查结果类
    /// </summary>
    public class HealthCheckResult
    {
        /// <summary>
        /// 健康状态
        /// </summary>
        public HealthStatus Status { get; set; }
        
        /// <summary>
        /// 健康状态描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 详细信息
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// 检查时间
        /// </summary>
        public DateTime CheckTime { get; set; }
        
        /// <summary>
        /// 错误信息（如果有）
        /// </summary>
        public string ErrorMessage { get; set; }
    }
    
    /// <summary>
    /// 健康状态枚举
    /// </summary>
    public enum HealthStatus
    {
        /// <summary>
        /// 健康
        /// </summary>
        Healthy,
        /// <summary>
        /// 亚健康（可正常工作，但有潜在问题）
        /// </summary>
        Degraded,
        /// <summary>
        /// 不健康（无法正常工作）
        /// </summary>
        Unhealthy
    }



}
