
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
using System.Windows.Interop;
using SqlSugar;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.Processor;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 销售退货
    /// </summary>
    public partial class tb_SaleOutReController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 审核销售退货 库存加回
        /// 如果有翻新明细，则要将明细出库用来重新打包处理等物料
        /// 
        /// 部分退货退款	- 生成退货单（负向出库）
        //- 生成退款单 → 反向核销原收款	- 应收应付表：恢复 RemainAmount
        //- 收付款表：插入退款单（状态=已审核）
        //6. 全部退货退款	- 生成全额退款单 → 冲销所有核销记录
        //- 关闭原应收单	- 核销表：标记所有关联记录为已冲销
        //- 应收应付表：状态=已冲销
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rrs = new ReturnResults<T>();
            tb_SaleOutRe entity = ObjectEntity as tb_SaleOutRe;
            try
            {
                if (entity == null)
                {
                    rrs.Succeeded = false;
                    return rrs;
                }
                //支持无出库单退货
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //如果退回单是引用了销售出库来的。则所退产品要在订单出库明细中体现出来。回写。
                if (entity.SaleOut_MainID.HasValue && entity.SaleOut_MainID.Value > 0)
                {

                    //更新销售订单和出库单。这两个可以通过销售出库单的导航查询得到
                    entity.tb_saleout = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(w => w.SaleOut_MainID == entity.SaleOut_MainID)
                                    .Includes(t => t.tb_SaleOutDetails, c => c.tb_proddetail)
                                    .Includes(t => t.tb_saleorder, b => b.tb_SaleOrderDetails)
                                    .Includes(t => t.tb_saleorder, b => b.tb_SaleOuts, c => c.tb_SaleOutDetails)
                            .FirstAsync();
                    entity.tb_saleout.AcceptChanges();


                    //如果采购订单的供应商和这里入库的供应商不相同，要提示
                    if (entity.CustomerVendor_ID != entity.tb_saleout.CustomerVendor_ID)
                    {
                        rrs.Succeeded = false;
                        rrs.ErrorMsg = $"销售退回单的客户和销售出库的客户不同!请检查数据后重试！";
                        return rrs;
                    }


                    if (entity.tb_saleout.TotalAmount < entity.TotalAmount || entity.tb_saleout.ForeignTotalAmount < entity.ForeignTotalAmount)
                    {
                        //特殊情况 要选择客户信息，还要进一步完善！TODO
                        rrs.ErrorMsg = "退货金额不能大于出库金额。如果特殊情况，请单独使用【其他费用支出】完成退款。";
                        rrs.Succeeded = false;
                        return rrs;
                    }
                }
                //要注意的是  如果销售订单中有 多行相同SKU的的情况（实际是不同配置时） 出库退库要把订单的明细主键带上。
                if (entity != null)
                {
                    entity.HasChanged = false;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (entity.tb_saleout != null)
                {
                    #region   将更新销售订单ReturnedQty已退数量，销售出库单OrderReturnTotalQty订单退回数
                    // 如果退回单是引用了销售订单来的 则所退产品要在订单出库明细中。
                    // 回写出库单
                    foreach (var child in entity.tb_SaleOutReDetails)
                    {
                        bool exist = entity.tb_saleout.tb_SaleOutDetails.Where(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID).Any();
                        if (!exist)
                        {
                            _unitOfWorkManage.RollbackTran();
                            View_ProdDetail view_Prod = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(child.ProdDetailID);
                            if (view_Prod != null)
                            {
                                string prodName = "【" + view_Prod.SKU + "】" + view_Prod.CNName;
                                rrs.ErrorMsg = $"{prodName} ，不存在于对应销售出库的明细数据中!";
                            }
                            else
                            {
                                rrs.ErrorMsg = $"数量为:{child.Quantity}的产品，不存在于对应销售出库的明细数据中!";
                            }
                            rrs.Succeeded = false;
                            return rrs;
                        }
                    }

                    foreach (var child in entity.tb_saleout.tb_SaleOutDetails)
                    {
                        tb_SaleOutReDetail returnDetail = entity.tb_SaleOutReDetails
                            .Where(c => c.ProdDetailID == child.ProdDetailID
                            && c.Location_ID == child.Location_ID
                            && c.SaleOutDetail_ID == child.SaleOutDetail_ID
                            ).FirstOrDefault();
                        if (returnDetail == null) //这里主要 是因为 条件是后面加的前面退货明细中没有出库行号值
                        {
                            returnDetail = entity.tb_SaleOutReDetails
                            .Where(c => c.ProdDetailID == child.ProdDetailID
                            && c.Location_ID == child.Location_ID
                            ).FirstOrDefault();
                        }
                        //如果仅退款则数量不能加回？
                        if (returnDetail == null || entity.RefundOnly)
                        {
                            continue;
                        }

                        //出库的总退回数量=这次退回的。加之前的。意思是一个出库单，可以退多次。但是不能超过出库数量
                        child.TotalReturnedQty += returnDetail.Quantity;
                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (child.TotalReturnedQty > child.Quantity)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rrs.ErrorMsg = $"销售退回单：{entity.ReturnNo}中，明细退回总数量不能大于出库数量！请检查该出库单是否已经退回过！";
                            rrs.Succeeded = false;
                            return rrs;
                        }
                        if (child.TotalReturnedQty < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rrs.ErrorMsg = $"销售退回单：{entity.ReturnNo}中，明细退回总数量不能小于0！请检查数据后重试！";
                            rrs.Succeeded = false;
                            return rrs;
                        }
                    }



                    if (!entity.RefundOnly)
                    {
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutDetail>(entity.tb_saleout.tb_SaleOutDetails)
                               .UpdateColumns(t => new { t.TotalReturnedQty })
                               .ExecuteCommandAsync();

                        #region 回写销售订单

                        //2024-4-15思路更新:如果销售订单中有相同的产品的多行情况。时 如订单: A 5PCS  A2PCS  ,出库也可以多行，A 2,A3, A2 按订单循环
                        //回写订单退回数量

                        List<tb_SaleOutReDetail> outReDetailsList = new List<tb_SaleOutReDetail>();
                        outReDetailsList.AddRange(entity.tb_SaleOutReDetails);

                        for (int i = 0; i < entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                        {
                            tb_SaleOrderDetail orderDetail = entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails[i];
                            var totalReturnedQty = outReDetailsList.Where(c => c.ProdDetailID == orderDetail.ProdDetailID
                            && c.Location_ID == orderDetail.Location_ID).ToList().Sum(c => c.Quantity);
                            //没有退回
                            if (totalReturnedQty == 0)
                            {
                                continue;
                            }

                            orderDetail.TotalReturnedQty += totalReturnedQty;

                            //上面明细中 没办法处理 相同料号多次录入订单情况。这里 合并数量来判断。
                            int totalOrderTotalQty = entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails.Where(c => c.ProdDetailID == orderDetail.ProdDetailID && c.Location_ID == orderDetail.Location_ID).Sum(c => c.Quantity);

                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (orderDetail.TotalReturnedQty > totalOrderTotalQty)
                            {
                                _unitOfWorkManage.RollbackTran();
                                View_ProdDetail ProdDetail = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(orderDetail.ProdDetailID);

                                if (ProdDetail != null)
                                {
                                    rrs.ErrorMsg = $"销售出库退回时，出库单：{entity.tb_saleout.SaleOutNo}中，{ProdDetail.SKU} 退回总数{orderDetail.TotalReturnedQty}不能大于订单中对应数量{orderDetail.Quantity}！";
                                }
                                else
                                {
                                    rrs.ErrorMsg = $"销售出库退回时，出库单：{entity.tb_saleout.SaleOutNo}中， 退回总数{orderDetail.TotalReturnedQty}不能大于订单中对应数量{orderDetail.Quantity}！";
                                }
                                rrs.Succeeded = false;
                                return rrs;
                            }
                        }

                        //更新已退数量
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails)
                            .UpdateColumns(t => new { t.TotalReturnedQty }).ExecuteCommandAsync();

                        //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                        //相当于出库的。全退了。
                        if (entity.tb_saleout.TotalQty == entity.tb_saleout.tb_SaleOutDetails.Sum(c => c.TotalReturnedQty))
                        {
                            entity.tb_saleout.DataStatus = (int)DataStatus.完结;
                            await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleout).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                        }

                        #endregion
                    }
                    #endregion
                }

                if (!entity.RefundOnly)
                {
                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                    foreach (var child in entity.tb_SaleOutReDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        //更新库存
                        inv.Quantity = inv.Quantity + child.Quantity;
                        BusinessHelper.Instance.EditEntity(inv);

                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestStorageTime = System.DateTime.Now;
                        #endregion
                        invUpdateList.Add(inv);
                    }

                    DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (Counter == 0)
                    {
                        _logger.LogInformation($"{entity.ReturnNo}审核时，更新库存结果为0行，请检查数据！");
                    }

                    if (entity.tb_SaleOutReRefurbishedMaterialsDetails != null)
                    {
                        List<tb_Inventory> invMaterialsUpdateList = new List<tb_Inventory>();
                        //如果有翻新明细则要出库 减少库存
                        foreach (var child in entity.tb_SaleOutReRefurbishedMaterialsDetails)
                        {
                            #region 库存表的更新 这里应该是必需有库存的数据，
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                            if (inv == null)
                            {
                                _unitOfWorkManage.RollbackTran();
                                rrs.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法当入销售退货翻新物料。请使用【期初盘点】【采购入库】】【生产缴库】的方式进行盘点后，再操作。";
                                rrs.Succeeded = false;
                                return rrs;
                            }
                            else
                            {
                                inv.Quantity = inv.Quantity - child.Quantity;
                                BusinessHelper.Instance.EditEntity(inv);
                            }
                            // CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);
                            //inv.Inv_Cost = child.TransactionPrice;//这里需要计算，根据系统设置中的算法计算。
                            //inv.CostFIFO = child.TransactionPrice;
                            //inv.CostMonthlyWA = child.TransactionPrice;
                            //inv.CostMovingWA = child.TransactionPrice;
                            inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                            inv.LatestStorageTime = System.DateTime.Now;

                            #endregion
                            invMaterialsUpdateList.Add(inv);
                        }

                        if (invMaterialsUpdateList.Count > 0)
                        {
                            int invMaterialsCounter = await _unitOfWorkManage.GetDbClient().Updateable(invMaterialsUpdateList)
                                .UpdateColumns(t => new { t.Quantity, t.LatestStorageTime })
                                .ExecuteCommandAsync();
                            if (invMaterialsCounter == 0)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception("翻新物料的库存更新失败！");
                            }
                        }

                    }
                }

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    //处理财务数据 退货退货
                    #region 销售退款 财务处理 不管什么情况都是生成红字应收【金额为负】

                    //如果是有出库情况，则反冲。如果是没有出库情况。则生成付款单
                    //退货单审核后生成红字应收单（负金额）
                    var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                    tb_FM_ReceivablePayable Payable = await ctrpayable.BuildReceivablePayable(entity);


                    ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(Payable, false);
                    if (rmr.Succeeded)
                    {

                        rrs.ReturnObjectAsOtherEntity = rmr.ReturnObject;

                        //已经是等审核。 审核时会核销预收付款
                        //应收 负 在 就是退款 审核时还要仔细跟进一下
                        //如果是平台订单 则自动审核

                    }

                    #endregion
                }

                //这里要更新状态！！！！！！！！！！
                if (entity.RefundStatus.HasValue && ((RefundStatus)entity.RefundStatus.Value).ToString().Contains("已退款"))
                {
                    entity.RefundStatus = (int)RefundStatus.已退款已退货;
                    if (entity.RefundOnly)
                    {
                        entity.RefundStatus = (int)RefundStatus.已退款未退货;
                    }
                    if (entity.tb_saleout != null)
                    {
                        if (entity.tb_saleout.RefundStatus == (int)RefundStatus.已退款未退货 || entity.tb_saleout.RefundStatus == (int)RefundStatus.已退款等待退货)
                        {
                            entity.tb_saleout.RefundStatus = (int)RefundStatus.已退款已退货;
                        }
                    }
                }

                if (entity.RefundStatus.HasValue && ((RefundStatus)entity.RefundStatus.Value).ToString().Contains("未退款"))
                {
                    entity.RefundStatus = (int)RefundStatus.未退款已退货;

                    if (entity.tb_saleout != null)
                    {
                        if (entity.tb_saleout.RefundStatus == (int)RefundStatus.未退款等待退货)
                        {
                            entity.tb_saleout.RefundStatus = (int)RefundStatus.未退款已退货;
                        }
                    }
                }

                if (entity.tb_saleout != null && entity.tb_saleout.HasChanged)
                {
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity.tb_saleout)
                         .UpdateColumns(t => new { t.RefundStatus })
                         .ExecuteCommandAsync();
                }

                entity.ApprovalResults = true;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.DataStatus = (int)DataStatus.确认;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var last = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.ApprovalStatus,
                    it.DataStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                    it.RefundStatus,
                }).ExecuteCommandAsync();

                _unitOfWorkManage.CommitTran();
                rrs.ReturnObject = entity as T;
                rrs.Succeeded = true;
                return rrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                rrs.Succeeded = false;
                rrs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex, "事务回滚" + ex.Message);
                return rrs;
            }

        }





        /// <summary>
        ///反审
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {

            tb_SaleOutRe entity = ObjectEntity as tb_SaleOutRe;

            ReturnResults<T> rrs = new ReturnResults<T>();
            try
            {
                if (entity == null)
                {
                    rrs.Succeeded = false;
                    return rrs;
                }

                //支持无出库单退货

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //如果退回单是引用了销售出库来的。则所退产品要在订单出库明细中体现出来。回写。
                if (entity.SaleOut_MainID.HasValue && entity.SaleOut_MainID.Value > 0)
                {
                    //更新销售订单和出库单。这两个可以通过销售出库单的导航查询得到
                    entity.tb_saleout = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(w => w.SaleOut_MainID == entity.SaleOut_MainID)
                                    .Includes(t => t.tb_SaleOutDetails, c => c.tb_proddetail)
                                    .Includes(t => t.tb_saleorder, b => b.tb_SaleOrderDetails)
                                    .Includes(t => t.tb_saleorder, b => b.tb_SaleOuts, c => c.tb_SaleOutDetails)
                            .FirstAsync();
                    entity.tb_saleout.AcceptChanges();

                    if (entity.tb_saleout.TotalAmount < entity.TotalAmount || entity.tb_saleout.ForeignTotalAmount < entity.ForeignTotalAmount)
                    {
                        //特殊情况 要选择客户信息，还要进一步完善！TODO
                        rrs.ErrorMsg = "退货金额不能大于出库金额。如果特殊情况，请单独使用【其他费用支出】完成退款。";
                        rrs.Succeeded = false;
                        return rrs;
                    }
                }
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //处理业务数据
                if (!entity.RefundOnly)
                {
                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                    foreach (var child in entity.tb_SaleOutReDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);

                        //更新库存
                        inv.Quantity = inv.Quantity - child.Quantity;
                        BusinessHelper.Instance.EditEntity(inv);

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
                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestStorageTime = System.DateTime.Now;
                        #endregion
                        invUpdateList.Add(inv);
                    }
                    DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var InvCounter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (InvCounter == 0)
                    {
                        _logger.LogInformation($"销售退回单{entity.ReturnNo}审核时，更新库存结果为0行，请检查数据！");
                    }
                    invUpdateList.Clear();
                    if (entity.tb_SaleOutReRefurbishedMaterialsDetails != null)
                    {
                        //如果有翻新明细则要出库 减少库存
                        foreach (var child in entity.tb_SaleOutReRefurbishedMaterialsDetails)
                        {
                            #region 库存表的更新 这里应该是必需有库存的数据，
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                            if (inv != null)
                            {
                                //更新库存
                                inv.Quantity = inv.Quantity + child.Quantity; //翻新用的耗材这里反审就是还回去。用加
                                BusinessHelper.Instance.EditEntity(inv);
                            }

                            //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                            inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                            inv.LatestStorageTime = System.DateTime.Now;
                            #endregion
                            invUpdateList.Add(inv);
                        }
                        var MaterialsInvCounter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                        if (MaterialsInvCounter == 0)
                        {
                            _logger.LogInformation($"销售退回单{entity.ReturnNo}审核时，更新物料库存结果为0行，请检查数据！");
                        }
                    }


                    #region 回写销售订单  使用了销售出库的数据，要放在出库退回数量修改前面
                    if (entity.tb_saleout != null)
                    {
                        //2024-4-15思路更新:如果销售订单中有相同的产品的多行情况。时 如订单: A 5PCS  A2PCS  ,出库也可以多行，A 2,A3, A2 按订单循环
                        //回写订单退回数量 反审，就是恢复当前的数量 （变化的是上面的 退回数量）这里只是求和 保存回去？

                        List<tb_SaleOutReDetail> outReDetailsList = new List<tb_SaleOutReDetail>();
                        outReDetailsList.AddRange(entity.tb_SaleOutReDetails);

                        for (int i = 0; i < entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                        {
                            tb_SaleOrderDetail orderDetail = entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails[i];
                            var totalReturnedQty = outReDetailsList.Where(c => c.ProdDetailID == orderDetail.ProdDetailID
                            && c.Location_ID == orderDetail.Location_ID).ToList().Sum(c => c.Quantity);
                            //没有退回
                            if (totalReturnedQty == 0)
                            {
                                continue;
                            }

                            orderDetail.TotalReturnedQty -= totalReturnedQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (orderDetail.TotalReturnedQty < 0)
                            {
                                _unitOfWorkManage.RollbackTran();
                                rrs.ErrorMsg = $"销售退回单反审核时，对应销售订单：{entity.tb_saleout.tb_saleorder.SOrderNo}中，明细退回总数量不能小于0！请检查数据后重试！";
                                rrs.Succeeded = false;
                                return rrs;
                            }

                        }

                        //更新已退数量
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails)
                            .UpdateColumns(t => new { t.TotalReturnedQty }).ExecuteCommandAsync();

                    }

                    #endregion

                    #region   将更新销售订单ReturnedQty已退数量，销售出库单OrderReturnTotalQty订单退回数
                    if (entity.tb_saleout != null)
                    {
                        foreach (var child in entity.tb_saleout.tb_SaleOutDetails)
                        {
                            tb_SaleOutReDetail returnDetail = entity.tb_SaleOutReDetails
                                .Where(c => c.ProdDetailID == child.ProdDetailID
                                && c.Location_ID == child.Location_ID
                                && c.SaleOutDetail_ID == child.SaleOutDetail_ID
                                ).FirstOrDefault();
                            if (returnDetail == null) //这里主要 是因为 条件是后面加的前面退货明细中没有出库行号值
                            {
                                returnDetail = entity.tb_SaleOutReDetails
                                .Where(c => c.ProdDetailID == child.ProdDetailID
                                && c.Location_ID == child.Location_ID
                                ).FirstOrDefault();
                            }
                            if (returnDetail == null)
                            {
                                continue;
                            }
                            //出库的总退回数量=这次退回的。加之前的。意思是一个出库单，可以退多次。但是不能超过出库数量
                            child.TotalReturnedQty -= returnDetail.Quantity;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (child.TotalReturnedQty > child.Quantity)
                            {
                                _unitOfWorkManage.RollbackTran();
                                rrs.ErrorMsg = $"销售退回单：{entity.ReturnNo}中，SKU明细的退回总数量不能大于出库数量！";
                                rrs.Succeeded = false;
                                return rrs;
                            }
                            if (child.TotalReturnedQty < 0)
                            {
                                _unitOfWorkManage.RollbackTran();
                                rrs.ErrorMsg = $"销售退回单：{entity.ReturnNo}中，明细退回总数量不能小于0！请检查数据后重试！";
                                rrs.Succeeded = false;
                                return rrs;
                            }
                        }
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleout.tb_SaleOutDetails).UpdateColumns(t => new { t.TotalReturnedQty }).ExecuteCommandAsync();

                        //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                        //相当于出库的。全退了。
                        if (entity.tb_saleout.TotalQty != entity.tb_saleout.tb_SaleOutDetails.Sum(c => c.TotalReturnedQty))
                        {
                            entity.tb_saleout.DataStatus = (int)DataStatus.确认;
                        }
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleout).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();

                    }

                    #endregion
                }


                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    //处理财务数据 退货退货
                    #region 销售退款 财务处理 不管什么情况都是生成红字应收【金额为负】-------------反审
                    //退货单审核后生成红字应收单（负金额），如果应收单状态 可以反审。则直接反审过来。否则只能开启新的流程。如销售订单重新买。
                    var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                    //反向应收 如果没核销则直接删除。否则倒算？  一个ID只会一次应付吗？是的应收应付只会一次。收款可以分开多次。
                    //被冲销的 找回来
                    tb_FM_ReceivablePayable returnpayable = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                           .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID
                                            && c.SourceBizType == (int)BizType.销售退回单 && c.SourceBillId == entity.SaleOutRe_ID
                                            ).SingleAsync();
                    if (returnpayable != null)
                    {
                        if (returnpayable.ARAPStatus == (int)ARAPStatus.草稿
                            || returnpayable.ARAPStatus == (int)ARAPStatus.待审核 || returnpayable.ARAPStatus == (int)ARAPStatus.待支付)
                        {
                            //删除
                            bool deleters = await ctrpayable.BaseDeleteByNavAsync(returnpayable);
                        }
                        else
                        {
                            _unitOfWorkManage.RollbackTran();
                            rrs.ErrorMsg = $"销售退回单：{entity.ReturnNo}中,对应的应收红冲数据状态为【{(ARAPStatus)returnpayable.ARAPStatus}】。无法反审核！";
                            rrs.Succeeded = false;
                            return rrs;
                        }

                        /*
                        var PositivePayableList = await ctrpayable.BaseQueryByWhereAsync(c => c.CustomerVendor_ID == returnpayable.CustomerVendor_ID
                          && c.ARAPStatus == (int)ARAPStatus.已冲销
                          && (c.TotalLocalPayableAmount > 0 || c.TotalForeignPayableAmount > 0)
                          );

                        PositivePayableList = PositivePayableList.OrderBy(c => c.Created_at.Value).ToList();

                        //反向加回去
                        for (int i = 0; i < PositivePayableList.Count; i++)
                        {
                            var OriginalPayable = PositivePayableList[i];
                            #region 如果原始应收没有核销付款，则直接生成 红字应收核销

                            //判断 如果出库的应收金额和未核销余额一样。说明 客户还没有支付任何款，则可以直接全额红冲
                            OriginalPayable.LocalBalanceAmount += entity.TotalAmount;

                            returnpayable.LocalBalanceAmount -= entity.TotalAmount;

                            OriginalPayable.ForeignBalanceAmount += entity.ForeignTotalAmount;

                            returnpayable.ForeignBalanceAmount -= entity.ForeignTotalAmount;


                            OriginalPayable.ARAPStatus = (int)ARAPStatus.已冲销;
                            returnpayable.ARAPStatus = (int)ARAPStatus.已冲销;
                            //生成核销记录证明正负抵消应收应付
                            //生成一笔核销记录  应收红冲
                            tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement> settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();
                            await settlementController.GenerateSettlement(OriginalPayable, returnpayable);

                            await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(OriginalPayable).ExecuteCommandAsync();
                            await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(returnpayable).ExecuteCommandAsync();
                            #endregion
                        }
                        */
                    }

                    #endregion
                }



                //可能后面退部分这种，还需要进一步确认状态

                //反审
                if (entity.RefundStatus.HasValue && ((RefundStatus)entity.RefundStatus.Value).ToString().Contains("已退款"))
                {
                    entity.RefundStatus = (int)RefundStatus.已退款未退货;
                    if (entity.RefundOnly)
                    {
                        entity.RefundStatus = (int)RefundStatus.已退款未退货;
                    }
                    if (entity.tb_saleout != null)
                    {
                        if (entity.tb_saleout.RefundStatus == (int)RefundStatus.已退款已退货)
                        {
                            entity.tb_saleout.RefundStatus = (int)RefundStatus.已退款等待退货;
                        }
                    }
                }

                if (entity.RefundStatus.HasValue && ((RefundStatus)entity.RefundStatus.Value).ToString().Contains("未退款"))
                {
                    entity.RefundStatus = (int)RefundStatus.未退款等待退货;
                    if (entity.RefundOnly)
                    {
                        entity.RefundStatus = (int)RefundStatus.已退款未退货;
                    }
                    if (entity.tb_saleout != null)
                    {
                        if (entity.tb_saleout.RefundStatus == (int)RefundStatus.未退款等待退货)
                        {
                            entity.tb_saleout.RefundStatus = (int)RefundStatus.未退款等待退货;
                        }
                    }
                }

                if (entity.tb_saleout != null && entity.tb_saleout.HasChanged)
                {
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity.tb_saleout)
                         .UpdateColumns(t => new { t.RefundStatus })
                         .ExecuteCommandAsync();
                }



                entity.ApprovalOpinions = "反审";
                //后面已经修改为
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.DataStatus = (int)DataStatus.新建;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var last = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.RefundStatus,
                    it.ApprovalStatus,
                    it.DataStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                }).ExecuteCommandAsync();


                _unitOfWorkManage.CommitTran();
                rrs.Succeeded = true;
                rrs.ReturnObject = entity as T;
                return rrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, $"销售退回单：{entity.SaleOut_NO}");
                rrs.Succeeded = false;
                rrs.ErrorMsg = ex.Message;
                return rrs;
            }

        }



        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_SaleOutRe> list = await _appContext.Db.CopyNew().Queryable<tb_SaleOutRe>().Where(m => m.SaleOutRe_ID == ID)
                             .Includes(a => a.tb_customervendor)
                             .Includes(a => a.tb_employee)
                             .Includes(a => a.tb_saleout, b => b.tb_saleorder)
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                             .Includes(a => a.tb_SaleOutReDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                             .Includes(a => a.tb_SaleOutReRefurbishedMaterialsDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                             .ToListAsync();
            return list as List<T>;
        }


    }
}



