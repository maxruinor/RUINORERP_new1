// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:04
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
using AutoMapper;

namespace RUINORERP.Business
{
    /// <summary>
    /// 库存快照表
    /// </summary>
    public partial class tb_InventorySnapshotController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 获取商品在指定日期的库存快照（适配无快照日）
        /// </summary>
        public async Task<tb_InventorySnapshot> GetProductSnapshotOnDate(long productId, long Location_ID, DateTime date)
        {


            // 目标日期的结束时间
            var targetDateEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            // 1. 优先查询目标日期当天的快照
            var targetSnapshot =await  _unitOfWorkManage.GetDbClient().Queryable<tb_InventorySnapshot>()
                .Where(s => s.ProdDetailID == productId
                         && s.Location_ID == Location_ID
                         && s.SnapshotTime <= targetDateEnd
                         && s.SnapshotTime >= date.Date)
                   .FirstAsync();

            if (targetSnapshot != null)
                return targetSnapshot;

            // 2. 若当天无快照，查询最近的上一次快照
            return await _unitOfWorkManage.GetDbClient().Queryable<tb_InventorySnapshot>()
                .Where(s => s.ProdDetailID == productId
                         && s.Location_ID == Location_ID
                         && s.SnapshotTime <= targetDateEnd)
                .OrderBy(s => s.SnapshotTime, OrderByType.Desc)
                .Take(1)
                .FirstAsync();
        }


        


        /// <summary>
        /// 批量查询指定日期范围的快照（适配无快照日）
        /// </summary>
        public async Task<List<tb_InventorySnapshot>> GetSnapshotsInRange(long ProdDetailID, long Location_ID,
                                                          DateTime startDate, DateTime endDate)
        {
            // 1. 查询范围内所有快照
            var snapshotsInRange = await _unitOfWorkManage.GetDbClient().Queryable<tb_InventorySnapshot>()
                .Where(s => s.ProdDetailID == ProdDetailID
                         && s.Location_ID == Location_ID
                         && s.SnapshotTime >= startDate.Date
                         && s.SnapshotTime <= endDate.Date.AddDays(1).AddTicks(-1))
                .OrderBy(s => s.SnapshotTime)
                .ToListAsync();

            // 2. 补全缺失日期（用最近的快照填充）
            var result = new List<tb_InventorySnapshot>();
            var currentDate = startDate.Date;
            var lastValidSnapshot = (tb_InventorySnapshot)null;

            while (currentDate <= endDate.Date)
            {
                // 查找当前日期的快照
                var dailySnapshot = snapshotsInRange
                    .FirstOrDefault(s => s.SnapshotTime.Value.Date == currentDate);

                if (dailySnapshot != null)
                {
                    result.Add(dailySnapshot);
                    lastValidSnapshot = dailySnapshot;
                }
                else if (lastValidSnapshot != null)
                {
                    // 用最近的快照填充（复制一份，修改时间为当前日期）
                    var filledSnapshot = new tb_InventorySnapshot
                    {
                        SnapshotID = 0, // 新增记录ID通常由数据库自动生成，设为0或不赋值
                        ProdDetailID = lastValidSnapshot.ProdDetailID,
                        Location_ID = lastValidSnapshot.Location_ID,
                        Quantity = lastValidSnapshot.Quantity,
                        InitInventory = lastValidSnapshot.InitInventory,
                        Rack_ID = lastValidSnapshot.Rack_ID,
                        On_the_way_Qty = lastValidSnapshot.On_the_way_Qty,
                        Sale_Qty = lastValidSnapshot.Sale_Qty,
                        MakingQty = lastValidSnapshot.MakingQty,
                        NotOutQty = lastValidSnapshot.NotOutQty,
                        CostFIFO = lastValidSnapshot.CostFIFO,
                        CostMonthlyWA = lastValidSnapshot.CostMonthlyWA,
                        CostMovingWA = lastValidSnapshot.CostMovingWA,
                        Inv_AdvCost = lastValidSnapshot.Inv_AdvCost,
                        Inv_Cost = lastValidSnapshot.Inv_Cost,
                        Inv_SubtotalCostMoney = lastValidSnapshot.Inv_SubtotalCostMoney,
                        LatestOutboundTime = lastValidSnapshot.LatestOutboundTime,
                        LatestStorageTime = lastValidSnapshot.LatestStorageTime,
                        LastInventoryDate = lastValidSnapshot.LastInventoryDate,
                        SnapshotTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59),
                        Notes = $"自动填充：数据沿用{lastValidSnapshot.SnapshotTime:yyyy-MM-dd}的快照" // 补充说明便于追溯
                    };

                    result.Add(filledSnapshot);
                }

                currentDate = currentDate.AddDays(1);
            }

            return result;
        }



        /// <summary>
        /// 生成当日快照（仅当库存有变化时）
        /// </summary>
        public async Task<int> GenerateDailySnapshot()
        {
            var snapshotTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            return await GenerateDifferentialSnapshot(snapshotTime);
        }

        /// <summary>
        /// 差异化生成快照（仅包含有变化的库存）
        /// </summary>
        private async Task<int> GenerateDifferentialSnapshot(DateTime snapshotTime)
        {
            var invControl = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

            // 获取当前所有库存数据
            List<tb_Inventory> currentInventories = await invControl.QueryAsync();

            if (!currentInventories.Any())
                return 0;

            // 2. 获取每个商品-仓库的上一次快照时间
            var lastSnapshotTimes = GetLastSnapshotTimes(currentInventories);

            // 3. 筛选出需要生成快照的库存（有变动的）
            var needSnapshotInventories = currentInventories.Where(inv =>
            {
                var key = $"{inv.ProdDetailID}_{inv.Location_ID}";
                var lastSnapshotTime = lastSnapshotTimes.TryGetValue(key, out var time) ? time : (DateTime?)null;

                // 首次快照：无历史快照，必须生成
                if (!lastSnapshotTime.HasValue)
                    return true;

                // 强制生成：超过30天无快照，即使无变动也生成
                if (snapshotTime - lastSnapshotTime.Value > TimeSpan.FromDays(30))
                    return true;

                // 有变动：最近出入库时间晚于上一次快照时间
                var lastChangeTime = new[] { inv.LatestStorageTime, inv.LatestOutboundTime }
                    .Where(t => t.HasValue)
                    .DefaultIfEmpty(DateTime.MinValue)
                    .Max();

                return lastChangeTime > lastSnapshotTime.Value;
            }).ToList();

            // 4. 生成并插入快照
            if (!needSnapshotInventories.Any())
                return 0;

            var snapshots = mapper.Map<List<tb_InventorySnapshot>>(needSnapshotInventories);

            return _unitOfWorkManage.GetDbClient().Insertable(snapshots).ExecuteCommand();
        }

        /// <summary>
        /// 获取每个商品-仓库的上一次快照时间
        /// </summary>
        private Dictionary<string, DateTime> GetLastSnapshotTimes(List<tb_Inventory> inventories)
        {
            // 提取所有商品-仓库组合
            var productWarehouseKeys = inventories
                .Select(inv => new { inv.ProdDetailID, inv.Location_ID })
                .Distinct()
                .ToList();

            if (!productWarehouseKeys.Any())
                return new Dictionary<string, DateTime>();

            // 查询这些组合的最近一次快照时间
            var lastSnapshots = _unitOfWorkManage.GetDbClient().Queryable<tb_InventorySnapshot>()
                .Where(s => productWarehouseKeys.Any(pw =>
                    pw.ProdDetailID == s.ProdDetailID && pw.Location_ID == s.Location_ID))
                .GroupBy(s => new { s.ProdDetailID, s.Location_ID })
                .Select(s => new
                {
                    Key = s.ProdDetailID + "_" + s.Location_ID,
                    LastTime = SqlFunc.AggregateMax(s.SnapshotTime.Value)
                })
                .ToList();

            return lastSnapshots.ToDictionary(item => item.Key, item => item.LastTime);
        }


        /// <summary>
        /// 生成指定时间的库存快照
        /// </summary>
        public async Task<int> GenerateSnapshot(DateTime snapshotTime)
        {
            var invControl = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

            // 获取当前所有库存数据
            List<tb_Inventory> currentInventories = await invControl.QueryAsync();

            if (!currentInventories.Any())
                return 0;

            IMapper mapper = _appContext.GetRequiredService<IMapper>();

            var snapshots = mapper.Map<List<tb_InventorySnapshot>>(currentInventories);

            // 批量插入快照数据
            return await _unitOfWorkManage.GetDbClient().Insertable(snapshots).ExecuteCommandAsync();
        }

     


        /// <summary>
        /// 清理过期的快照数据
        /// </summary>
        public int CleanupExpiredSnapshots(int keepMonths = 12)
        {
            var cutoffDate = DateTime.Now.AddMonths(-keepMonths);

            return _unitOfWorkManage.GetDbClient().Deleteable<tb_InventorySnapshot>()
                .Where(s => s.SnapshotTime < cutoffDate)
                .ExecuteCommand();
        }


    }
}



