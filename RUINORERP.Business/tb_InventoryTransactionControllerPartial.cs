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
    /// 库存流水表
    /// </summary>
    public partial class tb_InventoryTransactionController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 记录库存流水
        /// </summary>
        public bool RecordTransaction(tb_InventoryTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            // 确保只有实际库存变化才记录流水
            if (transaction.QuantityChange == 0)
                return false;

            // 设置交易时间
            transaction.TransactionTime = DateTime.Now;

            // 执行插入操作
            return _unitOfWorkManage.GetDbClient().Insertable(transaction).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量记录库存流水
        /// </summary>
        public async Task<bool> BatchRecordTransactions(List<tb_InventoryTransaction> transactions)
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

            // 执行批量插入操作
            var rs = await _unitOfWorkManage.GetDbClient().Insertable(validTransactions).ExecuteReturnSnowflakeIdListAsync();
            return rs.Count > 0;
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



