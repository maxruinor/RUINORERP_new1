using System;

namespace RUINORERP.Server.Configuration
{
    /// <summary>
    /// 注册提醒配置类
    /// 用于配置注册到期提醒相关参数
    /// </summary>
    public class RegistrationReminderConfig
    {
        /// <summary>
        /// 提醒提前天数（默认30天）
        /// </summary>
        public int ReminderDays { get; set; } = 30;

        /// <summary>
        /// 到期提醒工作流执行时间（默认上午10点）
        /// </summary>
        public string ReminderWorkflowTime { get; set; } = "10:00";

        /// <summary>
        /// 注册信息更新工作流执行时间（默认晚上11点）
        /// </summary>
        public string UpdateWorkflowTime { get; set; } = "23:00";

        /// <summary>
        /// 是否启用到期提醒（默认启用）
        /// </summary>
        public bool EnableExpirationReminder { get; set; } = true;

        /// <summary>
        /// 是否启用注册信息自动更新（默认启用）
        /// </summary>
        public bool EnableAutoUpdate { get; set; } = true;

        /// <summary>
        /// 续费方式说明
        /// </summary>
        public string RenewalMethod { get; set; } = "请联系软件提供商";

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; } = "";

        /// <summary>
        /// 获取到期提醒工作流执行时间
        /// </summary>
        /// <returns>执行时间</returns>
        public DateTime GetReminderWorkflowExecutionTime()
        {
            var timeParts = ReminderWorkflowTime.Split(':');
            var hour = int.Parse(timeParts[0]);
            var minute = int.Parse(timeParts[1]);
            return DateTime.Today.AddHours(hour).AddMinutes(minute);
        }

        /// <summary>
        /// 获取注册信息更新工作流执行时间
        /// </summary>
        /// <returns>执行时间</returns>
        public DateTime GetUpdateWorkflowExecutionTime()
        {
            var timeParts = UpdateWorkflowTime.Split(':');
            var hour = int.Parse(timeParts[0]);
            var minute = int.Parse(timeParts[1]);
            return DateTime.Today.AddHours(hour).AddMinutes(minute);
        }

        /// <summary>
        /// 验证配置有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            // 验证提醒天数
            if (ReminderDays <= 0 || ReminderDays > 365)
            {
                return false;
            }

            // 验证时间格式
            if (!IsValidTimeFormat(ReminderWorkflowTime) || !IsValidTimeFormat(UpdateWorkflowTime))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 验证时间格式
        /// </summary>
        /// <param name="time">时间字符串</param>
        /// <returns>是否有效</returns>
        private bool IsValidTimeFormat(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                return false;
            }

            var parts = time.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int hour) || !int.TryParse(parts[1], out int minute))
            {
                return false;
            }

            if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
                return false;
            }

            return true;
        }
    }
}
