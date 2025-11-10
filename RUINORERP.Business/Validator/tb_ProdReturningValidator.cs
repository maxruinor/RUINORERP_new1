
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:18
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
    /// 归还单，如果部分无法归还，则强制结案借出单。生成一个财务数据做记录。验证类
    /// </summary>
    /*public partial class tb_ProdReturningValidator:AbstractValidator<tb_ProdReturning>*/
    public partial class tb_ProdReturningValidator:BaseValidatorGeneric<tb_ProdReturning>
    {
     

     public tb_ProdReturningValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProdReturning =>tb_ProdReturning.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("归还单位:下拉选择值不正确。");
 RuleFor(tb_ProdReturning =>tb_ProdReturning.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_ProdReturning =>tb_ProdReturning.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("归还人:下拉选择值不正确。");
 RuleFor(tb_ProdReturning =>tb_ProdReturning.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_ProdReturning =>tb_ProdReturning.ReturnNo).MaximumMixedLength(50).WithMessage("归还单号:不能超过最大长度,50.");

//***** 
 RuleFor(tb_ProdReturning =>tb_ProdReturning.TotalQty).NotNull().WithMessage("总数量:不能为空。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");


 RuleFor(tb_ProdReturning =>tb_ProdReturning.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

 RuleFor(tb_ProdReturning =>tb_ProdReturning.BorrowID).Must(CheckForeignKeyValue).WithMessage("借出单:下拉选择值不正确。");

 RuleFor(tb_ProdReturning =>tb_ProdReturning.BorrowNO).MaximumMixedLength(50).WithMessage("借出单号:不能超过最大长度,50.");
 RuleFor(tb_ProdReturning =>tb_ProdReturning.BorrowNO).NotEmpty().WithMessage("借出单号:不能为空。");


 RuleFor(tb_ProdReturning =>tb_ProdReturning.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ProdReturning =>tb_ProdReturning.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


//***** 
 RuleFor(tb_ProdReturning =>tb_ProdReturning.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_ProdReturning =>tb_ProdReturning.ApprovalOpinions).MaximumMixedLength(500).WithMessage("审批意见:不能超过最大长度,500.");

 RuleFor(tb_ProdReturning =>tb_ProdReturning.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_ProdReturning =>tb_ProdReturning.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(tb_ProdReturning =>tb_ProdReturning.CloseCaseOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");

           	                //long
                //ReturnID
                //tb_ProdReturningDetail
                //RuleFor(x => x.tb_ProdReturningDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProdReturningDetails).NotNull();
                //RuleForEach(x => x.tb_ProdReturningDetails).NotNull();
                //RuleFor(x => x.tb_ProdReturningDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProdReturningDetail> details)
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

