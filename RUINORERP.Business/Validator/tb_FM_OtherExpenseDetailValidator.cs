
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:29
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单验证类
    /// </summary>
    /*public partial class tb_FM_OtherExpenseDetailValidator:AbstractValidator<tb_FM_OtherExpenseDetail>*/
    public partial class tb_FM_OtherExpenseDetailValidator:BaseValidatorGeneric<tb_FM_OtherExpenseDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_OtherExpenseDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.ExpenseMainID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.ExpenseName).MaximumMixedLength(300).WithMessage("事由:不能超过最大长度,300.");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.ExpenseName).NotEmpty().WithMessage("事由:不能为空。");

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("归属部门:下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.ExpenseType_id).Must(CheckForeignKeyValueCanNull).WithMessage("费用类型:下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.ExpenseType_id).NotEmpty().When(x => x.ExpenseType_id.HasValue);

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("交易账号:下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("交易对象:下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.Subject_id).Must(CheckForeignKeyValueCanNull).WithMessage("会计科目:下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.Subject_id).NotEmpty().When(x => x.Subject_id.HasValue);


 RuleFor(x => x.SingleTotalAmount).PrecisionScale(19,4,true).WithMessage("单项总金额:小数位不能超过4。");


 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.Summary).MaximumMixedLength(100).WithMessage("摘要:不能超过最大长度,100.");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");

 RuleFor(x => x.UntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("所属项目:下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_OtherExpenseDetail =>tb_FM_OtherExpenseDetail.EvidenceImagePath).MaximumMixedLength(300).WithMessage("凭证图:不能超过最大长度,300.");

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

