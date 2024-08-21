using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.DataPortal;
using WorkflowCore.Interface;
using WorkflowCore.Models;
namespace RUINORERP.Server.Workflow.WFApproval.Steps
{

    /// <summary>
    /// 通知相关人员完成了审批
    /// </summary>
    public class NotifyApprovedCompleted : StepBody
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<NotifyApprovedCompleted> _logger;
        public string WorkId { get; set; }
        public string BillID { get; set; }
        public string BizType { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Approver { get; set; }


        public NotifyApprovedCompleted(
            ApplicationContext context,
            ILogger<NotifyApprovedCompleted> logger)
        {
            _context = context;
            _logger = logger;
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            WorkId = context.Workflow.Id;
            _logger.LogInformation($"通知完成" + WorkId);
            //通知谁 有来自谁的提交消息
            string msg = $"{DateTime.Now} 来自 {From} 的审核完成,单号{BillID}，请知悉！: {BizType}";
            //  ActionForServer.反解析发送凯旋时间
            foreach (var item in frmMain.Instance.sessionListBiz)
            {
                try
                {
                    OriginalData exMsg = new OriginalData();
                    exMsg.cmd = (byte)ServerCmdEnum.通知相关人员审批完成;
                    exMsg.One = null;
                    //这种可以写一个扩展方法  
                    ByteBuff tx = new ByteBuff(100);
                    tx.PushString(context.Workflow.Id);
                    tx.PushString(msg);
                    exMsg.Two = tx.toByte();
                    item.Value.AddSendData(exMsg);
                }
                catch (Exception ex)
                {
                    frmMain.Instance.PrintInfoLog("NotifyApprovedCompleted:" + ex.Message);
                }

            }


            return ExecutionResult.Next();
        }
    }
}
