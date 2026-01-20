using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.UI.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 增强的文件管理服务扩展方法
    /// 提供优化的查询和加载策略
    /// </summary>
    public static class FileManagementServiceExtensions
    {
        /// <summary>
        /// 获取关联图片列表（带HasAttachment检查）
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="fileService">文件管理服务</param>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="entity">业务实体</param>
        /// <param name="relatedField">关联字段（可选）</param>
        /// <param name="useLazyLoad">是否使用延迟加载（默认true）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>图片列表</returns>
        public static async Task<List<tb_FS_FileStorageInfo>> GetRelatedImagesEnhancedAsync<T>(
            this FileManagementService fileService,
            IUnitOfWorkManage unitOfWork,
            T entity,
            string relatedField = null,
            bool useLazyLoad = true,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            // 参数验证
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 快速判断：如果HasAttachment=false且不使用延迟加载，直接返回空列表
            if (!entity.HasAttachment && useLazyLoad)
            {
                return new List<tb_FS_FileStorageInfo>();
            }

            try
            {
                var db = unitOfWork.GetDbClient().CopyNew();

                // 构建查询条件
                var query = db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => r.BusinessId == entity.PrimaryKeyID)
                    .Where(r => r.IsActive == true)
                    .Where(r => r.isdeleted == false);

                // 如果指定了关联字段，按字段筛选
                if (!string.IsNullOrEmpty(relatedField))
                {
                    query = query.Where(r => r.RelatedField == relatedField);
                }

                // 使用Includes加载关联的文件信息
                var relations = await query
                    .Includes(r => r.tb_fs_filestorageinfo)
                    .ToListAsync(cancellationToken);

                // 提取文件信息列表
                var images = relations
                    .Where(r => r.tb_fs_filestorageinfo != null)
                    .Select(r => r.tb_fs_filestorageinfo)
                    .ToList();

                return images;
            }
            catch (Exception ex)
            {
                var logger = fileService.GetType().GetProperty("_log")?.GetValue(fileService) as ILogger;
                logger?.LogError(ex, "查询关联图片失败: EntityId={EntityId}, RelatedField={RelatedField}",
                    entity.PrimaryKeyID, relatedField);
                return new List<tb_FS_FileStorageInfo>();
            }
        }

        /// <summary>
        /// 批量获取业务实体的HasAttachment标志
        /// 优化：使用IN查询避免多次数据库访问
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="entityIds">实体ID列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>ID到HasAttachment标志的字典</returns>
        public static async Task<Dictionary<long, bool>> BatchGetHasAttachmentAsync<T>(
            this IUnitOfWorkManage unitOfWork,
            List<long> entityIds,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            if (entityIds == null || entityIds.Count == 0)
                return new Dictionary<long, bool>();

            try
            {
                var db = unitOfWork.GetDbClient().CopyNew();

                // 查询关联表
                var relations = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => entityIds.Contains(r.BusinessId))
                    .Where(r => r.IsActive == true)
                    .Where(r => r.isdeleted == false)
                    .GroupBy(r => r.BusinessId)
                    .Select(g => new { BusinessId = g.BusinessId })
                    .ToListAsync(cancellationToken);

                // 构建结果字典
                var result = new Dictionary<long, bool>();
                foreach (var entityId in entityIds)
                {
                    result[entityId] = relations.Any(r => r.BusinessId == entityId);
                }

                return result;
            }
            catch (Exception ex)
            {
                var logger = unitOfWork?.GetType().GetProperty("Logger")?.GetValue(unitOfWork) as ILogger;
                logger?.LogError(ex, "批量查询HasAttachment标志失败");
                return entityIds.ToDictionary(id => id, _ => false);
            }
        }

        /// <summary>
        /// 更新实体的HasAttachment标志
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="hasAttachment">是否有附件</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否更新成功</returns>
        public static async Task<bool> UpdateHasAttachmentFlagAsync<T>(
            this IUnitOfWorkManage unitOfWork,
            long businessId,
            bool hasAttachment,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            if (businessId <= 0)
                return false;

            try
            {
                var db = unitOfWork.GetDbClient();

                // 检查目标表是否有HasAttachment字段
                var tableName = db.DbMaintenance.GetTableNameByClass(typeof(T));
                var hasColumn = await db.DbMaintenance.IsAnyColumn(tableName, "HasAttachment");

                if (!hasColumn)
                {
                    // 如果数据库中没有HasAttachment字段，跳过更新
                    return false;
                }

                // 更新HasAttachment标志
                int affectedRows = await db.Updateable<T>()
                    .SetColumns(t => new T { HasAttachment = hasAttachment })
                    .Where(t => t.PrimaryKeyID == businessId)
                    .ExecuteCommandAsync(cancellationToken);

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                var logger = unitOfWork?.GetType().GetProperty("Logger")?.GetValue(unitOfWork) as ILogger;
                logger?.LogError(ex, "更新HasAttachment标志失败: BusinessId={BusinessId}, Value={Value}",
                    businessId, hasAttachment);
                return false;
            }
        }

        /// <summary>
        /// 根据RelatedField分类获取图片
        /// 返回字典：Key=RelatedField, Value=图片列表
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="fileService">文件管理服务</param>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="entity">业务实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>按字段分类的图片字典</returns>
        public static async Task<Dictionary<string, List<tb_FS_FileStorageInfo>>> GetImagesByFieldAsync<T>(
            this FileManagementService fileService,
            IUnitOfWorkManage unitOfWork,
            T entity,
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var result = new Dictionary<string, List<tb_FS_FileStorageInfo>>();

            // 快速判断：如果HasAttachment=false，直接返回空字典
            if (!entity.HasAttachment)
            {
                return result;
            }

            try
            {
                var db = unitOfWork.GetDbClient().CopyNew();

                var relations = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => r.BusinessId == entity.PrimaryKeyID)
                    .Where(r => r.IsActive == true)
                    .Where(r => r.isdeleted == false)
                    .Includes(r => r.tb_fs_filestorageinfo)
                    .ToListAsync(cancellationToken);

                // 按RelatedField分组
                foreach (var relation in relations)
                {
                    if (relation.tb_fs_filestorageinfo == null)
                        continue;

                    var field = relation.RelatedField ?? "Default";

                    if (!result.ContainsKey(field))
                    {
                        result[field] = new List<tb_FS_FileStorageInfo>();
                    }

                    result[field].Add(relation.tb_fs_filestorageinfo);
                }

                return result;
            }
            catch (Exception ex)
            {
                var logger = fileService.GetType().GetProperty("_log")?.GetValue(fileService) as ILogger;
                logger?.LogError(ex, "按字段分类查询图片失败: EntityId={EntityId}", entity.PrimaryKeyID);
                return result;
            }
        }

        /// <summary>
        /// 获取主图片（如果有的话）
        /// 优先从直接字段读取，其次从关联表查询
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="fileService">文件管理服务</param>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="entity">业务实体</param>
        /// <param name="directImageField">直接存储图片路径的字段名（如"ImagesPath"）</param>
        /// <param name="relatedField">关联字段名（如"MainImage"）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>主图片路径</returns>
        public static async Task<string> GetMainImageAsync<T>(
            this FileManagementService fileService,
            IUnitOfWorkManage unitOfWork,
            T entity,
            string directImageField = "ImagesPath",
            string relatedField = "MainImage",
            CancellationToken cancellationToken = default) where T : BaseEntity
        {
            if (entity == null)
                return null;

            // 策略1: 优先从直接字段读取
            var imageProperty = typeof(T).GetProperty(directImageField);
            if (imageProperty != null)
            {
                var imagePath = imageProperty.GetValue(entity) as string;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    return imagePath;
                }
            }

            // 策略2: 从关联表查询（仅在HasAttachment=true时）
            if (entity.HasAttachment)
            {
                var images = await fileService.GetRelatedImagesEnhancedAsync(
                    unitOfWork, entity, relatedField, false, cancellationToken);

                if (images.Any())
                {
                    // 从关联表获取图片数据（需要下载）
                    // 这里返回第一个图片的标识，实际使用时需要调用下载服务
                    return $"FileId:{images[0].FileId}";
                }
            }

            return null;
        }
    }
}
