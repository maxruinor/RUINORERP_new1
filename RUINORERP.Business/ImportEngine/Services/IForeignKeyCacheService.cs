using System;
using System.Collections.Generic;

namespace RUINORERP.Business.ImportEngine.Services
{
    /// <summary>
    /// 外键缓存服务接口
    /// 负责外键值的获取和验证，支持预加载和缓存功能
    /// </summary>
    public interface IForeignKeyCacheService
    {
        /// <summary>
        /// 预加载外键数据
        /// 在导入前批量查询所有关联表数据，缓存起来用于外键解析
        /// </summary>
        /// <param name="foreignKeyConfigs">外键配置列表</param>
        void PreloadForeignKeyData(IEnumerable<Models.ForeignKeyConfig> foreignKeyConfigs);

        /// <summary>
        /// 获取外键值
        /// 从缓存或数据库中查找外键对应的ID
        /// </summary>
        /// <param name="tableName">外键表名</param>
        /// <param name="fieldValue">字段值（如供应商名称）</param>
        /// <param name="errorMessage">错误信息输出</param>
        /// <returns>外键ID，如果未找到返回null</returns>
        object GetForeignKeyValue(string tableName, string fieldValue, out string errorMessage);

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 清除指定表的缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        void ClearCache(string tableName);
    }
}
