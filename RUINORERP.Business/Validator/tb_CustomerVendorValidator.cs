
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/08/2025 12:05:08
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
    /// 客户厂商表 开票资料这种与财务有关另外开表验证类
    /// </summary>
    /*public partial class tb_CustomerVendorValidator:AbstractValidator<tb_CustomerVendor>*/
    public partial class tb_CustomerVendorValidator:BaseValidatorGeneric<tb_CustomerVendor>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CustomerVendorValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVCode).MaximumLength(25).WithMessage("编号:不能超过最大长度,25.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVName).MaximumLength(127).WithMessage("全称:不能超过最大长度,127.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVName).NotEmpty().WithMessage("全称:不能为空。");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.ShortName).MaximumLength(25).WithMessage("简称:不能超过最大长度,25.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Type_ID).Must(CheckForeignKeyValueCanNull).WithMessage("客户厂商类型:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("责任人:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

//有默认值

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认交易方式:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Area).MaximumLength(25).WithMessage("所在地区:不能超过最大长度,25.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Contact).MaximumLength(25).WithMessage("联系人:不能超过最大长度,25.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.MobilePhone).MaximumLength(25).WithMessage("手机:不能超过最大长度,25.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Fax).MaximumLength(25).WithMessage("传真:不能超过最大长度,25.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Phone).MaximumLength(25).WithMessage("电话:不能超过最大长度,25.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Email).MaximumLength(50).WithMessage("邮箱:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Address).MaximumLength(127).WithMessage("地址:不能超过最大长度,127.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Website).MaximumLength(127).WithMessage("网址:不能超过最大长度,127.");

 RuleFor(x => x.CustomerCreditLimit).PrecisionScale(12,2,true).WithMessage("客户信用额度:小数位不能超过2。");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CustomerCreditDays).NotEmpty().When(x => x.CustomerCreditDays.HasValue);

 RuleFor(x => x.SupplierCreditLimit).PrecisionScale(12,2,true).WithMessage("供应商信用额度:小数位不能超过2。");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.SupplierCreditDays).NotEmpty().When(x => x.SupplierCreditDays.HasValue);




 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");


 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

//有默认值

//有默认值


           	        Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_OtherExpenseDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_PurGoodsRecommendDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

