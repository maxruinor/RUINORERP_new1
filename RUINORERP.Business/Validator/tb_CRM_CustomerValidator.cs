
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:26
// **************************************
using System;
﻿using SqlSugar;
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
    /// 目标客户-公海客户CRM系统中使用，给成交客户作外键引用验证类
    /// </summary>
    /*public partial class tb_CRM_CustomerValidator:AbstractValidator<tb_CRM_Customer>*/
    public partial class tb_CRM_CustomerValidator:BaseValidatorGeneric<tb_CRM_Customer>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CRM_CustomerValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CustomerName).MaximumLength(25).WithMessage("客户名称:不能超过最大长度,25.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("对接人:下拉选择值不正确。");
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.LeadID).Must(CheckForeignKeyValueCanNull).WithMessage("线索:下拉选择值不正确。");
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.LeadID).NotEmpty().When(x => x.LeadID.HasValue);

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Region_ID).Must(CheckForeignKeyValueCanNull).WithMessage("地区:下拉选择值不正确。");
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Region_ID).NotEmpty().When(x => x.Region_ID.HasValue);

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.ProvinceID).Must(CheckForeignKeyValueCanNull).WithMessage("省:下拉选择值不正确。");
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.ProvinceID).NotEmpty().When(x => x.ProvinceID.HasValue);

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CityID).Must(CheckForeignKeyValueCanNull).WithMessage("城市:下拉选择值不正确。");
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CityID).NotEmpty().When(x => x.CityID.HasValue);

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.wwSocialTools).MaximumLength(100).WithMessage("旺旺/IM工具:不能超过最大长度,100.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.SocialTools).MaximumLength(100).WithMessage("其他/IM工具:不能超过最大长度,100.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Contact_Name).MaximumLength(25).WithMessage("联系人姓名:不能超过最大长度,25.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Contact_Email).MaximumLength(50).WithMessage("邮箱:不能超过最大长度,50.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Contact_Phone).MaximumLength(15).WithMessage("电话:不能超过最大长度,15.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CustomerAddress).MaximumLength(150).WithMessage("客户地址:不能超过最大长度,150.");


//***** 
 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CustomerStatus).NotNull().WithMessage("客户状态:不能为空。");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CustomerTags).MaximumLength(250).WithMessage("客户标签:不能超过最大长度,250.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CoreProductInfo).MaximumLength(100).WithMessage("获客来源:不能超过最大长度,100.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.GetCustomerSource).MaximumLength(125).WithMessage("主营产品信息:不能超过最大长度,125.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.SalePlatform).MaximumLength(25).WithMessage("销售平台:不能超过最大长度,25.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Website).MaximumLength(127).WithMessage("网址:不能超过最大长度,127.");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.CustomerLevel).NotEmpty().When(x => x.CustomerLevel.HasValue);

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.PurchaseCount).NotEmpty().When(x => x.PurchaseCount.HasValue);

 RuleFor(x => x.TotalPurchaseAmount).PrecisionScale(19,4,true).WithMessage("采购金额:小数位不能超过4。");

 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.DaysSinceLastPurchase).NotEmpty().When(x => x.DaysSinceLastPurchase.HasValue);




 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");


 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_CRM_Customer =>tb_CRM_Customer.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


           	        Initialize();
     }







    
          private bool CheckForeignKeyValue(long ForeignKeyID)
        {
            bool rs = true;    
            if (ForeignKeyID == 0 || ForeignKeyID == -1)
            {
                return false;
            }
            return rs;
        }
        
        private bool CheckForeignKeyValueCanNull(long? ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID.HasValue)
            {
                if (ForeignKeyID == 0 || ForeignKeyID == -1)
                {
                    return false;
                }
            }
            return rs;
        
    }
}

}

