
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:14
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
    /// 标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定验证类
    /// </summary>
    /*public partial class tb_BOM_SDetailSubstituteMaterialValidator:AbstractValidator<tb_BOM_SDetailSubstituteMaterial>*/
    public partial class tb_BOM_SDetailSubstituteMaterialValidator:BaseValidatorGeneric<tb_BOM_SDetailSubstituteMaterial>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_BOM_SDetailSubstituteMaterialValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.SubID).NotNull().WithMessage(":不能为空。");


 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品详情:下拉选择值不正确。");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.SKU).MaximumMixedLength(80).WithMessage("SKU:不能超过最大长度,80.");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.property).MaximumMixedLength(255).WithMessage("子件属性:不能超过最大长度,255.");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.Unit_ID).Must(CheckForeignKeyValue).WithMessage("单位:下拉选择值不正确。");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.UnitConversion_ID).Must(CheckForeignKeyValueCanNull).WithMessage("单位换算:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.UnitConversion_ID).NotEmpty().When(x => x.UnitConversion_ID.HasValue);

 RuleFor(x => x.UsedQty).PrecisionScale(8,4,true).WithMessage("用量:小数位不能超过4。");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.Radix).NotEmpty().When(x => x.Radix.HasValue);

 RuleFor(x => x.LossRate).PrecisionScale(15,4,true).WithMessage("损耗率:小数位不能超过4。");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.InstallPosition).MaximumMixedLength(50).WithMessage("组装位置:不能超过最大长度,50.");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.PositionNo).MaximumMixedLength(50).WithMessage("位号:不能超过最大长度,50.");

 RuleFor(x => x.UnitCost).PrecisionScale(19,4,true).WithMessage("单位成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalUnitCost).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.PositionDesc).MaximumMixedLength(100).WithMessage("位号描述:不能超过最大长度,100.");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.ManufacturingProcessID).NotEmpty().When(x => x.ManufacturingProcessID.HasValue);

 RuleFor(x => x.OutputRate).PrecisionScale(8,4,true).WithMessage("产出率:小数位不能超过4。");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.Child_BOM_Node_ID).NotEmpty().When(x => x.Child_BOM_Node_ID.HasValue);

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.PriorityUseType).NotEmpty().When(x => x.PriorityUseType.HasValue);

//***** 
 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.Sort).NotNull().WithMessage("排序:不能为空。");

 RuleFor(tb_BOM_SDetailSubstituteMaterial =>tb_BOM_SDetailSubstituteMaterial.Summary).MaximumMixedLength(200).WithMessage("摘要:不能超过最大长度,200.");

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

