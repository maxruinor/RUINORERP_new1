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
        /// 是否忽略空值（为空时不导入）
        /// </summary>
        public bool IgnoreEmptyValue { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 映射配置名称
        /// </summary>
        public string MappingName { get; set; }

        /// <summary>
        /// 是否对该字段进行去重复处理
        /// 启用后，在解析和导入时会根据该字段的值去除重复的数据行
        /// 只保留第一次出现的记录，后续重复的记录将被过滤掉
        /// </summary>
        public bool RemoveDuplicates { get; set; }

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

        /// <summary>
        /// 获取所有启用忽略空值的映射
        /// </summary>
        /// <returns>忽略空值的映射配置列表</returns>
        public List<ColumnMapping> GetIgnoreEmptyValueMappings()
        {
            return this.Where(m => m.IgnoreEmptyValue).ToList();
        }

        /// <summary>
        /// 获取启用去重复值的映射配置
        /// </summary>
        /// <returns>启用了去重复的映射配置，如果没有则返回null</returns>
        public ColumnMapping GetRemoveDuplicatesMapping()
        {
            return this.FirstOrDefault(m => m.RemoveDuplicates);
        }

        /// <summary>
        /// 检查是否需要去重复
        /// </summary>
        /// <returns>是否启用去重复</returns>
        public bool IsRemoveDuplicatesEnabled()
        {
            return this.Any(m => m.RemoveDuplicates);
        }
    }
}