
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/23/2025 14:28:33
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 价格调整单明细验证类
    /// </summary>
    /*public partial class tb_FM_PriceAdjustmentDetailValidator:AbstractValidator<tb_FM_PriceAdjustmentDetail>*/
    public partial class tb_FM_PriceAdjustmentDetailValidator : BaseValidatorGeneric<tb_FM_PriceAdjustmentDetail>
    {
        public override void Initialize()
        {
            // 验证总差异金额不能为0
            RuleFor(x => x.TotalAmount_Diff).NotEqual(0).WithMessage("总差异金额:不能为0。");

            // 验证原价和现价必须有变化
            RuleFor(x => x)
                .Must(HavePriceChange)
                .WithMessage("明细中，如果原价和现价没有任何变化，则不能添加到调价明细中。请在行头右键删除。");
        }

        /// <summary>
        /// 验证原价和现价是否有变化
        /// </summary>
        /// <param name="detail">价格调整明细</param>
        /// <returns>有变化返回true，否则返回false</returns>
        private bool HavePriceChange(tb_FM_PriceAdjustmentDetail detail)
        {
            // 检查未税单价是否有变化
            if (detail.Original_UnitPrice_NoTax != detail.Correct_UnitPrice_NoTax)
            {
                return true;
            }

            // 检查含税单价是否有变化
            if (detail.Original_UnitPrice_WithTax != detail.Correct_UnitPrice_WithTax)
            {
                return true;
            }

            // 检查税率是否有变化
            if (detail.Original_TaxRate != detail.Correct_TaxRate)
            {
                return true;
            }

            // 检查税额是否有变化
            if (detail.Original_TaxAmount != detail.Correct_TaxAmount)
            {
                return true;
            }

            // 如果所有价格相关的字段都没有变化，则不允许保存
            return false;
        }
    }
}

