
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
            RuleFor(x => x.TransactionPrice).GreaterThan(0).When(c => c.Gift == false).WithMessage("明细中，成交价：非赠品时要大于零。");
        }
    }
}

