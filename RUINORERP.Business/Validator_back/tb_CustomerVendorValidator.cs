
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:34:48
// **************************************
using System;
﻿using SqlSugar;
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
    public partial class tb_CustomerVendorValidator:AbstractValidator<tb_CustomerVendor>
    {
     public tb_CustomerVendorValidator() 
     {
      RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVCode).MaximumLength(50).WithMessage("编号:不能超过最大长度,50.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVName).MaximumLength(255).WithMessage("全称:不能超过最大长度,255.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.CVName).NotEmpty().WithMessage("全称:不能为空。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.ShortName).MaximumLength(50).WithMessage("简称:不能超过最大长度,50.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Type_ID).Must(CheckForeignKeyValueCanNull).WithMessage("客户厂商类型:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("责任人:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认交易方式:下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Area).MaximumLength(50).WithMessage("所在地区:不能超过最大长度,50.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Contact).MaximumLength(50).WithMessage("联系人:不能超过最大长度,50.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Phone).MaximumLength(255).WithMessage("电话:不能超过最大长度,255.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Address).MaximumLength(255).WithMessage("地址:不能超过最大长度,255.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Website).MaximumLength(255).WithMessage("网址:不能超过最大长度,255.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
//有默认值
//有默认值
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.BankAccount_id).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_CustomerVendor =>tb_CustomerVendor.BankAccount_id).NotEmpty().When(x => x.BankAccount_id.HasValue);
       	
           	
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
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_OtherExpenseDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_PrePaymentBillDetail> details)
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

