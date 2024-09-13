
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:31
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
    /// 成本单价表 参考天思货品基本资料中的价格部分验证类
    /// </summary>
    /*public partial class tb_CostUnitPriceValidator:AbstractValidator<tb_CostUnitPrice>*/
    public partial class tb_CostUnitPriceValidator:BaseValidatorGeneric<tb_CostUnitPrice>
    {
     public tb_CostUnitPriceValidator() 
     {
      RuleFor(tb_CostUnitPrice =>tb_CostUnitPrice.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(tb_CostUnitPrice =>tb_CostUnitPrice.Group_id).NotEmpty().When(x => x.Group_id.HasValue);
 RuleFor(tb_CostUnitPrice =>tb_CostUnitPrice.SpecInstructions).MaximumLength(127).WithMessage("特殊说明:不能超过最大长度,127.");
 RuleFor(tb_CostUnitPrice =>tb_CostUnitPrice.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(tb_CostUnitPrice =>tb_CostUnitPrice.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_CostUnitPrice =>tb_CostUnitPrice.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

