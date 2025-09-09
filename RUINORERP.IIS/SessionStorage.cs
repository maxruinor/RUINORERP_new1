using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.IIS
{
    public class SessionStorage
    {
        // 使用带过期时间的会话存储
        private static readonly Dictionary<string, (DateTime expires, object userState)> storage = new Dictionary<string, (DateTime, object)>();
        private static readonly object lockObject = new object();

        // 添加定时清理过期会话
        private static System.Threading.Timer cleanupTimer;

        static SessionStorage()
        {
            // 每5分钟清理一次过期会话
            cleanupTimer = new System.Threading.Timer(CleanUpSessions, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        public static string Add(string sessionId, object state, int timeoutMinutes)
        {
            lock (lockObject)
            {
                storage[sessionId] = (DateTime.UtcNow.AddMinutes(timeoutMinutes), state);
                return sessionId;
            }
        }

        public static bool Exists(string sessionId)
        {
            lock (lockObject)
            {
                if (storage.TryGetValue(sessionId, out var session))
                {
                    // 检查会话是否过期
                    if (session.expires > DateTime.UtcNow)
                    {
                        return true;
                    }
                    else
                    {
                        // 会话已过期，删除它
                        storage.Remove(sessionId);
                        return false;
                    }
                }
                return false;
            }
        }

        public static object Get(string sessionId)
        {
            lock (lockObject)
            {
                if (storage.TryGetValue(sessionId, out var session))
                {
                    // 检查会话是否过期
                    if (session.expires > DateTime.UtcNow)
                    {
                        return session.userState;
                    }
                    else
                    {
                        // 会话已过期，删除它
                        storage.Remove(sessionId);
                    }
                }
                return null;
            }
        }

        public static void Remove(string sessionId)
        {
            lock (lockObject)
            {
                storage.Remove(sessionId);
            }
        }

        public static void CleanExpiredSessions()
        {
            lock (lockObject)
            {
                var expiredKeys = storage.Where(kvp => kvp.Value.expires <= DateTime.UtcNow)
                                         .Select(kvp => kvp.Key)
                                         .ToList();
                                         
                foreach (var key in expiredKeys)
                {
                    storage.Remove(key);
                }
            }
        }

        /// <summary>
        /// 定期清理过期会话
        /// </summary>
        private static void CleanUpSessions(object state)
        {
            CleanExpiredSessions();
        }

        // 获取当前活跃会话数量
        public static int GetActiveSessionCount()
        {
            lock (lockObject)
            {
                return storage.Count(kvp => kvp.Value.expires > DateTime.UtcNow);
            }
        }

        // 获取所有会话ID（用于监控）
        public static List<string> GetAllSessionIds()
        {
            lock (lockObject)
            {
                return storage.Where(kvp => kvp.Value.expires > DateTime.UtcNow)
                              .Select(kvp => kvp.Key)
                              .ToList();
            }
        }
    }
}