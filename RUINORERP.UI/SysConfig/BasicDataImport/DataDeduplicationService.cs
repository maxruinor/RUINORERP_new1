using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 数据去重服务类
    /// </summary>
    /// <remarks>
    /// 提供基于多字段组合的数据去重功能，支持灵活的去重策略
    /// </remarks>
    public class DataDeduplicationService
    {
        /// <summary>
        /// 去重结果
        /// </summary>
        public class DeduplicationResult
        {
            /// <summary>
            /// 去重后的数据表
            /// </summary>
            public DataTable DeduplicatedData { get; set; }

            /// <summary>
            /// 原始数据行数
            /// </summary>
            public int OriginalCount { get; set; }

            /// <summary>
            /// 去除的重复行数
            /// </summary>
            public int DuplicateCount { get; set; }

            /// <summary>
            /// 使用的去重字段列表
            /// </summary>
            public List<string> DeduplicateFields { get; set; }
        }

        /// <summary>
        /// 根据配置对数据进行去重处理1
        /// </summary>
        /// <param name="dataTable">原始数据表</param>
        /// <param name="config">导入配置</param>
        /// <returns>去重结果</returns>
        /// <exception cref="ArgumentNullException">参数为空时抛出异常</exception>
        /// <exception cref="InvalidOperationException">去重字段不存在时抛出异常</exception>
        public DeduplicationResult Deduplicate(DataTable dataTable, ImportConfiguration config)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var result = new DeduplicationResult
            {
                OriginalCount = dataTable.Rows.Count,
                DeduplicateFields = new List<string>()
            };

            // 如果未启用去重或没有数据，直接返回
            if (!config.EnableDeduplication || dataTable.Rows.Count == 0)
            {
                result.DeduplicatedData = dataTable.Copy();
                result.DuplicateCount = 0;
                return result;
            }

            // 获取有效的去重字段
            var deduplicateFields = config.GetEffectiveDeduplicateFields();

            if (deduplicateFields == null || deduplicateFields.Count == 0)
            {
                // 未配置去重字段，不执行去重
                result.DeduplicatedData = dataTable.Copy();
                result.DuplicateCount = 0;
                return result;
            }

            // 将数据库字段名转换为数据表显示名称
            var displayFieldNames = ConvertToDisplayNames(deduplicateFields, config);

            // 验证去重字段是否存在
            foreach (var field in displayFieldNames)
            {
                if (!dataTable.Columns.Contains(field))
                {
                    throw new InvalidOperationException($"去重字段 '{field}' 在数据表中不存在");
                }
            }

            result.DeduplicateFields = displayFieldNames;

            // 根据去重策略执行去重
            if (config.DeduplicateStrategy == DeduplicateStrategy.LastOccurrence)
            {
                result.DeduplicatedData = DeduplicateKeepLast(dataTable, displayFieldNames, config.IgnoreEmptyValuesInDeduplication);
            }
            else
            {
                // 默认保留第一条
                result.DeduplicatedData = DeduplicateKeepFirst(dataTable, displayFieldNames, config.IgnoreEmptyValuesInDeduplication);
            }

            result.DuplicateCount = result.OriginalCount - result.DeduplicatedData.Rows.Count;

            return result;
        }

        /// <summary>
        /// 将数据库字段名转换为数据表显示名称
        /// </summary>
        /// <param name="fieldNames">数据库字段名列表</param>
        /// <param name="config">导入配置</param>
        /// <returns>数据表显示名称列表</returns>
        /// <exception cref="InvalidOperationException">字段映射不存在时抛出异常</exception>
        private List<string> ConvertToDisplayNames(List<string> fieldNames, ImportConfiguration config)
        {
            var displayNames = new List<string>();

            foreach (var fieldName in fieldNames)
            {
                // 从ColumnMapping中查找对应的显示名称
                var mapping = config.GetMappingBySystemField(fieldName);
                if (mapping != null && !string.IsNullOrEmpty(mapping.SystemField?.Value))
                {
                    displayNames.Add(mapping.SystemField.Value);
                }
                else
                {
                    // 如果找不到映射，尝试直接使用字段名（可能数据表就是用的字段名）
                    displayNames.Add(fieldName);
                }
            }

            return displayNames;
        }

        /// <summary>
        /// 保留第一条记录的去重方法
        /// </summary>
        /// <param name="dataTable">原始数据表</param>
        /// <param name="deduplicateFields">去重字段列表</param>
        /// <param name="ignoreEmptyValues">是否忽略空值</param>
        /// <returns>去重后的数据表</returns>
        private DataTable DeduplicateKeepFirst(DataTable dataTable, List<string> deduplicateFields, bool ignoreEmptyValues)
        {
            var deduplicatedTable = dataTable.Clone();
            var seenKeys = new HashSet<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                var key = GenerateDeduplicateKey(row, deduplicateFields, ignoreEmptyValues);

                if (string.IsNullOrEmpty(key))
                {
                    // 空键，跳过该行（如果配置了忽略空值）
                    if (ignoreEmptyValues)
                    {
                        continue;
                    }
                    key = "[EMPTY]";
                }

                if (!seenKeys.Contains(key))
                {
                    seenKeys.Add(key);
                    deduplicatedTable.ImportRow(row);
                }
            }

            return deduplicatedTable;
        }

        /// <summary>
        /// 保留最后一条记录的去重方法
        /// </summary>
        /// <param name="dataTable">原始数据表</param>
        /// <param name="deduplicateFields">去重字段列表</param>
        /// <param name="ignoreEmptyValues">是否忽略空值</param>
        /// <returns>去重后的数据表</returns>
        private DataTable DeduplicateKeepLast(DataTable dataTable, List<string> deduplicateFields, bool ignoreEmptyValues)
        {
            var seenKeys = new Dictionary<string, DataRow>();

            foreach (DataRow row in dataTable.Rows)
            {
                var key = GenerateDeduplicateKey(row, deduplicateFields, ignoreEmptyValues);

                if (string.IsNullOrEmpty(key))
                {
                    // 空键，跳过该行（如果配置了忽略空值）
                    if (ignoreEmptyValues)
                    {
                        continue;
                    }
                    key = "[EMPTY]";
                }

                // 总是保存最新的一条记录
                seenKeys[key] = row;
            }

            // 复制表结构
            var deduplicatedTable = dataTable.Clone();

            // 按原始顺序添加记录
            var lastRows = new HashSet<DataRow>(seenKeys.Values);
            foreach (DataRow row in dataTable.Rows)
            {
                if (lastRows.Contains(row))
                {
                    deduplicatedTable.ImportRow(row);
                }
            }

            return deduplicatedTable;
        }

        /// <summary>
        /// 生成去重键
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="deduplicateFields">去重字段列表</param>
        /// <param name="ignoreEmptyValues">是否忽略空值</param>
        /// <returns>去重键字符串</returns>
        private string GenerateDeduplicateKey(DataRow row, List<string> deduplicateFields, bool ignoreEmptyValues)
        {
            var keyParts = new List<string>();

            foreach (var field in deduplicateFields)
            {
                var value = row[field];
                var valueStr = value?.ToString() ?? string.Empty;

                if (ignoreEmptyValues && string.IsNullOrWhiteSpace(valueStr))
                {
                    return string.Empty; // 返回空键表示应该跳过该行
                }

                keyParts.Add(valueStr);
            }

            return string.Join("|", keyParts);
        }

        /// <summary>
        /// 检查数据是否存在重复
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="deduplicateFields">去重字段列表</param>
        /// <param name="ignoreEmptyValues">是否忽略空值</param>
        /// <returns>重复记录的行索引列表（每个重复组的所有行索引）</returns>
        public List<List<int>> FindDuplicates(DataTable dataTable, List<string> deduplicateFields, bool ignoreEmptyValues = true)
        {
            var duplicateGroups = new List<List<int>>();
            var keyToRows = new Dictionary<string, List<int>>();

            // 按键分组所有行索引
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                var key = GenerateDeduplicateKey(row, deduplicateFields, ignoreEmptyValues);

                if (string.IsNullOrEmpty(key))
                {
                    continue; // 跳过空键
                }

                if (!keyToRows.ContainsKey(key))
                {
                    keyToRows[key] = new List<int>();
                }

                keyToRows[key].Add(i);
            }

            // 收集有重复的组（行数大于1的组）
            foreach (var kvp in keyToRows)
            {
                if (kvp.Value.Count > 1)
                {
                    duplicateGroups.Add(kvp.Value);
                }
            }

            return duplicateGroups;
        }
    }
}
