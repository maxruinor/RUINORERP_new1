
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:09:48
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 库存快照表验证类
    /// </summary>
    /*public partial class tb_InventorySnapshotValidator:AbstractValidator<tb_InventorySnapshot>*/
    public partial class tb_InventorySnapshotValidator:BaseValidatorGeneric<tb_InventorySnapshot>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_InventorySnapshotValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.ProdDetailID).NotNull().WithMessage("产品详情:不能为空。");

//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.Location_ID).NotNull().WithMessage("库位:不能为空。");

//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.Quantity).NotNull().WithMessage("实际库存:不能为空。");

//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.InitInventory).NotNull().WithMessage("期初数量:不能为空。");

 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.On_the_way_Qty).NotNull().WithMessage("在途库存:不能为空。");

//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.Sale_Qty).NotNull().WithMessage("拟销售量:不能为空。");

//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.MakingQty).NotNull().WithMessage("在制数量:不能为空。");

//***** 
 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.NotOutQty).NotNull().WithMessage("未发料量:不能为空。");

 RuleFor(x => x.CostFIFO).PrecisionScale(19,4,true).WithMessage("先进先出成本:小数位不能超过4。");

 RuleFor(x => x.CostMonthlyWA).PrecisionScale(19,4,true).WithMessage("月加权平均成本:小数位不能超过4。");

 RuleFor(x => x.CostMovingWA).PrecisionScale(19,4,true).WithMessage("移动加权平均成本:小数位不能超过4。");

 RuleFor(x => x.Inv_AdvCost).PrecisionScale(19,4,true).WithMessage("实际成本:小数位不能超过4。");

 RuleFor(x => x.Inv_Cost).PrecisionScale(19,4,true).WithMessage("产品成本:小数位不能超过4。");

 RuleFor(x => x.Inv_SubtotalCostMoney).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");





 RuleFor(tb_InventorySnapshot =>tb_InventorySnapshot.Notes).MaximumMixedLength(250).WithMessage("备注说明:不能超过最大长度,250.");

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

