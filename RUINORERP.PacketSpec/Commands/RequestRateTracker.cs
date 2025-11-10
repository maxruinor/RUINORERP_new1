using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 处理异步命令（优化版：分层解析）
    /// </summary>
    /// <param name="cmd">命令对象</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>处理结果</returns>
    /// <summary>
    /// 请求频率跟踪器 - 用于检测高频相同参数请求
    /// </summary>
    public class RequestRateTracker
    {
        private readonly ConcurrentDictionary<string, Queue<DateTime>> _sessionRequests = new();
        private readonly int _thresholdCount = 10; // 阈值：10秒内的请求数
        private readonly TimeSpan _timeWindow = TimeSpan.FromSeconds(10); // 时间窗口：10秒

        /// <summary>
        /// 检查是否为高频请求
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>如果是高频请求返回true，否则返回false</returns>
        public bool IsFrequentRequest(string sessionId)
        {
            var queue = _sessionRequests.GetOrAdd(sessionId, _ => new Queue<DateTime>());
            lock (queue)
            {
                var now = DateTime.Now;
                // 添加当前请求时间
                queue.Enqueue(now);

                // 移除时间窗口外的请求记录
                while (queue.Count > 0 && now - queue.Peek() > _timeWindow)
                {
                    queue.Dequeue();
                }

                // 检查是否超过阈值
                return queue.Count > _thresholdCount;
            }
        }
    }
}
