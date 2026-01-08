
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 21:24:00
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
    /// 跟进记录表验证类
    /// </summary>
    /*public partial class tb_CRM_FollowUpRecordsValidator:AbstractValidator<tb_CRM_FollowUpRecords>*/
    public partial class tb_CRM_FollowUpRecordsValidator : BaseValidatorGeneric<tb_CRM_FollowUpRecords>
    {
        public override void Initialize()
        {
            //
            RuleFor(x => x.FollowUpDate).LessThanOrEqualTo(System.DateTime.Now).WithMessage("跟进日期不能大于当前日期。");
            RuleFor(x => x.FollowUpContent).MinimumLength(10).WithMessage("跟进内容:长度要大于10。");
            RuleFor(x => x.LeadID).NotNull().When(c => !c.Customer_id.HasValue).WithMessage("如果目标客户为空时：线索不能为空。");
            RuleFor(x => x.FollowUpMethod).GreaterThan(0).WithMessage("跟进方式：请选择正确的下拉值。");

        }


    }
}

