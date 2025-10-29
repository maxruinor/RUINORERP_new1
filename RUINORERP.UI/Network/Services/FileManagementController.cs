using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Context;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global;
using System.IO;
using RUINORERP.Common.Extensions;
using LiveChartsCore.Geo;
using RUINORERP.UI.Network.Services;
using RUINORERP.Business;
using RUINOR.WinFormsUI.CustomPictureBox;
using System.Linq;
using static NPOI.HSSF.UserModel.HeaderFooter;
using FastReport.DevComponents.DotNetBar;
using FastReport.Data;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 文件管理服务实现类
    /// </summary>
    public class FileManagementController
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<FileManagementController> _logger;
        public readonly ApplicationContext _appContext;
        private readonly IEntityMappingService _mapper;
        public FileManagementController(IEntityMappingService mapper, IUnitOfWorkManage unitOfWorkManage, ILogger<FileManagementController> logger, ApplicationContext appContext = null)
        {
            _mapper = mapper;
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
            _appContext = appContext;
        }


        /// <summary>
        /// 上传文件（图片或其他类型文件）
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="OriginalFileName">文件名</param>
        /// <param name="fileData">文件数据</param>
        /// <param name="fileId">文件ID（可选），用于更新现有文件</param>
        /// <param name="updateReason">更新原因（可选），用于版本控制</param>
        /// <param name="useVersionControl">是否使用版本控制（可选），默认为false</param>
        /// <returns>上传结果，包含文件ID</returns>
        public async Task<FileUploadResponse> UploadImageAsync(BaseEntity entity, string OriginalFileName, byte[] fileData, string RelatedField, long? fileId = null, string updateReason = null, bool useVersionControl = false)
        {
            // 参数验证
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrEmpty(OriginalFileName))
                throw new ArgumentException("文件名不能为空", nameof(OriginalFileName));

            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("文件数据不能为空", nameof(fileData));

            try
            {
                // 获取文件管理服务
                var fileService = _appContext.GetRequiredService<FileManagementService>();

                // 获取实体信息
                var entityInfo = _mapper.GetEntityInfo(entity.GetType());
                if (entityInfo == null || string.IsNullOrEmpty(entityInfo.NoField))
                {
                    throw new ArgumentException("无效的业务实体类型");
                }

                // 获取业务编号
                string businessNo = entity.GetPropertyValue<string>(entityInfo.NoField).ToString();

                tb_FS_FileStorageInfo storageInfo = new tb_FS_FileStorageInfo();
                storageInfo.OriginalFileName = OriginalFileName;
                storageInfo.BusinessType = (int)entityInfo.BizType;
                storageInfo.FileData = fileData;

                // 如果提供了文件ID
                if (fileId.HasValue)
                {
                    storageInfo.FileId = fileId.Value;

                    // 根据版本控制开关决定是否启用版本控制
                    // 注意：tb_FS_FileStorageInfo类中没有IsUpdate和UpdateReason属性
                    // 版本控制逻辑将在服务器端处理
                    // 不使用版本控制时，仍然设置FileId，但不启用版本控制标志
                }

                // 准备上传请求
                var uploadRequest = new FileUploadRequest();
                uploadRequest.FileStorageInfos.Add(storageInfo);
                uploadRequest.BusinessNo = businessNo;
                uploadRequest.BusinessType = (int)entityInfo.BizType;
                uploadRequest.RelatedField = RelatedField;
                // 执行上传
                var response = await fileService.UploadFileAsync(uploadRequest);

                // 如果上传成功，创建业务关联
                if (response.IsSuccess && response.FileStorageInfos.Count > 0)
                {
                    // 获取业务关联服务
                    var businessRelationService = _appContext.GetRequiredService<tb_FS_BusinessRelationController<tb_FS_BusinessRelation>>();
                    foreach (var fileStorageInfo in response.FileStorageInfos)
                    {
                        //服务器创建更合理，因为每经过一个环节 出错几率更高
                        //// 创建业务关联记录
                        //var businessRelation = new tb_FS_BusinessRelation
                        //{
                        //    BusinessType = (int)entityInfo.BizType,
                        //    BusinessNo = businessNo,
                        //    FileId = fileStorageInfo.FileId,
                        //    IsMainFile = true,
                        //    Created_at = DateTime.Now,
                        //    Created_by = _appContext.CurrentUser?.UserID ?? 0
                        //};

                        //// 保存业务关联
                        //await businessRelationService.SaveOrUpdate(businessRelation);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "上传图片失败");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>($"上传图片失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 下载图片文件
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>文件下载响应列表</returns>
        public async Task<List<FileDownloadResponse>> DownloadImageAsync(BaseEntity entity)
        {
            List<FileDownloadResponse> fileDownloadResponses = new List<FileDownloadResponse>();
            // 获取文件管理服务
            var fileService = _appContext.GetRequiredService<FileManagementService>();
            try
            {
                var entityInfo = _mapper.GetEntityInfo(entity.GetType());
                if (entityInfo != null && entityInfo.Fields != null)
                {
                    string BusinessNo = entity.GetPropertyValue<string>(entityInfo.NoField).ToString();

                    // 使用db.CopyNew()创建独立的数据库连接上下文，避免连接共享导致的关闭问题
                    var db = _unitOfWorkManage.GetDbClient().CopyNew();
                    // 使用新的数据库连接上下文获取业务关联列表
                    var BusinessRelationList = await db.Queryable<tb_FS_BusinessRelation>()
                        .Where(c => c.BusinessType == (int)entityInfo.BizType && c.BusinessNo == BusinessNo)
                        .Includes(t => t.tb_fs_filestorageinfo)
                        .ToListAsync();
                    
                    // 预加载文件存储信息到内存中，避免异步操作后数据库连接关闭导致的延迟加载问题
                    var fileStorageInfosToDownload = new List<tb_FS_FileStorageInfo>();
                    foreach (var item in BusinessRelationList)
                    {
                        if (item.tb_fs_filestorageinfo != null)
                        {
                            // 创建一个新的对象并复制所有必要的属性，确保数据完全加载到内存中
                            var fileInfo = new tb_FS_FileStorageInfo
                            {
                                FileId = item.tb_fs_filestorageinfo.FileId,
                                FileSize = item.tb_fs_filestorageinfo.FileSize,
                                FileType = item.tb_fs_filestorageinfo.FileType,
                                FileExtension = item.tb_fs_filestorageinfo.FileExtension,
                                // 复制其他必要属性...
                                FileData = item.tb_fs_filestorageinfo.FileData // 如果文件数据已加载，直接复制
                            };
                            fileStorageInfosToDownload.Add(fileInfo);
                        }
                    }
                    
                    // 现在使用完全加载到内存中的文件信息进行下载操作
                    foreach (var fileInfo in fileStorageInfosToDownload)
                    {
                        // 创建下载请求
                        var request = new FileDownloadRequest
                        {
                            FileStorageInfo = fileInfo
                        };
                        
                        // 下载文件
                        var response = await fileService.DownloadFileAsync(request);
                        if (response.IsSuccess && response.FileStorageInfos != null && response.FileStorageInfos.Count > 0)
                        {
                            fileDownloadResponses.Add(FileDownloadResponse.CreateSuccess(response.FileStorageInfos, "文件下载成功"));
                        }
                        else
                        {
                            // 记录错误日志
                            System.Diagnostics.Debug.WriteLine($"图片下载失败: {response.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志
                System.Diagnostics.Debug.WriteLine($"图片下载异常: {ex.Message}\n{ex.StackTrace}");
                // 如果是阅读器关闭错误，提供更详细的错误信息
                if (ex.Message.Contains("阅读器关闭") || ex.Message.Contains("FieldCount"))
                {
                    System.Diagnostics.Debug.WriteLine("错误原因：数据库连接已关闭但仍尝试访问数据。请检查异步操作中的数据加载方式。");
                }
            }
            return fileDownloadResponses;
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

            ImageInfo imageInfo = new ImageInfo
            {
                // 基本信息映射
                FileId = fileStorageInfo.FileId,
                OriginalFileName = fileStorageInfo.OriginalFileName,
                FileSize = fileStorageInfo.FileSize,
                FileType = fileStorageInfo.FileType,
                FileExtension = fileStorageInfo.FileExtension,
                HashValue = fileStorageInfo.HashValue,
                ModifiedAt = fileStorageInfo.Modified_at,

                // 设置创建时间，如果没有则使用当前时间
                CreateTime = fileStorageInfo.Created_at ?? DateTime.Now,

                // 初始化元数据字典
                Metadata = new Dictionary<string, string>()
            };

            // 尝试解析Metadata字符串到字典
            if (!string.IsNullOrEmpty(fileStorageInfo.Metadata))
            {
                try
                {
                    // 这里可以根据实际的Metadata格式进行解析
                    // 如果是JSON格式，可以使用Newtonsoft.Json进行解析
                    // 暂时作为简单字符串存储
                    imageInfo.Metadata["OriginalMetadata"] = fileStorageInfo.Metadata;
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "解析文件元数据失败");
                }
            }

            // 添加额外的元数据信息
            if (fileStorageInfo.StorageProvider != null)
            {
                imageInfo.Metadata["StorageProvider"] = fileStorageInfo.StorageProvider;
            }

            if (fileStorageInfo.StoragePath != null)
            {
                imageInfo.Metadata["StoragePath"] = fileStorageInfo.StoragePath;
            }

            return imageInfo;
        }

        /// <summary>
        /// 将tb_FS_FileStorageInfo实体列表转换为ImageInfo列表
        /// </summary>
        /// <param name="fileStorageInfos">文件存储信息实体列表</param>
        /// <returns>转换后的ImageInfo对象列表</returns>
        public List<ImageInfo> ConvertToImageInfoList(List<tb_FS_FileStorageInfo> fileStorageInfos)
        {
            if (fileStorageInfos == null || fileStorageInfos.Count == 0)
                return new List<ImageInfo>();

            List<ImageInfo> imageInfos = new List<ImageInfo>();
            foreach (var fileStorageInfo in fileStorageInfos)
            {
                var imageInfo = ConvertToImageInfo(fileStorageInfo);
                if (imageInfo != null)
                {
                    imageInfos.Add(imageInfo);
                }
            }
            return imageInfos;
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

            tb_FS_FileStorageInfo fileStorageInfo = new tb_FS_FileStorageInfo
            {
                // 基本信息映射
                FileId = imageInfo.FileId,
                OriginalFileName = imageInfo.OriginalFileName,
                FileSize = imageInfo.FileSize,
                FileType = imageInfo.FileType,
                FileExtension = imageInfo.FileExtension,
                HashValue = imageInfo.HashValue,
                Created_at = imageInfo.CreateTime,
                Modified_at = imageInfo.ModifiedAt,

                // 设置默认值
                StorageProvider = "Local", // 默认本地存储
                CurrentVersion = 1,         // 初始版本
                Status = 0,                 // 正常状态
                ExpireTime = DateTime.MaxValue, // 永不过期
                StoragePath = string.Empty
            };

            // 处理元数据
            if (imageInfo.Metadata != null && imageInfo.Metadata.Count > 0)
            {
                try
                {
                    // 检查是否有原始元数据
                    if (imageInfo.Metadata.TryGetValue("OriginalMetadata", out string originalMetadata))
                    {
                        fileStorageInfo.Metadata = originalMetadata;
                    }
                    else
                    {
                        // 可以根据实际需要将字典转换为JSON格式或其他格式
                        // 这里简单地存储一些关键信息
                        fileStorageInfo.Metadata = string.Join(";", imageInfo.Metadata.Select(kv => $"{kv.Key}={kv.Value}"));
                    }

                    // 从元数据中提取存储信息
                    if (imageInfo.Metadata.TryGetValue("StorageProvider", out string storageProvider))
                    {
                        fileStorageInfo.StorageProvider = storageProvider;
                    }

                    if (imageInfo.Metadata.TryGetValue("StoragePath", out string storagePath))
                    {
                        fileStorageInfo.StoragePath = storagePath;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "处理图片元数据失败");
                    fileStorageInfo.Metadata = string.Empty;
                }
            }

            // 生成存储文件名（如果未提供）
            if (string.IsNullOrEmpty(fileStorageInfo.StorageFileName))
            {
                fileStorageInfo.StorageFileName = GenerateStorageFileName(imageInfo.OriginalFileName, imageInfo.FileId);
            }

            return fileStorageInfo;
        }

        /// <summary>
        /// 将ImageInfo列表转换为tb_FS_FileStorageInfo实体列表
        /// </summary>
        /// <param name="imageInfos">图片信息对象列表</param>
        /// <returns>转换后的tb_FS_FileStorageInfo实体列表</returns>
        public List<tb_FS_FileStorageInfo> ConvertToFileStorageInfoList(List<ImageInfo> imageInfos)
        {
            if (imageInfos == null || imageInfos.Count == 0)
                return new List<tb_FS_FileStorageInfo>();

            List<tb_FS_FileStorageInfo> fileStorageInfos = new List<tb_FS_FileStorageInfo>();
            foreach (var imageInfo in imageInfos)
            {
                var fileStorageInfo = ConvertToFileStorageInfo(imageInfo);
                if (fileStorageInfo != null)
                {
                    fileStorageInfos.Add(fileStorageInfo);
                }
            }
            return fileStorageInfos;
        }

        /// <summary>
        /// 生成存储文件名
        /// </summary>
        /// <param name="originalFileName">原始文件名</param>
        /// <param name="fileId">文件ID</param>
        /// <returns>生成的存储文件名</returns>
        private string GenerateStorageFileName(string originalFileName, long fileId)
        {
            string extension = string.Empty;
            if (!string.IsNullOrEmpty(originalFileName))
            {
                extension = Path.GetExtension(originalFileName);
            }

            // 使用文件ID和时间戳生成唯一的存储文件名
            return $"{fileId}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
        }



        /// <summary>
        /// 删除与业务实体关联的所有图片
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="physicalDelete">是否物理删除文件（true:物理删除，false:逻辑删除）</param>
        /// <returns>删除结果</returns>
        public async Task<FileDeleteResponse> DeleteImagesAsync(BaseEntity entity, bool physicalDelete = false)
        {
            FileDeleteResponse deleteResponse = new FileDeleteResponse();

            // 参数验证
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // 获取文件管理服务
                var fileService = _appContext.GetRequiredService<FileManagementService>();
                // 获取实体信息
                var entityInfo = _mapper.GetEntityInfo(entity.GetType());
                if (entityInfo == null || string.IsNullOrEmpty(entityInfo.NoField))
                {
                    throw new ArgumentException("无效的业务实体类型");
                }

                // 获取业务编号
                string businessNo = entity.GetPropertyValue<string>(entityInfo.NoField).ToString();
                int businessType = (int)entityInfo.BizType;

                // 创建删除请求
                FileDeleteRequest deleteRequest = new FileDeleteRequest();
                deleteRequest.BusinessNo = businessNo;
                deleteRequest.BusinessType = businessType;
                deleteRequest.PhysicalDelete = physicalDelete;

                if (entity.FileStorageInfoList != null && entity.FileStorageInfoList.Count > 0)
                {
                    deleteRequest.AddDeleteFileStorageInfo(entity.FileStorageInfoList);
                }
                else
                {  // 获取文件关联服务
                    var businessRelationService = _appContext.GetRequiredService<tb_FS_BusinessRelationController<tb_FS_BusinessRelation>>();
                    // 获取当前业务实体关联的所有文件关系
                    var businessRelationList = await businessRelationService.QueryByNavAsync(c =>
                        c.BusinessType == businessType && c.BusinessNo == businessNo);
                    deleteRequest.AddDeleteFileStorageInfo(businessRelationList.Select(x => x.tb_fs_filestorageinfo).ToList());
                }
                // 执行删除
                deleteResponse = await fileService.DeleteFileAsync(deleteRequest);
                return deleteResponse;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量删除图片失败");
                return ResponseFactory.CreateSpecificErrorResponse<FileDeleteResponse>("批量删除图片失败");
            }
        }

    }
}