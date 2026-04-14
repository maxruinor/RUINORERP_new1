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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

                _logger?.LogInformation("[FileBusinessService] 开始上传文件: OwnerTableName={OwnerTableName}, BusinessId={BusinessId}, RelatedField={RelatedField}, FileName={FileName}",
                    uploadRequest.OwnerTableName, uploadRequest.BusinessId, uploadRequest.RelatedField, fileName);

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
        public RUINORERP.Lib.BusinessImage.ImageInfo ConvertToImageInfo(tb_FS_FileStorageInfo fileStorageInfo)
        {
            if (fileStorageInfo == null)
                return null;

            return new RUINORERP.Lib.BusinessImage.ImageInfo
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
        public tb_FS_FileStorageInfo ConvertToFileStorageInfo(RUINORERP.Lib.BusinessImage.ImageInfo imageInfo)
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