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
        /// <param name="imageOutputDirectory">图片输出目录（可选）</param>
        public DynamicImporter(ISqlSugarClient db, IUnitOfWorkManage unitOfWorkManage = null, IForeignKeyService foreignKeyService = null, string imageOutputDirectory = null)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _unitOfWorkManage = unitOfWorkManage;  // ✅ 支持传入事务管理器
            _foreignKeyService = foreignKeyService ?? new ForeignKeyService(db);
            _excelParser = new DynamicExcelParser();
            _imageOutputDirectory = imageOutputDirectory;
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
        /// 重复值检测结果
        /// </summary>
        public class DuplicateValueCheckResult
        {
            /// <summary>
            /// 列名
            /// </summary>
            public string ColumnName { get; set; }

            /// <summary>
            /// 列显示名
            /// </summary>
            public string DisplayName { get; set; }

            /// <summary>
            /// 是否有重复值
            /// </summary>
            public bool HasDuplicates { get; set; }

            /// <summary>
            /// 重复的值列表（每个值及其出现次数）
            /// </summary>
            public Dictionary<object, int> DuplicateValues { get; set; } = new Dictionary<object, int>();

            /// <summary>
            /// 重复值的数量
            /// </summary>
            public int DuplicateCount => DuplicateValues?.Count ?? 0;
        }

        /// <summary>
        /// 检测数据表中指定列的重复值
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="columnName">列名（Excel列名或系统字段名）</param>
        /// <returns>重复值检测结果</returns>
        public DuplicateValueCheckResult CheckDuplicateValues(DataTable dataTable, ColumnMappingCollection mappings, string columnName = null)
        {
            var result = new DuplicateValueCheckResult();

            if (dataTable == null || dataTable.Rows.Count == 0 || mappings == null || mappings.Count == 0)
            {
                return result;
            }

            try
            {
                ColumnMapping targetMapping = null;

                if (!string.IsNullOrEmpty(columnName))
                {
                    // 根据指定的列名查找映射
                    targetMapping = mappings.FirstOrDefault(m => 
                        m.ExcelColumn?.Equals(columnName, StringComparison.OrdinalIgnoreCase) == true ||
                        m.SystemField?.Key?.Equals(columnName, StringComparison.OrdinalIgnoreCase) == true);
                }
                else
                {
                    // 自动查找唯一值列
                    targetMapping = mappings.FirstOrDefault(m =>  m.IsUniqueValue);
                }

                if (targetMapping == null)
                {
                    return result;
                }

                result.ColumnName = targetMapping.SystemField?.Key ?? targetMapping.ExcelColumn;
                result.DisplayName = targetMapping.SystemField?.Value ?? result.ColumnName;

                if (string.IsNullOrEmpty(result.ColumnName))
                {
                    return result;
                }

                // 统计每个值的出现次数
                var valueCount = new Dictionary<string, int>();
                string excelColumnName = targetMapping.ExcelColumn;

                if (dataTable.Columns.Contains(excelColumnName))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var value = row[excelColumnName]?.ToString() ?? "";

                        if (!string.IsNullOrEmpty(value))
                        {
                            if (valueCount.ContainsKey(value))
                            {
                                valueCount[value]++;
                            }
                            else
                            {
                                valueCount[value] = 1;
                            }
                        }
                    }

                    // 找出重复的值（出现次数 > 1）
                    foreach (var kvp in valueCount)
                    {
                        if (kvp.Value > 1)
                        {
                            result.DuplicateValues.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                result.HasDuplicates = result.DuplicateValues.Count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"检测重复值失败: {ex.Message}");
            }

            return result;
        }

        
        /// <summary>
        /// 动态导入数据（异步）
        /// ✅ 优化：支持已预处理数据的直接导入，避免重复处理
        /// </summary>
        /// <param name="dataTable">Excel数据表格（可能是已预处理的最终预览数据）</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">目标实体类型</param>
        /// <param name="importType">导入类型标识（用于区分客户和供应商等使用相同表的情况）</param>
        /// <param name="isPreprocessed">✅ 新增：数据是否已在预览阶段预处理过（默认false）</param>
        /// <returns>导入结果</returns>
        /// <exception cref="ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="ArgumentException">映射配置无效时抛出</exception>
        public async System.Threading.Tasks.Task<ImportResult> ImportAsync(DataTable dataTable, ColumnMappingCollection mappings, Type entityType, string importType = null, bool isPreprocessed = false)
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
                        // ✅ 创建实体对象：如果数据已预处理，则跳过外键解析和系统字段生成
                        var entity = CreateEntityFromRow(row, mappings, entityType, i + 2, isPreprocessed);

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
                        // ✅ 传递 isPreprocessed 标志，避免重复预处理
                        await BatchImportEntitiesAsync(entityList, entityType, result, mappings, importType, isPreprocessed);
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
        /// 计算多个配置的导入顺序（根据依赖关系）
        /// </summary>
        /// <param name="configs">导入配置列表</param>
        /// <returns>排序后的配置列表</returns>
        public List<ImportConfiguration> CalculateImportOrder(List<ImportConfiguration> configs)
        {
            if (configs == null || configs.Count == 0)
            {
                return new List<ImportConfiguration>();
            }

            if (configs.Count == 1)
            {
                return new List<ImportConfiguration> { configs[0] };
            }

            var result = new List<ImportConfiguration>();
            var processed = new HashSet<string>();
            var remaining = new List<ImportConfiguration>(configs);
            int maxIterations = configs.Count * configs.Count;
            int iteration = 0;

            while (remaining.Count > 0 && iteration < maxIterations)
            {
                bool foundAny = false;

                foreach (var config in remaining.ToList())
                {
                    string tableName = config.TargetTable?.Key;
                    if (string.IsNullOrEmpty(tableName))
                    {
                        tableName = config.EntityType;
                    }

                    if (string.IsNullOrEmpty(tableName))
                    {
                        result.Add(config);
                        remaining.Remove(config);
                        processed.Add(tableName ?? Guid.NewGuid().ToString());
                        foundAny = true;
                        continue;
                    }

                    bool dependenciesMet = true;
                    if (config.DependentTables != null && config.DependentTables.Count > 0)
                    {
                        foreach (var dep in config.DependentTables)
                        {
                            if (!processed.Contains(dep))
                            {
                                dependenciesMet = false;
                                break;
                            }
                        }
                    }

                    if (dependenciesMet)
                    {
                        result.Add(config);
                        remaining.Remove(config);
                        processed.Add(tableName);
                        foundAny = true;
                    }
                }

                iteration++;

                if (!foundAny && remaining.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"检测到循环依赖或未处理的表：{string.Join(", ", remaining.Select(c => c.TargetTable?.Key))}");
                    break;
                }
            }

            if (remaining.Count > 0)
            {
                result.AddRange(remaining);
            }

            return result;
        }

        /// <summary>
        /// 多个表的批量导入（支持关联表）
        /// </summary>
        /// <param name="configs">导入配置列表（每个配置包含数据表）</param>
        /// <param name="entityTypes">实体类型列表</param>
        /// <returns>导入结果列表</returns>
        public async System.Threading.Tasks.Task<List<ImportResult>> ImportMultipleTablesAsync(
            Dictionary<ImportConfiguration, DataTable> configDataMap,
            Dictionary<ImportConfiguration, Type> configTypeMap)
        {
            var results = new List<ImportResult>();

            try
            {
                var configs = configDataMap.Keys.ToList();
                var orderedConfigs = CalculateImportOrder(configs);

                foreach (var config in orderedConfigs)
                {
                    if (!configDataMap.TryGetValue(config, out DataTable data) ||
                        !configTypeMap.TryGetValue(config, out Type entityType))
                    {
                        continue;
                    }

                    var mappings = new ColumnMappingCollection(config?.ColumnMappings ?? new List<ColumnMapping>());

                    var result = await ImportAsync(data, mappings, entityType, null, isPreprocessed: true);
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"多表导入失败: {ex.Message}");
                throw;
            }

            return results;
        }

        /// <summary>
        /// 从数据行创建实体对象
        /// ✅ 优化：支持已预处理数据的直接导入，避免重复处理
        /// </summary>
        /// <param name="row">数据行（已应用映射，列名为SystemField）</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="rowNumber">行号</param>
        /// <param name="isPreprocessed">✅ 新增：数据是否已在预览阶段预处理过</param>
        /// <returns>实体对象</returns>
        private object CreateEntityFromRow(DataRow row, ColumnMappingCollection mappings, Type entityType, int rowNumber, bool isPreprocessed = false)
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
                            // ✅ 系统生成的值：如果数据已预处理，直接从DataTable中读取；否则留空由BatchPreProcessEntitiesAsync处理
                            if (isPreprocessed && dataTableContainsColumn(row.Table, mapping.SystemField?.Value))
                            {
                                cellValue = row[mapping.SystemField?.Value];
                                
                                // 如果值为占位符或错误标记，跳过（将由BatchPreProcessEntitiesAsync处理）
                                string strValue = cellValue?.ToString();
                                if (string.IsNullOrEmpty(strValue) || 
                                    strValue.StartsWith("[") && strValue.EndsWith("]"))
                                {
                                    cellValue = null; // 让后续预处理逻辑处理
                                }
                            }
                            else
                            {
                                cellValue = null; // 未预处理，留空由后续处理
                            }
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
                            // ✅ 外键关联：如果数据已预处理，直接从DataTable中读取ID；否则查询数据库
                            if (isPreprocessed && dataTableContainsColumn(row.Table, mapping.SystemField?.Value))
                            {
                                // 从预处理的DataTable中直接读取外键ID
                                cellValue = row[mapping.SystemField?.Value];
                                
                                // 如果值为空或错误标记，尝试重新查询
                                string strValue = cellValue?.ToString();
                                if (string.IsNullOrEmpty(strValue) || strValue.StartsWith("["))
                                {
                                    // 回退到实时查询
                                    string foreignKeyError;
                                    object foreignKeyId = _foreignKeyService.GetForeignKeyValue(row, mapping, rowNumber, out foreignKeyError);
                                    if (!string.IsNullOrEmpty(foreignKeyError))
                                    {
                                        throw new Exception(foreignKeyError);
                                    }
                                    cellValue = foreignKeyId;
                                }
                            }
                            else
                            {
                                // 未预处理，实时查询外键ID
                                string foreignKeyError;
                                object foreignKeyId = _foreignKeyService.GetForeignKeyValue(row, mapping, rowNumber, out foreignKeyError);
                                if (!string.IsNullOrEmpty(foreignKeyError))
                                {
                                    throw new Exception(foreignKeyError);
                                }
                                cellValue = foreignKeyId;
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

                    // ✅ 如果数据已预处理，跳过预设字段的检查（因为值已经在预览阶段生成）
                    if (!isPreprocessed && predefinedFields.Contains(mapping.SystemField?.Key))
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
        /// ✅ 增强：使用 ImportValidationAdapter 统一调用现有 FluentValidation 验证器体系
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>错误消息，如果验证通过则返回空字符串</returns>
        private string ValidateEntityWithValidator(object entity, Type entityType)
        {
            try
            {
                // ✅ 使用 ImportValidationAdapter 统一处理验证逻辑
                var validationAdapter = new ImportValidationAdapter(null);
                return validationAdapter.ValidateEntity(entity);
            }
            catch (Exception ex)
            {
                return $"验证过程出错: {ex.Message}";
            }
        }

        /// <summary>
        /// 批量导入实体到数据库
        /// 使用Storageable进行批量插入和更新操作，提升性能
        /// ✅ 优化：支持已预处理数据的直接导入
        /// </summary>
        /// <param name="entityList">实体列表</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="result">导入结果</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="importType">导入类型标识（用于区分客户和供应商等使用相同表的情况）</param>
        /// <param name="isPreprocessed">✅ 新增：数据是否已在预览阶段预处理过</param>
        private async System.Threading.Tasks.Task BatchImportEntitiesAsync(List<BaseEntity> entityList, Type entityType, ImportResult result, ColumnMappingCollection mappings, string importType = null, bool isPreprocessed = false)
        {
            try
            {
                // 从映射配置或FieldMetadata获取主键字段信息
                string primaryKeyName = GetPrimaryKeyFieldName(entityType);

                // 使用反射调用泛型的Storageable方法
                var method = typeof(DynamicImporter).GetMethod(nameof(BatchImportEntitiesInternalAsync),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var genericMethod = method.MakeGenericMethod(entityType);

                await (System.Threading.Tasks.Task)genericMethod.Invoke(this, new object[] { entityList, primaryKeyName, result, importType, isPreprocessed });
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
        /// ✅ 优化：支持已预处理数据的直接导入，避免重复预处理
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityList">实体列表</param>
        /// <param name="primaryKeyName">主键字段名</param>
        /// <param name="result">导入结果</param>
        /// <param name="importType">导入类型标识</param>
        /// <param name="isPreprocessed">✅ 新增：数据是否已在预览阶段预处理过</param>
        private async System.Threading.Tasks.Task BatchImportEntitiesInternalAsync<T>(List<BaseEntity> entityList, string primaryKeyName, ImportResult result, string importType = null, bool isPreprocessed = false) where T : BaseEntity, new()
        {
            try
            {
                // 将BaseEntity列表转换为强类型列表
                var typedList = entityList.Cast<T>().ToList();

                // ✅ 优化：如果数据未预处理，则批量处理特殊字段（一次性查询排序值，避免N次数据库查询）
                // 如果数据已在预览阶段预处理过，则跳过此步骤，直接使用预览中的值
                if (!isPreprocessed)
                {
                    await EntityImportHelper.BatchPreProcessEntitiesAsync<T>(typedList, _db, importType);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"数据已预处理，跳过 BatchPreProcessEntitiesAsync，直接导入 {typedList.Count} 条记录");
                }

                // ✅ 使用 DbClient（从事务管理器获取）1
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

                // ✅ 使用 Storageable 进行批量插入和更新（Upsert）
                // 参考 SqlSugar 文档：https://www.donet5.com/home/Doc?typeId=1193
                var storage = await dbClient.Storageable<T>(typedList).ToStorageAsync();
                
                // ✅ 批量插入新记录，自动生成雪花ID
                // ExecuteReturnPkList<long>() 支持批量返回主键（包括雪花ID），性能优于逐个生成
                var insertIds = await storage.AsInsertable.ExecuteReturnPkListAsync<long>();
                
                // ✅ 批量更新已有记录（如果配置了业务键去重策略为 Update）
                var updateCount = await storage.AsUpdateable.ExecuteCommandAsync();

                result.InsertedCount = insertIds.Count;
                result.UpdatedCount = updateCount;
                
                System.Diagnostics.Debug.WriteLine($"批量导入完成：新增 {insertIds.Count} 条（已分配雪花ID），更新 {updateCount} 条");
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

        /// <summary>
        /// ✅ 统一预处理服务：接收原始 DataTable，返回预处理后的 DataTable
        /// 所有业务逻辑（外键解析、系统字段生成等）都在这里处理
        /// UI层只需调用此方法，然后直接显示结果
        /// </summary>
        /// <param name="rawData">原始解析后的 DataTable</param>
        /// <param name="mappings">列映射配置</param>
        /// <param name="entityType">目标实体类型</param>
        /// <returns>预处理后的 DataTable（可直接用于导入）</returns>
        public async Task<DataTable> PreprocessDataAsync(DataTable rawData, ColumnMappingCollection mappings, Type entityType)
        {
            if (rawData == null || rawData.Rows.Count == 0)
            {
                return new DataTable();
            }

            // 创建结果表结构
            var result = rawData.Clone();
            
            // 预加载外键数据到缓存（性能优化）
            PreloadForeignKeyCache(mappings);

            // 处理每一行数据
            foreach (DataRow sourceRow in rawData.Rows)
            {
                DataRow targetRow = result.NewRow();
                
                // 复制所有原始数据
                foreach (DataColumn col in rawData.Columns)
                {
                    targetRow[col.ColumnName] = sourceRow[col.ColumnName];
                }
                
                // 处理需要预处理的字段
                foreach (var mapping in mappings)
                {
                    string fieldName = mapping.SystemField?.Value;
                    if (string.IsNullOrEmpty(fieldName) || !result.Columns.Contains(fieldName))
                        continue;

                    switch (mapping.DataSourceType)
                    {
                        case DataSourceType.ForeignKey:
                            // 外键关联：查询数据库获取真实ID
                            await ProcessForeignKeyFieldAsync(sourceRow, targetRow, mapping);
                            break;
                            
                        case DataSourceType.SystemGenerated:
                            // 系统生成字段：生成真实值
                            ProcessSystemGeneratedField(targetRow, mapping);
                            break;
                            
                        case DataSourceType.DefaultValue:
                            // 默认值：应用配置的默认值
                            ProcessDefaultValueField(targetRow, mapping);
                            break;
                            
                        case DataSourceType.FieldCopy:
                            // 字段复制：复制另一个字段的值
                            ProcessFieldCopyField(sourceRow, targetRow, mapping);
                            break;
                            
                        case DataSourceType.ColumnConcat:
                            // 列拼接：拼接多个列的值
                            ProcessColumnConcatField(sourceRow, targetRow, mapping);
                            break;
                    }
                }
                
                result.Rows.Add(targetRow);
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 预加载外键数据到缓存
        /// 使用共享的 ForeignKeyService 实例，确保验证和导入流程使用同一个缓存
        /// </summary>
        private void PreloadForeignKeyCache(ColumnMappingCollection mappings)
        {
            if (mappings == null || mappings.Count == 0)
                return;

            // 使用 ForeignKeyService 批量预加载所有外键数据
            _foreignKeyService.PreloadForeignKeyData(mappings);
        }

        /// <summary>
        /// 应用列映射配置转换数据（统一处理方法）
        /// 将Excel列名转换为数据库字段名，并处理各种数据源类型
        /// </summary>
        /// <param name="sourceData">源数据表格（从Excel解析得到）</param>
        /// <param name="mappings">列映射配置集合</param>
        /// <param name="entityType">目标实体类型（用于枚举转换）</param>
        /// <returns>转换后的数据表格（数据库字段名格式）</returns>
        public DataTable ApplyColumnMapping(DataTable sourceData, ColumnMappingCollection mappings, Type entityType = null)
        {
            if (sourceData == null || sourceData.Rows.Count == 0 || mappings == null || mappings.Count == 0)
            {
                return new DataTable();
            }

            // 创建结果表
            DataTable result = new DataTable();

            try
            {
                // 用于跟踪已添加的列，避免重复添加
                HashSet<string> addedColumns = new HashSet<string>();

                // 步骤1：创建结果表结构（使用SystemField.Value作为列名）
                foreach (var mapping in mappings)
                {
                    string columnName = mapping.SystemField?.Value;

                    // 避免重复添加列
                    if (!string.IsNullOrEmpty(columnName) && !addedColumns.Contains(columnName))
                    {
                        result.Columns.Add(columnName, typeof(string));
                        addedColumns.Add(columnName);
                    }

                    // 对于外键关联类型，需要额外添加外键来源列到解析结果中
                    if (mapping.DataSourceType == DataSourceType.ForeignKey &&
                        mapping.ForeignConfig != null &&
                        mapping.ForeignConfig.ForeignKeySourceColumn != null &&
                        !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn.Key))
                    {
                        string sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn.Key;

                        // 检查源数据中是否包含该列
                        if (sourceData.Columns.Contains(sourceColumnName) && !addedColumns.Contains(sourceColumnName))
                        {
                            result.Columns.Add(sourceColumnName, typeof(string));
                            addedColumns.Add(sourceColumnName);
                        }
                    }
                }

                // 步骤2：转换数据行
                foreach (DataRow sourceRow in sourceData.Rows)
                {
                    DataRow targetRow = result.NewRow();

                    foreach (var mapping in mappings)
                    {
                        switch (mapping.DataSourceType)
                        {
                            case DataSourceType.Excel:
                                // Excel数据源：直接从Excel列读取数据
                                string excelColumnName = mapping.SystemField?.Value;
                                if (sourceData.Columns.Contains(excelColumnName))
                                {
                                    object cellValue = sourceRow[excelColumnName];
                                    bool isEmpty = cellValue == DBNull.Value || string.IsNullOrEmpty(cellValue?.ToString());

                                    if (mapping.IgnoreEmptyValue && isEmpty)
                                    {
                                        targetRow[mapping.SystemField?.Value] = DBNull.Value;
                                    }
                                    else
                                    {
                                        // 如果是图片列，处理图片
                                        if (mapping.IsImageColumn && _imageOutputDirectory != null)
                                        {
                                            string imagePath = cellValue?.ToString();
                                            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                            {
                                                try
                                                {
                                                    string productCode = sourceRow["ProductCode"]?.ToString() ?? "IMG";
                                                    string fileName = $"{productCode}_{Path.GetFileName(imagePath)}";
                                                    string savedPath = Path.Combine(_imageOutputDirectory, fileName);
                                                    
                                                    // 复制图片到输出目录
                                                    File.Copy(imagePath, savedPath, true);
                                                    targetRow[mapping.SystemField?.Value] = savedPath;
                                                }
                                                catch (Exception ex)
                                                {
                                                    System.Diagnostics.Debug.WriteLine($"图片处理失败: {ex.Message}");
                                                    targetRow[mapping.SystemField?.Value] = imagePath;
                                                }
                                            }
                                            else
                                            {
                                                targetRow[mapping.SystemField?.Value] = imagePath ?? "";
                                            }
                                        }
                                        else
                                        {
                                            targetRow[mapping.SystemField?.Value] = cellValue?.ToString() ?? "";
                                        }
                                    }
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Value] = "";
                                }
                                break;

                            case DataSourceType.SystemGenerated:
                                // 系统生成的值：暂时留空或使用特殊标记
                                targetRow[mapping.SystemField?.Value] = "[系统生成]";
                                break;

                            case DataSourceType.DefaultValue:
                                // 默认值映射：使用配置的默认值
                                string defaultValue = mapping.DefaultValue ?? "";
                                // 检查是否需要转换为枚举值
                                if (entityType != null)
                                {
                                    Type enumType = EntityImportHelper.GetPredefinedEnumType(entityType.Name, mapping.SystemField?.Key ?? "");
                                    if (enumType != null && !string.IsNullOrEmpty(defaultValue))
                                    {
                                        try
                                        {
                                            defaultValue = Convert.ChangeType(Enum.Parse(enumType, defaultValue), typeof(int)).ToString();
                                        }
                                        catch
                                        {
                                            // 解析失败保持原值
                                        }
                                    }
                                }
                                targetRow[mapping.SystemField?.Value] = defaultValue;
                                break;

                            case DataSourceType.ForeignKey:
                                // 外键关联：保留源值用于后续处理
                                string foreignKeySourceValue = "";
                                string sourceColumn = mapping.ForeignConfig?.ForeignKeySourceColumn?.Key ?? mapping.ExcelColumn;

                                if (!string.IsNullOrEmpty(sourceColumn) &&
                                    !sourceColumn.StartsWith("[") &&
                                    sourceData.Columns.Contains(sourceColumn))
                                {
                                    foreignKeySourceValue = sourceRow[sourceColumn]?.ToString() ?? "";

                                    // 将外键来源列的值复制到结果表中
                                    if (result.Columns.Contains(sourceColumn))
                                    {
                                        targetRow[sourceColumn] = foreignKeySourceValue;
                                    }
                                }

                                if (!string.IsNullOrEmpty(foreignKeySourceValue))
                                {
                                    string sourceColumnDisplay = mapping.ForeignConfig?.ForeignKeySourceColumn?.Value ?? sourceColumn;
                                    targetRow[mapping.SystemField?.Key] = $"[通过关联外键:{sourceColumnDisplay}:{foreignKeySourceValue}->找{mapping.ForeignConfig?.ForeignKeyTable?.Value}.{mapping.ForeignConfig?.ForeignKeyField?.Value}]";
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Key] = $"[外键关联:{mapping.ForeignConfig?.ForeignKeyTable?.Value}.{mapping.ForeignConfig?.ForeignKeyField?.Value}]";
                                }
                                break;

                            case DataSourceType.SelfReference:
                                // 自身字段引用
                                targetRow[mapping.SystemField?.Value] = $"[自身引用:{mapping.SelfReferenceField?.Value}]";
                                break;

                            case DataSourceType.FieldCopy:
                                // 字段复制：复制同一记录中另一个字段的值
                                if (!string.IsNullOrEmpty(mapping.CopyFromField?.Key))
                                {
                                    var copyFromMapping = mappings.FirstOrDefault(m => m.SystemField?.Key == mapping.CopyFromField?.Key);

                                    if (copyFromMapping != null)
                                    {
                                        if (targetRow.Table.Columns.Contains(copyFromMapping.SystemField?.Value) &&
                                            targetRow[copyFromMapping.SystemField?.Value] != DBNull.Value &&
                                            !string.IsNullOrEmpty(targetRow[copyFromMapping.SystemField?.Value]?.ToString()))
                                        {
                                            object copiedValue = targetRow[copyFromMapping.SystemField?.Value];
                                            targetRow[mapping.SystemField?.Value] = copiedValue?.ToString() ?? "";
                                        }
                                        else if (!string.IsNullOrEmpty(copyFromMapping.ExcelColumn) &&
                                                sourceData.Columns.Contains(copyFromMapping.ExcelColumn))
                                        {
                                            object copiedValue = sourceRow[copyFromMapping.ExcelColumn];
                                            targetRow[mapping.SystemField?.Value] = copiedValue?.ToString() ?? "";
                                        }
                                        else
                                        {
                                            targetRow[mapping.SystemField?.Value] = "[字段复制:源数据为空]";
                                        }
                                    }
                                    else
                                    {
                                        targetRow[mapping.SystemField?.Value] = "[字段复制:找不到源字段]";
                                    }
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Value] = "[字段复制:未设置]";
                                }
                                break;

                            case DataSourceType.ColumnConcat:
                                // 列拼接：将Excel中的多个列值拼接后赋值给目标字段
                                if (mapping.ConcatConfig != null &&
                                    mapping.ConcatConfig.SourceColumns != null &&
                                    mapping.ConcatConfig.SourceColumns.Count >= 2)
                                {
                                    var concatValues = new List<string>();

                                    foreach (var sourceCol in mapping.ConcatConfig.SourceColumns)
                                    {
                                        if (sourceData.Columns.Contains(sourceCol))
                                        {
                                            object cellValue = sourceRow[sourceCol];
                                            string valueStr = cellValue?.ToString() ?? "";

                                            if (mapping.ConcatConfig.TrimWhitespace)
                                            {
                                                valueStr = valueStr.Trim();
                                            }

                                            if (mapping.ConcatConfig.IgnoreEmptyColumns &&
                                                string.IsNullOrEmpty(valueStr))
                                            {
                                                continue;
                                            }

                                            concatValues.Add(valueStr);
                                        }
                                        else
                                        {
                                            if (!mapping.ConcatConfig.IgnoreEmptyColumns)
                                            {
                                                concatValues.Add("");
                                            }
                                        }
                                    }

                                    string concatenatedValue = string.Join(
                                        mapping.ConcatConfig.Separator ?? "",
                                        concatValues);

                                    targetRow[mapping.SystemField?.Value] = concatenatedValue;
                                }
                                else
                                {
                                    targetRow[mapping.SystemField?.Value] = "[列拼接:配置无效]";
                                }
                                break;
                        }
                    }

                    result.Rows.Add(targetRow);
                }

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"应用列映射失败: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// 处理外键字段
        /// </summary>
        private async Task ProcessForeignKeyFieldAsync(DataRow sourceRow, DataRow targetRow, ColumnMapping mapping)
        {
            string sourceColumn = mapping.ForeignConfig?.ForeignKeySourceColumn?.Key ?? mapping.ExcelColumn;
            string targetTable = mapping.ForeignConfig?.ForeignKeyTable?.Value;
            string targetField = mapping.ForeignConfig?.ForeignKeyField?.Value;
            string fieldName = mapping.SystemField?.Value;

            if (string.IsNullOrEmpty(sourceColumn) || string.IsNullOrEmpty(fieldName))
                return;

            if (!sourceRow.Table.Columns.Contains(sourceColumn))
                return;

            string sourceValue = sourceRow[sourceColumn]?.ToString() ?? "";
            if (string.IsNullOrEmpty(sourceValue) || string.IsNullOrEmpty(targetTable) || string.IsNullOrEmpty(targetField))
                return;

            // 查询外键ID
            object foreignKeyId = _foreignKeyService.GetForeignKeyId(sourceValue, targetTable, targetField);
            targetRow[fieldName] = foreignKeyId?.ToString() ?? "";
        }

        /// <summary>
        /// 处理系统生成字段
        /// </summary>
        private void ProcessSystemGeneratedField(DataRow targetRow, ColumnMapping mapping)
        {
            string fieldName = mapping.SystemField?.Value;
            if (string.IsNullOrEmpty(fieldName))
                return;

            var fieldUpper = fieldName.ToUpper();

            // 时间字段
            if (fieldUpper.Contains("CREATETIME") || fieldUpper.Contains("CREATEDTIME"))
            {
                targetRow[fieldName] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (fieldUpper.Contains("CREATEDATE"))
            {
                targetRow[fieldName] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            // 用户字段
            else if (fieldUpper.Contains("CREATEUSER") || fieldUpper.Contains("CREATEBY"))
            {
                var currentUser = Business.BusinessHelper._appContext?.CurUserInfo;
                targetRow[fieldName] = currentUser?.EmployeeId.ToString() ?? "1";
            }
            // 状态字段
            else if (fieldUpper.Contains("STATUS") || fieldUpper.Contains("STATE"))
            {
                targetRow[fieldName] = mapping.DefaultValue ?? "1";
            }
            // 删除标记
            else if (fieldUpper.Contains("ISDELETED") || fieldUpper.Contains("DELETE"))
            {
                targetRow[fieldName] = "0";
            }
        }

        /// <summary>
        /// 处理默认值字段
        /// </summary>
        private void ProcessDefaultValueField(DataRow targetRow, ColumnMapping mapping)
        {
            string fieldName = mapping.SystemField?.Value;
            if (string.IsNullOrEmpty(fieldName))
                return;

            if (mapping.EnumDefaultConfig != null)
            {
                targetRow[fieldName] = mapping.EnumDefaultConfig.EnumValue.ToString();
            }
            else
            {
                targetRow[fieldName] = mapping.DefaultValue ?? "";
            }
        }

        /// <summary>
        /// 处理字段复制
        /// </summary>
        private void ProcessFieldCopyField(DataRow sourceRow, DataRow targetRow, ColumnMapping mapping)
        {
            string fieldName = mapping.SystemField?.Value;
            string copyFromField = mapping.CopyFromField?.Key;
            
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(copyFromField))
                return;

            if (sourceRow.Table.Columns.Contains(copyFromField))
            {
                targetRow[fieldName] = sourceRow[copyFromField]?.ToString() ?? "";
            }
        }

        /// <summary>
        /// 处理列拼接
        /// </summary>
        private void ProcessColumnConcatField(DataRow sourceRow, DataRow targetRow, ColumnMapping mapping)
        {
            string fieldName = mapping.SystemField?.Value;
            if (string.IsNullOrEmpty(fieldName) || mapping.ConcatConfig == null)
                return;

            var values = new List<string>();
            foreach (var sourceCol in mapping.ConcatConfig.SourceColumns)
            {
                if (sourceRow.Table.Columns.Contains(sourceCol))
                {
                    string value = sourceRow[sourceCol]?.ToString() ?? "";
                    if (mapping.ConcatConfig.TrimWhitespace)
                        value = value.Trim();
                    
                    if (!mapping.ConcatConfig.IgnoreEmptyColumns || !string.IsNullOrEmpty(value))
                    {
                        values.Add(value);
                    }
                }
            }

            targetRow[fieldName] = string.Join(mapping.ConcatConfig.Separator ?? "", values);
        }
    }
}
