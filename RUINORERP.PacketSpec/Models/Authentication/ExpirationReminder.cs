using System;

namespace RUINORERP.PacketSpec.Models.Authentication
{
    /// <summary>
    /// 到期提醒数据模型
    /// </summary>
    [Serializable]
    public class ExpirationReminder
    {
        /// <summary>
        /// 是否需要提醒
        /// </summary>
        public bool NeedsReminder { get; set; }

        /// <summary>
        /// 剩余天数
        /// </summary>
        public int DaysRemaining { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 提醒消息
        /// </summary>
        public string ReminderMessage { get; set; }

        /// <summary>
        /// 续费方式
        /// </summary>
        public string RenewalMethod { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExpirationReminder()
        {
            NeedsReminder = false;
            DaysRemaining = 0;
            ExpirationDate = DateTime.MinValue;
            ReminderMessage = string.Empty;
            RenewalMethod = string.Empty;
            ContactInfo = string.Empty;
        }

        /// <summary>
        /// 创建到期提醒
        /// </summary>
        /// <param name="daysRemaining">剩余天数</param>
        /// <param name="expirationDate">到期日期</param>
        /// <param name="renewalMethod">续费方式</param>
        /// <param name="contactInfo">联系信息</param>
        /// <returns>到期提醒实例</returns>
        public static ExpirationReminder Create(int daysRemaining, DateTime expirationDate, 
            string renewalMethod = "请联系软件提供商", string contactInfo = "")
        {
            return new ExpirationReminder
            {
                NeedsReminder = true,
                DaysRemaining = daysRemaining,
                ExpirationDate = expirationDate,
                ReminderMessage = $"系统注册将在 {daysRemaining} 天后到期，到期时间：{expirationDate:yyyy-MM-dd}",
                RenewalMethod = renewalMethod,
                ContactInfo = contactInfo
            };
        }
    }
}
