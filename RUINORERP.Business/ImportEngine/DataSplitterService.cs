using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Models;
using SqlSugar;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 数据拆分与外键注入服务
    /// </summary>
    public class DataSplitterService
    {
        private readonly IdRemappingEngine _remapper;
        private readonly DatabaseWriterService _dbWriter;
        private readonly ISqlSugarClient _db;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="remapper">ID重映射引擎（必需）</param>
        /// <param name="dbWriter">数据库写入服务（必需）</param>
        /// <param name="db">SqlSugar数据库客户端（必需）</param>
        /// <exception cref="ArgumentNullException">当必需参数为null时抛出</exception>
        public DataSplitterService(IdRemappingEngine remapper, DatabaseWriterService dbWriter, ISqlSugarClient db)
        {
            _remapper = remapper ?? throw new ArgumentNullException(nameof(remapper), "ID重映射引擎不能为空");
            _dbWriter = dbWriter ?? throw new ArgumentNullException(nameof(dbWriter), "数据库写入服务不能为空");
            _db = db ?? throw new ArgumentNullException(nameof(db), "数据库客户端不能为空");
        }

        /// <summary>
        /// 将包含多个业务逻辑的宽表拆分为多个子表数据
        /// </summary>
        public Dictionary<string, DataTable> SplitWideTable(DataTable sourceData, List<ImportProfile> allProfiles)
        {
            var result = new Dictionary<string, DataTable>();

            foreach (var profile in allProfiles)
            {
                // 1. 筛选出该表需要的列
                var targetDt = new DataTable();
                foreach (var mapping in profile.ColumnMappings)
                {
                    if (sourceData.Columns.Contains(mapping.ExcelHeader))
                    {
                        targetDt.Columns.Add(mapping.DbColumn);
                    }
                }

                // 2. 提取数据并去重（针对主表）
                var seenKeys = new HashSet<string>();
                foreach (DataRow sourceRow in sourceData.Rows)
                {
                    var key = string.Join("|", profile.BusinessKeys.Select(k => sourceRow[k]?.ToString()));
                    if (!seenKeys.Contains(key))
                    {
                        seenKeys.Add(key);
                        var newRow = targetDt.NewRow();
                        foreach (var mapping in profile.ColumnMappings)
                        {
                            if (sourceData.Columns.Contains(mapping.ExcelHeader))
                            {
                                newRow[mapping.DbColumn] = sourceRow[mapping.ExcelHeader];
                            }
                        }
                        targetDt.Rows.Add(newRow);
                    }
                }

                result[profile.TargetTable] = targetDt;
            }

            return result;
        }

        /// <summary>
        /// 宽表拆分主入口(支持依赖表和子表)
        /// </summary>
        /// <param name="sourceData">源数据表</param>
        /// <param name="wideProfile">宽表导入配置</param>
        /// <returns>拆分后的数据表字典</returns>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        /// <exception cref="InvalidOperationException">当服务未正确初始化时抛出</exception>
        public async Task<Dictionary<string, DataTable>> SplitWideTableWithDependenciesAsync(
            DataTable sourceData,
            WideTableImportProfile wideProfile)
        {
            // 前置条件检查：验证参数
            if (sourceData == null)
            {
                throw new ArgumentNullException(nameof(sourceData), "源数据表不能为空");
            }

            if (wideProfile == null)
            {
                throw new ArgumentNullException(nameof(wideProfile), "宽表导入配置不能为空");
            }

            // 前置条件检查：验证服务是否已初始化
            if (_remapper == null)
            {
                throw new InvalidOperationException("ID重映射引擎未初始化");
            }

            if (_dbWriter == null)
            {
                throw new InvalidOperationException("数据库写入服务未初始化");
            }

            if (_db == null)
            {
                throw new InvalidOperationException("数据库客户端未初始化");
            }

            var result = new Dictionary<string, DataTable>();

            // 阶段1: 预加载所有依赖表
            if (wideProfile.DependencyTables != null && wideProfile.DependencyTables.Any())
            {
                await _remapper.PreloadDependencyTablesAsync(wideProfile.DependencyTables);
            }

            // 阶段2: 处理依赖表(字典表)
            if (wideProfile.DependencyTables != null)
            {
                foreach (var depProfile in wideProfile.DependencyTables)
                {
                    var depData = ExtractTableData(sourceData, depProfile);
                    
                    // 写入依赖表并注册ID映射
                    if (depData.Rows.Count > 0)
                    {
                        await _dbWriter.BatchUpsertAsync(depData, depProfile, _remapper);
                    }
                    
                    result[depProfile.TargetTable] = depData;
                }
            }

            // 阶段3: 处理主表(注入外键ID)
            if (wideProfile.MasterTable != null)
            {
                var masterData = ExtractTableData(sourceData, wideProfile.MasterTable);
                await InjectForeignKeysIntoMasterTableAsync(masterData, wideProfile.MasterTable, wideProfile.DependencyTables);
                result[wideProfile.MasterTable.TargetTable] = masterData;
            }

            // 阶段4: 处理子表(如果需要)
            if (wideProfile.ChildTables != null)
            {
                foreach (var childProfile in wideProfile.ChildTables)
                {
                    var childData = ExtractChildTableData(sourceData, childProfile, wideProfile.MasterTable);
                    InjectParentForeignKey(childData, childProfile, wideProfile.MasterTable);
                    result[childProfile.TargetTable] = childData;
                }
            }

            return result;
        }

        /// <summary>
        /// 从宽表中提取指定表的数据并去重
        /// </summary>
        private DataTable ExtractTableData(DataTable sourceData, ImportProfile profile)
        {
            var targetDt = BuildEmptyDataTable(profile);
            var seenKeys = new HashSet<string>();

            foreach (DataRow sourceRow in sourceData.Rows)
            {
                var businessKey = string.Join("|", profile.BusinessKeys.Select(k => sourceRow[k]?.ToString()));

                if (!seenKeys.Contains(businessKey))
                {
                    seenKeys.Add(businessKey);
                    var newRow = targetDt.NewRow();

                    foreach (var mapping in profile.ColumnMappings)
                    {
                        if (sourceData.Columns.Contains(mapping.ExcelHeader))
                        {
                            newRow[mapping.DbColumn] = sourceRow[mapping.ExcelHeader];
                        }
                    }

                    targetDt.Rows.Add(newRow);
                }
            }

            return targetDt;
        }

        /// <summary>
        /// 从原始数据中提取指定表的数据(公共方法,供SmartImportEngine调用)
        /// </summary>
        public DataTable ExtractTableDataForPublic(DataTable sourceData, ImportProfile profile)
        {
            return ExtractTableData(sourceData, profile);
        }

        /// <summary>
        /// 构建空的数据表结构
        /// </summary>
        private DataTable BuildEmptyDataTable(ImportProfile profile)
        {
            var dt = new DataTable();
            
            foreach (var mapping in profile.ColumnMappings)
            {
                Type colType = GetDbType(mapping.DataType);
                dt.Columns.Add(mapping.DbColumn, colType);
            }

            return dt;
        }

        /// <summary>
        /// 获取数据库字段类型
        /// </summary>
        private Type GetDbType(string dataType)
        {
            return dataType?.ToLower() switch
            {
                "int" or "long" => typeof(long),
                "decimal" or "double" => typeof(decimal),
                "datetime" => typeof(DateTime),
                "bool" => typeof(bool),
                _ => typeof(string)
            };
        }

        /// <summary>
        /// 向主表注入外键ID(核心逻辑)
        /// </summary>
        private async Task InjectForeignKeysIntoMasterTableAsync(
            DataTable masterData,
            ImportProfile masterProfile,
            List<ImportProfile> dependencyTables)
        {
            foreach (DataRow row in masterData.Rows)
            {
                foreach (var mapping in masterProfile.ColumnMappings)
                {
                    if (mapping.TransformRule?.StartsWith("REF:") == true &&
                        mapping.ForeignConfig != null)
                    {
                        var fkConfig = mapping.ForeignConfig;
                        var businessKeyValue = row[mapping.ExcelHeader]?.ToString();

                        if (!string.IsNullOrEmpty(businessKeyValue))
                        {
                            try
                            {
                                // 查找对应的依赖表配置
                                var depProfile = dependencyTables?.FirstOrDefault(d => d.TargetTable == fkConfig.ForeignKeyTable.Key);

                                // 调用混合来源解析
                                var fkId = await _remapper.GetOrCreateForeignKeyIdAsync(
                                    fkConfig.ForeignKeyTable.Key,
                                    fkConfig.ForeignKeyField.Key,
                                    businessKeyValue,
                                    row,
                                    depProfile
                                );

                                // 将业务名称替换为物理ID
                                row[mapping.DbColumn] = fkId;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"处理外键字段 '{mapping.ExcelHeader}' 时出错: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 向主表注入外键ID(公共方法,供SmartImportEngine调用)
        /// </summary>
        public async Task InjectForeignKeysIntoMasterTableAsyncPublic(
            DataTable masterData,
            ImportProfile masterProfile,
            List<ImportProfile> dependencyTables)
        {
            await InjectForeignKeysIntoMasterTableAsync(masterData, masterProfile, dependencyTables);
        }

        /// <summary>
        /// 提取子表数据
        /// </summary>
        private DataTable ExtractChildTableData(DataTable sourceData, ChildTableConfig childProfile, ImportProfile masterProfile)
        {
            var targetDt = BuildEmptyDataTable(childProfile);

            foreach (DataRow sourceRow in sourceData.Rows)
            {
                var newRow = targetDt.NewRow();

                foreach (var mapping in childProfile.ColumnMappings)
                {
                    if (sourceData.Columns.Contains(mapping.ExcelHeader))
                    {
                        newRow[mapping.DbColumn] = sourceRow[mapping.ExcelHeader];
                    }
                }

                targetDt.Rows.Add(newRow);
            }

            return targetDt;
        }

        /// <summary>
        /// 向子表注入父表外键
        /// </summary>
        private void InjectParentForeignKey(DataTable childData, ChildTableConfig childProfile, ImportProfile masterProfile)
        {
            foreach (DataRow row in childData.Rows)
            {
                // 查找引用父表的字段
                foreach (var mapping in childProfile.ColumnMappings)
                {
                    if (mapping.TransformRule?.StartsWith("REF:") == true)
                    {
                        // 解析 REF:tb_Prod:ProdCode 格式
                        var parts = mapping.TransformRule.Substring(4).Split(':');
                        if (parts.Length >= 2)
                        {
                            string parentTableName = parts[0];
                            string parentBusinessKeyField = parts[1];

                            // 从当前行获取父表业务键值
                            var parentBusinessKeyValue = row[mapping.ExcelHeader]?.ToString();

                            if (!string.IsNullOrEmpty(parentBusinessKeyValue))
                            {
                                // 从Remapper获取父表ID
                                var parentId = _remapper.GetNewId(parentTableName, parentBusinessKeyValue);
                                if (parentId != null)
                                {
                                    row[mapping.DbColumn] = parentId;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在子表数据中注入父表生成的新ID
        /// </summary>
        public void InjectForeignKeys(DataTable childData, ImportProfile childProfile, string parentTableName)
        {
            foreach (DataRow row in childData.Rows)
            {
                // 查找配置中REF:规则的列
                var refMappings = childProfile.ColumnMappings.Where(m => m.TransformRule?.StartsWith("REF:") == true);
                
                foreach (var mapping in refMappings)
                {
                    if (!childData.Columns.Contains(mapping.ExcelHeader)) continue;

                    var oldKeyValue = row[mapping.ExcelHeader];
                    if (oldKeyValue != null && !string.IsNullOrEmpty(oldKeyValue.ToString()))
                    {
                        // 从Remapper获取新生成的ID
                        var newId = _remapper.GetNewId(parentTableName, oldKeyValue);
                        if (newId != null)
                        {
                            // 将Excel中的业务编码替换为数据库主键ID
                            row[mapping.DbColumn] = newId;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 用于DataTable行去重的比较器
    /// </summary>
    public class DataRowComparer : IEqualityComparer<DataRow>
    {
        private readonly List<string> _keys;

        public DataRowComparer(List<string> keys)
        {
            _keys = keys;
        }

        public bool Equals(DataRow x, DataRow y)
        {
            return _keys.All(k => x[k]?.ToString() == y[k]?.ToString());
        }

        public int GetHashCode(DataRow obj)
        {
            return string.Join("|", _keys.Select(k => obj[k]?.ToString())).GetHashCode();
        }
    }
}
