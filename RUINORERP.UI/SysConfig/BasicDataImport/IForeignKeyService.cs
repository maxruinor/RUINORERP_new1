using System;
using System.Collections.Generic;
using System.Data;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 外键服务接口
    /// 定义外键查询、验证和缓存的标准操作
    /// </summary>
    public interface IForeignKeyService
    {
        /// <summary>
        /// 预加载外键数据
        /// 根据列映射配置批量加载关联表数据到缓存
        /// </summary>
        /// <param name="mappings">列映射配置集合</param>
        void PreloadForeignKeyData(IEnumerable<ColumnMapping> mappings);

        /// <summary>
        /// 预加载指定表的外键数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        void PreloadForeignKeyData(string tableName, string fieldName);

        /// <summary>
        /// 清除缓存
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 获取外键值
        /// 根据映射配置从Excel数据中获取外键ID
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="errorMessage">错误消息输出</param>
        /// <returns>外键ID值</returns>
        object GetForeignKeyValue(DataRow row, ColumnMapping mapping, int rowNumber, out string errorMessage);

        /// <summary>
        /// 获取外键ID
        /// 优先从缓存中获取，缓存未命中时查询数据库
        /// </summary>
        /// <param name="foreignKeyValue">外键代码值</param>
        /// <param name="relatedTableName">关联表名</param>
        /// <param name="relatedTableField">关联表字段名</param>
        /// <returns>外键主键ID</returns>
        object GetForeignKeyId(string foreignKeyValue, string relatedTableName, string relatedTableField);

        /// <summary>
        /// 验证外键
        /// 检查外键值是否有效
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mapping">列映射配置</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="errorMessage">错误消息输出</param>
        /// <returns>是否验证通过</returns>
        bool ValidateForeignKey(DataRow row, ColumnMapping mapping, int rowNumber, out string errorMessage);
    }
}
