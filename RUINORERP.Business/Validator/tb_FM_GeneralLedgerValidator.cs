
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 23:00:25
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
    /// 总账表来源于凭证分类汇总是财务报表的基础数据验证类
    /// </summary>
    /*public partial class tb_FM_GeneralLedgerValidator:AbstractValidator<tb_FM_GeneralLedger>*/
    public partial class tb_FM_GeneralLedgerValidator:BaseValidatorGeneric<tb_FM_GeneralLedger>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_GeneralLedgerValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     //***** 
 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.GeneralLedgerID).NotNull().WithMessage("总账:不能为空。");

 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Subject_id).Must(CheckForeignKeyValueCanNull).WithMessage("会计科目:下拉选择值不正确。");
 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Subject_id).NotEmpty().When(x => x.Subject_id.HasValue);

 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage("币别:下拉选择值不正确。");
 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);


 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.TransactionType).MaximumLength(25).WithMessage("交易类型:不能超过最大长度,25.");

 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Description).MaximumLength(100).WithMessage("描述:不能超过最大长度,100.");

 RuleFor(x => x.Amount).PrecisionScale(19,4,true).WithMessage("金额:小数位不能超过4。");

 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);

 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);

 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");


 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

//***** 
 RuleFor(tb_FM_GeneralLedger =>tb_FM_GeneralLedger.TransactionDirection).NotNull().WithMessage("交易方向:不能为空。");

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

