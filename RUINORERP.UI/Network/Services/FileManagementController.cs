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
        public async Task<FileUploadResponse> UploadImageAsync(BaseEntity entity, string OriginalFileName, byte[] fileData, long? fileId = null, string updateReason = null, bool useVersionControl = false)
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



        /// <summary>
        /// 保存文件存储信息
        /// </summary>
        /// <param name="fileInfo">文件存储信息</param>
        /// <returns>保存结果</returns>
        public async Task<bool> SaveFileStorageInfoAsync(tb_FS_FileStorageInfo fileInfo)
        {
            try
            {
                if (fileInfo.FileId > 0)
                {
                    // 更新操作
                    var result = await _unitOfWorkManage.GetDbClient().Updateable(fileInfo).ExecuteCommandAsync();
                    return result > 0;
                }
                else
                {
                    // 新增操作
                    var result = await _unitOfWorkManage.GetDbClient().Insertable(fileInfo).ExecuteReturnSnowflakeIdAsync();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存文件存储信息时出错");
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取文件存储信息
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns>文件存储信息</returns>
        public async Task<tb_FS_FileStorageInfo> GetFileStorageInfoByIdAsync(long id)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>()
                    .FirstAsync(f => f.FileId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据ID获取文件存储信息时出错: {Id}", id);
                return null;
            }
        }

        /// <summary>
        /// 根据文件ID获取文件存储信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>文件存储信息</returns>
        public async Task<tb_FS_FileStorageInfo> GetFileStorageInfoByFileIdAsync(string fileId)
        {
            try
            {
                // 这里假设fileId是存储文件名的一部分，需要根据实际情况调整查询逻辑
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>()
                    .FirstAsync(f => f.StorageFileName.Contains(fileId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据文件ID获取文件存储信息时出错: {FileId}", fileId);
                return null;
            }
        }

        /// <summary>
        /// 获取所有文件存储信息
        /// </summary>
        /// <returns>文件存储信息列表</returns>
        public async Task<List<tb_FS_FileStorageInfo>> GetAllFileStorageInfosAsync()
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageInfo>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有文件存储信息时出错");
                return new List<tb_FS_FileStorageInfo>();
            }
        }

        /// <summary>
        /// 删除文件存储信息
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteFileStorageInfoAsync(long id)
        {
            try
            {
                var result = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FS_FileStorageInfo>(id).ExecuteCommandAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除文件存储信息时出错: {Id}", id);
                return false;
            }
        }

        /// <summary>
        /// 保存业务关联信息
        /// </summary>
        /// <param name="businessRelation">业务关联信息</param>
        /// <returns>保存结果</returns>
        public async Task<bool> SaveBusinessRelationAsync(tb_FS_BusinessRelation businessRelation)
        {
            try
            {
                if (businessRelation.RelationId > 0)
                {
                    // 更新操作
                    var result = await _unitOfWorkManage.GetDbClient().Updateable(businessRelation).ExecuteCommandAsync();
                    return result > 0;
                }
                else
                {
                    // 新增操作
                    var result = await _unitOfWorkManage.GetDbClient().Insertable(businessRelation).ExecuteReturnSnowflakeIdAsync();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存业务关联信息时出错");
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取业务关联信息
        /// </summary>
        /// <param name="id">关联ID</param>
        /// <returns>业务关联信息</returns>
        public async Task<tb_FS_BusinessRelation> GetBusinessRelationByIdAsync(long id)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_BusinessRelation>()
                    .FirstAsync(f => f.RelationId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据ID获取业务关联信息时出错: {Id}", id);
                return null;
            }
        }

        /// <summary>
        /// 根据文件ID获取业务关联信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>业务关联信息列表</returns>
        public async Task<List<tb_FS_BusinessRelation>> GetBusinessRelationsByFileIdAsync(long fileId)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_BusinessRelation>()
                    .Where(f => f.FileId == fileId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据文件ID获取业务关联信息时出错: {FileId}", fileId);
                return new List<tb_FS_BusinessRelation>();
            }
        }

        /// <summary>
        /// 获取所有业务关联信息
        /// </summary>
        /// <returns>业务关联信息列表</returns>
        public async Task<List<tb_FS_BusinessRelation>> GetAllBusinessRelationsAsync()
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_BusinessRelation>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有业务关联信息时出错");
                return new List<tb_FS_BusinessRelation>();
            }
        }

        /// <summary>
        /// 删除业务关联信息
        /// </summary>
        /// <param name="id">关联ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteBusinessRelationAsync(long id)
        {
            try
            {
                var result = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FS_BusinessRelation>(id).ExecuteCommandAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除业务关联信息时出错: {Id}", id);
                return false;
            }
        }

        /// <summary>
        /// 保存文件版本信息
        /// </summary>
        /// <param name="fileVersion">文件版本信息</param>
        /// <returns>保存结果</returns>
        public async Task<bool> SaveFileStorageVersionAsync(tb_FS_FileStorageVersion fileVersion)
        {
            try
            {
                if (fileVersion.VersionId > 0)
                {
                    // 更新操作
                    var result = await _unitOfWorkManage.GetDbClient().Updateable(fileVersion).ExecuteCommandAsync();
                    return result > 0;
                }
                else
                {
                    // 新增操作
                    var result = await _unitOfWorkManage.GetDbClient().Insertable(fileVersion).ExecuteReturnSnowflakeIdAsync();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存文件版本信息时出错");
                return false;
            }
        }

        /// <summary>
        /// 根据ID获取文件版本信息
        /// </summary>
        /// <param name="id">版本ID</param>
        /// <returns>文件版本信息</returns>
        public async Task<tb_FS_FileStorageVersion> GetFileStorageVersionByIdAsync(long id)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>()
                    .FirstAsync(f => f.VersionId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据ID获取文件版本信息时出错: {Id}", id);
                return null;
            }
        }

        /// <summary>
        /// 根据文件ID获取文件版本信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>文件版本信息列表</returns>
        public async Task<List<tb_FS_FileStorageVersion>> GetFileStorageVersionsByFileIdAsync(long fileId)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>()
                    .Where(f => f.FileId == fileId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "根据文件ID获取文件版本信息时出错: {FileId}", fileId);
                return new List<tb_FS_FileStorageVersion>();
            }
        }

        /// <summary>
        /// 获取所有文件版本信息
        /// </summary>
        /// <returns>文件版本信息列表</returns>
        public async Task<List<tb_FS_FileStorageVersion>> GetAllFileStorageVersionsAsync()
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有文件版本信息时出错");
                return new List<tb_FS_FileStorageVersion>();
            }
        }

        /// <summary>
        /// 删除文件版本信息
        /// </summary>
        /// <param name="id">版本ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteFileStorageVersionAsync(long id)
        {
            try
            {
                var result = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FS_FileStorageVersion>(id).ExecuteCommandAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除文件版本信息时出错: {Id}", id);
                return false;
            }
        }
    }
}