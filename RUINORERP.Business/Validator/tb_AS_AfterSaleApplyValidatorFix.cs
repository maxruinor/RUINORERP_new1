
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:34
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
    /// 售后申请单验证类
    /// </summary>
    /*public partial class tb_AS_AfterSaleApplyValidator:AbstractValidator<tb_AS_AfterSaleApply>*/
    public partial class tb_AS_AfterSaleApplyValidator : BaseValidatorGeneric<tb_AS_AfterSaleApply>
    {

        public override void Initialize()
        {
            RuleFor(tb_AS_AfterSaleApply => tb_AS_AfterSaleApply.Location_ID).NotNull().Must(CheckForeignKeyValue).WithMessage("售后暂存仓:下拉选择值不正确。");
            //***** 
            RuleFor(tb_AS_AfterSaleApply => tb_AS_AfterSaleApply.ExpenseAllocationMode).NotNull().WithMessage("费用承担模式:不能为空。");
            RuleFor(tb_AS_AfterSaleApply => tb_AS_AfterSaleApply.ExpenseBearerType).NotNull().WithMessage("费用承担方:不能为空。");
        }

    }
}

