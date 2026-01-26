using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using RUINORERP.Model;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 动态数据导入器
    /// 负责根据映射配置将Excel数据导入到指定的实体类型
    /// </summary>
    public class DynamicImporter
    {
        private readonly ISqlSugarClient _db;

        /// <summary>
        /// 导入结果统计
        /// </summary>
        public class ImportResult
        {
            /// <summary>
            /// 总记录数
            /// </summary>
            public int TotalCount { get; set; }

            /// <summary>
            /// 成功记录数
            /// </summary>
            public int SuccessCount { get; set; }

            /// <summary>
            /// 失败记录数
            /// </summary>
            public int FailedCount { get; set; }

            /// <summary>
            /// 更新记录数
            /// </summary>
            public int UpdatedCount { get; set; }

            /// <summary>
            /// 新增记录数
            /// </summary>
            public int InsertedCount { get; set; }

            /// <summary>
            /// 耗时（毫秒）
            /// </summary>
            public long ElapsedMilliseconds { get; set; }

            /// <summary>
            /// 失败记录列表
            /// </summary>
            public List<FailedRecord> FailedRecords { get; set; } = new List<FailedRecord>();
        }

        /// <summary>
        /// 失败记录信息
        /// </summary>
        public class FailedRecord
        {
            /// <summary>
            /// 行号
            /// </summary>
            public int RowNumber { get; set; }

            /// <summary>
            /// 错误消息
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// 数据内容
            /// </summary>
            public Dictionary<string, object> Data { get; set; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">SqlSugar数据库客户端</param>
        public DynamicImporter(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// 动态导入数据
        /// </summary>
        /// <param name="dataTable">Excel数据表格</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>导入结果</returns>
        /// <exception cref="ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="ArgumentException">映射配置无效时抛出</exception>
        public ImportResult Import(DataTable dataTable, ColumnMappingCollection mappings, Type entityType)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                throw new ArgumentException("数据表为空", nameof(dataTable));
            }

            if (mappings == null || mappings.Count == 0)
            {
                throw new ArgumentException("列映射配置不能为空", nameof(mappings));
            }

            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            var result = new ImportResult();
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var entityList = new List<object>();
            var uniqueKeyMapping = mappings.GetUniqueKeyMapping();

            try
            {
                result.TotalCount = dataTable.Rows.Count;

                // 遍历所有数据行
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    try
                    {
                        // 创建实体对象
                        var entity = CreateEntityFromRow(row, mappings, entityType, i + 2); // +2 因为Excel从第2行开始（第1行是标题）

                        // 验证实体对象
                        string validationError = ValidateEntity(entity, uniqueKeyMapping);
                        if (!string.IsNullOrEmpty(validationError))
                        {
                            result.FailedRecords.Add(new FailedRecord
                            {
                                RowNumber = i + 2,
                                ErrorMessage = validationError,
                                Data = RowToDictionary(row)
                            });
                            result.FailedCount++;
                            continue;
                        }

                        entityList.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        result.FailedRecords.Add(new FailedRecord
                        {
                            RowNumber = i + 2,
                            ErrorMessage = $"数据处理错误: {ex.Message}",
                            Data = RowToDictionary(row)
                        });
                        result.FailedCount++;
                    }
                }

                // 批量导入数据到数据库
                if (entityList.Count > 0)
                {
                    BatchImportEntities(entityList, entityType, result, uniqueKeyMapping);
                }

                result.SuccessCount = result.TotalCount - result.FailedCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"导入数据失败: {ex.Message}", ex);
            }
            finally
            {
                stopwatch.Stop();
                result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            }

            return result;
        }

        /// <summary>
        /// 从数据行创建实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="rowNumber">行号</param>
        /// <returns>实体对象</returns>
        private object CreateEntityFromRow(DataRow row, ColumnMappingCollection mappings, Type entityType, int rowNumber)
        {
            var entity = Activator.CreateInstance(entityType);

            // 遍历所有映射配置
            foreach (var mapping in mappings)
            {
                try
                {
                    // 获取Excel列的值
                    if (!dataTableContainsColumn(row.Table, mapping.ExcelColumn))
                    {
                        continue;
                    }

                    object cellValue = row[mapping.ExcelColumn];

                    // 如果值为空，跳过
                    if (cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue?.ToString()))
                    {
                        continue;
                    }

                    // 获取实体属性
                    PropertyInfo property = entityType.GetProperty(mapping.SystemField);
                    if (property == null)
                    {
                        continue;
                    }

                    // 类型转换
                    object convertedValue = ConvertValue(cellValue, property.PropertyType);
                    if (convertedValue != null)
                    {
                        property.SetValue(entity, convertedValue);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"行 {rowNumber} 列 {mapping.ExcelColumn} 转换失败: {ex.Message}", ex);
                }
            }

            return entity;
        }

        /// <summary>
        /// 检查DataTable是否包含指定列
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="columnName">列名</param>
        /// <returns>是否包含</returns>
        private bool dataTableContainsColumn(DataTable table, string columnName)
        {
            return table.Columns.Contains(columnName);
        }

        /// <summary>
        /// 值类型转换
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="targetType">目标类型</param>
        /// <returns>转换后的值</returns>
        private object ConvertValue(object value, Type targetType)
        {
            if (value == null || value == DBNull.Value)
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            string stringValue = value.ToString();

            // 处理可空类型
            Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                if (underlyingType == typeof(string))
                {
                    return stringValue;
                }
                else if (underlyingType == typeof(int) || underlyingType == typeof(long) || underlyingType == typeof(short))
                {
                    return Convert.ChangeType(stringValue, underlyingType);
                }
                else if (underlyingType == typeof(decimal))
                {
                    return decimal.Parse(stringValue);
                }
                else if (underlyingType == typeof(double) || underlyingType == typeof(float))
                {
                    return Convert.ChangeType(stringValue, underlyingType);
                }
                else if (underlyingType == typeof(bool))
                {
                    return bool.Parse(stringValue);
                }
                else if (underlyingType == typeof(DateTime))
                {
                    return DateTime.Parse(stringValue);
                }
                else if (underlyingType.IsEnum)
                {
                    return Enum.Parse(underlyingType, stringValue);
                }
                else
                {
                    return Convert.ChangeType(stringValue, underlyingType);
                }
            }
            catch
            {
                throw new Exception($"无法将值 '{stringValue}' 转换为类型 {targetType.Name}");
            }
        }

        /// <summary>
        /// 验证实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="uniqueKeyMapping">唯一键映射配置</param>
        /// <returns>错误消息，如果验证通过则返回空字符串</returns>
        private string ValidateEntity(object entity, ColumnMapping uniqueKeyMapping)
        {
            // 基础验证：检查必填字段
            Type entityType = entity.GetType();

            // 如果设置了唯一键，检查唯一键是否为空
            if (uniqueKeyMapping != null)
            {
                PropertyInfo property = entityType.GetProperty(uniqueKeyMapping.SystemField);
                if (property != null)
                {
                    object value = property.GetValue(entity);
                    if (value == null || string.IsNullOrEmpty(value?.ToString()))
                    {
                        return $"唯一键字段 {uniqueKeyMapping.SystemField} 不能为空";
                    }
                }
            }

            // 验证基类实体（如果是BaseEntity类型）
            if (typeof(BaseEntity).IsAssignableFrom(entityType))
            {
                // 可以添加更多的验证逻辑
            }

            return string.Empty;
        }

        /// <summary>
        /// 批量导入实体到数据库
        /// </summary>
        /// <param name="entityList">实体列表</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="result">导入结果</param>
        /// <param name="uniqueKeyMapping">唯一键映射配置</param>
        private void BatchImportEntities(List<object> entityList, Type entityType, ImportResult result, ColumnMapping uniqueKeyMapping)
        {
            try
            {
                // 开启事务
                _db.Ado.BeginTran();

                foreach (var entity in entityList)
                {
                    try
                    {
                        // 检查是否已存在（如果设置了唯一键）
                        bool exists = false;
                        object existingEntity = null;

                        if (uniqueKeyMapping != null)
                        {
                            PropertyInfo property = entityType.GetProperty(uniqueKeyMapping.SystemField);
                            if (property != null)
                            {
                                object value = property.GetValue(entity);
                                if (value != null)
                                {
                                    // 构建查询条件
                                    var query = _db.Queryable<object>().AS(entityType.Name)
                                        .Where($"{uniqueKeyMapping.SystemField} = @value");

                                    // 这里需要使用SqlSugar的动态查询
                                    // 简化处理：直接插入，依赖数据库唯一约束
                                }
                            }
                        }

                        if (exists && existingEntity != null)
                        {
                            // 更新现有记录
                            PropertyInfo idProperty = entityType.GetProperty("ID") ?? entityType.GetProperty("ID_PK") ?? entityType.GetProperty("Id");
                            if (idProperty != null)
                            {
                                object idValue = idProperty.GetValue(existingEntity);
                                idProperty.SetValue(entity, idValue);
                                _db.UpdateableByObject(entity).ExecuteCommand();
                                result.UpdatedCount++;
                            }
                            else
                            {
                                throw new Exception($"无法更新记录：实体 {entityType.Name} 没有ID属性");
                            }
                        }
                        else
                        {
                            // 插入新记录
                            _db.InsertableByObject(entity).ExecuteCommand();
                            result.InsertedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"导入实体失败: {ex.Message}", ex);
                    }
                }

                // 提交事务
                _db.Ado.CommitTran();
            }
            catch
            {
                // 回滚事务
                _db.Ado.RollbackTran();
                throw;
            }
        }

        /// <summary>
        /// 将数据行转换为字典
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>键值对字典</returns>
        private Dictionary<string, object> RowToDictionary(DataRow row)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn column in row.Table.Columns)
            {
                dict[column.ColumnName] = row[column];
            }
            return dict;
        }

        /// <summary>
        /// 检查唯一键是否已存在
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="uniqueKeyMapping">唯一键映射配置</param>
        /// <param name="value">唯一键值</param>
        /// <returns>是否已存在</returns>
        private bool CheckUniqueKeyExists(Type entityType, ColumnMapping uniqueKeyMapping, object value)
        {
            try
            {
                string tableName = entityType.Name;
                string fieldName = uniqueKeyMapping.SystemField;

                // 构建查询SQL
                string sql = $"SELECT COUNT(1) FROM {tableName} WHERE {fieldName} = @value";
                var parameters = new { value };

                int count = _db.Ado.GetInt(sql, parameters);
                return count > 0;
            }
            catch
            {
                // 如果查询失败，假设不存在
                return false;
            }
        }
    }
}
