using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 通知实体
    /// </summary>
    public class Notification
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public int RetryCount { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
    }

    /// <summary>
    /// 通知队列
    /// </summary>
    public class NotificationQueue
    {
        private List<Notification> _notifications = new List<Notification>();

        public void AddNotification(Notification notification)
        {
            lock (_notifications)
            {
                _notifications.Add(notification);
            }
        }

        public List<Notification> GetNotificationsForUser(string userId)
        {
            lock (_notifications)
            {
                return _notifications.Where(n => n.UserId == userId && n.Status == "Pending").ToList();
            }
        }

        public void UpdateNotificationStatus(string notificationId, string newStatus)
        {
            lock (_notifications)
            {
                var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
                if (notification != null)
                {
                    notification.Status = newStatus;
                }
            }
        }

        public void RemoveSentNotification(string notificationId)
        {
            lock (_notifications)
            {
                _notifications.RemoveAll(n => n.Id == notificationId && n.Status == "Sent");
            }
        }

    }
}
