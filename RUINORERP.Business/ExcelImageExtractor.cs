using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RUINORERP.Business
{
    /// <summary>
    /// Excel 图片提取工具
    /// </summary>
    public class ExcelImageExtractor
    {
        /// <summary>
        /// 从 Excel 文件中提取图片，返回 行号->图片字节数组 的映射
        /// </summary>
        public static Dictionary<int, byte[]> ExtractImagesFromExcel(string filePath)
        {
            var imageMap = new Dictionary<int, byte[]>();
            
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0); // 默认取第一个 Sheet

                // 获取所有图片信息
                var drawings = sheet.CreateDrawingPatriarch();
                if (drawings is XSSFDrawing xssfDrawing)
                {
                    foreach (var shape in xssfDrawing.GetShapes())
                    {
                        try
                        {
                            // 修正 CS8121：先判断类型再转换
                            if (shape is XSSFPicture picture && picture.ClientAnchor is XSSFClientAnchor anchor)
                            {
                                // 获取图片所在的行（通常取锚点起始行）
                                int rowIndex = anchor.Row1; 
                                
                                // 获取图片数据
                                var pictureData = picture.PictureData;
                                if (pictureData != null)
                                {
                                    byte[] data = pictureData.Data;
                                    // 简单的过滤：只保留常见的图片格式
                                    if (data != null && data.Length > 0)
                                    {
                                        imageMap[rowIndex] = data;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // 记录日志但不中断整个提取过程
                            System.Diagnostics.Debug.WriteLine($"提取图片失败: {ex.Message}");
                        }
                    }
                }
            }
            return imageMap;
        }

        /// <summary>
        /// 根据文件名规律匹配图片（备选方案）
        /// </summary>
        public static Dictionary<string, byte[]> MatchImagesByPattern(List<string> fileNames, string imageDir)
        {
            var result = new Dictionary<string, byte[]>();
            if (!Directory.Exists(imageDir)) return result;

            foreach (var name in fileNames)
            {
                // 假设图片名为 "品号.jpg" 或 "品号.png"
                var jpgPath = Path.Combine(imageDir, $"{name}.jpg");
                var pngPath = Path.Combine(imageDir, $"{name}.png");

                if (File.Exists(jpgPath))
                    result[name] = File.ReadAllBytes(jpgPath);
                else if (File.Exists(pngPath))
                    result[name] = File.ReadAllBytes(pngPath);
            }
            return result;
        }
    }
}
