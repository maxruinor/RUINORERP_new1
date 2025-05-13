using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Pipelines.Sockets.Unofficial;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerSession;
using SqlSugar;
using SuperSocket.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    /// <summary>
    /// 通知服务将来是不是数据库只放到一个类中去操作。这里是回写提醒结果日志等
    /// </summary>
    public class NotificationService : INotificationService
    {
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        private readonly ILogger<NotificationService> _logger;
        //private readonly IRealtimeNotifier _realtimeNotifier;
        // 添加邮件和短信服务依赖
        //private readonly IEmailService _emailService;
        //private readonly ISmsService _smsService;
        public NotificationService()
        {
           

        }
        //private readonly IEmailService _email;

        //public NotificationService(ISqlSugarClient db, SocketServer socket, IEmailService email)
        //{
        public NotificationService(ILogger<NotificationService> logger, 
            ApplicationContext _AppContextData, 
            IUnitOfWorkManage unitOfWorkManage
             //IRealtimeNotifier realtimeNotifier
            )
        {
            //_email = email;
            _logger = logger;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
            //_realtimeNotifier = realtimeNotifier;
        }

        public async Task SendNotificationAsync(IReminderRule rule, string message,object contextData)
        {
            try
            {
                // 记录到数据库
                // 记录到数据库
                var alert = new tb_ReminderAlert
                {
                    RuleId = rule.RuleId,
                    AlertTime = DateTime.Now,
                    Message = message,
                    //后面补一个属性
                    //ContextData = JsonConvert.SerializeObject(contextData)
                };
                await _unitOfWorkManage.GetDbClient().Insertable(alert).ExecuteCommandAsync();

                // 获取通知渠道
                List<NotificationChannel> channels = SmartReminderHelper.ParseChannels(rule.NotifyChannels);
                foreach (var channel in channels)
                {
                    if (channel == NotificationChannel.Realtime)
                    {
                        //foreach (var item in policy.tb_InventoryAlertTargets)
                        //{
                        //    if (item.tb_userinfo != null)
                        //    {
                        //        List<SessionforBiz> sessions = frmMain.Instance.sessionListBiz.Values.ToList().OrderBy(c => c.StartTime).ToList();
                        //        var session = sessions.FirstOrDefault(c => c.User.UserID == item.tb_userinfo.User_ID);
                        //        if (session != null)
                        //        {
                        //            if (session.State == SessionState.Connected)
                        //            {
                        //                //  "库存预警通知";
                        //                MessageModel msb = new MessageModel
                        //                {

                        //                    msg = message,
                        //                };
                        //                UserService.给客户端发消息实体(session, msb, true);

                        //                #region
                        //                // 处理不同通知通道
                        //                //foreach (var channel in policy.NotificationTypes)
                        //                //{
                        //                //    switch (channel)
                        //                //    {
                        //                //        case NotificationChannel.Realtime:
                        //                //            await SendRealtimeNotification(policy, message);
                        //                //            break;
                        //                //        case NotificationChannel.Email:
                        //                //            await SendEmailNotification(policy, message);
                        //                //            break;
                        //                //        case NotificationChannel.SMS:
                        //                //            await SendSmsNotification(policy, message);
                        //                //            break;
                        //                //        case NotificationChannel.Workflow:
                        //                //            await SendWorkflowNotification(policy, message);
                        //                //            break;
                        //                //    }
                        //                //}
                        //                #endregion


                        //            }
                        //        }
                        //    }
                        //}


                    }
                }



                //// 发送邮件
                //if (policy.NotificationTypes.Contains("Email"))
                //{
                //    var emails = await _db.Queryable<User>()
                //                        .Where(u => u.IsInventoryManager)
                //                        .Select(u => u.Email)
                //                        .ToListAsync();
                //    await _email.SendBatchAsync(emails, "库存预警通知", message);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送提醒失败：{Message}", message);
                // 这里可以添加重试逻辑
            }
        }
        /*

        #region 每个发送渠道的具体方法实现
        private async Task SendRealtimeNotification(tb_ReminderRule policy, string message)
        {
            var recipients = await GetRecipientsForChannel(policy, NotificationChannel.Realtime);
            foreach (var session in recipients)
            {
                MessageModel msb = new MessageModel
                {
                    msg = message,
                };
                UserService.给客户端发消息实体(session, msb, true);
            }
        }

        private async Task SendEmailNotification(tb_ReminderRule policy, string message)
        {
            var emails = await GetRecipientsForChannel(policy, NotificationChannel.Email);
            await _emailService.SendBatchAsync(emails, "库存预警通知", message);
        }

        private async Task SendSmsNotification(tb_ReminderRule policy, string message)
        {
            var phoneNumbers = await GetRecipientsForChannel(policy, NotificationChannel.SMS);
            foreach (var number in phoneNumbers)
            {
                await _smsService.SendAsync(number, message);
            }
        }

        private async Task SendWorkflowNotification(tb_ReminderRule policy, string message)
        {
            // 触发工作流提醒
            var workflowReminder = _appContext.Services.GetRequiredService<WorkflowReminderService>();
            workflowReminder.Trigger(new ReminderRequest
            {
                Type = "InventoryAlert",
                Context = new { Policy = policy, Message = message }
            });
        }

        private async Task<List<string>> GetRecipientsForChannel(
            tb_ReminderRule policy,
            NotificationChannel channel)
        {
            // 根据通道类型从数据库获取收件人列表
            var query = from target in policy.tb_InventoryAlertTargets
                        join user in _unitOfWorkManage.GetDbClient().Queryable<User>()
                            on target.tb_userinfo.User_ID equals user.UserID
                        where target.NotificationChannels.HasFlag(channel)
                        select user.GetRecipientForChannel(channel);

            return await query.ToListAsync();
        }
        #endregion

        */
    }
}
