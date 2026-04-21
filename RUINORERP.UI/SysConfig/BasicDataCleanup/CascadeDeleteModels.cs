// **************************************
// 文件：CascadeDeleteModels.cs
// 项目：RUINORERP
// 作者：AI Assistant
// 时间：2026-04-21
// 描述：级联删除相关的数据模型
// **************************************

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 级联删除计划
    /// </summary>
    public class CascadeDeletePlan
    {
        /// <summary>
        /// 根实体类型
        /// </summary>
        public Type RootEntityType { get; set; }

        /// <summary>
        /// 根实体ID列表
        /// </summary>
        public List<long> RootEntityIds { get; set; }

        /// <summary>
        /// 删除步骤列表(按依赖顺序排列)
        /// </summary>
        public List<CascadeDeleteStep> Steps { get; set; }

        /// <summary>
        /// 最大深度
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CascadeDeletePlan()
        {
            Steps = new List<CascadeDeleteStep>();
            RootEntityIds = new List<long>();
        }
    }

    /// <summary>
    /// 级联删除步骤
    /// </summary>
    public class CascadeDeleteStep
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 过滤表达式
        /// </summary>
        public LambdaExpression FilterExpression { get; set; }

        /// <summary>
        /// 层级深度(0为根节点)
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 外键字段名
        /// </summary>
        public string ForeignKeyField { get; set; }

        /// <summary>
        /// 父实体类型
        /// </summary>
        public Type ParentEntityType { get; set; }

        /// <summary>
        /// 是否已执行
        /// </summary>
        public bool IsExecuted { get; set; }

        /// <summary>
        /// 执行的记录数
        /// </summary>
        public int ExecutedCount { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime? ExecutionStartTime { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? ExecutionEndTime { get; set; }

        /// <summary>
        /// 执行耗时(毫秒)
        /// </summary>
        public long? ExecutionElapsedMs { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CascadeDeleteStep()
        {
            IsExecuted = false;
            ExecutedCount = 0;
        }
    }

    /// <summary>
    /// 级联删除结果
    /// </summary>
    public class CascadeDeleteResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 总删除记录数
        /// </summary>
        public int TotalDeletedCount { get; set; }

        /// <summary>
        /// 涉及的表数量
        /// </summary>
        public int AffectedTableCount { get; set; }

        /// <summary>
        /// 各步骤的执行结果
        /// </summary>
        public List<CascadeDeleteStep> StepResults { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 总耗时(毫秒)
        /// </summary>
        public long TotalElapsedMs { get; set; }

        /// <summary>
        /// 是否为测试模式
        /// </summary>
        public bool IsTestMode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CascadeDeleteResult()
        {
            StepResults = new List<CascadeDeleteStep>();
            IsSuccess = true;
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// 添加步骤结果
        /// </summary>
        public void AddStepResult(CascadeDeleteStep step, int deletedCount)
        {
            step.IsExecuted = true;
            step.ExecutedCount = deletedCount;
            step.ExecutionEndTime = DateTime.Now;
            step.ExecutionElapsedMs = (long)(step.ExecutionEndTime.Value - step.ExecutionStartTime.Value).TotalMilliseconds;
            
            StepResults.Add(step);
            TotalDeletedCount += deletedCount;
            AffectedTableCount = StepResults.Count;
        }

        /// <summary>
        /// 完成执行
        /// </summary>
        public void Complete()
        {
            EndTime = DateTime.Now;
            TotalElapsedMs = (long)(EndTime - StartTime).TotalMilliseconds;
        }

        /// <summary>
        /// 标记为失败
        /// </summary>
        public void MarkAsFailed(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
            Complete();
        }
    }
}
