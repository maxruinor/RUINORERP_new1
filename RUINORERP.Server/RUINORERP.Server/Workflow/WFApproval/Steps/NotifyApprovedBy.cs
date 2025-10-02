using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorkflowCore.Interface;
using WorkflowCore.Models;
namespace RUINORERP.Server.Workflow.WFApproval.Steps
{

    /// <summary>
    /// 通知审批人
    /// </summary>
    public class NotifyApprovedBy : StepBody
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<NotifyApprovedBy> _logger;
        public string WorkId { get; set; }
        public string BillID { get; set; }
        public string BizType { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Approver { get; set; }


        public NotifyApprovedBy(
            ApplicationContext context,
            ILogger<NotifyApprovedBy> logger)
        {
            _context = context;
            _logger = logger;
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            _logger.LogInformation($"通知审核人"+WorkId);
            //通知谁 有来自谁的提交消息
            string msg = $"{DateTime.Now} 来自 {From} 的审核请求,单号{BillID}，请及时处理！: {BizType}";
            //  ActionForServer.反解析发送凯旋时间
            foreach (var item in frmMain.Instance.sessionListBiz)
            {
                try
                {
                    OriginalData exMsg = new OriginalData();
                   // exMsg.Cmd = (byte)ServerCommand.通知审批人审批;
                    exMsg.One = null;
                    //这种可以写一个扩展方法  
                    ByteBuffer tx = new ByteBuffer(100);
                    tx.PushString(context.Workflow.Id);
                    tx.PushString(msg);
                    exMsg.Two = tx.ToByteArray();
                    item.Value.AddSendData(exMsg);
                }
                catch (Exception ex)
                {
                    frmMain.Instance.PrintInfoLog("NotifyApprovedBy:" + ex.Message);
                }

            }


            return ExecutionResult.Next();
        }
    }
}
