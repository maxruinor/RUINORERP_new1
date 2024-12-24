using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .StartWith(context => Console.WriteLine("任务开始时间: " + DateTime.Now))
               // .Schedule(() => DateTime.Now.AddSeconds(10)).Do(x => x
                ///    .StartWith(context => Console.WriteLine("定时任务执行: " + DateTime.Now)))
                //.OnError(onError => onError
                //    .Execute(context => Console.WriteLine("任务出错: " + context.Workflow.Exception.Message)))
                .Then(context => Console.WriteLine("任务结束时间: " + DateTime.Now));
        }


        // 这是一个模拟的长时间运行的任务
        private async Task<string> LongRunningTask(string parameter)
        {
            // 这里执行耗时的操作，例如大量计算或IO操作
            await Task.Delay(10000); // 假设的耗时操作
            return "任务完成";
        }
        //public void Build(IWorkflowBuilder<GlobalScheduledData> builder)
        //{
        //    builder
        //      .StartWith(context =>
        //      {

        //          Console.WriteLine("流程在凌晨 2 点开始执行！");
        //          return ExecutionResult.Next();
        //      });
        //}
    }


    /// <summary>
    /// 系统级的数据 会自动运行
    /// </summary>
    public class GlobalScheduledData
    {
        // 您的数据定义
    }
}
