﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/10/2024 14:15:54
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
    /// 产品转换单明细验证类
    /// </summary>
    /*public partial class tb_ProdConversionDetailValidator:AbstractValidator<tb_ProdConversionDetail>*/
    public partial class tb_ProdConversionDetailValidator : BaseValidatorGeneric<tb_ProdConversionDetail>
    {
        public override void Initialize()
        {
            RuleFor(x => x.ConversionQty).GreaterThan(0).WithMessage("转换数量:值要大于零。");
        }
    }

}
