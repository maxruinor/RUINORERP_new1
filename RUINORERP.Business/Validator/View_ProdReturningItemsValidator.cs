
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:25
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
    /// 归还明细统计验证类
    /// </summary>
    /*public partial class View_ProdReturningItemsValidator:AbstractValidator<View_ProdReturningItems>*/
    public partial class View_ProdReturningItemsValidator:BaseValidatorGeneric<View_ProdReturningItems>
    {
     public View_ProdReturningItemsValidator() 
     {
      RuleFor(View_ProdReturningItems =>View_ProdReturningItems.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.ReturnNo).MaximumLength(25).WithMessage("归还单号:不能超过最大长度,25.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.BorrowNO).MaximumLength(25).WithMessage("借出单号:不能超过最大长度,25.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.CloseCaseOpinions).MaximumLength(100).WithMessage("结案意见:不能超过最大长度,100.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Qty).NotEmpty().When(x => x.Qty.HasValue);

 RuleFor(View_ProdReturningItems =>View_ProdReturningItems.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");

       	
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

