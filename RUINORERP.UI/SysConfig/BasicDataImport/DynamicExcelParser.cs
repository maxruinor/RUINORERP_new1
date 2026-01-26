using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 动态Excel文件解析器
    /// 用于读取不同格式的Excel文件并转换为DataTable
    /// </summary>
    public class DynamicExcelParser
    {
        /// <summary>
        /// 解析Excel文件为DataTable（默认第一个Sheet）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <returns>解析后的DataTable</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="ArgumentException">当文件格式不支持时抛出</exception>
        /// <exception cref="Exception">当解析过程中发生错误时抛出</exception>
        public DataTable ParseExcelToDataTable(string filePath)
        {
            return ParseExcelToDataTable(filePath, 0);
        }

        /// <summary>
        /// 解析Excel文件的指定Sheet为DataTable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetIndex">Sheet索引（从0开始）</param>
        /// <returns>解析后的DataTable</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="ArgumentException">当文件格式不支持时抛出</exception>
        /// <exception cref="Exception">当解析过程中发生错误时抛出</exception>
        public DataTable ParseExcelToDataTable(string filePath, int sheetIndex)
        {
            return ParseExcelToDataTable(filePath, sheetIndex, -1);
        }

        /// <summary>
        /// 解析Excel文件的指定Sheet为DataTable（支持预览行数限制）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetIndex">Sheet索引（从0开始）</param>
        /// <param name="maxPreviewRows">预览最大行数，-1表示全部加载</param>
        /// <returns>解析后的DataTable</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="ArgumentException">当文件格式不支持时抛出</exception>
        /// <exception cref="Exception">当解析过程中发生错误时抛出</exception>
        public DataTable ParseExcelToDataTable(string filePath, int sheetIndex, int maxPreviewRows)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的Excel文件不存在", filePath);
            }

            string fileExtension = Path.GetExtension(filePath).ToLower();
            IWorkbook workbook = null;
            DataTable dataTable = null;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // 根据文件扩展名创建对应的Workbook对象
                switch (fileExtension)
                {
                    case ".xls":
                        workbook = new HSSFWorkbook(fs);
                        break;
                    case ".xlsx":
                        workbook = new XSSFWorkbook(fs);
                        break;
                    default:
                        throw new ArgumentException("不支持的文件格式，仅支持.xls和.xlsx格式的Excel文件");
                }

                // 检查Sheet索引是否有效
                if (sheetIndex < 0 || sheetIndex >= workbook.NumberOfSheets)
                {
                    throw new Exception($"Sheet索引 {sheetIndex} 无效，Excel文件中共有 {workbook.NumberOfSheets} 个工作表");
                }

                // 获取指定的工作表
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet == null)
                {
                    throw new Exception($"Excel文件中没有工作表 {sheetIndex}");
                }

                // 将工作表转换为DataTable
                dataTable = SheetToDataTable(sheet, maxPreviewRows);
            }

            return dataTable;
        }

        /// <summary>
        /// 将Excel工作表转换为DataTable（性能优化版本）
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <param name="maxPreviewRows">预览最大行数，-1表示全部加载</param>
        /// <returns>转换后的DataTable</returns>
        private DataTable SheetToDataTable(ISheet sheet, int maxPreviewRows = -1)
        {
            DataTable dataTable = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                throw new Exception("Excel工作表没有标题行");
            }

            int cellCount = headerRow.LastCellNum;
            var columnNames = new List<string>();

            // 创建DataTable的列
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                ICell cell = headerRow.GetCell(i);
                string columnName;

                if (cell != null)
                {
                    columnName = cell.ToString().Trim();
                    // 如果列名重复，添加序号后缀
                    if (columnNames.Contains(columnName))
                    {
                        int suffix = 1;
                        while (columnNames.Contains($"{columnName}_{suffix}"))
                        {
                            suffix++;
                        }
                        columnName = $"{columnName}_{suffix}";
                    }
                }
                else
                {
                    // 处理空列名
                    columnName = $"Column_{i + 1}";
                }

                columnNames.Add(columnName);
                dataTable.Columns.Add(columnName);
            }

            // 确定要读取的行数
            int lastRowToRead = maxPreviewRows > 0 ? Math.Min(maxPreviewRows + 1, sheet.LastRowNum + 1) : sheet.LastRowNum + 1;

            // 填充DataTable的数据行
            for (int i = 1; i < lastRowToRead; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    DataRow dataRow = dataTable.NewRow();
                    bool hasData = false;

                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell != null)
                        {
                            string cellValue = GetCellValue(cell);
                            dataRow[j] = cellValue;
                            if (!string.IsNullOrEmpty(cellValue))
                            {
                                hasData = true;
                            }
                        }
                        else
                        {
                            dataRow[j] = string.Empty;
                        }
                    }

                    // 只添加包含数据的行
                    if (hasData)
                    {
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// 获取Excel单元格的值
        /// </summary>
        /// <param name="cell">Excel单元格</param>
        /// <returns>单元格的值</returns>
        private string GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            string cellValue = string.Empty;

            switch (cell.CellType)
            {
                case CellType.String:
                    cellValue = cell.StringCellValue;
                    break;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        // 将NPOI的Java Date类型转换为.NET DateTime类型
                        DateTime dateTime = DateTime.FromOADate(cell.NumericCellValue);
                        cellValue = dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        cellValue = cell.NumericCellValue.ToString();
                    }
                    break;
                case CellType.Boolean:
                    cellValue = cell.BooleanCellValue.ToString();
                    break;
                case CellType.Formula:
                    try
                    {
                        cellValue = cell.NumericCellValue.ToString();
                    }
                    catch
                    {
                        cellValue = cell.StringCellValue;
                    }
                    break;
                default:
                    cellValue = string.Empty;
                    break;
            }

            return cellValue.Trim();
        }

        /// <summary>
        /// 获取Excel文件中的所有工作表名称
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <returns>工作表名称列表</returns>
        public string[] GetSheetNames(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的Excel文件不存在", filePath);
            }

            string fileExtension = Path.GetExtension(filePath).ToLower();
            IWorkbook workbook = null;
            string[] sheetNames;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // 根据文件扩展名创建对应的Workbook对象
                switch (fileExtension)
                {
                    case ".xls":
                        workbook = new HSSFWorkbook(fs);
                        break;
                    case ".xlsx":
                        workbook = new XSSFWorkbook(fs);
                        break;
                    default:
                        throw new ArgumentException("不支持的文件格式，仅支持.xls和.xlsx格式的Excel文件");
                }

                // 获取所有工作表名称
                sheetNames = new string[workbook.NumberOfSheets];
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    sheetNames[i] = workbook.GetSheetName(i);
                }
            }

            return sheetNames;
        }
    }
}