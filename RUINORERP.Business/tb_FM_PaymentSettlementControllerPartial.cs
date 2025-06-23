
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
using FluentValidation;
using System.ComponentModel;

namespace RUINORERP.Business
{
    /// <summary>
    /// 核销记录
    /// </summary>
    public partial class tb_FM_PaymentSettlementController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 预收款抵扣应收款（正向核销）
        /// 应收付审核时调用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentSettlement> GenerateSettlement(tb_FM_PreReceivedPayment preReceivedPayment,
            tb_FM_ReceivablePayable receivablePayable, decimal SettlementMoney, decimal foreignSettlementMoney)
        {


            //预收付款单 审核时 自动生成 收付款记录
            #region  生成核销记录

            tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
            SettlementRecord.ActionStatus = ActionStatus.新增;
            SettlementRecord.IsAutoSettlement = true;
            SettlementRecord.ReceivePaymentType = receivablePayable.ReceivePaymentType;
            SettlementRecord.Account_id = receivablePayable.Account_id;
            if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                SettlementRecord.SourceBizType = (int)BizType.预收款单;
                SettlementRecord.TargetBizType = (int)BizType.应收款单;
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款核销);
                SettlementRecord.SettlementType = (int)SettlementType.预收冲应收;

            }
            else
            {
                SettlementRecord.SourceBizType = (int)BizType.预付款单;
                SettlementRecord.SourceBizType = (int)BizType.应付款单;
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款核销);
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
                SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
                SettlementRecord.IsReversed = true;
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
        public async Task<tb_FM_PaymentSettlement> GenerateSettlement(tb_FM_PaymentRecord PaymentRecord, tb_FM_PaymentRecordDetail PaymentRecordDetail, tb_FM_ReceivablePayable receivablePayable)
        {
            //预收付款单 审核时 自动生成 收付款记录
            tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
            SettlementRecord = mapper.Map<tb_FM_PaymentSettlement>(PaymentRecordDetail);

            SettlementRecord.ActionStatus = ActionStatus.新增;
            SettlementRecord.IsAutoSettlement = true;
            SettlementRecord.ReceivePaymentType = PaymentRecord.ReceivePaymentType;
            if (SettlementRecord.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款核销);
                SettlementRecord.SettlementType = (int)SettlementType.收款核销;
                SettlementRecord.TargetBizType = (int)BizType.收款单;
                SettlementRecord.SourceBizType = (int)BizType.应收款单;
            }
            else
            {
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款核销);
                SettlementRecord.SettlementType = (int)SettlementType.付款核销;
                SettlementRecord.TargetBizType = (int)BizType.付款单;
                SettlementRecord.SourceBizType = (int)BizType.应付款单;
            }

            SettlementRecord.SettledLocalAmount = PaymentRecordDetail.LocalAmount;
            SettlementRecord.SettledForeignAmount = PaymentRecordDetail.ForeignAmount; ;
            SettlementRecord.Account_id = PaymentRecord.Account_id;
            if (SettlementRecord.SettledLocalAmount < 0 || SettlementRecord.SettledForeignAmount < 0)
            {
                SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
            }

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

            SettlementRecord.SettledForeignAmount = Math.Abs(receivablePayable.ForeignPaidAmount);
            SettlementRecord.SettledLocalAmount = Math.Abs(receivablePayable.LocalPaidAmount);

            BusinessHelper.Instance.InitEntity(SettlementRecord);
            long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentSettlement>(SettlementRecord).ExecuteReturnSnowflakeIdAsync();
            if (id > 0)
            {

            }

            return SettlementRecord;


        }




    }

}



