
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:30
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
    /// 采购入库单 供应商接到采购订单后，向企业发货，用户在收到货物时，可以先检验，对合格品进行入库，也可以直接入库，形成采购入库单。为了保证清楚地记录进货情况，对进货的管理就很重要，而在我们的系统中，凭证、收付款是根据进货单自动一环扣一环地切制，故详细输入进货单资料后，存货的数量、成本会随着改变，收付账款也会跟着你的立账方式变化；凭证亦会随着“您是否立即产生凭证”变化。采购入库单可以由采购订单、借入单、在途物资单转入，也可以手动录入新增单据。验证类
    /// </summary>
    /*public partial class tb_PurEntryReValidator:AbstractValidator<tb_PurEntryRe>*/
    public partial class tb_PurEntryReValidator:BaseValidatorGeneric<tb_PurEntryRe>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_PurEntryReValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.PurEntryReNo).MaximumLength(25).WithMessage("退回单号:不能超过最大长度,25.");
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.PurEntryReNo).NotEmpty().WithMessage("退回单号:不能为空。");

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("供应商:下拉选择值不正确。");

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款类型:下拉选择值不正确。");
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.PurEntryID).NotEmpty().When(x => x.PurEntryID.HasValue);

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.PurEntryNo).MaximumLength(25).WithMessage("入库单号:不能超过最大长度,25.");

//***** 
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.TotalQty).NotNull().WithMessage("合计数量:不能为空。");

 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("合计税额:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("合计金额:小数位不能超过4。");

 //RuleFor(x => x.ActualAmount).PrecisionScale(19,4,true).WithMessage("实退金额:小数位不能超过4。");

//***** 
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.ProcessWay).NotNull().WithMessage("处理方式:不能为空。");


 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.ShippingWay).MaximumLength(25).WithMessage("发货方式:不能超过最大长度,25.");




 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");




 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);

 
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);

 
//***** 
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


//***** 
 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");


 RuleFor(tb_PurEntryRe =>tb_PurEntryRe.VoucherNO).MaximumLength(25).WithMessage("凭证号码:不能超过最大长度,25.");

           	                //long
                //PurEntryRe_ID
                //tb_PurEntryReDetail
                //RuleFor(x => x.tb_PurEntryReDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_PurEntryReDetails).NotNull();
                //RuleForEach(x => x.tb_PurEntryReDetails).NotNull();
                //RuleFor(x => x.tb_PurEntryReDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_PurEntryReDetail> details)
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

