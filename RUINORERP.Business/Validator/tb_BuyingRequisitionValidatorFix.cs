
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:25
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
    /// 请购单，可能来自销售订单,也可以来自其它日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表

    /// </summary>
    /*public partial class tb_BuyingRequisitionValidator:AbstractValidator<tb_BuyingRequisition>*/
    public partial class tb_BuyingRequisitionValidator : BaseValidatorGeneric<tb_BuyingRequisition>
    {

        public override void Initialize()
        {
            RuleFor(x => x.Purpose).NotNull().WithMessage("用途:不能为空。");
            RuleFor(x => x.Purpose).MinimumLength(3).WithMessage("用途:长度要大于3。");
        }

    }

}

