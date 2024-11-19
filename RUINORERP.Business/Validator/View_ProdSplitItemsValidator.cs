
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:26
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
    /// 分割明细统计-只管明细产品的数据主表的用查询验证类
    /// </summary>
    /*public partial class View_ProdSplitItemsValidator:AbstractValidator<View_ProdSplitItems>*/
    public partial class View_ProdSplitItemsValidator:BaseValidatorGeneric<View_ProdSplitItems>
    {
     public View_ProdSplitItemsValidator() 
     {
      RuleFor(View_ProdSplitItems =>View_ProdSplitItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.SplitNo).MaximumLength(25).WithMessage("拆分单号:不能超过最大长度,25.");
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Qty).NotEmpty().When(x => x.Qty.HasValue);
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(View_ProdSplitItems =>View_ProdSplitItems.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
       	
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

