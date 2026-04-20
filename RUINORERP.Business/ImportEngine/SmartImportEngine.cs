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
    /// 智能导入引擎实现
    /// </summary>
    public class SmartImportEngine : ISmartImportEngine
    {
        private readonly string _profileDirectory;
        private readonly ExcelParserService _excelParser;
        private readonly ColumnMappingService _mappingService;
        private readonly DatabaseWriterService _dbWriter;
        private readonly ISqlSugarClient _db;

        public SmartImportEngine(ISqlSugarClient db = null)
        {
            _profileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles");
            if (!Directory.Exists(_profileDirectory))
            {
                Directory.CreateDirectory(_profileDirectory);
            }
            _excelParser = new ExcelParserService();
            _mappingService = new ColumnMappingService();
            
            // 必须通过构造函数传入 db 实例，避免 Business 层依赖 UI 层
            _db = db ?? throw new ArgumentNullException(nameof(db), "请传入 SqlSugarClient 实例");
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
            return await _excelParser.ParseAsync(filePath, 0, maxRows);
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
                // 1. 映射数据
                var mappedData = _mappingService.MapData(data, profile);
                report.TotalRows = mappedData.Rows.Count;

                // 2. 写入数据库
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
                // 1. 解析并映射数据
                var rawData = await _excelParser.ParseAsync(filePath);
                var mappedData = _mappingService.MapData(rawData, profile);

                report.TotalRows = mappedData.Rows.Count;

                // 2. 执行数据库写入
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
        /// 执行导入任务（内部方法，支持remapper）
        /// </summary>
        public async Task<ImportReport> ExecuteWithDataTableAsync(DataTable data, ImportProfile profile, IdRemappingEngine remapper)
        {
            var report = new ImportReport();
            try
            {
                // 1. 映射数据
                var mappedData = _mappingService.MapData(data, profile);
                report.TotalRows = mappedData.Rows.Count;

                // 2. 写入数据库
                report.SuccessRows = await _dbWriter.BatchUpsertAsync(mappedData, profile, remapper);
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

        public async Task<ImportReport> ExecuteAsync(string filePath, string profileName, bool isDryRun = false)
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
                var profile = JsonConvert.DeserializeObject<ImportProfile>(json);

                // 2. 解析并映射数据
                var rawData = await _excelParser.ParseAsync(filePath);
                var mappedData = _mappingService.MapData(rawData, profile);

                report.TotalRows = mappedData.Rows.Count;

                if (!isDryRun)
                {
                    // 3. 执行数据库写入（包含事务和 Upsert 逻辑）
                    report.SuccessRows = await _dbWriter.BatchUpsertAsync(mappedData, profile);
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

            return await Task.FromResult(report);
        }

        /// <summary>
        /// 执行宽表导入(支持多表拆分)
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
                var rawData = await _excelParser.ParseAsync(filePath);
                report.TotalRows = rawData.Rows.Count;

                // 3. 初始化服务
                var remapper = new IdRemappingEngine(_db);
                var splitter = new DataSplitterService(remapper, _dbWriter, _db);

                // 4. 执行宽表拆分
                var splitTables = await splitter.SplitWideTableWithDependenciesAsync(rawData, wideProfile);

                // 5. 按依赖顺序写入
                int totalSuccess = 0;

                // 先写依赖表(已在SplitWideTableWithDependenciesAsync中写入)
                // 再写主表
                if (splitTables.ContainsKey(wideProfile.MasterTable.TargetTable))
                {
                    var count = await _dbWriter.BatchUpsertAsync(
                        splitTables[wideProfile.MasterTable.TargetTable],
                        wideProfile.MasterTable,
                        remapper
                    );
                    totalSuccess += count;
                }

                // 最后写子表
                if (wideProfile.ChildTables != null)
                {
                    foreach (var childProfile in wideProfile.ChildTables)
                    {
                        if (splitTables.ContainsKey(childProfile.TargetTable))
                        {
                            var count = await _dbWriter.BatchUpsertAsync(
                                splitTables[childProfile.TargetTable],
                                childProfile,
                                remapper
                            );
                            totalSuccess += count;
                        }
                    }
                }

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

                var rawData = await _excelParser.ParseAsync(filePath);
                report.TotalRows = rawData.Rows.Count;

                var remapper = new IdRemappingEngine(_db);
                var splitter = new DataSplitterService(remapper, _dbWriter, _db);

                // 仅处理依赖表
                foreach (var depProfile in wideProfile.DependencyTables)
                {
                    var depData = ExtractTableFromRawData(rawData, depProfile);
                    var count = await _dbWriter.BatchUpsertAsync(depData, depProfile, remapper);
                    report.SuccessRows += count;
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

                var rawData = await _excelParser.ParseAsync(filePath);
                report.TotalRows = rawData.Rows.Count;

                var remapper = new IdRemappingEngine(_db);
                
                // 预加载依赖表数据(从数据库查询已存在的基础数据)
                if (wideProfile.DependencyTables != null && wideProfile.DependencyTables.Any())
                {
                    await remapper.PreloadDependencyTablesAsync(wideProfile.DependencyTables);
                }

                var splitter = new DataSplitterService(remapper, _dbWriter, _db);
                
                // 提取主表数据并注入外键ID
                var masterData = splitter.ExtractTableDataForPublic(rawData, wideProfile.MasterTable);
                await splitter.InjectForeignKeysIntoMasterTableAsyncPublic(masterData, wideProfile.MasterTable, wideProfile.DependencyTables);

                var count = await _dbWriter.BatchUpsertAsync(masterData, wideProfile.MasterTable, remapper);
                report.SuccessRows = count;

                report.IsSuccess = true;
                report.Message = $"主表导入成功，共处理 {count} 条记录。";
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = ex.Message;
            }

            return report;
        }

        /// <summary>
        /// 从原始数据中提取指定表的数据(公共方法)
        /// </summary>
        private DataTable ExtractTableFromRawData(DataTable rawData, ImportProfile profile)
        {
            var splitter = new DataSplitterService(new IdRemappingEngine(_db), _dbWriter, _db);
            return splitter.ExtractTableDataForPublic(rawData, profile);
        }
    }
}
