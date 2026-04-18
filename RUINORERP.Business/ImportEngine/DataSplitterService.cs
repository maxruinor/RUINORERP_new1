using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Models;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 数据拆分与外键注入服务
    /// </summary>
    public class DataSplitterService
    {
        private readonly IdRemappingEngine _remapper;

        public DataSplitterService(IdRemappingEngine remapper)
        {
            _remapper = remapper;
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
