
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 10:05:10
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
using RUINORERP.Global;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 维修领料单
    /// </summary>
    public partial class tb_AS_RepairMaterialPickupController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// ✅ P1新增：智能获取维修仓首次入库成本（与tb_AS_RepairInStockController共用逻辑）
        /// 优先级：1)其他仓库平均成本 2)产品标准成本 3)0
        /// </summary>
        /// <param name="prodDetailID">产品详情ID</param>
        /// <returns>成本价格</returns>
        private decimal GetRepairInventoryCost(long prodDetailID)
        {
            try
            {
                // 方案1：从其他仓库获取该产品的平均成本（排除维修仓）
                var otherWarehouseCosts = _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_Inventory>()
                    .Where(i => i.ProdDetailID == prodDetailID && i.Quantity > 0)
                    .Select(i => new { i.Inv_Cost, i.Quantity })
                    .ToList();

                if (otherWarehouseCosts != null && otherWarehouseCosts.Any())
                {
                    // 计算加权平均成本
                    decimal totalCost = otherWarehouseCosts.Sum(x => x.Inv_Cost * x.Quantity);
                    int totalQty = otherWarehouseCosts.Sum(x => x.Quantity);
                    
                    if (totalQty > 0)
                    {
                        decimal avgCost = totalCost / totalQty;
                        _logger.LogDebug($"维修仓首次入库，从其他仓库获取成本：产品ID={prodDetailID}, 平均成本={avgCost:F2}");
                        return Math.Round(avgCost, 4);
                    }
                }

                ///产品表中没有标准成品字段
                // 方案2：从产品主数据获取标准成本
                //var prodDetail = _unitOfWorkManage.GetDbClient()
                //    .Queryable<tb_ProdDetail>()
                //    .Where(p => p.ProdDetailID == prodDetailID)
                //    .First();

                //if (prodDetail != null && prodDetail.StdCost > 0)
                //{
                //    _logger.LogDebug($"维修仓首次入库，使用产品标准成本：产品ID={prodDetailID}, 标准成本={prodDetail.StdCost:F2}");
                //    return prodDetail.StdCost;
                //}

                // 方案3：返回0（后续通过盘点或调价设置成本）
                _logger.LogWarning($"维修仓首次入库，未找到成本信息：产品ID={prodDetailID}，成本设为0");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取维修仓成本失败：产品ID={prodDetailID}");
                return 0;
            }
        }

        /// <summary>
        /// 转为维修领料单
        /// </summary>
        /// <param name="RepairOrder"></param>
        public async Task<tb_AS_RepairMaterialPickup> ToRepairMaterialPickupAsync(tb_AS_RepairOrder RepairOrder)
        {
            tb_AS_RepairMaterialPickup entity = new tb_AS_RepairMaterialPickup();
            //转单
            if (RepairOrder != null)
            {
                entity = mapper.Map<tb_AS_RepairMaterialPickup>(RepairOrder);
                entity.ApprovalOpinions = "";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;


                List<string> tipsMsg = new List<string>();
                List<tb_AS_RepairMaterialPickupDetail> details = mapper.Map<List<tb_AS_RepairMaterialPickupDetail>>(RepairOrder.tb_AS_RepairOrderMaterialDetails);
                List<tb_AS_RepairMaterialPickupDetail> NewDetails = new List<tb_AS_RepairMaterialPickupDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    #region 每行产品ID唯一
                    var item = RepairOrder.tb_AS_RepairOrderMaterialDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                      && c.Location_ID == details[i].Location_ID);
                    details[i].ActualSentQty = 0;
                    details[i].ShouldSendQty = item.ShouldSendQty - item.ActualSentQty;// 减掉已经出库的数量
                    if (details[i].ShouldSendQty > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    else
                    {
                        var detail = _cacheManager.GetEntity<View_ProdDetail>(item.ProdDetailID);
                        if (detail != null)
                        {
                           // item.tb_proddetail = detail;
                            tipsMsg.Add($"当前维修工单物料中的SKU:{detail.SKU}已出库数量为{details[i].ActualSentQty}，当前行数据将不会加载到明细！");
                        }
                    }
                    #endregion
                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"【维修领料单】:{entity.RepairOrderNo}已全部领取，请检查是否正在重复操作！");
                }

                entity.tb_AS_RepairMaterialPickupDetails = NewDetails;
                entity.TotalCost = NewDetails.Sum(c => c.SubtotalCost);
                entity.TotalPrice = NewDetails.Sum(c => c.SubtotalPrice);
                entity.TotalSendQty = NewDetails.Sum(c => c.ActualSentQty);
                entity.TotalReQty = NewDetails.Sum(c => c.ReturnQty);

                entity.DeliveryDate = System.DateTime.Now;
                BusinessHelper.Instance.InitEntity(entity);
                IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                entity.MaterialPickupNO = await bizCodeService.GenerateBizBillNoAsync(BizType.维修领料单);
                entity.tb_as_repairorder = RepairOrder;

                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }

        /// <summary>
        /// 数量入到维修仓库
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_RepairMaterialPickup entity = ObjectEntity as tb_AS_RepairMaterialPickup;
            if (!entity.RepairOrderID.HasValue || entity.RepairOrderID.Value <= 0)
            {
                rmrs.ErrorMsg = $"维修领料单必须引用维修工单，请检查后再试！";
                rmrs.Succeeded = false;
                return rmrs;
            }
            try
            {

                if ((entity.TotalSendQty == 0 || entity.tb_AS_RepairMaterialPickupDetails.Sum(c => c.ActualSentQty) == 0))
                {
                    rmrs.ErrorMsg = $"单据总数量{entity.TotalSendQty}和明细数量之和{entity.tb_AS_RepairMaterialPickupDetails.Sum(c => c.ActualSentQty)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal RepairQty, DateTime LatestOutboundTime)>();

                //更新 仓的数量减少
                foreach (var child in entity.tb_AS_RepairMaterialPickupDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentRepairQty = child.ActualSentQty;
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间
                                                                 // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            //采购和销售都会提前处理。所以这里默认提供一行数据。成本和数量都可能为0
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0, // 初始数量
                                InitInventory = 0,
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充 。 售后维修仓  不处理成本 所以是不是在仓库设置中设置一个参数是否处理成本
                                Notes = "维修领料单创建",
                                Sale_Qty = 0,
                            };
                            BusinessHelper.Instance.InitEntity(inv);
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            RepairQty: currentRepairQty, // 首次累加
                                   LatestOutboundTime: currentOutboundTime
                        //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.RepairQty += currentRepairQty;

                        if (!_appContext.SysConfig.CheckNegativeInventory && (group.Inventory.Quantity - group.RepairQty) < 0)
                        {
                            // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                            rmrs.ErrorMsg = $"当前行仓库的SKU:{group.Inventory.tb_proddetail.SKU}库存为：{group.Inventory.Quantity}，维修领料量为：{group.RepairQty}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据";
                            rmrs.Succeeded = false;
                            return rmrs;
                        }

                        // 取最新出库时间（若当前时间更新，则覆盖）
                        group.LatestOutboundTime = System.DateTime.Now;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                List<((tb_Inventory Inventory, decimal RepairQty, DateTime LatestOutboundTime) Group, int BeforeQty)> inventoryWithBeforeQty = new List<((tb_Inventory Inventory, decimal RepairQty, DateTime LatestOutboundTime) Group, int BeforeQty)>();
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    
                    // ✅ P0修复：保存更新前的数量快照
                    int beforeQty = inv.Quantity;
                    
                    // ✅ P1修复：维修领料出库也需要计算成本（减少库存）
                    // 注意：先调用成本反算，再更新数量，保持时序一致
                    if (inv.Quantity > 0 && group.Value.RepairQty.ToInt() > 0)
                    {
                        // 使用当前库存成本进行反算
                        if (inv.Inv_Cost > 0)
                        {
                            CommService.CostCalculations.AntiCostCalculation(_appContext, inv, group.Value.RepairQty.ToInt(), inv.Inv_Cost);
                        }
                    }
                    
                    inv.Quantity -= group.Value.RepairQty.ToInt();
                    invList.Add(inv);
                    
                    // ✅ P0修复：保存更新前后的信息用于流水记录
                    inventoryWithBeforeQty.Add((group.Value, beforeQty));
                }

                if (entity.RepairOrderID.HasValue && entity.RepairOrderID.Value > 0)
                {
                    entity.tb_as_repairorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>()
                         .Includes(b => b.tb_AS_RepairOrderMaterialDetails)
                         .Where(c => c.RepairOrderID == entity.RepairOrderID)
                         .SingleAsync();

                    if (entity.tb_as_repairorder.RepairStatus != (int)RepairStatus.维修中)
                    {
                        throw new Exception($"维修领料单审核时，维修工单{entity.tb_as_repairorder.RepairOrderNo}状态不是维修中，请检查后再试！");
                    }

                    //更新维修工单中 材料明细的实发数量
                    for (int i = 0; i < entity.tb_AS_RepairMaterialPickupDetails.Count; i++)
                    {
                        var PickUpMaterial = entity.tb_AS_RepairMaterialPickupDetails[i];
                        if (PickUpMaterial.ActualSentQty == 0)
                        {
                            continue;
                        }
                        var detail = entity.tb_as_repairorder.tb_AS_RepairOrderMaterialDetails
                            .FirstOrDefault(c => c.ProdDetailID == PickUpMaterial.ProdDetailID && c.Location_ID == PickUpMaterial.Location_ID);
                        if (detail != null)
                        {
                            detail.ActualSentQty += PickUpMaterial.ActualSentQty;
                        }
                        if (detail.ActualSentQty > detail.ShouldSendQty)
                        {
                            throw new Exception($"维修领料单审核时，明细中实发数量{detail.ActualSentQty}，不能大于应发数量{detail.ShouldSendQty}，请检查后再试！");
                        }
                    }



                    await _unitOfWorkManage.BeginTranAsync();
                    var MaterialQtyResult = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_repairorder.tb_AS_RepairOrderMaterialDetails).UpdateColumns(it => new
                    {
                        it.ActualSentQty,
                    }).ExecuteCommandAsync();

                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                    if (Counter == 0)
                    {
                        await _unitOfWorkManage.RollbackTranAsync();
                        throw new Exception("维修领料单审核时，库存更新数据为0，更新失败！");
                    }

                    // ✅ 新增: 记录库存流水(售后维修领料减少库存)
                    List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                    foreach (var item in inventoryWithBeforeQty)
                    {
                        var group = item.Group;
                        int beforeQty = item.BeforeQty; // ✅ P0修复: 使用保存的快照
                        
                        if (group.Inventory != null)
                        {
                            decimal realtimeCost = group.Inventory.Inv_Cost;
                            
                            tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                            transaction.ProdDetailID = group.Inventory.ProdDetailID;
                            transaction.Location_ID = group.Inventory.Location_ID;
                            transaction.BizType = (int)BizType.维修领料单;
                            transaction.ReferenceId = entity.RMRID;
                            transaction.ReferenceNo = entity.MaterialPickupNO;
                            transaction.BeforeQuantity = beforeQty; // ✅ P0修复: 更新前的数量(快照)
                            transaction.QuantityChange = -group.RepairQty.ToInt(); // 维修领料减少库存
                            transaction.AfterQuantity = beforeQty - group.RepairQty.ToInt(); // ✅ P0修复: 更新后的数量
                            transaction.UnitCost = realtimeCost;
                            transaction.TransactionTime = DateTime.Now;
                            transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                            transaction.Notes = $"售后维修领料审核：{entity.MaterialPickupNO}，产品：{group.Inventory.tb_proddetail?.tb_prod?.CNName}";
                            
                            transactionList.Add(transaction);
                        }
                    }
                    
                    if (transactionList.Any())
                    {
                        tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                        await tranController.BatchRecordTransactionsWithRetry(transactionList);
                    }

                    //这部分是否能提出到上一级公共部分？
                    if (entity.tb_as_repairorder.RepairStatus != (int)RepairStatus.维修中)
                    {
                        entity.tb_as_repairorder.RepairStatus = (int)RepairStatus.维修中;
                        var RepairStatusResult = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_repairorder).UpdateColumns(it => new
                        {
                            it.RepairStatus,
                        }).ExecuteCommandAsync();
                    }

                    entity.DataStatus = (int)DataStatus.确认;
                    entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                    entity.ApprovalResults = true;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    //只更新指定列
                    var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairMaterialPickup>(entity).UpdateColumns(it => new
                    {
                        it.DataStatus,
                        it.ApprovalResults,
                        it.ApprovalStatus,
                        it.Approver_at,
                        it.Approver_by,
                        it.ApprovalOpinions
                    }).ExecuteCommandAsync();
                }

                // 注意信息的完整性
                await _unitOfWorkManage.CommitTranAsync();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }

        /// <summary>
        /// 维修仓库 出来 返回维修状态
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_RepairMaterialPickup entity = ObjectEntity as tb_AS_RepairMaterialPickup;
            try
            {

                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal RepairQty)>();

                //更新  仓的数量增加
                foreach (var child in entity.tb_AS_RepairMaterialPickupDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentRepairQty = child.ActualSentQty;
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间
                                                                 // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            throw new Exception("维修领料单库存表中没有该记录");
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            RepairQty: currentRepairQty // 首次累加
                                                        //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.RepairQty += currentRepairQty;
                        //if (!_appContext.SysConfig.CheckNegativeInventory && (group.Inventory.Quantity + group.RepairQty) < 0)
                        //{
                        //    // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                        //    rmrs.ErrorMsg = $"sku:{group.Inventory.tb_proddetail.SKU}库存为：{group.Inventory.Quantity}，维修领料数量为：{group.RepairQty}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据";
                        //    await _unitOfWorkManage.RollbackTranAsync();
                        //    rmrs.Succeeded = false;
                        //    return rmrs;
                        //}
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                List<((tb_Inventory Inventory, decimal RepairQty) Group, int BeforeQty)> inventoryWithBeforeQty2 = new List<((tb_Inventory Inventory, decimal RepairQty) Group, int BeforeQty)>();
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    
                    // ✅ P0修复：保存更新前的数量快照
                    int beforeQty = inv.Quantity;
                    
                    // ✅ P1修复：维修领料反审核时也需要回滚成本（增加库存）
                    // 注意：先调用成本计算，再更新数量，保持时序一致
                    if (inv.Quantity > 0 || group.Value.RepairQty.ToInt() > 0)
                    {
                        // 使用当前库存成本或首次入库时的成本
                        decimal costForCalc = inv.Inv_Cost > 0 ? inv.Inv_Cost : GetRepairInventoryCost(inv.ProdDetailID);
                        
                        if (costForCalc > 0)
                        {
                            // ✅ P0修复：保存计算前的数量，用于验证
                            int qtyBeforeCostCalc = inv.Quantity;
                            
                            CommService.CostCalculations.CostCalculation(_appContext, inv, group.Value.RepairQty.ToInt(), costForCalc);
                            
                            // ✅ P0修复：验证成本计算未意外修改数量
                            if (inv.Quantity != qtyBeforeCostCalc)
                            {
                                throw new InvalidOperationException(
                                    $"成本计算意外修改了库存数量！产品ID={inv.ProdDetailID}, " +
                                    $"预期数量={qtyBeforeCostCalc}, 实际数量={inv.Quantity}");
                            }
                        }
                    }
                    
                    // ✅ P0修复：显式更新数量，保持时序一致
                    inv.Quantity = inv.Quantity + group.Value.RepairQty.ToInt();
                    invList.Add(inv);
                    
                    // ✅ P0修复：保存更新前后的信息用于流水记录
                    inventoryWithBeforeQty2.Add((group.Value, beforeQty));
                }
                if (entity.RepairOrderID.HasValue && entity.RepairOrderID.Value > 0)
                {
                    entity.tb_as_repairorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>()
                         .Includes(b => b.tb_AS_RepairOrderMaterialDetails)
                         .Where(c => c.RepairOrderID == entity.RepairOrderID)
                         .SingleAsync();

                    if (entity.tb_as_repairorder.RepairStatus != (int)RepairStatus.维修中)
                    {
                        throw new Exception($"维修领料单审核时，维修工单{entity.tb_as_repairorder.RepairOrderNo}状态不是维修中，请检查后再试！");
                    }

                    //更新维修工单中 材料明细的实发数量
                    for (int i = 0; i < entity.tb_AS_RepairMaterialPickupDetails.Count; i++)
                    {
                        var PickUpMaterial = entity.tb_AS_RepairMaterialPickupDetails[i];
                        if (PickUpMaterial.ActualSentQty == 0)
                        {
                            continue;
                        }
                        var detail = entity.tb_as_repairorder.tb_AS_RepairOrderMaterialDetails
                            .FirstOrDefault(c => c.ProdDetailID == PickUpMaterial.ProdDetailID && c.Location_ID == PickUpMaterial.Location_ID);
                        if (detail != null)
                        {
                            detail.ActualSentQty -= PickUpMaterial.ActualSentQty;
                        }
                        if (detail.ActualSentQty < 0)
                        {
                            throw new Exception($"维修领料单反审核时，明细中实发数量{detail.ActualSentQty}，不能小于零，请检查后再试！");
                        }
                    }

                    await _unitOfWorkManage.BeginTranAsync();
                    var MaterialQtyResult = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_repairorder.tb_AS_RepairOrderMaterialDetails).UpdateColumns(it => new
                    {
                        it.ActualSentQty,
                    }).ExecuteCommandAsync();


                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                    if (Counter == 0)
                    {
                        await _unitOfWorkManage.RollbackTranAsync();
                        throw new Exception("库存更新数据为0，更新失败！");
                    }

                    // ✅ 新增: 记录反向库存流水(售后维修领料增加库存)
                    List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                    foreach (var item in inventoryWithBeforeQty2)
                    {
                        var group = item.Group;
                        int beforeQty = item.BeforeQty; // ✅ P0修复: 使用保存的快照
                        
                        if (group.Inventory != null)
                        {
                            decimal realtimeCost = group.Inventory.Inv_Cost;
                            
                            tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                            transaction.ProdDetailID = group.Inventory.ProdDetailID;
                            transaction.Location_ID = group.Inventory.Location_ID;
                            transaction.BizType = (int)BizType.维修领料单;
                            transaction.ReferenceId = entity.RMRID;
                            transaction.ReferenceNo = entity.MaterialPickupNO;
                            transaction.BeforeQuantity = beforeQty; // ✅ P0修复: 反审核前的数量(快照)
                            transaction.QuantityChange = group.RepairQty.ToInt(); // 反审核增加库存
                            transaction.AfterQuantity = beforeQty + group.RepairQty.ToInt(); // ✅ P0修复: 反审核后的数量
                            transaction.UnitCost = realtimeCost;
                            transaction.TransactionTime = DateTime.Now;
                            transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                            transaction.Notes = $"售后维修领料反审核：{entity.MaterialPickupNO}，产品：{group.Inventory.tb_proddetail?.tb_prod?.CNName}";
                            
                            transactionList.Add(transaction);
                        }
                    }
                    
                    if (transactionList.Any())
                    {
                        tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                        await tranController.BatchRecordTransactionsWithRetry(transactionList);
                    }
                    //领料与维修状态无关
                    entity.DataStatus = (int)DataStatus.新建;
                    entity.ApprovalStatus = null;
                    entity.ApprovalResults = null;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    //只更新指定列
                    var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                    {
                        it.DataStatus,
                        it.ApprovalResults,
                        it.ApprovalStatus,
                        it.Approver_at,
                        it.Approver_by,
                        it.ApprovalOpinions
                    }).ExecuteCommandAsync();
                }
                // 注意信息的完整性
                await _unitOfWorkManage.CommitTranAsync();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long Pickup_MainID)
        {

            List<tb_AS_RepairMaterialPickup> list = await _appContext.Db.CopyNew().Queryable<tb_AS_RepairMaterialPickup>().Where(m => m.RMRID == Pickup_MainID)
                             .Includes(a => a.tb_as_repairorder, b => b.tb_customervendor)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_AS_RepairMaterialPickupDetails, b => b.tb_location)
                            .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_AS_RepairMaterialPickupDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_AS_RepairMaterialPickupDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

    }
}



