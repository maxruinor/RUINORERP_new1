
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:36
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
    /// 标准物料表BOM明细-要适当冗余验证类
    /// </summary>
    /*public partial class tb_BOM_SDetailValidator:AbstractValidator<tb_BOM_SDetail>*/
    public partial class tb_BOM_SDetailValidator:BaseValidatorGeneric<tb_BOM_SDetail>
    {
     public tb_BOM_SDetailValidator() 
     {
      RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
//***** 
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.BOM_ID).NotNull().WithMessage("对应BOM:不能为空。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Remarks).MaximumLength(100).WithMessage("备注说明:不能超过最大长度,100.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Unit_ID).Must(CheckForeignKeyValue).WithMessage("单位:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.UnitConversion_ID).Must(CheckForeignKeyValueCanNull).WithMessage("单位换算:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.UnitConversion_ID).NotEmpty().When(x => x.UnitConversion_ID.HasValue);
 RuleFor(x => x.UsedQty).PrecisionScale(8,4,true).WithMessage("用量:小数位不能超过4。");
//***** 
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Radix).NotNull().WithMessage("基数:不能为空。");
 RuleFor(x => x.LossRate).PrecisionScale(15,4,true).WithMessage("损耗率:小数位不能超过4。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.InstallPosition).MaximumLength(25).WithMessage("组装位置:不能超过最大长度,25.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.PositionNo).MaximumLength(25).WithMessage("位号:不能超过最大长度,25.");
 RuleFor(x => x.MaterialCost).PrecisionScale(19,4,true).WithMessage("物料成本:小数位不能超过4。");
 RuleFor(x => x.SubtotalMaterialCost).PrecisionScale(19,4,true).WithMessage("物料小计:小数位不能超过4。");
 RuleFor(x => x.ManufacturingCost).PrecisionScale(19,4,true).WithMessage("制造费:小数位不能超过4。");
 RuleFor(x => x.OutManuCost).PrecisionScale(19,4,true).WithMessage("托工费:小数位不能超过4。");
 RuleFor(x => x.SubtotalManufacturingCost).PrecisionScale(19,4,true).WithMessage("制造费小计:小数位不能超过4。");
 RuleFor(x => x.SubtotalOutManuCost).PrecisionScale(19,4,true).WithMessage("托工费小计:小数位不能超过4。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.PositionDesc).MaximumLength(50).WithMessage("位号描述:不能超过最大长度,50.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.ManufacturingProcessID).NotEmpty().When(x => x.ManufacturingProcessID.HasValue);
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Child_BOM_Node_ID).NotEmpty().When(x => x.Child_BOM_Node_ID.HasValue);
 RuleFor(x => x.TotalSelfProductionAllCost).PrecisionScale(19,4,true).WithMessage("自产总成本:小数位不能超过4。");
 RuleFor(x => x.TotalOutsourcingAllCost).PrecisionScale(19,4,true).WithMessage("外发总成本:小数位不能超过4。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Substitute).NotEmpty().When(x => x.Substitute.HasValue);
 RuleFor(x => x.OutputRate).PrecisionScale(8,4,true).WithMessage("产出率:小数位不能超过4。");
//***** 
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Sort).NotNull().WithMessage("排序:不能为空。");
       	
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

