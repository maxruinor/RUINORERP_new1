
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/04/2025 18:55:01
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
    /// 提醒规则验证类
    /// </summary>
    /*public partial class tb_ReminderRuleValidator:AbstractValidator<tb_ReminderRule>*/
    public partial class tb_ReminderRuleValidator : BaseValidatorGeneric<tb_ReminderRule>
    {

        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.CheckIntervalByMinutes).GreaterThan(5).WithMessage("检测频率(分钟)：不能小于五分钟。");
            RuleFor(x => x.CheckIntervalByMinutes).LessThan(6000).WithMessage("检测频率(分钟)：不能大于六千分钟（100个小时）。");
        }
    }

}

