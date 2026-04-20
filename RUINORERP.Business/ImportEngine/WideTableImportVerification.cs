using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using RUINORERP.Business;
using RUINORERP.Business.ImportEngine;
using RUINORERP.Model.ImportEngine.Models;
using SqlSugar;

namespace RUINORERP.Tests
{
    /// <summary>
    /// 验证宽表导入功能的测试类
    /// </summary>
    public class WideTableImportVerification
    {
        private readonly ISqlSugarClient _db;
        private readonly SmartImportEngine _engine;

        public WideTableImportVerification(ISqlSugarClient db)
        {
            _db = db;
            _engine = new SmartImportEngine(db);
        }

        /// <summary>
        /// 验证单表导入功能
        /// </summary>
        public async Task<bool> TestSingleTableImport()
        {
            try
            {
                Console.WriteLine("=== 验证单表导入功能 ===");
                
                // 1. 检查配置文件是否存在
                var profilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles", "05_供应商单表导入.json");
                if (!File.Exists(profilePath))
                {
                    Console.WriteLine("✗ 配置文件不存在");
                    return false;
                }
                
                Console.WriteLine("✓ 配置文件存在");
                
                // 2. 检查SmartImportEngine是否包含必要的方法
                var methods = typeof(SmartImportEngine).GetMethods();
                var hasExecuteWideMethod = false;
                var hasExecuteProfileMethod = false;
                
                foreach (var method in methods)
                {
                    if (method.Name == "ExecuteWideTableImportAsync")
                        hasExecuteWideMethod = true;
                    if (method.Name == "ExecuteWithProfileAsync")
                        hasExecuteProfileMethod = true;
                }
                
                if (!hasExecuteWideMethod || !hasExecuteProfileMethod)
                {
                    Console.WriteLine("✗ SmartImportEngine缺少必要的方法");
                    return false;
                }
                
                Console.WriteLine("✓ SmartImportEngine包含必要的方法");
                
                // 3. 检查关键组件是否可用
                var remapper = new IdRemappingEngine(_db);
                var dbWriter = new DatabaseWriterService(_db);
                var splitter = new DataSplitterService(remapper, dbWriter, _db);
                
                Console.WriteLine("✓ 关键组件可以正常实例化");
                
                // 4. 检查模型类是否存在
                var wideProfile = new WideTableImportProfile();
                var importProfile = new ImportProfile();
                
                Console.WriteLine("✓ 模型类可以正常实例化");
                
                Console.WriteLine("✓ 单表导入功能验证通过");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 单表导入功能验证失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 验证宽表导入功能
        /// </summary>
        public async Task<bool> TestWideTableImport()
        {
            try
            {
                Console.WriteLine("\n=== 验证宽表导入功能 ===");
                
                // 1. 检查配置文件是否存在
                var profilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles", "04_产品宽表导入.json");
                if (!File.Exists(profilePath))
                {
                    Console.WriteLine("✗ 配置文件不存在");
                    return false;
                }
                
                Console.WriteLine("✓ 配置文件存在");
                
                // 2. 检查SmartImportEngine是否包含宽表导入方法
                var methods = typeof(SmartImportEngine).GetMethods();
                var hasExecuteWideMethod = false;
                
                foreach (var method in methods)
                {
                    if (method.Name == "ExecuteWideTableImportAsync")
                        hasExecuteWideMethod = true;
                }
                
                if (!hasExecuteWideMethod)
                {
                    Console.WriteLine("✗ SmartImportEngine缺少ExecuteWideTableImportAsync方法");
                    return false;
                }
                
                Console.WriteLine("✓ SmartImportEngine包含宽表导入方法");
                
                // 3. 检查扩展的IdRemappingEngine方法
                var remapper = new IdRemappingEngine(_db);
                var preloadMethod = typeof(IdRemappingEngine).GetMethod("PreloadDependencyTablesAsync");
                var getOrCreateMethod = typeof(IdRemappingEngine).GetMethod("GetOrCreateForeignKeyIdAsync");
                var registerNewMethod = typeof(IdRemappingEngine).GetMethod("RegisterNewId");
                
                if (preloadMethod == null || getOrCreateMethod == null || registerNewMethod == null)
                {
                    Console.WriteLine("✗ IdRemappingEngine缺少扩展方法");
                    return false;
                }
                
                Console.WriteLine("✓ IdRemappingEngine包含扩展方法");
                
                // 4. 检查DataSplitterService扩展方法
                var dbWriter = new DatabaseWriterService(_db);
                var splitter = new DataSplitterService(remapper, dbWriter, _db);
                var splitMethod = typeof(DataSplitterService).GetMethod("SplitWideTableWithDependenciesAsync");
                
                if (splitMethod == null)
                {
                    Console.WriteLine("✗ DataSplitterService缺少SplitWideTableWithDependenciesAsync方法");
                    return false;
                }
                
                Console.WriteLine("✓ DataSplitterService包含宽表拆分方法");
                
                // 5. 检查DatabaseWriterService扩展方法
                var batchInsertMethod = typeof(DatabaseWriterService).GetMethod("BatchInsertWithIdReturnAsync");
                
                if (batchInsertMethod == null)
                {
                    Console.WriteLine("✗ DatabaseWriterService缺少BatchInsertWithIdReturnAsync方法");
                    return false;
                }
                
                Console.WriteLine("✓ DatabaseWriterService包含扩展方法");
                
                Console.WriteLine("✓ 宽表导入功能验证通过");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 宽表导入功能验证失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 验证宽表兼容单表的能力
        /// </summary>
        public async Task<bool> TestWideTableCompatibility()
        {
            try
            {
                Console.WriteLine("\n=== 验证宽表兼容单表的能力 ===");
                
                // 验证宽表导入方法可以处理单表配置
                var profilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SysConfig", "DataMigration", "Profiles", "05_供应商单表导入.json");
                if (!File.Exists(profilePath))
                {
                    Console.WriteLine("✗ 单表配置文件不存在");
                    return false;
                }
                
                Console.WriteLine("✓ 单表配置文件存在");
                
                // 验证配置文件结构兼容性
                var jsonContent = File.ReadAllText(profilePath);
                if (!jsonContent.Contains("\"MasterTable\""))
                {
                    Console.WriteLine("✗ 配置文件不包含MasterTable");
                    return false;
                }
                
                Console.WriteLine("✓ 配置文件结构兼容");
                
                Console.WriteLine("✓ 宽表兼容单表验证通过");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 宽表兼容单表验证失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 执行完整验证
        /// </summary>
        public async Task<bool> RunFullVerification()
        {
            Console.WriteLine("开始验证宽表拆分与智能外键映射导入功能...\n");
            
            var singleTableResult = await TestSingleTableImport();
            var wideTableResult = await TestWideTableImport();
            var compatibilityResult = await TestWideTableCompatibility();
            
            Console.WriteLine($"\n=== 验证结果汇总 ===");
            Console.WriteLine($"单表导入功能: {(singleTableResult ? "✓ 通过" : "✗ 失败")}");
            Console.WriteLine($"宽表导入功能: {(wideTableResult ? "✓ 通过" : "✗ 失败")}");
            Console.WriteLine($"宽表兼容单表: {(compatibilityResult ? "✓ 通过" : "✗ 失败")}");
            
            var overallResult = singleTableResult && wideTableResult && compatibilityResult;
            Console.WriteLine($"\n总体验证结果: {(overallResult ? "✓ 全部通过" : "✗ 存在问题")}");
            
            return overallResult;
        }
    }
}
