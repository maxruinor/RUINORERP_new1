using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.ToolForm;
using RUINORERP.UI.Properties;
using RUINORERP.Common.Log4Net;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<NotificationService> _logger;
        
        // MSN提醒相关组件
        private TaskbarNotifier _taskbarNotifier;
        private readonly Form _ownerForm;

        public NotificationService(INotificationSender notificationSender, IDbConnection dbConnection, ILogger<NotificationService> logger, Form ownerForm = null)
        {
            _notificationSender = notificationSender;
            _dbConnection = dbConnection;
            _logger = logger;
            _ownerForm = ownerForm;
            
            // 初始化将在Initialize方法中完成
        }
        
        /// <summary>
        /// 初始化通知服务
        /// </summary>
        public void Initialize()
        {
            InitRemind();
        }

        /// <summary>
        /// 初始化提醒组件
        /// </summary>
        private void InitRemind()
        {
            _taskbarNotifier = new TaskbarNotifier();
            _taskbarNotifier.SetBackgroundBitmap(global::RUINORERP.UI.Properties.Resources.skin, Color.FromArgb(255, 0, 255));
            _taskbarNotifier.SetCloseBitmap(global::RUINORERP.UI.Properties.Resources.close, Color.FromArgb(255, 0, 255), new System.Drawing.Point(127, 8));
            _taskbarNotifier.TitleRectangle = new System.Drawing.Rectangle(40, 9, 70, 25);
            _taskbarNotifier.ContentRectangle = new System.Drawing.Rectangle(8, 41, 133, 68);
            _taskbarNotifier.TitleClick += new EventHandler(TitleClick);
            _taskbarNotifier.ContentClick += new EventHandler(ContentClick);
            _taskbarNotifier.CloseClick += new EventHandler(CloseClick);
        }

        void TitleClick(object obj, EventArgs ea)
        {
            // 可以在这里添加标题点击事件处理逻辑
        }

        void ContentClick(object obj, EventArgs ea)
        {
            // 可以在这里添加内容点击事件处理逻辑
        }

        void CloseClick(object obj, EventArgs ea)
        {
            _taskbarNotifier.Close();
        }

        /// <summary>
        /// MSN风格提醒
        /// </summary>
        public void MSNRemind(string title, string content, int delayShow, int delayStay, int delayHide, 
            bool closeClickable, bool titleClickable, bool contentClickable, bool selectionRectangle, 
            bool keepVisibleOnMouseOver, bool reShowOnMouseOver)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                return;
            }
            
            try
            {
                _taskbarNotifier.CloseClickable = closeClickable;
                _taskbarNotifier.TitleClickable = titleClickable;
                _taskbarNotifier.ContentClickable = contentClickable;
                _taskbarNotifier.EnableSelectionRectangle = selectionRectangle;
                _taskbarNotifier.KeepVisibleOnMousOver = keepVisibleOnMouseOver;
                _taskbarNotifier.ReShowOnMouseOver = reShowOnMouseOver;
                _taskbarNotifier.Show(title, content, delayShow, delayStay, delayHide);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex);
            }
        }

        /// <summary>
        /// 显示消息提醒
        /// </summary>
        public void ShowMsg(string msg, string caption = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            
            try
            {
                if (caption == null)
                {
                    caption = "消息提醒";
                }

                // 如果有宿主窗体，使用Invoke确保UI线程安全
                if (_ownerForm != null && _ownerForm.InvokeRequired)
                {
                    _ownerForm.Invoke(new Action(() =>
                    {
                        MSNRemind(caption, $"{msg}\r\n  [点击查看]", 2000, 5000, 3000, true, true, false, false, true, true);
                    }));
                }
                else
                {
                    MSNRemind(caption, $"{msg}\r\n  [点击查看]", 2000, 5000, 3000, true, true, false, false, true, true);
                }
            }
            catch (Exception ex)
            {
                _logger?.Error(ex);
            }
        }

        /// <summary>
        /// 通过NotificationBox显示通知
        /// </summary>
        public void ShowNotificationBox(string message)
        {
            try
            {
                NotificationBox.Instance().ShowForm(message);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex);
            }
        }

        public void SendNotification(Notification notification)
        {
            try
            {
                // 尝试发送通知
                _notificationSender?.SendAsync(notification).Wait();
            }
            catch (Exception ex)
            {
                // 记录错误日志
                _logger?.Error(ex);

                // 达到最大重试次数，通知管理员
                if (notification.RetryCount >= MaxRetryCount)
                {
                    string message = $"Failed to send notification to {notification.Recipient}: {ex.Message}";
                    // 可以调用内部的ShowMsg方法显示错误信息
                    ShowMsg(message, "通知发送失败");
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
