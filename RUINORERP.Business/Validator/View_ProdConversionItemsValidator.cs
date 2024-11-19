
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:23
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
    /// 转换明细统计验证类
    /// </summary>
    /*public partial class View_ProdConversionItemsValidator:AbstractValidator<View_ProdConversionItems>*/
    public partial class View_ProdConversionItemsValidator:BaseValidatorGeneric<View_ProdConversionItems>
    {
     public View_ProdConversionItemsValidator() 
     {
      RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.ConversionNo).MaximumLength(25).WithMessage("转换单号:不能超过最大长度,25.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Reason).MaximumLength(150).WithMessage("转换原因:不能超过最大长度,150.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.ProdDetailID_from).NotEmpty().When(x => x.ProdDetailID_from.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.BarCode_from).MaximumLength(127).WithMessage("来源条码:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.SKU_from).MaximumLength(127).WithMessage("来源SKU:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Type_ID_from).NotEmpty().When(x => x.Type_ID_from.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.CNName_from).MaximumLength(127).WithMessage("来源品名:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Model_from).MaximumLength(25).WithMessage("来源型号:不能超过最大长度,25.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Specifications_from).MaximumLength(500).WithMessage("来源规格:不能超过最大长度,500.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.property_from).MaximumLength(127).WithMessage("来源属性:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.ConversionQty).NotEmpty().When(x => x.ConversionQty.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.ProdDetailID_to).NotEmpty().When(x => x.ProdDetailID_to.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.BarCode_to).MaximumLength(127).WithMessage("目标条码:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.SKU_to).MaximumLength(127).WithMessage("目标SKU:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Type_ID_to).NotEmpty().When(x => x.Type_ID_to.HasValue);
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.CNName_to).MaximumLength(127).WithMessage("目标品名:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Model_to).MaximumLength(25).WithMessage("目标型号:不能超过最大长度,25.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Specifications_to).MaximumLength(500).WithMessage("目标规格:不能超过最大长度,500.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.property_to).MaximumLength(127).WithMessage("目标属性:不能超过最大长度,127.");
 RuleFor(View_ProdConversionItems =>View_ProdConversionItems.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
       	
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

