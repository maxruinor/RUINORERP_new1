// **************************************
// 文件：EntityMetadata.cs
// 项目：RUINORERP.Model
// 作者：AI Assistant
// 时间：2026-04-21
// 描述：实体元数据定义，用于结构化实体映射
// **************************************

using System;
using System.Collections.Generic;

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
        /// 实体名称（类名，用于反射查找）
        /// </summary>
        public string EntityName { get; set; }

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

        /// <summary>
        /// 主键字段名（数据库列名）
        /// </summary>
        public string PrimaryKeyName { get; set; }

        /// <summary>
        /// 主键属性名（C#属性名）
        /// </summary>
        public string PrimaryKeyProperty { get; set; }

        /// <summary>
        /// 子表关联关系列表（用于级联删除）
        /// </summary>
        public List<ChildRelationInfo> ChildRelations { get; set; } = new List<ChildRelationInfo>();
    }

    /// <summary>
    /// 子表关联信息
    /// </summary>
    public class ChildRelationInfo
    {
        public string ChildTableName { get; set; }
        public string ForeignKeyColumn { get; set; }
        public string NavigationProperty { get; set; }
    }
}
