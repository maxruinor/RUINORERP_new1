using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Context;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RUINORERP.Model.BusinessImage;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 文件业务服务类（重构版 - 统一的文件管理入口）
    /// 职责：
    /// 1. 文件上传/下载/删除（直接调用FileManagementService）
    /// 2. 业务实体与文件的关联管理
    /// 3. 数据模型转换（tb_FS_FileStorageInfo ↔ ImageInfo）
    ///
    /// 设计原则：
    /// - 单一职责：专注文件与业务的关联
    /// - 简洁直接：不依赖其他服务，直接调用FileManagementService
    /// - 消除嵌套：三层架构（UI → FileBusinessService → FileManagementService）
    /// </summary>
    public class FileBusinessService
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<FileBusinessService> _logger;
        private readonly ApplicationContext _appContext;
        private readonly IEntityMappingService _mapper;
        
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        private const int MaxImageWidth = 1920;
        
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        private const int MaxImageHeight = 1080;
        
        /// <summary>
        /// JPEG压缩质量(0-100),85是质量和大小的良好平衡
        /// </summary>
        private const long JpegQuality = 85;
        
        /// <summary>
        /// 需要压缩的最小文件大小(字节),小于此值不压缩(约500KB)
        /// </summary>
        private const long MinCompressSize = 500 * 1024;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapper">实体映射服务</param>
        /// <param name="unitOfWorkManage">工作单元管理</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="appContext">应用上下文</param>
        public FileBusinessService(
            IEntityMappingService mapper,
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<FileBusinessService> logger,
            ApplicationContext appContext = null)
        {
            _mapper = mapper;
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
            _appContext = appContext;
        }


        /// <summary>
        /// 上传文件（图片或其他类型）
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileData">文件数据</param>
        /// <param name="relatedField">关联字段名(如VoucherImage、PaymentImagePath等)</param>
        /// <param name="fileId">文件ID（可选），用于更新现有文件</param>
        /// <returns>上传结果，包含文件ID</returns>
        public async Task<FileUploadResponse> UploadImageAsync(BaseEntity entity, string fileName, byte[] fileData, string relatedField, long? fileId = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("文件名不能为空", nameof(fileName));
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("文件数据不能为空", nameof(fileData));

            try
            {
                // ✅ 图片压缩并获取宽高信息
                var compressResult = CompressImageIfNeeded(fileData, fileName);
                
                if (compressResult.CompressedData != null && compressResult.CompressedData.Length < fileData.Length)
                {
                    _logger?.LogInformation("[FileBusinessService] 图片已压缩: {OriginalSize}KB → {CompressedSize}KB, 压缩率{Ratio}%, 尺寸: {Width}x{Height}",
                        fileData.Length / 1024,
                        compressResult.CompressedData.Length / 1024,
                        (1 - (double)compressResult.CompressedData.Length / fileData.Length) * 100,
                        compressResult.Width,
                        compressResult.Height);
                    
                    fileData = compressResult.CompressedData;
                }
                else if (compressResult.Width > 0 && compressResult.Height > 0)
                {
                    // 未压缩,但仍然获取了宽高信息
                    _logger?.LogDebug("[FileBusinessService] 图片无需压缩, 尺寸: {Width}x{Height}, 大小: {Size}KB",
                        compressResult.Width,
                        compressResult.Height,
                        fileData.Length / 1024);
                }
                
                var fileService = _appContext.GetRequiredService<FileManagementService>();

                var storageInfo = new tb_FS_FileStorageInfo
                {
                    FileId = fileId ?? 0,
                    OriginalFileName = fileName,
                    FileData = fileData,
                    OwnerTableName = entity.GetType().Name
                };

                var uploadRequest = new FileUploadRequest
                {
                    FileStorageInfos = { storageInfo },
                    OwnerTableName = entity.GetType().Name,
                    BusinessId = entity.PrimaryKeyID,
                    BusinessNo = string.Empty,
                    RelatedField = relatedField
                };

                // 关键修复: 对于tb_ProdDetail实体,强制使用ProdDetailID作为BusinessId
                if (entity is tb_ProdDetail prodDetail)
                {
                    uploadRequest.BusinessId = prodDetail.ProdDetailID;
                    _logger?.LogInformation("[FileBusinessService] 检测到tb_ProdDetail实体,使用ProdDetailID={ProdDetailID}作为BusinessId", prodDetail.ProdDetailID);
                }

                // 验证BusinessId是否有效
                if (uploadRequest.BusinessId <= 0)
                {
                    _logger?.LogError("[FileBusinessService] BusinessId无效: {BusinessId}, 请确保实体已保存到数据库", uploadRequest.BusinessId);
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("业务ID无效,请先保存实体数据");
                }

                _logger?.LogInformation("[FileBusinessService] 开始上传文件: OwnerTableName={OwnerTableName}, BusinessId={BusinessId}, RelatedField={RelatedField}, FileName={FileName}, EntityPrimaryKeyID={PrimaryKeyID}",
                    uploadRequest.OwnerTableName, uploadRequest.BusinessId, uploadRequest.RelatedField, fileName, entity.PrimaryKeyID);

                var response = await fileService.UploadFileAsync(uploadRequest);

                // 记录响应结果
                if (response != null)
                {
                    _logger?.LogInformation("[FileBusinessService] 文件上传响应: IsSuccess={IsSuccess}, Message={Message}, FileCount={FileCount}",
                        response.IsSuccess, response.Message, response.FileStorageInfos?.Count ?? 0);
                    
                    if (response.FileStorageInfos != null)
                    {
                        foreach (var fsi in response.FileStorageInfos)
                        {
                            _logger?.LogInformation("[FileBusinessService] 上传后的文件信息: FileId={FileId}, OriginalFileName={OriginalFileName}",
                                fsi.FileId, fsi.OriginalFileName);
                        }
                    }
                }
                else
                {
                    _logger?.LogError("[FileBusinessService] 文件上传响应为空");
                }

                if (response == null)
                {
                    _logger?.LogError("上传文件失败：服务器返回了空的响应数据");
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("服务器返回了空的响应数据");
                }

                if (!response.IsSuccess)
                {
                    _logger?.LogError("上传文件失败: {Error}", response.ErrorMessage);
                    return response;
                }

                if (response.FileStorageInfos == null || response.FileStorageInfos.Count == 0)
                {
                    _logger?.LogError("上传文件失败：服务器返回的FileStorageInfos为空");
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("服务器返回的响应数据不完整");
                }

                // ✅ 关键优化: 上传成功后,将文件信息添加到ImageCacheService缓存
                try
                {
                    var imageCacheService = _appContext.GetRequiredService<RUINORERP.UI.Network.Services.ImageCacheService>();
                    if (imageCacheService != null && response.FileStorageInfos != null)
                    {
                        imageCacheService.AddImageInfos(response.FileStorageInfos);
                        _logger?.LogInformation("已将{Count}个上传的文件信息添加到ImageCacheService缓存", response.FileStorageInfos.Count);
                    }
                }
                catch (Exception cacheEx)
                {
                    _logger?.LogWarning(cacheEx, "添加文件信息到缓存失败,不影响上传结果");
                }

                // 上传成功后，将文件ID更新到业务实体的关联字段中
                if (!string.IsNullOrEmpty(relatedField) && entity != null)
                {
                    try
                    {
                        var propertyInfo = entity.GetType().GetProperty(relatedField);
                        if (propertyInfo != null)
                        {
                            // 获取所有上传成功的文件ID
                            List<long> fileIds = response.FileStorageInfos
                                .Where(f => f != null && f.FileId > 0)
                                .Select(f => f.FileId)
                                .ToList();

                            if (fileIds.Count > 0)
                            {
                                // 根据字段类型设置值
                                if (propertyInfo.PropertyType == typeof(string))
                                {
                                    // 字符串类型，多个文件ID用逗号分隔
                                    string fileIdsStr = string.Join(",", fileIds);
                                    propertyInfo.SetValue(entity, fileIdsStr);
                                    _logger?.LogInformation("已将文件ID更新到实体字段: {FieldName}={FileIds}", relatedField, fileIdsStr);
                                }
                                else if (propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?))
                                {
                                    // long类型，只设置第一个文件ID
                                    propertyInfo.SetValue(entity, fileIds[0]);
                                    _logger?.LogInformation("已将文件ID更新到实体字段: {FieldName}={FileId}", relatedField, fileIds[0]);
                                }
                            }
                            else
                            {
                                _logger?.LogWarning("[FileBusinessService] 上传成功但没有返回有效的FileId，fileIds列表为空");
                            }
                        }
                        else
                        {
                            _logger?.LogWarning("[FileBusinessService] 实体中不存在属性: {PropertyName}", relatedField);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "更新实体关联字段失败: FieldName={FieldName}", relatedField);
                    }
                }
                else
                {
                    _logger?.LogWarning("[FileBusinessService] 未更新实体字段: relatedField={RelatedField}, entity={Entity}",
                        relatedField, entity?.GetType().Name ?? "null");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "上传文件失败: FileName={FileName}", fileName);
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>($"上传文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 下载文件
        /// 通过反射获取实体关联字段的文件ID，然后下载文件
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="relatedField">关联字段名</param>
        /// <returns>文件下载响应</returns>
        public async Task<FileDownloadResponse> DownloadImageAsync(BaseEntity entity, string relatedField)
        {
            var fileService = _appContext.GetRequiredService<FileManagementService>();

            try
            {
                if (string.IsNullOrEmpty(relatedField) || entity == null)
                {
                    return FileDownloadResponse.CreateFailure("未指定关联字段或实体，无法下载文件");
                }

                var propertyInfo = entity.GetType().GetProperty(relatedField);
                if (propertyInfo == null)
                {
                    return FileDownloadResponse.CreateFailure($"实体中不存在关联字段 {relatedField}");
                }

                var fieldValue = propertyInfo.GetValue(entity);
                if (fieldValue == null)
                {
                    return FileDownloadResponse.CreateFailure($"关联字段 {relatedField} 的值为 null");
                }

                long fileId = GetFileIdFromValue(fieldValue);
                if (fileId <= 0)
                {
                    return FileDownloadResponse.CreateFailure($"关联字段 {relatedField} 的值不是有效的文件ID");
                }

                var request = new FileDownloadRequest
                {
                    FileStorageInfo = new tb_FS_FileStorageInfo { FileId = fileId }
                };

                var response = await fileService.DownloadFileAsync(request);

                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("下载文件成功: FileId={FileId}", fileId);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "下载文件失败: FieldName={FieldName}", relatedField);
                return FileDownloadResponse.CreateFailure($"下载文件异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 下载文件（泛型版本，使用表达式）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">业务实体</param>
        /// <param name="exp">字段表达式</param>
        /// <returns>文件下载响应</returns>
        public async Task<FileDownloadResponse> DownloadImageAsync<T>(BaseEntity entity, Expression<Func<T, object>> exp)
        {
            string relatedField = exp.GetMemberInfo().Name;
            return await DownloadImageAsync(entity, relatedField);
        }

        /// <summary>
        /// 从值中提取文件ID
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>文件ID</returns>
        private long GetFileIdFromValue(object value)
        {
            if (value is long longValue && longValue > 0)
                return longValue;
            if (value is int intValue && intValue > 0)
                return intValue;
            if (long.TryParse(value?.ToString(), out long parsedValue) && parsedValue > 0)
                return parsedValue;
            return 0;
        }

        /// <summary>
        /// 将tb_FS_FileStorageInfo实体转换为ImageInfo类
        /// </summary>
        /// <param name="fileStorageInfo">文件存储信息实体</param>
        /// <returns>转换后的ImageInfo对象</returns>
        public ImageInfo ConvertToImageInfo(tb_FS_FileStorageInfo fileStorageInfo)
        {
            if (fileStorageInfo == null)
                return null;

            return new ImageInfo
            {
                FileId = fileStorageInfo.FileId,
                OriginalFileName = fileStorageInfo.OriginalFileName,
                FileSize = fileStorageInfo.FileSize,
                FileType = fileStorageInfo.FileType,
                FileExtension = fileStorageInfo.FileExtension,
                HashValue = fileStorageInfo.HashValue,
                CreateTime = fileStorageInfo.Created_at ?? DateTime.Now,
                ModifiedAt = fileStorageInfo.Modified_at ?? DateTime.Now,
                Metadata = new Dictionary<string, string>
                {
                    ["StorageProvider"] = fileStorageInfo.StorageProvider ?? "Local",
                    ["StoragePath"] = fileStorageInfo.StoragePath ?? string.Empty,
                    ["StorageFileName"] = fileStorageInfo.StorageFileName ?? string.Empty
                }
            };
        }

        /// <summary>
        /// 将ImageInfo对象转换为tb_FS_FileStorageInfo实体
        /// </summary>
        /// <param name="imageInfo">图片信息对象</param>
        /// <returns>转换后的tb_FS_FileStorageInfo实体</returns>
        public tb_FS_FileStorageInfo ConvertToFileStorageInfo(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                return null;

            return new tb_FS_FileStorageInfo
            {
                FileId = imageInfo.FileId,
                OriginalFileName = imageInfo.OriginalFileName,
                FileSize = imageInfo.FileSize,
                FileType = imageInfo.FileType,
                FileExtension = imageInfo.FileExtension,
                HashValue = imageInfo.HashValue,
                Created_at = imageInfo.CreateTime,
                Modified_at = imageInfo.ModifiedAt,
                StorageProvider = "Local",
                StoragePath = string.Empty,
                StorageFileName = $"{imageInfo.FileId}_{DateTime.Now:yyyyMMddHHmmssfff}{imageInfo.FileExtension}",
                CurrentVersion = 1,
                FileStatus = (int)FileStatus.Active,
                ExpireTime = DateTime.MaxValue,
                Description = string.Empty,
                Metadata = string.Empty,
                isdeleted = false
            };
        }




        /// <summary>
        /// 删除与业务实体关联的所有文件
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="physicalDelete">是否物理删除文件（默认false:逻辑删除）</param>
        /// <returns>删除结果</returns>
        public async Task<FileDeleteResponse> DeleteImagesAsync(BaseEntity entity, bool physicalDelete = false)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            long businessId = entity.PrimaryKeyID;
            string ownerTableName = entity.GetType().Name;

            // 验证业务ID
            if (businessId <= 0)
            {
                _logger?.LogInformation("业务ID无效: {BusinessId}, 删除操作可能无法正确执行", businessId);
            }

            // 收集要删除的文件ID
            var fileIds = new List<long>();
            if (entity.FileStorageInfoList != null)
            {
                fileIds = entity.FileStorageInfoList
                    .Where(f => f != null && f.FileId > 0)
                    .Select(f => f.FileId)
                    .ToList();
            }

            if (fileIds.Count == 0)
            {
                return ResponseFactory.CreateSpecificErrorResponse<FileDeleteResponse>("没有文件需要删除");
            }

            return await DeleteImagesByIdsAsync(businessId, ownerTableName, fileIds, physicalDelete);
        }

        /// <summary>
        /// 删除指定的文件列表（推荐方法）
        /// </summary>
        /// <param name="businessId">业务ID（必填）</param>
        /// <param name="ownerTableName">业务表名（必填）</param>
        /// <param name="fileIds">要删除的文件ID列表（必填）</param>
        /// <param name="physicalDelete">是否物理删除文件（默认false）</param>
        /// <returns>删除结果</returns>
        public async Task<FileDeleteResponse> DeleteImagesByIdsAsync(
            long businessId,
            string ownerTableName,
            List<long> fileIds,
            bool physicalDelete = false)
        {
            if (businessId <= 0)
                return ResponseFactory.CreateSpecificErrorResponse<FileDeleteResponse>("业务ID无效");

            if (string.IsNullOrEmpty(ownerTableName))
                return ResponseFactory.CreateSpecificErrorResponse<FileDeleteResponse>("业务表名为空");

            if (fileIds == null || fileIds.Count == 0)
                return ResponseFactory.CreateSpecificErrorResponse<FileDeleteResponse>("文件ID列表为空");

            try
            {
                _logger?.LogInformation("开始删除文件: BusinessId={BusinessId}, OwnerTableName={OwnerTableName}, 数量={Count}",
                    businessId, ownerTableName, fileIds.Count);

                var fileService = _appContext.GetRequiredService<FileManagementService>();
                var deleteRequest = new FileDeleteRequest
                {
                    BusinessId = businessId,
                    PhysicalDelete = physicalDelete
                };

                foreach (var fileId in fileIds.Where(id => id > 0))
                {
                    deleteRequest.AddDeleteFileStorageInfo(new tb_FS_FileStorageInfo
                    {
                        FileId = fileId,
                        OwnerTableName = ownerTableName,
                        StorageProvider = "Local",
                        FileStatus = (int)FileStatus.Active
                    });
                }

                if (deleteRequest.FileStorageInfos.Count == 0)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<FileDeleteResponse>("没有有效的文件ID");
                }

                var response = await fileService.DeleteFileAsync(deleteRequest);

                if (response != null && response.IsSuccess)
                {
                    _logger?.LogInformation("删除文件成功: BusinessId={BusinessId}, 删除数量={Count}",
                        businessId, response.DeletedFileIds?.Count ?? 0);
                    
                    // ✅ 关键优化: 删除成功后,从ImageCacheService缓存中移除
                    try
                    {
                        var imageCacheService = _appContext.GetRequiredService<RUINORERP.UI.Network.Services.ImageCacheService>();
                        if (imageCacheService != null && response.DeletedFileIds != null && response.DeletedFileIds.Count > 0)
                        {
                            // 将List<string>转换为IEnumerable<long>
                            var deletedFileIds = response.DeletedFileIds
                                .Where(id => long.TryParse(id, out _))
                                .Select(id => long.Parse(id))
                                .ToList();
                            
                            if (deletedFileIds.Count > 0)
                            {
                                imageCacheService.RemoveImageInfos(deletedFileIds);
                                _logger?.LogInformation("已从ImageCacheService缓存中移除{Count}个已删除的文件", deletedFileIds.Count);
                            }
                        }
                    }
                    catch (Exception cacheEx)
                    {
                        _logger?.LogWarning(cacheEx, "清除缓存失败,不影响删除结果");
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "删除文件失败: BusinessId={BusinessId}, OwnerTableName={OwnerTableName}",
                    businessId, ownerTableName);
                return ResponseFactory.CreateSpecificErrorResponse<FileDeleteResponse>($"删除文件失败: {ex.Message}");
            }
        }

        #region 图片压缩功能

        /// <summary>
        /// 图片压缩结果
        /// </summary>
        private class CompressResult
        {
            public byte[] CompressedData { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        /// <summary>
        /// ✅ 图片压缩: 根据文件大小和尺寸判断是否需要压缩
        /// </summary>
        /// <param name="imageData">原始图片数据</param>
        /// <param name="fileName">文件名(用于判断格式)</param>
        /// <returns>压缩结果(包含压缩后的数据和宽高信息)</returns>
        private CompressResult CompressImageIfNeeded(byte[] imageData, string fileName)
        {
            var result = new CompressResult();
            
            try
            {
                // 1. 检查是否为图片格式
                string extension = Path.GetExtension(fileName)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || 
                    !new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" }.Contains(extension))
                {
                    return result; // 非图片格式,返回空结果
                }

                // 2. 尝试加载图片并获取尺寸
                using (var ms = new MemoryStream(imageData))
                using (var originalImage = Image.FromStream(ms))
                {
                    result.Width = originalImage.Width;
                    result.Height = originalImage.Height;
                    
                    // 3. 小文件不压缩
                    if (imageData.Length < MinCompressSize)
                    {
                        return result; // 返回宽高信息,但不压缩
                    }

                    // 4. 如果尺寸已经很小,仅对JPEG进行质量压缩
                    if (originalImage.Width <= MaxImageWidth && originalImage.Height <= MaxImageHeight)
                    {
                        if (extension == ".jpg" || extension == ".jpeg")
                        {
                            result.CompressedData = CompressJpegQuality(originalImage);
                        }
                        return result;
                    }

                    // 5. 需要缩放+压缩
                    _logger?.LogDebug("[图片压缩] 开始压缩: {Width}x{Height}, {Size}KB",
                        originalImage.Width, originalImage.Height, imageData.Length / 1024);

                    // 计算缩放比例
                    float widthRatio = (float)MaxImageWidth / originalImage.Width;
                    float heightRatio = (float)MaxImageHeight / originalImage.Height;
                    float ratio = Math.Min(widthRatio, heightRatio);

                    int newWidth = (int)(originalImage.Width * ratio);
                    int newHeight = (int)(originalImage.Height * ratio);
                    
                    // 更新结果中的宽高
                    result.Width = newWidth;
                    result.Height = newHeight;

                    // 创建缩放后的图片
                    using (var resizedImage = new Bitmap(newWidth, newHeight))
                    {
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                        }

                        // 6. 根据原格式保存
                        if (extension == ".jpg" || extension == ".jpeg")
                        {
                            result.CompressedData = SaveAsJpeg(resizedImage);
                        }
                        else if (extension == ".png")
                        {
                            result.CompressedData = SaveAsPng(resizedImage);
                        }
                        else
                        {
                            // 其他格式转为JPEG
                            result.CompressedData = SaveAsJpeg(resizedImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 压缩失败,返回原始宽高信息(如果有),不影响上传
                _logger?.LogWarning(ex, "[图片压缩] 压缩失败,使用原始图片");
            }
            
            return result;
        }

        /// <summary>
        /// 仅压缩JPEG质量(不改变尺寸)
        /// </summary>
        private byte[] CompressJpegQuality(Image image)
        {
            try
            {
                var encoder = GetEncoder(ImageFormat.Jpeg);
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, JpegQuality);

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, encoder, encoderParams);
                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 保存为JPEG格式
        /// </summary>
        private byte[] SaveAsJpeg(Image image)
        {
            try
            {
                var encoder = GetEncoder(ImageFormat.Jpeg);
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, JpegQuality);

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, encoder, encoderParams);
                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 保存为PNG格式
        /// </summary>
        private byte[] SaveAsPng(Image image)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取图片编码器
        /// </summary>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #endregion

        /// <summary>
        /// 根据路径获取文件信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件存储信息</returns>
        public async Task<tb_FS_FileStorageInfo> GetFileInfoByPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            try
            {
                var db = _unitOfWorkManage.GetDbClient().CopyNew();
                return await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(c => c.StoragePath == filePath || c.StorageFileName == filePath)
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "根据路径获取文件信息失败: FilePath={FilePath}", filePath);
                return null;
            }
        }
    }
}