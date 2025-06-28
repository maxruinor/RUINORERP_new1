
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/28/2024 17:10:40
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

    public partial class tb_sys_BillNoRuleValidator : BaseValidatorGeneric<tb_sys_BillNoRule>
    {

        public override void Initialize()
        {
            // 这里添加额外的初始化代码

            RuleFor(x => x.BizType).GreaterThan(-1).WithMessage("业务类型:必须选择");
            RuleFor(x => x.DateFormat).GreaterThan(-1).WithMessage("日期格式:必须选择");
            RuleFor(x => x.ResetMode).GreaterThan(-1).WithMessage("重置模式:必须选择");
            RuleFor(x => x.RuleName).MinimumLength(3).WithMessage("规则名称:输入长度要大于3个字符");
            RuleFor(x => x.Prefix).NotNull().WithMessage("前缀:不能为空");
            RuleFor(x => x.Prefix).MinimumLength(2).WithMessage("前缀:最小长度为2个字符");
            RuleFor(x => x.Prefix).MaximumLength(6).WithMessage("前缀:最大长度为6个字符");
            RuleFor(x => x.SequenceLength).GreaterThan(2).WithMessage("流水号长度:流水号长度必须在2~10之间");
            RuleFor(x => x.SequenceLength).LessThan(10).WithMessage("流水号长度:流水号长度必须在2~10之间");
        }
    }

}

