using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace RUINORERP.WF.WFServiceExtensions
{
    internal class WorkflowStartup
    {
        private readonly IWorkflowRegistry _registry;

        public WorkflowStartup(IWorkflowRegistry registry)
        {
            _registry = registry;
        }

        // 在这里注册工作流定义
        public void RegisterWorkflows()
        {
            // 使用IWorkflowRegistry注册工作流
            // _registry.RegisterWorkflow<YourWorkflow1, YourWorkflow1Definition>();
            // _registry.RegisterWorkflow<YourWorkflow2, YourWorkflow2Definition>();
            // ...其他工作流
           // _registry.RegisterWorkflow()


        }
    }
}
