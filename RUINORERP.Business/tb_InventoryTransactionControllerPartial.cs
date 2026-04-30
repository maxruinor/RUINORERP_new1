// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:06
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using SqlSugar;

namespace RUINORERP.Business
{
    /// <summary>
    /// 库存流水表1
    /// </summary>
    public partial class tb_InventoryTransactionController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 记录库存流水
        /// </summary>
        /// <param name="transaction">库存流水实体</param>
        /// <param name="useTransaction">是否使用当前事务（默认true）</param>
        /// <returns>是否成功</returns>
        public async Task<bool> RecordTransaction(tb_InventoryTransaction transaction, bool useTransaction = true)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            // 确保只有实际库存变化才记录流水
            if (transaction.QuantityChange == 0)
                return false;

            // 设置交易时间
            transaction.TransactionTime = DateTime.Now;

            // 获取数据库客户端
            var dbClient = _unitOfWorkManage.GetDbClient();

            // 检查当前事务状态（使用TranCount判断是否在事务中）
            bool inTransaction = _unitOfWorkManage.TranCount > 0;

            // 如果要求使用事务但当前没有事务，则开启新事务
            if (useTransaction && !inTransaction)
            {
                await _unitOfWorkManage.BeginTranAsync();
                try
                {
                    var result = await dbClient.Insertable(transaction).ExecuteCommandAsync();
                    await _unitOfWorkManage.CommitTranAsync();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    await _unitOfWorkManage.RollbackTranAsync();
                    throw new Exception($"插入库存流水失败（独立事务），TranID：{transaction.TranID}，错误：{ex.Message}", ex);
                }
            }
            else
            {
                // 在当前事务中执行，或直接执行（不使用事务）
                var result = await dbClient.Insertable(transaction).ExecuteCommandAsync();
                return result > 0;
            }
        }

        /// <summary>
        /// 批量记录库存流水
        /// </summary>
        /// <param name="transactions">库存流水列表</param>
        /// <param name="useTransaction">是否使用当前事务（默认true）</param>
        /// <returns>是否成功</returns>
        public async Task<bool> BatchRecordTransactions(List<tb_InventoryTransaction> transactions, bool useTransaction = true)
        {
            if (transactions == null || !transactions.Any())
                return false;

            // 过滤掉没有实际库存变化的记录
            var validTransactions = transactions.Where(t => t.QuantityChange != 0).ToList();
            if (!validTransactions.Any())
                return false;

            // 设置交易时间
            var now = DateTime.Now;
            foreach (var tran in validTransactions)
            {
                tran.TransactionTime = now;
            }

            // ✅ P0修复：强制使用当前事务，禁止嵌套事务
            // 如果调用方没有开启事务，则直接插入（不使用事务）
            var dbClient = _unitOfWorkManage.GetDbClient();
            bool inTransaction = _unitOfWorkManage.TranCount > 0;
            
            if (!inTransaction)
            {
                // ⚠️ 警告：不在事务中执行批量插入
                _logger.LogWarning($"[BatchRecordTransactions] 警告：当前不在事务中，直接插入 {validTransactions.Count} 条库存流水记录");
                var rs = await dbClient.Insertable(validTransactions)
                    .ExecuteReturnSnowflakeIdListAsync();
                return rs.Count > 0;
            }
            else
            {
                // ✅ 在当前事务中执行（推荐方式）
                var rs = await dbClient.Insertable(validTransactions)
                    .ExecuteReturnSnowflakeIdListAsync();
                return rs.Count > 0;
            }
        }

        /// <summary>
        /// 批量记录库存流水（带死锁重试机制）
        /// </summary>
        /// <param name="transactions">库存流水列表</param>
        /// <param name="useTransaction">是否使用当前事务（默认true）</param>
        /// <param name="maxRetries">最大重试次数（默认3）</param>
        /// <param name="initialRetryDelayMs">初始重试延迟（毫秒，默认100）</param>
        /// <returns>是否成功</returns>
        public async Task<bool> BatchRecordTransactionsWithRetry(
            List<tb_InventoryTransaction> transactions,
            bool useTransaction = true,
            int maxRetries = 3,
            int initialRetryDelayMs = 100)
        {
            int retryCount = 0;
            int retryDelayMs = initialRetryDelayMs;

            while (retryCount < maxRetries)
            {
                try
                {
                    return await BatchRecordTransactions(transactions, useTransaction);
                }
                catch (Exception ex)
                {
                    retryCount++;

                    // 检查是否为死锁或锁超时错误
                    bool isLockError = ex.Message.Contains("deadlock") ||
                                      ex.Message.Contains("1205") || // MySQL锁超时
                                      ex.Message.Contains("锁") ||
                                      ex.Message.Contains("lock") ||
                                      ex.Message.Contains("timeout") ||
                                      ex.Message.Contains("deadlocked");

                    if (!isLockError || retryCount >= maxRetries)
                    {
                        // 不是锁错误或已达到最大重试次数，重新抛出异常
                        throw new Exception(
                            $"批量插入库存流水失败，重试次数：{retryCount}/{maxRetries}，" +
                            $"记录数：{transactions?.Count ?? 0}，错误：{ex.Message}", ex);
                    }

                    // 等待后重试（指数退避）
                    Console.WriteLine(
                        $"检测到锁冲突，第{retryCount}次重试，等待{retryDelayMs}ms，" +
                        $"记录数：{transactions?.Count ?? 0}，错误：{ex.Message}");
                    await Task.Delay(retryDelayMs);

                    // 指数退避：每次延迟时间翻倍
                    retryDelayMs = Math.Min(retryDelayMs * 2, 2000); // 最大2秒
                }
            }

            return false;
        }

        /*
        /// <summary>
        /// 查询库存流水
        /// </summary>
        public PageResult<tb_InventoryTransaction> QueryTransactions(tb_InventoryTransactionQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryable = _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>()
                .WhereIF(!string.IsNullOrEmpty(query.ProductId), t => t.ProductId == query.ProductId)
                .WhereIF(!string.IsNullOrEmpty(query.WarehouseId), t => t.WarehouseId == query.WarehouseId)
                .WhereIF(query.TransactionType.HasValue, t => t.TransactionType == query.TransactionType)
                .WhereIF(query.ReferenceType.HasValue, t => t.ReferenceType == query.ReferenceType)
                .WhereIF(!string.IsNullOrEmpty(query.ReferenceId), t => t.ReferenceId == query.ReferenceId)
                .WhereIF(query.StartTime.HasValue, t => t.TransactionTime >= query.StartTime)
                .WhereIF(query.EndTime.HasValue, t => t.TransactionTime <= query.EndTime)
                .OrderBy(t => t.TransactionTime, OrderByType.Desc);

            var totalCount = queryable.Count();
            var items = queryable.Skip((query.PageIndex - 1) * query.PageSize)
                                 .Take(query.PageSize)
                                 .ToList();

            return new PageResult<tb_InventoryTransaction>
            {
                TotalCount = totalCount,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                Items = items
            };
        }
        */
        /// <summary>
        /// 获取指定时间点的库存
        /// </summary>
        public async Task<decimal> GetInventoryAtTime(long productId, long Location_ID, DateTime time)
        {
            // 查询时间点之前的最后一笔交易的库存
            var result = await _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>()
                .Where(t => t.ProdDetailID == productId
                         && t.Location_ID == Location_ID
                         && t.TransactionTime <= time)
                .OrderBy(t => t.TransactionTime, OrderByType.Desc)
                .Take(1)
                .Select(t => t.AfterQuantity)
                .FirstAsync();

            // 缓存结果，有效期10分钟
            //_cacheManager.Add(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        /// <summary>
        /// 获取商品的库存流水记录
        /// </summary>
        public List<tb_InventoryTransaction> GetProductTransactions(long productId, DateTime startTime, DateTime endTime)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>()
                .Where(t => t.ProdDetailID == productId
                         && t.TransactionTime >= startTime
                         && t.TransactionTime <= endTime)
                .OrderBy(t => t.TransactionTime)
                .ToList();
        }





    }
}



