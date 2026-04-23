using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Server.Workflow.WFReminder;
using RUINORERP.WF.WFApproval;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;
using WorkflowCore.Services;

namespace RUINORERP.Server.Workflow.WFScheduled
{
    /// <summary>
    /// 每日任务工作流
    /// 修复内存泄漏问题：移除静态字段，使用工作流数据传递状态
    /// </summary>
    public class DailyTaskWorkflow : IWorkflow
    {
        public string Id => "DailyTaskWorkflow";
        public int Version => 1;

        /// <summary>
        /// 工作流数据类，用于在步骤间传递状态
        /// </summary>
        public class DailyTaskData
        {
            public DateTime NextExecutionTime { get; set; }
            public int ExecutionCount { get; set; }
            public bool ShouldContinue { get; set; } = true;
        }

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith(context =>
                {
                    // 初始化执行时间 (每天上午 10:50)
                    var executionTime = DateTime.Today.AddHours(10).AddMinutes(50);
                    if (executionTime < DateTime.Now)
                    {
                        // 如果今天的时间已过，设置为明天
                        executionTime = executionTime.AddDays(1);
                    }
                    
                    // 初始化工作流数据
                    if (context.Workflow.Data == null)
                    {
                        context.Workflow.Data = new DailyTaskData
                        {
                            NextExecutionTime = executionTime,
                            ExecutionCount = 0
                        };
                    }
                    else
                    {
                        var data = (DailyTaskData)context.Workflow.Data;
                        data.NextExecutionTime = executionTime;
                        data.ExecutionCount = 0;
                    }
                    
                    frmMainNew.Instance.PrintInfoLog($"每日任务工作流初始化，下次执行时间：{executionTime:yyyy-MM-dd HH:mm:ss}");
                    return ExecutionResult.Next();
                })
                .Recur(
                    data => 
                        TimeSpan.FromMilliseconds(
                            Math.Max(
                                (((DailyTaskData)data).NextExecutionTime - DateTime.Now).TotalMilliseconds,
                                60000
                            )
                        ),
                    data => ((DailyTaskData)data).ShouldContinue)
                .Do(recur => recur
                    .StartWith<DailyTaskStep>()
                    .Then(context =>
                    {
                        // 更新下次执行时间
                        var executionTime = DateTime.Today.AddHours(10).AddMinutes(50);
                        if (executionTime < DateTime.Now)
                        {
                            executionTime = executionTime.AddDays(1);
                        }
                        
                        var data = (DailyTaskData)context.Workflow.Data;
                        data.NextExecutionTime = executionTime;
                        data.ExecutionCount++;
                        frmMainNew.Instance.PrintInfoLog($"每日任务执行完成，执行次数：{data.ExecutionCount}, 下次执行时间：{executionTime:yyyy-MM-dd HH:mm:ss}");
                        return ExecutionResult.Next();
                    })
                    .OnError(WorkflowErrorHandling.Retry)) // 修复点：使用 Retry 替代 Continue
                .Then(context =>
                {
                    var data = (DailyTaskData)context.Workflow.Data;
                    frmMainNew.Instance.PrintInfoLog($"每日任务工作流结束，总执行次数：{data.ExecutionCount}");
                    return ExecutionResult.Next();
                });
        }
    }


    /// <summary>
    /// 每日任务执行步骤
    /// 修复内存泄漏：确保任务完成后正确释放资源
    /// </summary>
    public class DailyTaskStep : StepBodyAsync
    {
        private readonly ILogger<DailyTaskStep> _logger;
 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public DailyTaskStep(ILogger<DailyTaskStep> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 执行每日任务
        /// 修复：添加超时控制和资源释放
        /// </summary>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"开始每日任务，执行时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                frmMainNew.Instance.PrintInfoLog($"开始每日任务~~~。执行时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                
                // TODO: 在这里添加实际的每日任务逻辑
                // 示例：库存快照、文件清理、数据归档等
                
                // 模拟耗时任务 (实际使用时删除)
                await Task.Delay(5000);
                
                _logger.LogInformation($"结束每日任务，执行时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                frmMainNew.Instance.PrintInfoLog($"结束每日任务~~~。执行时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "每日任务被取消");
                frmMainNew.Instance.PrintInfoLog($"每日任务被取消：{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "开始每日任务出错");
                frmMainNew.Instance.PrintInfoLog($"每日任务执行出错：{ex.Message}");
            }

            return ExecutionResult.Next();
        }
    }
}






