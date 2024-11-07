
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:23
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
    /// 采购订单明细表验证类
    /// </summary>
    /*public partial class tb_PurOrderDetailValidator:AbstractValidator<tb_PurOrderDetail>*/
    public partial class tb_PurOrderDetailValidator : BaseValidatorGeneric<tb_PurOrderDetail>
    {

        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("明细中，数量：要大于零。");
            RuleFor(x => x.Discount).GreaterThan(0).WithMessage("明细中，折扣：要大于零。");
            RuleFor(x => x.UnitPrice).GreaterThan(0).When(c => c.IsGift == false).WithMessage("明细中，单价：非赠品时要大于零。");
            RuleFor(x => x.TransactionPrice).GreaterThan(0).When(c => c.IsGift == false).WithMessage("明细中，单价：非赠品时要大于零。");
        }
    }


}



