using WorkflowCore.Interface;
namespace RUINORERP.UI.WorkFlowTester
{
    public class WorkWorkflow : IWorkflow
    {
        public string Id => "mywork";
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<worker>()
                .Then<NextWorker>()
                ;
                
        }
    }
}