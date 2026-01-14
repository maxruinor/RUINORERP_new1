
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:14
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节验证类
    /// </summary>
    /*public partial class tb_ManufacturingOrderDetailValidator:AbstractValidator<tb_ManufacturingOrderDetail>*/
    public partial class tb_ManufacturingOrderDetailValidator:BaseValidatorGeneric<tb_ManufacturingOrderDetail>
    {
     

     public tb_ManufacturingOrderDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.MOID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");


 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.ID).NotNull().WithMessage(":不能为空。");

//***** 
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.ParentId).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.BOM_NO).MaximumMixedLength(50).WithMessage("配方编号:不能超过最大长度,50.");

 RuleFor(x => x.ShouldSendQty).PrecisionScale(10,3,true).WithMessage("应发数:小数位不能超过3。");

 RuleFor(x => x.ActualSentQty).PrecisionScale(10,3,true).WithMessage("实发数:小数位不能超过3。");

 RuleFor(x => x.OverSentQty).PrecisionScale(10,3,true).WithMessage("超发数:小数位不能超过3。");

 RuleFor(x => x.WastageQty).PrecisionScale(10,3,true).WithMessage("损耗量:小数位不能超过3。");

 RuleFor(x => x.CurrentIinventory).PrecisionScale(10,3,true).WithMessage("现有库存:小数位不能超过3。");

 RuleFor(x => x.UnitCost).PrecisionScale(10,4,true).WithMessage("单位成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalUnitCost).PrecisionScale(10,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);


 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.AssemblyPosition).MaximumMixedLength(500).WithMessage("组装位置:不能超过最大长度,500.");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.AlternativeProducts).MaximumMixedLength(50).WithMessage("替代品:不能超过最大长度,50.");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.Prelevel_BOM_Desc).MaximumMixedLength(100).WithMessage("上级配方名称:不能超过最大长度,100.");

 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.Prelevel_BOM_ID).NotEmpty().When(x => x.Prelevel_BOM_ID.HasValue);

           	  
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

