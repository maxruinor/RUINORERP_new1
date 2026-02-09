using System;
using System.Drawing;
using System.Threading.Tasks;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 图片服务接口
    /// 由业务层实现，提供图片的下载、上传、删除等功能
    /// </summary>
    public interface IGridImageService
    {
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片字节数据</returns>
        Task<byte[]> DownloadImageAsync(string imageId);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <param name="fileName">文件名</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>上传结果，包含图片ID</returns>
        Task<string> UploadImageAsync(byte[] imageBytes, string fileName, int businessType = 0);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteImageAsync(string imageId);

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片信息</returns>
        Task<GridImageInfo> GetImageInfoAsync(string imageId);
    }

    /// <summary>
    /// 图片信息
    /// </summary>
    public class GridImageInfo
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 图片宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 图片高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }
    }

    /// <summary>
    /// 默认图片服务实现
    /// 提供基本的图片服务功能
    /// </summary>
    public class DefaultGridImageService : IGridImageService
    {
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片字节数据</returns>
        public async Task<byte[]> DownloadImageAsync(string imageId)
        {
            // 这里实现默认的下载逻辑
            // 例如从本地文件系统或默认位置加载图片
            
            // 示例实现：返回空字节数组
            return await Task.FromResult(new byte[0]);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <param name="fileName">文件名</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>上传结果，包含图片ID</returns>
        public async Task<string> UploadImageAsync(byte[] imageBytes, string fileName, int businessType = 0)
        {
            // 这里实现默认的上传逻辑
            // 例如保存到本地文件系统或默认位置
            
            // 示例实现：返回生成的图片ID
            string imageId = Guid.NewGuid().ToString();
            return await Task.FromResult(imageId);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteImageAsync(string imageId)
        {
            // 这里实现默认的删除逻辑
            // 例如从本地文件系统或默认位置删除图片
            
            // 示例实现：返回删除成功
            return await Task.FromResult(true);
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片信息</returns>
        public async Task<GridImageInfo> GetImageInfoAsync(string imageId)
        {
            // 这里实现默认的获取图片信息逻辑
            // 例如从本地文件系统或默认位置获取图片信息
            
            // 示例实现：返回默认的图片信息
            var info = new GridImageInfo
            {
                ImageId = imageId,
                FileName = "default.jpg",
                FileSize = 0,
                Width = 0,
                Height = 0,
                UploadTime = DateTime.Now,
                BusinessType = 0
            };
            return await Task.FromResult(info);
        }
    }

    /// <summary>
    /// 图片服务管理器
    /// 用于获取和设置图片服务实例
    /// </summary>
    public static class GridImageServiceManager
    {
        private static IGridImageService _imageService;

        /// <summary>
        /// 默认图片服务
        /// </summary>
        public static IGridImageService DefaultService { get; } = new DefaultGridImageService();

        /// <summary>
        /// 当前图片服务
        /// </summary>
        public static IGridImageService CurrentService
        {
            get => _imageService ?? DefaultService;
            set => _imageService = value;
        }

        /// <summary>
        /// 设置图片服务
        /// </summary>
        /// <param name="service">图片服务实例</param>
        public static void SetImageService(IGridImageService service)
        {
            _imageService = service;
        }

        /// <summary>
        /// 获取图片服务
        /// </summary>
        /// <returns>图片服务实例</returns>
        public static IGridImageService GetImageService()
        {
            return CurrentService;
        }
    }
}
