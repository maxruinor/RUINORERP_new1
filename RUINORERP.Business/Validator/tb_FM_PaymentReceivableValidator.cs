
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:05
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 应收款单验证类
    /// </summary>
    /*public partial class tb_FM_PaymentReceivableValidator:AbstractValidator<tb_FM_PaymentReceivable>*/
    public partial class tb_FM_PaymentReceivableValidator:BaseValidatorGeneric<tb_FM_PaymentReceivable>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentReceivableValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("往来单位:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款类型:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.PrePaymentReceiptID).Must(CheckForeignKeyValueCanNull).WithMessage("预收款单:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.PrePaymentReceiptID).NotEmpty().When(x => x.PrePaymentReceiptID.HasValue);

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);


 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("合计税额:小数位不能超过4。");

 RuleFor(x => x.UntaxedTotalAmont).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");

 RuleFor(x => x.TotalPayableAmount).PrecisionScale(19,4,true).WithMessage("应付总金额:小数位不能超过4。");

 RuleFor(x => x.TotalPaidAmount).PrecisionScale(19,4,true).WithMessage("已付总金额:小数位不能超过4。");


 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.PayReason).MaximumLength(250).WithMessage("付款用途:不能超过最大长度,250.");

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Remark).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");


 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_FM_PaymentReceivable =>tb_FM_PaymentReceivable.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //PaymentReceivableID
                //tb_FM_PaymentReceivableDetail
                //RuleFor(x => x.tb_FM_PaymentReceivableDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_PaymentReceivableDetails).NotNull();
                //RuleForEach(x => x.tb_FM_PaymentReceivableDetails).NotNull();
                //RuleFor(x => x.tb_FM_PaymentReceivableDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PaymentReceivableDetail> details)
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

