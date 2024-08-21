using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace RUINORERP.WF.WFApproval
{
    class ActivityWorkflow : IWorkflow<MyData>
    {
        public string Id => "activity-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith<HelloWorld>()
                .Activity("get-approval", (data) => data.Request)
                    .Output(data => data.ApprovedBy, step => step.Result)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "Approved by " + data.ApprovedBy)
                .Then<GoodbyeWorld>();
        }
    }

    class MyData
    {
        public string Request { get; set; }
        public string ApprovedBy { get; set; }
    }
}
