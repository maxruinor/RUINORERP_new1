using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.WF.WFApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using RUINORERP.Business;
using System.Collections.Concurrent;


namespace RUINORERP.WF
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
        public  void StartApproval(IWorkflowHost host, string billid, ConcurrentDictionary<string, string> workflowlist)
        {
            //  tb_StocktakeController<tb_Stocktake> ctr = appContext.GetRequiredService<tb_StocktakeController<tb_Stocktake>>();
            // tb_Stocktake data = await ctr.BaseQueryByIdAsync(billid);
            ApprovalWFData data = new ApprovalWFData();
            data.DocumentName = billid;
            var workflowId = host.StartWorkflow("001", data).Result;
            workflowlist.TryAdd(billid.ToString(), workflowId);
            // var approval = host.GetPendingActivity("get-approval", "worker1", TimeSpan.FromMinutes(1)).Result;
            // if (approval != null)
            // {
            //     System.Diagnostics.Debug.WriteLine("Approval required for " + approval.Parameters);
            //     host.SubmitActivitySuccess(approval.Token, "John Smith");
            // }
            //把ID加入到队列中？

        }


        public async void StartApproval(IWorkflowHost host, long billid, ConcurrentDictionary<string, string> workflowlist)
        {
            tb_StocktakeController<tb_Stocktake> ctr = appContext.GetRequiredService<tb_StocktakeController<tb_Stocktake>>();
            tb_Stocktake data = await ctr.BaseQueryByIdAsync(billid);
            var workflowId = host.StartWorkflow("盘点001", data).Result;
            workflowlist.TryAdd(billid.ToString(), workflowId);
            // var approval = host.GetPendingActivity("get-approval", "worker1", TimeSpan.FromMinutes(1)).Result;
            // if (approval != null)
            // {
            //     System.Diagnostics.Debug.WriteLine("Approval required for " + approval.Parameters);
            //     host.SubmitActivitySuccess(approval.Token, "John Smith");
            // }
            //把ID加入到队列中？

        }

        public async void StartApprovalNew(IWorkflowHost host, long billid, ConcurrentDictionary<string, string> workflowlist)
        {
            tb_StocktakeController<tb_Stocktake> ctr = appContext.GetRequiredService<tb_StocktakeController<tb_Stocktake>>();
            tb_Stocktake data = await ctr.BaseQueryByIdAsync(billid);
            var workflowId = host.StartWorkflow("BillApprovalWorkflow", data).Result;
            workflowlist.TryAdd(billid.ToString(), workflowId);
            // var approval = host.GetPendingActivity("get-approval", "worker1", TimeSpan.FromMinutes(1)).Result;
            // if (approval != null)
            // {
            //     System.Diagnostics.Debug.WriteLine("Approval required for " + approval.Parameters);
            //     host.SubmitActivitySuccess(approval.Token, "John Smith");
            // }
            //把ID加入到队列中？

        }
        public void PublishEvent(IWorkflowHost host, ApprovalWFData data, ConcurrentDictionary<string, string> workflowlist)
        {
            string workflowId;
            if (workflowlist.TryGetValue(data.BillId.ToString(), out workflowId))
            {
                host.PublishEvent("MyEventST", workflowId, data, DateTime.Now);
            }
        }


    }
}
