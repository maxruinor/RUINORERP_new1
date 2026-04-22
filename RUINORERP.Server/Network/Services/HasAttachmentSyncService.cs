using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// HasAttachment标志同步服务
    /// 负责在文件上传、删除、更新时同步业务表的HasAttachment标志位
    /// 支持事务处理，确保数据一致性
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
        /// 文件上传后同步HasAttachment标志（支持事务）
        /// </summary>
        /// <param name="OwnerTableName">业务类型</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="useTransaction">是否使用事务（默认true）</param>
        /// <returns>是否同步成功</returns>
        public async Task<bool> SyncOnFileUploadAsync(
            string OwnerTableName,
            long businessId,
            string businessNo,
            CancellationToken cancellationToken = default,
            bool useTransaction = true)
        {
            if (businessId <= 0)
                return false;

            try
            {
                await _syncLock.WaitAsync(cancellationToken);


                var db = _unitOfWorkManage.GetDbClient();

                // 根据业务类型确定表名和主键字段
                var cols = db.DbMaintenance.GetColumnInfosByTableName(OwnerTableName);
                var idFieldName = cols.FirstOrDefault(c => c.IsPrimarykey)?.DbColumnName;

                if (string.IsNullOrEmpty(OwnerTableName) || string.IsNullOrEmpty(idFieldName))
                {
                    _logger?.LogWarning("未知的业务类型，跳过HasAttachment同步: OwnerTableName={OwnerTableName}", OwnerTableName);
                    return false;
                }

                // 检查表是否有HasAttachment字段
                var hasColumn = db.DbMaintenance.IsAnyColumn(OwnerTableName, "HasAttachment");
                if (!hasColumn)
                {
                    _logger?.LogDebug("表 {TableName} 没有HasAttachment字段，跳过同步", OwnerTableName);
                    return false;
                }

                // 事务处理
                if (useTransaction)
                {
                    _unitOfWorkManage.BeginTran();
                }

                try
                {
                    // 更新HasAttachment标志为true
                    string updateSql = $"UPDATE {OwnerTableName} SET HasAttachment = 1 WHERE {idFieldName} = @BusinessId";
                    int affectedRows = await db.Ado.ExecuteCommandAsync(updateSql, new { BusinessId = businessId });

                    if (affectedRows > 0)
                    {
                        _logger?.LogDebug("已更新HasAttachment标志: Table={Table}, BusinessId={BusinessId}",
                            OwnerTableName, businessId);
                    }

                    if (useTransaction)
                    {
                        _unitOfWorkManage.CommitTran();
                    }

                    return affectedRows > 0;
                }
                catch (Exception)
                {
                    if (useTransaction && _unitOfWorkManage.GetDbClient().Ado.Transaction != null)
                    {
                        _unitOfWorkManage.RollbackTran();
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "同步HasAttachment标志失败（文件上传）: OwnerTableName={OwnerTableName}, BusinessId={BusinessId}",
                    OwnerTableName, businessId);
                return false;
            }
            finally
            {
                _syncLock.Release();
            }
        }


        public static string GetPrimaryKeyColName(Type type)
        {
            string PrimaryKeyColName = string.Empty;
            foreach (PropertyInfo field in type.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {

                    if (attr is SugarColumn sugarColumnAttr)
                    {

                        if (sugarColumnAttr.IsPrimaryKey)
                        {
                            PrimaryKeyColName = sugarColumnAttr.ColumnName;
                            break;
                        }

                    }
                }
            }
            return PrimaryKeyColName;
        }


        /// <summary>
        /// 文件删除后同步HasAttachment标志（支持事务）
        /// </summary>
        /// <param name="OwnerTableName">业务类型</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="useTransaction">是否使用事务（默认true）</param>
        /// <returns>是否同步成功</returns>
        public async Task<bool> SyncOnFileDeleteAsync(
            string OwnerTableName,
            long businessId,
            CancellationToken cancellationToken = default,
            bool useTransaction = true)
        {
            if (businessId <= 0)
                return false;

            try
            {
                await _syncLock.WaitAsync(cancellationToken);

                _logger?.LogDebug("开始同步HasAttachment标志（文件删除）: OwnerTableName={OwnerTableName}, BusinessId={BusinessId}",
                    OwnerTableName, businessId);

                var db = _unitOfWorkManage.GetDbClient();

                // 根据业务类型确定表名和主键字段
                var tableName = OwnerTableName;


                var cols = db.DbMaintenance.GetColumnInfosByTableName(OwnerTableName);
                var idFieldName = cols.FirstOrDefault(c => c.IsPrimarykey)?.DbColumnName;

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

                // 事务处理
                if (useTransaction)
                {
                    _unitOfWorkManage.BeginTran();
                }

                try
                {
                    // 检查是否还有其他关联的文件
                    string checkSql = @"
                        SELECT COUNT(*) 
                        FROM tb_FS_BusinessRelation 
                        WHERE BusinessId = @BusinessId 
                        AND OwnerTableName = @OwnerTableName 
                        AND IsActive = 1 
                        AND isdeleted = 0";

                    int remainingCount = await db.Ado.GetIntAsync(checkSql, new
                    {
                        BusinessId = businessId,
                        OwnerTableName = OwnerTableName
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

                    if (useTransaction)
                    {
                        _unitOfWorkManage.CommitTran();
                    }

                    return affectedRows > 0;
                }
                catch (Exception)
                {
                    if (useTransaction && _unitOfWorkManage.GetDbClient().Ado.Transaction != null)
                    {
                        _unitOfWorkManage.RollbackTran();
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "同步HasAttachment标志失败（文件删除）: OwnerTableName={OwnerTableName}, BusinessId={BusinessId}",
                    OwnerTableName, businessId);
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
        /// <param name="OwnerTableName">业务类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新的记录数</returns>
        public async Task<int> BatchSyncHasAttachmentAsync(
            string OwnerTableName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _syncLock.WaitAsync(cancellationToken);

                _logger?.LogDebug("开始批量同步HasAttachment标志: OwnerTableName={OwnerTableName}", OwnerTableName);

                var db = _unitOfWorkManage.GetDbClient();
                var tableName = OwnerTableName;
                var cols = db.DbMaintenance.GetColumnInfosByTableName(OwnerTableName);
                var idFieldName = cols.FirstOrDefault(c => c.IsPrimarykey)?.DbColumnName;

                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(idFieldName))
                {
                    _logger?.LogWarning("未知的业务类型，跳过批量同步: OwnerTableName={OwnerTableName}", OwnerTableName);
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
                            AND OwnerTableName = @OwnerTableName
                            AND IsActive = 1
                            AND isdeleted = 0
                        ) THEN 1 ELSE 0
                    END";

                int affectedRows = await db.Ado.ExecuteCommandAsync(updateSql, new { OwnerTableName = OwnerTableName });

                _logger?.LogDebug("批量同步完成: Table={Table}, UpdatedCount={Count}", tableName, affectedRows);

                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量同步HasAttachment标志失败: OwnerTableName={OwnerTableName}", OwnerTableName);
                return 0;
            }
            finally
            {
                _syncLock.Release();
            }
        }

        /// <summary>
        /// 批量同步指定业务ID的HasAttachment标志
        /// 用于修复指定业务数据
        /// </summary>
        /// <param name="OwnerTableName">业务类型</param>
        /// <param name="businessIds">业务ID列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="useTransaction">是否使用事务</param>
        /// <returns>更新的记录数</returns>
        public async Task<int> BatchSyncByBusinessIdsAsync(
            string OwnerTableName,
            IEnumerable<long> businessIds,
            CancellationToken cancellationToken = default,
            bool useTransaction = true)
        {
            if (businessIds == null || !businessIds.Any())
                return 0;

            try
            {
                await _syncLock.WaitAsync(cancellationToken);

                _logger?.LogDebug("开始批量同步指定业务ID的HasAttachment标志: OwnerTableName={OwnerTableName}, Count={Count}",
                    OwnerTableName, businessIds.Count());

                var db = _unitOfWorkManage.GetDbClient();
                var tableName = OwnerTableName;
                var cols = db.DbMaintenance.GetColumnInfosByTableName(OwnerTableName);
                var idFieldName = cols.FirstOrDefault(c => c.IsPrimarykey)?.DbColumnName;

                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(idFieldName))
                {
                    _logger?.LogWarning("未知的业务类型，跳过批量同步: OwnerTableName={OwnerTableName}", OwnerTableName);
                    return 0;
                }

                // 检查表是否有HasAttachment字段
                var hasColumn = db.DbMaintenance.IsAnyColumn(tableName, "HasAttachment");
                if (!hasColumn)
                {
                    _logger?.LogDebug("表 {TableName} 没有HasAttachment字段，跳过批量同步", tableName);
                    return 0;
                }

                // 事务处理
                if (useTransaction)
                {
                    _unitOfWorkManage.BeginTran();
                }

                try
                {
                    var affectedRows = 0;
                    foreach (var businessId in businessIds)
                    {
                        // 检查是否有关联的文件
                        string checkSql = @"
                            SELECT COUNT(*) 
                            FROM tb_FS_BusinessRelation 
                            WHERE BusinessId = @BusinessId 
                            AND OwnerTableName = @OwnerTableName 
                            AND IsActive = 1 
                            AND isdeleted = 0";

                        int count = await db.Ado.GetIntAsync(checkSql, new
                        {
                            BusinessId = businessId,
                            OwnerTableName = OwnerTableName
                        });

                        // 更新HasAttachment标志
                        int rows = db.Updateable<object>()
                            .SetColumns("HasAttachment", (count > 0 ? 1 : 0))
                            .AS(tableName)
                            .Where($"{idFieldName} = {businessId}")
                            .ExecuteCommand();

                        affectedRows += rows;
                    }

                    if (useTransaction)
                    {
                        _unitOfWorkManage.CommitTran();
                    }

                    _logger?.LogDebug("批量同步指定业务ID完成: Table={Table}, UpdatedCount={Count}", tableName, affectedRows);
                    return affectedRows;
                }
                catch (Exception)
                {
                    if (useTransaction && _unitOfWorkManage.GetDbClient().Ado.Transaction != null)
                    {
                        _unitOfWorkManage.RollbackTran();
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量同步指定业务ID的HasAttachment标志失败: OwnerTableName={OwnerTableName}", OwnerTableName);
                return 0;
            }
            finally
            {
                _syncLock.Release();
            }
        }

        /// <summary>
        /// 手动触发同步单个业务实体的HasAttachment标志
        /// 用于数据修复和手动同步
        /// </summary>
        /// <param name="OwnerTableName">业务类型</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>同步结果（是否有附件）</returns>
        public async Task<bool?> ManualSyncAsync(
            string OwnerTableName,
            long businessId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();

                var tableName = OwnerTableName;
                var cols = db.DbMaintenance.GetColumnInfosByTableName(OwnerTableName);
                var idFieldName = cols.FirstOrDefault(c => c.IsPrimarykey)?.DbColumnName;


                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(idFieldName))
                {
                    _logger?.LogWarning("未知的业务类型，跳过手动同步: OwnerTableName={OwnerTableName}", OwnerTableName);
                    return null;
                }

                // 检查表是否有HasAttachment字段
                var hasColumn = db.DbMaintenance.IsAnyColumn(tableName, "HasAttachment");
                if (!hasColumn)
                {
                    _logger?.LogDebug("表 {TableName} 没有HasAttachment字段，跳过手动同步", tableName);
                    return null;
                }

                // 检查是否有关联的文件
                string checkSql = @"
                    SELECT COUNT(*) 
                    FROM tb_FS_BusinessRelation 
                    WHERE BusinessId = @BusinessId 
                    AND OwnerTableName = @OwnerTableName 
                    AND IsActive = 1 
                    AND isdeleted = 0";

                int count = await db.Ado.GetIntAsync(checkSql, new
                {
                    BusinessId = businessId,
                    OwnerTableName = OwnerTableName
                });

                // 更新HasAttachment标志
                int affectedRows = db.Updateable<object>()
                    .SetColumns("HasAttachment", (count > 0 ? 1 : 0))
                    .AS(tableName)
                    .Where($"{idFieldName} = {businessId}")
                    .ExecuteCommand();

                if (affectedRows > 0)
                {
                    _logger?.LogDebug("手动同步成功: Table={Table}, BusinessId={BusinessId}, HasAttachment={HasAttachment}",
                        tableName, businessId, count > 0);
                }

                return count > 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "手动同步HasAttachment标志失败: OwnerTableName={OwnerTableName}, BusinessId={BusinessId}",
                    OwnerTableName, businessId);
                return null;
            }
        }

        #region 私有辅助方法




        #endregion
    }
}
