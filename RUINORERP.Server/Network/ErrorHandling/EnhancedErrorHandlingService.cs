using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.ErrorHandling
{
    /// <summary>
    /// 增强的错误处理服务
    /// 提供更全面的错误处理和诊断功能
    /// </summary>
    public class EnhancedErrorHandlingService
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ConcurrentDictionary<string, ErrorRecord> _errorRecords;
        private readonly int _maxErrorRecords = 1000; // 最大错误记录数

        public EnhancedErrorHandlingService(CommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _errorRecords = new ConcurrentDictionary<string, ErrorRecord>();
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <param name="handlerName">处理器名称</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="exception">异常</param>
        /// <param name="context">上下文信息</param>
        public void RecordError(string handlerId, string handlerName, uint commandId, Exception exception, ErrorContext context = null)
        {
            // 如果错误记录数已达上限，移除最旧的记录
            if (_errorRecords.Count >= _maxErrorRecords)
            {
                var oldestKey = _errorRecords.OrderBy(kvp => kvp.Value.Timestamp).First().Key;
                _errorRecords.TryRemove(oldestKey, out _);
            }

            var record = new ErrorRecord
            {
                Id = Guid.NewGuid().ToString(),
                HandlerId = handlerId,
                HandlerName = handlerName,
                CommandId = commandId,
                Exception = exception,
                Context = context,
                Timestamp = DateTime.UtcNow
            };

            _errorRecords[record.Id] = record;
        }

        /// <summary>
        /// 记录API响应错误
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <param name="handlerName">处理器名称</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="response">API响应</param>
        /// <param name="context">上下文信息</param>
        public void RecordApiResponseError(string handlerId, string handlerName, uint commandId, ResponseBase response, ErrorContext context = null)
        {
            var exception = new ApiResponseException(response.Message, response.Code)
            {
                ApiResponse = response
            };

            RecordError(handlerId, handlerName, commandId, exception, context);
        }

        /// <summary>
        /// 获取错误记录
        /// </summary>
        /// <param name="recordId">记录ID</param>
        /// <returns>错误记录</returns>
        public ErrorRecord GetErrorRecord(string recordId)
        {
            return _errorRecords.TryGetValue(recordId, out var record) ? record : null;
        }

        /// <summary>
        /// 获取所有错误记录
        /// </summary>
        /// <param name="limit">限制数量</param>
        /// <returns>错误记录列表</returns>
        public List<ErrorRecord> GetAllErrorRecords(int limit = 100)
        {
            return _errorRecords.Values
                .OrderByDescending(r => r.Timestamp)
                .Take(limit)
                .ToList();
        }

        /// <summary>
        /// 获取错误统计信息
        /// </summary>
        /// <returns>错误统计信息</returns>
        public ErrorStatistics GetErrorStatistics()
        {
            var records = _errorRecords.Values.ToList();
            var now = DateTime.UtcNow;
            var last24Hours = now.AddHours(-24);
            var recentRecords = records.Where(r => r.Timestamp >= last24Hours).ToList();

            return new ErrorStatistics
            {
                TotalErrors = records.Count,
                RecentErrors = recentRecords.Count,
                ErrorsByHandler = records.GroupBy(r => r.HandlerName)
                    .ToDictionary(g => g.Key, g => g.Count()),
                ErrorsByType = records.GroupBy(r => r.Exception.GetType().Name)
                    .ToDictionary(g => g.Key, g => g.Count()),
                ErrorsByCommand = records.GroupBy(r => r.CommandId)
                    .ToDictionary(g => g.Key, g => g.Count()),
                MostCommonErrors = GetMostCommonErrors(records),
                ErrorTrend = GetErrorTrend(recentRecords)
            };
        }

        /// <summary>
        /// 获取最常见的错误
        /// </summary>
        /// <param name="records">错误记录</param>
        /// <returns>最常见的错误列表</returns>
        private List<CommonErrorInfo> GetMostCommonErrors(List<ErrorRecord> records)
        {
            return records.GroupBy(r => new { r.Exception.GetType().Name, r.Exception.Message })
                .Select(g => new CommonErrorInfo
                {
                    ExceptionType = g.Key.Name,
                    Message = g.Key.Message,
                    Count = g.Count(),
                    FirstOccurrence = g.Min(r => r.Timestamp),
                    LastOccurrence = g.Max(r => r.Timestamp)
                })
                .OrderByDescending(e => e.Count)
                .Take(10)
                .ToList();
        }

        /// <summary>
        /// 获取错误趋势
        /// </summary>
        /// <param name="recentRecords">最近的错误记录</param>
        /// <returns>错误趋势</returns>
        private ErrorTrend GetErrorTrend(List<ErrorRecord> recentRecords)
        {
            if (!recentRecords.Any()) return new ErrorTrend();

            var hourlyCounts = new int[24];
            var now = DateTime.UtcNow;

            foreach (var record in recentRecords)
            {
                var hoursAgo = (int)(now - record.Timestamp).TotalHours;
                if (hoursAgo >= 0 && hoursAgo < 24)
                {
                    hourlyCounts[23 - hoursAgo]++;
                }
            }

            return new ErrorTrend
            {
                HourlyCounts = hourlyCounts,
                TrendDirection = CalculateTrendDirection(hourlyCounts)
            };
        }

        /// <summary>
        /// 计算趋势方向
        /// </summary>
        /// <param name="hourlyCounts">小时计数</param>
        /// <returns>趋势方向</returns>
        private TrendDirection CalculateTrendDirection(int[] hourlyCounts)
        {
            if (hourlyCounts.Length < 2) return TrendDirection.Stable;

            var firstHalf = hourlyCounts.Take(hourlyCounts.Length / 2).Average();
            var secondHalf = hourlyCounts.Skip(hourlyCounts.Length / 2).Average();

            if (secondHalf > firstHalf * 1.1) return TrendDirection.Increasing;
            if (secondHalf < firstHalf * 0.9) return TrendDirection.Decreasing;
            return TrendDirection.Stable;
        }

        /// <summary>
        /// 获取错误分析报告
        /// </summary>
        /// <returns>错误分析报告</returns>
        public string GetErrorAnalysisReport()
        {
            var stats = GetErrorStatistics();
            var report = new StringBuilder();
            
            report.AppendLine("=== 错误分析报告 ===");
            report.AppendLine($"生成时间: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();

            report.AppendLine("== 错误统计 ==");
            report.AppendLine($"总错误数: {stats.TotalErrors}");
            report.AppendLine($"最近24小时错误数: {stats.RecentErrors}");
            report.AppendLine($"错误趋势: {stats.ErrorTrend.TrendDirection}");
            report.AppendLine();

            report.AppendLine("== 按处理器分类 ==");
            foreach (var handler in stats.ErrorsByHandler.OrderByDescending(kvp => kvp.Value))
            {
                report.AppendLine($"  {handler.Key}: {handler.Value}个错误");
            }
            report.AppendLine();

            report.AppendLine("== 按错误类型分类 ==");
            foreach (var errorType in stats.ErrorsByType.OrderByDescending(kvp => kvp.Value))
            {
                report.AppendLine($"  {errorType.Key}: {errorType.Value}个错误");
            }
            report.AppendLine();

            report.AppendLine("== 按命令分类 ==");
            foreach (var command in stats.ErrorsByCommand.OrderByDescending(kvp => kvp.Value))
            {
                report.AppendLine($"  命令{command.Key}: {command.Value}个错误");
            }
            report.AppendLine();

            report.AppendLine("== 最常见的错误 ==");
            foreach (var error in stats.MostCommonErrors)
            {
                report.AppendLine($"  {error.ExceptionType}: {error.Message}");
                report.AppendLine($"    出现次数: {error.Count}");
                report.AppendLine($"    首次出现: {error.FirstOccurrence:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine($"    最后出现: {error.LastOccurrence:yyyy-MM-dd HH:mm:ss}");
                report.AppendLine();
            }

            return report.ToString();
        }

        /// <summary>
        /// 获取详细的错误信息
        /// </summary>
        /// <param name="record">错误记录</param>
        /// <returns>详细错误信息</returns>
        public string GetDetailedErrorInfo(ErrorRecord record)
        {
            var info = new StringBuilder();
            
            info.AppendLine("=== 详细错误信息 ===");
            info.AppendLine($"记录ID: {record.Id}");
            info.AppendLine($"处理器: {record.HandlerName} ({record.HandlerId})");
            info.AppendLine($"命令ID: {record.CommandId}");
            info.AppendLine($"时间戳: {record.Timestamp:yyyy-MM-dd HH:mm:ss}");
            info.AppendLine();
            
            info.AppendLine("异常信息:");
            info.AppendLine($"  类型: {record.Exception.GetType().FullName}");
            info.AppendLine($"  消息: {record.Exception.Message}");
            info.AppendLine($"  堆栈跟踪: {record.Exception.StackTrace}");
            info.AppendLine();
            
            if (record.Exception.InnerException != null)
            {
                info.AppendLine("内部异常:");
                info.AppendLine($"  类型: {record.Exception.InnerException.GetType().FullName}");
                info.AppendLine($"  消息: {record.Exception.InnerException.Message}");
                info.AppendLine($"  堆栈跟踪: {record.Exception.InnerException.StackTrace}");
                info.AppendLine();
            }
            
            if (record.Context != null)
            {
                info.AppendLine("上下文信息:");
                info.AppendLine($"  用户ID: {record.Context.UserId}");
                info.AppendLine($"  会话ID: {record.Context.SessionId}");
                info.AppendLine($"  客户端IP: {record.Context.ClientIP}");
                info.AppendLine($"  请求数据: {record.Context.RequestData}");
                info.AppendLine();
            }
            
            if (record.Exception is ApiResponseException apiEx && apiEx.ApiResponse != null)
            {
                info.AppendLine("API响应信息:");
                info.AppendLine($"  状态码: {apiEx.ApiResponse.Code}");
                info.AppendLine($"  消息: {apiEx.ApiResponse.Message}");
                info.AppendLine($"  是否成功: {apiEx.ApiResponse.IsSuccess}");
                info.AppendLine();
            }

            return info.ToString();
        }

        /// <summary>
        /// 重置错误统计
        /// </summary>
        public void ResetErrorStatistics()
        {
            _errorRecords.Clear();
        }

        /// <summary>
        /// 创建友好的错误响应
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="defaultMessage">默认消息</param>
        /// <param name="defaultCode">默认代码</param>
        /// <returns>API响应</returns>
        public ResponseBase CreateFriendlyErrorResponse(Exception exception, string defaultMessage = "处理请求时发生错误", int defaultCode = 500)
        {
            // 根据异常类型创建不同的错误响应
            switch (exception)
            {
                case ApiResponseException apiEx:
                    return ResponseBase.CreateError(apiEx.Message, apiEx.Code);
                    
                case ArgumentException argEx:
                    return ResponseBase.CreateError($"参数错误: {argEx.Message}", 400);
                    
                case UnauthorizedAccessException _:
                    return ResponseBase.CreateError("访问被拒绝", 403);
                    
                case TimeoutException _:
                    return ResponseBase.CreateError("请求超时", 408);
                    
                case NotImplementedException _:
                    return ResponseBase.CreateError("功能未实现", 501);
                    
                default:
                    // 记录未知错误
                    return ResponseBase.CreateError(defaultMessage, defaultCode);
            }
        }
    }

    /// <summary>
    /// 错误记录
    /// </summary>
    public class ErrorRecord
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 处理器ID
        /// </summary>
        public string HandlerId { get; set; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// 命令ID
        /// </summary>
        public uint CommandId { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 上下文信息
        /// </summary>
        public ErrorContext Context { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 错误上下文信息
    /// </summary>
    public class ErrorContext
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 其他自定义数据
        /// </summary>
        public Dictionary<string, object> CustomData { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// API响应异常
    /// </summary>
    public class ApiResponseException : Exception
    {
        /// <summary>
        /// API响应
        /// </summary>
        public ResponseBase ApiResponse { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        public ApiResponseException(string message, int code) : base(message)
        {
            Code = code;
        }

        public ApiResponseException(string message, int code, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }

    /// <summary>
    /// 错误统计信息
    /// </summary>
    public class ErrorStatistics
    {
        /// <summary>
        /// 总错误数
        /// </summary>
        public int TotalErrors { get; set; }

        /// <summary>
        /// 最近错误数
        /// </summary>
        public int RecentErrors { get; set; }

        /// <summary>
        /// 按处理器分类的错误数
        /// </summary>
        public Dictionary<string, int> ErrorsByHandler { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// 按错误类型分类的错误数
        /// </summary>
        public Dictionary<string, int> ErrorsByType { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// 按命令分类的错误数
        /// </summary>
        public Dictionary<uint, int> ErrorsByCommand { get; set; } = new Dictionary<uint, int>();

        /// <summary>
        /// 最常见的错误
        /// </summary>
        public List<CommonErrorInfo> MostCommonErrors { get; set; } = new List<CommonErrorInfo>();

        /// <summary>
        /// 错误趋势
        /// </summary>
        public ErrorTrend ErrorTrend { get; set; }
    }

    /// <summary>
    /// 常见错误信息
    /// </summary>
    public class CommonErrorInfo
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 出现次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 首次出现时间
        /// </summary>
        public DateTime FirstOccurrence { get; set; }

        /// <summary>
        /// 最后出现时间
        /// </summary>
        public DateTime LastOccurrence { get; set; }
    }

    /// <summary>
    /// 错误趋势
    /// </summary>
    public class ErrorTrend
    {
        /// <summary>
        /// 每小时错误数
        /// </summary>
        public int[] HourlyCounts { get; set; } = new int[24];

        /// <summary>
        /// 趋势方向
        /// </summary>
        public TrendDirection TrendDirection { get; set; } = TrendDirection.Stable;
    }

    /// <summary>
    /// 趋势方向
    /// </summary>
    public enum TrendDirection
    {
        /// <summary>
        /// 稳定
        /// </summary>
        Stable,

        /// <summary>
        /// 增加
        /// </summary>
        Increasing,

        /// <summary>
        /// 减少
        /// </summary>
        Decreasing
    }
}