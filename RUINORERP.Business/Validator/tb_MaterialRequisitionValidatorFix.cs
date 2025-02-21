
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:28
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
    /// 领料单(包括生产和托工)验证类
    /// </summary>
    /*public partial class tb_MaterialRequisitionValidator:AbstractValidator<tb_MaterialRequisition>*/
    public partial class tb_MaterialRequisitionValidator : BaseValidatorGeneric<tb_MaterialRequisition>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            // 如果选择了外发，则必须填写外发加工商
            RuleFor(x => x.CustomerVendor_ID).NotNull().When(x => x.Outgoing == true).WithMessage("选择外发时，必须要选择外发的工厂。");
            RuleFor(x => x.CustomerVendor_ID).GreaterThan(0).When(x => x.Outgoing == true).WithMessage("选择外发时，必须要选择外发的工厂。");
            RuleFor(x => x.DepartmentID).Must(CheckForeignKeyValueCanNull)
                .When(x => x.Outgoing == false)
                .WithMessage("生产部门:非外发时，生产部门必须选择。");
            RuleFor(x => x.DepartmentID).NotNull()
                .When(x => x.Outgoing == false)
                .WithMessage("生产部门:非外发时，生产部门必须选择。");

        }
    }

}

