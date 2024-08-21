
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:45
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
    public partial class tb_ProductionDemandDetailValidator:AbstractValidator<tb_ProductionDemandDetail>
    {
     public tb_ProductionDemandDetailValidator() 
     {
     //***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.PDID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("货品:下拉选择值不正确。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.MainID).Must(CheckForeignKeyValueCanNull).WithMessage("标准配方:下拉选择值不正确。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.MainID).NotEmpty().When(x => x.MainID.HasValue);
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.BOM_NO).MaximumLength(50).WithMessage("配方编号:不能超过最大长度,50.");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.NetRequirement).NotNull().WithMessage("净需求:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.GrossRequirement).NotNull().WithMessage("毛需求:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.NeedQuantity).NotNull().WithMessage("需求数量:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.BookInventory).NotNull().WithMessage("账面库存:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.AvailableStock).NotNull().WithMessage("可用库存:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.InTransitInventory).NotNull().WithMessage("在途库存:不能为空。");
//***** 
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.MakeProcessInventory).NotNull().WithMessage("在制库存:不能为空。");
 RuleFor(tb_ProductionDemandDetail =>tb_ProductionDemandDetail.Summary).MaximumLength(255).WithMessage("摘要:不能超过最大长度,255.");
       	
           	
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

