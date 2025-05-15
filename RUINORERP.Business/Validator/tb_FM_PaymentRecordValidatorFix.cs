
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 16:10:18
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using System.Linq;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    public partial class tb_FM_PaymentRecordValidator : BaseValidatorGeneric<tb_FM_PaymentRecord>
    {
       public override void Initialize()
       {
            // 验证明细列表中的币别是否一致，并且与主表币别相同
            RuleFor(x => x)
                .Must(HaveConsistentCurrency)
                .WithMessage("单据明细中的币别必须一致，且与主表币别相同");
        }

        private bool HaveConsistentCurrency(tb_FM_PaymentRecord record)
        {
            // 如果明细列表为空，默认通过验证（可根据业务需求调整）
            if (record.tb_FM_PaymentRecordDetails == null || !record.tb_FM_PaymentRecordDetails.Any())
            {
                return true;
            }

            // 获取明细中的第一个币别值
            var firstCurrency = record.tb_FM_PaymentRecordDetails.First().Currency_ID;

            // 检查所有明细的币别是否与第一个相同
            bool allDetailsSameCurrency = record.tb_FM_PaymentRecordDetails.All(d => d.Currency_ID == firstCurrency);

            // 检查主表币别是否与明细币别相同
            bool masterCurrencyMatches = record.Currency_ID == firstCurrency;

            return allDetailsSameCurrency && masterCurrencyMatches;
        }
    }

}

