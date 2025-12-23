
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
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_FinishedGoodsInv entity = ObjectEntity as tb_FinishedGoodsInv;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                 
                if (entity.tb_FinishedGoodsInvDetails == null)
                {
                    entity.tb_FinishedGoodsInvDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>()
                        .Where(c => c.FG_ID == entity.FG_ID)
                        .ToListAsync();
                }


                //更新制令单的QuantityDelivered已交付数量 ,如果全交完了。则结案
                //缴库复制。每次还是要先查询一下
                if (entity.MOID > 0)
                {
                    entity.tb_manufacturingorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                    .Includes(b => b.tb_proddetail, c => c.tb_prod)
                    .Includes(b => b.tb_bom_s, c => c.tb_BOM_SDetails)
                    .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                    .Includes(d => d.tb_ManufacturingOrderDetails, e => e.tb_bom_s, c => c.tb_BOM_SDetails, f => f.tb_BOM_SDetailSubstituteMaterials)
                    .Includes(b => b.tb_productiondemand, c => c.tb_productionplan, d => d.tb_ProductionPlanDetails)
                    //  .Includes(b => b.tb_productiondemand, c => c.tb_ManufacturingOrders, d => d.tb_ManufacturingOrderDetails)
                    .Includes(b => b.tb_MaterialRequisitions, c => c.tb_MaterialRequisitionDetails)
                   .Includes(a => a.tb_FinishedGoodsInvs, b => b.tb_FinishedGoodsInvDetails) //找到他名下的所有的缴库信息
                    .Where(c => c.MOID == entity.MOID)
                    .SingleAsync();
                }

                //如果制令单中的关键物料没有发全则不能缴库,或只是提醒，用全局变量控制？
                if (true)
                {

                }

                #region 由缴库更新库存



                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {

                    #region 库存表的更新 这里应该是必需有库存的数据，

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.InitInventory = (int)inv.Quantity;
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
                        CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, child.UnitCost);
                        var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                        // 递归更新所有上级BOM的成本
                        await ctrbom.UpdateParentBOMsAsync(inv.ProdDetailID, inv.Inv_Cost);

                        /*
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
                        if (bomDetails.Count > 0)
                        {
                            await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(bomDetails).ExecuteCommandAsync();
                        }

                        #endregion
                        */
                    }

                    inv.Quantity = inv.Quantity + child.Qty;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion

                    invUpdateList.Add(inv);

                }
                #endregion
                //List应该用ExecuteReturnSnowflakeIdListAsync 否则返回的是ID的值不是影响的行数。
                //var InvInsertCounter = await _unitOfWorkManage.GetDbClient().Insertable(invInsertList).ExecuteReturnSnowflakeIdAsync();


                // 使用LINQ查询
                var CheckNewInvList = invUpdateList
                    .GroupBy(i => new { i.ProdDetailID, i.Location_ID })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key.ProdDetailID)
                    .ToList();

                if (CheckNewInvList.Count > 0)
                {
                    //新增库存中有重复的商品，操作失败。请联系管理员。
                    rs.ErrorMsg = "新增库存中有重复的商品，操作失败。";
                    rs.Succeeded = false;
                    _logger.LogError(rs.ErrorMsg + "详细信息：" + string.Join(",", CheckNewInvList));
                    return rs;
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvMainCounter == 0)
                {
                    _logger.Debug($"{entity.DeliveryBillNo}缴库更新库存结果为0行，请检查数据！");
                }

                //如果缴库明细中的品不是来自制令单，则报错
                if (!entity.tb_FinishedGoodsInvDetails.Any(c => c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID && c.Location_ID == entity.tb_manufacturingorder.Location_ID))
                {
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    rs.ErrorMsg = $"缴库明细中有不属于当前制令单生产的产品及对应仓库!请检查数据后重试！";
                    return rs;
                }


                #region 由缴库单更新制令单
                if (entity.tb_manufacturingorder.tb_FinishedGoodsInvs == null)
                {
                    entity.tb_manufacturingorder.tb_FinishedGoodsInvs = new List<tb_FinishedGoodsInv>();
                }
                //先找到所有缴库明细,再找按制令单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                List<tb_FinishedGoodsInvDetail> detailList = new List<tb_FinishedGoodsInvDetail>();
                foreach (var item in entity.tb_manufacturingorder.tb_FinishedGoodsInvs.Where(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结).ToList())
                {
                    detailList.AddRange(item.tb_FinishedGoodsInvDetails);
                }

                //要加上当前缴款的明细。

                detailList.AddRange(entity.tb_FinishedGoodsInvDetails);

                //这里与采购订单不一样。采购订单是用明细去比较，这里是回写的是制令单，是主表。
                string prodName = entity.tb_manufacturingorder.tb_proddetail.tb_prod.CNName + entity.tb_manufacturingorder.tb_proddetail.tb_prod.Specifications;
                //找出所有这个制令单的对应 缴库的数量加总
                var PaidQuantity = detailList.Where(c => c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID && c.Location_ID == entity.tb_manufacturingorder.Location_ID).Sum(c => c.Qty);

                #region 缴库数量按制令单实发数根据BOM计算得到最多能入库的数量，如果超过则提示后退回
                decimal CanManufactureQtyBybom = 0;
                //按制令单中所有要发出的物料中数量为整数的最小值为算 ，小数可能是 胶水 纸箱这种耗材。暂时排除
                //最小可能产出量,关键物料且大于1的数量，根据BOM配方去计划
                var CanManufactureQtyBybomList = entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Where(c => c.ActualSentQty >= 1 && c.IsKeyMaterial.HasValue && c.IsKeyMaterial.Value == true).OrderByDescending(c => c.ActualSentQty).ToList();

                //注意如果制令单生成时手动或程序指定了替换料，这时也要把替换料对应 的BOM记录到制令单明细中。用于后面判断生成最小量成品
                if (CanManufactureQtyBybomList.Count > 0)
                {
                    tb_ManufacturingOrderDetail MinQtyDetail = CanManufactureQtyBybomList[0];
                    if (MinQtyDetail.ActualSentQty > 0)
                    {
                        //先找到对应的所属配方。再找到他在配方明细中的基数。
                        tb_BOM_S minbom = MinQtyDetail.tb_bom_s;
                        if (minbom != null)
                        {
                            tb_BOM_SDetail miniBomDetail = minbom.tb_BOM_SDetails.FirstOrDefault(c => c.ProdDetailID == MinQtyDetail.ProdDetailID);
                            if (miniBomDetail != null)
                            {
                                //实发数量/bom配方明细中的基准数量*BOM产出量=可以做的成品数量
                                CanManufactureQtyBybom = MinQtyDetail.ActualSentQty / miniBomDetail.UsedQty.ToDecimal() * minbom.OutputQty;
                            }
                            //TODO:注意 这里暂时不处理特殊情况。如果手动添加或替换料则暂时不算能生产的最小数量
                            //else
                            //{
                            //    //找不到就可能是替换料或自己手动添加的铺料。
                            //    throw new Exception("制令单明细没有指定所属配方，请修改数据后再试，或联系管理员。");
                            //}
                        }
                        //else
                        //{
                        //    //没有找到配方说明在生成制令单时。没有指定。注意如果制令单生成时手动或程序指定了替换料，这时也要把替换料对应 的BOM记录到制令单明细中。用于后面判断生成最小量成品
                        //    throw new Exception("制令单明细没有指定所属配方，请修改数据后再试，或联系管理员。");
                        //}

                    }
                    //如果总缴库数量大于最小制成数量则审核出错。
                    if (PaidQuantity > CanManufactureQtyBybom)
                    {
                        if (MessageBox.Show("系统检测到缴库数量大于发出的关键物料能生产的最小数量,你确定要审核通过吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                        {
                            string msg = $"制令单:{entity.tb_manufacturingorder.MONO}的【{prodName}】的缴库数不能大于制令单中发出物料能生产的最小数量。";
                            try
                            {
                                object obj = _cacheManager.GetEntity<View_ProdDetail>(MinQtyDetail.ProdDetailID);
                                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                                {
                                    //提示哪个关键物料实发数不够生产。
                                    msg += $"\r\n{prodDetail.SKU}:{prodDetail.CNName}实发数量不够生产{CanManufactureQtyBybom}";
                                }
                            }
                            catch (Exception tipEx)
                            {
                                _logger.Error(tipEx);
                            }

                            rs.ErrorMsg = msg;
                            _unitOfWorkManage.RollbackTran();
                            _logger.Debug(msg);
                            return rs;
                        }
                    }
                }

                //如果制令单中没有发任何数量的物料是不可能缴库的。
                if (entity.tb_manufacturingorder.tb_ManufacturingOrderDetails.Sum(c => c.ActualSentQty) == 0)
                {
                    string msg = $"制令单:{entity.tb_manufacturingorder.MONO}，没有任何物料发出。";
                    rs.ErrorMsg = msg;
                    _unitOfWorkManage.RollbackTran();
                    return rs;
                }

                #endregion
                if (PaidQuantity > entity.tb_manufacturingorder.ManufacturingQty)
                {
                    string msg = $"制令单:{entity.tb_manufacturingorder.MONO}的【{prodName}】的缴库数量不能大于制令单中要生产的数量。";
                    rs.ErrorMsg = msg;
                    _unitOfWorkManage.RollbackTran();
                    _logger.Debug(msg);
                    return rs;
                }
                else
                {
                    //当前行累计到交付,只是当前单的。不是以前的。
                    var RowQty = entity.tb_FinishedGoodsInvDetails.Where(c => c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID && c.Location_ID == entity.tb_manufacturingorder.Location_ID).Sum(c => c.Qty);
                    entity.tb_manufacturingorder.QuantityDelivered += RowQty;
                    //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                    if (entity.tb_manufacturingorder.QuantityDelivered > detailList.Sum(c => c.Qty))
                    {
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"缴库单：{entity.DeliveryBillNo}审核时，{entity.tb_manufacturingorder.SKU}缴库总数量不能大于制令单{entity.tb_manufacturingorder.MONO}中的生产数量！";
                        rs.Succeeded = false;
                        return rs;
                    }

                    //制令单已交数量和判断是否结案
                    if (entity.tb_manufacturingorder.QuantityDelivered == entity.tb_manufacturingorder.ManufacturingQty
                        && entity.tb_manufacturingorder.DataStatus == (int)DataStatus.确认 && entity.ApprovalStatus.Value == (int)ApprovalStatus.审核通过)
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
                if (entity.tb_manufacturingorder.PDID > 0)
                {
                    //2024-6-26修改为强引用了
                    //因为没有强引用 这里主动去查询
                    tb_ProductionDemand productionDemand = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                       .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                       .Includes(a => a.tb_productionplan, b => b.tb_ProductionPlanDetails)
                       .Where(c => c.PDID == entity.tb_manufacturingorder.PDID)
                       .SingleAsync();

                    //一个缴款单上面一个制令单。一个制令单 找到 需求单，再找到计划单。但是：需求下有多个制令单都来自于一个计划单，
                    //所以这里要循环加总保存到计划单中。
                    #region 更新计划单的完成数量
                    //标记一下，如果计划单明细有变化，则更新计划单明细
                    //
                    bool PlanDetailHasChanged = false;

                    foreach (var child in entity.tb_FinishedGoodsInvDetails)
                    {
                        tb_ProductionPlanDetail planDetail = productionDemand.tb_productionplan.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID);
                        if (planDetail != null)
                        {
                            //按理计划这样保存也是总数量。
                            planDetail.CompletedQuantity = planDetail.CompletedQuantity + child.Qty;
                            //意思是制令单时可以增加数量，大于计划数量。 缴库数量根据制令单来核对即可。但是不能大于1.5倍。不然就重新建计划
                            //这里是：比方 销售订单中 1000台  另10台是备品。isgift但是计划时要合在一起生产。这时计划目标是分开的。需求是合并算的。
                            if (planDetail.CompletedQuantity > (planDetail.Quantity * 1.5))
                            {
                                _unitOfWorkManage.RollbackTran();
                                rs.Succeeded = false;
                                rs.ErrorMsg = $"缴库明细中有完成数量大于计划数量的产品1.5倍，系统认为异常数量!请检查后重试！";
                                return rs;
                            }
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
                        //意思是制令单时可以修改计划数量。缴库根据制令单数量来，但是如果计划完成的数量 大于计划本身的1.5倍。认为不正常。暂时默认这样规则。
                        if (productionDemand.tb_productionplan.TotalCompletedQuantity > (productionDemand.tb_productionplan.TotalQuantity * 1.5))
                        {
                            _unitOfWorkManage.RollbackTran();
                            rs.Succeeded = false;
                            rs.ErrorMsg = $"缴库数量大于计划数量超过1.5倍!请检查数据后重试！";
                            return rs;
                        }
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
                            _unitOfWorkManage.RollbackTran();
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

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_FinishedGoodsInvDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 反审时 期初已经有数据
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    BusinessHelper.Instance.EditEntity(inv);
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestOutboundTime = System.DateTime.Now;
                    inv.MakingQty = inv.MakingQty + child.Qty;
                    CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Qty, child.UnitCost);
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



                #region

                //处理 制令单？ 要单独处理，查出来，因为没有用强引用
                //更新制令单的QuantityDelivered已交付数量 ,如果全交完了。则结案--的反操作

                if (entity.MOID > 0)
                {
                    entity.tb_manufacturingorder = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                    .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                    .Includes(b => b.tb_proddetail, c => c.tb_prod)
                    .Includes(a => a.tb_FinishedGoodsInvs, b => b.tb_FinishedGoodsInvDetails)
                    .Includes(b => b.tb_MaterialRequisitions, c => c.tb_MaterialRequisitionDetails)
                    .Where(c => c.MOID == entity.MOID)
                    .Single();
                }

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
                        _unitOfWorkManage.RollbackTran();
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
                if (entity.tb_manufacturingorder.PDID > 0)
                {


                    tb_ProductionDemand productionDemand = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                       .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                       .Includes(a => a.tb_productionplan, b => b.tb_ProductionPlanDetails)
                       .Where(c => c.PDID == entity.tb_manufacturingorder.PDID)
                       .SingleAsync();
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
                                    Payable.ARAPStatus = (int)ARAPStatus.待审核;
                                    Payable.Remark += $"引用的缴库单于{System.DateTime.Now.Date}被反审";
                                }
                                ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(Payable, false);
                            }

                            #endregion
                        }
                        catch (Exception)
                        {
                            _unitOfWorkManage.RollbackTran();
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



