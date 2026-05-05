using System;
using System.Data;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 基础数据导入工具扩展方法
    /// 提供常用的辅助方法，避免代码重复
    /// </summary>
    public static class BasicDataImportExtensions
    {
        /// <summary>
        /// 检查DataTable是否包含指定列（大小写不敏感）
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="columnName">列名</param>
        /// <returns>是否包含</returns>
        public static bool ContainsColumn(this DataTable table, string columnName)
        {
            if (table == null || string.IsNullOrEmpty(columnName))
                return false;

            return table.Columns.Contains(columnName);
        }

        /// <summary>
        /// 安全获取DataRow的值，处理DBNull
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="columnName">列名</param>
        /// <returns>单元格值，DBNull返回null</returns>
        public static object GetSafeValue(this DataRow row, string columnName)
        {
            if (row == null || string.IsNullOrEmpty(columnName) || !row.Table.ContainsColumn(columnName))
                return null;

            object value = row[columnName];
            return value == DBNull.Value ? null : value;
        }

        /// <summary>
        /// 安全获取DataRow的字符串值
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="columnName">列名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串值，空或DBNull返回默认值</returns>
        public static string GetSafeStringValue(this DataRow row, string columnName, string defaultValue = "")
        {
            object value = row.GetSafeValue(columnName);
            if (value == null)
                return defaultValue;

            string strValue = value.ToString();
            return string.IsNullOrEmpty(strValue) ? defaultValue : strValue;
        }

        /// <summary>
        /// 判断值是否为空（null、DBNull或空字符串）
        /// </summary>
        /// <param name="value">要检查的值</param>
        /// <returns>是否为空</returns>
        public static bool IsEmptyValue(this object value)
        {
            return value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString());
        }
    }
}
