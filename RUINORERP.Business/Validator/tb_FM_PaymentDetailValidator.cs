
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:10
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
    /// 付款申请单明细-对应的应付单据项目验证类
    /// </summary>
    /*public partial class tb_FM_PaymentDetailValidator:AbstractValidator<tb_FM_PaymentDetail>*/
    public partial class tb_FM_PaymentDetailValidator:BaseValidatorGeneric<tb_FM_PaymentDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.PaymentID).NotEmpty().When(x => x.PaymentID.HasValue);

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.tb__DepartmentID).NotEmpty().When(x => x.tb__DepartmentID.HasValue);

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");


 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.PayReasonItems).MaximumLength(100).WithMessage("付款项目/原因:不能超过最大长度,100.");
 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.PayReasonItems).NotEmpty().WithMessage("付款项目/原因:不能为空。");

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.Summary).MaximumLength(150).WithMessage("摘要:不能超过最大长度,150.");

 RuleFor(x => x.SubAmount).PrecisionScale(19,4,true).WithMessage("付款金额:小数位不能超过4。");

 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.SubPamountInWords).MaximumLength(50).WithMessage("大写金额:不能超过最大长度,50.");
 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.SubPamountInWords).NotEmpty().WithMessage("大写金额:不能为空。");


 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentDetail =>tb_FM_PaymentDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

