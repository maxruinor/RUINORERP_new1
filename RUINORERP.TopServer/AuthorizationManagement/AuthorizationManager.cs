using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.TopServer.ServerManagement;

namespace RUINORERP.TopServer.AuthorizationManagement
{
    /// <summary>
    /// 授权管理核心类
    /// 负责跟踪各客户服务器的授权状态、到期时间及授权限制
    /// </summary>
    public class AuthorizationManager
    {
        // 授权信息字典，键为服务器实例ID
        private ConcurrentDictionary<Guid, AuthorizationInfo> _authorizationInfos;
        // 授权到期提醒定时器
        private System.Timers.Timer _expiryReminderTimer;
        // 每日检查一次授权到期情况
        private const int EXPIRY_REMINDER_INTERVAL = 24 * 60 * 60 * 1000;

        /// <summary>
        /// 授权状态变化事件
        /// </summary>
        public event EventHandler<AuthorizationStatusChangedEventArgs> AuthorizationStatusChanged;

        /// <summary>
        /// 授权到期提醒事件
        /// </summary>
        public event EventHandler<AuthorizationExpiryReminderEventArgs> AuthorizationExpiryReminder;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthorizationManager()
        {
            _authorizationInfos = new ConcurrentDictionary<Guid, AuthorizationInfo>();

            // 初始化授权到期提醒定时器
            _expiryReminderTimer = new System.Timers.Timer(EXPIRY_REMINDER_INTERVAL);
            _expiryReminderTimer.Elapsed += OnExpiryReminderTimerElapsed;
            _expiryReminderTimer.AutoReset = true;
            _expiryReminderTimer.Start();
        }

        /// <summary>
        /// 获取服务器实例的授权信息
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <returns>授权信息</returns>
        public AuthorizationInfo GetAuthorizationInfo(Guid instanceId)
        {
            _authorizationInfos.TryGetValue(instanceId, out AuthorizationInfo authInfo);
            return authInfo;
        }

        /// <summary>
        /// 设置服务器实例的授权信息
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="authInfo">授权信息</param>
        public void SetAuthorizationInfo(Guid instanceId, AuthorizationInfo authInfo)
        {
            if (authInfo == null)
            {
                return;
            }

            // 保存旧状态
            var oldStatus = AuthorizationStatus.Invalid;
            if (_authorizationInfos.TryGetValue(instanceId, out AuthorizationInfo oldAuthInfo))
            {
                oldStatus = oldAuthInfo.Status;
            }

            // 添加或更新授权信息
            _authorizationInfos.AddOrUpdate(instanceId, authInfo, (key, oldValue) =>
            {
                // 更新现有授权信息
                oldValue.AuthorizationType = authInfo.AuthorizationType;
                oldValue.StartTime = authInfo.StartTime;
                oldValue.ExpireTime = authInfo.ExpireTime;
                oldValue.MaxUsers = authInfo.MaxUsers;
                oldValue.MaxTransactions = authInfo.MaxTransactions;
                oldValue.LicenseKey = authInfo.LicenseKey;
                oldValue.UpdateStatus();
                return oldValue;
            });

            // 获取更新后的授权信息
            _authorizationInfos.TryGetValue(instanceId, out AuthorizationInfo updatedAuthInfo);

            // 如果状态发生变化，触发状态变化事件
            if (oldStatus != updatedAuthInfo.Status)
            {
                AuthorizationStatusChanged?.Invoke(this, new AuthorizationStatusChangedEventArgs(updatedAuthInfo, oldStatus));
            }
        }

        /// <summary>
        /// 移除服务器实例的授权信息
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        public void RemoveAuthorizationInfo(Guid instanceId)
        {
            if (_authorizationInfos.TryRemove(instanceId, out AuthorizationInfo authInfo))
            {
                // 触发状态变化事件，状态变为无效
                AuthorizationStatusChanged?.Invoke(this, new AuthorizationStatusChangedEventArgs(authInfo, authInfo.Status));
            }
        }

        /// <summary>
        /// 验证服务器实例的授权
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <returns>验证结果</returns>
        public AuthorizationValidationResult ValidateAuthorization(Guid instanceId)
        {
            var result = new AuthorizationValidationResult();

            if (_authorizationInfos.TryGetValue(instanceId, out AuthorizationInfo authInfo))
            {
                // 更新授权状态
                authInfo.UpdateStatus();

                // 验证授权状态
                if (authInfo.Status == AuthorizationStatus.Valid)
                {
                    result.IsValid = true;
                    result.Message = "授权有效";
                }
                else
                {
                    result.IsValid = false;
                    result.Message = authInfo.Status == AuthorizationStatus.Expired ? "授权已过期" : "授权无效";
                }

                result.AuthorizationInfo = authInfo;
            }
            else
            {
                result.IsValid = false;
                result.Message = "未找到授权信息";
            }

            return result;
        }

        /// <summary>
        /// 验证服务器实例的授权
        /// </summary>
        /// <param name="instanceId">服务器实例ID</param>
        /// <param name="licenseKey">许可证密钥</param>
        /// <returns>验证结果</returns>
        public AuthorizationValidationResult ValidateAuthorization(Guid instanceId, string licenseKey)
        {
            var result = new AuthorizationValidationResult();

            if (_authorizationInfos.TryGetValue(instanceId, out AuthorizationInfo authInfo))
            {
                // 验证许可证密钥
                if (authInfo.LicenseKey != licenseKey)
                {
                    result.IsValid = false;
                    result.Message = "无效的许可证密钥";
                    return result;
                }

                // 更新授权状态
                authInfo.UpdateStatus();

                // 验证授权状态
                if (authInfo.Status == AuthorizationStatus.Valid)
                {
                    result.IsValid = true;
                    result.Message = "授权有效";
                }
                else
                {
                    result.IsValid = false;
                    result.Message = authInfo.Status == AuthorizationStatus.Expired ? "授权已过期" : "授权无效";
                }

                result.AuthorizationInfo = authInfo;
            }
            else
            {
                result.IsValid = false;
                result.Message = "未找到授权信息";
            }

            return result;
        }

        /// <summary>
        /// 更新所有授权信息的状态
        /// </summary>
        public void UpdateAllAuthorizationStatus()
        {
            foreach (var authInfo in _authorizationInfos.Values)
            {
                authInfo.UpdateStatus();
            }
        }

        /// <summary>
        /// 检查授权到期提醒
        /// </summary>
        private void CheckAuthorizationExpiry()
        {
            var now = DateTime.Now;
            foreach (var authInfo in _authorizationInfos.Values)
            {
                // 检查授权是否即将到期（7天内）
                if (authInfo.ExpireTime > now && (authInfo.ExpireTime - now).TotalDays <= 7)
                {
                    // 触发授权到期提醒事件
                    AuthorizationExpiryReminder?.Invoke(this, new AuthorizationExpiryReminderEventArgs(authInfo));
                }
            }
        }

        /// <summary>
        /// 授权到期提醒定时器事件处理
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void OnExpiryReminderTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckAuthorizationExpiry();
        }
    }

    /// <summary>
    /// 授权信息类
    /// </summary>
    public class AuthorizationInfo
    {
        /// <summary>
        /// 服务器实例ID
        /// </summary>
        public Guid InstanceId { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public AuthorizationType AuthorizationType { get; set; }

        /// <summary>
        /// 授权开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 授权到期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 最大用户数限制
        /// </summary>
        public int MaxUsers { get; set; }

        /// <summary>
        /// 最大事务数限制
        /// </summary>
        public int MaxTransactions { get; set; }

        /// <summary>
        /// 许可证密钥
        /// </summary>
        public string LicenseKey { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public AuthorizationStatus Status { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthorizationInfo()
        {
            StartTime = DateTime.Now;
            ExpireTime = DateTime.Now.AddYears(1); // 默认授权1年
            MaxUsers = 100; // 默认最大用户数100
            MaxTransactions = 1000000; // 默认最大事务数100万
            LicenseKey = GenerateLicenseKey();
            Status = AuthorizationStatus.Valid;
        }

        /// <summary>
        /// 生成许可证密钥
        /// </summary>
        /// <returns>许可证密钥</returns>
        private string GenerateLicenseKey()
        {
            return Guid.NewGuid().ToString().ToUpper().Replace("-", "");
        }

        /// <summary>
        /// 更新授权状态
        /// </summary>
        public void UpdateStatus()
        {
            var now = DateTime.Now;
            if (StartTime > now || ExpireTime < now)
            {
                Status = AuthorizationStatus.Expired;
            }
            else
            {
                Status = AuthorizationStatus.Valid;
            }
        }
    }

    /// <summary>
    /// 授权类型枚举
    /// </summary>
    public enum AuthorizationType
    {
        /// <summary>
        /// 试用版
        /// </summary>
        Trial = 0,
        /// <summary>
        /// 正式版
        /// </summary>
        Official = 1,
        /// <summary>
        /// 企业版
        /// </summary>
        Enterprise = 2,
        /// <summary>
        /// 定制版
        /// </summary>
        Custom = 3
    }

    /// <summary>
    /// 授权状态枚举
    /// </summary>
    public enum AuthorizationStatus
    {
        /// <summary>
        /// 无效
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 有效
        /// </summary>
        Valid = 1,
        /// <summary>
        /// 已过期
        /// </summary>
        Expired = 2
    }

    /// <summary>
    /// 授权验证结果类
    /// </summary>
    public class AuthorizationValidationResult
    {
        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 验证消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthorizationInfo AuthorizationInfo { get; set; }
    }

    /// <summary>
    /// 授权状态变化事件参数
    /// </summary>
    public class AuthorizationStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthorizationInfo AuthorizationInfo { get; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public AuthorizationStatus OldStatus { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorizationInfo">授权信息</param>
        /// <param name="oldStatus">旧状态</param>
        public AuthorizationStatusChangedEventArgs(AuthorizationInfo authorizationInfo, AuthorizationStatus oldStatus)
        {
            AuthorizationInfo = authorizationInfo;
            OldStatus = oldStatus;
        }
    }

    /// <summary>
    /// 授权到期提醒事件参数
    /// </summary>
    public class AuthorizationExpiryReminderEventArgs : EventArgs
    {
        /// <summary>
        /// 授权信息
        /// </summary>
        public AuthorizationInfo AuthorizationInfo { get; }

        /// <summary>
        /// 到期剩余天数
        /// </summary>
        public double DaysUntilExpiry { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorizationInfo">授权信息</param>
        public AuthorizationExpiryReminderEventArgs(AuthorizationInfo authorizationInfo)
        {
            AuthorizationInfo = authorizationInfo;
            DaysUntilExpiry = (authorizationInfo.ExpireTime - DateTime.Now).TotalDays;
        }
    }
}