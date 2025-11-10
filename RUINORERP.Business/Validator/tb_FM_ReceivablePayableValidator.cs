
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
    /// 应收应付表验证类
    /// </summary>
    /*public partial class tb_FM_ReceivablePayableValidator:AbstractValidator<tb_FM_ReceivablePayable>*/
    public partial class tb_FM_ReceivablePayableValidator:BaseValidatorGeneric<tb_FM_ReceivablePayable>
    {
     

     public tb_FM_ReceivablePayableValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.ARAPNo).MaximumMixedLength(30).WithMessage("单据编号:不能超过最大长度,30.");

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.SourceBizType).NotEmpty().When(x => x.SourceBizType.HasValue);

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.SourceBillId).NotEmpty().When(x => x.SourceBillId.HasValue);

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.SourceBillNo).MaximumMixedLength(30).WithMessage("来源单号:不能超过最大长度,30.");



 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("往来单位:下拉选择值不正确。");

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");




 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.PlatformOrderNo).MaximumMixedLength(100).WithMessage("平台单号:不能超过最大长度,100.");

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("公司账户:下拉选择值不正确。");
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.PayeeInfoID).Must(CheckForeignKeyValueCanNull).WithMessage("收款信息:下拉选择值不正确。");
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.PayeeAccountNo).MaximumMixedLength(100).WithMessage("收款账号:不能超过最大长度,100.");

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

//***** 
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.ReceivePaymentType).NotNull().WithMessage("收付类型:不能为空。");

 RuleFor(x => x.ShippingFee).PrecisionScale(19,4,true).WithMessage("运费:小数位不能超过4。");

 RuleFor(x => x.TotalForeignPayableAmount).PrecisionScale(19,4,true).WithMessage("总金额外币:小数位不能超过4。");

 RuleFor(x => x.TotalLocalPayableAmount).PrecisionScale(19,4,true).WithMessage("总金额本币:小数位不能超过4。");

 RuleFor(x => x.ForeignPaidAmount).PrecisionScale(19,4,true).WithMessage("已核销外币:小数位不能超过4。");

 RuleFor(x => x.LocalPaidAmount).PrecisionScale(19,4,true).WithMessage("已核销本币:小数位不能超过4。");

 RuleFor(x => x.ForeignReconciledAmount).PrecisionScale(19,4,true).WithMessage("已对账外币:小数位不能超过4。");

 RuleFor(x => x.LocalReconciledAmount).PrecisionScale(19,4,true).WithMessage("已对账本币:小数位不能超过4。");

 RuleFor(x => x.ForeignBalanceAmount).PrecisionScale(19,4,true).WithMessage("未核销外币:小数位不能超过4。");

 RuleFor(x => x.LocalBalanceAmount).PrecisionScale(19,4,true).WithMessage("未核销本币:小数位不能超过4。");


 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.InvoiceId).Must(CheckForeignKeyValueCanNull).WithMessage("发票:下拉选择值不正确。");
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.InvoiceId).NotEmpty().When(x => x.InvoiceId.HasValue);


//有默认值


 RuleFor(x => x.TaxTotalAmount).PrecisionScale(19,4,true).WithMessage("税额总计:小数位不能超过4。");

 RuleFor(x => x.UntaxedTotalAmont).PrecisionScale(19,4,true).WithMessage("未税总计:小数位不能超过4。");

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.ARAPStatus).NotEmpty().When(x => x.ARAPStatus.HasValue);

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Remark).MaximumMixedLength(300).WithMessage("备注:不能超过最大长度,300.");


 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.ApprovalOpinions).MaximumMixedLength(255).WithMessage("审批意见:不能超过最大长度,255.");

 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_ReceivablePayable =>tb_FM_ReceivablePayable.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //ARAPId
                //tb_FM_ReceivablePayableDetail
                //RuleFor(x => x.tb_FM_ReceivablePayableDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_ReceivablePayableDetails).NotNull();
                //RuleForEach(x => x.tb_FM_ReceivablePayableDetails).NotNull();
                //RuleFor(x => x.tb_FM_ReceivablePayableDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_StatementDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_ReceivablePayableDetail> details)
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

