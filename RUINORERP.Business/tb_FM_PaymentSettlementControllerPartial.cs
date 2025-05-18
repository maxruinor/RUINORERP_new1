
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 核销记录
    /// </summary>
    public partial class tb_FM_PaymentSettlementController<T> : BaseController<T> where T : class
    {


        /// <summary>
        /// 退款或红冲时，生成反向核销记录（IsReversed=1），并关联原记录。
        /// 正向不用。反向用？
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PaymentSettlement>> GenerateSettlement(tb_FM_PaymentRecord entity)
        {
            List<tb_FM_PaymentSettlement> SettlementRecords = new();
            //预收付款单 审核时 自动生成 收付款记录

            if (entity.tb_FM_PaymentRecordDetails != null)
            {
                foreach (var item in entity.tb_FM_PaymentRecordDetails)
                {
                    #region  生成核销记录

                    tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
                    SettlementRecord = mapper.Map<tb_FM_PaymentSettlement>(entity);
                    SettlementRecord.ActionStatus = ActionStatus.新增;
                    SettlementRecord.IsAutoSettlement = true;
                    SettlementRecord.ReceivePaymentType = entity.ReceivePaymentType;
                    if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                    {
                        SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款核销款);
                        SettlementRecord.SettlementType = (int)SettlementType.收款核销;
                        if (entity.TotalForeignAmount < 0 || entity.TotalLocalAmount < 0)
                        {
                            SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
                            SettlementRecord.IsReversed = true;
                        }
                        SettlementRecord.TargetBizType = (int)BizType.收款单;
                    }
                    else
                    {
                        SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款核销款);
                        SettlementRecord.SettlementType = (int)SettlementType.付款核销;
                        if (entity.TotalForeignAmount < 0 || entity.TotalLocalAmount < 0)
                        {
                            SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
                        }
                        SettlementRecord.TargetBizType = (int)BizType.付款单;
                    }

                    //按明细生成具体的核销记录
                    SettlementRecord.SourceBizType = item.SourceBizType;
                    SettlementRecord.SourceBillNo = item.SourceBillNo;
                    SettlementRecord.SourceBillId = item.SourceBilllId;

                    SettlementRecord.TargetBillId = entity.PaymentId;
                    SettlementRecord.TargetBillNo = entity.PaymentNo;

                    //SourceBillDetailID 应收时 可以按明细核销？
                    SettlementRecord.SettleDate = System.DateTime.Now;
                    if (SettlementRecord.SettledLocalAmount < 0)
                    {
                        SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
                    }

                    SettlementRecord.Currency_ID = entity.Currency_ID;

                    SettlementRecord.SettledForeignAmount = entity.TotalForeignAmount;
                    SettlementRecord.SettledLocalAmount = entity.TotalLocalAmount;
                    BusinessHelper.Instance.InitEntity(SettlementRecord);
                    #endregion
                    SettlementRecords.Add(SettlementRecord);
                }
            }

            List<long> ids = await _unitOfWorkManage.GetDbClient().Insertable(SettlementRecords).ExecuteReturnSnowflakeIdListAsync();
            if (ids.Count > 0)
            {

            }
            return SettlementRecords;
        }

        /// <summary>
        /// 退款或红冲时，生成反向核销记录（IsReversed=1），并关联原记录。
        /// 正向不用。反向用？
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentSettlement> GenerateSettlement(tb_FM_ReceivablePayable sourceEntity, tb_FM_ReceivablePayable targetEntity)
        {
            //预收付款单 审核时 自动生成 收付款记录
            tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
            SettlementRecord = mapper.Map<tb_FM_PaymentSettlement>(targetEntity);

            SettlementRecord.ActionStatus = ActionStatus.新增;
            SettlementRecord.IsAutoSettlement = true;
            SettlementRecord.ReceivePaymentType = targetEntity.ReceivePaymentType;
            if (targetEntity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款核销款);
                SettlementRecord.SettlementType = (int)SettlementType.收款核销;
                if (targetEntity.ForeignPaidAmount < 0 || targetEntity.ForeignPaidAmount < 0)
                {
                    SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
                    SettlementRecord.IsReversed = true;
                }
                SettlementRecord.TargetBizType = (int)BizType.应收款单;
                SettlementRecord.SourceBizType = (int)BizType.应收款单;
            }
            else
            {
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款核销款);
                SettlementRecord.SettlementType = (int)SettlementType.付款核销;
                if (targetEntity.ForeignPaidAmount < 0 || targetEntity.ForeignPaidAmount < 0)
                {
                    SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
                }
                SettlementRecord.TargetBizType = (int)BizType.应付款单;
                SettlementRecord.SourceBizType = (int)BizType.应付款单;
            }


            SettlementRecord.SourceBillNo = sourceEntity.ARAPNo;
            SettlementRecord.SourceBillId = sourceEntity.ARAPId;


            SettlementRecord.TargetBillId = targetEntity.ARAPId;
            SettlementRecord.TargetBillNo = targetEntity.ARAPNo;

            //SourceBillDetailID 应收时 可以按明细核销？
            SettlementRecord.SettleDate = System.DateTime.Now;
            if (SettlementRecord.SettledLocalAmount < 0)
            {
                SettlementRecord.SettlementType = (int)SettlementType.红冲核销;
            }

            SettlementRecord.Currency_ID = targetEntity.Currency_ID;

            SettlementRecord.ExchangeRate = targetEntity.ExchangeRate;
            if (true)
            {

            }

            SettlementRecord.SettledForeignAmount = Math.Abs(targetEntity.ForeignPaidAmount);
            SettlementRecord.SettledLocalAmount = Math.Abs(targetEntity.LocalPaidAmount);

            BusinessHelper.Instance.InitEntity(SettlementRecord);
            long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentSettlement>(SettlementRecord).ExecuteReturnSnowflakeIdAsync();
            if (id > 0)
            {

            }

            return SettlementRecord;
        }
    }

}



