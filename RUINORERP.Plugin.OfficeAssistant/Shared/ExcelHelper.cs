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
                    
                    // 不再过滤空列，保留所有列以确保数据完整性
                    // 添加所有列到DataTable
                    for (int i = 0; i < columnHeaders.Count; i++)
                    {
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
                        
                        // 为每一列填充数据
                        for (int colIndex = 0; colIndex < columnHeaders.Count; colIndex++)
                        {
                            // 正确地从cells中获取数据
                            // 需要找到对应列号的单元格
                            var cell = cells.FirstOrDefault(c => c.Address.ColumnNumber == (colIndex + 1));
                            if (cell != null)
                            {
                                dataRow[colIndex] = cell.Value;
                            }
                            else
                            {
                                // 如果单元格不存在或为空，设置为空值
                                dataRow[colIndex] = DBNull.Value;
                            }
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