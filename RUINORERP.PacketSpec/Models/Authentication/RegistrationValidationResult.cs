using RUINORERP.Model;
using System;

namespace RUINORERP.PacketSpec.Models.Authentication
{
    /// <summary>
    /// 注册验证结果
    /// </summary>
    [Serializable]
    public class RegistrationValidationResult
    {
        /// <summary>
        /// 验证是否通过
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 注册状态
        /// </summary>
        public RegistrationStatus Status { get; set; }

        /// <summary>
        /// 验证失败原因
        /// </summary>
        public string FailureReason { get; set; }

        /// <summary>
        /// 需要显示的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否需要显示警告
        /// </summary>
        public bool NeedsWarning { get; set; }

        /// <summary>
        /// 到期提醒信息
        /// </summary>
        public ExpirationReminder ExpirationReminder { get; set; }

        /// <summary>
        /// 注册信息
        /// </summary>
        public tb_sys_RegistrationInfo RegistrationInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RegistrationValidationResult()
        {
            IsValid = false;
            Status = RegistrationStatus.Expired;
            FailureReason = string.Empty;
            Message = string.Empty;
            NeedsWarning = false;
            ExpirationReminder = new ExpirationReminder();
            RegistrationInfo = null;
        }

        /// <summary>
        /// 创建成功的验证结果
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>验证结果</returns>
        public static RegistrationValidationResult CreateSuccess(tb_sys_RegistrationInfo registrationInfo)
        {
            return new RegistrationValidationResult
            {
                IsValid = true,
                Status = RegistrationStatus.Normal,
                FailureReason = string.Empty,
                Message = string.Empty,
                NeedsWarning = false,
                ExpirationReminder = new ExpirationReminder(),
                RegistrationInfo = registrationInfo
            };
        }

        /// <summary>
        /// 创建即将到期的验证结果
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <param name="daysRemaining">剩余天数</param>
        /// <returns>验证结果</returns>
        public static RegistrationValidationResult CreateExpiringSoon(tb_sys_RegistrationInfo registrationInfo, int daysRemaining)
        {
            return new RegistrationValidationResult
            {
                IsValid = true,
                Status = RegistrationStatus.ExpiringSoon,
                FailureReason = string.Empty,
                Message = $"系统注册将在 {daysRemaining} 天后到期，到期时间：{registrationInfo.ExpirationDate:yyyy-MM-dd}",
                NeedsWarning = true,
                ExpirationReminder = ExpirationReminder.Create(daysRemaining, registrationInfo.ExpirationDate),
                RegistrationInfo = registrationInfo
            };
        }

        /// <summary>
        /// 创建已过期的验证结果
        /// </summary>
        /// <param name="registrationInfo">注册信息</param>
        /// <returns>验证结果</returns>
        public static RegistrationValidationResult CreateExpired(tb_sys_RegistrationInfo registrationInfo)
        {
            return new RegistrationValidationResult
            {
                IsValid = false,
                Status = RegistrationStatus.Expired,
                FailureReason = "系统注册许可已过期",
                Message = "系统注册许可已过期，请联系软件提供商续期。",
                NeedsWarning = false,
                ExpirationReminder = new ExpirationReminder(),
                RegistrationInfo = registrationInfo
            };
        }

        /// <summary>
        /// 创建未注册的验证结果
        /// </summary>
        /// <returns>验证结果</returns>
        public static RegistrationValidationResult CreateNotRegistered()
        {
            return new RegistrationValidationResult
            {
                IsValid = false,
                Status = RegistrationStatus.Expired,
                FailureReason = "系统未注册",
                Message = "系统未注册，请先进行系统注册。",
                NeedsWarning = false,
                ExpirationReminder = new ExpirationReminder(),
                RegistrationInfo = null
            };
        }

        /// <summary>
        /// 创建验证失败的验证结果
        /// </summary>
        /// <param name="reason">失败原因</param>
        /// <returns>验证结果</returns>
        public static RegistrationValidationResult CreateValidationFailed(string reason)
        {
            return new RegistrationValidationResult
            {
                IsValid = false,
                Status = RegistrationStatus.Expired,
                FailureReason = reason,
                Message = reason,
                NeedsWarning = false,
                ExpirationReminder = new ExpirationReminder(),
                RegistrationInfo = null
            };
        }

        /// <summary>
        /// 创建用户数配置无效的验证结果
        /// </summary>
        /// <returns>验证结果</returns>
        public static RegistrationValidationResult CreateInvalidUserCount()
        {
            return new RegistrationValidationResult
            {
                IsValid = false,
                Status = RegistrationStatus.Expired,
                FailureReason = "注册许可的并发用户数配置无效",
                Message = "注册许可的并发用户数配置无效。",
                NeedsWarning = false,
                ExpirationReminder = new ExpirationReminder(),
                RegistrationInfo = null
            };
        }
    }
}
