using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using RUINORERP.Business.Processor;

namespace RUINORERP.Business.DataCorrectionServices
{
    /// <summary>
    /// 数据预览结果
    /// </summary>
    public class DataPreviewResult
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        
        /// <summary>
        /// 预览数据
        /// </summary>
        public DataTable Data { get; set; }
        
        /// <summary>
        /// 记录总数
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// 显示说明
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 是否需要修复的记录数
        /// </summary>
        public int NeedFixCount { get; set; }
    }
    
    /// <summary>
    /// 数据修复执行结果
    /// </summary>
    public class DataFixExecutionResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 影响的表清单
        /// </summary>
        public List<string> AffectedTables { get; set; } = new List<string>();
        
        /// <summary>
        /// 每个表影响的记录数
        /// </summary>
        public Dictionary<string, int> AffectedRows { get; set; } = new Dictionary<string, int>();
        
        /// <summary>
        /// 执行日志
        /// </summary>
        public List<string> Logs { get; set; } = new List<string>();
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
        
        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }
    }
    
    /// <summary>
    /// 数据修复服务接口
    /// </summary>
    public interface IDataCorrectionService
    {
        /// <summary>
        /// 修复项名称（唯一标识）
        /// </summary>
        string CorrectionName { get; }
        
        /// <summary>
        /// 功能名称
        /// </summary>
        string FunctionName { get; }
        
        /// <summary>
        /// 问题描述
        /// </summary>
        string ProblemDescription { get; }
        
        /// <summary>
        /// 影响表清单
        /// </summary>
        List<string> AffectedTables { get; }
        
        /// <summary>
        /// 修复逻辑说明
        /// </summary>
        string FixLogic { get; }
        
        /// <summary>
        /// 发生情形
        /// </summary>
        string OccurrenceScenario { get; }
        
        /// <summary>
        /// 获取查询过滤器（用于动态生成查询UI）
        /// </summary>
        /// <returns>查询过滤器，如果返回null则表示不支持动态查询</returns>
        QueryFilter GetQueryFilter();
        
        /// <summary>
        /// 获取查询使用的实体类型（用于动态生成查询UI）
        /// </summary>
        /// <returns>实体类型，例如 tb_PurOrderQueryDto、tb_PurEntryQueryDto 等</returns>
        Type GetQueryEntityType();
        
        /// <summary>
        /// 预览数据（支持多表）
        /// </summary>
        /// <param name="parameters">可选参数</param>
        /// <returns>多个表的预览结果</returns>
        Task<List<DataPreviewResult>> PreviewAsync(Dictionary<string, object> parameters = null);
        
        /// <summary>
        /// 执行修复
        /// </summary>
        /// <param name="testMode">是否为测试模式</param>
        /// <param name="parameters">可选参数，可包含SelectedIds等</param>
        /// <returns>执行结果</returns>
        Task<DataFixExecutionResult> ExecuteAsync(bool testMode = true, Dictionary<string, object> parameters = null);
        
        /// <summary>
        /// 验证是否可以执行
        /// </summary>
        /// <returns>验证结果和消息</returns>
        Task<(bool IsValid, string Message)> ValidateAsync();
    }
}
