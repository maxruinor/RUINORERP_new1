using RUINORERP.UI.WorkFlow.Steps;
using RUINORERP.UI.WorkFlowTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.UI.WorkFlow
{

    public class WFSaleOrderPorcessWorkflow : IWorkflow<ProcessData>
    {
        public string Id => "SOWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<ProcessData> builder)
        {

            builder
                .StartWith<SubmitStep>()
                .Recur(data => TimeSpan.FromSeconds(5), data => data.Counter > 10)
                .Do(recur => recur
                .StartWith(context => Console.WriteLine("Doing recurring task"))
                .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(2)))
                .Then(context => Console.WriteLine("Carry on"));
        }
    }

}
