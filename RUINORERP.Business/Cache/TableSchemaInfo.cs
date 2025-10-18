using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 表结构信息类，用于存储表的元数据信息
    /// </summary>
    public class TableSchemaInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 主键字段名
        /// </summary>
        public string PrimaryKeyField { get; set; }

        /// <summary>
        /// 主显示字段名（兼容原有逻辑）
        /// </summary>
        public string DisplayField { get; set; }

        /// <summary>
        /// 所有需要缓存的显示字段名列表
        /// </summary>
        public List<string> DisplayFields { get; set; } = new List<string>();

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 是否是视图
        /// </summary>
        public bool IsView { get; set; }

        /// <summary>
        /// 是否需要缓存
        /// </summary>
        public bool IsCacheable { get; set; } = true;

        /// <summary>
        /// 是否缓存整行数据（true）还是只缓存指定字段（false）
        /// 默认值为true，表示缓存整行数据以保持向后兼容
        /// </summary>
        public bool CacheWholeRow { get; set; } = true;

        /// <summary>
        /// 表描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 外键关系列表
        /// </summary>
        public List<ForeignKeyRelation> ForeignKeys { get; set; } = new List<ForeignKeyRelation>();

        /// <summary>
        /// 属性信息列表
        /// </summary>
        public List<PropertyInfo> Properties { get; set; } = new List<PropertyInfo>();

        /// <summary>
        /// 验证表结构信息是否完整
        /// </summary>
        /// <returns>验证结果</returns>
        public bool Validate()
        {
            // 基本验证：表名、主键字段、主显示字段和实体类型不能为空
            var basicValidation = !string.IsNullOrEmpty(TableName) &&
                                 !string.IsNullOrEmpty(PrimaryKeyField) &&
                                 !string.IsNullOrEmpty(DisplayField) &&
                                 EntityType != null;
            
            // 如果不是缓存整行，还需要验证DisplayFields列表不为空
            if (!CacheWholeRow && !basicValidation)
            {
                return false;
            }
            
            if (!CacheWholeRow)
            {
                return DisplayFields != null && DisplayFields.Count > 0;
            }
            
            return basicValidation;
        }

        /// <summary>
        /// 获取表结构信息的字符串表示
        /// </summary>
        /// <returns>表结构信息字符串</returns>
        public string ToInfoString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"表名: {TableName}");
            sb.AppendLine($"主键字段: {PrimaryKeyField}");
            sb.AppendLine($"主显示字段: {DisplayField}");
            sb.AppendLine($"所有显示字段: {string.Join(", ", DisplayFields)}");
            sb.AppendLine($"实体类型: {EntityType?.Name ?? "未指定"}");
            sb.AppendLine($"是否视图: {IsView}");
            sb.AppendLine($"是否缓存: {IsCacheable}");
            sb.AppendLine($"缓存策略: {(CacheWholeRow ? "缓存整行" : "只缓存指定字段")}");
            sb.AppendLine($"描述: {Description ?? "无"}");
            sb.AppendLine($"外键数量: {ForeignKeys?.Count ?? 0}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 外键关系类
    /// </summary>
    public class ForeignKeyRelation
    {
        /// <summary>
        /// 外键字段名
        /// </summary>
        public string ForeignKeyField { get; set; }

        /// <summary>
        /// 关联的表名
        /// </summary>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// 关联表的主键字段
        /// </summary>
        public string RelatedPrimaryKey { get; set; }

        /// <summary>
        /// 外键属性信息
        /// </summary>
        public PropertyInfo ForeignKeyProperty { get; set; }
    }
}