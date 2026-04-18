using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.Business.ImportEngine.Services
{
    /// <summary>
    /// 图片提取服务实现
    /// 负责从Excel文件中提取嵌入的图片，支持DISPIMG公式识别
    /// </summary>
    public class ImageExtractionService : IImageExtractionService
    {
        private string _outputDirectory;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImageExtractionService()
        {
            // 默认输出目录
            _outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportImages");
            EnsureDirectoryExists(_outputDirectory);
        }

        /// <summary>
        /// 从Excel文件中提取所有图片
        /// 支持.xls和.xlsx格式，支持DISPIMG公式识别
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetIndex">工作表索引（从0开始）</param>
        /// <returns>图片信息列表</returns>
        public async Task<List<ImageInfo>> ExtractImagesAsync(string excelFilePath, int sheetIndex = 0)
        {
            return await Task.Run(() =>
            {
                var images = new List<ImageInfo>();

                if (!File.Exists(excelFilePath))
                {
                    Debug.WriteLine($"[图片提取] 文件不存在: {excelFilePath}");
                    return images;
                }

                try
                {
                    using (var fs = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        IWorkbook workbook = CreateWorkbook(fs, excelFilePath);
                        if (workbook == null)
                        {
                            return images;
                        }

                        // 检查Sheet索引是否有效
                        if (sheetIndex < 0 || sheetIndex >= workbook.NumberOfSheets)
                        {
                            Debug.WriteLine($"[图片提取] Sheet索引 {sheetIndex} 无效");
                            return images;
                        }

                        ISheet sheet = workbook.GetSheetAt(sheetIndex);
                        if (sheet == null)
                        {
                            Debug.WriteLine($"[图片提取] Sheet {sheetIndex} 不存在");
                            return images;
                        }

                        // 提取图片
                        images = ExtractImagesFromSheet(workbook, sheet);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[图片提取错误] 提取Excel图片失败: {ex.Message}");
                    Debug.WriteLine($"[图片提取错误] 堆栈跟踪: {ex.StackTrace}");
                }

                Debug.WriteLine($"[图片提取] 总共提取到 {images.Count} 张图片");
                return images;
            });
        }

        /// <summary>
        /// 保存图片到指定目录
        /// </summary>
        /// <param name="imageData">图片数据</param>
        /// <param name="fileName">文件名（不含扩展名）</param>
        /// <param name="format">图片格式</param>
        /// <returns>保存后的完整路径</returns>
        public async Task<string> SaveImageAsync(byte[] imageData, string fileName, string format = "png")
        {
            return await Task.Run(() =>
            {
                try
                {
                    EnsureDirectoryExists(_outputDirectory);

                    // 确保文件名包含扩展名
                    if (!fileName.EndsWith($".{format}", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = $"{fileName}.{format}";
                    }

                    string fullPath = Path.Combine(_outputDirectory, fileName);

                    // 检查文件是否已存在，避免重复保存
                    if (File.Exists(fullPath))
                    {
                        Debug.WriteLine($"[保存图片] 图片已存在，跳过保存: {fileName}");
                        return fullPath;
                    }

                    File.WriteAllBytes(fullPath, imageData);
                    Debug.WriteLine($"[保存图片] 成功保存图片: {fileName}, 路径: {fullPath}, 大小: {imageData.Length} 字节");

                    return fullPath;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[保存图片] 保存图片失败: {ex.Message}");
                    return string.Empty;
                }
            });
        }

        /// <summary>
        /// 设置图片保存目录
        /// </summary>
        /// <param name="outputDirectory">输出目录路径</param>
        public void SetOutputDirectory(string outputDirectory)
        {
            if (string.IsNullOrEmpty(outputDirectory))
            {
                throw new ArgumentNullException(nameof(outputDirectory));
            }

            _outputDirectory = outputDirectory;
            EnsureDirectoryExists(_outputDirectory);
        }

        #region 私有方法

        /// <summary>
        /// 创建工作簿对象
        /// </summary>
        private IWorkbook CreateWorkbook(FileStream fs, string filePath)
        {
            string fileExtension = Path.GetExtension(filePath).ToLower();

            switch (fileExtension)
            {
                case ".xls":
                    return new HSSFWorkbook(fs);
                case ".xlsx":
                    return new XSSFWorkbook(fs);
                default:
                    Debug.WriteLine($"[图片提取] 不支持的文件格式: {fileExtension}");
                    return null;
            }
        }

        /// <summary>
        /// 从工作表中提取图片
        /// </summary>
        private List<ImageInfo> ExtractImagesFromSheet(IWorkbook workbook, ISheet sheet)
        {
            var images = new List<ImageInfo>();

            try
            {
                // 获取工作簿中的所有图片数据
                var allPictures = workbook.GetAllPictures();
                Debug.WriteLine($"[图片提取] 工作簿中共有 {allPictures?.Count ?? 0} 张图片");

                if (allPictures == null || allPictures.Count == 0)
                {
                    return images;
                }

                // 根据不同的工作表类型提取图片
                if (sheet is HSSFSheet hssfSheet)
                {
                    // 对于.xls文件
                    Debug.WriteLine($"[图片提取] 检测到.xls格式的工作表");
                    ExtractImagesFromHSSFSheet(hssfSheet, images);
                }
                else if (sheet is XSSFSheet xssfSheet)
                {
                    // 对于.xlsx文件
                    Debug.WriteLine($"[图片提取] 检测到.xlsx格式的工作表");
                    ExtractImagesFromXSSFSheet(xssfSheet, images);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[图片提取错误] 提取图片失败: {ex.Message}");
            }

            return images;
        }

        /// <summary>
        /// 从HSSFSheet（.xls）中提取图片
        /// </summary>
        private void ExtractImagesFromHSSFSheet(HSSFSheet hssfSheet, List<ImageInfo> images)
        {
            var patriarch = hssfSheet.DrawingPatriarch as HSSFPatriarch;
            if (patriarch != null)
            {
                Debug.WriteLine($"[图片提取] 找到DrawingPatriarch，子对象数量: {patriarch.Children?.Count ?? 0}");

                foreach (HSSFShape shape in patriarch.Children)
                {
                    if (shape is HSSFPicture picture && picture.PictureData != null)
                    {
                        var anchor = picture.Anchor as HSSFClientAnchor;
                        var imageInfo = new ImageInfo
                        {
                            RowIndex = anchor?.Row1 ?? 0,
                            ColumnIndex = anchor?.Col1 ?? 0,
                            ImageData = picture.PictureData.Data,
                            ImageFormat = GetImageFormat(picture.PictureData.MimeType)
                        };

                        Debug.WriteLine($"[图片提取] 提取到图片: 行={imageInfo.RowIndex}, 列={imageInfo.ColumnIndex}, 格式={imageInfo.ImageFormat}");
                        images.Add(imageInfo);
                    }
                }
            }
            else
            {
                Debug.WriteLine($"[图片提取] 未找到DrawingPatriarch");
            }
        }

        /// <summary>
        /// 从XSSFSheet（.xlsx）中提取图片
        /// </summary>
        private void ExtractImagesFromXSSFSheet(XSSFSheet xssfSheet, List<ImageInfo> images)
        {
            var drawings = xssfSheet.GetDrawingPatriarch() as XSSFDrawing;
            if (drawings != null)
            {
                Debug.WriteLine($"[图片提取] 找到XSSFDrawing");

                foreach (var shape in drawings.GetShapes())
                {
                    if (shape is XSSFPicture picture && picture.PictureData != null)
                    {
                        // 获取图片的锚点位置信息
                        var anchor = picture.GetAnchor() as XSSFClientAnchor;
                        int rowIndex = anchor?.Row1 ?? 0;
                        int colIndex = anchor?.Col1 ?? 0;

                        var imageInfo = new ImageInfo
                        {
                            RowIndex = rowIndex,
                            ColumnIndex = colIndex,
                            ImageData = picture.PictureData.Data,
                            ImageFormat = GetImageFormat(picture.PictureData.MimeType)
                        };

                        Debug.WriteLine($"[图片提取] 提取到图片: 行={rowIndex}, 列={colIndex}, 格式={imageInfo.ImageFormat}");
                        images.Add(imageInfo);
                    }
                }
            }
            else
            {
                Debug.WriteLine($"[图片提取] 未找到XSSFDrawing");
            }
        }

        /// <summary>
        /// 获取图片格式
        /// </summary>
        private string GetImageFormat(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                return "png";
            }

            switch (mimeType.ToLower())
            {
                case "image/jpeg":
                case "image/jpg":
                    return "jpg";
                case "image/png":
                    return "png";
                case "image/gif":
                    return "gif";
                case "image/bmp":
                    return "bmp";
                default:
                    return "png";
            }
        }

        /// <summary>
        /// 确保目录存在
        /// </summary>
        private void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Debug.WriteLine($"[图片提取] 创建目录: {directory}");
            }
        }

        #endregion
    }
}
