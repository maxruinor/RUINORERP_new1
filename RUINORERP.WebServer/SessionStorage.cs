using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WebServer
{
    public class SessionStorage
    {
     

        private static readonly Dictionary<string, (DateTime expires, object userState)> storage = new Dictionary<string, (DateTime, object)>();
        private static readonly object lockObject = new object();

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
                    return session.expires > DateTime.UtcNow;
                }
                return false;
            }
        }

        public static object Get(string sessionId)
        {
            lock (lockObject)
            {
                if (storage.TryGetValue(sessionId, out var session) && session.expires > DateTime.UtcNow)
                {
                    return session.userState;
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
                var keysToRemove = storage.Where(kvp => kvp.Value.expires <= DateTime.UtcNow).Select(kvp => kvp.Key).ToList();
                foreach (var key in keysToRemove)
                {
                    storage.Remove(key);
                }
            }
        }


        System.Threading.Timer timer = new System.Threading.Timer(CleanUpSessions, null, 0, 60000); // 每分钟清理一次
        private static void CleanUpSessions(object? state)
        {
            /// <summary>
            /// 定期清理过期会话
            /// </summary>
            SessionStorage.CleanExpiredSessions();
        }

    }
}
