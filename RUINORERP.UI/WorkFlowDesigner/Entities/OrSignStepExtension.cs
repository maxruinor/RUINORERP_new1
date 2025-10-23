using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RUINORERP.UI.WorkFlowDesigner.Entities
{
    /// <summary>
    /// 或签步骤扩展属性 - 包含超时处理和通知机制
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OrSignStepExtension
    {
        private int _timeoutHours; // 超时时间（小时）
        private string _timeoutAction; // 超时后的处理动作 (自动通过/自动拒绝/提醒上级等)
        private List<string> _notificationEmails; // 通知邮箱列表
        private bool _enableNotifications; // 是否启用通知

        public OrSignStepExtension()
        {
            _timeoutHours = 24; // 默认24小时超时
            _timeoutAction = "Remind"; // 默认超时动作为提醒
            _notificationEmails = new List<string>();
            _enableNotifications = false;
        }

        [JsonProperty("TimeoutHours")]
        public int TimeoutHours
        {
            get { return _timeoutHours; }
            set { _timeoutHours = value; }
        }

        [JsonProperty("TimeoutAction")]
        public string TimeoutAction
        {
            get { return _timeoutAction; }
            set { _timeoutAction = value; }
        }

        [JsonProperty("NotificationEmails")]
        public List<string> NotificationEmails
        {
            get { return _notificationEmails; }
            set { _notificationEmails = value; }
        }

        [JsonProperty("EnableNotifications")]
        public bool EnableNotifications
        {
            get { return _enableNotifications; }
            set { _enableNotifications = value; }
        }

        /// <summary>
        /// 添加通知邮箱
        /// </summary>
        /// <param name="email"></param>
        public void AddNotificationEmail(string email)
        {
            // TODO: 添加邮箱格式验证
            if (!_notificationEmails.Contains(email))
            {
                _notificationEmails.Add(email);
            }
        }

        /// <summary>
        /// 移除通知邮箱
        /// </summary>
        /// <param name="email"></param>
        public void RemoveNotificationEmail(string email)
        {
            _notificationEmails.Remove(email);
        }
    }
}