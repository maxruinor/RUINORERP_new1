using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.PacketSpec.Commands.EnhancedHandlerStatistics
{
    /// <summary>
    /// 命令处理器执行记录
    /// 记录单个命令的执行信息
    /// </summary>
    public class CommandExecutionRecord
    {
        /// <summary>
        /// 命令ID
        /// </summary>
        public string CommandId { get; }
        
        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime StartTime { get; }
        
        /// <summary>
        /// 结束执行时间
        /// </summary>
        public DateTime EndTime { get; }
        
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool IsSuccess { get; }
        
        /// <summary>
        /// 错误信息（如果执行失败）
        /// </summary>
        public string ErrorMessage { get; }
        
        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs
        {
            get { return (long)(EndTime - StartTime).TotalMilliseconds; }
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="startTime">开始执行时间</param>
        /// <param name="endTime">结束执行时间</param>
        /// <param name="isSuccess">执行是否成功</param>
        /// <param name="errorMessage">错误信息</param>
        public CommandExecutionRecord(string commandId, DateTime startTime, DateTime endTime, bool isSuccess, string errorMessage = null)
        {
            CommandId = commandId;
            StartTime = startTime;
            EndTime = endTime;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
    
    /// <summary>
    /// 增强版命令处理器统计类
    /// 提供更全面的命令处理统计功能
    /// 包括总请求数、超时请求数、成功率、平均处理时间等指标
    /// </summary>
    public class EnhancedHandlerStatisticsClass
    {
        // 总请求数
        private long _totalRequests;
        
        // 成功请求数
        private long _successfulRequests;
        
        // 超时请求数
        private long _timeoutRequests;
        
        // 失败请求数
        private long _failedRequests;
        
        // 总处理时间（毫秒）
        private long _totalProcessingTime;
        
        // 最小处理时间（毫秒）
        private long _minProcessingTime;
        
        // 最大处理时间（毫秒）
        private long _maxProcessingTime;
        
        // 最近执行记录队列
        private readonly ConcurrentQueue<CommandExecutionRecord> _recentRecords;
        
        // 最大最近记录数
        private const int MaxRecentRecords = 100;
        
        // 命令类型统计字典
        private readonly ConcurrentDictionary<string, CommandTypeStatistics> _commandTypeStats;
        
        /// <summary>
        /// 总请求数
        /// </summary>
        public long TotalRequests => _totalRequests;
        
        /// <summary>
        /// 成功请求数
        /// </summary>
        public long SuccessfulRequests => _successfulRequests;
        
        /// <summary>
        /// 超时请求数
        /// </summary>
        public long TimeoutRequests => _timeoutRequests;
        
        /// <summary>
        /// 失败请求数
        /// </summary>
        public long FailedRequests => _failedRequests;
        
        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTime => _totalRequests > 0 ? (double)_totalProcessingTime / _totalRequests : 0;
        
        /// <summary>
        /// 最小处理时间（毫秒）
        /// </summary>
        public long MinProcessingTime => _minProcessingTime;
        
        /// <summary>
        /// 最大处理时间（毫秒）
        /// </summary>
        public long MaxProcessingTime => _maxProcessingTime;
        
        /// <summary>
        /// 成功率（百分比）
        /// </summary>
        public double SuccessRate => _totalRequests > 0 ? (double)_successfulRequests / _totalRequests * 100 : 0;
        
        /// <summary>
        /// 超时率（百分比）
        /// </summary>
        public double TimeoutRate => _totalRequests > 0 ? (double)_timeoutRequests / _totalRequests * 100 : 0;
        
        /// <summary>
        /// 失败率（百分比）
        /// </summary>
        public double FailureRate => _totalRequests > 0 ? (double)_failedRequests / _totalRequests * 100 : 0;
        
        /// <summary>
        /// 构造函数
        /// 初始化增强版命令处理器统计类
        /// </summary>
        public EnhancedHandlerStatisticsClass()
        {
            _totalRequests = 0;
            _successfulRequests = 0;
            _timeoutRequests = 0;
            _failedRequests = 0;
            _totalProcessingTime = 0;
            _minProcessingTime = long.MaxValue;
            _maxProcessingTime = 0;
            _recentRecords = new ConcurrentQueue<CommandExecutionRecord>();
            _commandTypeStats = new ConcurrentDictionary<string, CommandTypeStatistics>();
        }
        
        /// <summary>
        /// 记录命令执行信息
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="startTime">开始执行时间</param>
        /// <param name="endTime">结束执行时间</param>
        /// <param name="isSuccess">执行是否成功</param>
        /// <param name="isTimeout">是否超时</param>
        /// <param name="errorMessage">错误信息</param>
        public void RecordExecution(string commandId, DateTime startTime, DateTime endTime, bool isSuccess, bool isTimeout = false, string errorMessage = null)
        {
            if (string.IsNullOrEmpty(commandId))
                throw new ArgumentNullException(nameof(commandId));
            
            // 计算执行时间
            long executionTimeMs = (long)(endTime - startTime).TotalMilliseconds;
            
            // 更新统计数据
            lock (this)
            {
                _totalRequests++;
                _totalProcessingTime += executionTimeMs;
                
                if (executionTimeMs < _minProcessingTime)
                    _minProcessingTime = executionTimeMs;
                if (executionTimeMs > _maxProcessingTime)
                    _maxProcessingTime = executionTimeMs;
                
                if (isSuccess)
                {
                    _successfulRequests++;
                }
                else
                {
                    _failedRequests++;
                    if (isTimeout)
                        _timeoutRequests++;
                }
            }
            
            // 更新命令类型统计
            var commandStats = _commandTypeStats.GetOrAdd(commandId, id => new CommandTypeStatistics(id));
            commandStats.RecordExecution(startTime, endTime, isSuccess, isTimeout, errorMessage);
            
            // 添加到最近记录队列
            var record = new CommandExecutionRecord(commandId, startTime, endTime, isSuccess, errorMessage);
            _recentRecords.Enqueue(record);
            
            // 限制队列大小
            while (_recentRecords.Count > MaxRecentRecords)
            {
                _recentRecords.TryDequeue(out _);
            }
        }
        
        /// <summary>
        /// 获取最近的命令执行记录
        /// </summary>
        /// <param name="count">记录数量</param>
        /// <returns>命令执行记录列表</returns>
        public List<CommandExecutionRecord> GetRecentRecords(int count = 50)
        {
            if (count <= 0)
                count = 50;
            
            var list = _recentRecords.ToList();
            if (list.Count <= count)
                return list;
            
            return list.Skip(Math.Max(0, list.Count - count)).ToList();
        }
        
        /// <summary>
        /// 获取指定命令类型的统计信息
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令类型统计信息</returns>
        public CommandTypeStatistics GetCommandTypeStatistics(string commandId)
        {
            if (string.IsNullOrEmpty(commandId))
                throw new ArgumentNullException(nameof(commandId));
            
            _commandTypeStats.TryGetValue(commandId, out var stats);
            return stats;
        }
        
        /// <summary>
        /// 获取所有命令类型的统计信息
        /// </summary>
        /// <returns>命令类型统计信息列表</returns>
        public List<CommandTypeStatistics> GetAllCommandTypeStatistics()
        {
            return _commandTypeStats.Values.ToList();
        }
        
        /// <summary>
        /// 重置统计数据
        /// </summary>
        public void Reset()
        {
            lock (this)
            {
                _totalRequests = 0;
                _successfulRequests = 0;
                _timeoutRequests = 0;
                _failedRequests = 0;
                _totalProcessingTime = 0;
                _minProcessingTime = long.MaxValue;
                _maxProcessingTime = 0;
            }
            
            // 清空最近记录队列
            while (_recentRecords.TryDequeue(out _)) { }
            _commandTypeStats.Clear();
        }
        
        /// <summary>
        /// 生成统计报告
        /// </summary>
        /// <returns>统计报告字符串</returns>
        public string GenerateReport()
        {
            var report = new StringBuilder();
            report.AppendLine("=== Command Handler Statistics Report ===");
            report.AppendLine($"Total Requests: {TotalRequests}");
            report.AppendLine($"Successful Requests: {SuccessfulRequests} ({SuccessRate:F2}%)");
            report.AppendLine($"Failed Requests: {FailedRequests} ({FailureRate:F2}%)");
            report.AppendLine($"Timeout Requests: {TimeoutRequests} ({TimeoutRate:F2}%)");
            report.AppendLine($"Average Processing Time: {AverageProcessingTime:F2} ms");
            report.AppendLine($"Min Processing Time: {MinProcessingTime} ms");
            report.AppendLine($"Max Processing Time: {MaxProcessingTime} ms");
            report.AppendLine();
            
            // 添加命令类型统计
            if (_commandTypeStats.Count > 0)
            {
                report.AppendLine("Command Type Statistics:");
                foreach (var stats in _commandTypeStats.Values.OrderByDescending(s => s.TotalRequests))
                {
                    report.AppendLine($"- {stats.CommandId}:");
                    report.AppendLine($"  Total: {stats.TotalRequests}, Success: {stats.SuccessfulRequests} ({stats.SuccessRate:F2}%), Timeout: {stats.TimeoutRequests} ({stats.TimeoutRate:F2}%)");
                    report.AppendLine($"  Avg Time: {stats.AverageProcessingTime:F2} ms");
                }
            }
            
            return report.ToString();
        }
    }
    
    /// <summary>
    /// 命令类型统计信息
    /// </summary>
    public class CommandTypeStatistics
    {
        /// <summary>
        /// 命令ID
        /// </summary>
        public string CommandId { get; }
        
        /// <summary>
        /// 总请求数
        /// </summary>
        public long TotalRequests { get; private set; }
        
        /// <summary>
        /// 成功请求数
        /// </summary>
        public long SuccessfulRequests { get; private set; }
        
        /// <summary>
        /// 超时请求数
        /// </summary>
        public long TimeoutRequests { get; private set; }
        
        /// <summary>
        /// 失败请求数
        /// </summary>
        public long FailedRequests { get; private set; }
        
        /// <summary>
        /// 总处理时间（毫秒）
        /// </summary>
        public long TotalProcessingTime { get; private set; }
        
        /// <summary>
        /// 最小处理时间（毫秒）
        /// </summary>
        public long MinProcessingTime { get; private set; } = long.MaxValue;
        
        /// <summary>
        /// 最大处理时间（毫秒）
        /// </summary>
        public long MaxProcessingTime { get; private set; }
        
        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTime => TotalRequests > 0 ? (double)TotalProcessingTime / TotalRequests : 0;
        
        /// <summary>
        /// 成功率（百分比）
        /// </summary>
        public double SuccessRate => TotalRequests > 0 ? (double)SuccessfulRequests / TotalRequests * 100 : 0;
        
        /// <summary>
        /// 超时率（百分比）
        /// </summary>
        public double TimeoutRate => TotalRequests > 0 ? (double)TimeoutRequests / TotalRequests * 100 : 0;
        
        /// <summary>
        /// 失败率（百分比）
        /// </summary>
        public double FailureRate => TotalRequests > 0 ? (double)FailedRequests / TotalRequests * 100 : 0;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public CommandTypeStatistics(string commandId)
        {
            CommandId = commandId;
            TotalRequests = 0;
            SuccessfulRequests = 0;
            TimeoutRequests = 0;
            FailedRequests = 0;
            TotalProcessingTime = 0;
            MinProcessingTime = long.MaxValue;
            MaxProcessingTime = 0;
        }
        
        /// <summary>
        /// 记录命令执行信息
        /// </summary>
        /// <param name="startTime">开始执行时间</param>
        /// <param name="endTime">结束执行时间</param>
        /// <param name="isSuccess">执行是否成功</param>
        /// <param name="isTimeout">是否超时</param>
        /// <param name="errorMessage">错误信息</param>
        public void RecordExecution(DateTime startTime, DateTime endTime, bool isSuccess, bool isTimeout = false, string errorMessage = null)
        {
            // 计算执行时间
            long executionTimeMs = (long)(endTime - startTime).TotalMilliseconds;
            
            // 更新统计数据
            TotalRequests++;
            TotalProcessingTime += executionTimeMs;
            
            if (executionTimeMs < MinProcessingTime)
                MinProcessingTime = executionTimeMs;
            if (executionTimeMs > MaxProcessingTime)
                MaxProcessingTime = executionTimeMs;
            
            if (isSuccess)
            {
                SuccessfulRequests++;
            }
            else
            {
                FailedRequests++;
                if (isTimeout)
                    TimeoutRequests++;
            }
        }
    }
}
