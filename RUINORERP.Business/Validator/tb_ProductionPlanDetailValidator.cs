
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:19
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
    /// 生产计划明细验证类
    /// </summary>
    /*public partial class tb_ProductionPlanDetailValidator:AbstractValidator<tb_ProductionPlanDetail>*/
    public partial class tb_ProductionPlanDetailValidator:BaseValidatorGeneric<tb_ProductionPlanDetail>
    {
     

     public tb_ProductionPlanDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.PPID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.Specifications).MaximumMixedLength(1000).WithMessage("规格:不能超过最大长度,1000.");

 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

//***** 
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.Quantity).NotNull().WithMessage("计划数量:不能为空。");


 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.BOM_ID).Must(CheckForeignKeyValue).WithMessage("配方名称:下拉选择值不正确。");

 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");

//***** 
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.CompletedQuantity).NotNull().WithMessage("完成数量:不能为空。");

//***** 
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.AnalyzedQuantity).NotNull().WithMessage("已分析数量:不能为空。");


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

