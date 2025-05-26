
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
using RUINORERP.Business.FMService;
using MapsterMapper;
using IMapper = AutoMapper.IMapper;
using System.Text;
using System.Windows.Forms;

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
            rmrs.ErrorMsg = "付款记录，已审核后不能反审。只能红冲";
            await Task.Delay(0);
            return rmrs;
        }

        /// <summary>
        /// 付款单审核通过时，更新对应的业务单据的收款状态。更新余额
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
                //如果一个单位，正好正向500，负数-500 ，相抵消是正好为0，则可以为零。审核后要将应收应付核销掉。
                //只有明细中有负数才可能等于0
                if (entity.tb_FM_PaymentRecordDetails.Any(c => c.LocalAmount > 0) || (entity.TotalLocalAmount == 0 && entity.TotalForeignAmount == 0))
                {
                    rmrs.ErrorMsg = "付款金额不能为0!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region

                //多个应收可以合成一个收款 。所以明细中就是对应的应收单。
                if (entity.tb_FM_PaymentRecordDetails != null)
                {
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
                                    receivablePayable.ARAPStatus = (long)ARAPStatus.已结清;
                                }
                                //付过，没付结清
                                if ((receivablePayable.ForeignBalanceAmount > 0 && receivablePayable.ForeignPaidAmount > 0)
                                    || (receivablePayable.LocalBalanceAmount > 0 && receivablePayable.LocalPaidAmount > 0))
                                {
                                    receivablePayable.ARAPStatus = (long)ARAPStatus.部分支付;
                                }
                                //写回业务 原始单据的完结状态，销售出库。销售订单。
                                //通过的来源类型，来源单号，来源编号分组得到原始单据数据组后再根据类型分别处理更新状态

                                if (receivablePayable.SourceBizType == (long)BizType.销售出库单)
                                {
                                    if (receivablePayable.ARAPStatus == (long)ARAPStatus.已结清)
                                    {
                                        #region 更新对应业务的单据状态和付款情况

                                        tb_SaleOut saleOut = await _appContext.Db.Queryable<tb_SaleOut>()
                                            .Includes(c => c.tb_saleorder, b => b.tb_SaleOuts)
                                          .Where(c => c.DataStatus == (long)DataStatus.确认
                                         && c.SaleOut_MainID == receivablePayable.SourceBillId).SingleAsync();
                                        if (saleOut != null)
                                        {
                                            //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                            if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == saleOut.TotalAmount)
                                            {
                                                saleOut.DataStatus = (int)DataStatus.完结;
                                                saleOut.PayStatus = (int)PayStatus.全部付款;
                                            }

                                            if (saleOut.tb_saleorder.tb_SaleOuts != null)
                                            {
                                                //如果这个出库单的上级 订单 是我次出库的。他出库的状态都是全部付款了。则这个订单就全部付款了。
                                                //订单要保证全部出库了。才能这样算否则就先不管订单状态。只是部分付款
                                                if (
                                                    saleOut.tb_saleorder.TotalQty == saleOut.tb_saleorder.tb_SaleOuts.Sum(c => c.TotalQty) &&
                                                    saleOut.tb_saleorder.tb_SaleOuts.Where(c => c.PayStatus == (int)PayStatus.全部付款).ToList().Count ==
                                                    saleOut.tb_saleorder.tb_SaleOuts.Count)
                                                {
                                                    saleOut.tb_saleorder.PayStatus = (int)PayStatus.全部付款;
                                                }
                                                else
                                                {
                                                    saleOut.tb_saleorder.PayStatus = (int)PayStatus.部分付款;
                                                }
                                            }

                                            saleOrderUpdateList.Add(saleOut.tb_saleorder);
                                            saleOutUpdateList.Add(saleOut);
                                        }

                                        #endregion
                                    }
                                }
                                if (receivablePayable.SourceBizType == (long)BizType.销售退回单)
                                {
                                    //退货单审核后生成红字应收单（负金额）
                                    //没有记录支付状态，只标记结案处理
                                    if (receivablePayable.ARAPStatus == (long)ARAPStatus.已结清)
                                    {
                                        tb_SaleOutRe saleOutRe = await _appContext.Db.Queryable<tb_SaleOutRe>()
                                            .Where(c => c.DataStatus == (long)DataStatus.确认
                                         && c.SaleOutRe_ID == receivablePayable.SourceBillId).SingleAsync();
                                        if (saleOutRe != null)
                                        {
                                            saleOutRe.DataStatus = (int)DataStatus.完结;
                                        }
                                        //退款情况少，直接更新。
                                        await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutRe>(saleOutRe).ExecuteCommandAsync();
                                    }
                                }

                                if (receivablePayable.SourceBizType == (long)BizType.采购入库单)
                                {
                                    if (receivablePayable.ARAPStatus == (long)ARAPStatus.已结清)
                                    {
                                        #region 更新对应业务的单据状态和付款情况

                                        tb_PurEntry purEntiry = await _appContext.Db.Queryable<tb_PurEntry>()
                                            .Includes(c => c.tb_purorder, b => b.tb_PurEntries)
                                          .Where(c => c.DataStatus == (long)DataStatus.确认
                                         && c.PurEntryID == receivablePayable.SourceBillId).SingleAsync();
                                        if (purEntiry != null)
                                        {
                                            //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                            if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == purEntiry.TotalAmount)
                                            {
                                                purEntiry.DataStatus = (int)DataStatus.完结;
                                                purEntiry.PayStatus = (int)PayStatus.全部付款;
                                            }

                                            if (purEntiry.tb_purorder.tb_PurEntries != null)
                                            {
                                                //如果这个出库单的上级 订单 是我次出库的。他出库的状态都是全部付款了。则这个订单就全部付款了。
                                                //订单要保证全部出库了。才能这样算否则就先不管订单状态。只是部分付款
                                                if (
                                                    purEntiry.tb_purorder.TotalQty == purEntiry.tb_purorder.tb_PurEntries.Sum(c => c.TotalQty) &&
                                                    purEntiry.tb_purorder.tb_PurEntries.Where(c => c.PayStatus == (int)PayStatus.全部付款).ToList().Count ==
                                                    purEntiry.tb_purorder.tb_PurEntries.Count)
                                                {
                                                    purEntiry.tb_purorder.PayStatus = (int)PayStatus.全部付款;
                                                }
                                                else
                                                {
                                                    purEntiry.tb_purorder.PayStatus = (int)PayStatus.部分付款;
                                                }
                                            }
                                            purOrderUpdateList.Add(purEntiry.tb_purorder);
                                            purEntryUpdateList.Add(purEntiry);
                                        }

                                        #endregion
                                    }
                                }

                                if (receivablePayable.SourceBizType == (long)BizType.采购退货单)
                                {
                                    //厂商退款 时才处理
                                    //退货单审核后生成红字应收单（负金额）
                                    //没有记录支付状态，只标记结案处理
                                    if (receivablePayable.ARAPStatus == (long)ARAPStatus.已结清)
                                    {
                                        tb_SaleOutRe saleOutRe = await _appContext.Db.Queryable<tb_SaleOutRe>()
                                            .Where(c => c.DataStatus == (long)DataStatus.确认
                                         && c.SaleOutRe_ID == receivablePayable.SourceBillId).SingleAsync();
                                        if (saleOutRe != null)
                                        {
                                            saleOutRe.DataStatus = (int)DataStatus.完结;
                                        }
                                        //退款情况少，直接更新。
                                        await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutRe>(saleOutRe).ExecuteCommandAsync();
                                    }
                                }

                                if (receivablePayable.SourceBizType == (long)BizType.销售价格调整单 || receivablePayable.SourceBizType == (long)BizType.采购价格调整单)
                                {
                                    if (receivablePayable.ARAPStatus == (long)ARAPStatus.已结清)
                                    {
                                        #region 更新对应业务的单据状态和付款情况

                                        tb_FM_PriceAdjustment priceAdjustment = await _appContext.Db.Queryable<tb_FM_PriceAdjustment>()
                                            .Includes(c => c.tb_FM_PriceAdjustmentDetails, b => b.tb_fm_priceadjustment)
                                          .Where(c => c.DataStatus == (long)DataStatus.确认
                                         && c.AdjustId == receivablePayable.SourceBillId).SingleAsync();
                                        if (priceAdjustment != null)
                                        {
                                            //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                            if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == priceAdjustment.TotalLocalDiffAmount)
                                            {
                                                priceAdjustment.DataStatus = (int)DataStatus.完结;
                                                //价格调整单是不是也要加一个付款方式？区别账期？
                                                //priceAdjustment.PayStatus = (int)PayStatus.全部付款;
                                            }
                                            //退款情况少，直接更新。
                                            await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PriceAdjustment>(priceAdjustment).ExecuteCommandAsync();
                                        }

                                        #endregion
                                    }
                                }
                            }

                            receivablePayableUpdateList.AddRange(receivablePayableList);

                            #endregion

                            #region 生成核销记录
                            //收到，付了钱。审核就会生成一笔核销记录  收款抵扣应收
                            //这里是不是只记录应收应付来的？  预收付只是一个记录过程？
                            List<tb_FM_PaymentSettlement> paymentSettlements = await settlementController.GenerateSettlement(entity);
                            var settlementIds = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentSettlement>(paymentSettlements).ExecuteReturnSnowflakeIdListAsync();
                            #endregion
                            #region  对应的业务性单据 状态更新
                            //比方销售退货单 退款了就是结案。
                            //销售出库也对应结案。（出库对应订单结案）
                            //采购也一样逻辑
                            //价格调整单 成本 利润的影响
                            #endregion

                        }

                        //单纯收款不用产生核销记录。核销要与业务相关联 这里只处理 应收，预收，对账单
                        //退款时写回上级预收付款单 状态为 已冲销.预先处理，不用核销.只是一个收款记录
                        //负数时
                        //这里收款审核，面对预先处理。只是一个记录，并且回写预收生效，待核销。不用其它。
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
                                    prePayment.PrePaymentStatus = (long)PrePaymentStatus.待核销;
                                    prePayment.ForeignBalanceAmount += RecordDetail.ForeignAmount;
                                    prePayment.LocalBalanceAmount += RecordDetail.LocalAmount;

                                    //抵扣时 更新核销金额
                                    //prePayment.ForeignPaidAmount+=entity.ForeignPaidAmount;
                                    //prePayment.LocalPaidAmount += entity.LocalPaidAmount;

                                    //预收付的退款操作，对应收付款审核时。要找他对应的正向预收付单。修改状态。和退回金额。
                                    if (RecordDetail.LocalAmount < 0 || RecordDetail.ForeignAmount < 0)
                                    {
                                        //负数时，他一定有一个正数的收款单。并且对应一个对应的预收付单。，预收则要转为已冲销。自己则为
                                        //预收的退款操作时。 应该是去找他相同的

                                        #region 通过他的来源单据，找到对应的预收付单
                                        //应该只有一条。 会不会更新的就是自己 prePayment
                                        tb_FM_PreReceivedPayment oldPrePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                            .Where(c => c.PreRPID == RecordDetail.SourceBilllId && c.PrePaymentStatus == (long)PrePaymentStatus.待核销)
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
                                                oldPrePayment.PrePaymentStatus = (long)PrePaymentStatus.已冲销;
                                            }

                                            //有退，有核销 则是  部分核销
                                            if ((oldPrePayment.LocalPaidAmount > 0 && oldPrePayment.LocalRefundAmount > 0) ||
                                                (oldPrePayment.ForeignPaidAmount > 0 && oldPrePayment.ForeignRefundAmount > 0))
                                            {
                                                oldPrePayment.PrePaymentStatus = (long)PrePaymentStatus.部分核销;
                                            }

                                            //有退部分,还没有核销，后面可能退，也可能核销掉 则是  部分核销
                                            if ((oldPrePayment.LocalPaidAmount == 0 && oldPrePayment.LocalRefundAmount > 0 && oldPrePayment.LocalRefundAmount < oldPrePayment.LocalPrepaidAmount) ||
                                                (oldPrePayment.ForeignPaidAmount == 0 && oldPrePayment.ForeignRefundAmount > 0 && oldPrePayment.ForeignRefundAmount < oldPrePayment.ForeignPrepaidAmount))
                                            {
                                                oldPrePayment.PrePaymentStatus = (long)PrePaymentStatus.待核销;
                                            }
                                            //更新原来的上一个预付记录
                                            preReceivedPaymentUpdateList.Add(oldPrePayment);
                                        }
                                        #endregion

                                        #region 通过他的来源单据，找到对应的预收付单的收款单。标记为已冲销 !!!!!!!!!!

                                        //要调式
                                        tb_FM_PaymentRecord oldPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                        .Where(c => c.tb_FM_PaymentRecordDetails.Any(c => c.SourceBilllId == RecordDetail.SourceBilllId) && c.PaymentStatus == (long)PaymentStatus.已支付)
                                         .SingleAsync();
                                        if (oldPayment != null)
                                        {
                                            oldPayment.IsReversed = true;
                                            oldPayment.ReversedPaymentId = entity.PaymentId;
                                            oldPayment.ReversedPaymentNo = entity.PaymentNo;
                                            oldPayment.PaymentStatus = (long)PaymentStatus.已冲销;
                                            //更新原来的上一个预付记录
                                            await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(oldPayment).ExecuteCommandAsync();
                                        }

                                        //生成核销记录
                                        //退款或红冲时。审核就会生成一笔核销记录  收款抵扣应收
                                        await settlementController.GenerateSettlement(entity);

                                        #endregion
                                    }


                                }
                            }
                            //这里要测试哦！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！会不会更新的就是自己，
                            preReceivedPaymentUpdateList.AddRange(PreReceivablePayableList);

                        }
                    }
                    if (receivablePayableUpdateList.Count > 0)
                    {
                        var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(receivablePayableUpdateList).ExecuteCommandAsync();
                        if (r > 0)
                        {
                            //更新应收付状态和金额
                        }
                    }

                    if (preReceivedPaymentUpdateList.Count > 0)
                    {

                        //更新
                        var preRs = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(preReceivedPaymentUpdateList).UpdateColumns(it =>
                                    new
                                    {
                                        it.PrePaymentStatus,
                                        it.ForeignBalanceAmount,
                                        it.LocalBalanceAmount,
                                    }).ExecuteCommandAsync();
                    }

                }


                //等待真正支付
                entity.PaymentStatus = (long)PaymentStatus.已支付;

                //更新账户余额
                if (entity.tb_fm_account == null && entity.Account_id.HasValue)
                {
                    entity.tb_fm_account = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(c => c.Account_id == entity.Account_id).FirstAsync();
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
                BusinessHelper.Instance.ApproverEntity(entity);

                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(entity).ExecuteCommandAsync();
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




        // 生成收付款记录表
        /// <summary>
        /// 生成收付款记录表
        /// </summary>
        /// <param name="entity">预收付表</param>
        /// <param name="isRefund">true 如果是退款时 金额为负，SettlementType=退款红冲</param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentRecord> CreatePaymentRecord(tb_FM_PreReceivedPayment entity, bool isRefund)
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
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
            }
            else
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
            }
            tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
            #region 明细 

            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecordDetail.SourceBizType = (int)BizType.预收款单;
            }
            else
            {
                paymentRecordDetail.SourceBizType = (int)BizType.预付款单;
            }
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
            paymentRecord.PaymentStatus = (long)PaymentStatus.待审核;
            BusinessHelper.Instance.InitEntity(paymentRecord);
            long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(paymentRecord).ExecuteReturnSnowflakeIdAsync();
            if (id > 0)
            {
                paymentRecordDetail.PaymentId = id;
                await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecordDetail>(paymentRecordDetail).ExecuteReturnSnowflakeIdAsync();
                paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);
            }
            return paymentRecord;
        }

        // 生成收付款记录表
        public async Task<ReturnResults<tb_FM_PaymentRecord>> CreatePaymentRecord(List<tb_FM_ReceivablePayable> entities, bool SaveToDb = false, tb_FM_PaymentRecord OriginalPaymentRecord = null)
        {
            //通过应收 自动生成 收付款记录
            ReturnResults<tb_FM_PaymentRecord> rs = new ReturnResults<tb_FM_PaymentRecord>();
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
            if (entities[0].ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
            }
            else
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
            }

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
                    paymentRecord.ReversedPaymentId = OriginalPaymentRecord.PaymentId;
                    paymentRecord.ReversedPaymentNo = OriginalPaymentRecord.PaymentNo;
                }
            }

            //默认给第一个
            paymentRecord.PayeeInfoID = entities[0].PayeeInfoID;
            paymentRecord.CustomerVendor_ID = entities[0].CustomerVendor_ID;
            paymentRecord.PayeeAccountNo = entities[0].PayeeAccountNo;
            paymentRecord.tb_FM_PaymentRecordDetails = NewDetails;

            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
            var checkList = paymentRecord.tb_FM_PaymentRecordDetails.GroupBy(c => c.SourceBizType, c => c.SourceBilllId).ToList();
            if (checkList.Count > 1)
            {
                rs.ErrorMsg = ("收付款单明细中，同一业务下同一张单据不能重复分行收款。\r\n有相同业务下的单据必须为一行。");
                rs.ReturnObject = paymentRecord;
                return rs;
            }


            BusinessHelper.Instance.InitEntity(paymentRecord);
            //            paymentRecord.ReferenceNo=entity.no 流水
            if (SaveToDb)
            {
                //自动提交
                paymentRecord.PaymentStatus = (long)PaymentStatus.待审核;
                var ctr = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                //比方 暂时没有供应商  又是外键，则是如何处理的？
                bool vb = ShowInvalidMessage(ctr.Validator(paymentRecord));
                if (!vb)
                {
                    rs.ErrorMsg = "数据校验失败。";
                    rs.ReturnObject = paymentRecord;
                    return rs;
                }
                var paymentRecordController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                ReturnMainSubResults<tb_FM_PaymentRecord> rsms = await paymentRecordController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord);
                if (rsms.Succeeded)
                {
                    paymentRecord.tb_FM_PaymentRecordDetails.ForEach(c => c.PaymentId = rsms.ReturnObject.PaymentId);
                }
                else
                {
                    //记录错误日志
                    _logger.LogError(rsms.ErrorMsg);
                }
            }
            else
            {
                paymentRecord.PaymentStatus = (long)PaymentStatus.草稿;
            }
            rs.Succeeded = true;
            rs.ReturnObject = paymentRecord;
            return rs;
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
                        entity.PaymentStatus = (long)PaymentStatus.待审核;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        //只更新指定列
                        // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(entity).ExecuteCommandAsync();
                    }
                }
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();

                //_logger.Info(approvalEntity.bizName + "审核事务成功");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                _logger.Error(approvalEntity.bizName + "事务回滚");
                return false;
            }

        }
        /// <summary>
        /// 批量结案  销售订单标记结案，数据状态为8, 
        /// 如果还没有出库。但是结案的订单时。修正拟出库数量。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案 是仓库和业务确定这个订单不再执行的一个确认过程。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_FM_PaymentRecord> entitys = new List<tb_FM_PaymentRecord>();
            entitys = NeedCloseCaseList as List<tb_FM_PaymentRecord>;

            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 结案
                //更新拟销售量  减少
                for (int m = 0; m < entitys.Count; m++)
                {
                    //判断 能结案的 是关闭的意思。就是没有收到款 作废
                    // 检查预付款取消
                    var preStatus = PrePaymentStatus.已生效 | PrePaymentStatus.部分核销;
                    bool hasRelated = true; // 存在核销单, 有关联的单据的时候
                    bool canCancel = FMPaymentStatusHelper.CanCancel(preStatus, hasRelated); // 返回false



                    if (entitys[m].PaymentStatus == (long)PaymentStatus.已冲销 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }

                    entitys[m].PaymentStatus = (long)PaymentStatus.已取消;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient()
                        .Updateable<tb_FM_PaymentRecord>(entitys[m])
                        .UpdateColumns(it => new
                        {
                            it.PaymentStatus,
                            it.ApprovalStatus,
                            it.ApprovalResults,
                            it.ApprovalOpinions,
                            it.PaymentImagePath,
                            it.Modified_by,
                            it.Modified_at,
                            it.Remark
                        }).ExecuteCommandAsync();
                }
                #endregion
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }

        }

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PaymentRecord> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PaymentRecord>()
                .Where(m => m.PaymentId == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .ToListAsync();
            return list as List<T>;
        }




    }

}



