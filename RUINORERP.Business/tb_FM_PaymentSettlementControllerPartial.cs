
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
using FluentValidation;
using System.ComponentModel;
using System.Threading;

namespace RUINORERP.Business
{
    /// <summary>
    /// 核销记录
    /// </summary>
    public partial class tb_FM_PaymentSettlementController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 坏账核销记录
        /// 应收付坏账标记时调用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentSettlement> GenerateSettlement(tb_FM_ReceivablePayable receivablePayable, decimal SettlementMoney, decimal foreignSettlementMoney)
        {
            //预收付款单 审核时 自动生成 收付款记录
            #region  生成核销记录

            tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
            SettlementRecord.ActionStatus = ActionStatus.新增;
            SettlementRecord.IsAutoSettlement = true;
            SettlementRecord.ReceivePaymentType = receivablePayable.ReceivePaymentType;
            SettlementRecord.Account_id = receivablePayable.Account_id;
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                SettlementRecord.TargetBizType = (int)BizType.应收款单;
                // 确保异步调用正确执行
                SettlementRecord.SettlementNo = await Task.Run(async () => 
                    await bizCodeService.GenerateBizBillNoAsync(BizType.收款核销, CancellationToken.None));
            }
            else
            {
                SettlementRecord.SourceBizType = (int)BizType.应付款单;
                // 确保异步调用正确执行
                SettlementRecord.SettlementNo = await Task.Run(async () => 
                    await bizCodeService.GenerateBizBillNoAsync(BizType.付款核销, CancellationToken.None));
            }
            SettlementRecord.SettlementType = (int)SettlementType.坏账核销;
            SettlementRecord.Currency_ID = receivablePayable.Currency_ID;
            SettlementRecord.ExchangeRate = receivablePayable.ExchangeRate;
            SettlementRecord.CustomerVendor_ID = receivablePayable.CustomerVendor_ID;
            SettlementRecord.SettledForeignAmount = foreignSettlementMoney;
            SettlementRecord.SettledLocalAmount = SettlementMoney;
            SettlementRecord.IsAutoSettlement = true;

            //if (SettlementRecord.SettledLocalAmount < 0 || SettlementRecord.SettledForeignAmount < 0)
            //{
            //    SettlementRecord.SettlementType = (int)SettlementType.红字核销;
            //    SettlementRecord.IsReversed = true;
            //}

            SettlementRecord.TargetBillId = receivablePayable.ARAPId;
            SettlementRecord.TargetBillNo = receivablePayable.ARAPNo;
            SettlementRecord.SettleDate = System.DateTime.Now;

            BusinessHelper.Instance.InitEntity(SettlementRecord);
            #endregion

            List<long> ids = await _unitOfWorkManage.GetDbClient().Insertable(SettlementRecord).ExecuteReturnSnowflakeIdListAsync();
            if (ids.Count > 0)
            {

            }
            return SettlementRecord;
        }




        /// <summary>
        /// 预收款抵扣应收款（正向核销）
        /// 应收付审核时调用
        /// 应收付反审时 用负数，
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentSettlement> GenerateSettlement(tb_FM_PreReceivedPayment preReceivedPayment,
            tb_FM_ReceivablePayable receivablePayable, decimal SettlementMoney, decimal foreignSettlementMoney, tb_FM_PaymentSettlement ReversedPaymentSettlement = null)
        {


            //预收付款单 审核时 自动生成 收付款记录
            #region  生成核销记录

            tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
            SettlementRecord.ActionStatus = ActionStatus.新增;
            SettlementRecord.IsAutoSettlement = true;
            SettlementRecord.ReceivePaymentType = receivablePayable.ReceivePaymentType;
            SettlementRecord.Account_id = receivablePayable.Account_id;
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                SettlementRecord.SourceBizType = (int)BizType.预收款单;
                SettlementRecord.TargetBizType = (int)BizType.应收款单;
                SettlementRecord.SettlementNo = await bizCodeService.GenerateBizBillNoAsync(BizType.收款核销, CancellationToken.None);
                SettlementRecord.SettlementType = (int)SettlementType.预收冲应收;

            }
            else
            {
                SettlementRecord.SourceBizType = (int)BizType.预付款单;
                SettlementRecord.TargetBizType = (int)BizType.应付款单;
                SettlementRecord.SettlementNo = await bizCodeService.GenerateBizBillNoAsync(BizType.付款核销, CancellationToken.None);
                SettlementRecord.SettlementType = (int)SettlementType.预付冲应付;
            }

            SettlementRecord.Currency_ID = preReceivedPayment.Currency_ID;
            SettlementRecord.ExchangeRate = receivablePayable.ExchangeRate;
            SettlementRecord.CustomerVendor_ID = receivablePayable.CustomerVendor_ID;
            SettlementRecord.SettledForeignAmount = foreignSettlementMoney;
            SettlementRecord.SettledLocalAmount = SettlementMoney;
            SettlementRecord.IsAutoSettlement = true;

            if (SettlementRecord.SettledLocalAmount < 0 || SettlementRecord.SettledForeignAmount < 0)
            {
                SettlementRecord.SettlementType = (int)SettlementType.红字核销;
                SettlementRecord.IsReversed = true;
                if (ReversedPaymentSettlement != null)
                {
                    SettlementRecord.ReversedSettlementID = ReversedPaymentSettlement.SettlementId;
                    ReversedPaymentSettlement.IsReversed = true;
                    await _unitOfWorkManage.GetDbClient().Updateable(ReversedPaymentSettlement).UpdateColumns(it => new { it.IsReversed }).ExecuteCommandAsync();
                }
            }

            SettlementRecord.SourceBillNo = preReceivedPayment.PreRPNO;
            SettlementRecord.SourceBillId = preReceivedPayment.PreRPID;

            SettlementRecord.TargetBillId = receivablePayable.ARAPId;
            SettlementRecord.TargetBillNo = receivablePayable.ARAPNo;
            SettlementRecord.SettleDate = System.DateTime.Now;

            BusinessHelper.Instance.InitEntity(SettlementRecord);
            #endregion

            List<long> ids = await _unitOfWorkManage.GetDbClient().Insertable(SettlementRecord).ExecuteReturnSnowflakeIdListAsync();
            if (ids.Count > 0)
            {

            }
            return SettlementRecord;
        }

        /// <summary>
        /// 应收单转收款（正向核销）
        /// 客户支付账期订单款项，用收款单核销应收单。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentSettlement> GenerateSettlement(tb_FM_PaymentRecord PaymentRecord, tb_FM_PaymentRecordDetail PaymentRecordDetail, tb_FM_ReceivablePayable receivablePayable, decimal amountToWriteOff)
        {

            //预收付款单 审核时 自动生成 收付款记录
            tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
            SettlementRecord = mapper.Map<tb_FM_PaymentSettlement>(PaymentRecordDetail);

            if (PaymentRecord.CustomerVendor_ID.HasValue)
            {
                SettlementRecord.CustomerVendor_ID = PaymentRecord.CustomerVendor_ID.Value;
            }

            SettlementRecord.ActionStatus = ActionStatus.新增;
            SettlementRecord.IsAutoSettlement = true;
            SettlementRecord.ReceivePaymentType = PaymentRecord.ReceivePaymentType;
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            if (SettlementRecord.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                SettlementRecord.SettlementNo = await bizCodeService.GenerateBizBillNoAsync(BizType.收款核销, CancellationToken.None);
                SettlementRecord.SettlementType = (int)SettlementType.收款核销;
                SettlementRecord.TargetBizType = (int)BizType.收款单;
                SettlementRecord.SourceBizType = (int)BizType.应收款单;
            }
            else
            {
                SettlementRecord.SettlementNo = await bizCodeService.GenerateBizBillNoAsync(BizType.付款核销, CancellationToken.None);
                SettlementRecord.SettlementType = (int)SettlementType.付款核销;
                SettlementRecord.TargetBizType = (int)BizType.付款单;
                SettlementRecord.SourceBizType = (int)BizType.应付款单;
            }

            SettlementRecord.Account_id = PaymentRecord.Account_id;
            if (amountToWriteOff < 0)
            {
                SettlementRecord.SettlementType = (int)SettlementType.红字核销;
            }
            //成熟系统的设计目标是保证财务数据的清晰性、一致性和可审计性。核销记录本身不应该使用负数来区分方向。
            SettlementRecord.SettledLocalAmount = Math.Abs(amountToWriteOff);  // 使用绝对值确保金额为正
            SettlementRecord.SettledForeignAmount = 0; //暂时不支付外币
            //应收单转收款
            SettlementRecord.SourceBillNo = receivablePayable.ARAPNo;
            SettlementRecord.SourceBillId = receivablePayable.ARAPId;

            SettlementRecord.TargetBillId = PaymentRecordDetail.PaymentId;
            SettlementRecord.TargetBillNo = PaymentRecord.PaymentNo;

            SettlementRecord.SettleDate = System.DateTime.Now;

            SettlementRecord.Currency_ID = receivablePayable.Currency_ID;

            SettlementRecord.ExchangeRate = receivablePayable.ExchangeRate;
            if (true)
            {

            }

            BusinessHelper.Instance.InitEntity(SettlementRecord);
            long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentSettlement>(SettlementRecord).ExecuteReturnSnowflakeIdAsync();
            if (id > 0)
            {

            }

            return SettlementRecord;


        }




    }

}



