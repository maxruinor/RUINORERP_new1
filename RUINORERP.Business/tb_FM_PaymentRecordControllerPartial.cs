
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2024 00:33:16
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
using RUINORERP.Global;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using AutoMapper;
using RUINORERP.Business.StatusManagerService;
using MapsterMapper;
using IMapper = AutoMapper.IMapper;
using System.Text;
using System.Windows.Forms;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PaymentRecordController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 审核了就不能反审了。只能冲销
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            rmrs.ErrorMsg = "付款记录，不能反审。只能红冲";
            await Task.Delay(0);
            return rmrs;
        }

        /// <summary>
        /// 付款单审核通过时，更新对应的业务单据的收款状态。更新余额
        /// 如:一个订单 开始预付一次，后面再预付一次，全款都付完成要更新为全额预付
        /// 通常收款付款单审核时。已经处理完了 预收付。除非特殊情况。不可用时。
        /// 自动核销关联的应收/应付单
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PaymentRecord entity = ObjectEntity as tb_FM_PaymentRecord;
            var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();
            try
            {

                if (entity == null)
                {
                    rmrs.ErrorMsg = "付款单不能为空!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }

                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {
                    // 非平台来源且没有收款信息时，返回错误
                    if (!entity.IsFromPlatform.GetValueOrDefault() && !entity.PayeeInfoID.HasValue)
                    {
                        rmrs.ErrorMsg = "付款时，对方的收款信息必填!";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }


                //不能直接将实体entity重新查询赋值，否则反应到UI时不是相同对象了。
                if (entity.tb_FM_PaymentRecordDetails == null)
                {
                    entity.tb_FM_PaymentRecordDetails = await _appContext.Db.Queryable<tb_FM_PaymentRecordDetail>()
                                .Where(c => c.PaymentId == entity.PaymentId).ToListAsync();
                }


                //如果一个单位，正好正向500，负数-500 ，相抵消是正好为0，则可以为零。审核后要将应收应付核销掉。
                //只有明细中有负数才可能等于0
                if (entity.TotalLocalAmount == 0 && entity.TotalForeignAmount == 0 && entity.tb_FM_PaymentRecordDetails.Sum(c => c.LocalAmount) != 0)
                {
                    rmrs.ErrorMsg = "非正负红冲时，付款金额不能为0!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }
                if (entity.TotalLocalAmount == 0 && !entity.tb_FM_PaymentRecordDetails.Any(c => c.LocalAmount < 0))
                {
                    rmrs.ErrorMsg = "非正负红冲时，付款金额不能为0!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }
                //审核时 要检测明细中对应的相同业务类型下不能有相同来源单号。除非有正负总金额为0对冲情况。或是两行数据?
                var PendingApprovalDetails = await _appContext.Db.Queryable<tb_FM_PaymentRecordDetail>()
                    .Includes(c => c.tb_fm_paymentrecord)
                .Where(c => c.tb_fm_paymentrecord.PaymentStatus >= (int)PaymentStatus.已支付).ToListAsync();

                //要把自己也算上。不能大于1 ，entity是等待审核。所以拼一起
                PendingApprovalDetails.AddRange(entity.tb_FM_PaymentRecordDetails);
                if (!ValidatePaymentDetails(PendingApprovalDetails, rmrs))
                {
                    //rmrs.ErrorMsg = "相同业务类型下不能有相同的来源单号!审核失败。";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }

                #region

                //相同客户，多个应收可以合成一个收款 。所以明细中就是对应的应收单。
                //为了提高性能 将按业务类型分组后再找到对应的单据去处理
                //目前 所有业务都进应收应付 简化逻辑 
                entity.tb_FM_PaymentRecordDetails.GroupBy(c => c.SourceBizType).Select(c => new { SourceBizType = c.Key }).ToList();

                var details = entity.tb_FM_PaymentRecordDetails;
                Dictionary<int, List<long>> GroupResult = details
                    .GroupBy(d => d.SourceBizType)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(d => d.PaymentDetailId).ToList()
                    );
                //应收付
                List<tb_FM_ReceivablePayable> receivablePayableUpdateList = new List<tb_FM_ReceivablePayable>();

                //预收付
                List<tb_FM_PreReceivedPayment> preReceivedPaymentUpdateList = new List<tb_FM_PreReceivedPayment>();
                List<tb_SaleOut> saleOutUpdateList = new List<tb_SaleOut>();
                List<tb_SaleOrder> saleOrderUpdateList = new List<tb_SaleOrder>();

                List<tb_PurEntry> purEntryUpdateList = new List<tb_PurEntry>();
                List<tb_PurOrder> purOrderUpdateList = new List<tb_PurOrder>();

                List<tb_PurEntryRe> purEntryReUpdateList = new List<tb_PurEntryRe>();
                List<tb_SaleOutRe> SaleOutReUpdateList = new List<tb_SaleOutRe>();

                List<tb_FM_PriceAdjustment> priceAdjustmentUpdateList = new List<tb_FM_PriceAdjustment>();
                List<tb_FM_PaymentRecord> oldPaymentUpdateList = new List<tb_FM_PaymentRecord>();
                List<tb_FM_OtherExpense> otherExpenseUpdateList = new List<tb_FM_OtherExpense>();
                List<tb_FM_ExpenseClaim> expenseClaimUpdateList = new List<tb_FM_ExpenseClaim>();


                List<tb_AS_RepairOrder> RepairOrderUpdateList = new List<tb_AS_RepairOrder>();


                //相同客户，多个应收可以合成一个收款 。所以明细中就是对应的应收单。
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                foreach (var group in GroupResult)
                {
                    long[] sourcebillids = entity.tb_FM_PaymentRecordDetails.Where(c => group.Value.Contains(c.PaymentDetailId)).Select(c => c.SourceBilllId).ToArray();
                    //收付款明细记录中，如果金额为负数则是 退款红冲
                    //收款单审核时。除了保存核销记录，还要来源的 如 应收中的 余额这种更新
                    if (group.Key == (int)BizType.应收款单 || group.Key == (int)BizType.应付款单)
                    {
                        #region 应收单余额更新
                        List<tb_FM_ReceivablePayable> receivablePayableList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                             .Includes(c => c.tb_FM_ReceivablePayableDetails)
                             .Where(c => sourcebillids.Contains(c.ARAPId))
                             .ToListAsync();
                        foreach (var receivablePayable in receivablePayableList)
                        {
                            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
                            //应收应付明细中可以相同的。负数对冲。最终收款时只会合并。
                            tb_FM_PaymentRecordDetail RecordDetail = entity.tb_FM_PaymentRecordDetails.FirstOrDefault(c => c.SourceBilllId == receivablePayable.ARAPId);
                            receivablePayable.ForeignPaidAmount += RecordDetail.ForeignAmount;
                            receivablePayable.LocalPaidAmount += RecordDetail.LocalAmount;
                            receivablePayable.ForeignBalanceAmount -= RecordDetail.ForeignAmount;
                            receivablePayable.LocalBalanceAmount -= RecordDetail.LocalAmount;
                            if (receivablePayable.ForeignBalanceAmount == 0 || receivablePayable.LocalBalanceAmount == 0)
                            {
                                receivablePayable.ARAPStatus = (int)ARAPStatus.全部支付;
                            }
                            //付过，没付结清
                            if ((receivablePayable.ForeignBalanceAmount > 0 && receivablePayable.ForeignPaidAmount > 0)
                                || (receivablePayable.LocalBalanceAmount > 0 && receivablePayable.LocalPaidAmount > 0))
                            {
                                receivablePayable.ARAPStatus = (int)ARAPStatus.部分支付;
                            }
                            //写回业务 原始单据的完结状态，销售出库。销售订单。
                            //通过的来源类型，来源单号，来源编号分组得到原始单据数据组后再根据类型分别处理更新状态

                            if (receivablePayable.SourceBizType == (int)BizType.销售出库单)
                            {
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    #region 更新对应业务的单据状态和付款情况

                                    tb_SaleOut saleOut = await _appContext.Db.Queryable<tb_SaleOut>()
                                        .Includes(c => c.tb_saleorder, b => b.tb_SaleOuts)
                                      .Where(c => c.DataStatus >= (int)DataStatus.确认 && c.SaleOut_MainID == receivablePayable.SourceBillId).SingleAsync();
                                    if (saleOut != null)
                                    {
                                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == saleOut.TotalAmount)
                                        {
                                            //财务只管财务的状态
                                            // saleOut.DataStatus = (int)DataStatus.完结;
                                            saleOut.PayStatus = (int)PayStatus.全部付款;
                                            saleOut.Paytype_ID = entity.Paytype_ID.Value;
                                        }
                                        else
                                        {
                                            saleOut.PayStatus = (int)PayStatus.部分付款;
                                            saleOut.Paytype_ID = entity.Paytype_ID.Value;
                                        }
                                        if (saleOut.tb_saleorder.tb_SaleOuts != null)
                                        {
                                            //如果这个出库单的上级订单，的其它出库单的他出库的状态都是全部付款了。则这个订单就全部付款了。（排除自己）
                                            //订单要保证全部出库了。才能这样算否则就先不管订单状态。只是部分付款
                                            List<tb_SaleOut> otherSaleOuts = saleOut.tb_saleorder.tb_SaleOuts
                                                .Where(c => c.SaleOut_MainID != saleOut.SaleOut_MainID && c.PayStatus == (int)PayStatus.全部付款).ToList();

                                            if (receivablePayable.LocalPaidAmount == saleOut.TotalAmount
                                                && otherSaleOuts.Sum(c => c.TotalAmount) + saleOut.TotalAmount == saleOut.tb_saleorder.TotalAmount)
                                            {
                                                saleOut.tb_saleorder.PayStatus = (int)PayStatus.全部付款;
                                                saleOut.tb_saleorder.Paytype_ID = entity.Paytype_ID.Value;
                                            }
                                            else
                                            {
                                                saleOut.tb_saleorder.PayStatus = (int)PayStatus.部分付款;
                                                saleOut.tb_saleorder.Paytype_ID = entity.Paytype_ID.Value;
                                            }

                                        }
                                        saleOrderUpdateList.Add(saleOut.tb_saleorder);
                                        saleOutUpdateList.Add(saleOut);
                                    }

                                    #endregion
                                }
                            }

                            if (receivablePayable.SourceBizType == (int)BizType.销售退回单)
                            {
                                //退货单审核后生成红字应收单（负金额）
                                //没有记录支付状态，只标记结案处理
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    tb_SaleOutRe saleOutRe = await _appContext.Db.Queryable<tb_SaleOutRe>()
                                        .Where(c => c.DataStatus >= (int)DataStatus.确认
                                     && c.SaleOutRe_ID == receivablePayable.SourceBillId).SingleAsync();
                                    if (saleOutRe != null)
                                    {
                                        saleOutRe.DataStatus = (int)DataStatus.完结;
                                        saleOutRe.PayStatus = (int)PayStatus.全部付款;
                                        saleOutRe.Paytype_ID = entity.Paytype_ID;
                                        SaleOutReUpdateList.Add(saleOutRe);
                                    }

                                }
                            }

                            if (receivablePayable.SourceBizType == (int)BizType.采购入库单)
                            {
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    #region 更新对应业务的单据状态和付款情况

                                    tb_PurEntry purEntiry = await _appContext.Db.Queryable<tb_PurEntry>()
                                        .Includes(c => c.tb_purorder, b => b.tb_PurEntries)
                                      .Where(c => c.DataStatus >= (int)DataStatus.确认
                                     && c.PurEntryID == receivablePayable.SourceBillId).SingleAsync();
                                    if (purEntiry != null)
                                    {
                                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == purEntiry.TotalAmount)
                                        {
                                            //财务只管财务的状态?
                                            // purEntiry.DataStatus = (int)DataStatus.完结;
                                            purEntiry.PayStatus = (int)PayStatus.全部付款;
                                            purEntiry.Paytype_ID = entity.Paytype_ID;
                                        }

                                        if (purEntiry.tb_purorder.tb_PurEntries != null)
                                        {
                                            //如果这个出库单的上级 订单 是我次出库的。他出库的状态都是全部付款了。则这个订单就全部付款了。
                                            //订单要保证全部出库了。才能这样算否则就先不管订单状态。只是部分付款
                                            List<tb_PurEntry> otherPurEntrys = purEntiry.tb_purorder.tb_PurEntries
                                            .Where(c => c.PurEntryID != purEntiry.PurEntryID && c.PayStatus == (int)PayStatus.全部付款).ToList();

                                            if (receivablePayable.LocalPaidAmount == purEntiry.TotalAmount
                                                && otherPurEntrys.Sum(c => c.TotalAmount) + purEntiry.TotalAmount == purEntiry.tb_purorder.TotalAmount)
                                            {
                                                purEntiry.tb_purorder.PayStatus = (int)PayStatus.全部付款;
                                                purEntiry.tb_purorder.Paytype_ID = entity.Paytype_ID;
                                            }
                                            else
                                            {
                                                purEntiry.tb_purorder.PayStatus = (int)PayStatus.部分付款;
                                                purEntiry.tb_purorder.Paytype_ID = entity.Paytype_ID;
                                            }
                                        }
                                        purOrderUpdateList.Add(purEntiry.tb_purorder);
                                        purEntryUpdateList.Add(purEntiry);
                                    }

                                    #endregion
                                }
                            }

                            if (receivablePayable.SourceBizType == (int)BizType.采购退货单)
                            {
                                //厂商退款 时才处理
                                //退货单审核后生成红字应收单（负金额）
                                //没有记录支付状态，只标记结案处理
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付 || receivablePayable.ARAPStatus == (int)ARAPStatus.部分支付)
                                {
                                    tb_PurEntryRe purEntryRe = await _appContext.Db.Queryable<tb_PurEntryRe>()
                                        .Where(c => c.DataStatus >= (int)DataStatus.确认
                                     && c.PurEntryRe_ID == receivablePayable.SourceBillId).SingleAsync();
                                    if (purEntryRe != null)
                                    {
                                        if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                        {
                                            purEntryRe.DataStatus = (int)DataStatus.完结;
                                            purEntryRe.PayStatus = (int)PayStatus.全部付款;
                                        }
                                        else
                                        {
                                            purEntryRe.PayStatus = (int)PayStatus.部分付款;
                                        }
                                        purEntryRe.Paytype_ID = entity.Paytype_ID;
                                        purEntryReUpdateList.Add(purEntryRe);
                                    }
                                }
                            }

                            if (receivablePayable.SourceBizType == (int)BizType.销售价格调整单 || receivablePayable.SourceBizType == (int)BizType.采购价格调整单)
                            {
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    #region 更新对应业务的单据状态和付款情况

                                    tb_FM_PriceAdjustment priceAdjustment = await _appContext.Db.Queryable<tb_FM_PriceAdjustment>()
                                        .Includes(c => c.tb_FM_PriceAdjustmentDetails, b => b.tb_fm_priceadjustment)
                                      .Where(c => c.DataStatus >= (int)DataStatus.确认
                                     && c.AdjustId == receivablePayable.SourceBillId).SingleAsync();
                                    if (priceAdjustment != null)
                                    {
                                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == priceAdjustment.TotalLocalDiffAmount)
                                        {
                                            //priceAdjustment.DataStatus = (int)DataStatus.完结;
                                            //价格调整单是不是也要加一个付款方式？区别账期？
                                            priceAdjustment.PayStatus = (int)PayStatus.全部付款;
                                            priceAdjustment.Paytype_ID = entity.Paytype_ID;
                                            priceAdjustmentUpdateList.Add(priceAdjustment);
                                        }
                                    }

                                    #endregion
                                }
                            }


                            if (receivablePayable.SourceBizType == (int)BizType.其他费用收入
                                || receivablePayable.SourceBizType == (int)BizType.其他费用支出)
                            {
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    #region 更新对应业务的单据状态和付款情况

                                    tb_FM_OtherExpense OtherExpense = await _appContext.Db.Queryable<tb_FM_OtherExpense>()
                                        .Includes(c => c.tb_FM_OtherExpenseDetails)
                                      .Where(c => c.DataStatus >= (int)DataStatus.确认
                                     && c.ExpenseMainID == receivablePayable.SourceBillId).SingleAsync();
                                    if (OtherExpense != null)
                                    {
                                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == OtherExpense.TotalAmount)
                                        {
                                            OtherExpense.DataStatus = (int)DataStatus.完结;
                                            OtherExpense.PayStatus = (int)PayStatus.全部付款;
                                            OtherExpense.Paytype_ID = entity.Paytype_ID;
                                            otherExpenseUpdateList.Add(OtherExpense);
                                        }
                                    }

                                    #endregion
                                }
                            }

                            if (receivablePayable.SourceBizType == (int)BizType.费用报销单)
                            {
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    #region 更新对应业务的单据状态和付款情况

                                    tb_FM_ExpenseClaim ExpenseClaim = await _appContext.Db.Queryable<tb_FM_ExpenseClaim>()
                                        .Includes(c => c.tb_FM_ExpenseClaimDetails)
                                      .Where(c => c.DataStatus >= (int)DataStatus.确认
                                     && c.ClaimMainID == receivablePayable.SourceBillId).SingleAsync();
                                    if (ExpenseClaim != null)
                                    {
                                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == ExpenseClaim.ClaimAmount)
                                        {
                                            ExpenseClaim.DataStatus = (int)DataStatus.完结;
                                            ExpenseClaim.CloseCaseOpinions = "全部付款";
                                            //ExpenseClaim.PayStatus = (int)PayStatus.全部付款;
                                            //ExpenseClaim.Paytype_ID = entity.Paytype_ID;
                                            expenseClaimUpdateList.Add(ExpenseClaim);
                                        }
                                    }

                                    #endregion
                                }
                            }


                            if (receivablePayable.SourceBizType == (int)BizType.维修工单)
                            {
                                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    #region 更新对应业务的单据状态和付款情况

                                    tb_AS_RepairOrder RepairOrder = await _appContext.Db.Queryable<tb_AS_RepairOrder>()
                                        .Includes(c => c.tb_as_aftersaleapply, b => b.tb_AS_AfterSaleDeliveries)
                                      .Where(c => c.DataStatus >= (int)DataStatus.确认 && c.RepairOrderID == receivablePayable.SourceBillId).SingleAsync();
                                    if (RepairOrder != null)
                                    {
                                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == RepairOrder.TotalAmount)
                                        {
                                            //财务只管财务的状态
                                            // saleOut.DataStatus = (int)DataStatus.完结;
                                            RepairOrder.PayStatus = (int)PayStatus.全部付款;
                                            RepairOrder.Paytype_ID = entity.Paytype_ID.Value;
                                        }
                                        else
                                        {
                                            RepairOrder.PayStatus = (int)PayStatus.部分付款;
                                            RepairOrder.Paytype_ID = entity.Paytype_ID.Value;
                                        }

                                        RepairOrderUpdateList.Add(RepairOrder);
                                    }

                                    #endregion
                                }
                            }

                            //应收应付 正反都 生成核销记录
                            await settlementController.GenerateSettlement(entity, RecordDetail, receivablePayable);
                        }

                        receivablePayableUpdateList.AddRange(receivablePayableList);

                        #endregion
                    }

                    //单纯收款不用产生核销记录。核销要与业务相关联 这里只处理 应收，预收，对账单
                    //退款时写回上级预收付款单 状态为 已冲销.预先处理，不用核销.只是一个收款记录
                    //负数时
                    //这里收款审核，面对预先处理。只是一个记录，并且回写预收生效，待核销。不用生成核销记录。
                    if (group.Key == (int)BizType.预收款单 || group.Key == (int)BizType.预付款单)
                    {
                        List<tb_FM_PreReceivedPayment> PreReceivablePayableList = await _appContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                           .Where(c => sourcebillids.Contains(c.PreRPID))
                           .ToListAsync();
                        foreach (var prePayment in PreReceivablePayableList)
                        {
                            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
                            tb_FM_PaymentRecordDetail RecordDetail = entity.tb_FM_PaymentRecordDetails.FirstOrDefault(c => c.SourceBilllId == prePayment.PreRPID);
                            if (prePayment != null)
                            {
                                prePayment.PrePaymentStatus = (int)PrePaymentStatus.待核销;
                                prePayment.PrePayDate = DateTime.Now;
                                prePayment.ForeignBalanceAmount += RecordDetail.ForeignAmount;
                                prePayment.LocalBalanceAmount += RecordDetail.LocalAmount;

                                //预收付的退款操作，对应收付款审核时。要找他对应的正向预收付单。修改状态。和退回金额。
                                if (RecordDetail.LocalAmount < 0 || RecordDetail.ForeignAmount < 0)
                                {
                                    //负数时，他一定有一个正数的收款单。并且对应一个对应的预收付单。，预收则要转为已冲销。自己则为
                                    //预收的退款操作时。 应该是去找他相同的

                                    #region 通过他的来源单据，找到对应的预收付单
                                    //应该只有一条。 会不会更新的就是自己 prePayment
                                    //这是是预收付的  收款单时的处理？ 也可能是预收付的退款?
                                    tb_FM_PreReceivedPayment oldPrePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                        .Where(c => c.PreRPID == RecordDetail.SourceBilllId && c.PrePaymentStatus == (int)PrePaymentStatus.待核销)
                                       .SingleAsync();
                                    if (oldPrePayment != null)
                                    {
                                        oldPrePayment.LocalRefundAmount += Math.Abs(RecordDetail.LocalAmount);
                                        oldPrePayment.ForeignRefundAmount += Math.Abs(RecordDetail.ForeignAmount);
                                        oldPrePayment.LocalBalanceAmount = oldPrePayment.LocalBalanceAmount - Math.Abs(oldPrePayment.LocalRefundAmount) - Math.Abs(oldPrePayment.LocalPaidAmount);
                                        oldPrePayment.ForeignBalanceAmount = oldPrePayment.ForeignBalanceAmount - Math.Abs(oldPrePayment.ForeignRefundAmount) - Math.Abs(oldPrePayment.ForeignPaidAmount);
                                        //全退了则是 已冲销
                                        if (oldPrePayment.LocalRefundAmount == oldPrePayment.LocalPrepaidAmount || oldPrePayment.ForeignRefundAmount == oldPrePayment.ForeignPrepaidAmount)
                                        {
                                            oldPrePayment.PrePaymentStatus = (int)PrePaymentStatus.已退款;
                                            oldPrePayment.IsAvailable = false;
                                        }

                                        //有退，有核销 则是  部分核销
                                        if ((oldPrePayment.LocalPaidAmount > 0 && oldPrePayment.LocalRefundAmount > 0) ||
                                            (oldPrePayment.ForeignPaidAmount > 0 && oldPrePayment.ForeignRefundAmount > 0))
                                        {
                                            oldPrePayment.PrePaymentStatus = (int)PrePaymentStatus.部分核销;
                                        }

                                        //有退部分,还没有核销，后面可能退，也可能核销掉 则是  部分核销
                                        if ((oldPrePayment.LocalPaidAmount == 0 && oldPrePayment.LocalRefundAmount > 0 && oldPrePayment.LocalRefundAmount < oldPrePayment.LocalPrepaidAmount) ||
                                            (oldPrePayment.ForeignPaidAmount == 0 && oldPrePayment.ForeignRefundAmount > 0 && oldPrePayment.ForeignRefundAmount < oldPrePayment.ForeignPrepaidAmount))
                                        {
                                            oldPrePayment.PrePaymentStatus = (int)PrePaymentStatus.待核销;
                                        }
                                        //更新原来的上一个预付记录
                                        preReceivedPaymentUpdateList.Add(oldPrePayment);
                                    }
                                    #endregion

                                    #region 通过他的来源单据，找到对应的预收付单的收款单。标记为已关闭 !!!!!!!!!! 收款单 有是否反冲标记， 预收付中有退回金额

                                    //要调式
                                    //tb_FM_PaymentRecord oldPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                    //.Where(c => c.tb_FM_PaymentRecordDetails.Any(c => c.SourceBilllId == RecordDetail.SourceBilllId)
                                    //&& c.PaymentStatus == (int)PaymentStatus.已支付)
                                    // .SingleAsync();

                                    tb_FM_PaymentRecord oldPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                    .Where(c => c.tb_FM_PaymentRecordDetails.Any(c => c.SourceBilllId == RecordDetail.SourceBilllId)
                                    && c.PaymentStatus == (int)PaymentStatus.已支付)
                                     .FirstAsync();
                                    if (oldPayment != null)
                                    {
                                        // 更新原始记录 指向[负数]冲销记录
                                        oldPayment.ReversedByPaymentId = entity.PaymentId;
                                        oldPayment.ReversedByPaymentNo = entity.PaymentNo;
                                        oldPaymentUpdateList.Add(oldPayment);
                                        // 指向原始记录
                                        entity.ReversedOriginalId = oldPayment.PaymentId;
                                        entity.ReversedOriginalNo = oldPayment.PaymentNo;
                                    }
                                    entity.IsReversed = true;

                                    #endregion

                                    #region 对应的订单变为取消 不调用业务的取消是事务不能嵌套

                                    if (prePayment.SourceBizType == (int)BizType.销售订单 || prePayment.SourceBizType == (int)BizType.采购订单)
                                    {
                                        var ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                                        if (prePayment.SourceBizType == (int)BizType.销售订单)
                                        {
                                            var saleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                                                .Includes(c => c.tb_SaleOrderDetails)
                                            .Where(c => c.SOrder_ID == prePayment.SourceBillId)
                                            .SingleAsync();
                                            if (saleOrder != null)
                                            {

                                                saleOrder.ApprovalOpinions += $" 订金退款，订单取消作废";
                                                saleOrder.DataStatus = (int)DataStatus.作废;
                                                saleOrderUpdateList.Add(saleOrder);

                                            }


                                            #region 更新库存的拟销量

                                            List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                                            foreach (var child in saleOrder.tb_SaleOrderDetails)
                                            {
                                                #region 库存表的更新 ，
                                                tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                                                //更新在途库存
                                                inv.Sale_Qty = inv.Sale_Qty - child.Quantity;
                                                BusinessHelper.Instance.EditEntity(inv);
                                                #endregion
                                                invUpdateList.Add(inv);
                                            }

                                            DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                                            var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);

                                            #endregion

                                        }
                                        else if (prePayment.SourceBizType == (int)BizType.采购订单)
                                        {
                                            var purOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                                .Includes(c => c.tb_PurOrderDetails)
                                            .Where(c => c.SOrder_ID == prePayment.SourceBillId)
                                            .SingleAsync();
                                            if (purOrder != null)
                                            {
                                                purOrder.ApprovalOpinions += $" 订金退款，订单取消";
                                                purOrder.DataStatus = (int)DataStatus.作废;
                                                purOrderUpdateList.Add(purOrder);
                                            }

                                            #region 更新库存的拟销量

                                            List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                                            foreach (var child in purOrder.tb_PurOrderDetails)
                                            {
                                                #region 库存表的更新 ，
                                                tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                                                inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;
                                                BusinessHelper.Instance.EditEntity(inv);
                                                #endregion
                                                invUpdateList.Add(inv);
                                            }

                                            DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                                            var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);

                                            #endregion


                                        }
                                    }

                                    #endregion

                                }

                                #region  预收的收款确认时，可以自动核销应收的单据，基本是 销售订单的进款。去核销出库的应收。因为操作时间差。会有这种情况。正向反向都可以

                                //如果是[预收款]的确认到账，则会自动去核销[应收款]一一对应的单据[订单对应出库单]。减少工作量，反过来。如果预收款等待核销。应收审核时也会自动
                                if (_appContext.FMConfig.EnablePaymentAutoOffsetAR)
                                {
                                    if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                    {
                                        if (prePayment.SourceBizType == (int)BizType.销售订单)
                                        {
                                            var SaleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                                                .Includes(c => c.tb_SaleOuts, b => b.tb_saleorder)
                                            .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID && c.SOrder_ID == prePayment.SourceBillId)
                                            .SingleAsync();

                                            if (SaleOrder != null)
                                            {
                                                long[] SaleOutIds = SaleOrder.tb_SaleOuts.Select(c => c.SaleOut_MainID).ToArray();

                                                //部分支付暂时不自动处理
                                                var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                                                var receivablePayables = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                                                .Where(c => c.ARAPStatus >= (int)ARAPStatus.待支付
                                                                && c.CustomerVendor_ID == entity.CustomerVendor_ID
                                                                && SaleOutIds.Contains(c.SourceBillId.Value))
                                                                .ToListAsync();


                                                var receivablePayableController = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                                                //一切刚刚好时才能去核销
                                                foreach (var receivablePayable in receivablePayables)
                                                {
                                                    if (receivablePayable.TotalLocalPayableAmount == entity.TotalLocalAmount &&
                                                        receivablePayable.TotalLocalPayableAmount == prePayment.LocalBalanceAmount)
                                                    {
                                                        List<tb_FM_PreReceivedPayment> ProcessPreReceivablePayableList = new List<tb_FM_PreReceivedPayment>();
                                                        ProcessPreReceivablePayableList.Add(prePayment);
                                                        await receivablePayableController.ApplyManualPaymentAllocation(receivablePayable, ProcessPreReceivablePayableList);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //预付的确认时，可以核销应付
                                if (_appContext.FMConfig.EnablePaymentAutoOffsetAP)
                                {
                                    if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                    {
                                        if (prePayment.SourceBizType == (int)BizType.采购订单)
                                        {
                                            var PurOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                                .Includes(c => c.tb_PurEntries, b => b.tb_purorder)
                                            .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID && c.SOrder_ID == prePayment.SourceBillId)
                                            .SingleAsync();

                                            if (PurOrder != null)
                                            {
                                                long[] PurEntryIDs = PurOrder.tb_PurEntries.Select(c => c.PurEntryID).ToArray();

                                                //部分支付暂时不自动处理
                                                var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                                                var receivablePayables = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                                                .Where(c => c.ARAPStatus >= (int)ARAPStatus.待支付
                                                                && c.CustomerVendor_ID == entity.CustomerVendor_ID
                                                                && PurEntryIDs.Contains(c.SourceBillId.Value))
                                                                .ToListAsync();


                                                var receivablePayableController = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                                                //一切刚刚好时才能去核销
                                                foreach (var receivablePayable in receivablePayables)
                                                {
                                                    if (receivablePayable.TotalLocalPayableAmount == entity.TotalLocalAmount &&
                                                        receivablePayable.TotalLocalPayableAmount == prePayment.LocalBalanceAmount)
                                                    {
                                                        List<tb_FM_PreReceivedPayment> ProcessPreReceivablePayableList = new List<tb_FM_PreReceivedPayment>();
                                                        ProcessPreReceivablePayableList.Add(prePayment);
                                                        await receivablePayableController.ApplyManualPaymentAllocation(receivablePayable, ProcessPreReceivablePayableList);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                                #endregion
                                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                {
                                    if (prePayment.SourceBizType == (int)BizType.销售订单)
                                    {
                                        //如果是多次预付，则合并订单中订金字段的预付金额
                                        //多次预付款时，则要找到这个订单名下的所有订金预付款的单。如果只有一行。则不用累计。如果是多行。要需要？

                                        #region 更新对应业务的单据状态和订金金额情况

                                        List<tb_FM_PreReceivedPayment> SameOrderPrePayments = await _appContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                                                            .Where(c => c.SourceBillId == prePayment.SourceBillId)
                                                            .Where(c => c.SourceBizType == (int)BizType.销售订单)
                                                            .Where(c => c.PrePaymentStatus >= (int)PrePaymentStatus.待核销)
                                                            .Where(c => c.PreRPID != prePayment.PreRPID)//排除当前的
                                                            .ToListAsync();
                                        if (SameOrderPrePayments != null && SameOrderPrePayments.Count > 0)
                                        {
                                            tb_SaleOrder saleOrder = await _appContext.Db.Queryable<tb_SaleOrder>()
                                                 .Where(c => c.SOrder_ID == prePayment.SourceBillId).SingleAsync();
                                            if (saleOrder != null)
                                            {
                                                //原来的。加上当前的
                                                saleOrder.Deposit = SameOrderPrePayments.Sum(c => c.LocalPrepaidAmount) + prePayment.LocalPrepaidAmount;
                                                //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。

                                                if (saleOrder.Deposit == saleOrder.TotalAmount)
                                                {
                                                    saleOrder.PayStatus = (int)PayStatus.全额预付;
                                                    saleOrder.Paytype_ID = entity.Paytype_ID.Value;
                                                }
                                                else
                                                {
                                                    saleOrder.PayStatus = (int)PayStatus.部分付款;
                                                    saleOrder.Paytype_ID = entity.Paytype_ID.Value;
                                                }
                                                saleOrderUpdateList.Add(saleOrder);
                                            }

                                        }

                                        #endregion
                                    }
                                }

                            }
                        }
                        //这里要测试哦！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！会不会更新的就是自己，
                        preReceivedPaymentUpdateList.AddRange(PreReceivablePayableList);


                    }
                }


                if (oldPaymentUpdateList.Any())
                {
                    //更新原来的上一个预付记录
                    await _unitOfWorkManage.GetDbClient().Updateable(oldPaymentUpdateList).UpdateColumns(t => new
                    {
                        t.ReversedByPaymentId,
                        t.ReversedByPaymentNo
                    }
                    ).ExecuteCommandAsync();
                }

                if (otherExpenseUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(otherExpenseUpdateList).UpdateColumns(t => new
                    {
                        t.DataStatus,
                        t.Paytype_ID,
                        t.PayStatus,
                    }).ExecuteCommandAsync();
                }

                if (priceAdjustmentUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(priceAdjustmentUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (purEntryReUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(purEntryReUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (SaleOutReUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutRe>(SaleOutReUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }
                if (saleOutUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(saleOutUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (RepairOrderUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(RepairOrderUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (saleOrderUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(saleOrderUpdateList).UpdateColumns(t => new
                    {
                        t.Deposit,
                        t.ApprovalOpinions,
                        t.DataStatus,
                        t.PayStatus,
                        t.Paytype_ID,
                    }).ExecuteCommandAsync();

                }

                if (purEntryUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(purEntryUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (purOrderUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(purOrderUpdateList).UpdateColumns(t => new
                    {
                        t.ApprovalOpinions,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (receivablePayableUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(receivablePayableUpdateList).UpdateColumns(it =>
                                new
                                {
                                    it.ARAPStatus,
                                    it.ForeignPaidAmount,
                                    it.LocalPaidAmount,
                                    it.LocalBalanceAmount,
                                    it.ForeignBalanceAmount,
                                }).ExecuteCommandAsync();
                }

                if (preReceivedPaymentUpdateList.Any())
                {
                    //更新
                    var preRs = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(preReceivedPaymentUpdateList).UpdateColumns(it =>
                                new
                                {
                                    it.PrePayDate,
                                    it.PrePaymentStatus,
                                    it.ForeignBalanceAmount,
                                    it.LocalBalanceAmount,
                                }).ExecuteCommandAsync();
                }



                //更新账户余额
                if (entity.tb_fm_account == null && entity.Account_id.HasValue)
                {
                    entity.tb_fm_account = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(c => c.Account_id == entity.Account_id).FirstAsync();
                    //和账户相同的币种才更新
                    if (entity.tb_fm_account.Currency_ID == entity.Currency_ID)
                    {
                        if (entity.tb_fm_account.Currency_ID == _appContext.BaseCurrency.Currency_ID)
                        {
                            entity.tb_fm_account.CurrentBalance += entity.TotalLocalAmount;
                        }
                        else
                        {
                            entity.tb_fm_account.CurrentBalance += entity.TotalForeignAmount;
                        }
                    }

                    await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_Account>(entity.tb_fm_account).UpdateColumns(it => new { it.CurrentBalance }).ExecuteCommandAsync();
                }
                #endregion

                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                //等待真正支付
                entity.PaymentStatus = (int)PaymentStatus.已支付;

                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(entity).UpdateColumns(it => new
                {
                    it.ApprovalStatus,
                    it.PaymentStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                    it.Paytype_ID
                }).ExecuteCommandAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }
        }

        public static bool ValidatePaymentDetails(List<tb_FM_PaymentRecordDetail> paymentDetails, ReturnResults<T> returnResults = null)
        {
            // 按来源业务类型分组
            var groupedByBizType = paymentDetails
                .GroupBy(d => d.SourceBizType)
                .ToList();

            foreach (var bizTypeGroup in groupedByBizType)
            {
                // 按来源单号分组
                var groupedByBillNo = bizTypeGroup
                    .GroupBy(d => d.SourceBillNo)
                    .ToList();

                foreach (var billNoGroup in groupedByBillNo)
                {
                    var items = billNoGroup.ToList();

                    // 如果只有一条记录，直接通过
                    if (items.Count == 1)
                        continue;

                    // 如果有两条记录，检查是否为对冲情况
                    if (items.Count == 2)
                    {
                        // 计算本币金额总和
                        decimal totalLocalAmount = items.Sum(i => i.LocalAmount);
                        // 计算外币金额总和
                        decimal totalForeignAmount = items.Sum(i => i.ForeignAmount);

                        // 检查是否满足对冲条件（总和接近0，考虑浮点数精度问题）
                        if (Math.Abs(totalLocalAmount) < 0.001m && Math.Abs(totalForeignAmount) < 0.001m)
                            continue;
                    }
                    returnResults.ErrorMsg = $"{(ReceivePaymentType)paymentDetails[0].tb_fm_paymentrecord.ReceivePaymentType}单中不能存在相同业务来源的数据:{(BizType)groupedByBizType[0].Key}，来源单号为:{groupedByBillNo[0].Key}。";
                    returnResults.ErrorMsg += $"\r\n通常是生成了重复{(ReceivePaymentType)paymentDetails[0].tb_fm_paymentrecord.ReceivePaymentType}单。请仔细核对！";
                    // 其他情况均视为不合法
                    return false;
                }
            }

            return true;
        }


        public tb_FM_PaymentRecord BuildPaymentRecord(tb_FM_ExpenseClaim entity)
        {
            //预收付款单 审核时 自动生成 收付款记录

            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entity);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;

            paymentRecord.ReceivePaymentType = (int)ReceivePaymentType.付款;
            paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.费用报销单);

            tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
            #region 明细 
            paymentRecordDetail.SourceBillNo = entity.ClaimNo;
            paymentRecordDetail.SourceBilllId = entity.ClaimMainID;
            paymentRecordDetail.Currency_ID = entity.Currency_ID;
            paymentRecordDetail.LocalAmount = entity.ClaimAmount;
            paymentRecordDetail.SourceBizType = (int)BizType.费用报销单;
            paymentRecordDetail.Summary = $"由费用报销单{entity.ClaimNo}转换自动生成。";

            #endregion
            paymentRecord.TotalLocalAmount = paymentRecordDetail.LocalAmount;
            paymentRecord.TotalForeignAmount = paymentRecordDetail.ForeignAmount;

            paymentRecord.PaymentDate = entity.DocumentDate;

            paymentRecord.Reimburser = entity.Employee_ID;
            paymentRecord.CustomerVendor_ID = null;
            paymentRecord.PayeeInfoID = entity.PayeeInfoID;
            //paymentRecord.PaymentImagePath = entity.CloseCaseImagePath;
            if (entity.tb_fm_payeeinfo != null)
            {
                paymentRecord.PayeeAccountNo = entity.tb_fm_payeeinfo.Account_No;
            }


            paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();

            // paymentRecord.ReferenceNo=entity.no
            //自动提交 审核，等待确认收款 或支付 【实际核对收款情况到账】
            paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);

            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

            BusinessHelper.Instance.InitEntity(paymentRecord);
            //long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(paymentRecord).ExecuteReturnSnowflakeIdAsync();
            //if (id > 0)
            //{
            //    paymentRecordDetail.PaymentId = id;
            //    await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecordDetail>(paymentRecordDetail).ExecuteReturnSnowflakeIdAsync();
            //    paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);
            //}
            return paymentRecord;
        }

        public tb_FM_PaymentRecord BuildPaymentRecord(tb_FM_OtherExpense entity)
        {
            //其它费用收入支出 审核时 自动生成 收付款记录
            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entity);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            //0  支出  1为收入
            if (entity.EXPOrINC == true)
            {
                paymentRecord.ReceivePaymentType = (int)ReceivePaymentType.收款;
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
            }
            else
            {
                paymentRecord.ReceivePaymentType = (int)ReceivePaymentType.付款;
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
            }
            tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
            #region 明细 


            paymentRecordDetail.SourceBillNo = entity.ExpenseNo;
            paymentRecordDetail.SourceBilllId = entity.ExpenseMainID;
            if (entity.Currency_ID.HasValue)
            {
                paymentRecordDetail.Currency_ID = entity.Currency_ID.GetValueOrDefault();
            }
            else
            {
                paymentRecordDetail.Currency_ID = _appContext.BaseCurrency.Currency_ID;
            }

            if (paymentRecord.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecordDetail.SourceBizType = (int)BizType.其他费用收入;
                paymentRecordDetail.Summary = $"由其他费用收入单{entity.ExpenseNo}转换自动生成。";
            }
            else
            {

                paymentRecordDetail.SourceBizType = (int)BizType.其他费用支出;
                paymentRecordDetail.Summary = $"由其他费用支出单{entity.ExpenseNo}转换自动生成。";
            }
            //支出不用负数。后面会通过 ReceivePaymentType
            paymentRecordDetail.LocalAmount = entity.TotalAmount;
            #endregion

            //一单就一行时才这样
            paymentRecord.TotalLocalAmount = paymentRecordDetail.LocalAmount;
            paymentRecord.TotalForeignAmount = paymentRecordDetail.ForeignAmount;

            paymentRecord.PaymentDate = entity.DocumentDate;

            //paymentRecord.CustomerVendor_ID = entity.cus;
            //paymentRecord.PayeeInfoID = entity.PayeeInfoID;
            //paymentRecord.PaymentImagePath = entity.PaymentImagePath;
            //paymentRecord.PayeeAccountNo = entity.PayeeAccountNo;

            paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();

            // paymentRecord.ReferenceNo=entity.no
            //自动提交 审核，等待确认收款 或支付 【实际核对收款情况到账】
            paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);

            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

            BusinessHelper.Instance.InitEntity(paymentRecord);
            //long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(paymentRecord).ExecuteReturnSnowflakeIdAsync();
            //if (id > 0)
            //{
            //    paymentRecordDetail.PaymentId = id;
            //    await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecordDetail>(paymentRecordDetail).ExecuteReturnSnowflakeIdAsync();
            //    paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);
            //}
            return paymentRecord;
        }

        // 生成收付款记录表
        /// <summary>
        /// 生成收付款记录表
        /// </summary>
        /// <param name="entity">预收付表</param>
        /// <param name="isRefund">true 如果是退款时 金额为负，SettlementType=退款红冲</param>
        /// <returns></returns>
        public tb_FM_PaymentRecord BuildPaymentRecord(tb_FM_PreReceivedPayment entity, bool isRefund)
        {
            //预收付款单 审核时 自动生成 收付款记录

            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entity);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            paymentRecord.ReceivePaymentType = entity.ReceivePaymentType;
            paymentRecord.Employee_ID = entity.Employee_ID;
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
            }
            else
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
            }
            tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
            #region 明细   一笔预收付款单只有一条明细

            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecordDetail.SourceBizType = (int)BizType.预收款单;
            }
            else
            {
                paymentRecordDetail.SourceBizType = (int)BizType.预付款单;
            }
            paymentRecordDetail.IsFromPlatform = entity.IsFromPlatform;
            paymentRecordDetail.SourceBillNo = entity.PreRPNO;
            paymentRecordDetail.SourceBilllId = entity.PreRPID;
            paymentRecordDetail.ExchangeRate = entity.ExchangeRate;
            paymentRecordDetail.Currency_ID = entity.Currency_ID;
            paymentRecordDetail.ExchangeRate = entity.ExchangeRate;
            #endregion
            if (isRefund)
            {
                paymentRecordDetail.ForeignAmount = -entity.ForeignPrepaidAmount;
                paymentRecordDetail.LocalAmount = -entity.LocalPrepaidAmount;
            }
            else
            {
                paymentRecordDetail.LocalAmount = entity.LocalPrepaidAmount;
                paymentRecordDetail.ForeignAmount = entity.ForeignPrepaidAmount;
            }
            paymentRecordDetail.Summary = $"来自预{(ReceivePaymentType)entity.ReceivePaymentType}{entity.PreRPNO}。";
            if (entity.PrePaymentReason.Contains("平台单号"))
            {
                paymentRecordDetail.Summary += entity.PrePaymentReason;
            }



            paymentRecord.TotalLocalAmount = paymentRecordDetail.LocalAmount;
            paymentRecord.TotalForeignAmount = paymentRecordDetail.ForeignAmount;
            paymentRecord.PaymentDate = entity.PrePayDate;
            paymentRecord.Currency_ID = entity.Currency_ID;
            paymentRecord.CustomerVendor_ID = entity.CustomerVendor_ID;
            paymentRecord.PayeeInfoID = entity.PayeeInfoID;
            paymentRecord.PaymentImagePath = entity.PaymentImagePath;
            paymentRecord.PayeeAccountNo = entity.PayeeAccountNo;
            paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();

            // paymentRecord.ReferenceNo=entity.no
            //自动提交 审核，等待确认收款 或支付 【实际核对收款情况到账】
            paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);

            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

            BusinessHelper.Instance.InitEntity(paymentRecord);
            //long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(paymentRecord).ExecuteReturnSnowflakeIdAsync();
            //if (id > 0)
            //{
            //    paymentRecordDetail.PaymentId = id;
            //    await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecordDetail>(paymentRecordDetail).ExecuteReturnSnowflakeIdAsync();
            //    paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);
            //}
            return paymentRecord;
        }

        // 生成收付款记录表
        public tb_FM_PaymentRecord BuildPaymentRecord(List<tb_FM_ReceivablePayable> entities, tb_FM_PaymentRecord OriginalPaymentRecord = null)
        {
            //通过应收 自动生成 收付款记录
            //ReturnResults<tb_FM_PaymentRecord> rs = new ReturnResults<tb_FM_PaymentRecord>();
            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entities[0]);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            paymentRecord.ReceivePaymentType = entities[0].ReceivePaymentType;


            List<tb_FM_PaymentRecordDetail> details = mapper.Map<List<tb_FM_PaymentRecordDetail>>(entities);
            List<tb_FM_PaymentRecordDetail> NewDetails = new List<tb_FM_PaymentRecordDetail>();

            for (int i = 0; i < details.Count; i++)
            {
                #region 明细 
                tb_FM_PaymentRecordDetail paymentRecordDetail = details[i];
                if (paymentRecord.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    paymentRecordDetail.SourceBizType = (int)BizType.应收款单;
                }
                else
                {
                    paymentRecordDetail.SourceBizType = (int)BizType.应付款单;
                }
                paymentRecordDetail.Summary = $"由应{((ReceivePaymentType)paymentRecord.ReceivePaymentType).ToString()}转换自动生成。";
                var entity = entities.FirstOrDefault(c => c.ARAPId == details[i].SourceBilllId);
                if (entity != null)
                {
                    paymentRecordDetail.Summary += entity.Remark;
                }
                #endregion
                NewDetails.Add(paymentRecordDetail);
            }

            paymentRecord.PaymentDate = System.DateTime.Now;
            paymentRecord.Currency_ID = paymentRecord.Currency_ID;

            //应收的余额给到付款单。创建收款
            paymentRecord.TotalForeignAmount = NewDetails.Sum(c => c.ForeignAmount);
            paymentRecord.TotalLocalAmount = NewDetails.Sum(c => c.LocalAmount);
            if (paymentRecord.TotalForeignAmount < 0 || paymentRecord.TotalLocalAmount < 0)
            {
                paymentRecord.IsReversed = true;
                if (OriginalPaymentRecord != null)
                {
                    paymentRecord.ReversedOriginalId = OriginalPaymentRecord.PaymentId;
                    paymentRecord.ReversedOriginalNo = OriginalPaymentRecord.PaymentNo;
                }
            }

            //默认给第一个
            paymentRecord.PayeeInfoID = entities[0].PayeeInfoID;
            paymentRecord.CustomerVendor_ID = entities[0].CustomerVendor_ID;
            paymentRecord.PayeeAccountNo = entities[0].PayeeAccountNo;
            paymentRecord.tb_FM_PaymentRecordDetails = NewDetails;
            if (entities[0].ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
                if (paymentRecord.tb_FM_PaymentRecordDetails.Where(c => c.IsFromPlatform.HasValue && c.IsFromPlatform == true).ToList().Count == paymentRecord.tb_FM_PaymentRecordDetails.Count)
                {
                    paymentRecord.IsFromPlatform = true;
                }
            }
            else
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
            }
            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
            var checkList = paymentRecord.tb_FM_PaymentRecordDetails.GroupBy(c => c.SourceBizType, c => c.SourceBilllId).ToList();
            if (checkList.Count > 1)
            {
                throw new Exception("收付款单明细中，同一业务下同一张单据不能重复分次收款。\r\n相同业务下的单据必须为一行。");
                //rs.ErrorMsg = ("收付款单明细中，同一业务下同一张单据不能重复分次收款。\r\n相同业务下的单据必须为一行。");
                //rs.ReturnObject = paymentRecord;
                //return rs;
            }
            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());


            BusinessHelper.Instance.InitEntity(paymentRecord);
            //            paymentRecord.ReferenceNo=entity.no 流水
            //if (SaveToDb)
            //{
            //    //自动提交
            //    paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            //    var ctr = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
            //    //比方 暂时没有供应商  又是外键，则是如何处理的？
            //    bool vb = ShowInvalidMessage(ctr.Validator(paymentRecord));
            //    if (!vb)
            //    {
            //        rs.ErrorMsg = "数据校验失败。";
            //        rs.ReturnObject = paymentRecord;
            //        return rs;
            //    }
            //    var paymentRecordController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
            //    ReturnMainSubResults<tb_FM_PaymentRecord> rsms = await paymentRecordController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord);
            //    if (rsms.Succeeded)
            //    {
            //        paymentRecord.tb_FM_PaymentRecordDetails.ForEach(c => c.PaymentId = rsms.ReturnObject.PaymentId);
            //    }
            //    else
            //    {
            //        //记录错误日志
            //        _logger.LogError(rsms.ErrorMsg);
            //    }
            //}
            //else
            //{
            paymentRecord.PaymentStatus = (int)PaymentStatus.草稿;
            return paymentRecord;
            ////}
            //rs.Succeeded = true;
            //rs.ReturnObject = paymentRecord;
            //return rs;
        }

        public static bool ShowInvalidMessage(ValidationResult results)
        {
            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }
            if (!results.IsValid)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return results.IsValid;
        }
        public async Task<bool> BaseLogicDeleteAsync(tb_FM_PaymentRecord ObjectEntity)
        {
            //  ReturnResults<tb_FM_PaymentRecordController> rrs = new Business.ReturnResults<tb_FM_PaymentRecordController>();
            int count = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PaymentRecord>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                //rrs.Succeeded = true;
                return true;
                ////生成时暂时只考虑了一个主键的情况
                // MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentRecordController>(entity);
            }
            return false;
        }

        public async Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model, bool UseTran = false) where C : class
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            //缓存当前编辑的对象。如果撤销就回原来的值
            T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
            try
            {

                tb_FM_PaymentRecord entity = model as tb_FM_PaymentRecord;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };

                if (UseTran)
                {
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();
                }


                if (entity.PaymentId > 0)
                {

                    rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PaymentRecord>(entity as tb_FM_PaymentRecord)
               .Include(m => m.tb_FM_PaymentRecordDetails)
           .ExecuteCommandAsync();
                }
                else
                {
                    rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PaymentRecord>(entity as tb_FM_PaymentRecord)
            .Include(m => m.tb_FM_PaymentRecordDetails)

            .ExecuteCommandAsync();


                }

                if (UseTran)
                {
                    // 注意信息的完整性
                    _unitOfWorkManage.CommitTran();
                }
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.PaymentId;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                if (UseTran)
                {
                    _unitOfWorkManage.RollbackTran();
                }
                //出错后，取消生成的ID等值
                command.Undo();
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
                _logger.Error(ex);
            }

            return rsms;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async virtual Task<bool> BatchApproval(List<tb_FM_PaymentRecord> entitys, ApprovalEntity approvalEntity)
        {
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                if (!approvalEntity.ApprovalResults)
                {
                    if (entitys == null)
                    {
                        return false;
                    }
                }
                else
                {
                    foreach (var entity in entitys)
                    {
                        //这部分是否能提出到上一级公共部分？
                        entity.PaymentStatus = (int)PaymentStatus.待审核;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                    }

                    //只更新指定列
                    var result = _unitOfWorkManage.GetDbClient().Updateable(entitys).UpdateColumns(it => new
                    {
                        it.PaymentStatus,
                        it.ApprovalOpinions,
                        it.ApprovalResults,
                        it.ApprovalStatus,
                        it.Approver_at,
                        it.Approver_by
                    }).ExecuteCommandHasChangeAsync();
                }
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                _logger.Error(approvalEntity.bizName + "事务回滚");
                return false;
            }

        }


        /// <summary>
        /// 这里如果查询得足够详细。就类似对账单了？
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PaymentRecord> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PaymentRecord>()
                .Where(m => m.PaymentId == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_customervendor)
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_FM_PaymentRecordDetails)
                              .Includes(a => a.tb_FM_PaymentRecords_Originals)
                              .Includes(a => a.tb_FM_PaymentRecords_Reverseds)
                              .Includes(a => a.tb_fm_paymentrecord_Original)
                              .Includes(a => a.tb_fm_paymentrecord_Reversed)
                            .ToListAsync();


            foreach (var item in list)
            {
                //为了查询效率。收款明细中，按业务类型查。虽然少。但是有这种情况。如 又有预付，又有应付
                //相同客户，多个应收可以合成一个收款 。所以明细中就是对应的应收单。
                //为了提高性能 将按业务类型分组后再找到对应的单据去处理
                //目前 所有业务都进应收应付 简化逻辑 
                var groupList = item.tb_FM_PaymentRecordDetails.GroupBy(c => c.SourceBizType).Select(c => new { SourceBizType = c.Key }).ToList();

                var details = item.tb_FM_PaymentRecordDetails;
                Dictionary<int, List<long>> GroupResult = details
                    .GroupBy(d => d.SourceBizType)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(d => d.PaymentDetailId).ToList()
                    );

                foreach (var group in GroupResult)
                {
                    long[] sourcebillids = item.tb_FM_PaymentRecordDetails.Where(c => group.Value.Contains(c.PaymentDetailId)).Select(c => c.SourceBilllId).ToArray();

                    if (group.Key == (int)BizType.应收款单 || group.Key == (int)BizType.应付款单)
                    {
                        #region  
                        List<tb_FM_ReceivablePayable> receivablePayableList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                             .Includes(c => c.tb_FM_ReceivablePayableDetails)
                             .Where(c => sourcebillids.Contains(c.ARAPId))
                             .ToListAsync();
                        foreach (var detail in item.tb_FM_PaymentRecordDetails)
                        {
                            detail.tb_FM_ReceivablePayables = receivablePayableList.Where(c => c.SourceBillId.Value == detail.SourceBilllId).ToList();
                        }

                        #endregion
                    }


                    if (group.Key == (int)BizType.预收款单 || group.Key == (int)BizType.预付款单)
                    {
                        List<tb_FM_PreReceivedPayment> PreReceivablePayableList = await _appContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                           .Where(c => sourcebillids.Contains(c.PreRPID))
                           .ToListAsync();
                        foreach (var detail in item.tb_FM_PaymentRecordDetails)
                        {
                            detail.tb_FM_PreReceivedPayments = PreReceivablePayableList.Where(c => c.SourceBillId.Value == detail.SourceBilllId).ToList();
                        }

                    }
                }

            }

            return list as List<T>;
        }




    }

}



