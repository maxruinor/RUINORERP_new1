
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/16/2025 11:47:57
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using SharpYaml.Tokens;
using System.Runtime.Remoting.Contexts;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 部门表是否分层验证类
    /// </summary>
    /*public partial class tb_DepartmentValidator:AbstractValidator<tb_Department>*/
    public partial class tb_DepartmentValidator : BaseValidatorGeneric<tb_Department>
    {
        public override void Initialize()
        {
            RuleFor(x => x.DepartmentCode)
            .Custom((value, context) =>
            {
                var prod = context.InstanceToValidate as tb_Department;
                // 确保实体不为null  并且是新增时才判断
                if (prod != null && prod.DepartmentID == 0)
                {
                    string propertyName = context.PropertyName;
                    // 在这里使用 propertyName
                    // Console.WriteLine($"正在验证的属性: {propertyName}");
                    // 实际的唯一性验证逻辑
                    if (!BeUniqueName(propertyName, value))
                    {
                        context.AddFailure("部门代号不能重复。");
                    }
                }
            });

        }
    }

}

