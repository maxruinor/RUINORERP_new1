using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Models;
using SqlSugar;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 智能导入引擎 - 简化版
    /// 移除IdRemappingEngine和DataSplitterService依赖，直接使用数据库操作
    /// </summary>
    public class SmartImportEngine : ISmartImportEngine
    {
        private readonly string _profileDirectory;
        private readonly ISqlSugarClient _db;
        private readonly ExcelParserService _excelParser;
        private readonly ColumnMappingService _mappingService;
        private readonly DatabaseWriterService _dbWriter;

        public SmartImportEngine(ISqlSugarClient db = null)
        {
            _profileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles");
            if (!Directory.Exists(_profileDirectory))
            {
                Directory.CreateDirectory(_profileDirectory);
            }
            
            _db = db ?? throw new ArgumentNullException(nameof(db), "请传入 SqlSugarClient 实例");
            
            // 初始化服务实例
            _excelParser = new ExcelParserService();
            _mappingService = new ColumnMappingService();
            _dbWriter = new DatabaseWriterService(_db);
        }

        public List<string> GetAvailableProfiles()
        {
            if (!Directory.Exists(_profileDirectory)) return new List<string>();
            
            return Directory.GetFiles(_profileDirectory, "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public async Task<DataTable> PreviewDataAsync(string filePath, string profileName, int maxRows = 50)
        {
            // 加载配置获取表索引
            var profilePath = Path.Combine(_profileDirectory, $"{profileName}.json");
            if (!File.Exists(profilePath))
            {
                throw new FileNotFoundException($"未找到导入方案配置文件: {profileName}");
            }

            var json = File.ReadAllText(profilePath);
            var profile = JsonConvert.DeserializeObject<ImportProfile>(json);
            int sheetIndex = profile?.SheetIndex ?? 0;

            // 解析Excel
            return await _excelParser.ParseAsync(filePath, sheetIndex, maxRows);
        }

        public async Task<DataTable> ParseExcelAsync(string filePath, int sheetIndex = 0)
        {
            return await _excelParser.ParseAsync(filePath, sheetIndex);
        }

        /// <summary>
        /// 执行导入任务（使用已解析的DataTable）
        /// </summary>
        public async Task<ImportReport> ExecuteWithDataTableAsync(DataTable data, ImportProfile profile)
        {
            var report = new ImportReport();
            try
            {
                // 映射数据
                var mappedData = _mappingService.MapData(data, profile);
                report.TotalRows = mappedData.Rows.Count;

                // 写入数据库
                report.SuccessRows = await _dbWriter.BatchUpsertAsync(mappedData, profile, null);
                
                report.IsSuccess = true;
                report.Message = $"表 [{profile.TargetTable}] 成功处理 {report.SuccessRows} 行。";
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = ex.Message;
            }
            return report;
        }

        /// <summary>
        /// 执行导入任务（直接传入配置对象）
        /// </summary>
        public async Task<ImportReport> ExecuteWithProfileAsync(string filePath, ImportProfile profile)
        {
            var report = new ImportReport();
            try
            {
                // 解析并映射数据
                var rawData = await _excelParser.ParseAsync(filePath, profile.SheetIndex);
                var mappedData = _mappingService.MapData(rawData, profile);

                report.TotalRows = mappedData.Rows.Count;

                // 执行数据库写入
                report.SuccessRows = await _dbWriter.BatchUpsertAsync(mappedData, profile, null);
                
                report.IsSuccess = true;
                report.Message = $"导入成功，共处理 {report.SuccessRows} / {report.TotalRows} 行数据。";
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// 执行导入任务（兼容重载，保留接口）
        /// 【已废弃】此重载忽略remapper参数，仅为兼容旧代码
        /// </summary>
        [Obsolete("请使用 ExecuteWithDataTableAsync(DataTable data, ImportProfile profile)，remapper参数已废弃")]
        public async Task<ImportReport> ExecuteWithDataTableAsync(DataTable data, ImportProfile profile, object remapper)
        {
            // 忽略remapper参数，使用简化实现
            return await ExecuteWithDataTableAsync(data, profile);
        }

        public async Task<ImportReport> ExecuteAsync(string filePath, string profileName, bool isDryRun = false)
        {
            var report = new ImportReport();
            try
            {
                // 加载配置
                var profilePath = Path.Combine(_profileDirectory, $"{profileName}.json");
                if (!File.Exists(profilePath))
                {
                    throw new FileNotFoundException($"未找到导入方案配置文件: {profileName}");
                }

                var json = File.ReadAllText(profilePath);
                var profile = JsonConvert.DeserializeObject<ImportProfile>(json);

                if (profile == null)
                {
                    throw new Exception("配置文件格式错误");
                }

                // 解析并映射数据
                var rawData = await _excelParser.ParseAsync(filePath, profile.SheetIndex);
                var mappedData = _mappingService.MapData(rawData, profile);

                report.TotalRows = mappedData.Rows.Count;

                if (!isDryRun)
                {
                    // 执行数据库写入
                    report.SuccessRows = await _dbWriter.BatchUpsertAsync(mappedData, profile, null);
                    report.Message = $"导入成功，共处理 {report.SuccessRows} / {report.TotalRows} 行数据。";
                }
                else
                {
                    report.SuccessRows = 0;
                    report.Message = $"模拟运行成功，共 {report.TotalRows} 行数据待处理。";
                }
                
                report.IsSuccess = true;
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// 执行宽表导入(支持多表拆分) - 简化实现
        /// 使用宽表Profile配置，按依赖顺序导入
        /// </summary>
        public async Task<ImportReport> ExecuteWideTableImportAsync(string filePath, string profileName)
        {
            var report = new ImportReport();
            try
            {
                // 1. 加载配置
                var profilePath = Path.Combine(_profileDirectory, $"{profileName}.json");
                if (!File.Exists(profilePath))
                {
                    throw new FileNotFoundException($"未找到导入方案配置文件: {profileName}");
                }

                var json = File.ReadAllText(profilePath);
                var wideProfile = JsonConvert.DeserializeObject<WideTableImportProfile>(json);

                if (wideProfile == null || wideProfile.MasterTable == null)
                {
                    throw new Exception("配置文件格式错误，缺少MasterTable配置");
                }

                // 2. 解析Excel
                var rawData = await _excelParser.ParseAsync(filePath, wideProfile.MasterTable.SheetIndex);
                report.TotalRows = rawData.Rows.Count;

                // 3. 使用全局事务按顺序导入
                int totalSuccess = 0;
                await _db.Ado.UseTranAsync(async () =>
                {
                    // 先导入依赖表
                    if (wideProfile.DependencyTables != null)
                    {
                        foreach (var depProfile in wideProfile.DependencyTables)
                        {
                            var depData = ExtractAndMapTableData(rawData, depProfile);
                            if (depData != null && depData.Rows.Count > 0)
                            {
                                var count = await _dbWriter.BatchUpsertAsync(depData, depProfile, null);
                                totalSuccess += count;
                            }
                        }
                    }

                    // 再导入主表
                    var masterData = ExtractAndMapTableData(rawData, wideProfile.MasterTable);
                    if (masterData != null && masterData.Rows.Count > 0)
                    {
                        var count = await _dbWriter.BatchUpsertAsync(masterData, wideProfile.MasterTable, null);
                        totalSuccess += count;
                    }

                    // 最后导入子表
                    if (wideProfile.ChildTables != null)
                    {
                        foreach (var childProfile in wideProfile.ChildTables)
                        {
                            var childData = ExtractAndMapTableData(rawData, childProfile);
                            if (childData != null && childData.Rows.Count > 0)
                            {
                                var count = await _dbWriter.BatchUpsertAsync(childData, childProfile, null);
                                totalSuccess += count;
                            }
                        }
                    }
                });

                report.SuccessRows = totalSuccess;
                report.IsSuccess = true;
                report.Message = $"宽表导入成功，共处理 {totalSuccess} 条记录。";
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// 分步导入 - 仅导入依赖表(基础数据)
        /// 用于策略二: 先手动导入基础表
        /// </summary>
        public async Task<ImportReport> ImportDependencyTablesOnlyAsync(string filePath, string profileName)
        {
            var report = new ImportReport();
            try
            {
                var profilePath = Path.Combine(_profileDirectory, $"{profileName}.json");
                if (!File.Exists(profilePath))
                {
                    throw new FileNotFoundException($"未找到配置文件: {profileName}");
                }

                var json = File.ReadAllText(profilePath);
                var wideProfile = JsonConvert.DeserializeObject<WideTableImportProfile>(json);

                if (wideProfile.DependencyTables == null || !wideProfile.DependencyTables.Any())
                {
                    throw new Exception("该配置没有依赖表配置");
                }

                // 解析Excel
                var sheetIndex = wideProfile.DependencyTables[0].SheetIndex;
                var rawData = await _excelParser.ParseAsync(filePath, sheetIndex);
                report.TotalRows = rawData.Rows.Count;

                // 仅处理依赖表
                foreach (var depProfile in wideProfile.DependencyTables)
                {
                    var depData = ExtractAndMapTableData(rawData, depProfile);
                    if (depData != null && depData.Rows.Count > 0)
                    {
                        var count = await _dbWriter.BatchUpsertAsync(depData, depProfile, null);
                        report.SuccessRows += count;
                    }
                }

                report.IsSuccess = true;
                report.Message = $"依赖表导入成功，共处理 {report.SuccessRows} 条记录。";
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// 分步导入 - 仅导入主表(需先导入依赖表)
        /// 用于策略二: 基础数据已存在,直接导入主表
        /// </summary>
        public async Task<ImportReport> ImportMasterTableOnlyAsync(string filePath, string profileName)
        {
            var report = new ImportReport();
            try
            {
                var profilePath = Path.Combine(_profileDirectory, $"{profileName}.json");
                if (!File.Exists(profilePath))
                {
                    throw new FileNotFoundException($"未找到配置文件: {profileName}");
                }

                var json = File.ReadAllText(profilePath);
                var wideProfile = JsonConvert.DeserializeObject<WideTableImportProfile>(json);

                if (wideProfile.MasterTable == null)
                {
                    throw new Exception("该配置没有主表配置");
                }

                // 解析Excel
                var rawData = await _excelParser.ParseAsync(filePath, wideProfile.MasterTable.SheetIndex);
                report.TotalRows = rawData.Rows.Count;

                // 提取主表数据
                var masterData = ExtractAndMapTableData(rawData, wideProfile.MasterTable);
                
                if (masterData != null && masterData.Rows.Count > 0)
                {
                    var count = await _dbWriter.BatchUpsertAsync(masterData, wideProfile.MasterTable, null);
                    report.SuccessRows = count;
                }

                report.IsSuccess = true;
                report.Message = $"主表导入成功，共处理 {report.SuccessRows} 条记录。";
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// 从原始数据中提取并映射指定表的数据
        /// </summary>
        private DataTable ExtractAndMapTableData(DataTable rawData, ImportProfile profile)
        {
            if (rawData == null || profile == null) return null;

            // 获取业务键列表（用于判断行是否有效）
            var businessKeys = profile.BusinessKeys ?? new List<string>();
            
            // 根据配置筛选需要的列
            var result = rawData.Clone();
            
            foreach (DataRow row in rawData.Rows)
            {
                // 检查是否应该包含此行（基于BusinessKeys判断）
                bool shouldInclude = false;
                
                if (businessKeys.Any())
                {
                    // 有业务键配置，检查该行是否有值
                    foreach (var key in businessKeys)
                    {
                        // 尝试在映射中查找对应的Excel列
                        var mapping = profile.ColumnMappings?.FirstOrDefault(m => m.DbColumn == key);
                        if (mapping != null)
                        {
                            var value = row[GetExcelColumnIndex(rawData, mapping.ExcelHeader)];
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                shouldInclude = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // 没有业务键，所有非空行都包含
                    shouldInclude = true;
                }

                if (shouldInclude)
                {
                    result.ImportRow(row);
                }
            }

            // 映射列
            return _mappingService.MapData(result, profile);
        }

        /// <summary>
        /// 获取Excel列的索引
        /// </summary>
        private int GetExcelColumnIndex(DataTable dt, string columnName)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取AI智能列映射建议
        /// </summary>
        public async Task<RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult> GetAiMappingSuggestionsAsync(System.Collections.Generic.List<string> excelHeaders, Type targetEntityType)
        {
            try
            {
                if (targetEntityType == null || !typeof(RUINORERP.Model.BaseEntity).IsAssignableFrom(targetEntityType))
                {
                    return new RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult();
                }

                var instance = Activator.CreateInstance(targetEntityType) as RUINORERP.Model.BaseEntity;
                if (instance == null) return new RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult();

                var dbFields = instance.ImportableFields.Select(f => new RUINORERP.Model.ImportFieldInfo
                {
                    ColumnName = f.ColumnName,
                    Description = f.Description,
                    IsPrimaryKey = f.IsPrimaryKey,
                    IsForeignKey = f.IsForeignKey
                }).ToList();

                var aiService = new RUINORERP.Business.AIServices.DataImport.ColumnMappingService();
                return await aiService.AnalyzeWithMetadataAsync(excelHeaders, dbFields);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AI Mapping] 发生异常: {ex.Message}\n{ex.StackTrace}");
                return new RUINORERP.Business.AIServices.DataImport.IntelligentMappingResult();
            }
        }
    }
}
