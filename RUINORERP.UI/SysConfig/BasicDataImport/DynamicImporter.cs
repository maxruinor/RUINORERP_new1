using RUINORERP.Business.BizMapperService;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

        private readonly IEntityMappingService _entityInfoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">SqlSugar数据库客户端</param>
        public DynamicImporter(ISqlSugarClient db, IEntityMappingService entityInfoService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _entityInfoService = entityInfoService;
        }

        /// <summary>
        /// 动态导入数据（异步）
        /// </summary>
        /// <param name="dataTable">Excel数据表格</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">目标实体类型</param>
        /// <param name="importType">导入类型标识（用于区分客户和供应商等使用相同表的情况）</param>
        /// <returns>导入结果</returns>
        /// <exception cref="ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="ArgumentException">映射配置无效时抛出</exception>
        public async System.Threading.Tasks.Task<ImportResult> ImportAsync(DataTable dataTable, ColumnMappingCollection mappings, Type entityType, string importType = null)
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
            var entityList = new List<BaseEntity>();
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

                        // 使用Validator进行业务验证
                        string validationError = ValidateEntityWithValidator(entity, entityType);
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

                        entityList.Add(entity as BaseEntity);
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
                    try
                    {
                        await BatchImportEntitiesAsync(entityList, entityType, result, mappings, importType);
                    }
                    catch (Exception batchEx)
                    {
                        // 批量导入失败，记录错误详情
                        throw new Exception($"批量导入实体时发生错误: {batchEx.Message}\n详细信息: {batchEx.InnerException?.Message ?? "无"}", batchEx);
                    }
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
        /// <param name="row">数据行（已应用映射，列名为SystemField）</param>
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
                    // 根据数据来源类型获取值
                    object cellValue = null;

                    switch (mapping.DataSourceType)
                    {
                        case DataSourceType.Excel:
                            // Excel数据源
                            if (dataTableContainsColumn(row.Table, mapping.SystemField?.Key))
                            {
                                cellValue = row[mapping.SystemField?.Key];

                                // 如果配置了忽略空值且值为DBNull，则跳过该字段
                                if (cellValue == DBNull.Value && mapping.IgnoreEmptyValue)
                                {
                                    continue;
                                }
                            }
                            break;

                        case DataSourceType.SystemGenerated:
                            // 系统生成的值，不从Excel读取，后续可以在导入后处理
                            cellValue = null;
                            break;

                        case DataSourceType.DefaultValue:
                            // 默认值映射
                            cellValue = mapping.DefaultValue;
                            break;

                        case DataSourceType.ForeignKey:
                            // 外键关联
                            // 从映射后的数据表中获取显示值，然后查询关联表获取ID
                            if (dataTableContainsColumn(row.Table, mapping.SystemField?.Key))
                            {
                                string displayValue = row[mapping.SystemField?.Key]?.ToString();
                                if (!string.IsNullOrEmpty(displayValue) &&
                                    !string.IsNullOrEmpty(mapping.ForeignKeyTable?.Key) &&
                                    !string.IsNullOrEmpty(mapping.ForeignKeyField?.Key))
                                {
                                    // 查找关联表中的对应值
                                    object foreignKeyId = GetForeignKeyId(displayValue, mapping.ForeignKeyTable?.Key, mapping.ForeignKeyField?.Key);
                                    if (foreignKeyId != null)
                                    {
                                        cellValue = foreignKeyId;
                                    }
                                    else
                                    {
                                        throw new Exception($"行 {rowNumber} 外键值 '{displayValue}' 在关联表 {mapping.ForeignKeyTable?.Key} 的字段 {mapping.ForeignKeyField?.Key} 中未找到对应记录");
                                    }
                                }
                            }
                            break;

                            case DataSourceType.SelfReference:
                                // 自身字段引用
                                // 从映射后的数据表中获取显示值，然后从已导入的数据中查找对应的引用值
                                if (dataTableContainsColumn(row.Table, mapping.SystemField?.Key))
                                {
                                    string displayValue = row[mapping.SystemField?.Key]?.ToString();
                                    if (!string.IsNullOrEmpty(displayValue) &&
                                        !string.IsNullOrEmpty(mapping.SelfReferenceField?.Key))
                                    {
                                        // 处理自身引用逻辑（在导入过程中实现）
                                        cellValue = displayValue; // 暂时使用显示值，后续在导入过程中处理
                                    }
                                }
                                break;

                            case DataSourceType.FieldCopy:
                                // 字段复制
                                // 复制同一记录中另一个字段的值
                                if (!string.IsNullOrEmpty(mapping.CopyFromField?.Key))
                                {
                                    if (dataTableContainsColumn(row.Table, mapping.CopyFromField?.Key))
                                    {
                                        cellValue = row[mapping.CopyFromField?.Key];
                                    }
                                }
                                break;
                    }

                    // 如果值为空，检查是否有默认值
                    if (cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue?.ToString()))
                    {
                        if (!string.IsNullOrEmpty(mapping.DefaultValue))
                        {
                            cellValue = mapping.DefaultValue;
                        }
                        else
                        {
                            // 非必填字段且值为空，跳过
                            continue;
                        }
                    }

                    // 获取实体属性
                    PropertyInfo property = entityType.GetProperty(mapping.SystemField?.Key);
                    if (property == null)
                    {
                        throw new Exception($"实体 {entityType.Name} 不存在属性 {mapping.SystemField?.Key}");
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
                    throw new Exception($"行 {rowNumber} 字段 {mapping.SystemField} (Excel列: {mapping.ExcelColumn}) 转换失败: {ex.Message}", ex);
                }
            }

            return entity;
        }

        /// <summary>
        /// 从ExcelColumn中提取默认值
        /// 格式：[默认值:值] 字段名
        /// </summary>
        /// <param name="excelColumn">Excel列名</param>
        /// <returns>默认值</returns>
        private string ExtractDefaultValue(string excelColumn)
        {
            try
            {
                int startIndex = excelColumn.IndexOf('[') + 1;
                int endIndex = excelColumn.IndexOf(']');
                if (startIndex > 0 && endIndex > startIndex)
                {
                    string defaultValuePart = excelColumn.Substring(startIndex, endIndex - startIndex);
                    // 提取冒号后面的值
                    int colonIndex = defaultValuePart.IndexOf(':');
                    if (colonIndex > -1)
                    {
                        return defaultValuePart.Substring(colonIndex + 1).Trim();
                    }
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取外键ID
        /// </summary>
        /// <param name="foreignKeyValue">外键值</param>
        /// <param name="relatedTableName">关联表名</param>
        /// <param name="relatedTableField">关联表字段</param>
        /// <returns>外键ID</returns>
        private object GetForeignKeyId(string foreignKeyValue, string relatedTableName, string relatedTableField)
        {
            try
            {
                // 构建查询SQL，假设关联表的主键字段为ID
                string sql = $"SELECT ID FROM {relatedTableName} WHERE {relatedTableField} = @value";
                var parameters = new { value = foreignKeyValue };

                // 执行查询
                var result = _db.Ado.GetDataTable(sql, parameters);
                if (result != null && result.Rows.Count > 0)
                {
                    return result.Rows[0]["ID"];
                }

                // 如果没有找到，返回null
                return null;
            }
            catch
            {
                // 如果查询失败，返回null
                return null;
            }
        }

        /// <summary>
        /// 获取自身引用字段的值
        /// 从已导入的数据中查找引用值
        /// </summary>
        /// <param name="displayValue">显示值</param>
        /// <param name="tableName">表名</param>
        /// <param name="selfReferenceField">自身引用字段</param>
        /// <returns>自身引用值</returns>
        private object GetSelfReferenceValue(string displayValue, string tableName, string selfReferenceField)
        {
            try
            {
                // 构建查询SQL
                string sql = $"SELECT {selfReferenceField} FROM {tableName} WHERE ID = @value";
                var parameters = new { value = displayValue };

                // 执行查询
                var result = _db.Ado.GetDataTable(sql, parameters);
                if (result != null && result.Rows.Count > 0)
                {
                    return result.Rows[0][selfReferenceField];
                }

                // 如果没有找到，返回null
                return null;
            }
            catch
            {
                // 如果查询失败，返回null
                return null;
            }
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
        /// 使用Validator进行业务验证
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>错误消息，如果验证通过则返回空字符串</returns>
        private string ValidateEntityWithValidator(object entity, Type entityType)
        {
            try
            {
                Type actualEntityType = entity.GetType();

                // 根据命名规则构造验证器类型名: 实体名 + Validator
                string validatorName = actualEntityType.Name + "Validator";
                
                // 在 RUINORERP.Business.Validator 命名空间下查找验证器
                Type validatorType = Type.GetType($"RUINORERP.Business.Validator.{validatorName}");

                if (validatorType != null)
                {
                    try
                    {
                        // 创建验证器实例
                        // 验证器构造函数需要 ApplicationContext 参数，传 null 使用默认值
                        var validatorInstance = Activator.CreateInstance(validatorType, new object[] { null });

                        // 获取验证器的 Validate 方法
                        MethodInfo validateMethod = validatorType.GetMethod("Validate",
                            BindingFlags.Public | BindingFlags.Instance);

                        if (validateMethod != null)
                        {
                            // 调用验证方法
                            var validationResult = validateMethod.Invoke(validatorInstance, new[] { entity });

                            // 处理验证结果
                            if (validationResult != null)
                            {
                                Type validationResultType = validationResult.GetType();

                                // 检查是否有验证错误
                                PropertyInfo errorsProperty = validationResultType.GetProperty("Errors");
                                if (errorsProperty != null)
                                {
                                    var errors = errorsProperty.GetValue(validationResult);
                                    if (errors != null)
                                    {
                                        Type errorsType = errors.GetType();
                                        PropertyInfo countProperty = errorsType.GetProperty("Count");
                                        if (countProperty != null)
                                        {
                                            int errorCount = (int)countProperty.GetValue(errors);
                                            if (errorCount > 0)
                                            {
                                                // 获取第一个错误信息
                                                MethodInfo indexerMethod = errorsType.GetMethod("get_Item");
                                                if (indexerMethod != null)
                                                {
                                                    var firstError = indexerMethod.Invoke(errors, new object[] { 0 });
                                                    PropertyInfo errorMessageProperty = firstError.GetType().GetProperty("ErrorMessage");
                                                    if (errorMessageProperty != null)
                                                    {
                                                        return errorMessageProperty.GetValue(firstError)?.ToString() ?? "验证失败";
                                                    }
                                                }
                                                return "验证失败";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // 验证器调用失败，记录日志但不阻止导入
                        MainForm.Instance.ShowStatusText($"验证器调用失败: {ex.Message}");
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return $"验证过程出错: {ex.Message}";
            }
        }

        /// <summary>
        /// <summary>
        /// 批量导入实体到数据库
        /// 使用Storageable进行批量插入和更新操作，提升性能
        /// </summary>
        /// <param name="entityList">实体列表</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="result">导入结果</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="importType">导入类型标识（用于区分客户和供应商等使用相同表的情况）</param>
        private async System.Threading.Tasks.Task BatchImportEntitiesAsync(List<BaseEntity> entityList, Type entityType, ImportResult result, ColumnMappingCollection mappings, string importType = null)
        {
            try
            {
                // 从映射配置或FieldMetadata获取主键字段信息
                string primaryKeyName = GetPrimaryKeyFieldName(entityType);

                // 使用反射调用泛型的Storageable方法
                var method = typeof(DynamicImporter).GetMethod(nameof(BatchImportEntitiesInternalAsync),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var genericMethod = method.MakeGenericMethod(entityType);

                await (System.Threading.Tasks.Task)genericMethod.Invoke(this, new object[] { entityList, primaryKeyName, result, importType });
            }
            catch (Exception ex)
            {
                throw new Exception($"批量导入失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取实体类型的主键字段名
        /// 优先从映射配置获取，其次从FieldMetadata获取
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>主键字段名</returns>
        private string GetPrimaryKeyFieldName(Type entityType)
        {
            // 从FieldMetadata获取主键字段
            var metadata = FieldMetadataService.GetFieldMetadata(entityType);
            var primaryKeyMetadata = metadata.Values.FirstOrDefault(m => m.IsPrimaryKey);

            if (primaryKeyMetadata != null)
            {
                return primaryKeyMetadata.FieldName;
            }

            // 如果没有找到，尝试从entityInfo获取
            var entityInfo = _entityInfoService.GetEntityInfoByTableName(entityType.Name);
            return entityInfo?.IdField ?? "ID";
        }

        /// <summary>
        /// 批量导入实体内部实现（泛型方法）
        /// 使用Storageable进行批量插入和更新，主键>0时更新，主键<=0时插入
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityList">实体列表</param>
        /// <param name="primaryKeyName">主键字段名</param>
        /// <param name="result">导入结果</param>
        private async System.Threading.Tasks.Task BatchImportEntitiesInternalAsync<T>(List<BaseEntity> entityList, string primaryKeyName, ImportResult result, string importType = null) where T : BaseEntity, new()
        {
            try
            {
                // 将BaseEntity列表转换为强类型列表
                var typedList = entityList.Cast<T>().ToList();

                // 导入前处理特殊字段
                foreach (var entity in typedList)
                {
                    EntityImportHelper.PreProcessEntity(typeof(T), entity, _db, importType);
                }

                // 开启事务
                _db.Ado.BeginTran();

                try
                {
                    // 使用Storageable进行批量操作
                    // 根据主键值判断：主键>0时更新，主键<=0时插入
                    var storage = await _db.Storageable(typedList).ToStorageAsync();

                    // 执行插入操作（主键<=0的记录），返回雪花ID列表
                    List<long> insertedIds = await storage.AsInsertable.ExecuteReturnSnowflakeIdListAsync();
                    result.InsertedCount += insertedIds.Count;

                    // 执行更新操作（主键>0的记录）
                    int updatedCount = await storage.AsUpdateable.ExecuteCommandAsync();
                    result.UpdatedCount += updatedCount;

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
            catch (Exception ex)
            {
                throw new Exception($"批量导入实体失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查值是否为默认值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>是否为默认值</returns>
        private bool IsDefaultValue(object value)
        {
            if (value == null)
            {
                return true;
            }

            Type type = value.GetType();

            if (type == typeof(int) || type == typeof(long) || type == typeof(short))
            {
                return Convert.ToInt64(value) == 0;
            }
            else if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            {
                return Convert.ToDouble(value) == 0;
            }
            else if (type == typeof(string))
            {
                return string.IsNullOrEmpty(value.ToString());
            }
            else if (type == typeof(DateTime))
            {
                return Convert.ToDateTime(value) == DateTime.MinValue;
            }

            return false;
        }

        /// <summary>
        /// 将数据行转换为字典
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>字典</returns>
        private Dictionary<string, object> RowToDictionary(DataRow row)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in row.Table.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            return dict;
        }

        /// <summary>
        /// 将实体对象转换为字典
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>字典</returns>
        private Dictionary<string, object> EntityToDictionary(object entity)
        {
            var dict = new Dictionary<string, object>();
            Type entityType = entity.GetType();

            foreach (var property in entityType.GetProperties())
            {
                try
                {
                    object value = property.GetValue(entity);
                    dict[property.Name] = value;
                }
                catch
                {
                    // 忽略无法读取的属性
                }
            }

            return dict;
        }
    }
}
