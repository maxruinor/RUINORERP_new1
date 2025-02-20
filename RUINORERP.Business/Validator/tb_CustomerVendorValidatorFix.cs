
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/22/2024 18:12:39
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
    /// 客户厂商表 开票资料这种与财务有关另外开表验证类
    /// </summary>
    /*public partial class tb_CustomerVendorValidator:AbstractValidator<tb_CustomerVendor>*/
    public partial class tb_CustomerVendorValidator : BaseValidatorGeneric<tb_CustomerVendor>
    {
        public override void Initialize()
        {
            //不在这里判断了。因为如果不用CRM模块时这个就不判断了。直接UI保存时判断
            //  RuleFor(x => x.IsVendor).NotEqual(false).When(x => x.Customer_id.HasValue == false).WithMessage("总金额：添加客户时，总金额要大于零。");
            //RuleFor(customer => customer.CVName).Must(BeUniqueName).WithMessage("全称不能重复。");

            //RuleFor(u => u.Contact_Email)
            //.NotEmpty().WithMessage("邮箱：不能为空。")
            //.EmailAddress().WithMessage("邮箱：格式不正确。");


            RuleFor(customer => customer.CVName)
          .Custom((value, context) =>
          {
              var customer = context.InstanceToValidate as tb_CustomerVendor; // 假设你的实体类名为Customer

              // 确保customer不为null  并且是新增时才判断
              if (customer != null && customer.CustomerVendor_ID == 0)
              {
                  string propertyName = context.PropertyName;
                  // 在这里使用 propertyName
                  // Console.WriteLine($"正在验证的属性: {propertyName}");
                  // 实际的唯一性验证逻辑
                  if (!BeUniqueName(propertyName, value))
                  {
                      context.AddFailure("全称不能重复。");
                  }
              }
          });

       

            // 或者使用更简洁的方式
            RuleFor(customer => customer.IsCustomer)
                .Custom((isCustomer, context) =>
                {
                    var customer = context.InstanceToValidate as tb_CustomerVendor;
                    if (!customer.IsCustomer && !customer.IsVendor && !customer.IsOther)
                    {
                        context.AddFailure("【客户】，【供应商】或【其它往来单位】，至少 选一个。");
                    }
                });
            RuleFor(customer => customer.IsVendor)
              .Custom((isCustomer, context) =>
              {
                  var customer = context.InstanceToValidate as tb_CustomerVendor;
                  if (!customer.IsCustomer && !customer.IsVendor && !customer.IsOther)
                  {
                      context.AddFailure("【客户】，【供应商】或【其它往来单位】，至少 选一个。");
                  }
              });

            RuleFor(customer => customer.IsOther)
              .Custom((isCustomer, context) =>
              {
                  var customer = context.InstanceToValidate as tb_CustomerVendor;
                  if (!customer.IsCustomer && !customer.IsVendor && !customer.IsOther)
                  {
                      context.AddFailure("【客户】，【供应商】或【其它往来单位】，至少 选一个。");
                  }
              });


            /*
            RuleFor(x => x.IsCustomer).
           Custom((contact, context) =>
           {
               var customer = context.InstanceToValidate as tb_CustomerVendor; // 假设你的实体类名为Customer
               bool isEmployeeValid = customer.IsCustomer;
               bool isCustomerVendorValid = customer.IsCustomer && customer.IsVendor && customer.IsOther;
               if (!isEmployeeValid && !isCustomerVendorValid)
               {
                   context.AddFailure("【客户】，【供应商】或【其它往来单位】，至少 选一个。");
               }
           });
            RuleFor(x => x.IsVendor).
            Custom((contact, context) =>
            {
                var customer = context.InstanceToValidate as tb_CustomerVendor; // 假设你的实体类名为Customer
                bool isEmployeeValid = customer.IsVendor;
                bool isCustomerVendorValid = customer.IsCustomer && customer.IsVendor && customer.IsOther;
                if (!isEmployeeValid && !isCustomerVendorValid)
                {
                    context.AddFailure("【客户】，【供应商】或【其它往来单位】，至少 选一个。");
                }
            });
            RuleFor(x => x.IsOther).
        Custom((contact, context) =>
        {
            var customer = context.InstanceToValidate as tb_CustomerVendor; // 假设你的实体类名为Customer
            bool isEmployeeValid = customer.IsOther;
            bool isCustomerVendorValid = customer.IsCustomer && customer.IsVendor && customer.IsOther;
            if (!isEmployeeValid && !isCustomerVendorValid)
            {
                context.AddFailure("【客户】，【供应商】或【其它往来单位】，至少 选一个。");
            }
        });

            */
        }
    }

}

