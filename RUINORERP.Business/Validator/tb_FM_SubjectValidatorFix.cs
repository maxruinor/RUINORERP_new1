
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:11
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
    /// 会计科目表，财务系统中使用验证类
    /// </summary>
    /*public partial class tb_FM_SubjectValidator:AbstractValidator<tb_FM_Subject>*/
    public partial class tb_FM_SubjectValidator : BaseValidatorGeneric<tb_FM_Subject>
    {
        public override void Initialize()
        {
            RuleFor(tb_FM_Subject => tb_FM_Subject.Subject_code).NotNull().WithMessage("科目代码:不能为空。");
        }
    }

}

