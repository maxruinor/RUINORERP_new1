
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/28/2024 15:53:20
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
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度验证类
    /// </summary>
    /*public partial class tb_gl_CommentValidator:AbstractValidator<tb_gl_Comment>*/
    public partial class tb_gl_CommentValidator : BaseValidatorGeneric<tb_gl_Comment>
    {
        public override void Initialize()
        {
            RuleFor(x => x.CommentContent).MinimumLength(2).WithMessage("批注内容:输入长度要大于1。");
        }
    }
}

