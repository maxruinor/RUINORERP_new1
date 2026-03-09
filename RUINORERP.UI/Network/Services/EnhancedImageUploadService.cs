using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.BusinessImage;
using RUINORERP.Model;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 增强的图片上传服务
    /// 提供重试机制、进度跟踪和完善的错误处理
    /// </summary>
    public class EnhancedImageUploadService
    {
        private readonly IImageService _imageService;
        private readonly ILogger<EnhancedImageUploadService> _logger;
        private readonly SemaphoreSlim _uploadLock = new SemaphoreSlim(1, 1);
        
        /// <summary>
        /// 最大重试次数
        /// </summary>
        public const int MaxRetryCount = 3;
        
        /// <summary>
        /// 重试延迟（毫秒）
        /// </summary>
        public const int RetryDelayMs = 1000;
        
        /// <summary>
        /// 上传超时时间（秒）
        /// </summary>
        public const int UploadTimeoutSeconds = 30;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="imageService">图片服务</param>
        /// <param name="logger">日志记录器</param>
        public EnhancedImageUploadService(IImageService imageService, ILogger<EnhancedImageUploadService> logger)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 带重试机制的图片上传
方法
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="retryCount">当前重试次数</param>
        /// <returns>上传结果</returns>
        public async Task<ImageUploadResult> UploadWithRetryAsync(ImageInfo imageInfo, int retryCount = 0)
        {
            if (imageInfo == null)
            {
                return new ImageUploadResult
                {
                    Success = false,
                    ErrorMessage = "图片信息不能为空",
                    ImageInfo = imageInfo
                };
            }

            try
            {
                _logger?.LogInformation($"开始上传图片: {imageInfo.OriginalFileName}, 重试次数: {retryCount}");

                // 设置超时
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(UploadTimeoutSeconds)))
                {
                    var uploadedImage = await _imageService.UploadImageAsync(imageInfo);
                    
                    if (uploadedImage != null && uploadedImage.FileId > 0)
                    {
                        _logger?.LogInformation($"图片上传成功: {imageInfo.OriginalFileName}, FileId: {uploadedImage.FileId}");
                        return new ImageUploadResult
                        {
                            Success = true,
                            ImageInfo = uploadedImage,
                            RetryCount = retryCount
                        };
                    }
                    else
                    {
                        _logger?.LogWarning($"图片上传返回空结果: {imageInfo.OriginalFileName}");
                        return await HandleUploadFailureAsync(imageInfo, "上传返回空结果", retryCount);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger?.LogError($"图片上传超时: {imageInfo.OriginalFileName}");
                return await HandleUploadFailureAsync(imageInfo, "上传超时", retryCount);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"图片上传异常: {imageInfo.OriginalFileName}");
                return await HandleUploadFailureAsync(imageInfo, ex.Message, retryCount);
            }
        }

        /// <summary>
        /// 处理上传失败
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="retryCount">当前重试次数</param>
        /// <returns>上传结果</returns>
        private async Task<ImageUploadResult> HandleUploadFailureAsync(ImageInfo imageInfo, string errorMessage, int retryCount)
        {
            if (retryCount < MaxRetryCount)
            {
                _logger?.LogWarning($"上传失败，准备重试: {imageInfo.OriginalFileName}, 错误: {errorMessage}, 重试次数: {retryCount + 1}/{MaxRetryCount}");
                
                // 延迟后重试
                await Task.Delay(RetryDelayMs);
                return await UploadWithRetryAsync(imageInfo, retryCount + 1);
            }
            else
            {
                _logger?.LogError($"上传失败，已达到最大重试次数: {imageInfo.OriginalFileName}, 错误: {errorMessage}");
                return new ImageUploadResult
                {
                    Success = false,
                    ErrorMessage = errorMessage,
                    ImageInfo = imageInfo,
                    RetryCount = retryCount,
                    MaxRetriesReached = true
                };
            }
        }

        /// <summary>
        /// 批量上传图片（带重试机制）
        /// </summary>
        /// <param name="imageInfos">图片信息列表</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>批量上传结果</returns>
        public async Task<BatchImageUploadResult> BatchUploadWithRetryAsync(
            List<ImageInfo> imageInfos, 
            Action<UploadProgress> progressCallback = null)
        {
            if (imageInfos == null || imageInfos.Count == 0)
            {
                return new BatchImageUploadResult
                {
                    Success = true,
                    TotalCount = 0,
                    SuccessCount = 0,
                    FailedCount = 0,
                    Results = new List<ImageUploadResult>()
                };
            }

            var results = new List<ImageUploadResult>();
            var successCount = 0;
            var failedCount = 0;
            var totalCount = imageInfos.Count;

            _logger?.LogInformation($"开始批量上传图片，总数: {totalCount}");

            for (int i = 0; i < imageInfos.Count; i++)
            {
                var imageInfo = imageInfos[i];
                
                try
                {
                    var result = await UploadWithRetryAsync(imageInfo);
                    results.Add(result);
                    
                    if (result.Success)
                    {
                        successCount++;
                    }
                    else
                    {
                        failedCount++;
                    }

                    // 报告进度
                    progressCallback?.Invoke(new UploadProgress
                    {
                        CurrentIndex = i + 1,
                        TotalCount = totalCount,
                        SuccessCount = successCount,
                        FailedCount = failedCount,
                        CurrentImage = imageInfo.OriginalFileName,
                        CurrentResult = result
                    });
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"批量上传图片异常: {imageInfo.OriginalFileName}");
                    failedCount++;
                    results.Add(new ImageUploadResult
                    {
                        Success = false,
                        ErrorMessage = ex.Message,
                        ImageInfo = imageInfo,
                        RetryCount = MaxRetryCount,
                        MaxRetriesReached = true
                    });
                }
            }

            var batchResult = new BatchImageUploadResult
            {
                Success = failedCount == 0,
                TotalCount = totalCount,
                SuccessCount = successCount,
                FailedCount = failedCount,
                Results = results
            };

            _logger?.LogInformation($"批量上传完成，总数: {totalCount}, 成功: {successCount}, 失败: {failedCount}");

            return batchResult;
        }

        /// <summary>
        /// 同步图片（增强版，带重试和错误处理）
        /// </summary>
        /// <returns>同步结果</returns>
        public async Task<List<ImageSyncResult>> SyncImagesWithRetryAsync()
        {
            try
            {
                _logger?.LogInformation("开始同步图片（增强版）");
                
                // 获取待上传和待删除的图片
                var pendingUploads = ImageStateManager.Instance.GetPendingUploadImages();
                var pendingDeletes = ImageStateManager.Instance.GetPendingDeleteImages();

                _logger?.LogInformation($"待上传图片: {pendingUploads.Count}, 待删除图片: {pendingDeletes.Count}");

                var syncResults = new List<ImageSyncResult>();

                // 处理待上传图片
                if (pendingUploads.Count > 0)
                {
                    var uploadResult = await BatchUploadWithRetryAsync(pendingUploads);
                    
                    var uploadSyncResult = new ImageSyncResult
                    {
                        SyncType = ImageSyncType.Add,
                        IsSuccess = uploadResult.Success,
                        ImageIds = uploadResult.Results.Where(r => r.Success).Select(r => r.ImageInfo.FileId).ToList()
                    };

                    if (!uploadResult.Success)
                    {
                        uploadSyncResult.ErrorMessage = $"部分上传失败: {uploadResult.FailedCount}/{uploadResult.TotalCount}";
                    }

                    syncResults.Add(uploadSyncResult);
                }

                // 处理待删除图片
                if (pendingDeletes.Count > 0)
                {
                    var deleteResult = await BatchDeleteWithRetryAsync(pendingDeletes);
                    
                    var deleteSyncResult = new ImageSyncResult
                    {
                        SyncType = ImageSyncType.Delete,
                        IsSuccess = deleteResult.Success,
                        ImageIds = deleteResult.Results.Where(r => r.Success).Select(r => r.ImageInfo.FileId).ToList()
                    };

                    if (!deleteResult.Success)
                    {
                        deleteSyncResult.ErrorMessage = $"部分删除失败: {deleteResult.FailedCount}/{deleteResult.TotalCount}";
                    }

                    syncResults.Add(deleteSyncResult);
                }

                _logger?.LogInformation($"图片同步完成，处理了 {syncResults.Count} 个同步操作");
                return syncResults;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "图片同步异常");
                throw;
            }
        }

        /// <summary>
        /// 批量删除图片（带重试机制）
        /// </summary>
        /// <param name="imageInfos">图片信息列表</param>
        /// <returns>批量删除结果</returns>
        private async Task<BatchImageUploadResult> BatchDeleteWithRetryAsync(List<ImageInfo> imageInfos)
        {
            var results = new List<ImageUploadResult>();
            var successCount = 0;
            var failedCount = 0;

            foreach (var imageInfo in imageInfos)
            {
                try
                {
                    var result = await DeleteWithRetryAsync(imageInfo);
                    results.Add(result);
                    
                    if (result.Success)
                    {
                        successCount++;
                    }
                    else
                    {
                        failedCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"删除图片异常: {imageInfo.FileId}");
                    failedCount++;
                    results.Add(new ImageUploadResult
                    {
                        Success = false,
                        ErrorMessage = ex.Message,
                        ImageInfo = imageInfo,
                        RetryCount = MaxRetryCount,
                        MaxRetriesReached = true
                    });
                }
            }

            return new BatchImageUploadResult
            {
                Success = failedCount == 0,
                TotalCount = imageInfos.Count,
                SuccessCount = successCount,
                FailedCount = failedCount,
                Results = results
            };
        }

        /// <summary>
        /// 带重试机制的图片删除
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="retryCount">当前重试次数</param>
        /// <returns>删除结果</returns>
        private async Task<ImageUploadResult> DeleteWithRetryAsync(ImageInfo imageInfo, int retryCount = 0)
        {
            try
            {
                _logger?.LogInformation($"开始删除图片: {imageInfo.FileId}, 重试次数: {retryCount}");

                var deleted = await _imageService.DeleteImageAsync(imageInfo.FileId, imageInfo.BusinessId);
                
                if (deleted)
                {
                    _logger?.LogInformation($"图片删除成功: {imageInfo.FileId}");
                    return new ImageUploadResult
                    {
                        Success = true,
                        ImageInfo = imageInfo,
                        RetryCount = retryCount
                    };
                }
                else
                {
                    _logger?.LogWarning($"图片删除失败: {imageInfo.FileId}");
                    return await HandleDeleteFailureAsync(imageInfo, "删除失败", retryCount);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"删除图片异常: {imageInfo.FileId}");
                return await HandleDeleteFailureAsync(imageInfo, ex.Message, retryCount);
            }
        }

        /// <summary>
        /// 处理删除失败
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="retryCount">当前重试次数</param>
        /// <returns>删除结果</returns>
        private async Task<ImageUploadResult> HandleDeleteFailureAsync(ImageInfo imageInfo, string errorMessage, int retryCount)
        {
            if (retryCount < MaxRetryCount)
            {
                _logger?.LogWarning($"删除失败，准备重试: {imageInfo.FileId}, 错误: {errorMessage}, 重试次数: {retryCount + 1}/{MaxRetryCount}");
                
                await Task.Delay(RetryDelayMs);
                return await DeleteWithRetryAsync(imageInfo, retryCount + 1);
            }
            else
            {
                _logger?.LogError($"删除失败，已达到最大重试次数: {imageInfo.FileId}, 错误: {errorMessage}");
                return new ImageUploadResult
                {
                    Success = false,
                    ErrorMessage = errorMessage,
                    ImageInfo = imageInfo,
                    RetryCount = retryCount,
                    MaxRetriesReached = true
                };
            }
        }

        /// <summary>
        /// 获取上传统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public ImageUploadStatistics GetStatistics()
        {
            var stateManager = ImageStateManager.Instance;
            return new ImageUploadStatistics
            {
                PendingUploadCount = stateManager.PendingUploadCount,
                PendingDeleteCount = stateManager.PendingDeleteCount,
                ProcessingCount = stateManager.ProcessingCount,
                TotalImageCount = stateManager.TotalImageCount
            };
        }
    }

    /// <summary>
    /// 图片上传结果
    /// </summary>
    public class ImageUploadResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 图片信息
        /// </summary>
        public ImageInfo ImageInfo { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 是否达到最大重试次数
        /// </summary>
        public bool MaxRetriesReached { get; set; }
    }

    /// <summary>
    /// 批量图片上传结果
    /// </summary>
    public class BatchImageUploadResult
    {
        /// <summary>
        /// 是否全部成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 上传结果列表
        /// </summary>
        public List<ImageUploadResult> Results { get; set; }
    }

    /// <summary>
    /// 上传进度信息
    /// </summary>
    public class UploadProgress
    {
        /// <summary>
        /// 当前索引
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败数量
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 当前图片名称
        /// </summary>
        public string CurrentImage { get; set; }

        /// <summary>
        /// 当前上传结果
        /// </summary>
        public ImageUploadResult CurrentResult { get; set; }
    }

    /// <summary>
    /// 图片上传统计信息
    /// </summary>
    public class ImageUploadStatistics
    {
        /// <summary>
        /// 待上传数量
        /// </summary>
        public int PendingUploadCount { get; set; }

        /// <summary>
        /// 待删除数量
        /// </summary>
        public int PendingDeleteCount { get; set; }

        /// <summary>
        /// 处理中数量
        /// </summary>
        public int ProcessingCount { get; set; }

        /// <summary>
        /// 总图片数量
        /// </summary>
        public int TotalImageCount { get; set; }
    }
}