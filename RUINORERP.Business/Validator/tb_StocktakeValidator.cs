
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:37
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
    /// 盘点表验证类
    /// </summary>
    /*public partial class tb_StocktakeValidator:AbstractValidator<tb_Stocktake>*/
    public partial class tb_StocktakeValidator:BaseValidatorGeneric<tb_Stocktake>
    {
     public tb_StocktakeValidator() 
     {
      RuleFor(tb_Stocktake =>tb_Stocktake.Employee_ID).Must(CheckForeignKeyValue).WithMessage("盘点负责人:下拉选择值不正确。");
 RuleFor(tb_Stocktake =>tb_Stocktake.Location_ID).Must(CheckForeignKeyValue).WithMessage("盘点仓库:下拉选择值不正确。");
 RuleFor(tb_Stocktake =>tb_Stocktake.CheckNo).MaximumLength(25).WithMessage("盘点单号:不能超过最大长度,25.");
 RuleFor(tb_Stocktake =>tb_Stocktake.CheckNo).NotEmpty().WithMessage("盘点单号:不能为空。");
//***** 
 RuleFor(tb_Stocktake =>tb_Stocktake.CheckMode).NotNull().WithMessage("盘点方式:不能为空。");
//***** 
 RuleFor(tb_Stocktake =>tb_Stocktake.Adjust_Type).NotNull().WithMessage("调整类型:不能为空。");
 RuleFor(tb_Stocktake =>tb_Stocktake.CheckResult).NotEmpty().When(x => x.CheckResult.HasValue);
//***** 
 RuleFor(tb_Stocktake =>tb_Stocktake.CarryingTotalQty).NotNull().WithMessage("载账总数量:不能为空。");
 RuleFor(x => x.CarryingTotalAmount).PrecisionScale(19,6,true).WithMessage("载账总成本:小数位不能超过6。");
 RuleFor(tb_Stocktake =>tb_Stocktake.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Stocktake =>tb_Stocktake.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_Stocktake =>tb_Stocktake.Notes).MaximumLength(500).WithMessage("备注:不能超过最大长度,500.");
//***** 
 RuleFor(tb_Stocktake =>tb_Stocktake.DiffTotalQty).NotNull().WithMessage("差异总数量:不能为空。");
 RuleFor(x => x.DiffTotalAmount).PrecisionScale(19,6,true).WithMessage("差异总金额:小数位不能超过6。");
//***** 
 RuleFor(tb_Stocktake =>tb_Stocktake.CheckTotalQty).NotNull().WithMessage("盘点总数量:不能为空。");
 RuleFor(x => x.CheckTotalAmount).PrecisionScale(19,6,true).WithMessage("盘点总成本:小数位不能超过6。");
//***** 
 RuleFor(tb_Stocktake =>tb_Stocktake.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(tb_Stocktake =>tb_Stocktake.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(tb_Stocktake =>tb_Stocktake.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_Stocktake =>tb_Stocktake.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
           	                //long
                //MainID
                //tb_StocktakeDetail
                //RuleFor(x => x.tb_StocktakeDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_StocktakeDetails).NotNull();
                //RuleForEach(x => x.tb_StocktakeDetails).NotNull();
                //RuleFor(x => x.tb_StocktakeDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
                Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_StocktakeDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

