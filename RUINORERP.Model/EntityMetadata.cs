// **************************************
// 文件：EntityMetadata.cs
// 项目：RUINORERP.Model
// 作者：AI Assistant
// 时间：2026-04-21
// 描述：实体元数据定义，用于结构化实体映射
// **************************************

using System;

namespace RUINORERP.Model
{
    /// <summary>
    /// 实体元数据
    /// </summary>
    public class EntityMetadata
    {
        /// <summary>
        /// 分类：基础主数据/业务单据/财务相关/系统配置
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 显示名称（用于UI）
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; set; }
    }
}
