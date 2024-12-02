
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2024 15:56:40
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 采购入库退回单验证类
    /// </summary>
    /*public partial class tb_PurEntryReDetailValidator:AbstractValidator<tb_PurEntryReDetail>*/
    public partial class tb_PurEntryReDetailValidator : BaseValidatorGeneric<tb_PurEntryReDetail>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("明细中，数量：要大于零。");
            RuleFor(x => x.Discount).GreaterThan(0).WithMessage("明细中，折扣：要大于零。");
            RuleFor(x => x.UnitPrice).GreaterThan(0).When(c => c.IsGift == false).WithMessage("明细中，单价：非赠品时要大于零。");
            RuleFor(x => x.TransactionPrice).GreaterThan(0).When(c => c.IsGift == false).WithMessage("明细中，单价：非赠品时要大于零。");
            RuleFor(x => x.TransactionPrice).GreaterThan(0).When(c => c.tb_purentryre != null && c.tb_purentryre.PurEntryID.HasValue == false)
                .WithMessage("在没有引用入库单中明细数据时，单价：非赠品时要大于零。");
            //如果成交小计不等于成交价*数量，则抛出异常
            RuleFor(x => x.TransactionPrice * x.Quantity).Equal(x => x.SubtotalTrPriceAmount).WithMessage("明细中，成交小计：要等于成交价*数量。");
        }
    }

}

