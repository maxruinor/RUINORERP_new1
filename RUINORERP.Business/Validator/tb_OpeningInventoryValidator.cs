
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:31
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
    /// 期初存货来自期初盘点或业务上首次库存入库验证类
    /// </summary>
    public partial class tb_OpeningInventoryValidator:AbstractValidator<tb_OpeningInventory>
    {
     public tb_OpeningInventoryValidator() 
     {
      RuleFor(tb_OpeningInventory =>tb_OpeningInventory.Inventory_ID).Must(CheckForeignKeyValueCanNull).WithMessage("库存:下拉选择值不正确。");
 RuleFor(tb_OpeningInventory =>tb_OpeningInventory.Inventory_ID).NotEmpty().When(x => x.Inventory_ID.HasValue);
//***** 
 RuleFor(tb_OpeningInventory =>tb_OpeningInventory.InitQty).NotNull().WithMessage("期初库存:不能为空。");
 RuleFor(x => x.Cost_price).PrecisionScale(19,4,true).WithMessage("成本价格:小数位不能超过4。");
 RuleFor(x => x.Subtotal_Cost_Price).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");
 RuleFor(tb_OpeningInventory =>tb_OpeningInventory.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
 RuleFor(tb_OpeningInventory =>tb_OpeningInventory.RefNO).MaximumLength(50).WithMessage("引用单据:不能超过最大长度,50.");
 RuleFor(tb_OpeningInventory =>tb_OpeningInventory.RefBizType).MaximumLength(50).WithMessage("单据类型:不能超过最大长度,50.");
 RuleFor(tb_OpeningInventory =>tb_OpeningInventory.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");
       	
           	
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

