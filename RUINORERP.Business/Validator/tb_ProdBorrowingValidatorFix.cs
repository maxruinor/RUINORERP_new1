
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:29
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
    /// 产品借出单验证类
    /// </summary>
    /*public partial class tb_ProdBorrowingValidator:AbstractValidator<tb_ProdBorrowing>*/
    public partial class tb_ProdBorrowingValidator : BaseValidatorGeneric<tb_ProdBorrowing>
    {
        public override void Initialize()
        {
            RuleFor(x => x.Reason).NotEmpty().WithMessage("借出原因:不能为空.");
            RuleFor(x => x.Reason).MinimumLength(3).WithMessage("借出原因:最小长度为3.");
            RuleFor(x => x.CustomerVendor_ID)
                .Custom((value, context) =>
                {
                    var entity = context.InstanceToValidate as tb_ProdBorrowing;
                    if (entity != null)
                    {
                        //根据配置判断预交日期是不是必须填写
                        //实际情况是 保存时可能不清楚交期，保存后截图发给供应商后才知道。这时提交才要求
                        if (ValidatorConfig.CurrentValue.借出单的接收单位必填)
                        {
                            if (entity.CustomerVendor_ID == null || !entity.CustomerVendor_ID.HasValue || entity.CustomerVendor_ID.Value <= 0)
                            {
                                context.AddFailure("接收单位：必须填写。");
                            }
                            RuleFor(x => x.CustomerVendor_ID).NotNull().WithMessage("接收单位：必须填写。");
                            RuleFor(x => x.CustomerVendor_ID).NotEmpty().When(c => c.CustomerVendor_ID.HasValue).WithMessage("接收单位：必须填写。");
                        }
                    }
                });

        }

    }

}

