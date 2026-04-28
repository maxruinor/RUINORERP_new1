using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 心跳包批量处理器
    /// 用于批量处理多个心跳请求，减少系统开销
    /// </summary>
    public class HeartbeatBatchProcessor : IDisposable
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<HeartbeatBatchProcessor> _logger;
        
        // 批量处理队列
        private readonly ConcurrentQueue<HeartbeatRequestInfo> _heartbeatQueue;
        private readonly Timer _batchTimer;
        
        // 批处理配置
        private const int BATCH_SIZE = 10; // 每批处理10个心跳请求
        private const int BATCH_INTERVAL_MS = 100; // 100毫秒批量处理间隔
        
        private bool _disposed = false;
        private readonly object _lockObject = new object();

        /// <summary>
        /// 心跳请求信息
        /// </summary>
        private class HeartbeatRequestInfo
        {
            public HeartbeatRequest Request { get; set; }
            public string SessionId { get; set; }
            public DateTime EnqueueTime { get; set; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatBatchProcessor(
            ISessionService sessionService,
            ILogger<HeartbeatBatchProcessor> logger)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _heartbeatQueue = new ConcurrentQueue<HeartbeatRequestInfo>();
            _batchTimer = new Timer(ProcessBatch, null, TimeSpan.FromMilliseconds(BATCH_INTERVAL_MS), 
                                  TimeSpan.FromMilliseconds(BATCH_INTERVAL_MS));
            
            _logger.LogInformation("心跳批量处理器初始化完成");
        }

        /// <summary>
        /// 添加心跳请求到批量处理队列
        /// </summary>
        /// <param name="request">心跳请求</param>
        /// <param name="sessionId">会话ID</param>
        public void EnqueueHeartbeat(HeartbeatRequest request, string sessionId)
        {
            if (_disposed)
                return;

            try
            {
                var requestInfo = new HeartbeatRequestInfo
                {
                    Request = request,
                    SessionId = sessionId,
                    EnqueueTime = DateTime.Now
                };
                
                _heartbeatQueue.Enqueue(requestInfo);
                
                _logger.LogDebug($"心跳请求已加入批量队列: SessionId={sessionId}, 队列长度={_heartbeatQueue.Count}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"添加心跳请求到批量队列失败: SessionId={sessionId}");
            }
        }

        /// <summary>
        /// 批量处理心跳请求
        /// </summary>
        private void ProcessBatch(object state)
        {
            if (_disposed)
                return;

            try
            {
                // 获取一批心跳请求
                var batch = DequeueBatch();
                if (!batch.Any())
                    return;

                _logger.LogDebug($"开始批量处理心跳请求，批次大小: {batch.Count}");

                var tasks = batch.Select(requestInfo => Task.Run(() =>
                {
                    try
                    {
                        ProcessSingleHeartbeat(requestInfo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"批量处理单个心跳请求请求失败: SessionId={requestInfo.SessionId}");
                    }
                }));

                Task.WhenAll(tasks).GetAwaiter().GetResult();

                _logger.LogDebug($"批量处理完成，处理了 {batch.Count} 个心跳请求");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "批量处理心跳请求时发生异常");
            }
        }

        /// <summary>
        /// 从队列中取出一批心跳请求
        /// </summary>
        private List<HeartbeatRequestInfo> DequeueBatch()
        {
            var batch = new List<HeartbeatRequestInfo>();
            
            for (int i = 0; i < BATCH_SIZE && _heartbeatQueue.TryDequeue(out var requestInfo); i++)
            {
                batch.Add(requestInfo);
            }
            
            return batch;
        }

        /// <summary>
        /// 处理单个心跳请求
        /// </summary>
        private void ProcessSingleHeartbeat(HeartbeatRequestInfo requestInfo)
        {
            try
            {
                var heartbeatRequest = requestInfo.Request;
                var sessionId = requestInfo.SessionId;

                // 获取会话信息
                var sessionInfo = _sessionService.GetSession(sessionId);
                if (sessionInfo == null)
                {
                    _logger.LogWarning($"会话不存在，无法处理心跳: SessionId={sessionId}");
                    return;
                }

                // 验证心跳请求
                if (!heartbeatRequest.IsValid())
                {
                    _logger.LogWarning($"心跳请求无效: SessionId={sessionId}");
                    return;
                }

                // 更新会话活动时间
                sessionInfo.LastActivityTime = DateTime.Now;
                sessionInfo.LastHeartbeat = DateTime.Now;

                // 如果有用户操作信息，更新会话
                if (heartbeatRequest.UserOperationInfo != null)
                {
                    UpdateSessionWithUserInfo(sessionInfo, heartbeatRequest.UserOperationInfo);
                    
                    // 如果心跳请求中包含计算机名，也更新到会话信息
                    if (!string.IsNullOrEmpty(heartbeatRequest.ComputerName))
                    {
                        sessionInfo.UserInfo.MachineName = heartbeatRequest.ComputerName;
                    }
                }
                else
                {
                    // 即使没有UserOperationInfo，如果有ComputerName也要更新
                    if (!string.IsNullOrEmpty(heartbeatRequest.ComputerName))
                    {
                        sessionInfo.UserInfo.MachineName = heartbeatRequest.ComputerName;
                    }
                }

                // 轻量级更新会话
                _sessionService.UpdateSessionLight(sessionInfo);

                _logger.LogDebug($"心跳处理完成: SessionId={sessionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理单个心跳请求失败: SessionId={requestInfo.SessionId}");
            }
        }

        /// <summary>
        /// 更新会话用户信息
        /// </summary>
        private void UpdateSessionWithUserInfo(SessionInfo sessionInfo, UserOperationInfo userInfo)
        {
            if (sessionInfo?.UserInfo == null || userInfo == null)
                return;

            // 批量更新用户信息

            sessionInfo.UserInfo.UserName = userInfo.UserName;
            sessionInfo.UserInfo.DisplayName = userInfo.DisplayName;
            sessionInfo.UserInfo.CurrentModule = userInfo.CurrentModule;
            sessionInfo.UserInfo.CurrentForm = userInfo.CurrentForm;
            sessionInfo.UserInfo.LoginTime = userInfo.LoginTime;
            sessionInfo.UserInfo.HeartbeatCount = userInfo.HeartbeatCount;
            sessionInfo.UserInfo.ClientVersion = userInfo.ClientVersion;
            sessionInfo.UserInfo.ClientIp = userInfo.ClientIp;
            sessionInfo.UserInfo.IdleTime = userInfo.IdleTime;
            sessionInfo.UserInfo.IsSuperUser = userInfo.IsSuperUser;
            sessionInfo.UserInfo.IsAuthorized = userInfo.IsAuthorized;
            sessionInfo.UserInfo.OperatingSystem = userInfo.OperatingSystem ;
            sessionInfo.UserInfo.MachineName = userInfo.MachineName;
            sessionInfo.UserInfo.CpuInfo = userInfo.CpuInfo;
            sessionInfo.UserInfo.MemorySize = userInfo.MemorySize;
        }

        /// <summary>
        /// 获取队列统计信息
        /// </summary>
        public HeartbeatQueueStats GetQueueStats()
        {
            return new HeartbeatQueueStats
            {
                QueueLength = _heartbeatQueue.Count,
                IsProcessing = false, // 简化实现
                LastProcessTime = DateTime.Now
            };
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void ClearQueue()
        {
            while (_heartbeatQueue.TryDequeue(out _))
            {
                // 清空队列
            }
            _logger.LogInformation("心跳处理队列已清空");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                _batchTimer?.Dispose();
                ClearQueue();
                _logger.LogInformation("心跳批量处理器已释放");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放心跳批量处理器时发生异常");
            }
        }
    }

    /// <summary>
    /// 心跳队列统计信息
    /// </summary>
    public class HeartbeatQueueStats
    {
        /// <summary>
        /// 队列长度
        /// </summary>
        public int QueueLength { get; set; }

        /// <summary>
        /// 是否正在处理
        /// </summary>
        public bool IsProcessing { get; set; }

        /// <summary>
        /// 最后处理时间
        /// </summary>
        public DateTime LastProcessTime { get; set; }
    }
}