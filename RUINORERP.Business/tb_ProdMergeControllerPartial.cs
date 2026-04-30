
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/01/2023 18:04:35
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
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using System.Collections;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 入库单 非生产领料/退料
    /// </summary>
    public partial class tb_ProdMergeController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 组合单审核  母件增加，子件减少
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>


        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_ProdMerge entity = ObjectEntity as tb_ProdMerge;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys = new List<(long ProdDetailID, long Location_ID)>();
                allKeys.Add((entity.ProdDetailID, entity.Location_ID));
                if (entity.tb_ProdMergeDetails != null)
                {
                    foreach (var detail in entity.tb_ProdMergeDetails)
                    {
                        allKeys.Add((detail.ProdDetailID, detail.Location_ID));
                    }
                }

                var invDict1 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                // ✅ 修复: 保存库存快照(在修改前),用于后续记录流水
                var invSnapshotDict1 = new Dictionary<(long ProdDetailID, long Location_ID), (int BeforeQuantity, decimal Inv_Cost)>();
                if (allKeys.Count > 0)
                {
                    var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict1 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                    
                    // ✅ 修复: 在修改前保存快照
                    foreach (var inv in inventoryList)
                    {
                        var key = (inv.ProdDetailID, inv.Location_ID);
                        invSnapshotDict1[key] = (inv.Quantity, inv.Inv_Cost);
                    }
                }
                #endregion

                // 开启事务，保证数据一致性
                await _unitOfWorkManage.BeginTranAsync();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //增加母件
                // ✅ 从预加载字典获取（死锁优化）
                var keyMother = (entity.ProdDetailID, entity.Location_ID);
                invDict1.TryGetValue(keyMother, out var invMother);
                if (invMother == null)
                {
                    await _unitOfWorkManage.RollbackTranAsync();
                    rs.ErrorMsg = $"母件首次增加库存必须通过【采购入库】，【期初盘点】或【缴库记录】产生过库存记录。";
                    rs.Succeeded = false;
                    return rs;

                }
                else
                {
                    //如果母件 成本为零 则将子件的加总赋值。实际少了加工费这些。缴款才是合理的入库成本变更的方式。这里暂时应急
                    if (invMother.Inv_Cost == 0)
                    {
                        invMother.Inv_Cost = entity.tb_ProdMergeDetails.Sum(c => c.UnitCost);
                    }
                    //更新库存
                    invMother.Quantity = invMother.Quantity + entity.MergeTargetQty;
                    invMother.LatestStorageTime = DateTime.Now;
                    BusinessHelper.Instance.EditEntity(invMother);
                }
                int InvInsertCounter = await _unitOfWorkManage.GetDbClient().Updateable(invMother).ExecuteCommandAsync();
                if (InvInsertCounter > 0)
                {
                    #region 子件减少
                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                    foreach (var child in entity.tb_ProdMergeDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        // ✅ 从预加载字典获取（死锁优化）
                        var key = (child.ProdDetailID, child.Location_ID);
                        invDict1.TryGetValue(key, out var inv);
                        if (inv != null)
                        {
                            if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.Qty) < 0)
                            {
                                if (child.tb_proddetail != null)
                                {
                                    rs.ErrorMsg = $"{child.tb_proddetail.SKU}库存为：{inv.Quantity}，组合消耗量为：{child.Qty}\r\n 系统设置不允许负库存， 请检查消耗数量与库存相关数据";
                                }
                                else
                                {
                                    rs.ErrorMsg = $"库存为：{inv.Quantity}，组合消耗量为：{child.Qty}\r\n 系统设置不允许负库存， 请检查消耗数量与库存相关数据";
                                }
                                await _unitOfWorkManage.RollbackTranAsync();
                                rs.Succeeded = false;
                                return rs;
                            }
                            //更新库存
                            inv.Quantity = inv.Quantity - child.Qty;
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        else
                        {
                            // 子件库存不存在，创建新记录
                            inv = new tb_Inventory();
                            inv.Location_ID = child.Location_ID;
                            inv.ProdDetailID = child.ProdDetailID;
                            inv.InitInventory = 0;
                            
                            // 如果不允许负库存，且消耗量>0，则报错
                            if (!_appContext.SysConfig.CheckNegativeInventory && child.Qty > 0)
                            {
                                rs.ErrorMsg = $"当前子件{child.tb_proddetail?.SKU ?? "未知"},在对应仓库中没有库存数据，无法消耗{child.Qty}个。请先通过【采购入库】或【期初盘点】建立库存。";
                                await _unitOfWorkManage.RollbackTranAsync();
                                rs.Succeeded = false;
                                return rs;
                            }
                            
                            // 允许负库存时，直接设置负数
                            inv.Quantity = -child.Qty;
                            BusinessHelper.Instance.InitEntity(inv);
                        }
                        /*
                      直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                     平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                     先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                     后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                     数据来源可以是多种多样的，例如：
                     采购价格：从供应商处购买产品或物品时的价格。
                     生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                     市场价格：参考市场上类似产品或物品的价格。
                      */
                        //计算成本切割时没有指定，则保持之前的。如果后面优化要子件可以指定成本时，这里也可以参与计算
                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        // 、、inv.CostFIFO = child.Cost;
                        //、 inv.CostMonthlyWA = child.Cost;
                        // inv.CostMovingWA = child.Cost;
                        inv.LatestStorageTime = System.DateTime.Now;

                        #endregion
                        invUpdateList.Add(inv);

                    }


                    // 使用LINQ查询
                    var CheckNewInvList = invUpdateList.Where(c => c.Inventory_ID == 0)
                        .GroupBy(i => new { i.ProdDetailID, i.Location_ID })
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key.ProdDetailID)
                        .ToList();

                    if (CheckNewInvList.Count > 0)
                    {
                        //新增库存中有重复的商品，操作失败。请联系管理员。
                        rs.ErrorMsg = "新增库存中有重复的商品，操作失败。";
                        rs.Succeeded = false;
                        await _unitOfWorkManage.RollbackTranAsync(); // ⚠️ P0 BUG修复：事务中返回前必须回滚
                        _logger.LogError(rs.ErrorMsg + "详细信息：" + string.Join(",", CheckNewInvList));
                        return rs;
                    }

                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (Counter == 0)
                    {
                        await _unitOfWorkManage.RollbackTranAsync();
                        throw new Exception("子件库存更新失败！");
                    }

                    // ✅ 新增: 记录库存流水(产品合并-母件增加,子件减少)
                    List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                    
                    // 1. 记录母件增加流水
                    // ✅ P0修复: 使用修改前保存的快照字典获取正确的数据
                    var keyMotherForTrans = (entity.ProdDetailID, entity.Location_ID);
                    if (invSnapshotDict1.TryGetValue(keyMotherForTrans, out var snapshotMother))
                    {
                        tb_InventoryTransaction motherTransaction = new tb_InventoryTransaction();
                        motherTransaction.ProdDetailID = entity.ProdDetailID;
                        motherTransaction.Location_ID = entity.Location_ID;
                        motherTransaction.BizType = (int)BizType.产品组合单;
                        motherTransaction.ReferenceId = entity.MergeID;
                        motherTransaction.ReferenceNo = entity.MergeNo;
                        motherTransaction.BeforeQuantity = snapshotMother.BeforeQuantity; // ✅ 使用修改前的快照数量
                        motherTransaction.QuantityChange = entity.MergeTargetQty; // 母件增加
                        motherTransaction.AfterQuantity = snapshotMother.BeforeQuantity + entity.MergeTargetQty; // ✅ 使用快照计算更新后的数量
                        motherTransaction.UnitCost = snapshotMother.Inv_Cost; // 使用更新前的成本
                        motherTransaction.TransactionTime = DateTime.Now;
                        motherTransaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        motherTransaction.Notes = $"产品合并审核：{entity.MergeNo}，母件增加，产品：{invMother?.tb_proddetail?.tb_prod?.CNName}";
                        transactionList.Add(motherTransaction);
                    }
                    
                    // 2. 记录子件减少流水
                    foreach (var child in entity.tb_ProdMergeDetails)
                    {
                        var key = (child.ProdDetailID, child.Location_ID);
                        if (invSnapshotDict1.TryGetValue(key, out var snapshotChild))
                        {
                            // 获取产品名称
                            invDict1.TryGetValue(key, out var invForTrans);
                            
                            tb_InventoryTransaction childTransaction = new tb_InventoryTransaction();
                            childTransaction.ProdDetailID = child.ProdDetailID;
                            childTransaction.Location_ID = child.Location_ID;
                            childTransaction.BizType = (int)BizType.产品组合单;
                            childTransaction.ReferenceId = entity.MergeID;
                            childTransaction.ReferenceNo = entity.MergeNo;
                            childTransaction.BeforeQuantity = snapshotChild.BeforeQuantity; // ✅ 使用修改前的快照数量
                            childTransaction.QuantityChange = -child.Qty; // 子件减少
                            childTransaction.AfterQuantity = snapshotChild.BeforeQuantity - child.Qty; // ✅ 使用快照计算更新后的数量
                            childTransaction.UnitCost = snapshotChild.Inv_Cost; // 使用更新前的成本
                            childTransaction.TransactionTime = DateTime.Now;
                            childTransaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                            childTransaction.Notes = $"产品合并审核：{entity.MergeNo}，子件消耗，产品：{invForTrans?.tb_proddetail?.tb_prod?.CNName}";
                            transactionList.Add(childTransaction);
                        }
                    }
                    
                    // 批量记录库存流水(带死锁重试机制)
                    if (transactionList.Any())
                    {
                        tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                        await tranController.BatchRecordTransactionsWithRetry(transactionList);
                    }

                    //这部分是否能提出到上一级公共部分？
                    entity.DataStatus = (int)DataStatus.确认;
                    // entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                    //后面已经修改为
                    ///  entity.ApprovalResults = approvalEntity.ApprovalResults;
                    entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                             .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                             .ExecuteCommandHasChangeAsync();
                    #endregion
                }



                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                await _unitOfWorkManage.CommitTranAsync();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.Succeeded = false;
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }
        }



        /// <summary>
        ///组合单反审  母件减少，子件增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_ProdMerge entity = ObjectEntity as tb_ProdMerge;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys2 = new List<(long ProdDetailID, long Location_ID)>();
                allKeys2.Add((entity.ProdDetailID, entity.Location_ID));
                if (entity.tb_ProdMergeDetails != null)
                {
                    foreach (var detail in entity.tb_ProdMergeDetails)
                    {
                        allKeys2.Add((detail.ProdDetailID, detail.Location_ID));
                    }
                }

                var invDict2 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                // ✅ 修复: 保存反审核前库存快照
                var invSnapshotDict2 = new Dictionary<(long ProdDetailID, long Location_ID), (int BeforeQuantity, decimal Inv_Cost)>();
                if (allKeys2.Count > 0)
                {
                    var requiredKeys = allKeys2.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict2 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                    
                    // ✅ 修复: 在修改前保存快照
                    foreach (var inv in inventoryList)
                    {
                        var key = (inv.ProdDetailID, inv.Location_ID);
                        invSnapshotDict2[key] = (inv.Quantity, inv.Inv_Cost);
                    }
                }
                #endregion

                // 开启事务，保证数据一致性
                await _unitOfWorkManage.BeginTranAsync();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //母件减少
                // ✅ 从预加载字典获取（死锁优化）
                var keyMother = (entity.ProdDetailID, entity.Location_ID);
                invDict2.TryGetValue(keyMother, out var invMother);
                if (invMother != null)
                {
                    //更新库存
                    invMother.Quantity = invMother.Quantity - entity.MergeTargetQty;
                    invMother.LatestOutboundTime = DateTime.Now;
                    BusinessHelper.Instance.EditEntity(invMother);
                }
                else
                {
                    await _unitOfWorkManage.RollbackTranAsync();
                    throw new Exception("系统对应的仓库中没有母件库存,请检查数据！ ");
                }
                int InvInsertCounter = await _unitOfWorkManage.GetDbClient().Updateable(invMother).ExecuteCommandAsync();
                if (InvInsertCounter > 0)
                {
                    //子件增加
                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                    foreach (var child in entity.tb_ProdMergeDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        //标记是否有期初
                        // ✅ 从预加载字典获取（死锁优化）
                        var key = (child.ProdDetailID, child.Location_ID);
                        invDict2.TryGetValue(key, out var inv);
                        if (inv != null)
                        {
                            //更新库存
                            inv.Quantity = inv.Quantity + child.Qty;
                            BusinessHelper.Instance.EditEntity(inv);
                        }

                        /*
                      直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                     平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                     先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                     后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                     数据来源可以是多种多样的，例如：
                     采购价格：从供应商处购买产品或物品时的价格。
                     生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                     市场价格：参考市场上类似产品或物品的价格。
                      */
                        //inv.Inv_Cost = child.Cost;//这里需要计算，根据系统设置中的算法计算。
                        //inv.CostFIFO = child.Cost;
                        //inv.CostMonthlyWA = child.Cost;
                        //inv.CostMovingWA = child.Cost;
                        inv.ProdDetailID = child.ProdDetailID;
                        // inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestStorageTime = System.DateTime.Now;
                        #endregion
                        invUpdateList.Add(inv);
                    }
                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (Counter == 0)
                    {
                        await _unitOfWorkManage.RollbackTranAsync();
                        throw new Exception("子件库存更新失败！");
                    }

                    // ✅ 新增: 记录反向库存流水(产品合并反审-母件减少,子件增加)
                    List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                    
                    // 1. 记录母件减少流水
                    // ✅ P0修复: 使用修改前保存的快照字典获取正确的数据
                    var keyMotherForTrans = (entity.ProdDetailID, entity.Location_ID);
                    if (invSnapshotDict2.TryGetValue(keyMotherForTrans, out var snapshotMother))
                    {
                        tb_InventoryTransaction motherTransaction = new tb_InventoryTransaction();
                        motherTransaction.ProdDetailID = entity.ProdDetailID;
                        motherTransaction.Location_ID = entity.Location_ID;
                        motherTransaction.BizType = (int)BizType.产品组合单;
                        motherTransaction.ReferenceId = entity.MergeID;
                        motherTransaction.ReferenceNo = entity.MergeNo;
                        motherTransaction.BeforeQuantity = snapshotMother.BeforeQuantity; // ✅ 使用修改前的快照数量
                        motherTransaction.QuantityChange = -entity.MergeTargetQty; // 母件减少
                        motherTransaction.AfterQuantity = snapshotMother.BeforeQuantity - entity.MergeTargetQty; // ✅ 使用快照计算更新后的数量
                        motherTransaction.UnitCost = snapshotMother.Inv_Cost; // 使用更新前的成本
                        motherTransaction.TransactionTime = DateTime.Now;
                        motherTransaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        motherTransaction.Notes = $"产品合并反审核：{entity.MergeNo}，母件减少，产品：{invMother?.tb_proddetail?.tb_prod?.CNName}";
                        transactionList.Add(motherTransaction);
                    }
                    
                    // 2. 记录子件增加流水
                    foreach (var child in entity.tb_ProdMergeDetails)
                    {
                        var key = (child.ProdDetailID, child.Location_ID);
                        if (invSnapshotDict2.TryGetValue(key, out var snapshotChild))
                        {
                            // 获取产品名称
                            invDict2.TryGetValue(key, out var invForTrans);
                            
                            tb_InventoryTransaction childTransaction = new tb_InventoryTransaction();
                            childTransaction.ProdDetailID = child.ProdDetailID;
                            childTransaction.Location_ID = child.Location_ID;
                            childTransaction.BizType = (int)BizType.产品组合单;
                            childTransaction.ReferenceId = entity.MergeID;
                            childTransaction.ReferenceNo = entity.MergeNo;
                            childTransaction.BeforeQuantity = snapshotChild.BeforeQuantity; // ✅ 使用修改前的快照数量
                            childTransaction.QuantityChange = child.Qty; // 子件增加
                            childTransaction.AfterQuantity = snapshotChild.BeforeQuantity + child.Qty; // ✅ 使用快照计算更新后的数量
                            childTransaction.UnitCost = snapshotChild.Inv_Cost; // 使用更新前的成本
                            childTransaction.TransactionTime = DateTime.Now;
                            childTransaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                            childTransaction.Notes = $"产品合并反审核：{entity.MergeNo}，子件退回，产品：{invForTrans?.tb_proddetail?.tb_prod?.CNName}";
                            transactionList.Add(childTransaction);
                        }
                    }
                    
                    // 批量记录反向库存流水(带死锁重试机制)
                    if (transactionList.Any())
                    {
                        tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                        await tranController.BatchRecordTransactionsWithRetry(transactionList);
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;

                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                             .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                             .ExecuteCommandHasChangeAsync();
                // 注意信息的完整性
                await _unitOfWorkManage.CommitTranAsync();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                await _unitOfWorkManage.RollbackTranAsync();
                rs.Succeeded = false;
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }
        }

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_ProdMerge> list = await _appContext.Db.CopyNew().Queryable<tb_ProdMerge>().Where(m => m.MergeID == ID)
                             .Includes(a => a.tb_bom_s)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProdMergeDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }



    }
}



