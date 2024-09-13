
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:46
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
    /// 存货预警特性表验证类
    /// </summary>
    /*public partial class tb_Inv_Alert_AttributeValidator:AbstractValidator<tb_Inv_Alert_Attribute>*/
    public partial class tb_Inv_Alert_AttributeValidator:BaseValidatorGeneric<tb_Inv_Alert_Attribute>
    {
     public tb_Inv_Alert_AttributeValidator() 
     {
      RuleFor(tb_Inv_Alert_Attribute =>tb_Inv_Alert_Attribute.Inventory_ID).Must(CheckForeignKeyValueCanNull).WithMessage("库存:下拉选择值不正确。");
 RuleFor(tb_Inv_Alert_Attribute =>tb_Inv_Alert_Attribute.Inventory_ID).NotEmpty().When(x => x.Inventory_ID.HasValue);
 RuleFor(tb_Inv_Alert_Attribute =>tb_Inv_Alert_Attribute.AlertPeriod).NotEmpty().When(x => x.AlertPeriod.HasValue);
 RuleFor(tb_Inv_Alert_Attribute =>tb_Inv_Alert_Attribute.Max_quantity).NotEmpty().When(x => x.Max_quantity.HasValue);
 RuleFor(tb_Inv_Alert_Attribute =>tb_Inv_Alert_Attribute.Min_quantity).NotEmpty().When(x => x.Min_quantity.HasValue);
 RuleFor(tb_Inv_Alert_Attribute =>tb_Inv_Alert_Attribute.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Inv_Alert_Attribute =>tb_Inv_Alert_Attribute.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

