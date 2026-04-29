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

/// <summary>
/// IP黑名单管理器
/// ⚠️ NAT环境重要提示：
/// 1. 外网环境下，多台客户端通过路由器/NAT上网时会共享同一公网IP
/// 2. 封禁某个外网IP会导致该IP下的所有正常用户无法访问系统
/// 3. 建议优先使用用户名级别的限制（如登录次数限制），谨慎使用IP封禁功能
/// 4. 仅在确认是恶意攻击且已验证不会影响正常用户时，才使用IP封禁
/// 
/// 适用场景：
/// - 内网环境：可以安全使用IP封禁（每个设备有独立IP）
/// - 外网环境：需要特别谨慎，建议结合用户名+IP进行综合判断
/// </summary>
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

    /// <summary>
    /// 封禁IP地址
    /// ⚠️ NAT环境警告：外网环境下多台客户端可能共享同一公网IP（通过路由器/NAT）
    /// 封禁某个IP会影响该IP下的所有正常用户，请谨慎使用！
    /// 建议优先使用用户名级别的限制，仅在确认是恶意攻击时才使用IP封禁
    /// </summary>
    public static void BanIp(string ip, TimeSpan duration)
    {
        BanIp(ip, duration, "系统自动封禁");
    }

    /// <summary>
    /// 封禁IP地址（带原因说明）
    /// ⚠️ NAT环境警告：外网环境下多台客户端可能共享同一公网IP（通过路由器/NAT）
    /// 封禁某个IP会影响该IP下的所有正常用户，请谨慎使用！
    /// 建议优先使用用户名级别的限制，仅在确认是恶意攻击时才使用IP封禁
    /// </summary>
    public static void BanIp(string ip, TimeSpan duration, string reason)
    {
        var expiry = DateTime.Now.Add(duration);
        _bannedIPs.AddOrUpdate(ip, expiry, (_, _) => expiry);

        UpdateUiCollection(ip, expiry, reason);
    }

    private static void UpdateUiCollection(string ip, DateTime expiry, string reason = "系统自动封禁")
    {
        if (_uiContext == null) return;

        void UpdateAction()
        {
            var existing = _bannedList.FirstOrDefault(x => x.IP地址 == ip);
            if (existing != null)
            {
                existing.解封时间 = expiry;
                existing.Reason = reason;
            }
            else
            {
                _bannedList.Add(new BlacklistEntry 
                { 
                    IP地址 = ip, 
                    解封时间 = expiry, 
                    BanTime = DateTime.Now,
                    Reason = reason 
                });
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
    
    // 获取所有黑名单条目
    public static List<BlacklistEntry> GetBlacklistEntries()
    {
        return _bannedList.ToList();
    }
    
    public static void AddIpToBlacklist(string ipAddress, string reason)
    {
        // 添加IP到黑名单，保存封禁原因
        BanIp(ipAddress, TimeSpan.FromDays(30), string.IsNullOrEmpty(reason) ? "手动添加" : reason);
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