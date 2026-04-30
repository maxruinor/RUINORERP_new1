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
            _mappingService = new ColumnMappingService(_db);  // ✅ 传入数据库连接以支持外键解析
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
