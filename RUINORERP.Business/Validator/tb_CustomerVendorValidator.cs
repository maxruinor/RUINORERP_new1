
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 客户厂商表验证类
    /// </summary>
    /*public partial class tb_CustomerVendorValidator:AbstractValidator<tb_CustomerVendor>*/
    public partial class tb_CustomerVendorValidator:BaseValidatorGeneric<tb_CustomerVendor>
    {
     

     public tb_CustomerVendorValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVCode).MaximumMixedLength(50).WithMessage("编号:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVName).MaximumMixedLength(255).WithMessage("全称:不能超过最大长度,255.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVName).NotEmpty().WithMessage("全称:不能为空。");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.ShortName).MaximumMixedLength(50).WithMessage("简称:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Type_ID).Must(CheckForeignKeyValueCanNull).WithMessage("客户厂商类型:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("责任人:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

//有默认值

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认交易方式:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage("目标客户:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Area).MaximumMixedLength(50).WithMessage("所在地区:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Contact).MaximumMixedLength(50).WithMessage("联系人:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.MobilePhone).MaximumMixedLength(50).WithMessage("手机:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Fax).MaximumMixedLength(50).WithMessage("传真:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Phone).MaximumMixedLength(50).WithMessage("座机:不能超过最大长度,50.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Email).MaximumMixedLength(100).WithMessage("邮箱:不能超过最大长度,100.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Address).MaximumMixedLength(255).WithMessage("地址:不能超过最大长度,255.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Website).MaximumMixedLength(255).WithMessage("网址:不能超过最大长度,255.");

 RuleFor(x => x.CustomerCreditLimit).PrecisionScale(12,2,true).WithMessage("客户信用额度:小数位不能超过2。");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CustomerCreditDays).NotEmpty().When(x => x.CustomerCreditDays.HasValue);

 RuleFor(x => x.SupplierCreditLimit).PrecisionScale(12,2,true).WithMessage("供应商信用额度:小数位不能超过2。");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.SupplierCreditDays).NotEmpty().When(x => x.SupplierCreditDays.HasValue);




 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.SpecialNotes).MaximumMixedLength(500).WithMessage("特殊要求:不能超过最大长度,500.");

 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


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

