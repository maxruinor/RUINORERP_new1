
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
    /// 系统使用者公司验证类
    /// </summary>
    /*public partial class tb_CompanyValidator:AbstractValidator<tb_Company>*/
    public partial class tb_CompanyValidator : BaseValidatorGeneric<tb_Company>
    {
        public override void Initialize()
        {
            //RuleFor(x => x.CNName).NotNull().WithMessage("简称不能为空。");
            //RuleFor(x => x.CNName).MinimumLength(2).WithMessage("简称:长度要大于1。");

            RuleFor(x => x.ShortName).NotNull().WithMessage("简称不能为空。");
            RuleFor(x => x.ShortName).MinimumLength(2).WithMessage("简称:长度要大于1。");
        }
    }

}

