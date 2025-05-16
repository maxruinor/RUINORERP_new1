
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
using System.Linq;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{

    public partial class tb_SaleOutValidator : BaseValidatorGeneric<tb_SaleOut>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.TotalQty).GreaterThan(0).WithMessage("总数量：要大于零。");
            //如果成交小计不等于成交价*数量，则抛出异常
            //总金额包了运费要大于等于成交小计
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(x => x.tb_SaleOutDetails.Sum(c => (c.TransactionPrice ) * c.Quantity)).WithMessage("明细中，成交小计：要等于成交价*数量。");
            RuleFor(x => x.TotalCost).Equal(x => x.tb_SaleOutDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity)).WithMessage("明细中，成本小计：要等于成本加定制成本*数量。");

        }
    }
}

