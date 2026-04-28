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
    /// Excel图片解析信息
    /// 用于存储从Excel中提取的图片数据
    /// </summary>
    public class ExcelImageInfo
    {
        /// <summary>
        /// 行索引（Excel中的行号，从0开始）
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 图片数据
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 图片类型扩展名（如.jpg, .png）
        /// </summary>
        public string ImageType { get; set; }

        /// <summary>
        /// 图片宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 图片索引
        /// </summary>
        public int PictureIndex { get; set; }

        /// <summary>
        /// 获取图片大小（KB）
        /// </summary>
        public double SizeKB => ImageData?.Length / 1024.0 ?? 0;
    }

    /// <summary>
    /// Excel解析结果
    /// 用于存储解析后的数据和图片信息
    /// </summary>
    public class ExcelParseResult
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 工作表索引
        /// </summary>
        public int SheetIndex { get; set; }

        /// <summary>
        /// 标题行索引
        /// </summary>
        public int HeaderRowIndex { get; set; }

        /// <summary>
        /// 解析的数据表
        /// </summary>
        public DataTable DataTable { get; set; }

        /// <summary>
        /// 图片映射（行索引 -> 图片列表）
        /// </summary>
        public Dictionary<int, List<ExcelImageInfo>> Images { get; set; }

        /// <summary>
        /// 是否包含图片
        /// </summary>
        public bool HasImages => Images != null && Images.Count > 0;

        /// <summary>
        /// 获取指定行的图片
        /// </summary>
        public List<ExcelImageInfo> GetImagesForRow(int rowIndex)
        {
            if (Images?.ContainsKey(rowIndex) == true)
                return Images[rowIndex];
            return null;
        }

        /// <summary>
        /// 获取数据行的图片（考虑标题行偏移）
        /// </summary>
        public List<ExcelImageInfo> GetImagesForDataRow(int dataRowIndex)
        {
            int excelRowIndex = HeaderRowIndex + 1 + dataRowIndex;
            return GetImagesForRow(excelRowIndex);
        }
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
        /// 提取Excel工作表中的所有图片（公共方法）
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <param name="workbook">Excel工作簿</param>
        /// <returns>图片信息列表</returns>
        public List<ExcelImageInfo> ExtractImagesFromSheet(ISheet sheet, IWorkbook workbook)
        {
            return ExtractAllImages(workbook, sheet);
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
                System.Diagnostics.Debug.WriteLine($"[图片提取] 工作簿中共有 {allPictures?.Count ?? 0} 张图片");

                if (allPictures == null || allPictures.Count == 0)
                {
                    return images;
                }

                // 根据不同的工作表类型提取图片
                if (sheet is HSSFSheet hssfSheet)
                {
                    // 对于.xls文件
                    System.Diagnostics.Debug.WriteLine($"[图片提取] 检测到.xls格式的工作表");

                    var patriarch = hssfSheet.DrawingPatriarch as HSSFPatriarch;
                    if (patriarch != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[图片提取] 找到DrawingPatriarch，子对象数量: {patriarch.Children?.Count ?? 0}");

                        foreach (HSSFShape shape in patriarch.Children)
                        {
                            if (shape is HSSFPicture picture && picture.PictureData != null)
                            {
                                var imageInfo = new ExcelImageInfo
                                {
                                    // 尝试使用DISPIMG ID作为图片ID，否则使用GUID
                                    RowIndex = GetPictureRowIndex(picture),
                                    ColumnIndex = GetPictureColumnIndex(picture),
                                    ImageData = picture.PictureData.Data,
                                    ImageType = GetImageFormat(picture.PictureData.MimeType)
                                };

                                System.Diagnostics.Debug.WriteLine($"[图片提取] 提取到图片: 行={imageInfo.RowIndex}, 列={imageInfo.ColumnIndex}, 格式={imageInfo.ImageType}");
                                images.Add(imageInfo);
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[图片提取] 未找到DrawingPatriarch，尝试使用GetAllPictures方法");

                        // 备用方法：遍历所有图片，尝试匹配到单元格位置
                        foreach (var pictureDataObj in allPictures)
                        {
                            if (pictureDataObj is IPictureData pictureData)
                            {
                                var imageInfo = new ExcelImageInfo
                                {
                                    RowIndex = 0,
                                    ColumnIndex = 0,
                                    ImageData = pictureData.Data,
                                    ImageType = GetImageFormat(pictureData.MimeType)
                                };

                                System.Diagnostics.Debug.WriteLine($"[图片提取] 备用方法提取到图片: 行={imageInfo.RowIndex}, 列={imageInfo.ColumnIndex}, 格式={imageInfo.ImageType}");
                                images.Add(imageInfo);
                            }
                        }
                    }
                }
                else if (sheet is XSSFSheet xssfSheet)
                {
                    // 对于.xlsx文件
                    System.Diagnostics.Debug.WriteLine($"[图片提取] 检测到.xlsx格式的工作表");

                    var drawings = xssfSheet.GetDrawingPatriarch() as XSSFDrawing;
                    if (drawings != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[图片提取] 找到XSSFDrawing");

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
                                    RowIndex = rowIndex,
                                    ColumnIndex = colIndex,
                                    ImageData = picture.PictureData.Data,
                                    ImageType = GetImageFormat(picture.PictureData.MimeType)
                                };

                                System.Diagnostics.Debug.WriteLine($"[图片提取] 提取到图片: 行={rowIndex}, 列={colIndex}, 格式={imageInfo.ImageType}");
                                images.Add(imageInfo);
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[图片提取] 未找到XSSFDrawing");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[图片提取] 总共提取到 {images.Count} 张图片");
            }
            catch (Exception ex)
            {
                // 图片提取失败不影响数据读取，只记录错误
                System.Diagnostics.Debug.WriteLine($"[图片提取错误] 提取Excel图片失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[图片提取错误] 堆栈跟踪: {ex.StackTrace}");
            }

            return images;
        }

        /// <summary>
        /// 获取图片ID（优先使用DISPIMG公式中引用的ID）
        /// </summary>
        /// <param name="picture">图片对象</param>
        /// <returns>图片ID</returns>
        private string GetPictureId(object picture)
        {
            try
            {
                // 尝试通过反射获取图片的真实ID
                // NPOI中，XSSFPicture有GetPackageRelationshipId方法
                var type = picture.GetType();
                var method = type.GetMethod("GetPackageRelationshipId", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (method != null)
                {
                    var result = method.Invoke(picture, null);
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }

                // 如果获取不到，尝试通过Formula获取
                var formulaProperty = type.GetProperty("Formula", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (formulaProperty != null)
                {
                    var formula = formulaProperty.GetValue(picture)?.ToString();
                    if (!string.IsNullOrEmpty(formula))
                    {
                        // 尝试从公式中提取ID
                        var match = System.Text.RegularExpressions.Regex.Match(formula, @"DISPIMG\s*\(\s*[""']([^""']+)[""']");
                        if (match.Success)
                        {
                            return match.Groups[1].Value;
                        }
                    }
                }

                // 如果都获取不到，返回GUID
                return Guid.NewGuid().ToString();
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
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

            // 检查是否是DISPIMG公式（支持 _xlfn.DISPIMG 和 DISPIMG）
            if (formula != null &&
                formula.IndexOf("DISPIMG", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                System.Diagnostics.Debug.WriteLine($"[公式处理] 发现DISPIMG公式: {formula}, 位置: 行{rowIndex}, 列{colIndex}");

                // 提取图片ID
                var match = Regex.Match(formula, @"DISPIMG\s*\(\s*[""']([^""']+)[""']\s*,\s*\d+\s*\)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string imageId = match.Groups[1].Value;
                    System.Diagnostics.Debug.WriteLine($"[公式处理] 提取到图片ID: {imageId}");

                    // 查找对应的图片（通过行号列号匹配）
                    var image = imageDictionary.Values.FirstOrDefault(img => img.RowIndex == rowIndex && img.ColumnIndex == colIndex);
                    if (image != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[公式处理] 找到匹配的图片，保存路径中...");
                        return SaveImage(image, rowIndex, colIndex);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[公式处理] 未找到匹配的图片，图片字典中有 {imageDictionary.Count} 张图片");
                        // 打印所有图片位置用于调试
                        foreach (var img in imageDictionary.Values)
                        {
                            System.Diagnostics.Debug.WriteLine($"[公式处理] 字典中的图片位置: 行={img.RowIndex}, 列={img.ColumnIndex}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[公式处理] 正则匹配失败");
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
                // 确保目录存在
                if (!Directory.Exists(_imageSavePath))
                {
                    Directory.CreateDirectory(_imageSavePath);
                    System.Diagnostics.Debug.WriteLine($"[保存图片] 创建图片保存目录: {_imageSavePath}");
                }

                // 使用图片ID（DISPIMG中的ID）作为文件名的一部分
                string fileName;

                // 使用行号列号和GUID生成文件名
                fileName = $"image_r{rowIndex}_c{colIndex}_{Guid.NewGuid().ToString().Substring(0, 8)}{imageInfo.ImageType}";

                string fullPath = Path.Combine(_imageSavePath, fileName);

                // 检查文件是否已存在，避免重复保存
                if (File.Exists(fullPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[保存图片] 图片已存在，跳过保存: {fileName}");
                    return fileName;
                }

                File.WriteAllBytes(fullPath, imageInfo.ImageData);
                System.Diagnostics.Debug.WriteLine($"[保存图片] 成功保存图片: {fileName}, 路径: {fullPath}, 大小: {imageInfo.ImageData.Length} 字节");
                return fileName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[保存图片] 保存图片失败: {ex.Message}");
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

        /// <summary>
        /// 解析Excel文件（支持图片提取）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetIndex">工作表索引（默认0）</param>
        /// <param name="headerRowIndex">标题行索引（默认0）</param>
        /// <returns>解析结果</returns>
        public ExcelParseResult Parse(string filePath, int sheetIndex = 0, int headerRowIndex = 0)
        {
            var result = new ExcelParseResult
            {
                FilePath = filePath,
                SheetIndex = sheetIndex,
                HeaderRowIndex = headerRowIndex
            };

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook;
                if (filePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new HSSFWorkbook(fs);
                }
                else
                {
                    throw new NotSupportedException("不支持的文件格式");
                }

                var sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet == null)
                    return result;

                var images = ExtractAllImages(workbook, sheet);
                result.DataTable = SheetToDataTable(sheet, -1, images);
                result.Images = new Dictionary<int, List<ExcelImageInfo>>();

                foreach (var image in images)
                {
                    if (!result.Images.ContainsKey(image.RowIndex))
                    {
                        result.Images[image.RowIndex] = new List<ExcelImageInfo>();
                    }
                    result.Images[image.RowIndex].Add(image);
                }
            }

            return result;
        }

        /// <summary>
        /// 保存图片到文件
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="outputDirectory">输出目录</param>
        /// <param name="fileName">文件名（不含扩展名）</param>
        /// <returns>保存的文件路径</returns>
        public string SaveImageToFile(ExcelImageInfo imageInfo, string outputDirectory, string fileName)
        {
            if (imageInfo?.ImageData == null)
                return null;

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string fullFileName = $"{fileName}{imageInfo.ImageType}";
            string filePath = Path.Combine(outputDirectory, fullFileName);

            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, imageInfo.ImageData);
            }

            return filePath;
        }

        /// <summary>
        /// 解析Excel文件的指定列（性能优化版本，只读取配置中映射的列）
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetIndex">Sheet索引（从0开始）</param>
        /// <param name="columnMappings">列映射配置列表</param>
        /// <param name="maxPreviewRows">预览最大行数，-1表示全部加载</param>
        /// <returns>解析后的DataTable（只包含映射配置的列）</returns>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        /// <exception cref="ArgumentException">当文件格式不支持时抛出</exception>
        /// <exception cref="Exception">当解析过程中发生错误时抛出</exception>
        public DataTable ParseExcelWithColumns(string filePath, int sheetIndex, List<ColumnMapping> columnMappings, int maxPreviewRows = -1)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的Excel文件不存在", filePath);
            }

            if (columnMappings == null || columnMappings.Count == 0)
            {
                throw new ArgumentException("列映射配置不能为空", nameof(columnMappings));
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

                // 将工作表转换为DataTable（只读取配置的列）
                dataTable = SheetToDataTableWithColumns(sheet, columnMappings, maxPreviewRows, images);
            }

            return dataTable;
        }

        /// <summary>
        /// 将Excel工作表转换为DataTable（只读取配置中映射的列）
        /// </summary>
        /// <param name="sheet">Excel工作表</param>
        /// <param name="columnMappings">列映射配置列表</param>
        /// <param name="maxPreviewRows">预览最大行数，-1表示全部加载</param>
        /// <param name="images">图片信息列表</param>
        /// <returns>转换后的DataTable（只包含映射配置的列）</returns>
        private DataTable SheetToDataTableWithColumns(ISheet sheet, List<ColumnMapping> columnMappings, int maxPreviewRows = -1, List<ExcelImageInfo> images = null)
        {
            DataTable dataTable = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                throw new Exception("Excel工作表没有标题行");
            }

            // 构建需要读取的Excel列名集合（包括直接映射、外键来源列、拼接源列等）
            var requiredExcelColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var columnIndexMap = new Dictionary<string, int>(); // Excel列名 -> 列索引

            foreach (var mapping in columnMappings)
            {
                // 1. 添加Excel数据源的列
                if (mapping.DataSourceType == DataSourceType.Excel && !string.IsNullOrEmpty(mapping.ExcelColumn))
                {
                    requiredExcelColumns.Add(mapping.ExcelColumn);
                }

                // 2. 添加外键关联的来源列
                if (mapping.DataSourceType == DataSourceType.ForeignKey &&
                    mapping.ForeignConfig != null &&
                    !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn?.Key))
                {
                    requiredExcelColumns.Add(mapping.ForeignConfig.ForeignKeySourceColumn.Key);
                }

                // 3. 添加列拼接的源列
                if (mapping.DataSourceType == DataSourceType.ColumnConcat &&
                    mapping.ConcatConfig != null &&
                    mapping.ConcatConfig.SourceColumns != null)
                {
                    foreach (var sourceCol in mapping.ConcatConfig.SourceColumns)
                    {
                        if (!string.IsNullOrEmpty(sourceCol))
                        {
                            requiredExcelColumns.Add(sourceCol);
                        }
                    }
                }

                // 4. 添加字段复制的源列
                if (mapping.DataSourceType == DataSourceType.FieldCopy &&
                    !string.IsNullOrEmpty(mapping.CopyFromField?.Key))
                {
                    // 需要找到被复制字段的Excel列
                    var copyFromMapping = columnMappings.FirstOrDefault(m => m.SystemField?.Key == mapping.CopyFromField.Key);
                    if (copyFromMapping != null && !string.IsNullOrEmpty(copyFromMapping.ExcelColumn))
                    {
                        requiredExcelColumns.Add(copyFromMapping.ExcelColumn);
                    }
                }
            }

            // 建立Excel列名到列索引的映射
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                ICell cell = headerRow.GetCell(i);
                if (cell != null)
                {
                    string columnName = GetCellValue(cell).Trim();
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        columnIndexMap[columnName] = i;
                    }
                }
            }

            // 创建结果表的列（使用SystemField.Value作为列名）
            // 数据库列名与Excel列名的对应关系通过ColumnMapping配置管理
            var addedColumns = new HashSet<string>();
            foreach (var mapping in columnMappings)
            {
                string columnName = mapping.SystemField?.Value;
                if (!string.IsNullOrEmpty(columnName) && !addedColumns.Contains(columnName))
                {
                    dataTable.Columns.Add(columnName, typeof(string));
                    addedColumns.Add(columnName);
                }

                // 对于外键关联类型，额外添加外键来源列到结果表中
                if (mapping.DataSourceType == DataSourceType.ForeignKey &&
                    mapping.ForeignConfig != null &&
                    !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn?.Key))
                {
                    string sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn.Key;
                    if (!addedColumns.Contains(sourceColumnName))
                    {
                        dataTable.Columns.Add(sourceColumnName, typeof(string));
                        addedColumns.Add(sourceColumnName);
                    }
                }
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

            // 填充DataTable的数据行（只读取需要的列）
            for (int i = 1; i < lastRowToRead; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    DataRow dataRow = dataTable.NewRow();
                    bool hasData = false;

                    // 遍历每个映射配置，按需读取对应的Excel列
                    foreach (var mapping in columnMappings)
                    {
                        string targetColumnName = mapping.SystemField?.Value;
                        if (string.IsNullOrEmpty(targetColumnName))
                            continue;

                        object cellValue = null;
                        bool isImageColumn = false;

                        // 根据不同的数据来源类型读取数据
                        switch (mapping.DataSourceType)
                        {
                            case DataSourceType.Excel:
                                if (!string.IsNullOrEmpty(mapping.ExcelColumn) &&
                                    columnIndexMap.TryGetValue(mapping.ExcelColumn, out int excelColIndex))
                                {
                                    ICell cell = row.GetCell(excelColIndex);
                                    if (cell != null)
                                    {
                                        // 检查是否是图片公式
                                        if (cell.CellType == CellType.Formula)
                                        {
                                            cellValue = HandleFormulaCell(cell, imageDictionary, i, excelColIndex);
                                            isImageColumn = mapping.IsImageColumn;
                                        }
                                        else
                                        {
                                            cellValue = GetCellValue(cell);
                                        }
                                    }
                                }
                                break;

                            case DataSourceType.ForeignKey:
                                // 读取外键来源列的值
                                if (mapping.ForeignConfig != null &&
                                    !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn?.Key) &&
                                    columnIndexMap.TryGetValue(mapping.ForeignConfig.ForeignKeySourceColumn.Key, out int fkColIndex))
                                {
                                    ICell cell = row.GetCell(fkColIndex);
                                    if (cell != null)
                                    {
                                        cellValue = GetCellValue(cell);
                                    }
                                }
                                break;

                            case DataSourceType.ColumnConcat:
                                // 列拼接不需要在这里处理，在ApplyColumnMapping中统一处理
                                cellValue = "";
                                break;

                            case DataSourceType.FieldCopy:
                                // 字段复制不需要在这里处理，在ApplyColumnMapping中统一处理
                                cellValue = "";
                                break;

                            default:
                                // 其他类型（系统生成、默认值等）在ApplyColumnMapping中处理
                                cellValue = "";
                                break;
                        }

                        // 设置目标列的值
                        if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                        {
                            dataRow[targetColumnName] = cellValue;
                            hasData = true;
                        }
                        else
                        {
                            dataRow[targetColumnName] = DBNull.Value;
                        }

                        // 如果是外键关联，同时设置外键来源列的值
                        if (mapping.DataSourceType == DataSourceType.ForeignKey &&
                            mapping.ForeignConfig != null &&
                            !string.IsNullOrEmpty(mapping.ForeignConfig.ForeignKeySourceColumn?.Key))
                        {
                            string sourceColumnName = mapping.ForeignConfig.ForeignKeySourceColumn.Key;
                            if (dataTable.Columns.Contains(sourceColumnName) && cellValue != null)
                            {
                                dataRow[sourceColumnName] = cellValue;
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
    }
}
