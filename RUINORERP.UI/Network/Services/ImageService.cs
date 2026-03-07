using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.BusinessImage;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.FileManagement;
using System.Linq;
using System;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 图片服务实现
    /// </summary>
    public class ImageService : IImageService
    {
        private readonly FileManagementService _fileManagementService;
        private readonly ImageStateManager _stateManager;
        private readonly ImageCache _cache;
        private readonly ILogger<ImageService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileManagementService">文件管理服务</param>
        /// <param name="cache">图片缓存</param>
        /// <param name="logger">日志记录器</param>
        public ImageService(
            FileManagementService fileManagementService,
            ImageCache cache,
            ILogger<ImageService> logger)
        {
            _fileManagementService = fileManagementService;
            _stateManager = ImageStateManager.Instance; // 使用静态单例
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>上传后的图片信息</returns>
        public async Task<ImageInfo> UploadImageAsync(ImageInfo imageInfo)
        {
            try
            {
                _logger.LogInformation($"开始上传图片: {imageInfo.OriginalFileName}");
                
                // 创建文件存储信息
                var fileStorageInfo = new tb_FS_FileStorageInfo
                {
                    OriginalFileName = imageInfo.OriginalFileName,
                    FileData = imageInfo.ImageData,
                    FileExtension = imageInfo.FileExtension,
                    FileSize = imageInfo.FileSize,
                    FileType = "image",
                    OwnerTableName = imageInfo.OwnerTableName
                };
                
                // 创建上传请求
                var request = new FileUploadRequest();
                request.FileStorageInfos.Add(fileStorageInfo);
                request.OwnerTableName = imageInfo.OwnerTableName;
                request.BusinessId = imageInfo.BusinessId;
                request.RelatedField = imageInfo.RelatedField;
                
                // 调用文件管理服务上传图片
                var response = await _fileManagementService.UploadFileAsync(request);
                
                if (response != null && response.IsSuccess && response.FileStorageInfos != null && response.FileStorageInfos.Count > 0)
                {
                    var uploadedFile = response.FileStorageInfos[0];
                    imageInfo.FileId = uploadedFile.FileId;
                    imageInfo.Status = ImageStatus.Uploaded;
                    _stateManager.AddImage(imageInfo);
                    
                    // 缓存图片
                    if (imageInfo.ImageData != null && imageInfo.ImageData.Length > 0)
                    {
                        using (var ms = new MemoryStream(imageInfo.ImageData))
                        {
                            var image = Image.FromStream(ms);
                            _cache.AddImage(imageInfo.FileId, image);
                        }
                    }
                    
                    _logger.LogInformation($"图片上传成功: {imageInfo.OriginalFileName}, ID: {imageInfo.FileId}");
                    return imageInfo;
                }
                
                _logger.LogError($"图片上传失败: {imageInfo.OriginalFileName}");
                return null;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "上传图片时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <returns>图片信息</returns>
        public async Task<ImageInfo> DownloadImageAsync(long fileId)
        {
            try
            {
                _logger.LogInformation($"开始下载图片: {fileId}");
                
                // 先从缓存获取
                var cachedImage = _cache.GetImage(fileId);
                if (cachedImage != null)
                {
                    _logger.LogInformation($"从缓存获取图片: {fileId}");
                    var imageInfo = _stateManager.GetImageInfo(fileId);
                    if (imageInfo != null)
                    {
                        return imageInfo;
                    }
                }
                
                // 创建下载请求
                var request = new FileDownloadRequest
                {
                    FileStorageInfo = new tb_FS_FileStorageInfo { FileId = fileId }
                };
                
                // 调用文件管理服务下载图片
                var response = await _fileManagementService.DownloadFileAsync(request);
                if (response != null && response.IsSuccess && response.FileStorageInfos != null && response.FileStorageInfos.Count > 0)
                {
                    var downloadedFile = response.FileStorageInfos[0];
                    var imageInfo = new ImageInfo
                    {
                        FileId = fileId,
                        ImageData = downloadedFile.FileData,
                        OriginalFileName = downloadedFile.OriginalFileName,
                        FileExtension = downloadedFile.FileExtension,
                        FileSize = downloadedFile.FileSize,
                        FileType = downloadedFile.FileType,
                        Status = ImageStatus.Uploaded
                    };
                    
                    // 缓存图片
                    if (imageInfo.ImageData != null && imageInfo.ImageData.Length > 0)
                    {
                        using (var ms = new MemoryStream(imageInfo.ImageData))
                        {
                            var image = Image.FromStream(ms);
                            _cache.AddImage(fileId, image);
                        }
                    }
                    
                    _stateManager.AddImage(imageInfo);
                    _logger.LogInformation($"图片下载成功: {fileId}");
                    return imageInfo;
                }
                
                _logger.LogError($"图片下载失败: {fileId}");
                return null;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "下载图片时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 批量下载图片
        /// </summary>
        /// <param name="fileIds">图片ID列表</param>
        /// <returns>图片信息列表</returns>
        public async Task<List<ImageInfo>> DownloadImagesAsync(List<long> fileIds)
        {
            var result = new List<ImageInfo>();
            
            foreach (var fileId in fileIds)
            {
                var imageInfo = await DownloadImageAsync(fileId);
                if (imageInfo != null)
                {
                    result.Add(imageInfo);
                }
            }
            
            return result;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <param name="businessId">业务ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteImageAsync(long fileId, long businessId)
        {
            try
            {
                _logger.LogInformation($"开始删除图片: {fileId}");
                
                // 创建文件存储信息
                var fileStorageInfo = new tb_FS_FileStorageInfo
                {
                    FileId = fileId
                };
                
                // 创建删除请求
                var request = new FileDeleteRequest();
                request.BusinessId = businessId;
                request.AddDeleteFileStorageInfo(fileStorageInfo);
                
                // 调用文件管理服务删除图片
                var response = await _fileManagementService.DeleteFileAsync(request);
                
                if (response != null && response.IsSuccess)
                {
                    // 更新状态
                    _stateManager.UpdateImageStatus(fileId, ImageStatus.Deleted);
                    // 从缓存移除
                    _cache.RemoveImage(fileId);
                    _logger.LogInformation($"图片删除成功: {fileId}");
                    return true;
                }
                else
                {
                    _logger.LogError($"图片删除失败: {fileId}");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "删除图片时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <param name="fileIds">图片ID列表</param>
        /// <param name="businessId">业务ID</param>
        /// <returns>删除结果</returns>
        public async Task<Dictionary<long, bool>> DeleteImagesAsync(List<long> fileIds, long businessId)
        {
            var result = new Dictionary<long, bool>();
            
            foreach (var fileId in fileIds)
            {
                var success = await DeleteImageAsync(fileId, businessId);
                result[fileId] = success;
            }
            
            return result;
        }

        /// <summary>
        /// 获取图片状态
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <returns>图片状态</returns>
        public ImageStatus GetImageStatus(long fileId)
        {
            return _stateManager.GetImageStatus(fileId);
        }

        /// <summary>
        /// 同步图片 - 返回详细的同步结果
        /// </summary>
        /// <returns>图片同步结果列表</returns>
        public async Task<List<ImageSyncResult>> SyncImagesAsync()
        {
            var syncResults = new List<ImageSyncResult>();
            
            try
            {
                _logger.LogInformation("开始同步图片");
                
                // 同步待上传图片
                var pendingUploads = _stateManager.GetPendingUploadImages();
                var uploadGroups = pendingUploads.GroupBy(img => img.BusinessId);
                
                foreach (var group in uploadGroups)
                {
                    var uploadedImageIds = new List<long>();
                    var result = new ImageSyncResult
                    {
                        BusinessId = group.Key,
                        SyncType = ImageSyncType.Add
                    };
                    
                    foreach (var imageInfo in group)
                    {
                        try
                        {
                            var uploaded = await UploadImageAsync(imageInfo);
                            if (uploaded != null)
                            {
                                uploadedImageIds.Add(uploaded.FileId);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"上传图片失败: {imageInfo.OriginalFileName}");
                            result.IsSuccess = false;
                            result.ErrorMessage += $"上传{imageInfo.OriginalFileName}失败; ";
                        }
                    }
                    
                    result.ImageIds = uploadedImageIds;
                    if (uploadedImageIds.Count > 0)
                    {
                        syncResults.Add(result);
                    }
                }
                
                // 同步待删除图片
                var pendingDeletes = _stateManager.GetPendingDeleteImages();
                var deleteGroups = pendingDeletes.GroupBy(img => img.BusinessId);
                
                foreach (var group in deleteGroups)
                {
                    var deletedImageIds = new List<long>();
                    var result = new ImageSyncResult
                    {
                        BusinessId = group.Key,
                        SyncType = ImageSyncType.Delete
                    };
                    
                    foreach (var imageInfo in group)
                    {
                        try
                        {
                            var deleted = await DeleteImageAsync(imageInfo.FileId, imageInfo.BusinessId);
                            if (deleted)
                            {
                                deletedImageIds.Add(imageInfo.FileId);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"删除图片失败: {imageInfo.FileId}");
                            result.IsSuccess = false;
                            result.ErrorMessage += $"删除图片{imageInfo.FileId}失败; ";
                        }
                    }
                    
                    result.ImageIds = deletedImageIds;
                    if (deletedImageIds.Count > 0)
                    {
                        syncResults.Add(result);
                    }
                }
                
                _logger.LogInformation($"图片同步完成，处理了 {syncResults.Count} 个同步操作");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "同步图片时发生错误");
                throw;
            }
            
            return syncResults;
        }

        // 移除所有过度设计的复杂方法
        // 保持简洁清晰的设计，专注核心功能

        /// <summary>
        /// 清理图片缓存
        /// </summary>
        public void ClearCache()
        {
            _cache.ClearCache();
            _logger.LogInformation("图片缓存已清理");
        }
    }
}