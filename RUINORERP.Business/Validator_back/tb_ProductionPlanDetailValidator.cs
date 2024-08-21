
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:47
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
    /// 生产计划明细验证类
    /// </summary>
    public partial class tb_ProductionPlanDetailValidator:AbstractValidator<tb_ProductionPlanDetail>
    {
     public tb_ProductionPlanDetailValidator() 
     {
     //***** 
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.PPID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
//***** 
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.BOM_NO).MaximumLength(50).WithMessage("配方编号:不能超过最大长度,50.");
 RuleFor(tb_ProductionPlanDetail =>tb_ProductionPlanDetail.Summary).MaximumLength(255).WithMessage("摘要:不能超过最大长度,255.");
       	
           	
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

