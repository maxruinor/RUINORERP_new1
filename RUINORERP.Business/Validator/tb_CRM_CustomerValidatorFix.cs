
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 17:24:06
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
    /// 箱规表验证类
    /// </summary>
    public partial class tb_CRM_CustomerValidator : BaseValidatorGeneric<tb_CRM_Customer>
    {
        public override void Initialize()
        {
            RuleFor(x => x.CustomerName).NotNull().WithMessage("客户名称:不能为空。");
            RuleFor(x => x.CustomerName).MinimumLength(3).WithMessage("客户名称:长度要大于3。");
            //  RuleFor(customer => customer.CustomerName).Must(BeUniqueName).WithMessage("客户名称不能重复。");
            RuleFor(customer => customer.CustomerName)
         .Custom((value, context) =>
         {
             var customer = context.InstanceToValidate as tb_CRM_Customer; // 假设你的实体类名为Customer

             // 确保customer不为null  并且是新增时才判断
             if (customer != null && customer.Customer_id == 0)
             {
                 string propertyName = context.PropertyName;
                 // 在这里使用 propertyName
                 // Console.WriteLine($"正在验证的属性: {propertyName}");
                 // 实际的唯一性验证逻辑
                 if (!BeUniqueName(propertyName, value))
                 {
                     context.AddFailure("客户名称不能重复。");
                 }
             }
         });
        }
    }
}

