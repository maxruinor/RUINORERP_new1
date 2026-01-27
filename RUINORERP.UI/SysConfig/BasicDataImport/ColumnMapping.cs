using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射配置模型
    /// 用于存储Excel列与系统字段的映射关系
    /// </summary>
    public class ColumnMapping
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Excel列名
        /// </summary>
        public string ExcelColumn { get; set; }

        /// <summary>
        /// 系统字段名
        /// </summary>
        public string SystemField { get; set; }


        /// <summary>
        /// 是否值唯一
        /// </summary>
        public bool IsUniqueValue { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 映射配置名称
        /// </summary>
        public string MappingName { get; set; }

        /// <summary>
        /// 目标实体类型名称
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否为外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 是否系统生成
        /// </summary>
        public bool IsSystemGenerated { get; set; }

        /// <summary>
        /// 关联表名（外键关联的表）
        /// </summary>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// 关联表字段（中文名称，用于显示）
        /// </summary>
        public string RelatedTableField { get; set; }

        /// <summary>
        /// 关联表字段（实际字段名）
        /// </summary>
        public string RelatedTableFieldName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 列映射配置集合
    /// </summary>
    public class ColumnMappingCollection : List<ColumnMapping>
    {
        /// <summary>
        /// 根据Excel列名获取映射配置
        /// </summary>
        /// <param name="excelColumn">Excel列名</param>
        /// <returns>列映射配置</returns>
        public ColumnMapping GetMappingByExcelColumn(string excelColumn)
        {
            return this.FirstOrDefault(m => m.ExcelColumn.Equals(excelColumn, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 根据系统字段名获取映射配置
        /// </summary>
        /// <param name="systemField">系统字段名</param>
        /// <returns>列映射配置</returns>
        public ColumnMapping GetMappingBySystemField(string systemField)
        {
            return this.FirstOrDefault(m => m.SystemField.Equals(systemField, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取唯一标识列映射
        /// </summary>
        /// <returns>唯一标识列映射配置</returns>
        public ColumnMapping GetUniqueKeyMapping()
        {
            return this.FirstOrDefault(m => m.IsUniqueValue);
        }
    }
}