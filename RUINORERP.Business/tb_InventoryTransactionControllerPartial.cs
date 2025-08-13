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
using RUINORERP.Extensions.Middlewares;
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
    public partial class tb_InventoryTransactionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 记录库存流水
        /// </summary>
        public bool RecordTransaction(tb_InventoryTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            transaction.TransactionTime = DateTime.Now;

            return _unitOfWorkManage.GetDbClient().Insertable(transaction).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量记录库存流水
        /// </summary>
        public bool BatchRecordTransactions(List<tb_InventoryTransaction> transactions)
        {
            if (transactions == null || !transactions.Any())
                return false;

            var now = DateTime.Now;
            foreach (var tran in transactions)
            {
                tran.TransactionTime = now;
            }

            return _unitOfWorkManage.GetDbClient().Insertable(transactions).ExecuteCommand() > 0;
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
        public decimal GetInventoryAtTime(string productId, string warehouseId, DateTime time)
        {
            if (string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));

            if (string.IsNullOrEmpty(warehouseId))
                throw new ArgumentNullException(nameof(warehouseId));

            // 尝试从缓存获取
            var cacheKey = $"InventoryAtTime_{productId}_{warehouseId}_{time:yyyyMMddHHmmss}";
            var cachedValue = _cacheManager.Get(cacheKey);
            if (cachedValue != null && decimal.TryParse(cachedValue.ToString(), out decimal cachedResult))
            {
                return cachedResult;
            }

            // 查询时间点之前的最后一笔交易的库存
            var result = _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>()
                .Where(t => t.ProductId == productId
                         && t.WarehouseId == warehouseId
                         && t.TransactionTime <= time)
                .OrderBy(t => t.TransactionTime, OrderByType.Desc)
                .Take(1)
                .Select(t => t.AfterQuantity)
                .FirstOrDefault();

            // 缓存结果，有效期10分钟
            //_cacheManager.Add(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }

        /// <summary>
        /// 获取商品的库存流水记录
        /// </summary>
        public List<tb_InventoryTransaction> GetProductTransactions(string productId, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrEmpty(productId))
                throw new ArgumentNullException(nameof(productId));

            return _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>()
                .Where(t => t.ProductId == productId
                         && t.TransactionTime >= startTime
                         && t.TransactionTime <= endTime)
                .OrderBy(t => t.TransactionTime)
                .ToList();
        }





    }
}



