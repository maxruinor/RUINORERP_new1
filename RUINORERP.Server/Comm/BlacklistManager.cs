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
    private static readonly HashSet<string> _allowedIPs = new HashSet<string>();
    private static readonly List<IpRange> _allowedRanges = new List<IpRange>();
    private static SynchronizationContext _uiContext;

    public event NotifyCollectionChangedEventHandler CollectionChanged;
    public static BindingList<BlacklistEntry> BannedList => _bannedList;

    public static void Initialize(SynchronizationContext context)
    {
        _uiContext = context ?? throw new ArgumentNullException(nameof(context));
        _bannedList.RaiseListChangedEvents = true;
        InitializeAllowedIpRanges();
    }

    /// <summary>
    /// 初始化允许的IP段（内网IP段）
    /// </summary>
    private static void InitializeAllowedIpRanges()
    {
        _allowedRanges.Add(new IpRange("10.0.0.0", "10.255.255.255"));
        _allowedRanges.Add(new IpRange("172.16.0.0", "172.31.255.255"));
        _allowedRanges.Add(new IpRange("192.168.0.0", "192.168.255.255"));
        _allowedRanges.Add(new IpRange("127.0.0.0", "127.255.255.255"));
        _allowedRanges.Add(new IpRange("169.254.0.0", "169.254.255.255"));
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
            throw new InvalidOperationException($"无法解除IP地址[{ip}]的封禁: UI上下文未初始化。请在使用黑名单管理功能前先调用Initialize()方法初始化UI上下文。");

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
    
    // 添加缺失的方法
    public static List<BlacklistEntry> GetBlacklistEntries()
    {
        return _bannedList.ToList();
    }
    
    public static void AddIpToBlacklist(string ipAddress, string reason)
    {
        // 添加IP到黑名单，这里简单实现，实际可能需要保存原因
        BanIp(ipAddress, TimeSpan.FromDays(30)); // 默认封禁30天
    }
    
    public static void RemoveFromBlacklist(string ipAddress)
    {
        UnbanIp(ipAddress);
    }
    
    public static void ClearBlacklist()
    {
        var ips = _bannedList.Select(x => x.IP地址).ToList();
        foreach (var ip in ips)
        {
            UnbanIp(ip);
        }
    }

    /// <summary>
    /// 添加允许的IP（白名单）
    /// </summary>
    public static void AddAllowedIp(string ip)
    {
        if (!string.IsNullOrWhiteSpace(ip))
            _allowedIPs.Add(ip);
    }

    /// <summary>
    /// 检查IP是否在允许列表中（内网或白名单）
    /// </summary>
    public static bool IsIpAllowed(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
            return false;

        if (_allowedIPs.Contains(ip))
            return true;

        foreach (var range in _allowedRanges)
        {
            if (range.Contains(ip))
                return true;
        }

        return false;
    }
}

/// <summary>
/// IP范围类
/// </summary>
public class IpRange
{
    private readonly uint _start;
    private readonly uint _end;

    public IpRange(string startIp, string endIp)
    {
        _start = IpToUInt(startIp);
        _end = IpToUInt(endIp);
    }

    public bool Contains(string ip)
    {
        uint ipUInt = IpToUInt(ip);
        return ipUInt >= _start && ipUInt <= _end;
    }

    private static uint IpToUInt(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
            return 0;

        if (!System.Net.IPAddress.TryParse(ip, out var ipAddress))
            return 0;

        byte[] bytes = ipAddress.GetAddressBytes();
        if (bytes.Length != 4)
            return 0;

        uint result = 0;
        foreach (var b in bytes)
        {
            result = (result << 8) | b;
        }
        return result;
    }
}