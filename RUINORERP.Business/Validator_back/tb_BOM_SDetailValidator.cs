
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:34:39
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
    public partial class tb_BOM_SDetailValidator:AbstractValidator<tb_BOM_SDetail>
    {
     public tb_BOM_SDetailValidator() 
     {
      RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("货品详情:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
//***** 
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.MainID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Type_ID).Must(CheckForeignKeyValue).WithMessage("类型:下拉选择值不正确。");
//有默认值
//有默认值
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.property).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.SubItemName).MaximumLength(200).WithMessage("子件名称:不能超过最大长度,200.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.SubItemSpec).MaximumLength(200).WithMessage("子件规格:不能超过最大长度,200.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Remarks).MaximumLength(200).WithMessage("备注说明:不能超过最大长度,200.");
 RuleFor(x => x.UsedQty).PrecisionScale(8,3,true).WithMessage("用量:小数位不能超过3。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.DeputyQty).NotEmpty().When(x => x.DeputyQty.HasValue);
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Radix).NotEmpty().When(x => x.Radix.HasValue);
 RuleFor(x => x.LossRate).PrecisionScale(5,4,true).WithMessage("损耗率:小数位不能超过4。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.InstallPosition).MaximumLength(50).WithMessage("组装位置:不能超过最大长度,50.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.PositionNo).MaximumLength(50).WithMessage("位号:不能超过最大长度,50.");
 RuleFor(x => x.UnitCost).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");
 RuleFor(x => x.SubtotalCost).PrecisionScale(19,4,true).WithMessage("物料成本:小数位不能超过4。");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.PositionDesc).MaximumLength(100).WithMessage("位号描述:不能超过最大长度,100.");
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.ManufacturingProcessID).NotEmpty().When(x => x.ManufacturingProcessID.HasValue);
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Child_BOM_Node_ID).NotEmpty().When(x => x.Child_BOM_Node_ID.HasValue);
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Substitute).NotEmpty().When(x => x.Substitute.HasValue);
 RuleFor(x => x.OutputRate).PrecisionScale(8,4,true).WithMessage("产出率:小数位不能超过4。");
//***** 
 RuleFor(tb_BOM_SDetail =>tb_BOM_SDetail.Sort).NotNull().WithMessage("排序:不能为空。");
       	
           	
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

