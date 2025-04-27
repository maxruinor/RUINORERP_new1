
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

        //生成核销记录
        public async Task<tb_FM_PaymentSettlement> GenerateSettlement(tb_FM_PaymentRecord entity)
        {
            //预收付款单 审核时 自动生成 收付款记录
            IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
            tb_FM_PaymentSettlement SettlementRecord = new tb_FM_PaymentSettlement();
            SettlementRecord = mapper.Map<tb_FM_PaymentSettlement>(entity);

            SettlementRecord.ActionStatus = ActionStatus.新增;
         
            SettlementRecord.ReceivePaymentType = entity.ReceivePaymentType;
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款核销);
                SettlementRecord.SettlementType = (int)SettlementType.收款核销;
               
            }
            else
            {
                SettlementRecord.SettlementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款核销);
                SettlementRecord.SettlementType = (int)SettlementType.付款核销;
            }

            SettlementRecord.BizType = entity.BizType;
            SettlementRecord.SourceBillNO = entity.SourceBillNO;
            SettlementRecord.SourceBillID = entity.SourceBilllID;
            //SourceBillDetailID 应收时 可以按明细核销？
            SettlementRecord.SettleDate = System.DateTime.Now;
            if (SettlementRecord.SettledLocalAmount < 0)
            {
                SettlementRecord.SettlementType = (int)SettlementType.退款红冲;
            }
            switch (entity.BizType)
            {
                case (int)BizType.收款核销:
                    SettlementRecord.SettlementType = (int)SettlementType.收款核销;
                    break;
                case (int)BizType.付款核销:
                    SettlementRecord.SettlementType = (int)SettlementType.付款核销;
                    break;
                default:
                    break;
            }
           if( entity.Currency_ID.HasValue)
            {
                SettlementRecord.SourceCurrencyID = entity.Currency_ID.Value;
            }
            SettlementRecord.SourceExchangeRate = entity.ExchangeRate;
            if (true)
            {

            }
            SettlementRecord.SourceBizType= entity.BizType;


            SettlementRecord.BizType = entity.BizType;
            SettlementRecord.SettledForeignAmount = entity.ForeignPaidAmount;
            SettlementRecord.SettledLocalAmount = entity.LocalPaidAmount;
          

            BusinessHelper.Instance.InitEntity(SettlementRecord);
            long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(SettlementRecord).ExecuteReturnSnowflakeIdAsync();
            if (id > 0)
            {

            }

            return SettlementRecord;
        }



    }

}



