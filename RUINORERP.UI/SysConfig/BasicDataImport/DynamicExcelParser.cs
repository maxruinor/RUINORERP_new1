using System;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 动态Excel文件解析器
    /// 用于读取不同格式的Excel文件并转换为DataTable
    /// </summary>
    public class DynamicExcelParser
    {
        /// <summary>
        /// 解析Excel文件为DataTable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <returns>解析后的DataTable</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="ArgumentException">当文件格式不支持时抛出</exception>
        /// <exception cref="Exception">当解析过程中发生错误时抛出</exception>
        public DataTable ParseExcelToDataTable(string filePath)
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

                // 获取第一个工作表
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet == null)
                {
                    throw new Exception("Excel文件中没有工作表");
                }

                // 将工作表转换为DataTable
                dataTable = SheetToDataTable(sheet);
            }

            return dataTable;
        }

        /// <summary>
        /// 将Excel工作表转换为DataTable
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <returns>转换后的DataTable</returns>
        private DataTable SheetToDataTable(ISheet sheet)
        {
            DataTable dataTable = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            // 创建DataTable的列
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                ICell cell = headerRow.GetCell(i);
                if (cell != null)
                {
                    string columnName = cell.ToString();
                    // 如果列名重复，添加序号后缀
                    if (dataTable.Columns.Contains(columnName))
                    {
                        int suffix = 1;
                        while (dataTable.Columns.Contains($"{columnName}_{suffix}"))
                        {
                            suffix++;
                        }
                        columnName = $"{columnName}_{suffix}";
                    }
                    dataTable.Columns.Add(columnName);
                }
                else
                {
                    // 处理空列名
                    dataTable.Columns.Add($"Column_{i + 1}");
                }
            }

            // 填充DataTable的数据行
            for (int i = 1; i <= sheet.LastRowNum; i++)
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
                        cellValue = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
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