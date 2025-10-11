using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RUINORERP.Plugin.OfficeAssistant.Core
{
    /// <summary>
    /// Excel对比引擎
    /// </summary>
    public class ComparisonEngine
    {
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
                // 读取文件数据
                var oldData = ReadExcelData(oldFile, config.OldWorksheetName);
                var newData = ReadExcelData(newFile, config.NewWorksheetName);
                
                // 构建索引
                var oldIndex = BuildIndex(oldData, config.OldKeyColumns);
                var newIndex = BuildIndex(newData, config.NewKeyColumns);
                
                // 执行对比
                var result = new ComparisonResult();
                result.AddedRecords = FindAddedRecords(oldIndex, newIndex, config);
                result.DeletedRecords = FindDeletedRecords(oldIndex, newIndex, config);
                result.ModifiedRecords = FindModifiedRecords(oldIndex, newIndex, config);
                
                // 生成摘要
                result.Summary = new ComparisonSummary
                {
                    TotalOldRecords = oldData.Rows.Count,
                    TotalNewRecords = newData.Rows.Count,
                    AddedCount = result.AddedRecords.Count,
                    DeletedCount = result.DeletedRecords.Count,
                    ModifiedCount = result.ModifiedRecords.Count
                };
                
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"对比过程中发生错误: {ex.Message}", ex);
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
            var dataTable = new DataTable();
            
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = string.IsNullOrEmpty(worksheetName) 
                    ? workbook.Worksheet(1) 
                    : workbook.Worksheet(worksheetName);
                
                // 添加列
                var firstRow = worksheet.FirstRowUsed();
                if (firstRow != null)
                {
                    foreach (var cell in firstRow.Cells())
                    {
                        dataTable.Columns.Add(cell.Value.ToString());
                    }
                    
                    // 添加数据行
                    var rows = worksheet.RowsUsed().Skip(1); // 跳过标题行
                    foreach (var row in rows)
                    {
                        var dataRow = dataTable.NewRow();
                        var cellIndex = 0;
                        foreach (var cell in row.Cells())
                        {
                            if (cellIndex < dataTable.Columns.Count)
                            {
                                dataRow[cellIndex] = cell.Value;
                                cellIndex++;
                            }
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            
            return dataTable;
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
            
            foreach (DataRow row in data.Rows)
            {
                var key = GenerateKey(row, keyColumns);
                if (!index.ContainsKey(key))
                {
                    index[key] = row;
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
        /// 将DataRow转换为字典
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>字典</returns>
        private Dictionary<string, object> ConvertRowToDictionary(DataRow row)
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                dict[$"列{i}"] = row[i];
            }
            return dict;
        }
    }
}