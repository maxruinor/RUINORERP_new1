using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.UI.WorkFlowTester
{
    public class WorkWorkflow2 : IWorkflow<MyNameClass>
    {
        public string Id => "mywork2";
        public int Version => 2;

        public void Build(IWorkflowBuilder<MyNameClass> builder)
        {
            builder
                .StartWith<NextWorker>()
           .Recur(data => TimeSpan.FromSeconds(5), data => data.Counter > 10)
           .Do(recur => recur
           .StartWith(context => Console.WriteLine("Doing recurring task"))
           .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromMinutes(2)))
            .Then(context => Console.WriteLine("Carry on"));
         
            /*
            builder
                .StartWith(context => ExecutionResult.Next())
                .WaitFor("MyEvent", (data, context) => context.Workflow.Id, data => DateTime.Now)
               */



        }
    }
}