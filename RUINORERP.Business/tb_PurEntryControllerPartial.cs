
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
using RUINORERP.Business.CommService;
using AutoMapper;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Business
{
    public partial class tb_PurEntryController<T>
    {

        /// <summary>
        /// 返回批量审核的结果
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_PurEntry entity = ObjectEntity as tb_PurEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //采购入库总数量和明细求和检查
                if (entity.TotalQty.Equals(entity.tb_PurEntryDetails.Sum(c => c.Quantity)) == false)
                {
                    rs.ErrorMsg = $"采入入库数量与明细之和不相等!请检查数据后重试！";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }

                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();



                //处理采购订单
                entity.tb_purorder = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                     .Includes(a => a.tb_PurEntries, b => b.tb_PurEntryDetails)
                     .Includes(t => t.tb_PurOrderDetails)
                     .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                     .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                     .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                     .Single();

                if (entity.tb_purorder == null)
                {
                    rs.ErrorMsg = $"没有找到对应的采购订单!请检查数据后重试！";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }

                //如果入库明细中的产品。不存在于订单中。审核失败。
                foreach (var child in entity.tb_PurEntryDetails)
                {
                    if (!entity.tb_purorder.tb_PurOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                    {
                        rs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"入库明细中有产品不属于当前订单!请检查数据后重试！";
                        return rs;
                    }
                }

                //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                List<tb_PurEntryDetail> detailList = new List<tb_PurEntryDetail>();
                foreach (var item in entity.tb_purorder.tb_PurEntries)
                {
                    detailList.AddRange(item.tb_PurEntryDetails);
                }

                //分两种情况处理。
                for (int i = 0; i < entity.tb_purorder.tb_PurOrderDetails.Count; i++)
                {
                    //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                    string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                              entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                    //明细中有相同的产品或物品。
                    var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                    {
                        #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                        if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                        {
                            //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        #endregion

                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                        //行数一致时，判断入库的数量是否大于订单数量。（费赠品）
                        && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID).Where(c => c.IsGift.HasValue && !c.IsGift.Value).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                        {
                            string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量\r\n" + $"或存在针对当前采购订单重复录入了采购入库单，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            //算出交付的数量
                            entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"入库单：{entity.PurEntryNo}审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，入库总数量不能大于订单数量！");
                            }
                        }
                    }
                    else
                    {
                        //一对一时
                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID).Where(c => c.IsGift.HasValue && !c.IsGift.Value).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                        {

                            string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量\r\n" + $"或存在针对当前采购订单重复录入了采购入库单，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            //当前行累计到交付
                            var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID).Sum(c => c.Quantity);
                            entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"入库单：{entity.PurEntryNo}审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，入库总数量不能大于订单数量！");
                            }
                        }
                    }
                }

                //更新已交数量
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrderDetail>(entity.tb_purorder.tb_PurOrderDetails).ExecuteCommandAsync();
                if (poCounter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Debug(entity.PurEntryNo + "==>" + entity.PurOrder_NO + $"对应 的订单更新成功===重点代码 看已交数量是否正确");
                    }
                }

                foreach (tb_PurEntryDetail child in entity.tb_PurEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    bool Opening = false;
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        Opening = true;
                        inv = new tb_Inventory();
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "采购入库创建";//后面修改数据库是不需要？
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
                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;

                    //直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。适用范围：适用于存货的实物流转比较符合先进先出的假设，比如食品、药品等有保质期限制的商品，先购进的存货会先发出销售。

                    //数据来源可以是多种多样的，例如：
                    //采购价格：从供应商处购买产品或物品时的价格。
                    //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                    //市场价格：参考市场上类似产品或物品的价格。
                    if (child.IsGift.HasValue && child.IsGift == false && child.TransactionPrice > 0)
                    {
                        CommService.CostCalculations.CostCalculation(_appContext, inv, child.Quantity, child.TransactionPrice);
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
                    }

                    inv.Quantity = inv.Quantity + child.Quantity;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion

                    if (child.IsGift.HasValue && child.IsGift == false)
                    {
                        #region 更新采购价格
                        //注意这里的人是指采购订单录入的人。不是采购入库的人。
                        tb_PriceRecordController<tb_PriceRecord> ctrPriceRecord = _appContext.GetRequiredService<tb_PriceRecordController<tb_PriceRecord>>();
                        tb_PriceRecord priceRecord = _unitOfWorkManage.GetDbClient().Queryable<tb_PriceRecord>()
                        .Where(c => c.Employee_ID == entity.tb_purorder.Employee_ID && c.ProdDetailID == child.ProdDetailID).First();
                        //如果存在则更新，否则插入
                        if (priceRecord == null)
                        {
                            priceRecord = new tb_PriceRecord();
                        }
                        priceRecord.Employee_ID = entity.tb_purorder.Employee_ID;
                        if (child.TransactionPrice != priceRecord.PurPrice)
                        {
                            priceRecord.PurPrice = child.TransactionPrice;
                            priceRecord.PurDate = System.DateTime.Now;
                            priceRecord.ProdDetailID = child.ProdDetailID;
                            ReturnResults<tb_PriceRecord> rrpr = await ctrPriceRecord.SaveOrUpdate(priceRecord);
                        }


                        #endregion
                    }


                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(child.ProdDetailID + "==>" + child.property + "库存更新成功");
                        }

                        if (Opening)
                        {
                            #region 处理期初  
                            //库存都没有。期初也会没有 ,并且期初只会新增，并且数据只是期初盘点时才有，不会修改。
                            tb_OpeningInventory oinv = new tb_OpeningInventory();
                            oinv.Inventory_ID = rr.ReturnObject.Inventory_ID;
                            oinv.Cost_price = rr.ReturnObject.Inv_Cost;
                            oinv.Subtotal_Cost_Price = oinv.Cost_price * oinv.InitQty;
                            oinv.InitInvDate = entity.EntryDate;
                            oinv.RefBillID = entity.PurEntryID;
                            oinv.RefNO = entity.PurEntryNo;
                            oinv.InitQty = 0;
                            oinv.InitInvDate = System.DateTime.Now;
                            CommBillData cbd = bcf.GetBillData<tb_PurEntry>(entity);
                            //oinv.RefBizType = cbd.BizType;
                            //TODO 还要完善引用数据
                            await ctrOPinv.AddReEntityAsync(oinv);
                            #endregion
                        }
                    }
                }


                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {

                    #region 生成应付 ,应付从预付中抵扣 同时 核销

                    var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                    //出库时，全部生成应收，账期的。就加上到期日
                    //有付款过的。就去预收中抵扣，不够的金额及状态标识出来生成对账单
                    ReturnMainSubResults<tb_FM_ReceivablePayable> results = await ctrpayable.CreateReceivablePayable(entity, false);
                    if (results.Succeeded)
                    {
                        tb_FM_ReceivablePayable payable = results.ReturnObject;
                        if (entity.PayStatus != (int)PayStatus.未付款)
                        {
                            #region 去预付中抵扣相同币种的情况下的预付款，生成付款单，并且生成核销记录
                            //按客户查找所有的未核销完的预付款记录。并且是审核过的。
                            List<tb_FM_PreReceivedPayment> prePayments = await _unitOfWorkManage.GetDbClient()
                                .Queryable<tb_FM_PreReceivedPayment>()
                                .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID
                                 && c.Currency_ID == entity.Currency_ID // 添加币种条件
                                 && c.IsAvailable == true
                                && (c.PrePaymentStatus == (long)PrePaymentStatus.已生效
                                 || c.PrePaymentStatus == (long)PrePaymentStatus.部分核销))
                                .OrderBy(c => c.PrePayDate)
                                .ToListAsync();

                            decimal ForeignTotalAmount = entity.ForeignTotalAmount;
                            decimal TotalAmount = entity.TotalAmount;
                            List<tb_FM_PaymentSettlement> writeoffs = new List<tb_FM_PaymentSettlement>(); // 用于存储核销记录
                            for (int i = 0; i < prePayments.Count; i++)
                            {
                                decimal prePayForeignAmount = 0;
                                decimal prePayLocalAmount = 0;

                                if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
                                {
                                    //出库金额ForeignTotalAmount和 预付金额prePayments[i].ForeignBalanceAmount 比较
                                    prePayForeignAmount = Math.Min(prePayments[i].ForeignBalanceAmount, ForeignTotalAmount);

                                    //预付款余额
                                    prePayments[i].ForeignBalanceAmount -= prePayForeignAmount;

                                    //预付款已核销金额
                                    prePayments[i].ForeignPaidAmount += prePayForeignAmount;

                                    ForeignTotalAmount -= prePayForeignAmount;

                                    if (prePayments[i].ForeignBalanceAmount == 0)
                                    {
                                        prePayments[i].PrePaymentStatus = (long)PrePaymentStatus.全额核销;
                                    }
                                    else
                                    {
                                        prePayments[i].PrePaymentStatus = (long)PrePaymentStatus.部分核销;
                                    }
                                    // 更新应付表

                                    //已经核销，从客户的预付款中扣到的金额
                                    payable.ForeignPaidAmount += prePayForeignAmount;

                                    //应付款的余额，表示未核销，还要从客户收取的金额
                                    payable.ForeignBalanceAmount = entity.ForeignTotalAmount - payable.ForeignPaidAmount;
                                    if (payable.ForeignBalanceAmount == 0)
                                    {
                                        payable.ARAPStatus = (long)ARAPStatus.已结清;
                                    }
                                    else
                                    {
                                        payable.ARAPStatus = (long)ARAPStatus.部分支付;
                                    }
                                }
                                else
                                {
                                    prePayLocalAmount = Math.Min(prePayments[i].LocalBalanceAmount, TotalAmount);
                                    prePayments[i].LocalBalanceAmount -= prePayLocalAmount;
                                    prePayments[i].LocalPaidAmount += prePayLocalAmount;
                                    TotalAmount -= prePayLocalAmount;
                                    if (prePayments[i].LocalBalanceAmount == 0)
                                    {
                                        prePayments[i].PrePaymentStatus = (long)ARAPStatus.已结清;
                                    }
                                    else
                                    {
                                        prePayments[i].PrePaymentStatus = (long)ARAPStatus.部分支付;
                                    }
                                    // 更新应付表
                                    payable.LocalPaidAmount += prePayLocalAmount;
                                    payable.LocalBalanceAmount = entity.TotalAmount - payable.LocalPaidAmount;
                                    if (payable.LocalBalanceAmount == 0)
                                    {
                                        payable.ARAPStatus = (long)ARAPStatus.已结清;
                                    }
                                    else
                                    {
                                        payable.ARAPStatus = (long)ARAPStatus.部分支付;
                                    }
                                }
                                // 生成核销记录证明从预付中付款抵扣应付
                                tb_FM_PaymentSettlement writeoff = new tb_FM_PaymentSettlement();
                                writeoff.SettlementType = (int)SettlementType.预付冲应付;
                                writeoff.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款核销);
                                writeoff.SettleDate = DateTime.Now;
                                writeoff.ReceivePaymentType = (int)ReceivePaymentType.付款;
                                writeoff.Account_id = prePayments[i].Account_id;

                                //若源单与目标单币种不同，需按汇率转换后核销，并在记录中明确标注：
                                //这里实现的是币种相同的情况，即应付和预付是相同币种，自动核销，否则要手动核销
                                //SettledForeignAmount = 1000,          --按来源单据币种（USD）
                                //TargetExchangeRate = 0.85,            --目标单据汇率（USD→EUR）
                                //SettledLocalAmount = 1000 * 0.85-- 转换后的本币金额（EUR）


                                writeoff.SourceBizType = (int)BizType.预付款单;
                                writeoff.SourceBillId = prePayments[i].PreRPID;
                                writeoff.SourceBillNo = prePayments[i].PreRPNO;
                                writeoff.Currency_ID = payable.Currency_ID;
                            
                                    writeoff.Currency_ID = prePayments[i].Currency_ID;

                              
                                    writeoff.ExchangeRate = prePayments[i].ExchangeRate;
                               

                                writeoff.TargetBillId = payable.ARAPId; // 应付单ID
                                writeoff.TargetBillNo = payable.ARAPNo; // 应付单号
                                writeoff.TargetBizType = (int)BizType.应付单;
                                writeoff.CustomerVendor_ID = prePayments[i].CustomerVendor_ID;
                               
                              
                                    writeoff.ExchangeRate = payable.ExchangeRate;
                               
                                writeoff.IsReversed = false;
                                writeoff.SettledForeignAmount = prePayForeignAmount;
                                writeoff.SettledLocalAmount = prePayLocalAmount;
                                writeoff.IsAutoSettlement = true;
                                BusinessHelper.Instance.InitEntity(writeoff);
                                writeoffs.Add(writeoff);

                                if (ForeignTotalAmount == 0 || TotalAmount == 0)
                                {
                                    break;
                                }

                            }
                            // 插入核销记录
                            if (writeoffs.Count > 0)
                            {
                                await _unitOfWorkManage.GetDbClient().Insertable(writeoffs).ExecuteReturnSnowflakeIdListAsync();
                            }

                            //统计更新预付款单
                            if (prePayments.Count > 0)
                            {
                                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(prePayments)
                                    .UpdateColumns(it => new
                                    {
                                        it.PrePaymentStatus,
                                        it.ForeignBalanceAmount,
                                        it.ForeignPaidAmount,
                                        it.LocalBalanceAmount,
                                        it.LocalPaidAmount,

                                    }).ExecuteCommandAsync();
                            }

                            await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(payable).ExecuteCommandAsync();

                            #endregion
                        }
                    }

                    #endregion
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //  entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //  entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                int counter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(entity).ExecuteCommandAsync();
                if (counter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        // _logger.Info(entity.PurEntryNo + "==>" + "状态更新成功");
                    }
                }

                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量 TODO 销售也会有这种情况
                if (entity.tb_purorder != null && entity.tb_purorder.DataStatus == (int)DataStatus.确认 &&
                    (entity.TotalQty == entity.tb_purorder.TotalQty || entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity)
                    == entity.tb_purorder.TotalQty))
                {
                    entity.tb_purorder.DataStatus = (int)DataStatus.完结;
                    entity.tb_purorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核入库单:" + entity.PurEntryNo + "结案。"; ;
                    int poendcounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder)
                        .UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                    if (poendcounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(entity.tb_purorder.PurOrderNo + "==>" + "结案状态更新成功");
                        }
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
                _logger.Error(ex, "事务回滚" + ex.Message);
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
            tb_PurEntry entity = ObjectEntity as tb_PurEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
                //判断是否能反审? 意思是。我这个入库单错了。但是你都当入库成功进行了后面的操作了，现在要反审，那肯定不行。所以，要判断，
                if (entity.tb_PurEntryRes != null
                    && (entity.tb_PurEntryRes.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurEntryRes.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rs.ErrorMsg = "存在已确认或已完结，或已审核的采购入库退回单，不能反审核  ";
                    return rs;
                }


                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                foreach (var child in entity.tb_PurEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 期初已经有数据了，则要
                    bool Opening = false;
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        Opening = true;
                        inv = new tb_Inventory();

                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
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

                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;
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
                    if (child.IsGift.HasValue && child.IsGift == false && child.TransactionPrice > 0)
                    {
                        CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Quantity, child.TransactionPrice);
                        //赠品不更新。价格为0的不更新。
                        #region 更新BOM价格,当前产品存在哪些BOM中，则更新所有BOM的价格包含主子表数据的变化

                        tb_BOM_SDetailController<tb_BOM_SDetail> ctrtb_BOM_SDetail = _appContext.GetRequiredService<tb_BOM_SDetailController<tb_BOM_SDetail>>();
                        List<tb_BOM_SDetail> bomDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>()
                        .Includes(b => b.tb_bom_s, c => c.tb_BOM_SDetails)
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
                    }
                    inv.Quantity = inv.Quantity - child.Quantity;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;

                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        if (Opening)
                        {
                            #region 处理期初
                            //库存都没有。期初也会没有 ,并且期初只会新增，不会修改。
                            tb_OpeningInventory oinv = new tb_OpeningInventory();
                            oinv.Inventory_ID = rr.ReturnObject.Inventory_ID;
                            oinv.Cost_price = rr.ReturnObject.Inv_Cost;
                            oinv.Subtotal_Cost_Price = oinv.Cost_price * oinv.InitQty;
                            oinv.InitInvDate = entity.EntryDate;
                            oinv.RefBillID = entity.PurEntryID;
                            oinv.RefNO = entity.PurEntryNo;
                            oinv.InitQty = 0;
                            oinv.InitInvDate = System.DateTime.Now;
                            CommBillData cbd = bcf.GetBillData<tb_PurEntry>(entity);
                            //oinv.RefBizType = cbd.BizType;
                            //TODO 还要完善引用数据
                            await ctrOPinv.AddReEntityAsync(oinv);
                            #endregion
                        }
                    }
                }

                if (entity.tb_purorder != null)
                {
                    #region  反审检测写回 退回

                    //处理采购订单
                    entity.tb_purorder = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                         .Includes(a => a.tb_PurEntries, b => b.tb_PurEntryDetails)
                         .Includes(t => t.tb_PurOrderDetails)
                         .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                         .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                         .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                         .Single();


                    //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                    List<tb_PurEntryDetail> detailList = new List<tb_PurEntryDetail>();
                    foreach (var item in entity.tb_purorder.tb_PurEntries)
                    {
                        detailList.AddRange(item.tb_PurEntryDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_purorder.tb_PurOrderDetails.Count; i++)
                    {
                        //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                        string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                        //明细中有相同的产品或物品。
                        var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                        {
                            #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            #endregion

                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                                && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                        else
                        {
                            //一对一时
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {

                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                //当前行累计到交付
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID).Sum(c => c.Quantity);
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                    }


                    #endregion

                    //更新已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrderDetail>(entity.tb_purorder.tb_PurOrderDetails).ExecuteCommandAsync();
                    if (updatecounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}明细中的已交数更新出错！");
                    }

                }
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(entity).ExecuteCommandAsync();

                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量
                if (entity.tb_purorder != null && entity.tb_purorder.TotalQty != entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity))
                {
                    entity.tb_purorder.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                }


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
            List<tb_PurEntry> list = await _appContext.Db.CopyNew().Queryable<tb_PurEntry>().Where(m => m.PurEntryID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                          .Includes(a => a.tb_department)
                           .Includes(a => a.tb_PurEntryDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_PurEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }





    }
}



