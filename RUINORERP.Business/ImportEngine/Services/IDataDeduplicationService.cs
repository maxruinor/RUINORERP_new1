using System;
using System.Collections.Generic;
using System.Data;

namespace RUINORERP.Business.ImportEngine.Services
{
    /// <summary>
    /// 去重结果
    /// </summary>
    public class DeduplicationResult
    {
        /// <summary>
        /// 去重后的数据表
        /// </summary>
        public DataTable DeduplicatedData { get; set; }

        /// <summary>
        /// 原始数据行数
        /// </summary>
        public int OriginalCount { get; set; }

        /// <summary>
        /// 去除的重复行数
        /// </summary>
        public int DuplicateCount { get; set; }

        /// <summary>
        /// 使用的去重字段列表
        /// </summary>
        public List<string> DeduplicationFields { get; set; } = new List<string>();
    }

    /// <summary>
    /// 数据去重服务接口
    /// 提供基于多字段组合的数据去重功能，支持灵活的去重策略
    /// </summary>
    public interface IDataDeduplicationService
    {
        /// <summary>
        /// 根据配置对数据进行去重处理
        /// </summary>
        /// <param name="dataTable">原始数据表</param>
        /// <param name="deduplicationFields">去重字段列表</param>
        /// <param name="strategy">去重策略（FirstOccurrence / LastOccurrence）</param>
        /// <returns>去重结果</returns>
        /// <exception cref="ArgumentNullException">参数为空时抛出异常</exception>
        /// <exception cref="InvalidOperationException">去重字段不存在时抛出异常</exception>
        DeduplicationResult Deduplicate(
            DataTable dataTable, 
            List<string> deduplicationFields, 
            string strategy = "FirstOccurrence");
    }
}
