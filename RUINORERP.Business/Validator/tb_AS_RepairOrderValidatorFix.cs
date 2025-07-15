
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:43:14
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Global;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 维修工单  工时费 材料费验证类
    /// </summary>
    /*public partial class tb_AS_RepairOrderValidator:AbstractValidator<tb_AS_RepairOrder>*/
    public partial class tb_AS_RepairOrderValidator : BaseValidatorGeneric<tb_AS_RepairOrder>
    {
        public override void Initialize()
        {
            //RuleFor(x => x.TotalMaterialAmount).GreaterThan(0).When(x => x.tb_AS_RepairOrderMaterialDetails.Any(s => s.Gift == false)).WithMessage("总金额：明细中有非赠品产品时，总金额要大于零。");
            RuleFor(x => x.TotalQty).GreaterThan(0).WithMessage("总数量：要大于零。");
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0).WithMessage("总金额：要大于零。");
            // RuleFor(x => x.TotalAmount).Equal(x => x.tb_AS_RepairOrderMaterialDetails.Sum(c => (c.TransactionPrice) * c.Quantity) + x.FreightIncome).WithMessage("总金额，成交小计：要等于成交价*数量，包含运费。");
            //RuleFor(x => x.TotalCost).Equal(x => x.tb_AS_RepairOrderMaterialDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity)).WithMessage("总金额，成本小计：要等于（成本价+定制成本）*数量。");
            RuleFor(x => x.PayStatus).GreaterThan(0).WithMessage("付款状态:不能为空。");
            RuleFor(x => x.Paytype_ID).GreaterThan(0).When
(c => c.PayStatus != (int)PayStatus.未付款).WithMessage("付款方式:有付款的情况下，付款方式不能为空。");
        }
    }

}

