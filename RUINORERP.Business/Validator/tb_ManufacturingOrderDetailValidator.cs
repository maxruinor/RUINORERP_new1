
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/28/2024 11:55:42
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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节验证类
    /// </summary>
    public partial class tb_ManufacturingOrderDetailValidator:AbstractValidator<tb_ManufacturingOrderDetail>
    {
     public tb_ManufacturingOrderDetailValidator() 
     {
     //***** 
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.MOID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.ID).NotNull().WithMessage(":不能为空。");
//***** 
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.ParentId).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.BOM_NO).MaximumLength(25).WithMessage("配方编号:不能超过最大长度,25.");
 RuleFor(x => x.ShouldSendQty).PrecisionScale(10,3,true).WithMessage("应发数:小数位不能超过3。");
 RuleFor(x => x.ActualSentQty).PrecisionScale(10,3,true).WithMessage("实发数:小数位不能超过3。");
 RuleFor(x => x.OverSentQty).PrecisionScale(10,3,true).WithMessage("超发数:小数位不能超过3。");
 RuleFor(x => x.WastageQty).PrecisionScale(10,3,true).WithMessage("损耗量:小数位不能超过3。");
 RuleFor(x => x.CurrentIinventory).PrecisionScale(10,3,true).WithMessage("现有库存:小数位不能超过3。");
 RuleFor(x => x.MaterialCost).PrecisionScale(10,4,true).WithMessage("物料成本 :小数位不能超过4。");
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.AssemblyPosition).MaximumLength(250).WithMessage("组装位置:不能超过最大长度,250.");
 RuleFor(tb_ManufacturingOrderDetail =>tb_ManufacturingOrderDetail.Prelevel_BOM_Desc).MaximumLength(100).WithMessage("上级配方:不能超过最大长度,100.");
       	
           	
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

