using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// Excel图片信息
    /// </summary>
    public class ExcelImageInfo
    {
        /// <summary>
        /// 图片ID（从DISPIMG公式中提取）
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        /// 图片数据
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        public string ImageFormat { get; set; }

        /// <summary>
        /// 所在行号
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 所在列号
        /// </summary>
        public int ColumnIndex { get; set; }
    }

    /// <summary>
    /// 动态Excel文件解析器
    /// 用于读取不同格式的Excel文件并转换为DataTable
    /// 支持提取Excel中的内嵌图片
    /// </summary>
    public class DynamicExcelParser
    {
        /// <summary>
        /// 图片保存目录
        /// </summary>
        private readonly string _imageSavePath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DynamicExcelParser()
        {
            _imageSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages");
            if (!Directory.Exists(_imageSavePath))
            {
                Directory.CreateDirectory(_imageSavePath);
            }
        }

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

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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

                // 提取所有图片信息
                var images = ExtractAllImages(workbook, sheet);

                // 将工作表转换为DataTable
                dataTable = SheetToDataTable(sheet, maxPreviewRows, images);
            }

            return dataTable;
        }

        /// <summary>
        /// 解析Excel文件的指定名称的Sheet为DataTable（支持预览行数限制）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetName">Sheet名称</param>
        /// <param name="maxPreviewRows">预览最大行数，-1表示全部加载</param>
        /// <returns>解析后的DataTable</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="ArgumentException">当文件格式不支持时抛出</exception>
        /// <exception cref="Exception">当解析过程中发生错误时抛出</exception>
        public DataTable ParseExcelToDataTable(string filePath, string sheetName, int maxPreviewRows)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的Excel文件不存在", filePath);
            }

            string fileExtension = Path.GetExtension(filePath).ToLower();
            IWorkbook workbook = null;
            DataTable dataTable = null;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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

                // 根据名称获取工作表
                ISheet sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                {
                    throw new Exception($"Excel文件中没有名称为 '{sheetName}' 的工作表");
                }

                // 提取所有图片信息
                var images = ExtractAllImages(workbook, sheet);

                // 将工作表转换为DataTable
                dataTable = SheetToDataTable(sheet, maxPreviewRows, images);
            }

            return dataTable;
        }

        /// <summary>
        /// 提取Excel工作表中的所有图片
        /// </summary>
        /// <param name="workbook">Excel工作簿</param>
        /// <param name="sheet">Excel工作表</param>
        /// <returns>图片信息列表</returns>
        private List<ExcelImageInfo> ExtractAllImages(IWorkbook workbook, ISheet sheet)
        {
            var images = new List<ExcelImageInfo>();

            try
            {
                // 获取工作簿中的所有图片数据
                var allPictures = workbook.GetAllPictures();
                if (allPictures == null || allPictures.Count == 0)
                {
                    return images;
                }

                // 根据不同的工作表类型提取图片
                if (sheet is HSSFSheet hssfSheet)
                {
                    // 对于.xls文件
                    var patriarch = hssfSheet.DrawingPatriarch as HSSFPatriarch;
                    if (patriarch != null)
                    {
                        foreach (HSSFShape shape in patriarch.Children)
                        {
                            if (shape is HSSFPicture picture && picture.PictureData != null)
                            {
                                var imageInfo = new ExcelImageInfo
                                {
                                    ImageId = Guid.NewGuid().ToString(),
                                    ImageData = picture.PictureData.Data,
                                    ImageFormat = GetImageFormat(picture.PictureData.MimeType),
                                    RowIndex = GetPictureRowIndex(picture),
                                    ColumnIndex = GetPictureColumnIndex(picture)
                                };
                                images.Add(imageInfo);
                            }
                        }
                    }
                }
                else if (sheet is XSSFSheet xssfSheet)
                {
                    // 对于.xlsx文件
                    var drawings = xssfSheet.GetDrawingPatriarch() as XSSFDrawing;
                    if (drawings != null)
                    {
                        foreach (var shape in drawings.GetShapes())
                        {
                            if (shape is XSSFPicture picture && picture.PictureData != null)
                            {
                                // 获取图片的锚点位置信息
                                var anchor = picture.GetAnchor() as XSSFClientAnchor;
                                int rowIndex = anchor?.Row1 ?? 0;
                                int colIndex = anchor?.Col1 ?? 0;

                                var imageInfo = new ExcelImageInfo
                                {
                                    ImageId = Guid.NewGuid().ToString(),
                                    ImageData = picture.PictureData.Data,
                                    ImageFormat = GetImageFormat(picture.PictureData.MimeType),
                                    RowIndex = rowIndex,
                                    ColumnIndex = colIndex
                                };
                                images.Add(imageInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 图片提取失败不影响数据读取，只记录错误
                System.Diagnostics.Debug.WriteLine($"提取Excel图片失败: {ex.Message}");
            }

            return images;
        }

        /// <summary>
        /// 获取图片格式
        /// </summary>
        /// <param name="mimeType">MIME类型</param>
        /// <returns>图片格式</returns>
        private string GetImageFormat(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                return ".png";
            }

            switch (mimeType.ToLower())
            {
                case "image/jpeg":
                case "image/jpg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                case "image/gif":
                    return ".gif";
                case "image/bmp":
                    return ".bmp";
                default:
                    return ".png";
            }
        }

        /// <summary>
        /// 获取图片所在行号
        /// </summary>
        /// <param name="picture">图片对象</param>
        /// <returns>行号</returns>
        private int GetPictureRowIndex(HSSFPicture picture)
        {
            var anchor = picture.Anchor as HSSFClientAnchor;
            return anchor?.Row1 ?? 0;
        }

        /// <summary>
        /// 获取图片所在列号
        /// </summary>
        /// <param name="picture">图片对象</param>
        /// <returns>列号</returns>
        private int GetPictureColumnIndex(HSSFPicture picture)
        {
            var anchor = picture.Anchor as HSSFClientAnchor;
            return anchor?.Col1 ?? 0;
        }



        /// <summary>
        /// 将Excel工作表转换为DataTable（性能优化版本，支持图片）
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <param name="maxPreviewRows">预览最大行数，-1表示全部加载</param>
        /// <param name="images">图片信息列表</param>
        /// <returns>转换后的DataTable</returns>
        private DataTable SheetToDataTable(ISheet sheet, int maxPreviewRows = -1, List<ExcelImageInfo> images = null)
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
                    columnName = GetCellValue(cell).Trim();
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

            // 创建图片字典，按行号和列号索引
            var imageDictionary = new Dictionary<(int row, int col), ExcelImageInfo>();
            if (images != null)
            {
                foreach (var image in images)
                {
                    var key = (image.RowIndex, image.ColumnIndex);
                    if (!imageDictionary.ContainsKey(key))
                    {
                        imageDictionary[key] = image;
                    }
                }
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
                            // 检查是否是图片公式
                            if (cell.CellType == CellType.Formula)
                            {
                                string cellValue = HandleFormulaCell(cell, imageDictionary, i, j);
                                dataRow[j] = cellValue;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    hasData = true;
                                }
                            }
                            else
                            {
                                string cellValue = GetCellValue(cell);
                                dataRow[j] = cellValue;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    hasData = true;
                                }
                            }
                        }
                        else
                        {
                            // 检查该位置是否有图片
                            if (imageDictionary.TryGetValue((i, j), out ExcelImageInfo imageInfo))
                            {
                                string imagePath = SaveImage(imageInfo, i, j);
                                dataRow[j] = imagePath;
                                hasData = true;
                            }
                            else
                            {
                                dataRow[j] = string.Empty;
                            }
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
        /// 处理包含公式的单元格
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="imageDictionary">图片字典</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colIndex">列号</param>
        /// <returns>单元格值</returns>
        private string HandleFormulaCell(ICell cell, Dictionary<(int row, int col), ExcelImageInfo> imageDictionary, int rowIndex, int colIndex)
        {
            string formula = cell.CellFormula;

            // 检查是否是DISPIMG公式
            if (formula != null && formula.StartsWith("=DISPIMG", StringComparison.OrdinalIgnoreCase))
            {
                // 提取图片ID
                var match = Regex.Match(formula, @"DISPIMG\s*\(\s*[""']([^""']+)[""']\s*,\s*\d+\s*\)");
                if (match.Success)
                {
                    string imageId = match.Groups[1].Value;

                    // 查找对应的图片
                    var image = imageDictionary.Values.FirstOrDefault(img => img.ImageId.Contains(imageId));
                    if (image != null)
                    {
                        return SaveImage(image, rowIndex, colIndex);
                    }
                }
            }

            // 尝试获取公式的计算结果
            try
            {
                switch (cell.CachedFormulaResultType)
                {
                    case CellType.String:
                        return cell.StringCellValue?.Trim() ?? string.Empty;
                    case CellType.Numeric:
                        return cell.NumericCellValue.ToString();
                    case CellType.Boolean:
                        return cell.BooleanCellValue.ToString();
                    default:
                        return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 保存图片到本地
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colIndex">列号</param>
        /// <returns>保存后的图片路径</returns>
        private string SaveImage(ExcelImageInfo imageInfo, int rowIndex, int colIndex)
        {
            try
            {
                string fileName = $"image_r{rowIndex}_c{colIndex}_{Guid.NewGuid().ToString().Substring(0, 8)}{imageInfo.ImageFormat}";
                string fullPath = Path.Combine(_imageSavePath, fileName);
                File.WriteAllBytes(fullPath, imageInfo.ImageData);
                return fileName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存图片失败: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取Excel单元格的值（性能优化版本）
        /// </summary>
        /// <param name="cell">Excel单元格</param>
        /// <returns>单元格的值</returns>
        private string GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            try
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        return cell.StringCellValue?.Trim() ?? string.Empty;

                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            // 使用更高效的日期转换
                            double dateValue = cell.NumericCellValue;
                            DateTime dateTime = DateTime.FromOADate(dateValue);
                            return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            return cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                        }

                    case CellType.Boolean:
                        return cell.BooleanCellValue ? "1" : "0";

                    case CellType.Blank:
                        return string.Empty;

                    case CellType.Formula:
                        // 性能优化：先尝试使用缓存值
                        try
                        {
                            switch (cell.CachedFormulaResultType)
                            {
                                case CellType.String:
                                    return cell.StringCellValue?.Trim() ?? string.Empty;
                                case CellType.Numeric:
                                    return cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                                case CellType.Boolean:
                                    return cell.BooleanCellValue ? "1" : "0";
                                default:
                                    return string.Empty;
                            }
                        }
                        catch
                        {
                            return string.Empty;
                        }

                    default:
                        return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
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

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
