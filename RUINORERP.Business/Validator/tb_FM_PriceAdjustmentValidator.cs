
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:12
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
    /// 价格调整单验证类
    /// </summary>
    /*public partial class tb_FM_PriceAdjustmentValidator:AbstractValidator<tb_FM_PriceAdjustment>*/
    public partial class tb_FM_PriceAdjustmentValidator:BaseValidatorGeneric<tb_FM_PriceAdjustment>
    {
     

     public tb_FM_PriceAdjustmentValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.AdjustNo).MaximumMixedLength(30).WithMessage("调整编号:不能超过最大长度,30.");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("往来单位:下拉选择值不正确。");

//***** 
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.ReceivePaymentType).NotNull().WithMessage("收付类型:不能为空。");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.SourceBizType).NotEmpty().When(x => x.SourceBizType.HasValue);

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.SourceBillId).NotEmpty().When(x => x.SourceBillId.HasValue);

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.SourceBillNo).MaximumMixedLength(30).WithMessage("来源单号:不能超过最大长度,30.");

//***** 
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.PayStatus).NotNull().WithMessage("付款状态:不能为空。");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款类型:下拉选择值不正确。");
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.AdjustReason).MaximumMixedLength(400).WithMessage("调整原因:不能超过最大长度,400.");


 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.InvoiceId).NotEmpty().When(x => x.InvoiceId.HasValue);



 RuleFor(x => x.TotalLocalDiffAmount).PrecisionScale(19,4,true).WithMessage("总差异金额:小数位不能超过4。");

 RuleFor(x => x.TaxTotalDiffLocalAmount).PrecisionScale(19,4,true).WithMessage("总差异税额:小数位不能超过4。");

//***** 
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Remark).MaximumMixedLength(300).WithMessage("备注:不能超过最大长度,300.");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.ApprovalOpinions).MaximumMixedLength(255).WithMessage("审批意见:不能超过最大长度,255.");

 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");


 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PriceAdjustment =>tb_FM_PriceAdjustment.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


           	                //long?
                //AdjustId
                //tb_FM_PriceAdjustmentDetail
                //RuleFor(x => x.tb_FM_PriceAdjustmentDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_PriceAdjustmentDetails).NotNull();
                //RuleForEach(x => x.tb_FM_PriceAdjustmentDetails).NotNull();
                //RuleFor(x => x.tb_FM_PriceAdjustmentDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PriceAdjustmentDetail> details)
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

