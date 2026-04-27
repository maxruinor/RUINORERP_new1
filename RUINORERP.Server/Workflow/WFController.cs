using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using RUINORERP.Business;
using System.Collections.Concurrent;
using RUINORERP.Server.Workflow.WFApproval;

namespace RUINORERP.Server.Workflow
{
    public class WFController
    {
        public ApplicationContext appContext;

        public WFController(ApplicationContext _appContext = null)
        {
            appContext = _appContext;
        }
        //public void Start(IWorkflowHost host)
        //{
        //    var workflowId = host.StartWorkflow("activity-sample", new MyData { Request = "Spend $1,000,000" }).Result;

        //    var approval = host.GetPendingActivity("get-approval", "worker1", TimeSpan.FromMinutes(1)).Result;

        //    if (approval != null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Approval required for " + approval.Parameters);
        //        host.SubmitActivitySuccess(approval.Token, "John Smith");
        //    }


        //    //var approval1 = host.GetPendingActivity("get-approval", "worker1", TimeSpan.FromMinutes(1)).Result;
        //    if (approval != null)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Approval required for " + approval.Parameters);
        //        host.SubmitActivitySuccess(approval.Token, "John Smith");
        //    }

        //}
        public void StartApproval(IWorkflowHost host, string billid, ConcurrentDictionary<string, string> workflowlist)
        {
            //  tb_StocktakeController<tb_Stocktake> ctr = appContext.GetRequiredService<tb_StocktakeController<tb_Stocktake>>();
            // tb_Stocktake data = await ctr.BaseQueryByIdAsync(billid);
            //ApprovalWFData data = new ApprovalWFData();
            //data.DocumentName = billid;
            //var workflowId = host.StartWorkflow("001", data).Result;
            //workflowlist.TryAdd(billid.ToString(), workflowId);
            // var approval = host.GetPendingActivity("get-approval", "worker1", TimeSpan.FromMinutes(1)).Result;
            // if (approval != null)
            // {
            //     System.Diagnostics.Debug.WriteLine("Approval required for " + approval.Parameters);
            //     host.SubmitActivitySuccess(approval.Token, "John Smith");
            // }
            //把ID加入到队列中？

        }


        public async Task<string> StartApprovalWorkflowAsync(IWorkflowHost host, ApprovalWFData entity, ConcurrentDictionary<string, string> workflowlist)
        {
            var workflowId = await host.StartWorkflow("单据审核", entity);
            workflowlist.TryAdd(entity.approvalEntity.BillID.ToString(), workflowId);

            return workflowId;
        }

        public async Task StartApprovalNewAsync(IWorkflowHost host, long billid, ConcurrentDictionary<string, string> workflowlist)
        {
            try
            {
                tb_StocktakeController<tb_Stocktake> ctr = appContext.GetRequiredService<tb_StocktakeController<tb_Stocktake>>();
                tb_Stocktake data = await ctr.BaseQueryByIdAsync(billid);
                var workflowId = await host.StartWorkflow("BillApprovalWorkflow", data);
                workflowlist.TryAdd(billid.ToString(), workflowId);
            }
            catch (Exception ex)
            {
                // 记录错误日志，避免进程崩溃
                System.Diagnostics.Debug.WriteLine($"StartApprovalNewAsync failed: {ex.Message}");
                throw;
            }
        }

        public void PublishEvent(IWorkflowHost host, ApprovalWFData data, ConcurrentDictionary<string, string> workflowlist)
        {
            string workflowId;
            if (workflowlist.TryGetValue(data.approvalEntity.BillID.ToString(), out workflowId))
            {
                host.PublishEvent("审核结果", workflowId, data.approvalEntity, DateTime.Now);
            }
        }


    }
}
