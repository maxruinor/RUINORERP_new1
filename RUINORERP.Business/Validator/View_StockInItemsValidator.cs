
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:10:36
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
    /// 其他入库统计验证类
    /// </summary>
    /*public partial class View_StockInItemsValidator:AbstractValidator<View_StockInItems>*/
    public partial class View_StockInItemsValidator:BaseValidatorGeneric<View_StockInItems>
    {
     public View_StockInItemsValidator() 
     {
      RuleFor(View_StockInItems =>View_StockInItems.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.BillNo).MaximumLength(25).WithMessage("其他出库单号:不能超过最大长度,25.");
 RuleFor(View_StockInItems =>View_StockInItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(View_StockInItems =>View_StockInItems.RefNO).MaximumLength(25).WithMessage("引用单号:不能超过最大长度,25.");
 RuleFor(View_StockInItems =>View_StockInItems.RefBizType).NotEmpty().When(x => x.RefBizType.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.Qty).NotEmpty().When(x => x.Qty.HasValue);

 RuleFor(View_StockInItems =>View_StockInItems.Summary).MaximumLength(250).WithMessage("摘要:不能超过最大长度,250.");

 RuleFor(View_StockInItems =>View_StockInItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(View_StockInItems =>View_StockInItems.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
 RuleFor(View_StockInItems =>View_StockInItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_StockInItems =>View_StockInItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_StockInItems =>View_StockInItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_StockInItems =>View_StockInItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_StockInItems =>View_StockInItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(View_StockInItems =>View_StockInItems.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
 RuleFor(View_StockInItems =>View_StockInItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
       	
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

