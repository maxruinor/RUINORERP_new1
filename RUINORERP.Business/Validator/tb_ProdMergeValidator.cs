
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:30:03
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
    /// 产品组合单验证类
    /// </summary>
    public partial class tb_ProdMergeValidator:AbstractValidator<tb_ProdMerge>
    {
     public tb_ProdMergeValidator() 
     {
      RuleFor(tb_ProdMerge =>tb_ProdMerge.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_ProdMerge =>tb_ProdMerge.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.MergeNo).MaximumLength(25).WithMessage("组合单号:不能超过最大长度,25.");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产出对象:下拉选择值不正确。");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
//***** 
 RuleFor(tb_ProdMerge =>tb_ProdMerge.MergeTargetQty).NotNull().WithMessage("产出数量:不能为空。");
//***** 
 RuleFor(tb_ProdMerge =>tb_ProdMerge.MergeSourceTotalQty).NotNull().WithMessage("子件总数量:不能为空。");
 
 RuleFor(tb_ProdMerge =>tb_ProdMerge.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.BOM_ID).Must(CheckForeignKeyValueCanNull).WithMessage("拆分配方:下拉选择值不正确。");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.BOM_No).MaximumLength(25).WithMessage("配方编号:不能超过最大长度,25.");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ProdMerge =>tb_ProdMerge.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
//***** 
 RuleFor(tb_ProdMerge =>tb_ProdMerge.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(tb_ProdMerge =>tb_ProdMerge.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_ProdMerge =>tb_ProdMerge.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
           	                //long
                //MergeID
                //tb_ProdMergeDetail
                //RuleFor(x => x.tb_ProdMergeDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProdMergeDetails).NotNull();
                //RuleForEach(x => x.tb_ProdMergeDetails).NotNull();
                //RuleFor(x => x.tb_ProdMergeDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProdMergeDetail> details)
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

