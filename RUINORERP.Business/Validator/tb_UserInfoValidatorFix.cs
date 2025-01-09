
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:33
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
    /// 用户表验证类
    /// </summary>
    /*public partial class tb_UserInfoValidator:AbstractValidator<tb_UserInfo>*/
    public partial class tb_UserInfoValidator : BaseValidatorGeneric<tb_UserInfo>
    {

        public override void Initialize()
        {
            RuleFor(x => x.Employee_ID)
             .Custom((value, context) =>
             {
                 var customer = context.InstanceToValidate as tb_UserInfo; // 假设你的实体类名为Customer

                 // 确保customer不为null  并且是新增时才判断
                 if (customer != null && customer.Employee_ID.HasValue && customer.Employee_ID == 0)
                 {
                     string propertyName = context.PropertyName;
                     // 实际的唯一性验证逻辑
                     if (!BeUniqueName(propertyName, value.Value.ToString()))
                     {
                         context.AddFailure("员工不能重复，一个用户只能属于一个员工。");
                     }
                 }
             });
        }
    }
}

