﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:29
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
    /// 产品借出单验证类
    /// </summary>
    /*public partial class tb_ProdBorrowingValidator:AbstractValidator<tb_ProdBorrowing>*/
    public partial class tb_ProdBorrowingValidator : BaseValidatorGeneric<tb_ProdBorrowing>
    {
        public override void Initialize()
        {
            RuleFor(x => x.Reason).NotEmpty().WithMessage("借出原因:不能为空.");
            RuleFor(x => x.Reason).MinimumLength(3).WithMessage("借出原因:最小长度为3.");

            

        }

    }

}
