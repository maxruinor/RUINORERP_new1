
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
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rrs = new ReturnResults<T>();
            tb_SaleOutRe entity = ObjectEntity as tb_SaleOutRe;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                if (entity == null)
                {
                    _unitOfWorkManage.RollbackTran();
                    rrs.Succeeded = false;
                    return rrs;
                }

                if (!entity.RefundOnly)
                {

                    //如果退回单是引用了销售订单来的。则所退产品要在订单出库明细中体现出来。回写。
                    if (entity.SaleOut_MainID.HasValue && entity.SaleOut_MainID.Value > 0)
                    {
                        if (entity.tb_saleout == null || entity.tb_saleout.tb_SaleOutDetails == null)
                        {
                            //更新销售订单和出库单。这两个可以通过销售出库单的导航查询得到
                            entity.tb_saleout = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(w => w.SaleOut_MainID == entity.SaleOut_MainID)
                                            .Includes(t => t.tb_SaleOutDetails, c => c.tb_proddetail)
                                            .Includes(t => t.tb_saleorder, b => b.tb_SaleOrderDetails)
                                    .FirstAsync();
                        }

                        #region   将更新销售订单ReturnedQty已退数量，销售出库单OrderReturnTotalQty订单退回数
                        //要注意的是  如果销售订单中有 多行相同SKU的的情况（实际是不同配置时） 出库退库要把订单的明细主键带上。
                        if (entity != null)
                        {
                            entity.HasChanged = false;
                        }
                        if (entity.tb_saleout != null)
                        {
                            // 如果退回单是引用了销售订单来的 则所退产品要在订单出库明细中。
                            foreach (var child in entity.tb_SaleOutReDetails)
                            {
                                bool exist = entity.tb_saleout.tb_SaleOutDetails.Where(c => c.ProdDetailID == child.ProdDetailID).Any();
                                if (!exist)
                                {
                                    View_ProdDetail view_Prod = await _unitOfWorkManage.GetDbClient().Queryable<View_ProdDetail>().Where(w => w.ProdDetailID == child.ProdDetailID).FirstAsync();
                                    string prodName = "【" + view_Prod.SKU + "】" + view_Prod.CNName;
                                    _unitOfWorkManage.RollbackTran();
                                    rrs.ErrorMsg = $"{prodName} ，不存在于对应销售出库的明细数据中!";
                                    rrs.Succeeded = false;
                                    return rrs;
                                }
                            }


                            foreach (var child in entity.tb_saleout.tb_SaleOutDetails)
                            {
                                tb_SaleOutReDetail returnDetail = entity.tb_SaleOutReDetails.Where(c => c.ProdDetailID == child.ProdDetailID).FirstOrDefault();
                                if (returnDetail == null)
                                {
                                    continue;
                                }
                                //出库的总退回数量=这次退回的。加之前的。意思是一个出库单，可以退多次。但是不能超过出库数量
                                child.TotalReturnedQty += returnDetail.Quantity;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (child.TotalReturnedQty > child.Quantity)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    rrs.ErrorMsg = $"销售退回单中：{entity.ReturnNo}中，明细退回总数量不能大于出库数量！请检查该出库单是否已经退回过！";
                                    rrs.Succeeded = false;
                                    return rrs;
                                }
                            }
                            await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutDetail>(entity.tb_saleout.tb_SaleOutDetails).ExecuteCommandAsync();

                            #region

                            //2024-4-15思路更新:如果销售订单中有相同的产品的多行情况。时 如订单: A 5PCS  A2PCS  ,出库也可以多行，A 2,A3, A2 按订单循环
                            List<tb_SaleOrderDetail> maskedList = new List<tb_SaleOrderDetail>();
                            for (int i = 0; i < entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                            {
                                tb_SaleOrderDetail orderDetail = entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails[i];
                                //判断是否订单中也录入了多行。这个很重要
                                //List<tb_SaleOrderDetail> orderDetailLines = new List<tb_SaleOrderDetail>();
                                //orderDetailLines = entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails.Where(c => c.ProdDetailID == orderDetail.ProdDetailID
                                //  && c.SaleOrderDetail_ID == orderDetail.SaleOrderDetail_ID
                                //).ToList();
                                List<tb_SaleOutDetail> outDetails = entity.tb_saleout.tb_SaleOutDetails
                                    .Where(c => c.ProdDetailID == orderDetail.ProdDetailID
                                    && c.Location_ID == orderDetail.Location_ID
                                    && c.SaleOrderDetail_ID == orderDetail.SaleOrderDetail_ID
                                ).ToList();
                                //没有出库过时，下一行
                                if (outDetails == null || outDetails.Count == 0)
                                {
                                    continue;
                                }
                                //针对 明细中存在相同产品录多行情况的处理
                                //总出库明细中的退回总数
                                int totalReturnedQty = outDetails.Sum(x => x.TotalReturnedQty);
                                orderDetail.TotalReturnedQty = totalReturnedQty;
                                //{
                                //    //多行时，则只要总数量多于等于订单的数量 ，则先将最合适的数量放回
                                //    //并且出库数量不能大于订单总数量 
                                //    //by 2025-3-20 多行时可以根据出库明细中的订单明细ID来找
                                //    if (totaloutqty >= orderDetail.Quantity)
                                //    {
                                //        orderDetail.TotalReturnedQty += orderDetail.Quantity;
                                //    }
                                //    else
                                //    {
                                //        orderDetail.TotalReturnedQty += totaloutqty;
                                //    }
                                //}

                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (orderDetail.TotalReturnedQty > orderDetail.Quantity)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    rrs.ErrorMsg = $"销售出库退回时，出库单：{entity.tb_saleout.SaleOutNo}中，SKU的退回总数不能大于订单中对应数量！";
                                    rrs.Succeeded = false;
                                    return rrs;
                                }

                                //更新已退数量
                                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity.tb_saleout.tb_saleorder.tb_SaleOrderDetails).ExecuteCommandAsync();

                                //!!!!!!!!2024-7-11  思路 重新定义为：退货不改变结案状态，因为在计算业绩时已经减掉了退回情况。
                                //销售出库单，如果来自于销售订单，则要把退回数量累加到订单中的退回总数量 并且如果数量够 也认为自动结案  全出库全退库
                                //if (saleout.tb_saleorder != null && saleout.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalReturnedQty) == saleout.tb_saleorder.TotalQty)
                                //{
                                //    saleout.tb_saleorder.DataStatus = (int)DataStatus.完结;
                                //    await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                                //}

                                #endregion
                            }


                            int ReturnTotalQty = entity.tb_SaleOutReDetails.Sum(c => c.Quantity);
                            //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                            //相当于出库的。全退了。
                            if (entity.tb_saleout.TotalQty == ReturnTotalQty)
                            {
                                entity.tb_saleout.DataStatus = (int)DataStatus.完结;
                            }
                            await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleout).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();

                            #endregion

                        }
                        foreach (var child in entity.tb_SaleOutReDetails)
                        {
                            #region 库存表的更新 这里应该是必需有库存的数据，
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                            if (inv != null)
                            {
                                //更新库存
                                inv.Quantity = inv.Quantity + child.Quantity;
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
                            //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                            inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                            inv.LatestStorageTime = System.DateTime.Now;
                            #endregion
                            ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                            if (rr.Succeeded)
                            {

                            }
                            else
                            {
                                return rrs;
                            }

                        }

                        if (entity.tb_SaleOutReRefurbishedMaterialsDetails != null)
                        {
                            //如果有翻新明细则要出库 减少库存
                            foreach (var child in entity.tb_SaleOutReRefurbishedMaterialsDetails)
                            {
                                #region 库存表的更新 这里应该是必需有库存的数据，
                                bool Opening = false;
                                tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                                if (inv == null)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    rrs.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法进行销售退货。请使用【期初盘点】【采购入库】】【生产缴库】的方式进行盘点后，再操作。";
                                    rrs.Succeeded = false;
                                    return rrs;
                                    Opening = true;
                                    inv = new tb_Inventory();
                                    inv.InitInventory = (int)inv.Quantity;
                                    inv.Quantity = inv.Quantity - child.Quantity;
                                    inv.Notes = "来自于销售退货翻新明细审核";//后面修改数据库是不需要？
                                    BusinessHelper.Instance.InitEntity(inv);
                                }
                                else
                                {
                                    inv.Quantity = inv.Quantity - child.Quantity;
                                    BusinessHelper.Instance.EditEntity(inv);
                                }

                                //采购订单时添加 。这里减掉在路上的数量
                                inv.ProdDetailID = child.ProdDetailID;
                                inv.Location_ID = child.Location_ID;
                                // 直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                                //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                                //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                                //后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                                //数据来源可以是多种多样的，例如：
                                //采购价格：从供应商处购买产品或物品时的价格。
                                //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                                //市场价格：参考市场上类似产品或物品的价格。

                                // CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);

                                //inv.Inv_Cost = child.TransactionPrice;//这里需要计算，根据系统设置中的算法计算。
                                //inv.CostFIFO = child.TransactionPrice;
                                //inv.CostMonthlyWA = child.TransactionPrice;
                                //inv.CostMovingWA = child.TransactionPrice;
                                inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                                inv.LatestStorageTime = System.DateTime.Now;



                                #endregion
                                ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                                if (rr.Succeeded)
                                {
                                    if (Opening)
                                    {
                                        tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                                        #region 处理期初
                                        //库存都没有。期初也会没有 ,并且期初只会新增，不会修改。
                                        tb_OpeningInventory oinv = new tb_OpeningInventory();
                                        oinv.Inventory_ID = rr.ReturnObject.Inventory_ID;
                                        oinv.Cost_price = rr.ReturnObject.Inv_Cost;
                                        oinv.Subtotal_Cost_Price = oinv.Cost_price * oinv.InitQty;
                                        oinv.InitInvDate = entity.ReturnDate;
                                        oinv.RefBillID = entity.SaleOutRe_ID;
                                        oinv.RefNO = entity.ReturnNo; ;
                                        oinv.InitQty = 0;
                                        oinv.InitInvDate = System.DateTime.Now;
                                        //CommBillData cbd = bcf.GetBillData<tb_PurEntry>(entitys[ii]);
                                        //oinv.RefBizType = cbd.BizType;
                                        //TODO 还要完善引用数据
                                        await ctrOPinv.AddReEntityAsync(oinv);
                                        #endregion
                                    }
                                }
                                else
                                {
                                    return rrs;
                                }

                            }
                        }
                    }
                }
                else
                {
                    //正常在仅退款时 录入销售退货单后 审核时就会生成一个
                    /*
                     仅退款的财务处理
    生成记账凭证：
    记账凭证名称：通常称为“退款记账凭证”或“仅退款记账凭证”。
    会计分录：
    借：应收账款（红字）
    贷：主营业务收入（红字）
    贷：应交税费—销项税（红字）
    生成报损清单：
    报损清单：用于记录资产损失或费用支出，确保资产损失的合法性。
    主要内容：包括退货申请单、能证明资产损失确属已实际发生的合法证据，如发票、收据等。
    仅退款的特殊处理
    核对账户余额：
    完成记账后，核对账户余额，确保所有账目准确无误。
    附上原始凭证：
    每笔退款都需要附上相关的原始凭证，如发票、收据等，以证明交易的真实性。
    生成报损清单：
    商家应妥善留存电商平台的相关规则（网页截图等），把单笔“快速退货退款”业务、“仅退款不退货”业务的判定结论（通知）、退款单据、订货单、发货凭证、快递（物流）运输单据等资料（含电子资料），作为进行相关账务处理的凭证。
    仅退款的会计分录示例
    客户退回已支付的货款：
    借：银行存款（红字）
    贷：主营业务收入（红字）
    贷：应交税费—销项税（红字）
    商品退回结转营业成本：
    借：营业成本（红字）
    贷：库存商品（红字）
    总结
    在仅退款的情况下，财务处理需要生成“退款记账凭证”和“报损清单”，确保财务记录的准确性和合法性。通过附上原始凭证和报损清单，可以为后续的财务审核和税务检查提供依据。
                     */
                }

                //更新累计退回数量
                // await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutReDetail>(entity.tb_SaleOutReDetails).ExecuteCommandAsync();

                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //entity.ApprovalResults = ae.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.DataStatus = (int)DataStatus.确认;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutRe>(entity).ExecuteCommandAsync();
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
                _logger.Error(ex, "事务回滚");
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                if (entity == null)
                {
                    return rrs;

                }
                else
                {
                    foreach (var child in entity.tb_SaleOutReDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv != null)
                        {
                            //更新库存
                            inv.Quantity = inv.Quantity - child.Quantity;
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
                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestStorageTime = System.DateTime.Now;
                        #endregion
                        ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                        if (rr.Succeeded)
                        {

                        }
                        else
                        {
                            return rrs;
                        }

                    }

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
                                inv.Quantity = inv.Quantity + child.Quantity;
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
                            //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                            inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                            inv.LatestStorageTime = System.DateTime.Now;
                            #endregion
                            ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                            if (rr.Succeeded)
                            {

                            }
                            else
                            {
                                return rrs;
                            }

                        }
                    }

                }
                //更库累计退回数量
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutReDetail>(entity.tb_SaleOutReDetails).ExecuteCommandAsync();

                #region   //将更新销售订单ReturnedQty已退数量，销售出库单OrderReturnTotalQty订单退回数

                //更新销售订单和出库单。这两个可以通过销售出库单的导航查询得到

                tb_SaleOut saleout = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(w => w.SaleOut_MainID == entity.SaleOut_MainID)
                                .Includes(t => t.tb_SaleOutDetails)
                                .Includes(t => t.tb_saleorder, b => b.tb_SaleOrderDetails)
                        .FirstAsync();
                if (entity != null)
                {
                    entity.HasChanged = false;
                }
                if (saleout != null)
                {

                    foreach (var child in saleout.tb_SaleOutDetails)
                    {
                        tb_SaleOutReDetail returnDetail = entity.tb_SaleOutReDetails.Where(c => c.ProdDetailID == child.ProdDetailID).FirstOrDefault();
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
                            rrs.ErrorMsg = $"销售退回单中：{entity.ReturnNo}中，SKU明细的退回总数量不能大于出库数量！";
                            rrs.Succeeded = false;
                            return rrs;
                        }
                    }
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutDetail>(saleout.tb_SaleOutDetails).ExecuteCommandAsync();

                    #region

                    //2024-4-15思路更新:如果销售订单中有相同的产品的多行情况。时 如订单: A 5PCS  A2PCS  ,出库也可以多行，A 2,A3, A2 按订单循环
                    List<tb_SaleOrderDetail> maskedList = new List<tb_SaleOrderDetail>();
                    for (int i = 0; i < saleout.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                    {
                        tb_SaleOrderDetail orderDetail = saleout.tb_saleorder.tb_SaleOrderDetails[i];
                        //判断是否订单中也录入了多行。这个很重要
                        List<tb_SaleOrderDetail> orderDetailLines = new List<tb_SaleOrderDetail>();
                        orderDetailLines = saleout.tb_saleorder.tb_SaleOrderDetails.Where(c => c.ProdDetailID == orderDetail.ProdDetailID).ToList();
                        List<tb_SaleOutDetail> outDetails = saleout.tb_SaleOutDetails.Where(c => c.ProdDetailID == orderDetail.ProdDetailID && c.Location_ID == orderDetail.Location_ID).ToList();
                        if (outDetails == null || outDetails.Count == 0)
                        {
                            continue;
                        }
                        //针对 明细中存在相同产品录多行情况的处理
                        //思路是上面出库明细中的数量已经明确。相同产品及库位下的数量按行数减 TODO:
                        int totaloutqty = outDetails.Sum(x => x.TotalReturnedQty);
                        //按订单来算，如果相同产品这给过值，则要减掉，第一行应该是0
                        totaloutqty = totaloutqty + orderDetailLines.Sum(x => x.TotalReturnedQty);

                        if (orderDetailLines.Count == 1)//就一行时，简单处理
                        {
                            orderDetail.TotalReturnedQty -= totaloutqty;
                        }
                        else
                        {
                            //多行时，则只要总数量多于等于订单的数量 ，则先将最合适的数量放回
                            //并且出库数量不能大于订单总数量 
                            if (totaloutqty >= orderDetail.Quantity)
                            {
                                orderDetail.TotalReturnedQty -= orderDetail.Quantity;
                            }
                            else
                            {

                                orderDetail.TotalReturnedQty -= totaloutqty;
                            }
                        }

                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (orderDetail.TotalReturnedQty > orderDetail.Quantity)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rrs.ErrorMsg = $"销售出库退回时，销售单：{saleout.SaleOutNo}中，SKU的退回总数量不能大于订单数量！";
                            rrs.Succeeded = false;
                            return rrs;
                        }


                    }
                    //更新已退数量
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(saleout.tb_saleorder.tb_SaleOrderDetails).ExecuteCommandAsync();

                    //!!!!!!!!2024-7-11  思路 重新定义为：退货不改变结案状态，因为在计算业绩时已经减掉了退回情况。
                    //销售出库单，如果来自于销售订单，则要把退回数量累加到订单中的退回总数量 并且如果数量够 也认为自动结案  全出库全退库
                    //if (saleout.tb_saleorder != null && saleout.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalReturnedQty) != saleout.tb_saleorder.TotalQty)
                    //{
                    //    saleout.tb_saleorder.DataStatus = (int)DataStatus.确认;
                    //    await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                    //}

                    #endregion
                }
                int ReturnTotalQty = entity.tb_SaleOutReDetails.Sum(c => c.Quantity);
                //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                //相当于出库的。全退了。
                if (saleout.TotalQty != ReturnTotalQty)
                {
                    saleout.DataStatus = (int)DataStatus.确认;
                }
                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();

                #endregion
                entity.ApprovalOpinions = "反审";
                //后面已经修改为
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.DataStatus = (int)DataStatus.新建;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutRe>(entity).ExecuteCommandAsync();
                _unitOfWorkManage.CommitTran();
                rrs.Succeeded = true;
                rrs.ReturnObject = entity as T;
                return rrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
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



