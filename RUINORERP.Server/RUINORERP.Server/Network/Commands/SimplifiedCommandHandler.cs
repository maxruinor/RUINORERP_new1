using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// 为避免命名空间冲突，使用别名
using HandlerStatisticsType = RUINORERP.PacketSpec.Commands.HandlerStatistics;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 简化命令处理器基类
    /// 提供简化的命令处理方式，自动处理请求解析和响应构建
    /// </summary>
    /// <typeparam name="TRequest">请求数据类型</typeparam>
    /// <typeparam name="TResponse">响应数据类型</typeparam>
    public abstract class SimplifiedCommandHandler<TRequest, TResponse> : ICommandHandler
    {
        private bool _isInitialized = false;
        private HandlerStatus _status = HandlerStatus.Uninitialized;
        private HandlerStatisticsType _statistics = new HandlerStatisticsType();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// 构造函数
        /// </summary>
        protected SimplifiedCommandHandler()
        {
            _statistics = new HandlerStatisticsType();
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>API响应</returns>
        public async Task<ResponseBase> HandleAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            _stopwatch.Restart();
            
            try
            {
                // 更新统计信息
                _statistics.CurrentProcessingCount++;
                var startTime = DateTime.UtcNow;
                
                // 验证输入参数
                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }
                
                // 从数据包中解析请求数据
                var request = command.Packet.GetJsonData<TRequest>();
                
                // 添加空值检查
                if (request == null)
                {
                    // 记录错误信息到统计信息
                    var errorProcessingTime = _stopwatch.ElapsedMilliseconds;
                    UpdateStatistics(errorProcessingTime, false, new ArgumentNullException(nameof(request)));
                    
                    var errorResponse = ResponseBase.CreateError("无法解析请求数据")
                        .WithMetadata("ErrorCode", "INVALID_REQUEST_DATA");
                    return ConvertToApiResponse(errorResponse);
                }
                
                // 处理请求并获取响应
                var result = await ProcessRequestAsync(request, cancellationToken);
                
                // 添加空值检查
                if (result == null)
                {
                    // 记录信息到统计信息
                    var nullResultProcessingTime = _stopwatch.ElapsedMilliseconds;
                    UpdateStatistics(nullResultProcessingTime, true); // 仍然标记为成功，只是返回空数据
                    
                    // 创建成功的API响应，但数据为空
                    var successResponse = ResponseBase.CreateSuccess("请求处理成功，但无返回数据")
                        .WithMetadata("Data", null);
                    return ConvertToApiResponse(successResponse);
                }
                
                // 计算处理时间
                var endTime = DateTime.UtcNow;
                var processingTimeMs = (long)(endTime - startTime).TotalMilliseconds;
                
                // 更新统计信息
                UpdateStatistics(processingTimeMs, true);
                
                // 创建成功的API响应
                var response = ResponseBase.CreateSuccess("操作成功");
               
                // 将泛型数据添加到元数据中
                //if (response.Data != null)
                //{
                //    apiResponse.WithMetadata("Data", response.Data);
                //}
                return response;
            }
            catch (Exception ex)
            {
                // 记录错误信息到统计信息
                var exceptionProcessingTime = _stopwatch.ElapsedMilliseconds;
                
                // 更新统计信息
                UpdateStatistics(exceptionProcessingTime, false, ex);
                
                // 处理异常并创建错误响应
                var errorResponse = ResponseBase.CreateError($"处理请求时发生错误: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "REQUEST_PROCESSING_ERROR")
                    .WithMetadata("ExceptionType", ex.GetType().FullName);
                return ConvertToApiResponse(errorResponse);
            }
            finally
            {
                _stopwatch.Stop();
                _statistics.CurrentProcessingCount--;
            }
        }

        /// <summary>
        /// 处理请求的核心逻辑
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应数据</returns>
        protected abstract Task<TResponse> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化结果</returns>
        public virtual async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
        {
            if (_isInitialized)
                return true;

            try
            {
                _statistics.StartTime = DateTime.UtcNow;
                _status = HandlerStatus.Initialized;
                _isInitialized = true;
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _status = HandlerStatus.Error;
                // 记录初始化错误
                _statistics.LastError = ex.Message;
                _statistics.LastErrorTime = DateTime.UtcNow;
                return false;
            }
        }

        /// <summary>
        /// 启动处理器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>启动结果</returns>
        public virtual async Task<bool> StartAsync(CancellationToken cancellationToken = default)
        {
            if (!_isInitialized)
                return false;

            try
            {
                _status = HandlerStatus.Running;
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _status = HandlerStatus.Error;
                // 记录启动错误
                _statistics.LastError = ex.Message;
                _statistics.LastErrorTime = DateTime.UtcNow;
                return false;
            }
        }

        /// <summary>
        /// 停止处理器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>停止结果</returns>
        public virtual async Task<bool> StopAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _status = HandlerStatus.Stopped;
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _status = HandlerStatus.Error;
                // 记录停止错误
                _statistics.LastError = ex.Message;
                _statistics.LastErrorTime = DateTime.UtcNow;
                return false;
            }
        }

        /// <summary>
        /// 检查是否能处理指定命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>是否能处理</returns>
        public virtual bool CanHandle(ICommand command)
        {
            if (SupportedCommands == null || SupportedCommands.Count == 0)
                return true;

            return SupportedCommands.Contains(command.CommandIdentifier.FullCode);
        }

        /// <summary>
        /// 获取处理器名称
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// 获取处理器ID
        /// </summary>
        public virtual string HandlerId => GetType().FullName;

        /// <summary>
        /// 获取处理器优先级
        /// </summary>
        public virtual int Priority => 0;

        /// <summary>
        /// 获取是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// 获取处理器状态
        /// </summary>
        public virtual HandlerStatus Status => _status;

        /// <summary>
        /// 获取支持的命令列表
        /// </summary>
        public virtual IReadOnlyList<uint> SupportedCommands => new List<uint>();

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public virtual HandlerStatisticsType GetStatistics()
        {
            return _statistics;
        }

        /// <summary>
        /// 重置处理器统计信息
        /// </summary>
        public virtual void ResetStatistics()
        {
            _statistics = new HandlerStatisticsType
            {
                StartTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 更新统计信息
        /// </summary>
        /// <param name="processingTime">处理时间</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="exception">异常信息（可选）</param>
        private void UpdateStatistics(long processingTime, bool isSuccess, Exception exception = null)
        {
            _statistics.TotalCommandsProcessed++;
            
            if (isSuccess)
            {
                _statistics.SuccessfulCommands++;
            }
            else
            {
                _statistics.FailedCommands++;
                // 记录错误信息
                if (exception != null)
                {
                    _statistics.LastError = exception.Message;
                    _statistics.LastErrorTime = DateTime.UtcNow;
                    _statistics.LastErrorStackTrace = exception.StackTrace;
                    // 记录到日志
                    LogError($"命令处理失败: {exception.Message}", exception);
                }
            }
            
            if (processingTime > 0)
            {
                _statistics.LastProcessTime = DateTime.UtcNow;
                
                if (processingTime > _statistics.MaxProcessingTimeMs)
                    _statistics.MaxProcessingTimeMs = processingTime;
                
                if (_statistics.MinProcessingTimeMs == 0 || processingTime < _statistics.MinProcessingTimeMs)
                    _statistics.MinProcessingTimeMs = processingTime;
                
                // 更新平均处理时间
                _statistics.AverageProcessingTimeMs = ((_statistics.AverageProcessingTimeMs * (_statistics.TotalCommandsProcessed - 1)) + processingTime) / _statistics.TotalCommandsProcessed;
            }
            
            // 更新超时统计
            if (processingTime > 5000) // 超过5秒认为是超时
            {
                _statistics.TimeoutCount++;
                LogWarning($"命令处理超时: 处理时间 {processingTime}ms");
            }
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        protected void LogDebug(string message)
        {
            // 可以根据需要实现调试日志记录
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        protected void LogInfo(string message)
        {
            // 日志记录已在基类中实现
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        protected void LogWarning(string message)
        {
            // 可以根据需要实现警告日志记录
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        protected void LogError(string message, Exception ex = null)
        {
            // 错误日志记录已在基类中实现
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            _status = HandlerStatus.Disposed;
            // 默认实现不执行任何操作
        }

        /// <summary>
        /// 将ResponseBase转换为ApiResponse
        /// </summary>
        /// <param name="baseResponse">基础响应对象</param>
        /// <returns>ApiResponse对象</returns>
        private ResponseBase ConvertToApiResponse(ResponseBase baseResponse)
        {
            var response = new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                Code = baseResponse.Code,
                Timestamp = baseResponse.Timestamp,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata,
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }
}