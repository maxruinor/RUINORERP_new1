using RUINORERP.Business.BizMapperService;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Model.ImportEngine.Enums;
using RUINORERP.Repository.UnitOfWorks;  // ✅ 新增：IUnitOfWorkManage
using RUINORERP.UI.SysConfig.BasicDataImport;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private readonly IUnitOfWorkManage _unitOfWorkManage;  // ✅ 新增：统一事务管理
        private readonly IForeignKeyService _foreignKeyService;
        private readonly DynamicExcelParser _excelParser;
        private string _imageOutputDirectory;
        private ImportConfiguration _currentConfig;  // ✅ 当前导入配置，用于读取业务键等信息

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
            /// 图片导入数量
            /// </summary>
            public int ImageCount { get; set; }

            /// <summary>
            /// 耗时（毫秒）
            /// </summary>
            public long ElapsedMilliseconds { get; set; }

            /// <summary>
            /// 失败记录列表
            /// </summary>
            public List<FailedRecord> FailedRecords { get; set; } = new List<FailedRecord>();

            /// <summary>
            /// 导入的图片路径列表
            /// </summary>
            public List<string> ImportedImagePaths { get; set; } = new List<string>();
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
        /// <param name="unitOfWorkManage">统一事务管理器（推荐）</param>
        /// <param name="foreignKeyService">外键服务（可选，默认创建新实例）</param>
        public DynamicImporter(ISqlSugarClient db, IUnitOfWorkManage unitOfWorkManage = null, IForeignKeyService foreignKeyService = null)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _unitOfWorkManage = unitOfWorkManage;  // ✅ 支持传入事务管理器
            _foreignKeyService = foreignKeyService ?? new ForeignKeyService(db);
            _excelParser = new DynamicExcelParser();
        }

        /// <summary>
        /// 设置当前导入配置
        /// ✅ 用于传递 ImportConfiguration，以便读取业务键等配置信息
        /// </summary>
        /// <param name="config">导入配置对象</param>
        public void SetCurrentConfiguration(ImportConfiguration config)
        {
            _currentConfig = config;
        }

        /// <summary>
        /// 设置图片输出目录
        /// </summary>
        /// <param name="directory">图片输出目录</param>
        public void SetImageOutputDirectory(string directory)
        {
            _imageOutputDirectory = directory;
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
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
                        // 记录更详细的错误信息
                        string detailedError = $"批量导入实体时发生错误: {batchEx.Message}";
                        if (batchEx.InnerException != null)
                        {
                            detailedError += $"\n内部异常: {batchEx.InnerException.Message}";
                        }
                        detailedError += $"\n堆栈跟踪: {batchEx.StackTrace}";
                        
                        throw new Exception(detailedError, batchEx);
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
        /// 从Excel文件导入数据（支持图片提取）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">目标实体类型</param>
        /// <param name="sheetIndex">工作表索引</param>
        /// <param name="headerRowIndex">标题行索引</param>
        /// <param name="importType">导入类型标识</param>
        /// <returns>导入结果</returns>
        public async System.Threading.Tasks.Task<ImportResult> ImportFromExcelAsync(
            string filePath, 
            ColumnMappingCollection mappings, 
            Type entityType, 
            int sheetIndex = 0, 
            int headerRowIndex = 0,
            string importType = null)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new ArgumentException("文件路径无效或文件不存在", nameof(filePath));
            }

            // 检查是否有图片字段映射
            var imageMappings = mappings.Where(m => m.DataSourceType == DataSourceType.ExcelImage || m.IsImageColumn).ToList();
            bool hasImageFields = imageMappings.Count > 0;

            ExcelParseResult parseResult = null;
            DataTable dataTable;

            if (hasImageFields)
            {
                // 使用增强解析器提取图片
                parseResult = _excelParser.Parse(filePath, sheetIndex, headerRowIndex);
                dataTable = parseResult.DataTable;
            }
            else
            {
                // 普通解析，不提取图片
                dataTable = _excelParser.Parse(filePath, sheetIndex, headerRowIndex).DataTable;
            }

            // 执行数据导入
            var result = await ImportAsync(dataTable, mappings, entityType, importType);

            // 处理图片导入
            if (hasImageFields && parseResult != null && parseResult.HasImages)
            {
                try
                {
                    await ProcessImageImportAsync(parseResult, mappings, result, entityType);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"图片导入处理失败: {ex.Message}");
                }
            }

            return result;
        }

        /// <summary>
        /// 处理图片导入
        /// </summary>
        private async System.Threading.Tasks.Task ProcessImageImportAsync(
            ExcelParseResult parseResult, 
            ColumnMappingCollection mappings, 
            ImportResult importResult,
            Type entityType)
        {
            var imageMappings = mappings.Where(m => m.DataSourceType == DataSourceType.ExcelImage || m.IsImageColumn).ToList();
            if (imageMappings.Count == 0) return;

            // 确定图片输出目录
            string outputDir = _imageOutputDirectory;
            if (string.IsNullOrEmpty(outputDir))
            {
                // 使用默认目录
                outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages", entityType.Name);
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            int totalImages = 0;

            // 遍历所有数据行处理图片
            for (int i = 0; i < parseResult.DataTable.Rows.Count; i++)
            {
                var images = parseResult.GetImagesForDataRow(i);
                if (images == null || images.Count == 0) continue;

                var row = parseResult.DataTable.Rows[i];

                foreach (var mapping in imageMappings)
                {
                    var config = mapping.ImageConfig ?? new ExcelImageConfig();
                    string namingColumn = config.NamingReferenceColumn;

                    // 获取图片文件名基础
                    string baseName = GetImageFileName(row, i, config, namingColumn, mappings);

                    // 处理该行的所有图片
                    for (int imgIdx = 0; imgIdx < images.Count; imgIdx++)
                    {
                        var imageInfo = images[imgIdx];
                        string fileName = images.Count > 1 ? $"{baseName}_{imgIdx + 1}" : baseName;

                        // 保存图片
                        string savedPath = _excelParser.SaveImageToFile(imageInfo, outputDir, fileName);
                        if (!string.IsNullOrEmpty(savedPath))
                        {
                            importResult.ImportedImagePaths.Add(savedPath);
                            totalImages++;

                            // 根据存储类型处理图片数据
                            object valueToStore = GetImageStorageValue(savedPath, imageInfo, config);
                            
                            // 更新数据库中的图片字段值
                            await UpdateEntityImageFieldAsync(entityType, row, mapping, valueToStore, mappings);
                        }
                    }
                }
            }

            importResult.ImageCount = totalImages;
        }

        /// <summary>
        /// 获取图片文件名
        /// </summary>
        private string GetImageFileName(DataRow row, int rowIndex, ExcelImageConfig config, string namingColumn, ColumnMappingCollection mappings)
        {
            switch (config.NamingRule)
            {
                case ImageNamingRule.ColumnValue:
                    if (!string.IsNullOrEmpty(namingColumn) && row.Table.Columns.Contains(namingColumn))
                    {
                        string value = row[namingColumn]?.ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            // 清理文件名中的非法字符
                            return string.Join("_", value.Split(Path.GetInvalidFileNameChars()));
                        }
                    }
                    return $"Image_{rowIndex + 1}";

                case ImageNamingRule.Guid:
                    return Guid.NewGuid().ToString("N");

                case ImageNamingRule.Timestamp:
                    return DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + rowIndex;

                case ImageNamingRule.Combined:
                    if (!string.IsNullOrEmpty(namingColumn) && row.Table.Columns.Contains(namingColumn))
                    {
                        string value = row[namingColumn]?.ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            string cleanValue = string.Join("_", value.Split(Path.GetInvalidFileNameChars()));
                            return $"{cleanValue}_{rowIndex + 1}";
                        }
                    }
                    return $"Image_{rowIndex + 1}";

                case ImageNamingRule.AutoIncrement:
                default:
                    return $"Image_{rowIndex + 1}";
            }
        }

        /// <summary>
        /// 根据存储类型获取图片存储值
        /// </summary>
        private object GetImageStorageValue(string filePath, ExcelImageInfo imageInfo, ExcelImageConfig config)
        {
            switch (config.StorageType)
            {
                case ImageStorageType.Base64:
                    return Convert.ToBase64String(imageInfo.ImageData);

                case ImageStorageType.Binary:
                    return imageInfo.ImageData;

                case ImageStorageType.FilePath:
                default:
                    return filePath;
            }
        }

        /// <summary>
        /// 更新实体图片字段
        /// </summary>
        private async System.Threading.Tasks.Task UpdateEntityImageFieldAsync(Type entityType, DataRow row, ColumnMapping mapping, object value, ColumnMappingCollection mappings)
        {
            // 获取唯一标识字段用于定位记录
            var uniqueMapping = mappings.GetUniqueKeyMapping();
            if (uniqueMapping == null) return;

            string uniqueField = uniqueMapping.SystemField?.Key;
            string uniqueValue = row[uniqueMapping.SystemField?.Value]?.ToString();
            if (string.IsNullOrEmpty(uniqueValue)) return;

            string imageField = mapping.SystemField?.Key;
            if (string.IsNullOrEmpty(imageField)) return;

            try
            {
                // 【P0修复】使用更严格的SQL注入防护 - 白名单验证
                string tableName = entityType.Name;
                
                // 1. 首先检查是否为合法的SQL标识符
                if (!IsValidSqlIdentifier(tableName) || !IsValidSqlIdentifier(imageField) || !IsValidSqlIdentifier(uniqueField))
                {
                    throw new ArgumentException("无效的表名或字段名，可能包含非法字符");
                }
                
                // 2. 【P0修复】额外验证：确保表名和字段名来自已知的实体元数据（白名单机制）
                if (!IsKnownEntityField(entityType, imageField) || !IsKnownEntityField(entityType, uniqueField))
                {
                    throw new ArgumentException("字段名不在实体的已知字段列表中，拒绝执行");
                }
                
                // 3. 使用参数化查询（已经实现，保持不变）
                string sql = $"UPDATE {tableName} SET {imageField} = @value WHERE {uniqueField} = @uniqueValue";
                
                await _db.Ado.ExecuteCommandAsync(sql, new { value, uniqueValue });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"更新图片字段失败: {ex.Message}");
                throw; // 【P0修复】重新抛出异常，让调用方处理
            }
        }

        /// <summary>
        /// 验证SQL标识符是否合法（仅允许字母、数字、下划线）
        /// 【P0修复】加强验证规则
        /// </summary>
        private bool IsValidSqlIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return false;
            
            // 【P0修复】更严格的正则表达式：
            // 1. 只能包含字母、数字、下划线
            // 2. 不能以数字开头
            // 3. 长度限制在1-128字符之间
            // 4. 不能包含连续的下划线
            if (identifier.Length < 1 || identifier.Length > 128)
                return false;
                
            if (!System.Text.RegularExpressions.Regex.IsMatch(identifier, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
                return false;
                
            // 检查是否包含连续下划线
            if (identifier.Contains("__"))
                return false;
                
            return true;
        }
        
        /// <summary>
        /// 【P0修复】验证字段是否属于实体的已知字段（白名单机制）
        /// </summary>
        private bool IsKnownEntityField(Type entityType, string fieldName)
        {
            if (entityType == null || string.IsNullOrEmpty(fieldName))
                return false;
            
            try
            {
                // 通过反射检查实体是否有该属性
                var property = entityType.GetProperty(fieldName, 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                return property != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从数据行创建实体对象
        /// </summary>
        /// <param name="row">数据行（已应用映射，列名为SystemField）</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="rowNumber">行号</param>
        /// <returns>实体对象</returns>
        /// <summary>
        /// 从数据行创建实体对象
        /// </summary>
        private object CreateEntityFromRow(DataRow row, ColumnMappingCollection mappings, Type entityType, int rowNumber)
        {
            var entity = Activator.CreateInstance(entityType);

            // 获取该实体类型的预设字段（在导入时会自动填充默认值的字段）
            var predefinedFields = EntityImportHelper.GetPredefinedFields(entityType);

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
                            if (dataTableContainsColumn(row.Table, mapping.SystemField?.Value))
                            {
                                cellValue = row[mapping.SystemField?.Value];

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
                            if (mapping.EnumDefaultConfig != null)
                            {
                                // 如果是枚举类型默认值，使用枚举的数值
                                cellValue = mapping.EnumDefaultConfig.EnumValue;
                            }
                            else
                            {
                                // 普通默认值
                                cellValue = mapping.DefaultValue;
                            }
                            break;

                        case DataSourceType.ForeignKey:
                            // 外键关联
                            // 使用ForeignKeyService获取外键值
                            string foreignKeyError;
                            object foreignKeyId = _foreignKeyService.GetForeignKeyValue(row, mapping, rowNumber, out foreignKeyError);
                            if (!string.IsNullOrEmpty(foreignKeyError))
                            {
                                throw new Exception(foreignKeyError);
                            }
                            cellValue = foreignKeyId;
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
                                // 获取被复制字段的映射配置
                                var copyFromMapping = mappings.FirstOrDefault(m => m.SystemField?.Key == mapping.CopyFromField?.Key);

                                if (copyFromMapping != null && !string.IsNullOrEmpty(copyFromMapping.SystemField?.Value))
                                {
                                    // 从当前行中读取被复制字段的值
                                    if (dataTableContainsColumn(row.Table, copyFromMapping.SystemField.Value))
                                    {
                                        cellValue = row[copyFromMapping.SystemField.Value];
                                    }
                                }
                            }
                            break;

                        case DataSourceType.ColumnConcat:
                            // 列拼接
                            // 在ApplyColumnMapping阶段已经处理了拼接，直接从数据表中读取拼接后的值
                            if (dataTableContainsColumn(row.Table, mapping.SystemField?.Value))
                            {
                                cellValue = row[mapping.SystemField?.Value];
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

                    // 跳过预设字段，这些字段会在PreProcessEntity中自动填充默认值
                    if (predefinedFields.Contains(mapping.SystemField?.Key))
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
                    throw new Exception($"行 {rowNumber} 字段 {mapping.SystemField} (Excel列: {mapping.ExcelColumn}) 转换失败: {ex.Message}", ex);
                }
            }

            return entity;
        }


        /// <summary>
        /// 【已废弃】获取外键ID - 请使用 ForeignKeyService.GetForeignKeyId() 代替
        /// 此方法保留仅为兼容旧代码，将在未来版本中移除
        /// </summary>
        [Obsolete("请使用 ForeignKeyService.GetForeignKeyId() 代替，该方法支持缓存优化")]
        private object GetForeignKeyId(string foreignKeyValue, string relatedTableName, string relatedTableField)
        {
            try
            {
                if (string.IsNullOrEmpty(foreignKeyValue) ||
                    string.IsNullOrEmpty(relatedTableName) ||
                    string.IsNullOrEmpty(relatedTableField))
                {
                    return null;
                }

                // 构建查询SQL，通过代码字段查询主键ID
                // 例如：SELECT ID FROM tb_ProdCategories WHERE CategoryCode = 'CATEGORY001'
                // 例如：SELECT ID FROM tb_CustomerVendor WHERE VendorName = '供应商A'
                string sql = $"SELECT ID FROM {relatedTableName} WHERE {relatedTableField} = @value";

                // 使用参数化查询防止SQL注入
                var parameters = new { value = foreignKeyValue.Trim() };

                // 执行查询
                var result = _db.Ado.GetDataTable(sql, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    // 检查是否有多个匹配记录（唯一性验证）
                    if (result.Rows.Count > 1)
                    {
                        // 对于供应商表等需要唯一性验证的表，记录警告
                        System.Diagnostics.Debug.WriteLine(
                            $"警告：外键查询返回多个结果。表：{relatedTableName}，字段：{relatedTableField}，值：{foreignKeyValue}，匹配数：{result.Rows.Count}");

                        // 如果启用了严格模式，可以抛出异常
                        // throw new Exception($"外键值 '{foreignKeyValue}' 在表 {relatedTableName} 中不唯一，找到 {result.Rows.Count} 条记录");
                    }

                    return result.Rows[0]["ID"];
                }

                // 如果没有找到，尝试模糊匹配（对于供应商名称等可能包含空格的情况）
                if (relatedTableField.ToLower().Contains("name") ||
                    relatedTableField.ToLower().Contains("vendor") ||
                    relatedTableField.ToLower().Contains("supplier"))
                {
                    sql = $"SELECT ID FROM {relatedTableName} WHERE {relatedTableField} LIKE @value";
                    parameters = new { value = $"%{foreignKeyValue.Trim()}%" };
                    result = _db.Ado.GetDataTable(sql, parameters);

                    if (result != null && result.Rows.Count > 0)
                    {
                        if (result.Rows.Count > 1)
                        {
                            System.Diagnostics.Debug.WriteLine(
                                $"警告：模糊匹配返回多个结果。表：{relatedTableName}，字段：{relatedTableField}，值：{foreignKeyValue}");
                        }
                        return result.Rows[0]["ID"];
                    }
                }

                // 如果没有找到，返回null
                return null;
            }
            catch (Exception ex)
            {
                // 记录错误信息
                System.Diagnostics.Debug.WriteLine($"查询外键ID失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取自身引用字段的值
        /// 【待完善】当前仅实现基础查询逻辑，自身引用关系的完整处理需在导入流程中实现
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
                    // 使用 TryParse 避免异常
                    try
                    {
                        var enumValue = Enum.Parse(underlyingType, stringValue, ignoreCase: true);
                        return enumValue;
                    }
                    catch
                    {
                        throw new Exception($"值 '{stringValue}' 不是有效的 {underlyingType.Name} 枚举值");
                    }
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
        /// 从entityInfo获取主键字段信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>主键字段名</returns>
        /// <summary>
        /// 获取实体类型的主键字段名
        /// 【修复】使用 EntityRegistry 或 BaseEntity 动态获取主键，禁止硬编码
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>主键字段名（数据库列名）</returns>
        private string GetPrimaryKeyFieldName(Type entityType)
        {
            if (entityType == null)
            {
                return "ID"; // 默认值
            }

            // 优先从 EntityRegistry 获取（结构化元数据）
            var metadata = RUINORERP.Model.EntityRegistry.Entities
                .FirstOrDefault(e => e.EntityType == entityType);

            if (metadata != null && !string.IsNullOrEmpty(metadata.PrimaryKeyName))
            {
                return metadata.PrimaryKeyName;
            }

            // 降级方案：使用 BaseEntity 的动态方法获取主键
            // 创建临时实例调用 GetPrimaryKeyColName()
            try
            {
                if (typeof(RUINORERP.Model.BaseEntity).IsAssignableFrom(entityType))
                {
                    var instance = Activator.CreateInstance(entityType) as RUINORERP.Model.BaseEntity;
                    if (instance != null)
                    {
                        string pkColName = instance.GetPrimaryKeyColName();
                        if (!string.IsNullOrEmpty(pkColName))
                        {
                            return pkColName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"通过 BaseEntity 获取主键失败: {ex.Message}");
            }

            // 最后的降级方案：尝试从 SugarColumn 特性获取
            try
            {
                var pkProperty = entityType.GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttribute<SqlSugar.SugarColumn>()?.IsPrimaryKey == true);

                if (pkProperty != null)
                {
                    var sugarColumn = pkProperty.GetCustomAttribute<SqlSugar.SugarColumn>();
                    return sugarColumn?.ColumnName ?? pkProperty.Name;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"通过反射获取主键失败: {ex.Message}");
            }

            // 最终降级：返回默认值
            return "ID";
        }

        /// <summary>
        /// 批量导入实体内部实现（泛型方法）
        /// ✅ 修复：使用统一事务管理器 + Storageable + 雪花ID
        /// ✅ 增强：支持按业务字段去重（跳过数据库中已存在的记录）
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
                    await EntityImportHelper.PreProcessEntityAsync(typeof(T), entity, _db, importType);
                }

                // ✅ 使用 DbClient（从事务管理器获取）
                var dbClient = _unitOfWorkManage?.GetDbClient() ?? _db;

                // ✅ 检查是否启用数据库级别去重（按业务字段跳过已存在记录）
                var businessKeys = GetBusinessKeysForTable(typeof(T));
                
                if (businessKeys != null && businessKeys.Count > 0)
                {
                    // 根据存在性策略处理记录
                    typedList = await ProcessExistenceCheckAsync<T>(dbClient, typedList, businessKeys);
                }

                if (typedList.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("所有记录均已存在，跳过导入");
                    result.InsertedCount = 0;
                    result.UpdatedCount = 0;
                    return;
                }

                // ✅ 使用 Storageable 进行批量插入和更新（参考 DbHelper.cs）
                var storage = await dbClient.Storageable<T>(typedList).ToStorageAsync();
                
                // ✅ 新增记录自动生成雪花ID
                var insertIds = await storage.AsInsertable.ExecuteReturnSnowflakeIdListAsync();
                
                // ✅ 更新已有记录
                var updateCount = await storage.AsUpdateable.ExecuteCommandAsync();

                result.InsertedCount = insertIds.Count;
                result.UpdatedCount = updateCount;
                
                System.Diagnostics.Debug.WriteLine($"批量导入完成：新增 {insertIds.Count} 条，更新 {updateCount} 条");
            }
            catch (Exception ex)
            {
                throw new Exception($"批量导入实体失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 【未使用】检查值是否为默认值
        /// 当前无调用点，如不需要可删除
        /// </summary>
        [Obsolete("此方法当前无调用点，如不需要可删除")]
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
        /// 【调试用】将实体对象转换为字典
        /// 仅用于调试目的，无业务调用
        /// </summary>
        [Obsolete("仅用于调试，无业务调用")]
        private Dictionary<string, object> EntityToDictionary(object entity)
        {
            var dict = new Dictionary<string, object>();
            Type entityType = entity.GetType();

            foreach (var prop in entityType.GetProperties())
            {
                var value = prop.GetValue(entity);
                if (value != null)
                {
                    dict[prop.Name] = value;
                }
            }
            return dict;
        }

        /// <summary>
        /// 获取表的业务键字段列表
        /// ✅ 从 ImportConfiguration.ColumnMappings 中读取标记为 IsBusinessKey 的字段
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>业务键字段名列表</returns>
        private List<string> GetBusinessKeysForTable(Type entityType)
        {
            if (_currentConfig?.ColumnMappings == null || _currentConfig.ColumnMappings.Count == 0)
            {
                return null;
            }

            // 从配置中筛选出标记为 IsBusinessKey 的字段
            var businessKeys = _currentConfig.ColumnMappings
                .Where(m => m.IsBusinessKey && !string.IsNullOrEmpty(m.SystemField?.Key))
                .Select(m => m.SystemField.Key)
                .ToList();

            return businessKeys.Count > 0 ? businessKeys : null;
        }

        /// <summary>
        /// 根据存在性策略处理记录
        /// ✅ 支持 Skip（跳过）、Update（更新）、Error（报错）三种策略
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dbClient">数据库客户端</param>
        /// <param name="entities">待导入的实体列表</param>
        /// <param name="businessKeys">业务键字段列表</param>
        /// <returns>处理后的实体列表</returns>
        private async System.Threading.Tasks.Task<List<T>> ProcessExistenceCheckAsync<T>(
            ISqlSugarClient dbClient,
            List<T> entities,
            List<string> businessKeys) where T : BaseEntity, new()
        {
            if (entities == null || entities.Count == 0 || businessKeys == null || businessKeys.Count == 0)
            {
                return entities;
            }

            // 获取第一个业务键的配置，以确定存在性策略
            var firstBusinessKeyMapping = _currentConfig?.ColumnMappings
                .FirstOrDefault(m => m.IsBusinessKey && !string.IsNullOrEmpty(m.SystemField?.Key));

            var strategy = firstBusinessKeyMapping?.ExistenceStrategy ?? ExistenceStrategy.Skip;

            switch (strategy)
            {
                case ExistenceStrategy.Skip:
                    return await FilterExistingRecordsAsync(dbClient, entities, businessKeys);
                
                case ExistenceStrategy.Update:
                    // TODO: 实现更新策略 - 标记需要更新的记录
                    System.Diagnostics.Debug.WriteLine("更新策略暂未实现，使用跳过策略");
                    return await FilterExistingRecordsAsync(dbClient, entities, businessKeys);
                
                case ExistenceStrategy.Error:
                    // TODO: 实现报错策略 - 检测冲突并抛出异常
                    await CheckForConflictsAsync(dbClient, entities, businessKeys);
                    return entities; // 如果没有冲突，返回全部
                
                default:
                    return await FilterExistingRecordsAsync(dbClient, entities, businessKeys);
            }
        }

        /// <summary>
        /// 过滤掉数据库中已存在的记录（Skip策略）
        /// ✅ 按业务字段查询数据库，跳过已存在的记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dbClient">数据库客户端</param>
        /// <param name="entities">待导入的实体列表</param>
        /// <param name="businessKeys">业务键字段列表</param>
        /// <returns>过滤后的实体列表（只包含不存在于数据库中的记录）</returns>
        private async System.Threading.Tasks.Task<List<T>> FilterExistingRecordsAsync<T>(
            ISqlSugarClient dbClient,
            List<T> entities,
            List<string> businessKeys) where T : BaseEntity, new()
        {
            if (entities == null || entities.Count == 0 || businessKeys == null || businessKeys.Count == 0)
            {
                return entities;
            }

            var filteredList = new List<T>();
            var tableName = typeof(T).Name;

            // 构建查询条件：按业务键批量查询
            // 例如：WHERE VendorName IN ('供应商1', '供应商2', ...)
            var conditions = new List<string>();
            var parameters = new List<SugarParameter>();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var conditionParts = new List<string>();

                foreach (var keyField in businessKeys)
                {
                    var prop = typeof(T).GetProperty(keyField);
                    if (prop != null)
                    {
                        var value = prop.GetValue(entity);
                        if (value != null)
                        {
                            string paramName = $"@key_{i}_{keyField}";
                            conditionParts.Add($"{keyField} = {paramName}");
                            parameters.Add(new SugarParameter(paramName, value));
                        }
                    }
                }

                if (conditionParts.Count > 0)
                {
                    conditions.Add($"({string.Join(" AND ", conditionParts)})");
                }
            }

            if (conditions.Count == 0)
            {
                return entities; // 没有有效的业务键，返回全部
            }

            // 执行查询：查找数据库中已存在的记录
            string whereClause = string.Join(" OR ", conditions);
            string sql = $"SELECT {string.Join(",", businessKeys)} FROM {tableName} WHERE {whereClause}";

            try
            {
                var existingRecords = await dbClient.Ado.GetDataTableAsync(sql, parameters.ToArray());

                // 构建已存在记录的 HashSet（用于快速查找）
                var existingKeys = new HashSet<string>();
                foreach (System.Data.DataRow row in existingRecords.Rows)
                {
                    var keyValues = new List<string>();
                    foreach (var keyField in businessKeys)
                    {
                        keyValues.Add(row[keyField]?.ToString() ?? "");
                    }
                    existingKeys.Add(string.Join("|", keyValues));
                }

                // 过滤掉已存在的记录
                foreach (var entity in entities)
                {
                    var keyValues = new List<string>();
                    foreach (var keyField in businessKeys)
                    {
                        var prop = typeof(T).GetProperty(keyField);
                        var value = prop?.GetValue(entity)?.ToString() ?? "";
                        keyValues.Add(value);
                    }

                    string entityKey = string.Join("|", keyValues);

                    if (!existingKeys.Contains(entityKey))
                    {
                        filteredList.Add(entity); // 数据库中不存在，保留
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"跳过已存在记录: {entityKey}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"数据库去重（跳过策略）：原始 {entities.Count} 条，过滤后 {filteredList.Count} 条");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"数据库去重失败: {ex.Message}，将导入所有记录");
                return entities; // 出错时返回全部，保证导入不中断
            }

            return filteredList;
        }

        /// <summary>
        /// 检查是否存在冲突记录（Error策略）
        /// ✅ 如果检测到冲突，抛出异常
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dbClient">数据库客户端</param>
        /// <param name="entities">待导入的实体列表</param>
        /// <param name="businessKeys">业务键字段列表</param>
        private async System.Threading.Tasks.Task CheckForConflictsAsync<T>(
            ISqlSugarClient dbClient,
            List<T> entities,
            List<string> businessKeys) where T : BaseEntity, new()
        {
            if (entities == null || entities.Count == 0 || businessKeys == null || businessKeys.Count == 0)
            {
                return;
            }

            var tableName = typeof(T).Name;
            var conflicts = new List<string>();

            // 构建查询条件：按业务键批量查询
            var conditions = new List<string>();
            var parameters = new List<SugarParameter>();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var conditionParts = new List<string>();

                foreach (var keyField in businessKeys)
                {
                    var prop = typeof(T).GetProperty(keyField);
                    if (prop != null)
                    {
                        var value = prop.GetValue(entity);
                        if (value != null)
                        {
                            string paramName = $"@key_{i}_{keyField}";
                            conditionParts.Add($"{keyField} = {paramName}");
                            parameters.Add(new SugarParameter(paramName, value));
                        }
                    }
                }

                if (conditionParts.Count > 0)
                {
                    conditions.Add($"({string.Join(" AND ", conditionParts)})");
                }
            }

            if (conditions.Count == 0)
            {
                return;
            }

            // 执行查询：查找数据库中已存在的记录
            string whereClause = string.Join(" OR ", conditions);
            string sql = $"SELECT {string.Join(",", businessKeys)} FROM {tableName} WHERE {whereClause}";

            try
            {
                var existingRecords = await dbClient.Ado.GetDataTableAsync(sql, parameters.ToArray());

                // 收集冲突记录
                foreach (System.Data.DataRow row in existingRecords.Rows)
                {
                    var keyValues = new List<string>();
                    foreach (var keyField in businessKeys)
                    {
                        keyValues.Add(row[keyField]?.ToString() ?? "");
                    }
                    conflicts.Add(string.Join(" | ", keyValues));
                }

                // 如果有冲突，抛出异常
                if (conflicts.Count > 0)
                {
                    string conflictDetails = string.Join("\n", conflicts.Take(10)); // 最多显示10条
                    if (conflicts.Count > 10)
                    {
                        conflictDetails += $"\n... 还有 {conflicts.Count - 10} 条冲突记录";
                    }
                    throw new Exception($"检测到 {conflicts.Count} 条重复记录，导入中止。\n冲突记录示例：\n{conflictDetails}");
                }
            }
            catch (Exception ex) when (!(ex is Exception))
            {
                // 重新抛出非业务异常
                throw;
            }
        }
    }
}
