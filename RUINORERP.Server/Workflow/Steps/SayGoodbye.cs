using Microsoft.Extensions.Logging;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.Steps
{
    /// <summary>
    /// 工作流测试步骤 - 输出Goodbye消息
    /// </summary>
    public class SayGoodbye : StepBody
    {
        private readonly ILogger<SayGoodbye> _logger;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public SayGoodbye(ILogger<SayGoodbye> logger)
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
            _logger.LogInformation("Goodbye workflow step executed");
            Console.WriteLine("Goodbye");
            return ExecutionResult.Next();
        }
    }
}
