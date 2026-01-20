using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// HasAttachment标志同步服务
    /// 负责在文件上传、删除、更新时同步业务表的HasAttachment标志位
    /// </summary>
    public class HasAttachmentSyncService
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<HasAttachmentSyncService> _logger;
        private readonly ApplicationContext _applicationContext;
        private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 构造函数
        /// </summary>
        public HasAttachmentSyncService(
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<HasAttachmentSyncService> logger,
            ApplicationContext applicationContext)
        {
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _logger = logger;
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// 文件上传后同步HasAttachment标志
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否同步成功</returns>
        public async Task<bool> SyncOnFileUploadAsync(
            int businessType,
            long businessId,
            string businessNo,
            CancellationToken cancellationToken = default)
        {
            if (businessId <= 0)
                return false;

            try
            {
                await _syncLock.WaitAsync(cancellationToken);

                _logger?.LogDebug("开始同步HasAttachment标志（文件上传）: BusinessType={BusinessType}, BusinessId={BusinessId}",
                    businessType, businessId);

                var db = _unitOfWorkManage.GetDbClient();

                // 根据业务类型确定表名和主键字段
                var tableName = GetTableNameByBusinessType(businessType);
                var idFieldName = GetIdFieldNameByBusinessType(businessType);

                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(idFieldName))
                {
                    _logger?.LogWarning("未知的业务类型，跳过HasAttachment同步: BusinessType={BusinessType}", businessType);
                    return false;
                }

                // 检查表是否有HasAttachment字段
                var hasColumn = db.DbMaintenance.IsAnyColumn(tableName, "HasAttachment");
                if (!hasColumn)
                {
                    _logger?.LogDebug("表 {TableName} 没有HasAttachment字段，跳过同步", tableName);
                    return false;
                }

                // 更新HasAttachment标志为true
                string updateSql = $"UPDATE {tableName} SET HasAttachment = 1 WHERE {idFieldName} = @BusinessId";
                int affectedRows = db.Ado.ExecuteCommand(updateSql, new { BusinessId = businessId });

                if (affectedRows > 0)
                {
                    _logger?.LogDebug("已更新HasAttachment标志: Table={Table}, BusinessId={BusinessId}",
                        tableName, businessId);
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "同步HasAttachment标志失败（文件上传）: BusinessType={BusinessType}, BusinessId={BusinessId}",
                    businessType, businessId);
                return false;
            }
            finally
            {
                _syncLock.Release();
            }
        }

        /// <summary>
        /// 文件删除后同步HasAttachment标志
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否同步成功</returns>
        public async Task<bool> SyncOnFileDeleteAsync(
            int businessType,
            long businessId,
            CancellationToken cancellationToken = default)
        {
            if (businessId <= 0)
                return false;

            try
            {
                await _syncLock.WaitAsync(cancellationToken);

                _logger?.LogDebug("开始同步HasAttachment标志（文件删除）: BusinessType={BusinessType}, BusinessId={BusinessId}",
                    businessType, businessId);

                var db = _unitOfWorkManage.GetDbClient();

                // 根据业务类型确定表名和主键字段
                var tableName = GetTableNameByBusinessType(businessType);
                var idFieldName = GetIdFieldNameByBusinessType(businessType);

                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(idFieldName))
                {
                    return false;
                }

                // 检查表是否有HasAttachment字段
                var hasColumn = db.DbMaintenance.IsAnyColumn(tableName, "HasAttachment");
                if (!hasColumn)
                {
                    return false;
                }

                // 检查是否还有其他关联的文件
                string checkSql = @"
                    SELECT COUNT(*) 
                    FROM tb_FS_BusinessRelation 
                    WHERE BusinessId = @BusinessId 
                    AND BusinessType = @BusinessType 
                    AND IsActive = 1 
                    AND isdeleted = 0";

                int remainingCount = db.Ado.GetInt(checkSql, new
                {
                    BusinessId = businessId,
                    BusinessType = businessType
                });

                // 更新HasAttachment标志
                int affectedRows = db.Updateable<object>()
                    .SetColumns("HasAttachment", (remainingCount > 0 ? 1 : 0))
                    .AS(tableName)
                    .Where($"{idFieldName} = {businessId}")
                    .ExecuteCommand();

                if (affectedRows > 0)
                {
                    _logger?.LogDebug("已更新HasAttachment标志: Table={Table}, BusinessId={BusinessId}, HasAttachment={HasAttachment}",
                        tableName, businessId, remainingCount > 0);
                }

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "同步HasAttachment标志失败（文件删除）: BusinessType={BusinessType}, BusinessId={BusinessId}",
                    businessType, businessId);
                return false;
            }
            finally
            {
                _syncLock.Release();
            }
        }

        /// <summary>
        /// 批量同步HasAttachment标志
        /// 用于初始化或修复数据
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新的记录数</returns>
        public async Task<int> BatchSyncHasAttachmentAsync(
            int businessType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _syncLock.WaitAsync(cancellationToken);

                _logger?.LogDebug("开始批量同步HasAttachment标志: BusinessType={BusinessType}", businessType);

                var db = _unitOfWorkManage.GetDbClient();
                var tableName = GetTableNameByBusinessType(businessType);
                var idFieldName = GetIdFieldNameByBusinessType(businessType);

                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(idFieldName))
                {
                    _logger?.LogWarning("未知的业务类型，跳过批量同步: BusinessType={BusinessType}", businessType);
                    return 0;
                }

                // 检查表是否有HasAttachment字段
                var hasColumn = db.DbMaintenance.IsAnyColumn(tableName, "HasAttachment");
                if (!hasColumn)
                {
                    _logger?.LogDebug("表 {TableName} 没有HasAttachment字段，跳过批量同步", tableName);
                    return 0;
                }

                // 批量更新：有关联的设置为1，无关联的设置为0
                string updateSql = $@"
                    UPDATE {tableName}
                    SET HasAttachment = CASE
                        WHEN EXISTS (
                            SELECT 1 FROM tb_FS_BusinessRelation 
                            WHERE BusinessId = {tableName}.{idFieldName}
                            AND BusinessType = @BusinessType
                            AND IsActive = 1
                            AND isdeleted = 0
                        ) THEN 1 ELSE 0
                    END";

                int affectedRows = db.Ado.ExecuteCommand(updateSql, new { BusinessType = businessType });

                _logger?.LogDebug("批量同步完成: Table={Table}, UpdatedCount={Count}", tableName, affectedRows);

                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量同步HasAttachment标志失败: BusinessType={BusinessType}", businessType);
                return 0;
            }
            finally
            {
                _syncLock.Release();
            }
        }

        #region 私有辅助方法

        /// <summary>
        /// 根据业务类型获取表名
        /// </summary>
        private string GetTableNameByBusinessType(int businessType)
        {
            // TODO: 根据实际业务类型映射表名
            // 这里是示例映射，需要根据实际配置调整
            switch (businessType)
            {
                case 100: // 销售订单
                    return "tb_SaleOrder";
                case 200: // 产品
                    return "tb_Prod";
                case 201: // 产品明细
                    return "tb_ProdDetail";
                case 300: // 费用报销
                    return "tb_FM_ExpenseClaim";
                case 400: // 付款记录
                    return "tb_FM_PaymentRecord";
                default:
                    return null;
            }
        }

        /// <summary>
        /// 根据业务类型获取主键字段名
        /// </summary>
        private string GetIdFieldNameByBusinessType(int businessType)
        {
            // TODO: 根据实际业务类型映射主键字段
            switch (businessType)
            {
                case 100:
                    return "SOrder_ID";
                case 200:
                    return "Prod_ID";
                case 201:
                    return "ProdDetail_ID";
                case 300:
                    return "ExpenseClaim_ID";
                case 400:
                    return "PaymentRecord_ID";
                default:
                    return null;
            }
        }

        #endregion
    }
}
