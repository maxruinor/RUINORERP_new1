
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 16:10:18
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
    /// 付款申请单-目前代替纸的申请单将来完善明细则用付款单的主子表来完成系统可以根据客户来自动生成经人确认验证类
    /// </summary>
    /*public partial class tb_FM_PaymentApplicationValidator:AbstractValidator<tb_FM_PaymentApplication>*/
    public partial class tb_FM_PaymentApplicationValidator : BaseValidatorGeneric<tb_FM_PaymentApplication>
    {
        public override void Initialize()
        {
            RuleFor(x => x.PayReasonItems).MinimumLength(3).WithMessage("付款项目/原因:长度不能小于3。");
        }
    }

}

