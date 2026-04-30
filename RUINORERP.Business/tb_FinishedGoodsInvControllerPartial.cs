
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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static log4net.Appender.ColoredConsoleAppender;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Business
{

    /// <summary>
    /// 缴库单审核
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class tb_FinishedGoodsInvController<T>
    {

        /// <summary>
        /// 审核，先判断是否结案，再更新状态
        /// 优化：事务区间最小化，查询操作移到事务外
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_FinishedGoodsInv entity = ObjectEntity as tb_FinishedGoodsInv;
            ReturnResults<T> rs = new ReturnResults<T>();
            
            #region 【事务外】预处理阶段 - 批量预加载所有需要的数据
            
            // 1. 预加载库存数据
            var allKeys = new List<(long ProdDetailID, long Location_ID)>();
            if (entity.tb_FinishedGoodsInvDetails != null)
            {
                foreach (var detail in entity.tb_FinishedGoodsInvDetails)
                {
                    allKeys.Add((detail.ProdDetailID, detail.Location_ID));
                }
            }

            var invDict1 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
            // ✅ 修复: 保存库存快照(在修改前),用于后续记录流水
            var invSnapshotDict = new Dictionary<(long ProdDetailID, long Location_ID), (int BeforeQuantity, decimal Inv_Cost)>();
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
                    invSnapshotDict[key] = (inv.Quantity, inv.Inv_Cost);
                }
            }

            // 2. 预加载缴库单明细
            if (entity.tb_FinishedGoodsInvDetails == null)
            {
                entity.tb_FinishedGoodsInvDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>()
                    .Where(c => c.FG_ID == entity.FG_ID)
                    .ToListAsync();
            }

            // 3. 预加载制令单完整数据（包括关联数据）
            tb_ManufacturingOrder manufacturingOrder = null;
            if (entity.MOID > 0)
            {
                manufacturingOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                    .Includes(b => b.tb_proddetail, c => c.tb_prod)
                    .Includes(b => b.tb_bom_s, c => c.tb_BOM_SDetails)
                    .AsNavQueryable()
                    .Includes(d => d.tb_ManufacturingOrderDetails, e => e.tb_bom_s, c => c.tb_BOM_SDetails, f => f.tb_BOM_SDetailSubstituteMaterials)
                    .Includes(b => b.tb_productiondemand, c => c.tb_productionplan, d => d.tb_ProductionPlanDetails)
                    .Includes(b => b.tb_MaterialRequisitions, c => c.tb_MaterialRequisitionDetails)
                    .Includes(a => a.tb_FinishedGoodsInvs, b => b.tb_FinishedGoodsInvDetails)
                    .Where(c => c.MOID == entity.MOID)
                    .SingleAsync();
            }

            // 4. 预加载BOM明细
            var bomDetailDict = new Dictionary<long, tb_BOM_SDetail>();
            if (entity.MOID > 0 && entity.tb_FinishedGoodsInvDetails != null && manufacturingOrder != null && manufacturingOrder.BOM_ID > 0)
            {
                var prodDetailIds = entity.tb_FinishedGoodsInvDetails.Select(d => d.ProdDetailID).Distinct().ToList();
                if (prodDetailIds.Any())
                {
                    var bomDetails = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_BOM_SDetail>()
                        .Where(d => prodDetailIds.Contains(d.ProdDetailID) && d.BOM_ID == manufacturingOrder.BOM_ID)
                        .ToListAsync();
                    
                    bomDetailDict = bomDetails.ToDictionary(d => d.ProdDetailID);
                    _logger.LogDebug($"✅ 缴库单审核预加载BOM明细: {bomDetails.Count}条记录");
                }
            }

            // 5. 预加载需求分析单和计划单数据（如果需要）
            tb_ProductionDemand productionDemand = null;
            if (manufacturingOrder != null && manufacturingOrder.PDID > 0)
            {
                productionDemand = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                    .AsNavQueryable()
                    .Includes(a => a.tb_productionplan, b => b.tb_ProductionPlanDetails)
                    .Where(c => c.PDID == manufacturingOrder.PDID)
                    .SingleAsync();
            }
            
            #endregion
            
            // 将预加载的数据赋值给实体
            entity.tb_manufacturingorder = manufacturingOrder;
            
            #region 【事务外】验证阶段 - 所有业务规则验证在事务外执行
            
            // 验证0: 检查制令单是否存在
            if (entity.tb_manufacturingorder == null)
            {
                rs.Succeeded = false;
                rs.ErrorMsg = $"未找到对应的制令单(MOID={entity.MOID})!请检查数据后重试！";
                _logger.LogWarning(rs.ErrorMsg);
                return rs;
            }
            
            // 验证1: 检查缴库明细是否属于当前制令单
            if (!entity.tb_FinishedGoodsInvDetails.Any(c => 
                c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID && 
                c.Location_ID == entity.tb_manufacturingorder.Location_ID))
            {
                rs.Succeeded = false;
                rs.ErrorMsg = $"缴库明细中有不属于当前制令单生产的产品及对应仓库!请检查数据后重试！";
                _logger.LogWarning(rs.ErrorMsg);
                return rs;
            }
            
            // 验证2: 检查制令单是否有物料发出
            if (entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Sum(c => c.ActualSentQty) == 0)
            {
                rs.Succeeded = false;
                rs.ErrorMsg = $"制令单:{entity.tb_manufacturingorder.MONO}，没有任何物料发出。";
                _logger.LogWarning(rs.ErrorMsg);
                return rs;
            }
            
            // 验证3: 计算缴库数量并验证
            List<tb_FinishedGoodsInvDetail> detailList = new List<tb_FinishedGoodsInvDetail>();
            foreach (var item in entity.tb_manufacturingorder.tb_FinishedGoodsInvs.Where(c => 
                c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结).ToList())
            {
                detailList.AddRange(item.tb_FinishedGoodsInvDetails);
            }
            detailList.AddRange(entity.tb_FinishedGoodsInvDetails);
            
            var PaidQuantity = detailList.Where(c => 
                c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID && 
                c.Location_ID == entity.tb_manufacturingorder.Location_ID).Sum(c => c.Qty);
            
            // 验证3.1: 缴库数量不能大于制令单生产数量
            if (PaidQuantity > entity.tb_manufacturingorder.ManufacturingQty)
            {
                string prodName = entity.tb_manufacturingorder.tb_proddetail.tb_prod.CNName + 
                    entity.tb_manufacturingorder.tb_proddetail.tb_prod.Specifications;
                rs.Succeeded = false;
                rs.ErrorMsg = $"制令单:{entity.tb_manufacturingorder.MONO}的【{prodName}】的缴库数量不能大于制令单中要生产的数量。";
                _logger.LogWarning(rs.ErrorMsg);
                return rs;
            }
            
            // 验证3.2: 根据BOM计算最大可缴库数量
            // ✅ 修复: 累加所有审核通过的领料单中的关键物料实发数量(支持多次领料)
            decimal CanManufactureQtyBybom = 0;
            
            if (entity.tb_manufacturingorder.tb_MaterialRequisitions != null && entity.tb_manufacturingorder.tb_MaterialRequisitions.Any())
            {
                // 获取所有审核通过的领料单明细
                var approvedMaterialDetails = entity.tb_manufacturingorder.tb_MaterialRequisitions
                    .Where(mr => mr.DataStatus == (int)DataStatus.确认 || mr.ApprovalStatus == (int)ApprovalStatus.审核通过)
                    .SelectMany(mr => mr.tb_MaterialRequisitionDetails ?? new List<tb_MaterialRequisitionDetail>())
                    .ToList();
                
                if (approvedMaterialDetails.Any() && entity.tb_manufacturingorder.tb_ManufacturingOrderDetails != null)
                {
                    // 从制令单明细中获取关键物料列表
                    var keyMaterialProdDetailIds = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails
                        .Where(d => d.IsKeyMaterial.HasValue && d.IsKeyMaterial.Value == true)
                        .Select(d => d.ProdDetailID)
                        .Distinct()
                        .ToList();
                    
                    if (keyMaterialProdDetailIds.Any())
                    {
                        // 按物料分组,计算每个关键物料的总实发数量
                        var keyMaterialSentQtyDict = approvedMaterialDetails
                            .Where(d => keyMaterialProdDetailIds.Contains(d.ProdDetailID) && d.ActualSentQty > 0)
                            .GroupBy(d => d.ProdDetailID)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Sum(d => d.ActualSentQty)
                            );
                        
                        // ✅ 检查是否所有关键物料都有领料记录
                        var missingKeyMaterials = keyMaterialProdDetailIds
                            .Except(keyMaterialSentQtyDict.Keys)
                            .ToList();
                        
                        if (missingKeyMaterials.Any())
                        {
                            // 有必填的关键物料未领料,给出警告
                            string missingMaterials = string.Join(",", missingKeyMaterials.Take(3));
                            _logger.LogWarning($"制令单{entity.tb_manufacturingorder.MONO}有{missingKeyMaterials.Count}个关键物料未领料: {missingMaterials}");
                            
                            // 可以选择阻断或仅警告,这里选择允许继续但设置为0表示无法生产
                            CanManufactureQtyBybom = 0;
                        }
                        else if (keyMaterialSentQtyDict.Any())
                        {
                            // 找到限制产量的关键物料(能生产的最小数量)
                            decimal minCanProduceQty = decimal.MaxValue;
                            long limitingProdDetailId = 0;
                            
                            foreach (var kvp in keyMaterialSentQtyDict)
                            {
                                long prodDetailId = kvp.Key;
                                decimal totalSentQty = kvp.Value;
                                
                                // 从BOM中找到该物料的用量
                                var bomDetail = entity.tb_manufacturingorder.tb_bom_s?.tb_BOM_SDetails
                                    ?.FirstOrDefault(b => b.ProdDetailID == prodDetailId);
                                
                                if (bomDetail != null && bomDetail.UsedQty > 0)
                                {
                                    // 计算该物料能生产的成品数量 = 实发数量 / BOM用量 * BOM产出量
                                    decimal canProduceQty = totalSentQty / bomDetail.UsedQty.ToDecimal() * entity.tb_manufacturingorder.tb_bom_s.OutputQty;
                                    
                                    if (canProduceQty < minCanProduceQty)
                                    {
                                        minCanProduceQty = canProduceQty;
                                        limitingProdDetailId = prodDetailId;
                                    }
                                }
                            }
                            
                            if (minCanProduceQty < decimal.MaxValue)
                            {
                                CanManufactureQtyBybom = minCanProduceQty;
                                
                                // 如果总缴库数量大于最小制成数量则返回需要用户确认
                                if (PaidQuantity > CanManufactureQtyBybom)
                                {
                                    string msg = $"系统检测到缴库数量大于发出的关键物料能生产的最小数量";
                                    try
                                    {
                                        if (limitingProdDetailId > 0)
                                        {
                                            object obj = _cacheManager.GetEntity<View_ProdDetail>(limitingProdDetailId);
                                            if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                                            {
                                                msg += $"\r\n{prodDetail.SKU}:{prodDetail.CNName}总实发数量={keyMaterialSentQtyDict[limitingProdDetailId]},不够生产{CanManufactureQtyBybom}";
                                            }
                                        }
                                    }
                                    catch (Exception tipEx)
                                    {
                                        _logger.Error(tipEx);
                                    }
                                    
                                    return new ReturnResults<T>
                                    {
                                        Succeeded = false,
                                        ErrorMsg = $"NEED_CONFIRM:缴库数量超额确认:{msg}\r\n\r\n你确定要审核通过吗？"
                                    };
                                }
                            }
                        }
                    }
                }
            }
            
            // 验证4: 检查已交数量是否超过缴库总数
            // 注意：此验证已在验证3.1中覆盖(PaidQuantity <= ManufacturingQty)，这里可以省略
            // 保留注释以备后续审计
            
            // 验证5: 计划单数量限制检查(移到事务外,避免事务开启后回滚)
            // ✅ 优化: 提前检查计划单数量是否超过限制,减少事务持有时间
            if (productionDemand != null && entity.tb_manufacturingorder.PDID > 0)
            {
                // 验证5.1: 检查计划明细数量是否超过1.5倍
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    tb_ProductionPlanDetail planDetail = productionDemand.tb_productionplan.tb_ProductionPlanDetails
                        .FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID);
                    if (planDetail != null)
                    {
                        decimal newCompletedQty = planDetail.CompletedQuantity + child.Qty;
                        if (newCompletedQty > (planDetail.Quantity * 1.5m))
                        {
                            rs.Succeeded = false;
                            rs.ErrorMsg = $"缴库明细中有完成数量大于计划数量的产品1.5倍，系统认为异常数量!请检查后重试！";
                            _logger.LogWarning(rs.ErrorMsg + $" 计划单:{productionDemand.PPNo}, 产品:{child.ProdDetailID}, 计划数量:{planDetail.Quantity}, 新完成数量:{newCompletedQty}");
                            return rs;
                        }
                    }
                }
                
                // 验证5.2: 检查计划总数量是否超过1.5倍
                int totalPlanCompletedQuantity = productionDemand.tb_productionplan.tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity);
                int newTotalCompletedQuantity = totalPlanCompletedQuantity + entity.tb_FinishedGoodsInvDetails.Sum(c => c.Qty);
                if (newTotalCompletedQuantity > (productionDemand.tb_productionplan.TotalQuantity * 1.5))
                {
                    rs.Succeeded = false;
                    rs.ErrorMsg = $"缴库数量大于计划数量超过1.5倍!请检查数据后重试！";
                    _logger.LogWarning(rs.ErrorMsg + $" 计划单:{productionDemand.PPNo}, 计划总数:{productionDemand.tb_productionplan.TotalQuantity}, 新完成总数:{newTotalCompletedQuantity}");
                    return rs;
                }
            }
            
            #endregion
            
            // 【事务开始】只包含更新操作，最小化事务区间
            try
            {
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                await _unitOfWorkManage.BeginTranAsync();
      
                #region 由缴库更新库存

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {

                    #region 库存表的更新 这里应该是必需有库存的数据，
                    // ✅ 从预加载字典获取（死锁优化）
                    var key = (child.ProdDetailID, child.Location_ID);
                    invDict1.TryGetValue(key, out var inv);
                    if (inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.InitInventory =0;
                        inv.Notes = "由缴库时自动生成";//后面修改数据库是不需要？
                        inv.Inv_Cost = child.UnitCost;
                        inv.Inv_AdvCost = child.UnitCost;
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    else
                    {

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

                    //这里减掉在制的数量
                    inv.MakingQty = inv.MakingQty - child.Qty;

                    // 直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                    //后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                    //数据来源可以是多种多样的，例如：
                    //采购价格：从供应商处购买产品或物品时的价格。
                    //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                    //市场价格：参考市场上类似产品或物品的价格。

                    //TODO:这里需要根据系统设置中的算法计算。
                    // child.UnitCost = child.MaterialCost+child.LaborCost+child.

                    //定制订单时不影响标准配方的产品成本。这里是特别处理了。定制单使用了标准配方的BOM时。缴库只交数量不影响成本！！
                    if (!entity.tb_manufacturingorder.IsCustomizedOrder)
                    {
                        // ✅ 核心规则：缴库单审核时，必须直接取用 BOM 配方明细中的 RealTimeCost 作为成品入库成本
                        // 禁止从制令单中读取或继承成本值（制令单只是BOM成本的静态快照）
                        decimal effectiveCost = child.UnitCost; // 默认使用制令单成本(兼容旧数据)
                        
                        // ✅ P1优化: 使用预加载的BOM明细字典(避免循环中查询数据库)
                        if (bomDetailDict.TryGetValue(child.ProdDetailID, out var bomDetail))
                        {
                            // 优先级: RealTimeCost > UnitCost
                            if (bomDetail.RealTimeCost.HasValue && bomDetail.RealTimeCost.Value > 0)
                            {
                                effectiveCost = bomDetail.RealTimeCost.Value;  // ✅ 直接使用BOM实时成本
                            }
                            else if (bomDetail.UnitCost > 0)
                            {
                                effectiveCost = bomDetail.UnitCost;
                            }
                        }
                        
                        // ✅ P0修复: 成本波动检测(超过20%时记录警告)
                        if (inv.Quantity > 0 && inv.Inv_Cost > 0)
                        {
                            decimal costChangeRate = Math.Abs((inv.Inv_Cost - effectiveCost) / inv.Inv_Cost);
                            if (costChangeRate > 0.2m)
                            {
                                _logger.LogWarning(
                                    $"⚠️ 产品{child.ProdDetailID}成本波动超过20%: " +
                                    $"原库存成本={inv.Inv_Cost:F4}, " +
                                    $"新缴库成本={effectiveCost:F4}, " +
                                    $"波动率={costChangeRate:P2}, " +
                                    $"缴库单号={entity.DeliveryBillNo}");
                                
                                // 可选: 根据配置决定是否阻断
                                // if (_appContext.SysConfig.EnableCostChangeBlock)
                                // {
                                //     throw new Exception($"成本波动过大({costChangeRate:P2}),请核实后重新缴库!");
                                // }
                            }
                        }
                        
                        // ✅ P0修复：保存计算前的数量，用于验证
                        int qtyBeforeCostCalc = inv.Quantity;
                        
                        CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, effectiveCost);
                        
                        // ✅ P0修复：验证成本计算未意外修改数量
                        if (inv.Quantity != qtyBeforeCostCalc)
                        {
                            throw new InvalidOperationException(
                                $"成本计算意外修改了库存数量！产品ID={child.ProdDetailID}, " +
                                $"预期数量={qtyBeforeCostCalc}, 实际数量={inv.Quantity}");
                        }
                    }

                    // ✅ P0修复：显式更新数量，保持时序一致
                    inv.Quantity = inv.Quantity + child.Qty;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion

                    invUpdateList.Add(inv);

                }
                #endregion
                
                // ✅ 验证4: 检查库存更新列表中是否有重复的商品
                var CheckNewInvList = invUpdateList
                    .GroupBy(i => new { i.ProdDetailID, i.Location_ID })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key.ProdDetailID)
                    .ToList();

                if (CheckNewInvList.Count > 0)
                {
                    rs.ErrorMsg = "新增库存中有重复的商品，操作失败。";
                    rs.Succeeded = false;
                    await _unitOfWorkManage.RollbackTranAsync();
                    _logger.LogError(rs.ErrorMsg + "详细信息：" + string.Join(",", CheckNewInvList));
                    return rs;
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvMainCounter == 0)
                {
                    _logger.Debug($"{entity.DeliveryBillNo}缴库更新库存结果为0行，请检查数据！");
                }

                // ✅ 新增: 记录库存流水(缴库增加库存)
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    // ✅ P0修复: 使用修改前保存的快照字典获取正确的数据
                    if (invSnapshotDict.TryGetValue(key, out var snapshot))
                    {
                        // 创建库存流水记录
                        tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                        transaction.ProdDetailID = child.ProdDetailID;
                        transaction.Location_ID = child.Location_ID;
                        transaction.BizType = (int)BizType.缴库单;
                        transaction.ReferenceId = entity.FG_ID;
                        transaction.ReferenceNo = entity.DeliveryBillNo;
                        transaction.BeforeQuantity = snapshot.BeforeQuantity; // ✅ 使用修改前的快照数量
                        transaction.QuantityChange = child.Qty; // 缴库增加库存
                        transaction.AfterQuantity = snapshot.BeforeQuantity + child.Qty; // ✅ 更新后的数量
                        transaction.UnitCost = snapshot.Inv_Cost; // 使用更新前的成本
                        transaction.TransactionTime = DateTime.Now;
                        transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        
                        // 获取产品名称
                        invDict1.TryGetValue(key, out var invForName1);
                        transaction.Notes = $"缴库单审核：{entity.DeliveryBillNo}，产品：{invForName1?.tb_proddetail?.tb_prod?.CNName}";
                        
                        transactionList.Add(transaction);
                    }
                }
                
                // 批量记录库存流水(带死锁重试机制)
                if (transactionList.Any())
                {
                    tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                    await tranController.BatchRecordTransactionsWithRetry(transactionList);
                }

                // ✅ 性能优化: 批量更新BOM成本(在库存更新后,减少循环中的数据库操作)
                // 注意: 必须在事务内执行,以保证与库存更新的原子性
                if (!entity.tb_manufacturingorder.IsCustomizedOrder)
                {
                    var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                    var updatedProdDetailIds = invUpdateList.Select(i => i.ProdDetailID).Distinct().ToList();
                    
                    foreach (var prodDetailId in updatedProdDetailIds)
                    {
                        var inv = invUpdateList.FirstOrDefault(i => i.ProdDetailID == prodDetailId);
                        if (inv != null && inv.Inv_Cost > 0)
                        {
                            await ctrbom.UpdateParentBOMsAsync(prodDetailId, inv.Inv_Cost);
                        }
                    }
                }

                #region 由缴库单更新制令单
                if (entity.tb_manufacturingorder.tb_FinishedGoodsInvs == null)
                {
                    entity.tb_manufacturingorder.tb_FinishedGoodsInvs = new List<tb_FinishedGoodsInv>();
                }
                
                // 注意：detailList和PaidQuantity已在事务外计算，这里直接使用
                // 当前行累计到交付,只是当前单的。不是以前的。
                var RowQty = entity.tb_FinishedGoodsInvDetails.Where(c => 
                    c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID && 
                    c.Location_ID == entity.tb_manufacturingorder.Location_ID).Sum(c => c.Qty);
                entity.tb_manufacturingorder.QuantityDelivered += RowQty;

                // ✅ P0修复：制令单已交数量和判断是否结案 - 避免竞态条件
                // 重新查询制令单的最新状态,确保数据一致性
                var latestMO = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_ManufacturingOrder>()
                    .Where(m => m.MOID == entity.tb_manufacturingorder.MOID)
                    .FirstAsync();
                
                // 使用容差比较替代精确相等,避免浮点数精度问题
                if (Math.Abs(latestMO.QuantityDelivered + RowQty - latestMO.ManufacturingQty) < 0.0001m
                    && latestMO.DataStatus == (int)DataStatus.确认) // ✅ 检查制令单当前状态,而非缴库单状态
                {
                    entity.tb_manufacturingorder.DataStatus = (int)DataStatus.完结;
                    entity.tb_manufacturingorder.CloseCaseOpinions = $"缴库单:{entity.DeliveryBillNo}->制令单:{entity.tb_manufacturingorder.MONO},缴库单审核时，生产数量等于交付数量，自动结案";

                    //修改领料单状态 系统认为制令单已完成时。领料单也会结案
                    //但是有个前提是实发数据大于等于（有超发情况） 应该发的数量。并且是审核通过时
                    entity.tb_manufacturingorder.tb_MaterialRequisitions.Where(c => c.DataStatus == (int)DataStatus.确认 && entity.ApprovalStatus == (int)ApprovalStatus.审核通过).ToList().ForEach(c => c.DataStatus = (int)DataStatus.完结);
                    int pomrCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisition>(entity.tb_manufacturingorder.tb_MaterialRequisitions).ExecuteCommandAsync();
                    if (pomrCounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Debug(entity.DeliveryBillNo + "==>" + entity.MONo + $"对应 的所有领料单设置为结案。将不能再发料 更新成功===重点代码 看已交数量是否正确");
                        }
                    }
                }
                

                //更新制令单已交数量和判断是否结案
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrder>(entity.tb_manufacturingorder).ExecuteCommandAsync();
                if (poCounter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Debug(entity.DeliveryBillNo + "==>" + entity.MONo + $"对应 的制令单已交数量 更新成功===重点代码 看已交数量是否正确");
                    }
                }

                #endregion

                //更新计划单已交数量，制令单会引用需求分析，需求分析引用计划单
                // ✅ 优化：使用事务外预加载的 productionDemand 数据
                // 注意：计划单数量限制检查已在事务外完成(验证5)，这里只执行更新操作
                if (productionDemand != null && entity.tb_manufacturingorder.PDID > 0)
                {
                    //一个缴款单上面一个制令单。一个制令单 找到 需求单，再找到计划单。但是：需求下有多个制令单都来自于一个计划单，
                    //所以这里要循环加总保存到计划单中。
                    #region 更新计划单的完成数量
                    //标记一下，如果计划单明细有变化，则更新计划单明细
                    bool PlanDetailHasChanged = false;

                    foreach (var child in entity.tb_FinishedGoodsInvDetails)
                    {
                        tb_ProductionPlanDetail planDetail = productionDemand.tb_productionplan.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID);
                        if (planDetail != null)
                        {
                            //按理计划这样保存也是总数量。
                            planDetail.CompletedQuantity = planDetail.CompletedQuantity + child.Qty;
                            // ✅ 注意：数量限制检查已在事务外验证阶段完成(验证5)，这里不再重复检查
                            PlanDetailHasChanged = true;
                        }
                    }
                    if (PlanDetailHasChanged)
                    {
                        //更新计划单已交数量
                        int jkCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlanDetail>(productionDemand.tb_productionplan.tb_ProductionPlanDetails).ExecuteCommandAsync();
                        if (jkCounter > 0)
                        {
                            if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                            {
                                _logger.Debug(productionDemand.PPNo + $"对应的计划明细中完成数量更新成功===重点代码 看已交数量是否正确");
                            }
                        }
                    }


                    //标记一下，如果计划单有变化，则更新计划单
                    bool PlanHasChanged = false;
                    int totalPlanCompletedQuantity = productionDemand.tb_productionplan.tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity);
                    if (totalPlanCompletedQuantity != productionDemand.tb_productionplan.TotalCompletedQuantity)
                    {
                        productionDemand.tb_productionplan.TotalCompletedQuantity = totalPlanCompletedQuantity;
                        // ✅ 注意：总数限制检查已在事务外验证阶段完成(验证5.2)，这里不再重复检查
                        PlanHasChanged = true;
                    }

                    //如果计划数量等于已完成数量 结案？   完成数量大于等于计划算结案。 意思是制令单时可以修改计划数量。缴库根据制令单数量来
                    if (productionDemand.tb_productionplan.TotalQuantity <= productionDemand.tb_productionplan.TotalCompletedQuantity
                        && productionDemand.tb_productionplan.DataStatus == (int)DataStatus.确认)
                    {
                        productionDemand.tb_productionplan.DataStatus = (int)DataStatus.完结;
                        PlanHasChanged = true;
                    }
                    if (PlanHasChanged)
                    {
                        await _unitOfWorkManage.GetDbClient().Updateable(productionDemand.tb_productionplan).UpdateColumns(t => new { t.DataStatus, t.TotalCompletedQuantity }).ExecuteCommandAsync();
                    }


                    #endregion
                }

                if (entity.IsOutSourced)
                {
                    AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        //生成加工费用的应付款单   ,加工费一般不会预付，所以不会抵扣，要处理也只是在审核完后。这里不会处理
                        try
                        {
                            #region 生成应付
                            var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                            tb_FM_ReceivablePayable Payable = await ctrpayable.BuildReceivablePayable(entity, false);
                            ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(Payable, false);
                            if (rmr.Succeeded)
                            {
                                //已经是等审核。 审核时会核销预收付款
                                rs.ReturnObjectAsOtherEntity = rmr.ReturnObject;
                            }
                            #endregion
                        }
                        catch (Exception)
                        {
                            await _unitOfWorkManage.RollbackTranAsync();
                            throw new Exception("缴库时，生成加工费用的应付款单处理失败！");
                        }
                    }

                }


                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions,
                                it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
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
                rs.ErrorMsg = ex.Message;
                return rs;
            }

        }

        /// <summary>
        /// 从制令单创建缴库单(核心转换逻辑)
        /// 该方法封装了制令单到缴库单的完整转换逻辑,供UI层和转换器复用
        /// </summary>
        /// <param name="manufacturingOrder">源制令单</param>
        /// <returns>转换后的缴库单草稿对象</returns>
        public async Task<ReturnResults<tb_FinishedGoodsInv>> CreateFromManufacturingOrderAsync(tb_ManufacturingOrder manufacturingOrder)
        {
            var rs = new ReturnResults<tb_FinishedGoodsInv>();
            
            try
            {
                // 1. 参数验证
                if (manufacturingOrder == null)
                {
                    rs.Succeeded = false;
                    rs.ErrorMsg = "制令单不能为空";
                    return rs;
                }
                
                // 2. 业务规则验证
                if (manufacturingOrder.DataStatus != (int)DataStatus.确认)
                {
                    rs.Succeeded = false;
                    rs.ErrorMsg = "只能从已确认的制令单生成缴库单";
                    return rs;
                }
                
                if (manufacturingOrder.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    rs.Succeeded = false;
                    rs.ErrorMsg = "只能从已审核通过的制令单生成缴库单";
                    return rs;
                }
                
                // 3. 检查是否已全部缴库
                if (manufacturingOrder.QuantityDelivered >= manufacturingOrder.ManufacturingQty)
                {
                    rs.Succeeded = false;
                    rs.ErrorMsg = $"制令单:{manufacturingOrder.MONO}已全部缴库,无需再生成缴库单";
                    return rs;
                }
                
                // 4. 创建缴库单主表
                var finishedGoodsInv = new tb_FinishedGoodsInv();
                
                // 4.1 基础映射
                finishedGoodsInv.MOID = manufacturingOrder.MOID;
                finishedGoodsInv.MONo = manufacturingOrder.MONO;
                finishedGoodsInv.DepartmentID = manufacturingOrder.DepartmentID;
                finishedGoodsInv.Employee_ID = manufacturingOrder.Employee_ID;
                finishedGoodsInv.IsOutSourced = manufacturingOrder.IsOutSourced;
                
                // 4.2 设置外发工厂
                if (manufacturingOrder.IsOutSourced)
                {
                    finishedGoodsInv.CustomerVendor_ID = manufacturingOrder.CustomerVendor_ID_Out;
                }
                else
                {
                    finishedGoodsInv.CustomerVendor_ID = null;
                }
                
                // 4.3 初始化状态字段
                finishedGoodsInv.DataStatus = (int)DataStatus.草稿;
                finishedGoodsInv.ApprovalStatus = (int)ApprovalStatus.未审核;
                finishedGoodsInv.ApprovalResults = null;
                finishedGoodsInv.ApprovalOpinions = "";
                finishedGoodsInv.PrintStatus = 0;
                finishedGoodsInv.ActionStatus = ActionStatus.新增;
                finishedGoodsInv.DeliveryDate = DateTime.Now;
                finishedGoodsInv.Notes = $"由制令单{manufacturingOrder.MONO}生成";
                
                // 4.4 初始化实体(设置创建时间等)
                BusinessHelper.Instance.InitEntity(finishedGoodsInv);
                
                // 5. 创建缴库单明细
                var newDetails = new List<tb_FinishedGoodsInvDetail>();
                var tipsMsg = new List<string>();
                
                // 5.1 创建明细行(一个制令单对应一个成品,一行明细)
                var newDetail = new tb_FinishedGoodsInvDetail();
                
                // 5.2 计算应缴数量
                newDetail.PayableQty = manufacturingOrder.ManufacturingQty - manufacturingOrder.QuantityDelivered;
                newDetail.Qty = 0; // 实缴数量初始化为0,由用户手动输入
                newDetail.UnpaidQty = newDetail.PayableQty - newDetail.Qty;
                newDetail.Location_ID = manufacturingOrder.Location_ID;
                newDetail.ProdDetailID = manufacturingOrder.ProdDetailID;
                
                // 5.3 计算单位成本(按生产数量平均分摊)
                if (manufacturingOrder.ManufacturingQty > 0)
                {
                    newDetail.NetWorkingHours = Math.Round(manufacturingOrder.WorkingHour / manufacturingOrder.ManufacturingQty, 4);
                    newDetail.NetMachineHours = Math.Round(manufacturingOrder.MachineHour / manufacturingOrder.ManufacturingQty, 4);
                    newDetail.MaterialCost = Math.Round(manufacturingOrder.TotalMaterialCost / manufacturingOrder.ManufacturingQty, 4);
                    newDetail.ManuFee = Math.Round(manufacturingOrder.TotalManuFee / manufacturingOrder.ManufacturingQty, 4);
                    newDetail.ApportionedCost = Math.Round(manufacturingOrder.ApportionedCost / manufacturingOrder.ManufacturingQty, 4);
                    
                    newDetail.UnitCost = newDetail.MaterialCost + newDetail.ManuFee + newDetail.ApportionedCost;
                    newDetail.ProductionAllCost = Math.Round(newDetail.UnitCost * newDetail.Qty, 4);
                }
                
                // 5.4 添加到明细列表
                if (newDetail.PayableQty > 0)
                {
                    newDetails.Add(newDetail);
                }
                else
                {
                    tipsMsg.Add($"制令单:{manufacturingOrder.MONO}已全部缴库,请检查数据!");
                }
                
                // 5.5 设置明细集合
                finishedGoodsInv.tb_FinishedGoodsInvDetails = newDetails;
                
                // 5.6 关联制令单对象
                finishedGoodsInv.tb_manufacturingorder = manufacturingOrder;
                
                // 6. 返回结果
                rs.ReturnObject = finishedGoodsInv;
                rs.Succeeded = true;
                
                // 7. 记录提示信息(如果有)
                if (tipsMsg.Count > 0)
                {
                    // ⚠️ ReturnResults 没有 WarningMessages 属性，使用 ErrorMsg 记录提示
                    rs.ErrorMsg = string.Join("; ", tipsMsg);
                    _logger.LogWarning("制令单转换提示信息: {Tips}", string.Join("; ", tipsMsg));
                }
                
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从制令单创建缴库单失败,制令单号: {MONO}", manufacturingOrder?.MONO);
                rs.Succeeded = false;
                rs.ErrorMsg = $"转换失败: {ex.Message}";
                return rs;
            }
        }

        /// <summary>
        /// 反审核
        /// 优化：事务区间最小化，查询操作移到事务外
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_FinishedGoodsInv entity = ObjectEntity as tb_FinishedGoodsInv;
            ReturnResults<T> rs = new ReturnResults<T>();

            //判断是否能反审?
            if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
            {
                return rs;
            }

            #region 【事务外】预处理阶段 - 批量预加载所有需要的数据
            
            // 1. 预加载库存数据
            var allKeys2 = new List<(long ProdDetailID, long Location_ID)>();
            if (entity.tb_FinishedGoodsInvDetails != null)
            {
                foreach (var detail in entity.tb_FinishedGoodsInvDetails)
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

            // 2. 预加载制令单完整数据
            tb_ManufacturingOrder manufacturingOrder = null;
            if (entity.MOID > 0)
            {
                manufacturingOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                    .AsNavQueryable()
                    .Includes(b => b.tb_proddetail, c => c.tb_prod)
                    .Includes(a => a.tb_FinishedGoodsInvs, b => b.tb_FinishedGoodsInvDetails)
                    .Includes(b => b.tb_MaterialRequisitions, c => c.tb_MaterialRequisitionDetails)
                    .Where(c => c.MOID == entity.MOID)
                    .SingleAsync();
            }

            // 3. 预加载BOM明细
            var bomDetailDict2 = new Dictionary<long, tb_BOM_SDetail>();
            if (entity.MOID > 0 && entity.tb_FinishedGoodsInvDetails != null && manufacturingOrder != null && manufacturingOrder.BOM_ID > 0)
            {
                var prodDetailIds = entity.tb_FinishedGoodsInvDetails.Select(d => d.ProdDetailID).Distinct().ToList();
                if (prodDetailIds.Any())
                {
                    var bomDetails = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_BOM_SDetail>()
                        .Where(d => prodDetailIds.Contains(d.ProdDetailID) && d.BOM_ID == manufacturingOrder.BOM_ID)
                        .ToListAsync();
                    
                    bomDetailDict2 = bomDetails.ToDictionary(d => d.ProdDetailID);
                }
            }

            // 4. 预加载需求分析单和计划单数据（如果需要）
            tb_ProductionDemand productionDemand = null;
            if (manufacturingOrder != null && manufacturingOrder.PDID > 0)
            {
                productionDemand = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                    .AsNavQueryable()
                    .Includes(a => a.tb_productionplan, b => b.tb_ProductionPlanDetails)
                    .Where(c => c.PDID == manufacturingOrder.PDID)
                    .SingleAsync();
            }
            
            #endregion
            
            // 将预加载的数据赋值给实体
            entity.tb_manufacturingorder = manufacturingOrder;
            
            // 【事务开始】只包含更新操作，最小化事务区间
            try
            {
                await _unitOfWorkManage.BeginTranAsync();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 反审时 期初已经有数据
                    // ✅ 从预加载字典获取（死锁优化）
                    var key = (child.ProdDetailID, child.Location_ID);
                    invDict2.TryGetValue(key, out var inv);
                    BusinessHelper.Instance.EditEntity(inv);
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestOutboundTime = System.DateTime.Now;
                    inv.MakingQty = inv.MakingQty + child.Qty;
                    
                    // ✅ P1修复: 反审核时从BOM获取成本(优先级: RealTimeCost > UnitCost)
                    decimal effectiveCost = child.UnitCost; // 默认值
                    if (bomDetailDict2.TryGetValue(child.ProdDetailID, out var bomDetail))
                    {
                        if (bomDetail.RealTimeCost.HasValue && bomDetail.RealTimeCost.Value > 0)
                        {
                            effectiveCost = bomDetail.RealTimeCost.Value;
                        }
                        else if (bomDetail.UnitCost > 0)
                        {
                            effectiveCost = bomDetail.UnitCost;
                        }
                    }
                    
                    CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Qty, effectiveCost);
                    inv.Quantity = inv.Quantity - child.Qty;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    invUpdateList.Add(inv);

                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvMainCounter == 0)
                {
                    _logger.Debug($"{entity.DeliveryBillNo}更新库存结果为0行，请检查数据！");
                }

                // ✅ 新增: 记录反向库存流水(缴库减少库存)
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    // ✅ P0修复: 使用修改前保存的快照字典获取正确的数据
                    if (invSnapshotDict2.TryGetValue(key, out var snapshot))
                    {
                        // 创建反向库存流水记录
                        tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                        transaction.ProdDetailID = child.ProdDetailID;
                        transaction.Location_ID = child.Location_ID;
                        transaction.BizType = (int)BizType.缴库单;
                        transaction.ReferenceId = entity.FG_ID;
                        transaction.ReferenceNo = entity.DeliveryBillNo;
                        transaction.BeforeQuantity = snapshot.BeforeQuantity; // ✅ 使用修改前的快照数量
                        transaction.QuantityChange = -child.Qty; // 反审核减少库存
                        transaction.AfterQuantity = snapshot.BeforeQuantity - child.Qty; // ✅ 更新后的数量
                        transaction.UnitCost = snapshot.Inv_Cost; // 使用更新前的成本
                        transaction.TransactionTime = DateTime.Now;
                        transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        
                        // 获取产品名称
                        invDict2.TryGetValue(key, out var invForName2);
                        transaction.Notes = $"缴库单反审核：{entity.DeliveryBillNo}，产品：{invForName2?.tb_proddetail?.tb_prod?.CNName}";
                        
                        transactionList.Add(transaction);
                    }
                }
                
                // 批量记录反向库存流水(带死锁重试机制)
                if (transactionList.Any())
                {
                    tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                    await tranController.BatchRecordTransactionsWithRetry(transactionList);
                }

                // ✅ 优化: 缴库单反审核时，使用新的成本回滚方法(保留实时库存成本)
                var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    invDict2.TryGetValue(key, out var inv);
                    if (inv != null)
                    {
                        // ✅ 使用反审核后的库存成本,而非设为NULL
                        await ctrbom.RollbackParentBOMsCostWithInventoryAsync(child.ProdDetailID, inv.Inv_Cost);
                    }
                    else
                    {
                        // 如果找不到库存记录,使用明细中的成本作为回退
                        _logger.LogWarning("缴库单反审核时未找到库存记录,ProdDetailID={ProdDetailID},使用明细成本:{Cost}", 
                            child.ProdDetailID, child.UnitCost);
                        await ctrbom.RollbackParentBOMsCostWithInventoryAsync(child.ProdDetailID, child.UnitCost);
                    }
                }

                #region 处理制令单和计划单
                
                // ✅ 优化：使用事务外预加载的 manufacturingOrder 数据
                // 更新制令单的QuantityDelivered已交付数量 ,如果全交完了。则结案--的反操作

                if (entity.tb_manufacturingorder != null)
                {
                    #region  反审  退回  出库
                    //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。--反
                    List<tb_FinishedGoodsInvDetail> detailList = new List<tb_FinishedGoodsInvDetail>();
                    foreach (var item in entity.tb_manufacturingorder.tb_FinishedGoodsInvs)
                    {
                        detailList.AddRange(item.tb_FinishedGoodsInvDetails);
                    }

                    //这里与采购订单不一样。采购订单是用明细去比较，这里是回写的是制令单，是主表。
                    string prodName = entity.tb_manufacturingorder.tb_proddetail.tb_prod.CNName + entity.tb_manufacturingorder.tb_proddetail.tb_prod.Specifications;
                    //一对一时

                    //当前缴库行累计到交付
                    var RowQty = entity.tb_FinishedGoodsInvDetails.Where(c => c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID && c.Location_ID == entity.tb_manufacturingorder.Location_ID).Sum(c => c.Qty);
                    entity.tb_manufacturingorder.QuantityDelivered = entity.tb_manufacturingorder.QuantityDelivered - RowQty;
                    //如果已交数据大于制令单数量 给出警告实际操作中 使用其他方式将备品入库
                    if (entity.tb_manufacturingorder.QuantityDelivered < 0)
                    {
                        await _unitOfWorkManage.RollbackTranAsync();
                        throw new Exception($"缴库单：{entity.DeliveryBillNo}反审核时，对应的制令单：{entity.tb_manufacturingorder.MONO}，{prodName}的生产数量不能为负数！");
                    }


                    #endregion
                    if (entity.tb_manufacturingorder.QuantityDelivered != entity.tb_manufacturingorder.ManufacturingQty)
                    {
                        entity.tb_manufacturingorder.DataStatus = (int)DataStatus.确认;
                        entity.tb_manufacturingorder.CloseCaseOpinions = $"缴库单:{entity.DeliveryBillNo}->制令单:{entity.tb_manufacturingorder.MONO},缴库单反审时，生产数量不等于交付数量，取消自动结案";

                        //缴库的反审核  要不要影响领取料呢？  应该是不影响。因为 多次领取出来的。多次缴进去。没办法对应起来了。
                        //entity.tb_manufacturingorder.tb_MaterialRequisitions.Where(c => entity.ApprovalStatus == (int)ApprovalStatus.已审核).ToList().ForEach(c => c.DataStatus = (int)DataStatus.确认);
                        //int pomrCounter = await _unitOfWorkManage.GetDbClient()
                        //   .Updateable<tb_MaterialRequisition>(entity.tb_manufacturingorder.tb_MaterialRequisitions)
                        //   .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions })
                        //   .ExecuteCommandAsync();
                    }

                    //更新制令单的已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrder>(entity.tb_manufacturingorder)
                         .UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions, it.QuantityDelivered })
                        .ExecuteCommandAsync();
                    if (updatecounter == 0)
                    {

                    }
                }

                //更新计划单已交数量，制令单会引用需求分析，需求分析引用计划单
                // ✅ 优化：使用事务外预加载的 productionDemand 数据
                if (productionDemand != null && entity.tb_manufacturingorder.PDID > 0)
                {
                    foreach (var child in entity.tb_FinishedGoodsInvDetails)
                    {
                        tb_ProductionPlanDetail planDetail = productionDemand.tb_productionplan.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID);
                        if (planDetail != null)
                        {
                            planDetail.CompletedQuantity = planDetail.CompletedQuantity - child.Qty;
                        }
                    }
                    //更新计划单已交数量
                    int jkCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlanDetail>(productionDemand.tb_productionplan.tb_ProductionPlanDetails).ExecuteCommandAsync();
                    if (jkCounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Debug(productionDemand.PPNo + $"对应的计划明细中完成数量反审核 更新成功===重点代码 看已交数量是否正确");
                        }
                    }

                    productionDemand.tb_productionplan.TotalCompletedQuantity = productionDemand.tb_productionplan.tb_ProductionPlanDetails.Sum(c => c.CompletedQuantity);
                    //如果计划数量等于已完成数量 结案？
                    if (productionDemand.tb_productionplan.TotalQuantity != productionDemand.tb_productionplan.TotalCompletedQuantity)
                    {
                        productionDemand.tb_productionplan.DataStatus = (int)DataStatus.确认;

                        await _unitOfWorkManage.GetDbClient().Updateable(productionDemand.tb_productionplan).UpdateColumns(t => new { t.DataStatus, t.TotalCompletedQuantity }).ExecuteCommandAsync();
                    }

                }


                if (entity.IsOutSourced)
                {
                    AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        //生成加工费用的应付款单   ,加工费一般不会预付，所以不会抵扣，要处理也只是在审核完后。这里不会处理
                        try
                        {
                            #region 生成应付
                            var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                            var Payable = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>().Where(c => c.SourceBillId == entity.FG_ID && c.SourceBizType == (int)BizType.缴库单)
                                .FirstAsync();
                            if (Payable != null)
                            {
                                if (Payable.ARAPStatus >= (int)ARAPStatus.部分支付)
                                {
                                    throw new Exception("该缴库单已经有支付记录，不能反审核！");
                                }
                                else
                                {
                                    //Payable.ARAPStatus = (int)ARAPStatus.待审核;
                                    //Payable.Remark += $"引用的缴库单于{System.DateTime.Now.Date}被反审";
                                    //没有任何支付的，直接删除，因为审核时会重新生成。
                                    await ctrpayable.BaseDeleteAsync(Payable);
                                }
                                //ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(Payable, false);
                            }

                            #endregion
                        }
                        catch (Exception)
                        {
                            await _unitOfWorkManage.RollbackTranAsync();
                            throw new Exception("缴库时，生成加工费用的应付款单处理失败！");
                        }
                    }

                }

                #endregion

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();
                await _unitOfWorkManage.CommitTranAsync();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                await _unitOfWorkManage.RollbackTranAsync();
                _logger.Error(ex);
                rs.ErrorMsg = ex.Message;
                return rs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_FinishedGoodsInv> list = await _appContext.Db.CopyNew().Queryable<tb_FinishedGoodsInv>().Where(m => m.FG_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                          .Includes(a => a.tb_department)
                           .Includes(a => a.tb_FinishedGoodsInvDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_FinishedGoodsInvDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_FinishedGoodsInvDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }





    }
}



