using System;
using SqlSugar;

namespace RUINORERP.Server.BNR
{
    /// <summary>
    /// 数据库序号管理服务
    /// 提供序号的生成、查询、重置等管理功能
    /// </summary>
    public class DatabaseSequenceService
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlSugarClient">SqlSugar客户端实例</param>
        public DatabaseSequenceService(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }
        
        /// <summary>
        /// 获取下一个序号值
        /// </summary>
        /// <param name="key">序号键</param>
        /// <returns>序号值</returns>
        public long GetNextSequenceValue(string key)
        {
            var dbResult = _sqlSugarClient.Ado.UseTran(() =>
            {
                // 1. 尝试更新现有记录
                var rowsAffected = _sqlSugarClient.Ado.ExecuteCommand(
                    "UPDATE SequenceNumbers SET CurrentValue = CurrentValue + 1, LastUpdated = GETDATE() WHERE SequenceKey = @SequenceKey",
                    new { SequenceKey = key });
                
                if (rowsAffected == 0)
                {
                    // 2. 如果没有找到记录，则插入新记录
                    _sqlSugarClient.Ado.ExecuteCommand(
                        "INSERT INTO SequenceNumbers (SequenceKey, CurrentValue, LastUpdated) VALUES (@SequenceKey, 1, GETDATE())",
                        new { SequenceKey = key });
                    
                    return 1;
                }
                else
                {
                    // 3. 获取更新后的值
                    var result = Convert.ToInt64(_sqlSugarClient.Ado.GetScalar(
                        "SELECT CurrentValue FROM SequenceNumbers WHERE SequenceKey = @SequenceKey",
                        new { SequenceKey = key }));
                    
                    return result;
                }
            });
            
            return dbResult.Data;
        }
        
        /// <summary>
        /// 获取当前序号值（不增加）
        /// </summary>
        /// <param name="key">序号键</param>
        /// <returns>当前序号值</returns>
        public long GetCurrentSequenceValue(string key)
        {
            var resultValue = _sqlSugarClient.Ado.GetScalar(
                "SELECT CurrentValue FROM SequenceNumbers WHERE SequenceKey = @SequenceKey",
                new { SequenceKey = key });
            long? result = resultValue != null ? Convert.ToInt64(resultValue) : null;
                
            return result ?? 0;
        }
        
        /// <summary>
        /// 重置序号值
        /// </summary>
        /// <param name="key">序号键</param>
        /// <param name="newValue">新的序号值</param>
        public void ResetSequenceValue(string key, long newValue)
        {
            _sqlSugarClient.Ado.UseTran(() =>
            {
                // 1. 尝试更新现有记录
                var rowsAffected = _sqlSugarClient.Ado.ExecuteCommand(
                    "UPDATE SequenceNumbers SET CurrentValue = @NewValue, LastUpdated = GETDATE() WHERE SequenceKey = @SequenceKey",
                    new { SequenceKey = key, NewValue = newValue });
                
                if (rowsAffected == 0)
                {
                    // 2. 如果没有找到记录，则插入新记录
                    _sqlSugarClient.Ado.ExecuteCommand(
                        "INSERT INTO SequenceNumbers (SequenceKey, CurrentValue, LastUpdated) VALUES (@SequenceKey, @NewValue, GETDATE())",
                        new { SequenceKey = key, NewValue = newValue });
                }
            });
        }
        
        /// <summary>
        /// 删除序号记录
        /// </summary>
        /// <param name="key">序号键</param>
        public void DeleteSequence(string key)
        {
            _sqlSugarClient.Ado.ExecuteCommand(
                "DELETE FROM SequenceNumbers WHERE SequenceKey = @SequenceKey",
                new { SequenceKey = key });
        }
        
        /// <summary>
        /// 检查序号是否存在
        /// </summary>
        /// <param name="key">序号键</param>
        /// <returns>是否存在</returns>
        public bool SequenceExists(string key)
        {
            var countValue = _sqlSugarClient.Ado.GetScalar(
                "SELECT COUNT(1) FROM SequenceNumbers WHERE SequenceKey = @SequenceKey",
                new { SequenceKey = key });
            int count = Convert.ToInt32(countValue);
                
            return count > 0;
        }
    }
}