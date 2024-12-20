using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.IM
{
    public interface INotificationSender
    {
        Task SendAsync(Notification notification);
    }
    /// <summary>
    /// 负责发送通知，并根据重试计数来决定是否重试
    /// </summary>
    public class NotificationService
    {
        private const int MaxRetryCount = 5;
        private readonly INotificationSender _notificationSender;
        private readonly IDbConnection _dbConnection;

        public NotificationService(INotificationSender notificationSender, IDbConnection dbConnection)
        {
            _notificationSender = notificationSender;
            _dbConnection = dbConnection;
        }

        //public void SendNotifications()
        //{
        //        var retryCount = notification.RetryCount;
        //        if (retryCount < MaxRetryCount)
        //        {
        //            try
        //            {
        //                bool success = _notificationSender.Send(notification);
        //                if (success)
        //                {
        //                    _dbConnection.Execute("UPDATE Notifications SET Status = 'Sent' WHERE Id = @Id", notification);
        //                }
        //                else
        //                {
        //                    _dbConnection.Execute("UPDATE Notifications SET RetryCount = RetryCount + 1 WHERE Id = @Id", notification);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log the exception
        //                // Update the notification status to 'Failed' or increment retry count
        //            }
        //        }
        //        else
        //        {
        //            // Set status to 'Failed' after max retries
        //            _dbConnection.Execute("UPDATE Notifications SET Status = 'Failed' WHERE Id = @Id", notification);
        //        }
         
        //}


        public void SendNotification(Notification notification)
        {
            try
            {
                // 尝试发送通知
            }
            catch (Exception ex)
            {
                // 记录错误日志
                // LogError(ex);

                // 达到最大重试次数，通知管理员
                if (notification.RetryCount >= MaxRetryCount)
                {
                    string message = $"Failed to send notification to {notification.Recipient}: {ex.Message}";
                    //NotifyAdminViaEmail(message);
                    // 可以同时调用 NotifyAdminViaSlack(message) 或其他方法
                }
            }
        }
    }


    public class EmailNotificationSender : INotificationSender
    {
        private readonly SmtpClient _smtpClient;

        public EmailNotificationSender(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendAsync(Notification notification)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("noreply@yourdomain.com"),
                Subject = notification.Subject,
                Body = notification.Content,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(notification.Recipient));

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }

}
