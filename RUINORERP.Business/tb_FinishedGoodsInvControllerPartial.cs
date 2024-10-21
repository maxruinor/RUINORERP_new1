
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
using RUINORERP.Extensions.Middlewares;
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

namespace RUINORERP.Business
{
    public partial class tb_FinishedGoodsInvController<T>
    {

        /// <summary>
        /// 返回批量审核的结果 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_FinishedGoodsInv entity = ObjectEntity as tb_FinishedGoodsInv;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();


                //更新制令单的QuantityDelivered已交付数量 ,如果全交完了。则结案
                tb_ManufacturingOrder manufacturingOrder = null;
                if (entity.MOID > 0)
                {
                    manufacturingOrder = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                    .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                    .Includes(b => b.tb_proddetail, c => c.tb_prod)
                    .Includes(b => b.tb_productiondemand, c => c.tb_productionplan, d => d.tb_ProductionPlanDetails)
                    .Includes(b => b.tb_MaterialRequisitions)
                   .Includes(a => a.tb_FinishedGoodsInvs, b => b.tb_FinishedGoodsInvDetails) //找到他名下的所有的缴库信息
                    .Where(c => c.MOID == entity.MOID)
                    .Single();
                }

                //如果入库明细中的产品。不存在于订单中。审核失败。
                if (!entity.tb_FinishedGoodsInvDetails.Any(c => c.ProdDetailID == manufacturingOrder.ProdDetailID && c.Location_ID == manufacturingOrder.Location_ID))
                {
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    rs.ErrorMsg = $"缴库明细中有不属于当前制令单生产的产品及对应仓库!请检查数据后重试！";
                    return rs;
                }

                //更新计划单已交数量，制令单会引用需求分析，需求分析引用计划单
                if (manufacturingOrder.PDID > 0)
                {

                    //2024-6-26修改为强引用了。是不是可以优化？
                    //因为没有强引用 这里主动去查询
                    tb_ProductionDemand productionDemand = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                       .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                       .Includes(a => a.tb_productionplan, b => b.tb_ProductionPlanDetails)
                       .Where(c => c.PDID == manufacturingOrder.PDID)
                       .SingleAsync();


                    //标记一下，如果计划单明细有变化，则更新计划单明细
                    bool PlanDetailHasChanged = false;

                    foreach (var child in entity.tb_FinishedGoodsInvDetails)
                    {
                        tb_ProductionPlanDetail planDetail = productionDemand.tb_productionplan.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID);
                        if (planDetail != null)
                        {
                            planDetail.CompletedQuantity += child.Qty;
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
                        PlanHasChanged = true;
                    }

                    //如果计划数量等于已完成数量 结案？
                    if (productionDemand.tb_productionplan.TotalQuantity == productionDemand.tb_productionplan.TotalCompletedQuantity)
                    {
                        productionDemand.tb_productionplan.DataStatus = (int)DataStatus.完结;
                        PlanHasChanged = true;
                    }
                    if (PlanHasChanged)
                    {
                        await _unitOfWorkManage.GetDbClient().Updateable(productionDemand.tb_productionplan).UpdateColumns(t => new { t.DataStatus, t.TotalCompletedQuantity }).ExecuteCommandAsync();
                    }

                }


                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    bool Opening = false;
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        Opening = true;
                        inv = new tb_Inventory();
                        inv.Quantity = inv.Quantity + child.Qty;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    else
                    {
                        inv.Quantity = inv.Quantity + child.Qty;
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

                    CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, child.UnitCost);
                    #region 更新BOM价格,当前产品存在哪些BOM中，则更新所有BOM的价格包含主子表数据的变化

                    tb_BOM_SDetailController<tb_BOM_SDetail> ctrtb_BOM_SDetail = _appContext.GetRequiredService<tb_BOM_SDetailController<tb_BOM_SDetail>>();
                    List<tb_BOM_SDetail> bomDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>()
                    .Includes(b => b.tb_bom_s, d => d.tb_BOM_SDetails)
                    .Where(c => c.ProdDetailID == child.ProdDetailID).ToList();
                    foreach (tb_BOM_SDetail bomDetail in bomDetails)
                    {
                        //如果存在则更新 
                        bomDetail.UnitCost = inv.Inv_Cost;
                        bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                        if (bomDetail.tb_bom_s != null)
                        {
                            bomDetail.tb_bom_s.TotalMaterialCost = bomDetail.tb_bom_s.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                            bomDetail.tb_bom_s.OutProductionAllCosts = bomDetail.tb_bom_s.TotalMaterialCost + bomDetail.tb_bom_s.TotalOutManuCost + bomDetail.tb_bom_s.OutApportionedCost;
                            bomDetail.tb_bom_s.SelfProductionAllCosts = bomDetail.tb_bom_s.TotalMaterialCost + bomDetail.tb_bom_s.TotalSelfManuCost + bomDetail.tb_bom_s.SelfApportionedCost;
                            await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_S>(bomDetail.tb_bom_s).ExecuteCommandAsync();
                        }
                    }
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(bomDetails).ExecuteCommandAsync();

                    #endregion
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion

                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(child.ProdDetailID + "==>" + child.property + "缴库时库存更新成功");
                        }

                        if (Opening)
                        {
                            #region 处理期初
                            //库存都没有。期初也会没有 ,并且期初只会新增，不会修改。
                            tb_OpeningInventory oinv = new tb_OpeningInventory();
                            oinv.Inventory_ID = rr.ReturnObject.Inventory_ID;
                            oinv.Cost_price = rr.ReturnObject.Inv_Cost;
                            oinv.Subtotal_Cost_Price = oinv.Cost_price * oinv.InitQty;
                            oinv.InitInvDate = entity.DeliveryDate;
                            oinv.RefBillID = entity.FG_ID;
                            oinv.RefNO = entity.DeliveryBillNo;
                            string BizTypeName = Enum.GetName(typeof(BizType), BizType.缴库单);
                            oinv.RefBizType = BizTypeName;
                            oinv.InitQty = 0;
                            oinv.InitInvDate = System.DateTime.Now;
                            oinv.Notes = "由缴库时自动生成";
                            CommBillData cbd = bcf.GetBillData<tb_FinishedGoodsInv>(entity);
                            //oinv.RefBizType = cbd.BizType;
                            //TODO 还要完善引用数据
                            await ctrOPinv.AddReEntityAsync(oinv);
                            #endregion
                        }
                    }
                }

                //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                List<tb_FinishedGoodsInvDetail> detailList = new List<tb_FinishedGoodsInvDetail>();
                foreach (var item in manufacturingOrder.tb_FinishedGoodsInvs)
                {
                    detailList.AddRange(item.tb_FinishedGoodsInvDetails);
                }
                //这里与采购订单不一样。采购订单是用明细去比较，这里是回写的是制令单，是主表。
                string prodName = manufacturingOrder.tb_proddetail.tb_prod.CNName + manufacturingOrder.tb_proddetail.tb_prod.Specifications;
                //找出所有这个制令单的对应 缴库的数量加总
                var inQty = detailList.Where(c => c.ProdDetailID == manufacturingOrder.ProdDetailID && c.Location_ID == manufacturingOrder.Location_ID).Sum(c => c.Qty);
                if (inQty > manufacturingOrder.ManufacturingQty)
                {
                    string msg = $"制令单:{manufacturingOrder.MONO}的【{prodName}】的缴库数量不能大于制令单中要生产的数量，审核失败！";
                    MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _unitOfWorkManage.RollbackTran();
                    _logger.LogInformation(msg);
                    return rs;
                }
                else
                {
                    //当前行累计到交付,只是当前单的。不是以前的。
                    var RowQty = entity.tb_FinishedGoodsInvDetails.Where(c => c.ProdDetailID == manufacturingOrder.ProdDetailID && c.Location_ID == manufacturingOrder.Location_ID).Sum(c => c.Qty);
                    manufacturingOrder.QuantityDelivered += RowQty;
                    //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                    if (manufacturingOrder.QuantityDelivered > detailList.Sum(c => c.Qty))
                    {
                        throw new Exception($"缴库单：{entity.DeliveryBillNo}审核时，缴库总数量不能大于制令单{manufacturingOrder.MONO}中的生产数量！");
                    }

                    //制令单已交数量和判断是否结案
                    if (manufacturingOrder.QuantityDelivered == manufacturingOrder.ManufacturingQty)
                    {
                        manufacturingOrder.DataStatus = (int)DataStatus.完结;
                        manufacturingOrder.CloseCaseOpinions = $"缴库单:{entity.DeliveryBillNo}->制令单:{manufacturingOrder.MONO},缴库单审核时，生产数量等于交付数量，自动结案";

                        //修改领料单状态 系统认为制令单已完成时。领料单也会结案
                        manufacturingOrder.tb_MaterialRequisitions.ToList().ForEach(c => c.DataStatus = (int)DataStatus.完结);

                        int pomrCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisition>(manufacturingOrder.tb_MaterialRequisitions).ExecuteCommandAsync();
                        if (pomrCounter > 0)
                        {
                            if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                            {
                                _logger.Debug(entity.DeliveryBillNo + "==>" + entity.MONo + $"对应 的所有领料单设置为结案。将不能再发料 更新成功===重点代码 看已交数量是否正确");
                            }
                        }
                    }
                }


                //更新制令单已交数量和判断是否结案
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrder>(manufacturingOrder).ExecuteCommandAsync();
                if (poCounter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Debug(entity.DeliveryBillNo + "==>" + entity.MONo + $"对应 的制令单已交数量 更新成功===重点代码 看已交数量是否正确");
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                int counter = await _unitOfWorkManage.GetDbClient().Updateable<tb_FinishedGoodsInv>(entity).ExecuteCommandAsync();
                if (counter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Info(entity.DeliveryBillNo + "==>" + "缴库单的状态更新成功");
                    }
                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;

            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();

                rs.Succeeded = false;
                rs.ErrorMsg = ex.Message;
                return rs;
            }

        }



        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>


        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_FinishedGoodsInv entity = ObjectEntity as tb_FinishedGoodsInv;
            ReturnResults<T> rs = new ReturnResults<T>();

            try
            {
                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    return rs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
          

                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 反审时 期初已经有数据
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    inv.Quantity = inv.Quantity - child.Qty;
                    BusinessHelper.Instance.EditEntity(inv);
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestOutboundTime = System.DateTime.Now;
                    inv.MakingQty = inv.MakingQty + child.Qty;
                    CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Qty, child.UnitCost);
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        //反审不可能有期初库存
                    }
                }

                #region

                //处理 制令单？ 要单独处理，查出来，因为没有用强引用
                //更新制令单的QuantityDelivered已交付数量 ,如果全交完了。则结案--的反操作
                tb_ManufacturingOrder manufacturingOrder = null;
                if (entity.MOID > 0)
                {
                    manufacturingOrder = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                    .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                    .Includes(b => b.tb_proddetail, c => c.tb_prod)
                    .Includes(a => a.tb_FinishedGoodsInvs, b => b.tb_FinishedGoodsInvDetails)
                    .Where(c => c.MOID == entity.MOID)
                    .Single();
                }

                if (manufacturingOrder != null)
                {
                    #region  反审  退回  出库
                    //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。--反
                    List<tb_FinishedGoodsInvDetail> detailList = new List<tb_FinishedGoodsInvDetail>();
                    foreach (var item in manufacturingOrder.tb_FinishedGoodsInvs)
                    {
                        detailList.AddRange(item.tb_FinishedGoodsInvDetails);
                    }

                    //这里与采购订单不一样。采购订单是用明细去比较，这里是回写的是制令单，是主表。
                    string prodName = manufacturingOrder.tb_proddetail.tb_prod.CNName + manufacturingOrder.tb_proddetail.tb_prod.Specifications;
                    //一对一时

                    //当前缴库行累计到交付
                    var RowQty = entity.tb_FinishedGoodsInvDetails.Where(c => c.ProdDetailID == manufacturingOrder.ProdDetailID && c.Location_ID == manufacturingOrder.Location_ID).Sum(c => c.Qty);
                    manufacturingOrder.QuantityDelivered -= RowQty;
                    //如果已交数据大于制令单数量 给出警告实际操作中 使用其他方式将备品入库
                    if (manufacturingOrder.QuantityDelivered < 0)
                    {
                        throw new Exception($"缴库单：{entity.DeliveryBillNo}反审核时，对应的制令单：{manufacturingOrder.MONO}，{prodName}的生产数量不能为负数！");
                    }


                    #endregion
                    if (manufacturingOrder.QuantityDelivered != manufacturingOrder.ManufacturingQty)
                    {
                        manufacturingOrder.DataStatus = (int)DataStatus.确认;
                        manufacturingOrder.CloseCaseOpinions = $"缴库单:{entity.DeliveryBillNo}->制令单:{manufacturingOrder.MONO},缴库单反审时，生产数量不等于交付数量，取消自动结案";
                    }

                    //更新制令单的已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrder>(manufacturingOrder).ExecuteCommandAsync();
                    if (updatecounter == 0)
                    {

                    }
                }

                //更新计划单已交数量，制令单会引用需求分析，需求分析引用计划单
                if (manufacturingOrder.PDID > 0)
                {


                    tb_ProductionDemand productionDemand = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                       .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                       .Includes(a => a.tb_productionplan, b => b.tb_ProductionPlanDetails)
                       .Where(c => c.PDID == manufacturingOrder.PDID)
                       .SingleAsync();
                    foreach (var child in entity.tb_FinishedGoodsInvDetails)
                    {
                        tb_ProductionPlanDetail planDetail = productionDemand.tb_productionplan.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID);
                        if (planDetail != null)
                        {
                            planDetail.CompletedQuantity -= child.Qty;
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

                #endregion

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FinishedGoodsInv>(entity).ExecuteCommandAsync();
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
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



