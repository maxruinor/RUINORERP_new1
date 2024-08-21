
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:12
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
    /// 销售出库退回单验证类
    /// </summary>
    public partial class tb_SaleOutReValidator:AbstractValidator<tb_SaleOutRe>
    {
     public tb_SaleOutReValidator() 
     {
      RuleFor(tb_SaleOutRe =>tb_SaleOutRe.ReturnNo).MaximumLength(50).WithMessage("退回单号:不能超过最大长度,50.");
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("退货客户:下拉选择值不正确。");
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.SaleOut_MainID).NotEmpty().When(x => x.SaleOut_MainID.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.SO_NO).NotEmpty().When(x => x.SO_NO.HasValue);
//***** 
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.TotalQty).NotNull().WithMessage("退回总数量:不能为空。");
 RuleFor(x => x.ActualRefundAmont).PrecisionScale(19,4,true).WithMessage("实际退款金额:小数位不能超过4。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("退款金额合计:小数位不能超过4。");
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.TrackNo).MaximumLength(50).WithMessage("物流单号:不能超过最大长度,50.");
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.ApprovalOpinions).MaximumLength(200).WithMessage("审批意见:不能超过最大长度,200.");
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);
//***** 
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
//***** 
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
//***** 
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.OrderTotalOutQty).NotNull().WithMessage("订单累计数量:不能为空。");
//***** 
 RuleFor(tb_SaleOutRe =>tb_SaleOutRe.OrderReturnTotalQty).NotNull().WithMessage("订单退回数量:不能为空。");
       	
           	                //long
                //SaleOutRe_ID
                //tb_SaleOutReDetail
                RuleFor(c => c.tb_SaleOutReDetails).NotNull();
                RuleForEach(x => x.tb_SaleOutReDetails).NotNull();
                //RuleFor(x => x.tb_SaleOutReDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_SaleOutReDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
     }




        private bool DetailedRecordsNotEmpty(List<tb_SaleOutReDetail> details)
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

