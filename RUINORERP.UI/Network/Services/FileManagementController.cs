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
        private readonly IBusinessEntityMappingService _mapper;
        public FileManagementController(IBusinessEntityMappingService mapper, IUnitOfWorkManage unitOfWorkManage, ILogger<FileManagementController> logger, ApplicationContext appContext = null)
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
        /// <param name="fileId">文件ID</param>
        /// <param name="appContext">应用程序上下文</param>
        /// <returns>图片对象，如果下载失败则返回null</returns>
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

                    // 获取文件关联服务
                    var BusinessRelationService = _appContext.GetRequiredService<tb_FS_BusinessRelationController<tb_FS_BusinessRelation>>();
                    string BusinessNo = entity.GetPropertyValue<string>(entityInfo.NoField).ToString();

                    var BusinessRelationList = await BusinessRelationService.QueryByNavAsync(c => c.BusinessType == (int)entityInfo.BizType && c.BusinessNo == BusinessNo);
                    foreach (var item in BusinessRelationList)
                    {
                        if (item.tb_fs_filestorageinfo != null)
                        {
                            // 创建下载请求
                            var request = new FileDownloadRequest
                            {
                                FileStorageInfo = item.tb_fs_filestorageinfo
                            };
                            // 下载文件
                            #region
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
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志
                System.Diagnostics.Debug.WriteLine($"图片下载异常: {ex.Message}");
            }
            return fileDownloadResponses;
        }



        
        
       

        
    }
}