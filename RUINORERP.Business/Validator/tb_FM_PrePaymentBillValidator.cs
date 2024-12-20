
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
    /// 预收预付单,冲销动作会在付款单和收款单中体现验证类
    /// </summary>
    /*public partial class tb_FM_PrePaymentBillValidator:AbstractValidator<tb_FM_PrePaymentBill>*/
    public partial class tb_FM_PrePaymentBillValidator:BaseValidatorGeneric<tb_FM_PrePaymentBill>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PrePaymentBillValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     

 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);


 RuleFor(x => x.PreTotalAmount).PrecisionScale(19,4,true).WithMessage("预交易总金额:小数位不能超过4。");

 RuleFor(x => x.PrePaidTotalAmount).PrecisionScale(19,4,true).WithMessage("已预交易总金额:小数位不能超过4。");


 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.Remark).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");


 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_FM_PrePaymentBill =>tb_FM_PrePaymentBill.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //PrePaymentBill_id
                //tb_FM_PrePaymentBillDetail
                //RuleFor(x => x.tb_FM_PrePaymentBillDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_PrePaymentBillDetails).NotNull();
                //RuleForEach(x => x.tb_FM_PrePaymentBillDetails).NotNull();
                //RuleFor(x => x.tb_FM_PrePaymentBillDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PrePaymentBillDetail> details)
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

