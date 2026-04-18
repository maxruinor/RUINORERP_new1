using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 增强型 Excel 解析器
    /// 借鉴 UCDataImportTool 的图片提取功能
    /// 支持从 Excel 中提取数据和图片
    /// </summary>
    public class EnhancedExcelParser
    {
        /// <summary>
        /// 解析 Excel 文件（包含图片提取）
        /// </summary>
        /// <param name="filePath">Excel 文件路径</param>
        /// <param name="sheetIndex">工作表索引（默认 0）</param>
        /// <param name="headerRowIndex">标题行索引（默认 0）</param>
        /// <returns>解析结果（包含数据和图片）</returns>
        public ExcelParseResult Parse(string filePath, int sheetIndex = 0, int headerRowIndex = 0)
        {
            var result = new ExcelParseResult
            {
                FilePath = filePath,
                SheetIndex = sheetIndex,
                HeaderRowIndex = headerRowIndex
            };

            // 1. 解析数据
            result.DataTable = ParseData(filePath, sheetIndex, headerRowIndex);

            // 2. 提取图片
            try
            {
                result.Images = ExtractImages(filePath, sheetIndex);
                
                // 3. 关联图片到数据行
                if (result.Images.Count > 0)
                {
                    AssociateImagesWithData(result);
                }
            }
            catch (Exception ex)
            {
                // 图片提取失败不影响主流程
                System.Diagnostics.Debug.WriteLine($"图片提取失败: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 解析 Excel 数据
        /// </summary>
        private DataTable ParseData(string filePath, int sheetIndex, int headerRowIndex)
        {
            // 复用现有的 DynamicExcelParser 逻辑
            var parser = new DynamicExcelParser();
            return parser.ParseExcelToDataTable(filePath, sheetIndex, headerRowIndex);
        }

        /// <summary>
        /// 从 Excel 提取图片
        /// </summary>
        private Dictionary<int, List<ExcelImageInfo>> ExtractImages(string filePath, int sheetIndex)
        {
            var imageMap = new Dictionary<int, List<ExcelImageInfo>>();

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                
                // 根据文件扩展名选择解析器
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
                    throw new NotSupportedException("不支持的文件格式，仅支持 .xls 和 .xlsx");
                }

                var sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet == null)
                    return imageMap;

                // 使用 DynamicExcelParser 提取图片
                var parser = new DynamicExcelParser();
                var images = parser.ExtractImagesFromSheet(sheet, workbook);

                // 按行索引分组
                foreach (var image in images)
                {
                    int rowIndex = image.RowIndex;
                    if (!imageMap.ContainsKey(rowIndex))
                    {
                        imageMap[rowIndex] = new List<ExcelImageInfo>();
                    }
                    imageMap[rowIndex].Add(image);
                }
            }

            return imageMap;
        }

        /// <summary>
        /// 关联图片到数据行
        /// </summary>
        private void AssociateImagesWithData(ExcelParseResult result)
        {
            if (result.DataTable == null || result.Images == null)
                return;

            // 添加图片列（如果不存在）
            if (!result.DataTable.Columns.Contains("_Images"))
            {
                result.DataTable.Columns.Add("_Images", typeof(List<ExcelImageInfo>));
            }

            // 计算数据行起始索引（标题行之后）
            int dataStartRow = result.HeaderRowIndex + 1;

            // 遍历图片映射
            foreach (var kvp in result.Images)
            {
                int excelRowIndex = kvp.Key;
                var images = kvp.Value;

                // 计算对应的数据表行索引
                int dataRowIndex = excelRowIndex - dataStartRow;

                if (dataRowIndex >= 0 && dataRowIndex < result.DataTable.Rows.Count)
                {
                    result.DataTable.Rows[dataRowIndex]["_Images"] = images;
                }
            }
        }

        /// <summary>
        /// 获取图片扩展名
        /// </summary>
        private string GetImageExtension(string suggestExtension)
        {
            if (string.IsNullOrEmpty(suggestExtension))
                return ".png";

            string ext = suggestExtension.ToLower();
            switch (ext)
            {
                case "jpeg":
                case "jpg":
                    return ".jpg";
                case "png":
                    return ".png";
                case "gif":
                    return ".gif";
                case "bmp":
                    return ".bmp";
                default:
                    return "." + ext;
            }
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

            // 如果文件已存在，添加序号
            int counter = 1;
            string baseName = fileName;
            while (File.Exists(filePath))
            {
                fullFileName = $"{baseName}_{counter}{imageInfo.ImageType}";
                filePath = Path.Combine(outputDirectory, fullFileName);
                counter++;
            }

            File.WriteAllBytes(filePath, imageInfo.ImageData);
            return filePath;
        }

        /// <summary>
        /// 批量保存图片
        /// </summary>
        /// <param name="result">解析结果</param>
        /// <param name="outputDirectory">输出目录</param>
        /// <param name="namingColumn">用于命名图片的列名</param>
        /// <returns>保存的图片路径列表</returns>
        public List<string> SaveAllImages(ExcelParseResult result, string outputDirectory, string namingColumn = null)
        {
            var savedPaths = new List<string>();

            if (result?.DataTable == null || result.Images == null)
                return savedPaths;

            int imageIndex = 1;
            foreach (DataRow row in result.DataTable.Rows)
            {
                var images = row["_Images"] as List<ExcelImageInfo>;
                if (images == null || images.Count == 0)
                    continue;

                // 获取图片文件名
                string baseName;
                if (!string.IsNullOrEmpty(namingColumn) && 
                    result.DataTable.Columns.Contains(namingColumn))
                {
                    baseName = row[namingColumn]?.ToString() ?? $"Image_{imageIndex}";
                    // 清理文件名中的非法字符
                    baseName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                }
                else
                {
                    baseName = $"Image_{imageIndex}";
                }

                // 保存该行的所有图片
                for (int i = 0; i < images.Count; i++)
                {
                    string fileName = images.Count > 1 
                        ? $"{baseName}_{i + 1}" 
                        : baseName;

                    string savedPath = SaveImageToFile(images[i], outputDirectory, fileName);
                    if (savedPath != null)
                    {
                        savedPaths.Add(savedPath);
                    }
                }

                imageIndex++;
            }

            return savedPaths;
        }
    }

    /// <summary>
    /// Excel 解析结果
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
    /// Excel 图片信息
    /// </summary>
    public class ExcelImageInfo
    {
        /// <summary>
        /// 行索引（Excel 中的行号，从 0 开始）
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
        /// 图片类型扩展名（如 .jpg, .png）
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
}
