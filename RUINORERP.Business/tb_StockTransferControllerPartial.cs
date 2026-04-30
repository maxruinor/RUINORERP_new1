// *************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/12/2023 14:45:18
// **************************************
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace RUINORERP.Business
{
    public partial class tb_StockTransferController<T>
    {


        /// <summary>
        /// 调拨单审核，
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmsr = new ReturnResults<T>();
            tb_StockTransfer entity = ObjectEntity as tb_StockTransfer;
            try
            {
                if (entity == null)
                {
                    return rmsr;
                }

                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys = new List<(long ProdDetailID, long LocationID)>();
                
                foreach (var child in entity.tb_StockTransferDetails)
                {
                    allKeys.Add((child.ProdDetailID, entity.Location_ID_from));
                    allKeys.Add((child.ProdDetailID, entity.Location_ID_to));
                }

                var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.LocationID }).Distinct().ToList();

                var inventoryList = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_Inventory>()
                    .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.LocationID == i.Location_ID))
                    .ToListAsync();

                var invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                // ✅ 修复: 保存库存快照(在修改前),用于后续记录流水
                var invSnapshotDict = new Dictionary<(long ProdDetailID, long Location_ID), (int BeforeQuantity, decimal Inv_Cost)>();
                foreach (var inv in inventoryList)
                {
                    var key = (inv.ProdDetailID, inv.Location_ID);
                    invSnapshotDict[key] = (inv.Quantity, inv.Inv_Cost);
                }
                #endregion

                // 开启事务，保证数据一致性
                await _unitOfWorkManage.BeginTranAsync();

              var   ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                // 创建库存更新列表（合并调出和调入）
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                // 创建库存插入列表（仅用于新增库存）
                List<tb_Inventory> invInsertList = new List<tb_Inventory>();
                // 创建库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                foreach (var child in entity.tb_StockTransferDetails)
                {
                    // ✅ P0修复: 使用修改前保存的快照字典获取调出仓库正确的数据
                    var keyFrom = (child.ProdDetailID, entity.Location_ID_from);
                    if (!invSnapshotDict.TryGetValue(keyFrom, out var snapshotFrom))
                    {
                        snapshotFrom = (0, 0m);
                    }
                    
                    //先看库存表中是否存在记录。  

                    #region 库存表的更新 调出
                    //标记是否有期初

                    // ✅ 从预加载字典获取（死锁优化）
                    if (!invDict.TryGetValue((child.ProdDetailID, entity.Location_ID_from), out var invFrom) || invFrom == null)
                    {
                        rmsr.ErrorMsg = "调出仓库中不存在这个产品的库存，出库产品必须存在于仓库中。";
                        await _unitOfWorkManage.RollbackTranAsync();
                        rmsr.Succeeded = false;
                        return rmsr;
                    }

                    if (!_appContext.SysConfig.CheckNegativeInventory && (invFrom.Quantity - child.Qty) < 0)
                    {
                        rmsr.ErrorMsg = "系统设置不允许负库存，请检查调出数量与库存相关数据";
                        await _unitOfWorkManage.RollbackTranAsync();
                        rmsr.Succeeded = false;
                        return rmsr;
                    }

                    //更新库存
                    invFrom.Quantity = invFrom.Quantity - child.Qty;
                    invFrom.Inv_SubtotalCostMoney = invFrom.Inv_Cost * invFrom.Quantity;
                    invFrom.LatestOutboundTime = System.DateTime.Now;
                    BusinessHelper.Instance.EditEntity(invFrom);
                    // 添加到批量更新列表
                    invUpdateList.Add(invFrom);
                    
                    // 创建调出仓库的库存流水记录
                    tb_InventoryTransaction transactionFrom = new tb_InventoryTransaction();
                    transactionFrom.ProdDetailID = child.ProdDetailID;
                    transactionFrom.Location_ID = entity.Location_ID_from;
                    transactionFrom.BizType = (int)BizType.调拨单;
                    transactionFrom.ReferenceId = entity.StockTransferID;
                    transactionFrom.ReferenceNo = entity.StockTransferNo;
                    transactionFrom.BeforeQuantity = snapshotFrom.BeforeQuantity; // ✅ 使用修改前的快照数量
                    transactionFrom.QuantityChange = -child.Qty; // 调出减少库存
                    transactionFrom.AfterQuantity = snapshotFrom.BeforeQuantity - child.Qty; // ✅ 使用快照计算更新后的数量
                    transactionFrom.UnitCost = snapshotFrom.Inv_Cost; // 使用更新前的成本
                    transactionFrom.TransactionTime = DateTime.Now;
                    transactionFrom.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionFrom.Notes = $"调拨单审核：{entity.StockTransferNo}，调出仓库：{entity.Location_ID_from}，产品：{invFrom.tb_proddetail?.tb_prod?.CNName}";
                    transactionList.Add(transactionFrom);

                    #endregion

                    #region 库存表的更新  调入时不考虑成本价格,如果初次入库时则使用调出时的成本。
                    //标记是否有期初

                    // ✅ 从预加载字典获取（死锁优化）
                    if (!invDict.TryGetValue((child.ProdDetailID, entity.Location_ID_to), out var invTo) || invTo == null)
                    {
                        invTo = new tb_Inventory();
                        invTo.Location_ID = entity.Location_ID_to;
                        invTo.ProdDetailID = child.ProdDetailID;
                        invTo.Quantity = child.Qty;
                        invTo.InitInventory = (int)invTo.Quantity;
                        invTo.CostFIFO = invFrom.CostFIFO;
                        invTo.CostMonthlyWA = invFrom.CostMonthlyWA;
                        invTo.Inv_Cost = invFrom.Inv_Cost;
                        invTo.CostMovingWA = invFrom.CostMovingWA;
                        invTo.Inv_AdvCost = invFrom.Inv_AdvCost;
                        invTo.Inv_SubtotalCostMoney = invTo.Inv_Cost * invTo.Quantity;
                        invTo.LatestStorageTime = System.DateTime.Now;
                        invTo.Notes = "";
                        BusinessHelper.Instance.InitEntity(invTo);
                        invDict[(child.ProdDetailID, entity.Location_ID_to)] = invTo;
                        // 添加到插入列表
                        invInsertList.Add(invTo);
                    }
                    else
                    {
                        //更新库存
                        invTo.Quantity = invTo.Quantity + child.Qty;
                        invTo.CostMovingWA = invFrom.CostMovingWA;
                        invTo.Inv_AdvCost = invFrom.Inv_AdvCost;
                        invTo.Inv_Cost = invFrom.Inv_Cost;
                        invTo.Inv_SubtotalCostMoney = invTo.Inv_Cost * invTo.Quantity;
                        invTo.LatestStorageTime = System.DateTime.Now;
                        BusinessHelper.Instance.EditEntity(invTo);
                        // 添加到批量更新列表
                        invUpdateList.Add(invTo);
                    }
                    
                    // ✅ P0修复: 使用修改前保存的快照字典获取调入仓库正确的数据
                    var keyTo = (child.ProdDetailID, entity.Location_ID_to);
                    if (!invSnapshotDict.TryGetValue(keyTo, out var snapshotTo))
                    {
                        snapshotTo = (0, 0m);
                    }
                    
                    // 创建调入仓库的库存流水记录
                    tb_InventoryTransaction transactionTo = new tb_InventoryTransaction();
                    transactionTo.ProdDetailID = child.ProdDetailID;
                    transactionTo.Location_ID = entity.Location_ID_to;
                    transactionTo.BizType = (int)BizType.调拨单;
                    transactionTo.ReferenceId = entity.StockTransferID;
                    transactionTo.ReferenceNo = entity.StockTransferNo;
                    transactionTo.BeforeQuantity = snapshotTo.BeforeQuantity; // ✅ 使用修改前的快照数量
                    transactionTo.QuantityChange = child.Qty; // 调入增加库存
                    transactionTo.AfterQuantity = snapshotTo.BeforeQuantity + child.Qty; // ✅ 使用快照计算更新后的数量
                    transactionTo.UnitCost = snapshotFrom.Inv_Cost; // 使用调出仓库的成本
                    transactionTo.TransactionTime = DateTime.Now;
                    transactionTo.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionTo.Notes = $"调拨单审核：{entity.StockTransferNo}，调入仓库：{entity.Location_ID_to}，产品：{invTo.tb_proddetail?.tb_prod?.CNName}";
                    transactionList.Add(transactionTo);
                    #endregion
                }
                
                // 记录库存流水
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactionsWithRetry(transactionList);

                 
                // 批量更新所有库存
                if (invUpdateList.Any())
                {
                    int updateCount = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList)
                        .UpdateColumns(it => new { it.Quantity, it.Inv_SubtotalCostMoney, it.LatestOutboundTime, it.LatestStorageTime })
                        .ExecuteCommandAsync();
                    if (updateCount == 0)
                    {
                        await _unitOfWorkManage.RollbackTranAsync();
                        rmsr.ErrorMsg = "库存更新失败。";
                        throw new Exception("库存更新失败！");
                    }
                }

                // 批量插入新增库存
                if (invInsertList.Any())
                {
                    var insertResult = await _unitOfWorkManage.GetDbClient().Insertable(invInsertList).ExecuteReturnSnowflakeIdListAsync();
                    if (insertResult.Count == 0)
                    {
                        await _unitOfWorkManage.RollbackTranAsync();
                        rmsr.ErrorMsg = "库存插入失败。";
                        throw new Exception("库存插入失败！");
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

                // 注意信息的完整性
                await _unitOfWorkManage.CommitTranAsync();
                rmsr.ReturnObject = entity as T;
                rmsr.Succeeded = true;
                return rmsr;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
                _logger.Error(ex, BizTypeText + "事务回滚");
                rmsr.ErrorMsg = BizTypeText + ex.Message;
                return rmsr;
            }

        }


        /// <summary>
        /// 反审核1
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_StockTransfer entity = ObjectEntity as tb_StockTransfer;
            ReturnResults<T> rmsr = new ReturnResults<T>();
            try
            {
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys = new List<(long ProdDetailID, long LocationID)>();
                
                foreach (var child in entity.tb_StockTransferDetails)
                {
                    allKeys.Add((child.ProdDetailID, entity.Location_ID_from));
                    allKeys.Add((child.ProdDetailID, entity.Location_ID_to));
                }

                var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.LocationID }).Distinct().ToList();

                var inventoryList = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_Inventory>()
                    .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.LocationID == i.Location_ID))
                    .ToListAsync();

                var invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                // ✅ 修复: 保存反审核前库存快照
                var invSnapshotDict2 = new Dictionary<(long ProdDetailID, long Location_ID), (int BeforeQuantity, decimal Inv_Cost)>();
                foreach (var inv in inventoryList)
                {
                    var key = (inv.ProdDetailID, inv.Location_ID);
                    invSnapshotDict2[key] = (inv.Quantity, inv.Inv_Cost);
                }
                #endregion

                // 开启事务，保证数据一致性
                await _unitOfWorkManage.BeginTranAsync();

                // 创建批量更新列表
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                
                // 创建反向库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                foreach (var child in entity.tb_StockTransferDetails)
                {
                    // ✅ P0修复: 使用修改前保存的快照字典获取调出仓库正确的数据
                    var keyFrom = (child.ProdDetailID, entity.Location_ID_from);
                    if (!invSnapshotDict2.TryGetValue(keyFrom, out var snapshotFrom))
                    {
                        snapshotFrom = (0, 0m);
                    }
                    
                    //先看库存表中是否存在记录。  

                    #region 库存表的更新 反审时，调出的要加回来。
                    //标记是否有期初

                    // ✅ 从预加载字典获取（死锁优化）
                    if (!invDict.TryGetValue((child.ProdDetailID, entity.Location_ID_from), out var invFrom) || invFrom == null)
                    {
                        rmsr.ErrorMsg = "调出仓库中不存在这个产品的库存，出库产品必须存在于仓库中。";
                        await _unitOfWorkManage.RollbackTranAsync();
                        rmsr.Succeeded = false;
                        return rmsr;
                    }

                    //更新库存
                    invFrom.Quantity = invFrom.Quantity + child.Qty;
                    invFrom.LatestStorageTime = System.DateTime.Now;
                    invFrom.Inv_SubtotalCostMoney = invFrom.Inv_Cost * invFrom.Quantity;
                    BusinessHelper.Instance.EditEntity(invFrom);
                    // 添加到批量更新列表
                    invUpdateList.Add(invFrom);
                    
                    // 创建调出仓库的反向库存流水记录（反审核时调出仓库增加库存）
                    tb_InventoryTransaction transactionFrom = new tb_InventoryTransaction();
                    transactionFrom.ProdDetailID = child.ProdDetailID;
                    transactionFrom.Location_ID = entity.Location_ID_from;
                    transactionFrom.BizType = (int)BizType.调拨单;
                    transactionFrom.ReferenceId = entity.StockTransferID;
                    transactionFrom.ReferenceNo = entity.StockTransferNo;
                    transactionFrom.BeforeQuantity = snapshotFrom.BeforeQuantity; // ✅ 使用修改前的快照数量
                    transactionFrom.QuantityChange = child.Qty; // 反审核时调出仓库增加库存
                    transactionFrom.AfterQuantity = snapshotFrom.BeforeQuantity + child.Qty; // ✅ 使用快照计算更新后的数量
                    transactionFrom.UnitCost = snapshotFrom.Inv_Cost; // 使用更新前的成本
                    transactionFrom.TransactionTime = DateTime.Now;
                    transactionFrom.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionFrom.Notes = $"调拨单反审核：{entity.StockTransferNo}，调出仓库反向调整：{entity.Location_ID_from}，产品：{invFrom.tb_proddetail?.tb_prod?.CNName}";
                    transactionList.Add(transactionFrom);

                    #endregion

                    #region 库存表的更新  调入时不考虑成本价格,如果初次入库时则使用调出时的成本。
                    //标记是否有期初

                    // ✅ P0修复: 使用修改前保存的快照字典获取调入仓库正确的数据
                    var keyTo = (child.ProdDetailID, entity.Location_ID_to);
                    if (!invSnapshotDict2.TryGetValue(keyTo, out var snapshotTo))
                    {
                        snapshotTo = (0, 0m);
                    }

                    // ✅ 从预加载字典获取（死锁优化）
                    if (!invDict.TryGetValue((child.ProdDetailID, entity.Location_ID_to), out var invTo) || invTo == null)
                    {
                        //正常逻辑不会执行到这里
                        await _unitOfWorkManage.RollbackTranAsync();
                        throw new Exception("调入仓库中不存在这个产品的库存，出库产品必须存在于仓库中。");
                    }

                    if (!_appContext.SysConfig.CheckNegativeInventory && (invTo.Quantity - child.Qty) < 0)
                    {
                        rmsr.ErrorMsg = "系统设置不允许负库存，请检查调出数量与库存相关数据";
                        await _unitOfWorkManage.RollbackTranAsync();
                        rmsr.Succeeded = false;
                        return rmsr;
                    }

                    //更新库存
                    invTo.Quantity = invTo.Quantity - child.Qty;
                    invTo.LatestOutboundTime = System.DateTime.Now;
                    invTo.Inv_SubtotalCostMoney = invTo.Inv_Cost * invTo.Quantity;
                    BusinessHelper.Instance.EditEntity(invTo);
                    // 添加到批量更新列表
                    invUpdateList.Add(invTo);
                    
                    // 创建调入仓库的反向库存流水记录（反审核时调入仓库减少库存）
                    tb_InventoryTransaction transactionTo = new tb_InventoryTransaction();
                    transactionTo.ProdDetailID = child.ProdDetailID;
                    transactionTo.Location_ID = entity.Location_ID_to;
                    transactionTo.BizType = (int)BizType.调拨单;
                    transactionTo.ReferenceId = entity.StockTransferID;
                    transactionTo.ReferenceNo = entity.StockTransferNo;
                    transactionTo.BeforeQuantity = snapshotTo.BeforeQuantity; // ✅ 使用修改前的快照数量
                    transactionTo.QuantityChange = -child.Qty; // 反审核时调入仓库减少库存
                    transactionTo.AfterQuantity = snapshotTo.BeforeQuantity - child.Qty; // ✅ 使用快照计算更新后的数量
                    transactionTo.UnitCost = snapshotTo.Inv_Cost; // 使用更新前的成本
                    transactionTo.TransactionTime = DateTime.Now;
                    transactionTo.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionTo.Notes = $"调拨单反审核：{entity.StockTransferNo}，调入仓库反向调整：{entity.Location_ID_to}，产品：{invTo.tb_proddetail?.tb_prod?.CNName}";
                    transactionList.Add(transactionTo);
                    #endregion
                }
                
                // 批量更新所有库存
                int updateCount = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList)
                    .UpdateColumns(it => new { it.Quantity, it.Inv_SubtotalCostMoney, it.LatestOutboundTime, it.LatestStorageTime })
                    .ExecuteCommandAsync();
                if (updateCount == 0)
                {
                    await _unitOfWorkManage.RollbackTranAsync();
                    throw new Exception("库存更新失败！");
                }
                
                // 记录反向库存流水（带死锁重试机制）
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactionsWithRetry(transactionList);

                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "反审";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                            .ExecuteCommandHasChangeAsync();
                // 注意信息的完整性
                await _unitOfWorkManage.CommitTranAsync();
                rmsr.Succeeded = true;
                rmsr.ReturnObject = entity as T;
                return rmsr;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
                rmsr.ErrorMsg = BizTypeText + "反审失败，" + ex.Message;
                _logger.Error(ex, BizTypeText + "事务回滚");
                return rmsr;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_StockTransfer> list = await _appContext.Db.CopyNew().Queryable<tb_StockTransfer>().Where(m => m.StockTransferID == ID)
                             .Includes(a => a.tb_location)
                             .Includes(a => a.tb_location_locationIdTo)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_StockTransferDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

    }

}