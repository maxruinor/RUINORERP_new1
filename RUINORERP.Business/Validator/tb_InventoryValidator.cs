
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:30
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
    /// 库存表验证类
    /// </summary>
    public partial class tb_InventoryValidator:AbstractValidator<tb_Inventory>
    {
     public tb_InventoryValidator() 
     {
      RuleFor(tb_Inventory =>tb_Inventory.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品详情:下拉选择值不正确。");
 RuleFor(tb_Inventory =>tb_Inventory.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_Inventory =>tb_Inventory.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(tb_Inventory =>tb_Inventory.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.Quantity).NotNull().WithMessage("实际库存:不能为空。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.InitInventory).NotNull().WithMessage("期初数量:不能为空。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.Alert_Use).NotNull().WithMessage("使用预警:不能为空。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.On_the_way_Qty).NotNull().WithMessage("在途库存:不能为空。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.Sale_Qty).NotNull().WithMessage("拟销售量:不能为空。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.BatchNumber).NotNull().WithMessage("批次管理:不能为空。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.Alert_Quantity).NotNull().WithMessage("预警值:不能为空。");
 RuleFor(x => x.CostFIFO).PrecisionScale(19,4,true).WithMessage("先进先出成本:小数位不能超过4。");
 RuleFor(x => x.CostMonthlyWA).PrecisionScale(19,4,true).WithMessage("月加权平均成本:小数位不能超过4。");
 RuleFor(x => x.CostMovingWA).PrecisionScale(19,4,true).WithMessage("移动加权平均成本:小数位不能超过4。");
 RuleFor(x => x.Inv_AdvCost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");
 RuleFor(x => x.Inv_Cost).PrecisionScale(19,4,true).WithMessage("产品成本:小数位不能超过4。");
 RuleFor(x => x.Inv_SubtotalCostMoney).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.MakingQty).NotNull().WithMessage("在制数量:不能为空。");
//***** 
 RuleFor(tb_Inventory =>tb_Inventory.NotOutQty).NotNull().WithMessage("未发数量:不能为空。");
 RuleFor(tb_Inventory =>tb_Inventory.Notes).MaximumLength(250).WithMessage("备注说明:不能超过最大长度,250.");
 RuleFor(tb_Inventory =>tb_Inventory.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Inventory =>tb_Inventory.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
           	
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

