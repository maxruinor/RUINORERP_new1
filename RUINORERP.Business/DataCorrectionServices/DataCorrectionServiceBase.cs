using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using RUINORERP.Business.Processor;
using RUINORERP.Global;
using RUINORERP.Repository.UnitOfWorks;

namespace RUINORERP.Business.DataCorrectionServices
{
    /// <summary>
    /// 数据修复服务基类
    /// </summary>
    public abstract class DataCorrectionServiceBase : IDataCorrectionService
    {
        /// <summary>
        /// 工作单元管理器（通过构造函数注入）
        /// </summary>
        protected readonly IUnitOfWorkManage _unitOfWorkManage;
        
        /// <summary>
        /// 数据库客户端（使用工作单元管理器的数据库客户端）
        /// </summary>
        protected virtual SqlSugar.ISqlSugarClient Db => _unitOfWorkManage.GetDbClient();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        protected DataCorrectionServiceBase(IUnitOfWorkManage unitOfWorkManage)
        {
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
        }
        
        #region IDataCorrectionService 实现
        
        public abstract string CorrectionName { get; }
        
        public abstract string FunctionName { get; }
        
        public abstract string ProblemDescription { get; }
        
        public abstract List<string> AffectedTables { get; }
        
        public abstract string FixLogic { get; }
        
        public abstract string OccurrenceScenario { get; }
        
        /// <summary>
        /// 获取查询过滤器（用于动态生成查询UI）
        /// 默认返回null，表示不支持动态查询
        /// 子类可以重写此方法以支持动态查询条件
        /// </summary>
        public virtual QueryFilter GetQueryFilter()
        {
            return null;
        }
        
        /// <summary>
        /// 获取查询使用的实体类型（用于动态生成查询UI）
        /// 默认返回null，表示不支持动态查询
        /// 子类可以重写此方法返回对应的实体类型，例如 tb_PurOrderQueryDto、tb_PurEntryQueryDto 等
        /// </summary>
        public virtual Type GetQueryEntityType()
        {
            return null;
        }
        
        public abstract Task<List<DataPreviewResult>> PreviewAsync(Dictionary<string, object> parameters = null);
        
        public abstract Task<DataFixExecutionResult> ExecuteAsync(bool testMode = true, Dictionary<string, object> parameters = null);
        
        public virtual async Task<(bool IsValid, string Message)> ValidateAsync()
        {
            // 默认验证逻辑，子类可以重写
            return (true, "验证通过");
        }
        
        #endregion
        
        #region 辅助方法
        
        /// <summary>
        /// 创建空的DataTable
        /// </summary>
        protected DataTable CreateDataTable(string tableName)
        {
            var dt = new DataTable(tableName);
            return dt;
        }
        
        /// <summary>
        /// 添加日志
        /// </summary>
        protected void AddLog(DataFixExecutionResult result, string message)
        {
            result.Logs.Add($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
        }
        
        /// <summary>
        /// 记录受影响的表
        /// </summary>
        protected void RecordAffectedTable(DataFixExecutionResult result, string tableName, int rowCount)
        {
            if (!result.AffectedTables.Contains(tableName))
            {
                result.AffectedTables.Add(tableName);
            }
            
            if (result.AffectedRows.ContainsKey(tableName))
            {
                result.AffectedRows[tableName] += rowCount;
            }
            else
            {
                result.AffectedRows[tableName] = rowCount;
            }
        }
        
        /// <summary>
        /// 执行带事务的操作
        /// </summary>
        protected async Task<T> ExecuteWithTransactionAsync<T>(Func<Task<T>> action, bool testMode)
        {
            if (testMode)
            {
                // 测试模式：不启用事务
                return await action();
            }
            else
            {
                // 正式模式：启用事务
                try
                {
                    Db.Ado.BeginTran();
                    var result = await action();
                    Db.Ado.CommitTran();
                    return result;
                }
                catch
                {
                    Db.Ado.RollbackTran();
                    throw;
                }
            }
        }
        
        /// <summary>
        /// 限制DataTable行数
        /// </summary>
        protected DataTable LimitRows(DataTable dt, int maxRows = 100)
        {
            if (dt.Rows.Count <= maxRows)
                return dt;
            
            var limitedDt = dt.Clone();
            for (int i = 0; i < maxRows; i++)
            {
                limitedDt.ImportRow(dt.Rows[i]);
            }
            
            return limitedDt;
        }
        
        #endregion
    }
}
