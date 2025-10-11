using ClosedXML.Excel;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Plugin.OfficeAssistant.Shared
{
    /// <summary>
    /// Excel处理助手类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 读取Excel文件数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="worksheetName">工作表名称（可选）</param>
        /// <returns>数据表</returns>
        public static DataTable ReadExcelData(string filePath, string worksheetName = null)
        {
            return ReadExcelData(filePath, -1, worksheetName); // -1表示加载所有行
        }
        
        /// <summary>
        /// 读取Excel文件数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="maxRows">最大读取行数，-1表示无限制</param>
        /// <param name="worksheetName">工作表名称（可选）</param>
        /// <returns>数据表</returns>
        public static DataTable ReadExcelData(string filePath, int maxRows, string worksheetName = null)
        {
            var dataTable = new DataTable();
            
            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    IXLWorksheet worksheet = null;
                    
                    // 获取工作表
                    if (string.IsNullOrEmpty(worksheetName))
                    {
                        worksheet = workbook.Worksheet(1);
                    }
                    else
                    {
                        worksheet = workbook.Worksheet(worksheetName);
                    }
                    
                    if (worksheet == null)
                    {
                        throw new InvalidOperationException("无法找到指定的工作表");
                    }
                    
                    // 读取标题行
                    var firstRow = worksheet.FirstRowUsed();
                    if (firstRow == null)
                    {
                        throw new InvalidOperationException("工作表中没有数据");
                    }
                    
                    // 检查并处理列
                    var columnHeaders = new List<string>();
                    var columnCells = firstRow.Cells().ToList();
                    
                    // 获取工作表第一行的所有列（包括空列）
                    var firstRowColumns = worksheet.Row(firstRow.RowNumber());
                    int totalColumnsInRow = firstRowColumns.LastCellUsed()?.Address.ColumnNumber ?? 0;
                    
                    // 检查是否有跳过的空列
                    var actualCells = new List<IXLCell>(columnCells);
                    if (totalColumnsInRow > actualCells.Count)
                    {
                        // 存在空列，需要警告用户
                        var emptyColumnsList = new List<int>();
                        var usedColumns = new HashSet<int>(actualCells.Select(c => c.Address.ColumnNumber));
                        
                        for (int i = 1; i <= totalColumnsInRow; i++)
                        {
                            if (!usedColumns.Contains(i))
                            {
                                emptyColumnsList.Add(i);
                            }
                        }
                        
                        if (emptyColumnsList.Count > 0)
                        {
                            var emptyColumnsStr = string.Join(", ", emptyColumnsList);
                            throw new InvalidOperationException($"Excel文件中存在空白列标题，位于列: {emptyColumnsStr}。请填写所有列的标题以避免数据读取错误。");
                        }
                    }
                    
                    // 先收集所有列名
                    foreach (var cell in columnCells)
                    {
                        var columnName = cell.Value.ToString();
                        columnHeaders.Add(string.IsNullOrEmpty(columnName) ? "" : columnName);
                    }
                    
                    // 检查是否有空的列标题
                    var emptyHeaderIndices = new List<int>();
                    for (int i = 0; i < columnHeaders.Count; i++)
                    {
                        if (string.IsNullOrEmpty(columnHeaders[i]))
                        {
                            emptyHeaderIndices.Add(i);
                        }
                    }
                    
                    // 如果有空标题，给出警告
                    if (emptyHeaderIndices.Count > 0)
                    {
                        // 不直接抛出异常，而是给列分配默认名称
                        for (int i = 0; i < columnHeaders.Count; i++)
                        {
                            if (string.IsNullOrEmpty(columnHeaders[i]))
                            {
                                columnHeaders[i] = $"未命名列{i + 1}";
                            }
                        }
                    }
                    
                    // 读取数据行
                    var rows = worksheet.RowsUsed().Skip(1); // 跳过标题行
                    
                    // 检查每列是否有数据，过滤掉完全为空的列（只检查前几行以提高性能）
                    var emptyColumns = CheckEmptyColumns(rows, columnHeaders, maxRows);
                    
                    // 添加列到DataTable（跳过空列）
                    for (int i = 0; i < columnHeaders.Count; i++)
                    {
                        // 跳过空列
                        if (emptyColumns.Contains(i))
                            continue;
                            
                        var columnName = columnHeaders[i];
                        // 确保列名唯一
                        if (dataTable.Columns.Contains(columnName))
                        {
                            columnName = GetUniqueColumnName(dataTable, columnName);
                        }
                        dataTable.Columns.Add(columnName);
                    }
                    
                    // 添加有数据的行到DataTable
                    int rowCount = 0;
                    foreach (var row in rows)
                    {
                        // 如果设置了最大行数且已达到限制，则停止读取
                        if (maxRows > 0 && rowCount >= maxRows)
                            break;
                            
                        var dataRow = dataTable.NewRow();
                        var cells = row.Cells().ToList();
                        
                        // 重新计算非空列的索引
                        int actualColIndex = 0;
                        for (int colIndex = 0; colIndex < columnHeaders.Count; colIndex++)
                        {
                            // 跳过空列
                            if (emptyColumns.Contains(colIndex))
                                continue;
                            
                            // 正确地从cells中获取数据
                            // 需要找到对应列号的单元格
                            var cell = cells.FirstOrDefault(c => c.Address.ColumnNumber == (colIndex + 1));
                            if (cell != null)
                            {
                                dataRow[actualColIndex] = cell.Value;
                            }
                            else
                            {
                                // 如果单元格不存在或为空，设置为空值
                                dataRow[actualColIndex] = DBNull.Value;
                            }
                            actualColIndex++;
                        }
                        dataTable.Rows.Add(dataRow);
                        rowCount++;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取Excel文件时发生错误: {ex.Message}", ex);
            }
            
            return dataTable;
        }
        
        /// <summary>
        /// 异步读取Excel文件数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="worksheetName">工作表名称（可选）</param>
        /// <returns>数据表</returns>
        public static Task<DataTable> ReadExcelDataAsync(string filePath, string worksheetName = null)
        {
            return ReadExcelDataAsync(filePath, -1, worksheetName);
        }
        
        /// <summary>
        /// 异步读取Excel文件数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="maxRows">最大读取行数，-1表示无限制</param>
        /// <param name="worksheetName">工作表名称（可选）</param>
        /// <returns>数据表</returns>
        public static async Task<DataTable> ReadExcelDataAsync(string filePath, int maxRows, string worksheetName = null)
        {
            // 在线程池中执行耗时操作
            return await Task.Run(() => ReadExcelData(filePath, maxRows, worksheetName));
        }

        /// <summary>
        /// 检查空列
        /// </summary>
        /// <param name="rows">行集合</param>
        /// <param name="columnHeaders">列头</param>
        /// <param name="maxRows">最大检查行数</param>
        /// <returns>空列索引集合</returns>
        private static HashSet<int> CheckEmptyColumns(IEnumerable<IXLRow> rows, List<string> columnHeaders, int maxRows)
        {
            var emptyColumns = new HashSet<int>();
            
            // 检查每一列是否为空列
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                // 如果标题为空，则检查数据行是否也全部为空
                if (string.IsNullOrEmpty(columnHeaders[i]))
                {
                    // 标题为空，检查数据行是否也全部为空
                    bool hasData = false;
                    int checkRowCount = 0;
                    int maxCheckRows = maxRows > 0 ? Math.Min(maxRows, 100) : 100; // 最多检查100行
                    
                    foreach (var row in rows)
                    {
                        if (checkRowCount >= maxCheckRows)
                            break;
                            
                        var cells = row.Cells().ToList();
                        if (i < cells.Count)
                        {
                            var cellValue = cells[i].Value;
                            if (cellValue.ToString() != string.Empty)
                            {
                                hasData = true;
                                break;
                            }
                        }
                        checkRowCount++;
                    }
                    
                    // 如果标题和数据行都为空，则认为是空列
                    if (!hasData)
                    {
                        emptyColumns.Add(i);
                    }
                }
                // 如果标题不为空，则不管数据行是否为空，都不认为是空列
            }
            
            return emptyColumns;
        }

        /// <summary>
        /// 获取工作表列表
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>工作表名称列表</returns>
        public static List<string> GetWorksheetNames(string filePath)
        {
            var worksheetNames = new List<string>();
            
            try
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    foreach (var worksheet in workbook.Worksheets)
                    {
                        worksheetNames.Add(worksheet.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取工作表列表时发生错误: {ex.Message}", ex);
            }
            
            return worksheetNames;
        }
        
        /// <summary>
        /// 获取唯一列名
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="baseName">基础名称</param>
        /// <returns>唯一列名</returns>
        private static string GetUniqueColumnName(DataTable table, string baseName)
        {
            var newName = baseName;
            var counter = 1;
            
            while (table.Columns.Contains(newName))
            {
                newName = $"{baseName}_{counter}";
                counter++;
            }
            
            return newName;
        }
    }
}