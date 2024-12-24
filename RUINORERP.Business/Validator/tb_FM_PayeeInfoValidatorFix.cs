
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:27
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
    /// 收款信息，供应商报销人的收款账号验证类
    /// </summary>
    /*public partial class tb_FM_PayeeInfoValidator:AbstractValidator<tb_FM_PayeeInfo>*/
    public partial class tb_FM_PayeeInfoValidator : BaseValidatorGeneric<tb_FM_PayeeInfo>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            // 如果选择了外发，则必须填写外发加工商
            RuleFor(x => x.Account_name).NotNull().WithMessage("账户名称:不能为空。");
            RuleFor(x => x.Account_name).MinimumLength(2).WithMessage("账户名称:不能小于长度2。");
        }


    }

}

