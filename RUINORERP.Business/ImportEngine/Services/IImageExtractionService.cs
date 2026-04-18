using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Business.ImportEngine.Services
{
    /// <summary>
    /// 图片信息模型
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 行索引（从0开始）
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 列索引（从0开始）
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 图片数据（字节数组）
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 图片格式（jpg, png, gif等）
        /// </summary>
        public string ImageFormat { get; set; }

        /// <summary>
        /// 保存后的文件路径
        /// </summary>
        public string SavedPath { get; set; }
    }

    /// <summary>
    /// 图片提取服务接口
    /// 负责从Excel文件中提取嵌入的图片
    /// </summary>
    public interface IImageExtractionService
    {
        /// <summary>
        /// 从Excel文件中提取所有图片
        /// 支持.xls和.xlsx格式，支持DISPIMG公式识别
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetIndex">工作表索引（从0开始）</param>
        /// <returns>图片信息列表</returns>
        Task<List<ImageInfo>> ExtractImagesAsync(string excelFilePath, int sheetIndex = 0);

        /// <summary>
        /// 保存图片到指定目录
        /// </summary>
        /// <param name="imageData">图片数据</param>
        /// <param name="fileName">文件名（不含扩展名）</param>
        /// <param name="format">图片格式</param>
        /// <returns>保存后的完整路径</returns>
        Task<string> SaveImageAsync(byte[] imageData, string fileName, string format = "png");

        /// <summary>
        /// 设置图片保存目录
        /// </summary>
        /// <param name="outputDirectory">输出目录路径</param>
        void SetOutputDirectory(string outputDirectory);
    }
}
