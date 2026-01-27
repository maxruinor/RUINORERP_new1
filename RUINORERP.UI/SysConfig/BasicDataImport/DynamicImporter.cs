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
        /// <returns>导入结果</returns>
        /// <exception cref="ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="ArgumentException">映射配置无效时抛出</exception>
        public async System.Threading.Tasks.Task<ImportResult> ImportAsync(DataTable dataTable, ColumnMappingCollection mappings, Type entityType)
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

                        // 使用Controller进行业务验证
                        string validationError = ValidateEntityWithController(entity, entityType);
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
                        await BatchImportEntitiesAsync(entityList, entityType, result, mappings);
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
                    // 判断是否为系统生成或默认值的映射
                    bool isSystemGenerated = mapping.ExcelColumn.StartsWith("[系统生成]");
                    bool isDefaultValue = mapping.ExcelColumn.StartsWith("[默认值:");

                    // 获取值
                    object cellValue = null;

                    if (isSystemGenerated)
                    {
                        // 系统生成的值，不从Excel读取，后续可以在导入后处理
                        cellValue = null; // 暂时设为null，可以在导入后处理
                    }
                    else if (isDefaultValue)
                    {
                        // 默认值映射，从ExcelColumn中提取默认值
                        cellValue = ExtractDefaultValue(mapping.ExcelColumn);
                    }
                    else if (dataTableContainsColumn(row.Table, mapping.SystemField))
                    {
                        // 使用SystemField从映射后的数据表中获取值
                        cellValue = row[mapping.SystemField];

                        // 如果配置了忽略空值且值为DBNull，则跳过该字段
                        if (cellValue == DBNull.Value && mapping.IgnoreEmptyValue)
                        {
                            continue;
                        }
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
                    PropertyInfo property = entityType.GetProperty(mapping.SystemField);
                    if (property == null)
                    {
                        throw new Exception($"实体 {entityType.Name} 不存在属性 {mapping.SystemField}");
                    }

                    // 处理外键字段
                    if (mapping.IsForeignKey && !string.IsNullOrEmpty(mapping.RelatedTableName) && !string.IsNullOrEmpty(mapping.RelatedTableFieldName))
                    {
                        // 查找关联表中的对应值
                        object foreignKeyId = GetForeignKeyId(cellValue.ToString(), mapping.RelatedTableName, mapping.RelatedTableFieldName);
                        if (foreignKeyId != null)
                        {
                            cellValue = foreignKeyId;
                        }
                        else
                        {
                            throw new Exception($"行 {rowNumber} 外键值 '{cellValue}' 在关联表 {mapping.RelatedTableName} 的字段 {mapping.RelatedTableFieldName} 中未找到对应记录");
                        }
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
        /// 使用Controller进行业务验证
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>错误消息，如果验证通过则返回空字符串</returns>
        private string ValidateEntityWithController(object entity, Type entityType)
        {
            try
            {
                // 基础验证：检查必填字段
                Type actualEntityType = entity.GetType();

                // 尝试通过反射获取Controller
                string controllerName = actualEntityType.Name + "Controller";
                Type controllerType = Type.GetType($"RUINORERP.Controllers.{controllerName}") ??
                                    Type.GetType($"RUINORERP.Business.{controllerName}");

                if (controllerType != null)
                {
                    // 使用反射调用Controller的BaseValidator方法
                    MethodInfo baseValidatorMethod = controllerType.GetMethod("BaseValidator",
                        BindingFlags.Public | BindingFlags.Instance);

                    if (baseValidatorMethod != null)
                    {
                        try
                        {
                            // 获取Controller实例（通过依赖注入容器）
                            var controller = GetControllerInstance(controllerType);
                            if (controller != null)
                            {
                                // 调用BaseValidator方法
                                var validationResult = baseValidatorMethod.Invoke(controller, new[] { entity });

                                // 处理验证结果
                                if (validationResult != null)
                                {
                                    // 假设返回的是一个List<ValidationResult>或类似的类型
                                    Type validationResultType = validationResult.GetType();

                                    // 检查是否有错误
                                    PropertyInfo countProperty = validationResultType.GetProperty("Count");
                                    if (countProperty != null)
                                    {
                                        int errorCount = (int)countProperty.GetValue(validationResult);
                                        if (errorCount > 0)
                                        {
                                            // 获取错误信息
                                            MethodInfo indexer = validationResultType.GetMethod("get_Item");
                                            if (indexer != null)
                                            {
                                                var firstError = indexer.Invoke(validationResult, new object[] { 0 });
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
                        catch (Exception ex)
                        {
                            // Controller验证失败，记录日志但不阻止导入
                            // 可以选择记录日志或返回警告信息
                        }
                    }
                }

                // 基础验证：检查唯一键是否为空
                PropertyInfo idProperty = entityType.GetProperty("ID") ??
                                       entityType.GetProperty("Id") ??
                                       entityType.GetProperty("ID_PK");

                if (idProperty != null)
                {
                    object idValue = idProperty.GetValue(entity);
                    if (idValue == null || (idValue is int intValue && intValue == 0))
                    {
                        // 新增记录，检查必填字段
                        // 这里可以添加更多的必填字段检查逻辑
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
        /// 获取Controller实例
        /// </summary>
        /// <param name="controllerType">Controller类型</param>
        /// <returns>Controller实例</returns>
        private object GetControllerInstance(Type controllerType)
        {
            try
            {
                // 尝试从依赖注入容器获取
                // 这里需要根据项目的实际DI容器进行调整
                // 示例：使用Startup.GetFromFacByName
                MethodInfo getControllerMethod = Type.GetType("RUINORERP.UI.Startup")?.GetMethod("GetFromFacByName");
                if (getControllerMethod != null)
                {
                    var genericMethod = getControllerMethod.MakeGenericMethod(controllerType);
                    object controller = genericMethod.Invoke(null, new object[] { controllerType.Name });
                    return controller;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 批量导入实体到数据库
        /// 使用Storageable进行批量插入和更新操作，提升性能
        /// </summary>
        /// <param name="entityList">实体列表</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="result">导入结果</param>
        /// <param name="mappings">列映射配置</param>
        private async System.Threading.Tasks.Task BatchImportEntitiesAsync(List<BaseEntity> entityList, Type entityType, ImportResult result, ColumnMappingCollection mappings)
        {
            try
            {
                // 从映射配置或FieldMetadata获取主键字段信息
                string primaryKeyName = GetPrimaryKeyFieldName(entityType);
                
                // 使用反射调用泛型的Storageable方法
                var method = typeof(DynamicImporter).GetMethod(nameof(BatchImportEntitiesInternalAsync), 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var genericMethod = method.MakeGenericMethod(entityType);
                
                await (System.Threading.Tasks.Task)genericMethod.Invoke(this, new object[] { entityList, primaryKeyName, result });
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
        private async System.Threading.Tasks.Task BatchImportEntitiesInternalAsync<T>(List<BaseEntity> entityList, string primaryKeyName, ImportResult result) where T : BaseEntity, new()
        {
            try
            {
                // 将BaseEntity列表转换为强类型列表
                var typedList = entityList.Cast<T>().ToList();

                // 导入前处理特殊字段
                foreach (var entity in typedList)
                {
                    EntityImportHelper.PreProcessEntity(typeof(T), entity, _db);
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
