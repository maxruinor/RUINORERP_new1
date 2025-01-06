
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:27
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
    /// 期初应收应付账款表验证类
    /// </summary>
    /*public partial class tb_FM_Initial_PayAndReceivableValidator:AbstractValidator<tb_FM_Initial_PayAndReceivable>*/
    public partial class tb_FM_Initial_PayAndReceivableValidator:BaseValidatorGeneric<tb_FM_Initial_PayAndReceivable>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_Initial_PayAndReceivableValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.subject_id).Must(CheckForeignKeyValueCanNull).WithMessage("科目:下拉选择值不正确。");
 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.subject_id).NotEmpty().When(x => x.subject_id.HasValue);

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.account_id).NotEmpty().When(x => x.account_id.HasValue);

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.ExpenseType_id).Must(CheckForeignKeyValueCanNull).WithMessage("业务类型:下拉选择值不正确。");
 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.ExpenseType_id).NotEmpty().When(x => x.ExpenseType_id.HasValue);

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("外部交易对象:下拉选择值不正确。");
 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.ExpenseName).MaximumLength(100).WithMessage("事由:不能超过最大长度,100.");


 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");

//有默认值


 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.Notes).MaximumLength(50).WithMessage("备注:不能超过最大长度,50.");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.TaxRate).PrecisionScale(8,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.UntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");


 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");

 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_Initial_PayAndReceivable =>tb_FM_Initial_PayAndReceivable.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	        Initialize();
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

