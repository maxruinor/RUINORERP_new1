
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:15
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
    /// 生产需求分析表明细验证类
    /// </summary>
    /*public partial class tb_ProductionDemandDetailValidator:AbstractValidator<tb_ProductionDemandDetail>*/
    public partial class tb_ProductionDemandDetailValidator:BaseValidatorGeneric<tb_ProductionDemandDetail>
    {
     public tb_ProductionDemandDetailValidator() 
     {
     //***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.PDID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.BOM_ID).Must(CheckForeignKeyValueCanNull).WithMessage("标准配方:下拉选择值不正确。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.ID).NotEmpty().When(x => x.ID.HasValue);
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.ParentId).NotEmpty().When(x => x.ParentId.HasValue);
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.NetRequirement).NotNull().WithMessage("净需求:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.GrossRequirement).NotNull().WithMessage("毛需求:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.NeedQuantity).NotNull().WithMessage("实际需求:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.MissingQuantity).NotNull().WithMessage("缺少数量:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.BookInventory).NotNull().WithMessage("账面库存:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.AvailableStock).NotNull().WithMessage("可用库存:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.InTransitInventory).NotNull().WithMessage("在途库存:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.MakeProcessInventory).NotNull().WithMessage("在制库存:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.Sale_Qty).NotNull().WithMessage("拟销售量:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.NotOutQty).NotNull().WithMessage("未发数量:不能为空。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");
       	
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

