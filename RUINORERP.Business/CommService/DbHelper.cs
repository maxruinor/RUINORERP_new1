using Microsoft.Extensions.Logging;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 数据库操作辅助类1
    /// 提供批量新增、更新等核心数据操作功能
    /// 支持事务管理、并发控制、批量处理等企业级特性
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class DbHelper<T> where T : class, new()
    {
        #region 字段与属性

        /// <summary>
        /// 应用程序上下文
        /// </summary>
        public ApplicationContext _appContext;

        /// <summary>
        /// 工作单元管理（暴露出来方便使用）
        /// </summary>
        public IUnitOfWorkManage _unitOfWorkManage;

        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger<DbHelper<T>> _logger;

        /// <summary>
        /// 业务类型文本
        /// </summary>
        public string BizTypeText { get; set; }

        /// <summary>
        /// 业务类型整数
        /// </summary>
        public int BizTypeInt { get; set; }

        /// <summary>
        /// 批量操作的最大批次大小（默认1000）
        /// 仅用于新的增强方法
        /// </summary>
        public int MaxBatchSize { get; set; } = 1000;

        /// <summary>
        /// 操作前验证委托
        /// 仅用于新的增强方法
        /// </summary>
        public Func<List<T>, Task<bool>> BeforeSaveValidateAsync { get; set; }

        /// <summary>
        /// 操作后处理委托
        /// 仅用于新的增强方法
        /// </summary>
        public Func<List<T>, Task> AfterSaveAsync { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="unitOfWorkManage">工作单元管理</param>
        /// <param name="appContext">应用程序上下文</param>
        public DbHelper(ILogger<DbHelper<T>> logger, IUnitOfWorkManage unitOfWorkManage, ApplicationContext appContext = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _appContext = appContext;

            BizType bizType = BizMapperService.EntityMappingHelper.GetBizType(typeof(T));
            BizTypeText = bizType.ToString();
            BizTypeInt = (int)bizType;
        }

        #endregion

        #region 原有方法 - 保持完全不变，确保向后兼容

        /// <summary>
        /// 批量新增或保存
        /// 新增时会用雪花ID
        /// 依赖于外层事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> BaseAddOrUpdateAsync(List<T> list)
        {
            var x = _unitOfWorkManage.GetDbClient().Storageable(list).ToStorage();
            await x.AsUpdateable.ExecuteCommandAsync();
            await x.AsInsertable.ExecuteReturnSnowflakeIdListAsync();
            return list;
        }

        /// <summary>
        /// 批量新增或保存
        /// 新增时会用雪花ID
        /// 依赖于外层事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual async Task<long> BaseDefaultAddElseUpdateAsync(List<T> list)
        {
            long counter = 0;
            List<long> ids = new List<long>();
            var x = await _unitOfWorkManage.GetDbClient().Storageable<T>(list).ToStorageAsync();
            ids = await x.AsInsertable.ExecuteReturnSnowflakeIdListAsync();
            counter += await x.AsUpdateable.ExecuteCommandAsync();
            return counter + ids.Count;
        }

        /// <summary>
        /// 批量新增或保存
        /// 新增时会用雪花ID
        /// 单个实体也可以
        /// 事务依赖于外层
        /// </summary>
        /// <param name="paralist"></param>
        /// <returns></returns>
        public virtual async Task<long> BaseDefaultAddElseUpdateAsync(params T[] paralist)
        {
            List<T> list = new List<T>();
            foreach (var item in paralist)
            {
                list.Add(item);
            }
            return await BaseDefaultAddElseUpdateAsync(list);
        }

        #endregion

        #region 新增增强方法 - 提供更安全的操作

        /// <summary>
        /// 批量新增或更新（增强版，返回实体列表）
        /// 新增时会自动生成雪花ID
        /// 依赖外层事务，提供日志记录和可选的验证钩子
        /// 注意：此方法不会自动标记事务回滚，由外层调用者控制
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>处理后的实体列表</returns>
        /// <exception cref="ArgumentNullException">当entities为null时抛出</exception>
        public virtual async Task<List<T>> BatchAddOrUpdateAsync(List<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.Count == 0)
                return entities;

            var operationId = Guid.NewGuid().ToString("N");
            _logger.LogInformation("[DbHelper-{OperationId}] 开始批量增删改操作，实体类型: {EntityType}, 数量: {Count}",
                operationId, typeof(T).Name, entities.Count);

            try
            {
                ValidateTransactionContext();

                if (!await ValidateBeforeSaveAsync(entities))
                {
                    throw new InvalidOperationException("批量操作前置验证失败");
                }

                var result = await ExecuteBatchAddOrUpdateInternalAsync(entities, operationId);

                await ExecuteAfterSaveAsync(entities);

                _logger.LogInformation("[DbHelper-{OperationId}] 批量增删改操作完成", operationId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[DbHelper-{OperationId}] 批量增删改操作失败", operationId);
                throw;
            }
        }

        /// <summary>
        /// 批量新增或更新（增强版，返回操作数量）
        /// 新增时会自动生成雪花ID
        /// 依赖外层事务，提供日志记录和可选的验证钩子
        /// 注意：此方法不会自动标记事务回滚，由外层调用者控制
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>操作影响的总行数（新增数+更新数）</returns>
        /// <exception cref="ArgumentNullException">当entities为null时抛出</exception>
        public virtual async Task<long> BatchAddOrUpdateWithCountAsync(List<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.Count == 0)
                return 0;

            var operationId = Guid.NewGuid().ToString("N");
            _logger.LogInformation("[DbHelper-{OperationId}] 开始批量增删改操作（带计数），实体类型: {EntityType}, 数量: {Count}",
                operationId, typeof(T).Name, entities.Count);

            try
            {
                ValidateTransactionContext();

                if (!await ValidateBeforeSaveAsync(entities))
                {
                    throw new InvalidOperationException("批量操作前置验证失败");
                }

                var result = await ExecuteBatchAddOrUpdateWithCountInternalAsync(entities, operationId);

                await ExecuteAfterSaveAsync(entities);

                _logger.LogInformation("[DbHelper-{OperationId}] 批量增删改操作完成，影响行数: {Count}", operationId, result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[DbHelper-{OperationId}] 批量增删改操作（带计数）失败", operationId);
                throw;
            }
        }

        /// <summary>
        /// 批量新增或更新（增强版，参数数组形式）
        /// 新增时会自动生成雪花ID
        /// 依赖外层事务，提供日志记录和可选的验证钩子
        /// </summary>
        /// <param name="entities">实体数组</param>
        /// <returns>操作影响的总行数（新增数+更新数）</returns>
        public virtual async Task<long> BatchAddOrUpdateWithCountAsync(params T[] entities)
        {
            if (entities == null || entities.Length == 0)
                return 0;

            return await BatchAddOrUpdateWithCountAsync(entities.ToList());
        }

        #endregion

        #region 公共方法 - 事务辅助

        /// <summary>
        /// 检查是否在事务上下文中
        /// </summary>
        /// <returns>是否在事务中</returns>
        public bool IsInTransaction()
        {
            var state = _unitOfWorkManage.GetTransactionState();
            return state.IsActive;
        }

        /// <summary>
        /// 获取当前事务状态
        /// </summary>
        /// <returns>事务状态信息</returns>
        public string GetTransactionStatus()
        {
            var state = _unitOfWorkManage.GetTransactionState();
            return state.ToString();
        }

        #endregion

        #region 私有方法 - 核心实现

        /// <summary>
        /// 验证事务上下文
        /// </summary>
        private void ValidateTransactionContext()
        {
            var state = _unitOfWorkManage.GetTransactionState();
            if (!state.IsActive)
            {
                _logger.LogWarning("DbHelper操作未在事务上下文中执行，数据一致性将依赖数据库自动提交");
            }
        }

        /// <summary>
        /// 执行保存前验证
        /// </summary>
        private async Task<bool> ValidateBeforeSaveAsync(List<T> entities)
        {
            if (BeforeSaveValidateAsync != null)
            {
                return await BeforeSaveValidateAsync(entities);
            }
            return true;
        }

        /// <summary>
        /// 执行保存后处理
        /// </summary>
        private async Task ExecuteAfterSaveAsync(List<T> entities)
        {
            if (AfterSaveAsync != null)
            {
                await AfterSaveAsync(entities);
            }
        }

        /// <summary>
        /// 执行批量增删改内部实现（返回实体列表）
        /// </summary>
        private async Task<List<T>> ExecuteBatchAddOrUpdateInternalAsync(List<T> entities, string operationId)
        {
            var dbClient = _unitOfWorkManage.GetDbClient();
            var totalCount = entities.Count;
            var processed = 0;

            for (int i = 0; i < totalCount; i += MaxBatchSize)
            {
                var batchList = entities.GetRange(i, Math.Min(MaxBatchSize, totalCount - i));
                var batchSize = batchList.Count;
                _logger.LogDebug("[DbHelper-{OperationId}] 处理批次 {Processed}/{Total}, 批次大小: {BatchSize}",
                    operationId, processed, totalCount, batchSize);

                var storage = await dbClient.Storageable<T>(batchList).ToStorageAsync();
                
                await storage.AsInsertable.ExecuteReturnSnowflakeIdListAsync();
                await storage.AsUpdateable.ExecuteCommandAsync();

                processed += batchSize;
            }

            return entities;
        }

        /// <summary>
        /// 执行批量增删改内部实现（返回计数）
        /// </summary>
        private async Task<long> ExecuteBatchAddOrUpdateWithCountInternalAsync(List<T> entities, string operationId)
        {
            var dbClient = _unitOfWorkManage.GetDbClient();
            long totalAffected = 0;
            var totalCount = entities.Count;
            var processed = 0;

            for (int i = 0; i < totalCount; i += MaxBatchSize)
            {
                var batchList = entities.GetRange(i, Math.Min(MaxBatchSize, totalCount - i));
                var batchSize = batchList.Count;
                _logger.LogDebug("[DbHelper-{OperationId}] 处理批次 {Processed}/{Total}, 批次大小: {BatchSize}",
                    operationId, processed, totalCount, batchSize);

                var storage = await dbClient.Storageable<T>(batchList).ToStorageAsync();
                
                var insertIds = await storage.AsInsertable.ExecuteReturnSnowflakeIdListAsync();
                var updateCount = await storage.AsUpdateable.ExecuteCommandAsync();

                totalAffected += insertIds.Count + updateCount;
                processed += batchSize;

                _logger.LogDebug("[DbHelper-{OperationId}] 批次完成，新增: {InsertCount}, 更新: {UpdateCount}",
                    operationId, insertIds.Count, updateCount);
            }

            return totalAffected;
        }

        #endregion
    }

    /// <summary>
    /// 批量操作结果
    /// </summary>
    public class BatchOperationResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 新增数量
        /// </summary>
        public int InsertCount { get; set; }

        /// <summary>
        /// 更新数量
        /// </summary>
        public int UpdateCount { get; set; }

        /// <summary>
        /// 总影响行数
        /// </summary>
        public int TotalAffected => InsertCount + UpdateCount;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 操作耗时（毫秒）
        /// </summary>
        public long DurationMs { get; set; }
    }
}
