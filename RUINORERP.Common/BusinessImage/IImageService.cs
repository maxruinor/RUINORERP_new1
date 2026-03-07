using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Common.BusinessImage
{
    /// <summary>
    /// 图片服务接口
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>上传后的图片信息</returns>
        Task<ImageInfo> UploadImageAsync(ImageInfo imageInfo);
        
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <returns>图片信息</returns>
        Task<ImageInfo> DownloadImageAsync(long fileId);
        
        /// <summary>
        /// 批量下载图片
        /// </summary>
        /// <param name="fileIds">图片ID列表</param>
        /// <returns>图片信息列表</returns>
        Task<List<ImageInfo>> DownloadImagesAsync(List<long> fileIds);
        
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <param name="businessId">业务ID</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteImageAsync(long fileId, long businessId);
        
        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <param name="fileIds">图片ID列表</param>
        /// <param name="businessId">业务ID</param>
        /// <returns>删除结果</returns>
        Task<Dictionary<long, bool>> DeleteImagesAsync(List<long> fileIds, long businessId);
        
        /// <summary>
        /// 获取图片状态
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <returns>图片状态</returns>
        ImageStatus GetImageStatus(long fileId);
        
        /// <summary>
        /// 同步图片
        /// </summary>
        /// <returns>同步结果</returns>
        Task<List<ImageSyncResult>> SyncImagesAsync();
        
        /// <summary>
        /// 清理图片缓存
        /// </summary>
        void ClearCache();
    }
}