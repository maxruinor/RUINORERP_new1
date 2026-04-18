using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Models;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 列映射与数据转换服务
    /// </summary>
    public class ColumnMappingService
    {
        /// <summary>
        /// 根据配置将原始Excel DataTable转换为符合数据库要求的DataTable
        /// </summary>
        public DataTable MapData(DataTable sourceData, ImportProfile profile)
        {
            var targetDt = new DataTable();
            
            // 1. 构建目标表结构
            foreach (var mapping in profile.ColumnMappings)
            {
                Type colType = GetDbType(mapping.DataType);
                targetDt.Columns.Add(mapping.DbColumn, colType);
            }

            // 2. 逐行转换数据
            foreach (DataRow sourceRow in sourceData.Rows)
            {
                DataRow targetRow = targetDt.NewRow();
                bool isValidRow = true;

                foreach (var mapping in profile.ColumnMappings)
                {
                    if (!sourceData.Columns.Contains(mapping.ExcelHeader))
                    {
                        if (mapping.IsRequired)
                        {
                            throw new Exception($"Excel中缺少必填列: {mapping.ExcelHeader}");
                        }
                        continue;
                    }

                    object rawValue = sourceRow[mapping.ExcelHeader];
                    try
                    {
                        targetRow[mapping.DbColumn] = ConvertValue(rawValue, mapping);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"列[{mapping.ExcelHeader}]数据转换失败: {ex.Message}");
                    }
                }

                if (isValidRow) targetDt.Rows.Add(targetRow);
            }

            return targetDt;
        }

        private object ConvertValue(object value, ColumnMapping mapping)
        {
            if (value == null || value == DBNull.Value) 
                return mapping.IsRequired ? throw new Exception("必填项为空") : DBNull.Value;

            string strVal = value.ToString().Trim();
            if (string.IsNullOrEmpty(strVal) && !mapping.IsRequired) 
                return DBNull.Value;

            switch (mapping.DataType?.ToLower())
            {
                case "int":
                case "long":
                    return long.Parse(strVal);
                case "decimal":
                case "double":
                    return decimal.Parse(strVal);
                case "datetime":
                    return DateTime.Parse(strVal);
                case "bool":
                    return strVal == "1" || strVal.Equals("true", StringComparison.OrdinalIgnoreCase);
                default:
                    return strVal;
            }
        }

        private Type GetDbType(string dataType)
        {
            return dataType?.ToLower() switch
            {
                "int" => typeof(long),
                "decimal" => typeof(decimal),
                "datetime" => typeof(DateTime),
                "bool" => typeof(bool),
                _ => typeof(string)
            };
        }
    }
}
