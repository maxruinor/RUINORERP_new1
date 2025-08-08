
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:36
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
    /// 对账单主表验证类
    /// </summary>
    /*public partial class tb_FM_StatementValidator:AbstractValidator<tb_FM_Statement>*/
    public partial class tb_FM_StatementValidator:BaseValidatorGeneric<tb_FM_Statement>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_StatementValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_Statement =>tb_FM_Statement.StatementNo).MaximumMixedLength(30).WithMessage("对账单号:不能超过最大长度,30.");

 RuleFor(tb_FM_Statement =>tb_FM_Statement.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("往来单位:下拉选择值不正确。");

 RuleFor(tb_FM_Statement =>tb_FM_Statement.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(tb_FM_Statement =>tb_FM_Statement.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("公司账户:下拉选择值不正确。");
 RuleFor(tb_FM_Statement =>tb_FM_Statement.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_Statement =>tb_FM_Statement.PayeeInfoID).Must(CheckForeignKeyValueCanNull).WithMessage("收款信息:下拉选择值不正确。");
 RuleFor(tb_FM_Statement =>tb_FM_Statement.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);

 RuleFor(tb_FM_Statement =>tb_FM_Statement.PayeeAccountNo).MaximumMixedLength(100).WithMessage("收款账号:不能超过最大长度,100.");

//***** 
 RuleFor(tb_FM_Statement =>tb_FM_Statement.ReceivePaymentType).NotNull().WithMessage("收付类型:不能为空。");

 RuleFor(x => x.TotalForeignAmount).PrecisionScale(19,4,true).WithMessage("总金额外币:小数位不能超过4。");

 RuleFor(x => x.TotalLocalAmount).PrecisionScale(19,4,true).WithMessage("总金额本币:小数位不能超过4。");



 RuleFor(tb_FM_Statement =>tb_FM_Statement.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_FM_Statement =>tb_FM_Statement.StatementStatus).NotEmpty().When(x => x.StatementStatus.HasValue);

 RuleFor(tb_FM_Statement =>tb_FM_Statement.Remark).MaximumMixedLength(300).WithMessage("备注:不能超过最大长度,300.");


 RuleFor(tb_FM_Statement =>tb_FM_Statement.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_Statement =>tb_FM_Statement.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_Statement =>tb_FM_Statement.ApprovalOpinions).MaximumMixedLength(255).WithMessage("审批意见:不能超过最大长度,255.");

 RuleFor(tb_FM_Statement =>tb_FM_Statement.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_Statement =>tb_FM_Statement.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //StatementId
                //tb_FM_StatementDetail
                //RuleFor(x => x.tb_FM_StatementDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_StatementDetails).NotNull();
                //RuleForEach(x => x.tb_FM_StatementDetails).NotNull();
                //RuleFor(x => x.tb_FM_StatementDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
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

