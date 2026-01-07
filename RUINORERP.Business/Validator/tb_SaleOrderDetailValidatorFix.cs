
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 17:24:06
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
    /// 箱规表验证类
    /// </summary>
    public partial class tb_SaleOrderDetailValidator : BaseValidatorGeneric<tb_SaleOrderDetail>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码

            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("明细中，数量：要大于零。");
            RuleFor(x => x.Discount).GreaterThan(0).When(x => x.UnitPrice > 0 || x.TransactionPrice > 0).WithMessage("明细中，价格大于零时，折扣：要大于零。");

            RuleFor(x => x.UnitCommissionAmount).GreaterThanOrEqualTo(0).WithMessage("明细中，单品佣金：要大于零。");
            RuleFor(x => x.CommissionAmount).GreaterThanOrEqualTo(0).WithMessage("明细中，佣金小计：要大于零。");
            RuleFor(x => x.TaxRate).NotEqual(0).When(c => c.SubtotalTaxAmount != 0).WithMessage("明细中，税率：税额非零时不能为零。");
            RuleFor(x => x.SubtotalTaxAmount).NotEqual(0).When(c => c.TaxRate != 0).WithMessage("明细中，税额：税率非零时不能为零。");
            RuleFor(x => x.CommissionAmount).NotEqual(0).When(c => c.UnitCommissionAmount != 0).WithMessage("明细中，佣金小计：单品佣金非零时不能为零。");
            RuleFor(x => x.UnitCommissionAmount).NotEqual(0).When(c => c.CommissionAmount != 0).WithMessage("明细中，单品佣金：佣金小计非零时不能为零。");
            RuleFor(x => x.UnitPrice).GreaterThan(0).When(c => c.Gift == false).WithMessage("明细中，单价：非赠品时要大于零。");
            RuleFor(x => x.TransactionPrice).GreaterThan(0).When(c => c.Gift == false).WithMessage("明细中，成交价：非赠品时要大于零。");
            //RuleFor(x => x.Cost).GreaterThan(0).WithMessage("明细中，成本价：要大于零,请联系管理员。");
            //如果成交小计不等于成交价*数量，则抛出异常
            RuleFor(x => x.TransactionPrice * x.Quantity).Equal(x => x.SubtotalTransAmount).WithMessage("明细中，成交小计：要等于成交价*数量。");
            //RuleFor(x => x.Cost * x.Quantity).Equal(x => x.SubtotalCostAmount).WithMessage("明细中，成本小计：要等于成本价*数量,请联系管理员。");
        }
    }
}

