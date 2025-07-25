﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/25/2025 17:18:28
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
    /// 业务类型 报销，员工借支还款，运费验证类
    /// </summary>
    /*public partial class tb_FM_ExpenseTypeValidator:AbstractValidator<tb_FM_ExpenseType>*/
    public partial class tb_FM_ExpenseTypeValidator : BaseValidatorGeneric<tb_FM_ExpenseType>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.ReceivePaymentType).GreaterThan(0).WithMessage("收付类型:下拉选择值不正确。");
        }
    }

}

