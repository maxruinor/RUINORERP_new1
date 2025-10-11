using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RUINORERP.Plugin.OfficeAssistant.Shared;

namespace RUINORERP.Plugin.OfficeAssistant.Core
{
    /// <summary>
    /// 进度报告委托
    /// </summary>
    /// <param name="percentage">进度百分比</param>
    public delegate void ProgressReportHandler(int percentage);

    /// <summary>
    /// Excel对比引擎
    /// </summary>
    public class ComparisonEngine
    {
        /// <summary>
        /// 进度报告事件
        /// </summary>
        public event ProgressReportHandler ProgressReport;

        /// <summary>
        /// 执行Excel文件对比
        /// </summary>
        /// <param name="oldFile">旧文件路径</param>
        /// <param name="newFile">新文件路径</param>
        /// <param name="config">对比配置</param>
        /// <returns>对比结果</returns>
        public ComparisonResult Compare(string oldFile, string newFile, ComparisonConfig config)
        {
            try
            {
                // 报告进度20%
                OnProgressReport(20);

                // 读取文件数据
                var oldData = ExcelHelper.ReadExcelData(oldFile, config.OldWorksheetName);
                var newData = ExcelHelper.ReadExcelData(newFile, config.NewWorksheetName);
                
                // 报告进度30%
                OnProgressReport(30);

                // 构建索引
                var oldIndex = BuildIndex(oldData, config.OldKeyColumns);
                var newIndex = BuildIndex(newData, config.NewKeyColumns);
                
                // 报告进度40%
                OnProgressReport(40);

                // 执行对比
                var result = new ComparisonResult();
                
                switch (config.Mode)
                {
                    case ComparisonMode.ExistenceCheck:
                        // 存在性检查：明确标识"旧文件中有但新文件中没有"（删除）和"新文件中有但旧文件中没有"（新增）
                        result.AddedRecords = FindAddedRecords(oldIndex, newIndex, config);  // 新增：新文件中有但旧文件中没有
                        result.DeletedRecords = FindDeletedRecords(oldIndex, newIndex, config);  // 删除：旧文件中有但新文件中没有
                        result.ModifiedRecords = new List<ModifiedRecord>(); // 存在性检查不检查数据修改
                        // 查找相同键值的记录
                        result.SameRecords = FindSameRecords(oldIndex, newIndex, config);
                        break;
                        
                    case ComparisonMode.DataDifference:
                        // 数据差异：检查记录的存在性和数据差异
                        result.AddedRecords = FindAddedRecords(oldIndex, newIndex, config);
                        result.DeletedRecords = FindDeletedRecords(oldIndex, newIndex, config);
                        result.ModifiedRecords = FindModifiedRecords(oldIndex, newIndex, config);
                        result.SameRecords = new List<DiffRecord>(); // 数据差异模式下不返回相同记录
                        break;
                        
                    case ComparisonMode.CustomColumns:
                        // 自定义列对比：只对比指定的列
                        result.AddedRecords = FindAddedRecords(oldIndex, newIndex, config);
                        result.DeletedRecords = FindDeletedRecords(oldIndex, newIndex, config);
                        result.ModifiedRecords = FindModifiedRecords(oldIndex, newIndex, config);
                        result.SameRecords = new List<DiffRecord>(); // 自定义列对比模式下不返回相同记录
                        break;
                        
                    default:
                        // 默认使用数据差异模式
                        result.AddedRecords = FindAddedRecords(oldIndex, newIndex, config);
                        result.DeletedRecords = FindDeletedRecords(oldIndex, newIndex, config);
                        result.ModifiedRecords = FindModifiedRecords(oldIndex, newIndex, config);
                        result.SameRecords = new List<DiffRecord>(); // 默认模式下不返回相同记录
                        break;
                }
                
                // 报告进度90%
                OnProgressReport(90);

                // 生成摘要
                result.Summary = new ComparisonSummary
                {
                    TotalOldRecords = oldData.Rows.Count,
                    TotalNewRecords = newData.Rows.Count,
                    AddedCount = result.AddedRecords.Count,
                    DeletedCount = result.DeletedRecords.Count,
                    ModifiedCount = result.ModifiedRecords.Count
                };
                
                // 报告进度100%
                OnProgressReport(100);
                
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"对比过程中发生错误: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 触发进度报告事件
        /// </summary>
        /// <param name="percentage">进度百分比</param>
        private void OnProgressReport(int percentage)
        {
            ProgressReport?.Invoke(percentage);
        }
        
        /// <summary>
        /// 构建数据索引
        /// </summary>
        /// <param name="data">数据表</param>
        /// <param name="keyColumns">键列索引</param>
        /// <returns>索引字典</returns>
        private Dictionary<string, DataRow> BuildIndex(DataTable data, List<int> keyColumns)
        {
            var index = new Dictionary<string, DataRow>();
            int totalRows = data.Rows.Count;
            
            for (int i = 0; i < totalRows; i++)
            {
                DataRow row = data.Rows[i];
                var key = GenerateKey(row, keyColumns);
                // 注意：如果有重复的键值，后面的行会覆盖前面的行
                // 这是预期的行为，因为键值应该唯一标识一条记录
                index[key] = row;
                
                // 每处理10行报告一次进度，使进度更新更频繁
                if (i % 10 == 0 && totalRows > 0)
                {
                    // 进度范围：30-40%
                    int progress = 30 + (i * 10 / totalRows);
                    // 确保进度在合理范围内
                    progress = Math.Min(40, Math.Max(30, progress));
                    OnProgressReport(progress);
                }
            }
            
            return index;
        }
        
        /// <summary>
        /// 生成键值
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="keyColumns">键列索引</param>
        /// <returns>键值</returns>
        private string GenerateKey(DataRow row, List<int> keyColumns)
        {
            var keyParts = new List<string>();
            foreach (var columnIndex in keyColumns)
            {
                if (columnIndex < row.ItemArray.Length)
                {
                    keyParts.Add(row[columnIndex]?.ToString() ?? string.Empty);
                }
            }
            return string.Join("|", keyParts);
        }
        
        /// <summary>
        /// 查找新增记录
        /// </summary>
        /// <param name="oldIndex">旧数据索引</param>
        /// <param name="newIndex">新数据索引</param>
        /// <param name="config">对比配置</param>
        /// <returns>新增记录列表</returns>
        private List<DiffRecord> FindAddedRecords(
            Dictionary<string, DataRow> oldIndex, 
            Dictionary<string, DataRow> newIndex, 
            ComparisonConfig config)
        {
            var addedRecords = new List<DiffRecord>();
            int totalRecords = newIndex.Count;
            int processedCount = 0;
            
            foreach (var kvp in newIndex)
            {
                if (!oldIndex.ContainsKey(kvp.Key))
                {
                    var record = new DiffRecord
                    {
                        KeyValues = kvp.Key.Split('|'),
                        Data = ConvertRowToDictionary(kvp.Value)
                    };
                    addedRecords.Add(record);
                }
                
                processedCount++;
                // 每处理5条记录报告一次进度，使进度更新更频繁
                if (processedCount % 5 == 0 && totalRecords > 0)
                {
                    // 进度范围：40-50%
                    int progress = 40 + (processedCount * 10 / totalRecords);
                    // 确保进度在合理范围内
                    progress = Math.Min(50, Math.Max(40, progress));
                    OnProgressReport(progress);
                }
            }
            
            return addedRecords;
        }
        
        /// <summary>
        /// 查找删除记录
        /// </summary>
        /// <param name="oldIndex">旧数据索引</param>
        /// <param name="newIndex">新数据索引</param>
        /// <param name="config">对比配置</param>
        /// <returns>删除记录列表</returns>
        private List<DiffRecord> FindDeletedRecords(
            Dictionary<string, DataRow> oldIndex, 
            Dictionary<string, DataRow> newIndex, 
            ComparisonConfig config)
        {
            var deletedRecords = new List<DiffRecord>();
            int totalRecords = oldIndex.Count;
            int processedCount = 0;
            
            foreach (var kvp in oldIndex)
            {
                if (!newIndex.ContainsKey(kvp.Key))
                {
                    var record = new DiffRecord
                    {
                        KeyValues = kvp.Key.Split('|'),
                        Data = ConvertRowToDictionary(kvp.Value)
                    };
                    deletedRecords.Add(record);
                }
                
                processedCount++;
                // 每处理5条记录报告一次进度，使进度更新更频繁
                if (processedCount % 5 == 0 && totalRecords > 0)
                {
                    // 进度范围：50-60%
                    int progress = 50 + (processedCount * 10 / totalRecords);
                    // 确保进度在合理范围内
                    progress = Math.Min(60, Math.Max(50, progress));
                    OnProgressReport(progress);
                }
            }
            
            return deletedRecords;
        }
        
        /// <summary>
        /// 查找修改记录
        /// </summary>
        /// <param name="oldIndex">旧数据索引</param>
        /// <param name="newIndex">新数据索引</param>
        /// <param name="config">对比配置</param>
        /// <returns>修改记录列表</returns>
        private List<ModifiedRecord> FindModifiedRecords(
            Dictionary<string, DataRow> oldIndex, 
            Dictionary<string, DataRow> newIndex, 
            ComparisonConfig config)
        {
            var modifiedRecords = new List<ModifiedRecord>();
            int totalRecords = newIndex.Count;
            int processedCount = 0;
            
            foreach (var kvp in newIndex)
            {
                if (oldIndex.ContainsKey(kvp.Key))
                {
                    var oldRow = oldIndex[kvp.Key];
                    var newRow = kvp.Value;
                    
                    var differences = CompareRows(oldRow, newRow, config);
                    if (differences.Count > 0)
                    {
                        var record = new ModifiedRecord
                        {
                            KeyValues = kvp.Key.Split('|'),
                            Differences = differences
                        };
                        modifiedRecords.Add(record);
                    }
                }
                
                processedCount++;
                // 每处理5条记录报告一次进度，使进度更新更频繁
                if (processedCount % 5 == 0 && totalRecords > 0)
                {
                    // 进度范围：60-75%
                    int progress = 60 + (processedCount * 15 / totalRecords);
                    // 确保进度在合理范围内
                    progress = Math.Min(75, Math.Max(60, progress));
                    OnProgressReport(progress);
                }
            }
            
            return modifiedRecords;
        }
        
        /// <summary>
        /// 对比两行数据
        /// </summary>
        /// <param name="oldRow">旧行</param>
        /// <param name="newRow">新行</param>
        /// <param name="config">对比配置</param>
        /// <returns>差异字典</returns>
        private Dictionary<string, ValueDifference> CompareRows(
            DataRow oldRow, 
            DataRow newRow, 
            ComparisonConfig config)
        {
            var differences = new Dictionary<string, ValueDifference>();
            
            for (int i = 0; i < config.CompareColumns.Count; i++)
            {
                var oldColumnIndex = config.OldCompareColumns[i];
                var newColumnIndex = config.NewCompareColumns[i];
                
                if (oldColumnIndex < oldRow.ItemArray.Length && 
                    newColumnIndex < newRow.ItemArray.Length)
                {
                    var oldValue = oldRow[oldColumnIndex]?.ToString() ?? string.Empty;
                    var newValue = newRow[newColumnIndex]?.ToString() ?? string.Empty;
                    
                    if (!AreValuesEqual(oldValue, newValue, config))
                    {
                        differences[$"列{oldColumnIndex}"] = new ValueDifference
                        {
                            OldValue = oldValue,
                            NewValue = newValue
                        };
                    }
                }
            }
            
            return differences;
        }
        
        /// <summary>
        /// 判断两个值是否相等
        /// </summary>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        /// <param name="config">对比配置</param>
        /// <returns>是否相等</returns>
        private bool AreValuesEqual(string oldValue, string newValue, ComparisonConfig config)
        {
            if (config.IgnoreSpaces)
            {
                oldValue = oldValue.Trim();
                newValue = newValue.Trim();
            }
            
            if (config.CaseSensitive)
            {
                return oldValue.Equals(newValue);
            }
            else
            {
                return oldValue.Equals(newValue, StringComparison.OrdinalIgnoreCase);
            }
        }
        
        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="worksheetName">工作表名称</param>
        /// <returns>数据表</returns>
        private DataTable ReadExcelData(string filePath, string worksheetName)
        {
            return ExcelHelper.ReadExcelData(filePath, worksheetName);
        }
        
        /// <summary>
        /// 将DataRow转换为字典
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>字典</returns>
        private Dictionary<string, object> ConvertRowToDictionary(DataRow row)
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                // 使用列名作为键，而不是"列{i}"格式
                var columnName = row.Table.Columns[i].ColumnName;
                dict[columnName] = row[i];
            }
            return dict;
        }
        
        /// <summary>
        /// 查找相同记录（键值相同）
        /// </summary>
        /// <param name="oldIndex">旧数据索引</param>
        /// <param name="newIndex">新数据索引</param>
        /// <param name="config">对比配置</param>
        /// <returns>相同记录列表</returns>
        private List<DiffRecord> FindSameRecords(
            Dictionary<string, DataRow> oldIndex, 
            Dictionary<string, DataRow> newIndex, 
            ComparisonConfig config)
        {
            var sameRecords = new List<DiffRecord>();
            int totalRecords = newIndex.Count;
            int processedCount = 0;
            
            foreach (var kvp in newIndex)
            {
                if (oldIndex.ContainsKey(kvp.Key))
                {
                    // 键值相同，添加到相同记录列表
                    var record = new DiffRecord
                    {
                        KeyValues = kvp.Key.Split('|'),
                        Data = ConvertRowToDictionary(kvp.Value)
                    };
                    sameRecords.Add(record);
                }
                
                processedCount++;
                // 每处理5条记录报告一次进度，使进度更新更频繁
                if (processedCount % 5 == 0 && totalRecords > 0)
                {
                    // 进度范围：75-85%
                    int progress = 75 + (processedCount * 10 / totalRecords);
                    // 确保进度在合理范围内
                    progress = Math.Min(85, Math.Max(75, progress));
                    OnProgressReport(progress);
                }
            }
            
            return sameRecords;
        }
    }
}