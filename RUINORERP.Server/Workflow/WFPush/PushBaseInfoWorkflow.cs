using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.WFPush
{
    public class PushBaseInfoWorkflow : IWorkflow<PushData>
    {
        private readonly ILogger<PushBaseInfoWorkflow> _logger;
        public PushBaseInfoWorkflow(ILogger<PushBaseInfoWorkflow> logger)
        {
            _logger = logger;
        }
        public string Id => "PushBaseInfoWorkflow";

        public int Version => 1;


        public void Build(IWorkflowBuilder<PushData> builder)
        {

            builder
                .StartWith(context =>
                {
                    // 开始工作流任务
                    Console.WriteLine("Workflow started");
                    frmMainNew.Instance.PrintInfoLog("Workflow started");
                    return ExecutionResult.Next();
                })
                .Then<PushDataStep>()
                .Input(step => step.TagetTableName, data => data.InputData)//内置两个参数 step data，数据来自启动时的。
                .Then(context =>
                {
                    Console.WriteLine("workflow complete");
                    frmMainNew.Instance.PrintInfoLog("workflow complete");
                    _logger.LogInformation(" PushData Workflow complete");
                    //这个写到的文件中了
                    //HLH.Lib.Helper.log4netHelper.info("系统级Application_ThreadException");//
                    return ExecutionResult.Next();
                });
        }
    }
}
