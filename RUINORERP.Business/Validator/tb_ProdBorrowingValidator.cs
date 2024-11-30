
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:05
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
    /// 产品借出单验证类
    /// </summary>
    /*public partial class tb_ProdBorrowingValidator:AbstractValidator<tb_ProdBorrowing>*/
    public partial class tb_ProdBorrowingValidator:BaseValidatorGeneric<tb_ProdBorrowing>
    {
     public tb_ProdBorrowingValidator() 
     {
      RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("接收单位:下拉选择值不正确。");
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.Employee_ID).Must(CheckForeignKeyValue).WithMessage("借出人:下拉选择值不正确。");
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.BorrowNo).MaximumLength(25).WithMessage("借出单号:不能超过最大长度,25.");
//***** 
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.TotalQty).NotNull().WithMessage("总数量:不能为空。");
 RuleFor(x => x.TotalCost).PrecisionScale(19,6,true).WithMessage("总成本:小数位不能超过6。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,6,true).WithMessage("总金额:小数位不能超过6。");
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
//***** 
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.Reason).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.ApprovalOpinions).MaximumLength(250).WithMessage("借出原因:不能超过最大长度,250.");
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
 RuleFor(tb_ProdBorrowing =>tb_ProdBorrowing.CloseCaseOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");
       	
           	                //long
                //BorrowID
                //tb_ProdBorrowingDetail
                //RuleFor(x => x.tb_ProdBorrowingDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProdBorrowingDetails).NotNull();
                //RuleForEach(x => x.tb_ProdBorrowingDetails).NotNull();
                //RuleFor(x => x.tb_ProdBorrowingDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
                Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProdBorrowingDetail> details)
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

