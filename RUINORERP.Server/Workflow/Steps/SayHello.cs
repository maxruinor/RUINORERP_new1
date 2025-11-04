using Microsoft.Extensions.Logging;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.Steps
{
    /// <summary>
    /// 工作流测试步骤 - 输出Hello消息
    /// </summary>
    public class SayHello : StepBody
    {
        private readonly ILogger<SayHello> _logger;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public SayHello(ILogger<SayHello> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// 执行步骤逻辑
        /// </summary>
        /// <param name="context">步骤执行上下文</param>
        /// <returns>执行结果</returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Hello workflow step executed");
            Console.WriteLine("Hello");
            return ExecutionResult.Next();
        }
    }
}
