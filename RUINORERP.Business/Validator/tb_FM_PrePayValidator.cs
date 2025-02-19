
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:07
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
    /// 预付款单验证类
    /// </summary>
    /*public partial class tb_FM_PrePayValidator:AbstractValidator<tb_FM_PrePay>*/
    public partial class tb_FM_PrePayValidator:BaseValidatorGeneric<tb_FM_PrePay>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PrePayValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("往来单位:下拉选择值不正确。");

 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款类型:下拉选择值不正确。");
 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);


 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(x => x.TotalPreAmount).PrecisionScale(19,4,true).WithMessage("预付总金额:小数位不能超过4。");

 RuleFor(x => x.UnassignedAmount).PrecisionScale(19,4,true).WithMessage("剩余金额:小数位不能超过4。");

//***** 
 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.PayStatus).NotNull().WithMessage("付款状态:不能为空。");

 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.PayReason).MaximumLength(250).WithMessage("付款用途:不能超过最大长度,250.");

 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Remark).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");


 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_FM_PrePay =>tb_FM_PrePay.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //PrePayID
                //tb_FM_PrePayDetail
                //RuleFor(x => x.tb_FM_PrePayDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_PrePayDetails).NotNull();
                //RuleForEach(x => x.tb_FM_PrePayDetails).NotNull();
                //RuleFor(x => x.tb_FM_PrePayDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PrePayDetail> details)
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

