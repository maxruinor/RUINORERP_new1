using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Enums;  // ✅ 添加枚举引用
using RUINORERP.Model.ImportEngine.Models;
using SqlSugar;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 列映射与数据转换服务
    /// </summary>
    public class ColumnMappingService
    {
        private readonly ISqlSugarClient _db;

        public ColumnMappingService(ISqlSugarClient db = null)
        {
            _db = db;
        }

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
                        // ✅ 特殊处理：外键关联类型
                        if (mapping.DataSourceType == DataSourceType.ForeignKey && mapping.ForeignConfig != null)
                        {
                            targetRow[mapping.DbColumn] = ResolveForeignKey(rawValue, mapping, sourceRow);
                        }
                        else
                        {
                            targetRow[mapping.DbColumn] = ConvertValue(rawValue, mapping);
                        }
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

        /// <summary>
        /// 解析外键关联值
        /// 根据Excel中的标识值(如名称/编码)查询数据库获取对应的主键ID
        /// </summary>
        private object ResolveForeignKey(object excelValue, ColumnMapping mapping, DataRow sourceRow)
        {
            if (excelValue == null || excelValue == DBNull.Value)
            {
                return mapping.IsRequired ? throw new Exception("外键字段不能为空") : DBNull.Value;
            }

            string identifierValue = excelValue.ToString().Trim();
            if (string.IsNullOrEmpty(identifierValue))
            {
                return mapping.IsRequired ? throw new Exception("外键字段不能为空") : DBNull.Value;
            }

            var foreignConfig = mapping.ForeignConfig;
            
            // 确定用于查询的字段名和值
            string queryField;  // 数据库中用于匹配的字段(如CategoryName)
            string queryValue;  // 匹配的值

            // 如果配置了ForeignKeySourceColumn,使用它指定的字段
            if (foreignConfig.ForeignKeySourceColumn != null && !string.IsNullOrEmpty(foreignConfig.ForeignKeySourceColumn.Key))
            {
                // ForeignKeySourceColumn.Key = Excel列名(如"父类目名称")
                // ForeignKeySourceColumn.Value = 数据库字段名(如"CategoryName")
                queryField = foreignConfig.ForeignKeySourceColumn.Value ?? foreignConfig.ForeignKeyField?.Key;
                
                // 从Excel中获取该列的值
                if (sourceRow.Table.Columns.Contains(foreignConfig.ForeignKeySourceColumn.Key))
                {
                    queryValue = sourceRow[foreignConfig.ForeignKeySourceColumn.Key]?.ToString()?.Trim();
                }
                else
                {
                    queryValue = identifierValue;
                }
            }
            else
            {
                // 没有配置SourceColumn,直接使用当前字段的值
                queryField = foreignConfig.ForeignKeyField?.Key;
                queryValue = identifierValue;
            }

            if (string.IsNullOrEmpty(queryField) || string.IsNullOrEmpty(queryValue))
            {
                return mapping.IsRequired ? throw new Exception("外键关联配置不完整") : DBNull.Value;
            }

            // ✅ 查询数据库获取主键ID
            try
            {
                if (_db == null)
                {
                    throw new Exception("数据库连接未初始化,无法解析外键关联");
                }

                string tableName = foreignConfig.ForeignKeyTable?.Key;
                string primaryKeyField = foreignConfig.ForeignKeyField?.Key;

                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(primaryKeyField))
                {
                    throw new Exception("外键表或主键字段配置缺失");
                }

                // 构建查询SQL
                string sql = $"SELECT [{primaryKeyField}] FROM [{tableName}] WHERE [{queryField}] = @queryValue";
                var result = _db.Ado.GetScalar(sql, new { queryValue });

                if (result == null || result == DBNull.Value)
                {
                    // 找不到对应的记录
                    if (foreignConfig.AutoCreateIfNotExists)
                    {
                        // TODO: 可选功能 - 自动创建不存在的记录
                        throw new Exception($"未找到【{queryValue}】对应的记录,且不支持自动创建");
                    }
                    else
                    {
                        throw new Exception($"在表【{tableName}】中未找到【{queryField}={queryValue}】的记录");
                    }
                }

                // 返回找到的主键ID
                return Convert.ToInt64(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"解析外键失败({queryField}={queryValue}): {ex.Message}");
            }
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
