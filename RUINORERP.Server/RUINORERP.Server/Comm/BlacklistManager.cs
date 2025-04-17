using RUINORERP.Server.Comm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

public class BlacklistManager : INotifyCollectionChanged
{
    private static readonly BindingList<BlacklistEntry> _bannedList = new BindingList<BlacklistEntry>();
    private static readonly ConcurrentDictionary<string, DateTime> _bannedIPs = new ConcurrentDictionary<string, DateTime>();
    private static SynchronizationContext _uiContext;

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    // 使用 BindingList 替代 ObservableCollection 更适合 WinForms
    public static BindingList<BlacklistEntry> BannedList => _bannedList;

    public static void Initialize(SynchronizationContext context)
    {
        _uiContext = context ?? throw new ArgumentNullException(nameof(context));
        _bannedList.RaiseListChangedEvents = true;
    }

    public static void BanIp(string ip, TimeSpan duration)
    {
        var expiry = DateTime.Now.Add(duration);
        _bannedIPs.AddOrUpdate(ip, expiry, (_, _) => expiry);

        UpdateUiCollection(ip, expiry);
    }

    private static void UpdateUiCollection(string ip, DateTime expiry)
    {
        if (_uiContext == null) return;

        void UpdateAction()
        {
            var existing = _bannedList.FirstOrDefault(x => x.IP地址 == ip);
            if (existing != null)
            {
                existing.解封时间 = expiry;
            }
            else
            {
                _bannedList.Add(new BlacklistEntry { IP地址 = ip, 解封时间 = expiry });
            }
        }

        if (_uiContext == SynchronizationContext.Current)
        {
            UpdateAction();
        }
        else
        {
            _uiContext.Send(_ => UpdateAction(), null);
        }
    }

    
    
    public static bool IsIpBanned(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
            return false;

        return _bannedIPs.TryGetValue(ip, out var expiryTime) && expiryTime > DateTime.Now;
    }

    public static void UnbanIp(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
            return;

        _bannedIPs.TryRemove(ip, out _);

        if (_uiContext == null)
            throw new InvalidOperationException("UI context not initialized. Call Initialize() first.");

        if (_uiContext == SynchronizationContext.Current)
        {
            RemoveFromCollectionDirect(ip);
        }
        else
        {
            _uiContext.Post(_ => RemoveFromCollectionDirect(ip), null);
        }
    }

    private static void RemoveFromCollectionDirect(string ip)
    {
        var item = _bannedList.FirstOrDefault(x => x.IP地址 == ip);
        if (item != null)
        {
            _bannedList.Remove(item);
        }
    }

    public static void CleanupExpiredBans()
    {
        var now = DateTime.Now;
        var expiredIps = _bannedIPs.Where(kvp => kvp.Value <= now).Select(kvp => kvp.Key).ToList();

        foreach (var ip in expiredIps)
        {
            _bannedIPs.TryRemove(ip, out _);
            if (_uiContext != null)
            {
                if (_uiContext == SynchronizationContext.Current)
                {
                    RemoveFromCollectionDirect(ip);
                }
                else
                {
                    _uiContext.Post(_ => RemoveFromCollectionDirect(ip), null);
                }
            }
        }
    }
}