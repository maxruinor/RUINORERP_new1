
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 21:09:57
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 跟进计划表验证类
    /// </summary>
    /*public partial class tb_CRM_FollowUpPlansValidator:AbstractValidator<tb_CRM_FollowUpPlans>*/
    public partial class tb_CRM_FollowUpPlansValidator : BaseValidatorGeneric<tb_CRM_FollowUpPlans>
    {
        public override void Initialize()
        {
            RuleFor(x => x.Customer_id).NotNull().WithMessage("目标客户不能为空。");

            RuleFor(x => x.PlanStartDate.Date).GreaterThan(System.DateTime.Now.Date).WithMessage("开始日期要大于当前日期。");
            RuleFor(x => x.PlanEndDate).GreaterThan(t => t.PlanStartDate).WithMessage("结束日期要大于开始日期。");
            RuleFor(x => x.PlanContent).MinimumLength(10).WithMessage("计划内容:长度要大于10。");

        }

    }

}

