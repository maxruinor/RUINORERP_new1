using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 列映射扩展方法类
    /// 提供对List<ColumnMapping>的扩展方法
    /// </summary>
    public static class ColumnMappingExtensions
    {
        /// <summary>
        /// 获取唯一键映射
        /// </summary>
        /// <param name="mappings">列映射列表</param>
        /// <returns>唯一键映射，如果不存在则返回null</returns>
        public static ColumnMapping GetUniqueKeyMapping(this List<ColumnMapping> mappings)
        {
            if (mappings == null || mappings.Count == 0)
                return null;

            // 优先返回标记为业务键的映射
            var businessKeyMapping = mappings.FirstOrDefault(m => m.IsBusinessKey);
            if (businessKeyMapping != null)
                return businessKeyMapping;

            // 查找ID字段
            var idMapping = mappings.FirstOrDefault(m => 
                !string.IsNullOrEmpty(m.GetSystemFieldName()) && 
                m.GetSystemFieldName().Equals("ID", System.StringComparison.OrdinalIgnoreCase));
            if (idMapping != null)
                return idMapping;

            // 查找唯一值字段
            var uniqueMapping = mappings.FirstOrDefault(m => m.IsUniqueValue);
            if (uniqueMapping != null)
                return uniqueMapping;

            return null;
        }

        /// <summary>
        /// 根据系统字段获取映射
        /// </summary>
        /// <param name="mappings">列映射列表</param>
        /// <param name="systemField">系统字段名</param>
        /// <returns>对应的映射，如果不存在则返回null</returns>
        public static ColumnMapping GetMappingBySystemField(this List<ColumnMapping> mappings, string systemField)
        {
            if (mappings == null || mappings.Count == 0 || string.IsNullOrEmpty(systemField))
                return null;

            return mappings.FirstOrDefault(m => 
                !string.IsNullOrEmpty(m.GetSystemFieldName()) && 
                m.GetSystemFieldName().Equals(systemField, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 根据Excel列获取映射
        /// </summary>
        /// <param name="mappings">列映射列表</param>
        /// <param name="excelColumn">Excel列名</param>
        /// <returns>对应的映射，如果不存在则返回null</returns>
        public static ColumnMapping GetMappingByExcelColumn(this List<ColumnMapping> mappings, string excelColumn)
        {
            if (mappings == null || mappings.Count == 0 || string.IsNullOrEmpty(excelColumn))
                return null;

            return mappings.FirstOrDefault(m => 
                !string.IsNullOrEmpty(m.ExcelColumn) && 
                m.ExcelColumn.Equals(excelColumn, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}