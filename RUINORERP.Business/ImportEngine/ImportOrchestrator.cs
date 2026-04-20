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
    /// 导入编排器：处理多文件、依赖排序和跨表关联
    /// </summary>
    public class ImportOrchestrator
    {
        private readonly SmartImportEngine _engine;
        private readonly IdRemappingEngine _remapper;
        private readonly DataSplitterService _splitter;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">SqlSugar数据库客户端（必需）</param>
        /// <exception cref="ArgumentNullException">当db参数为null时抛出</exception>
        public ImportOrchestrator(ISqlSugarClient db)
        {
            if (db == null)
            {
                throw new ArgumentNullException(nameof(db), "数据库客户端不能为空");
            }

            _engine = new SmartImportEngine(db);
            _remapper = new IdRemappingEngine(db);
            
            // 创建DatabaseWriterService实例供DataSplitterService使用
            var dbWriter = new DatabaseWriterService(db);
            _splitter = new DataSplitterService(_remapper, dbWriter, db);
        }

        /// <summary>
        /// 执行复杂的组合导入任务
        /// </summary>
        /// <param name="mainExcelPath">主Excel路径</param>
        /// <param name="profileNames">需要执行的方案列表（会自动排序）</param>
        public async Task<ImportReport> ExecuteComplexImportAsync(string mainExcelPath, List<string> profileNames)
        {
            var report = new ImportReport { IsSuccess = true };
            
            try
            {
                // 1. 加载所有配置并排序
                var allProfiles = new List<ImportProfile>();
                foreach (var name in profileNames)
                {
                    var profile = LoadProfile(name);
                    if (profile != null) allProfiles.Add(profile);
                }

                var sortedProfiles = DependencyResolver.ResolveOrder(allProfiles);

                // 2. 预解析主Excel文件（如果是单文件多表场景）
                DataTable masterData = null;
                if (sortedProfiles.Any(p => string.IsNullOrEmpty(p.SourceExcelFile)))
                {
                    masterData = await _engine.ParseExcelAsync(mainExcelPath);
                }

                // 3. 按顺序执行导入
                foreach (var profile in sortedProfiles)
                {
                    DataTable currentData;

                    // 判断数据来源：是独立文件还是从主文件拆分
                    if (!string.IsNullOrEmpty(profile.SourceExcelFile))
                    {
                        var fullPath = Path.Combine(Path.GetDirectoryName(mainExcelPath), profile.SourceExcelFile);
                        currentData = await _engine.ParseExcelAsync(fullPath);
                    }
                    else
                    {
                        // 从主宽表中拆分出当前表所需的数据
                        currentData = _splitter.SplitWideTable(masterData, new List<ImportProfile> { profile })[profile.TargetTable];
                    }

                    // 4. 注入外键ID（如果该表依赖其他表）
                    if (profile.Dependencies.Any())
                    {
                        foreach (var depTable in profile.Dependencies)
                        {
                            _splitter.InjectForeignKeys(currentData, profile, depTable);
                        }
                    }

                    // 5. 执行入库并记录ID映射
                    var subReport = await _engine.ExecuteWithDataTableAsync(currentData, profile, _remapper);
                }
            }
            catch (Exception ex)
            {
                report.IsSuccess = false;
                report.Message = $"编排执行失败: {ex.Message}";
            }

            return report;
        }

        private ImportProfile LoadProfile(string name)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles", $"{name}.json");
            if (!File.Exists(path)) return null;
            return JsonConvert.DeserializeObject<ImportProfile>(File.ReadAllText(path));
        }
    }
}
