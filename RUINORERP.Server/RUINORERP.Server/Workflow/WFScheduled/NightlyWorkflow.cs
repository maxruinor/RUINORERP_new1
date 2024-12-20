using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services;

namespace RUINORERP.Server.Workflow.WFScheduled
{

    /// <summary>
    /// 定义一个工作流每天晚上2点运行
    /// 库存提醒，客户状态 ，客户的金额这些都可以在这里来处理？
    /// </summary>
    public class NightlyWorkflow : IWorkflow<GlobalScheduledData>
    {
        public string Id => "NightlyWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<GlobalScheduledData> builder)
        {
            builder
              .StartWith(context =>
              {
                  Console.WriteLine("流程在凌晨 2 点开始执行！");
                  return ExecutionResult.Next();
              });
        }
    }

  
    /// <summary>
    /// 系统级的数据 会自动运行
    /// </summary>
    public class GlobalScheduledData
    {
        // 您的数据定义
    }
}
