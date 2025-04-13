using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Threading;
    using System.Windows.Forms;

    public  class BlacklistManager : INotifyCollectionChanged
    {
    
        public static readonly ObservableCollection<BlacklistEntry> BannedList = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private static readonly ConcurrentDictionary<string, DateTime> _bannedIPs = new ConcurrentDictionary<string, DateTime>();

        // 添加对 UI 同步上下文的支持
        private static SynchronizationContext _uiContext;
        public static void Initialize(SynchronizationContext context)
        {
            _uiContext = context;
        }

        // 封禁IP（带线程安全更新）
        public static void BanIp(string ip, TimeSpan duration)
        {
            var expiry = DateTime.Now.Add(duration);
            _bannedIPs.AddOrUpdate(ip, expiry, (_, _) => expiry);

            // 使用 WinForms 的线程安全更新方式
            _uiContext?.Post(_ =>
            {
                var existing = BannedList.FirstOrDefault(x => x.IP地址 == ip);
                if (existing != null)
                {
                    existing.解封时间 = expiry;
                }
                else
                {
                    BannedList.Add(new BlacklistEntry { IP地址 = ip, 解封时间 = expiry });
                }
            }, null);
        }


 

        // 检查IP是否被封禁
        public static bool IsIpBanned(string ip)
        {
            return _bannedIPs.TryGetValue(ip, out var expiryTime) && expiryTime > DateTime.Now;
        }




        // 解封IP（带线程安全更新）
        public static void UnbanIp(string ip)
        {
            _bannedIPs.TryRemove(ip, out _);
            _uiContext?.Post(_ =>
            {
                var item = BannedList.FirstOrDefault(x => x.IP地址 == ip);
                if (item != null) BannedList.Remove(item);
            }, null);
        }

        // 清理过期的封禁记录
        public static void CleanupExpiredBans()
        {
            foreach (var ip in _bannedIPs.Keys)
            {
                if (_bannedIPs.TryGetValue(ip, out var expiry) && expiry <= DateTime.Now)
                {
                    _bannedIPs.TryRemove(ip, out _);
                }
            }
        }
    }
}
