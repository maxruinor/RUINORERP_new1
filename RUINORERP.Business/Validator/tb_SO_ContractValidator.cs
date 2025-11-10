
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:23
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
    /// 先销售合同再订单,条款内容后面补充 注意一个合同可以多个发票一个发票也可以多个合同验证类
    /// </summary>
    /*public partial class tb_SO_ContractValidator:AbstractValidator<tb_SO_Contract>*/
    public partial class tb_SO_ContractValidator:BaseValidatorGeneric<tb_SO_Contract>
    {
     

     public tb_SO_ContractValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_SO_Contract =>tb_SO_Contract.ID).Must(CheckForeignKeyValueCanNull).WithMessage("销售方:下拉选择值不正确。");
 RuleFor(tb_SO_Contract =>tb_SO_Contract.ID).NotEmpty().When(x => x.ID.HasValue);

 RuleFor(tb_SO_Contract =>tb_SO_Contract.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("采购方:下拉选择值不正确。");
 RuleFor(tb_SO_Contract =>tb_SO_Contract.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_SO_Contract =>tb_SO_Contract.BillingInfo_ID).Must(CheckForeignKeyValueCanNull).WithMessage("开票资料:下拉选择值不正确。");
 RuleFor(tb_SO_Contract =>tb_SO_Contract.BillingInfo_ID).NotEmpty().When(x => x.BillingInfo_ID.HasValue);

 RuleFor(tb_SO_Contract =>tb_SO_Contract.TemplateId).Must(CheckForeignKeyValueCanNull).WithMessage("明细:下拉选择值不正确。");
 RuleFor(tb_SO_Contract =>tb_SO_Contract.TemplateId).NotEmpty().When(x => x.TemplateId.HasValue);

 RuleFor(tb_SO_Contract =>tb_SO_Contract.SOContractNo).MaximumMixedLength(50).WithMessage("合同编号:不能超过最大长度,50.");


 RuleFor(tb_SO_Contract =>tb_SO_Contract.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);



 RuleFor(tb_SO_Contract =>tb_SO_Contract.TotalQty).NotEmpty().When(x => x.TotalQty.HasValue);

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");



 RuleFor(tb_SO_Contract =>tb_SO_Contract.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_SO_Contract =>tb_SO_Contract.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_SO_Contract =>tb_SO_Contract.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


 RuleFor(tb_SO_Contract =>tb_SO_Contract.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_SO_Contract =>tb_SO_Contract.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //SOContractID
                //tb_SO_ContractDetail
                //RuleFor(x => x.tb_SO_ContractDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_SO_ContractDetails).NotNull();
                //RuleForEach(x => x.tb_SO_ContractDetails).NotNull();
                //RuleFor(x => x.tb_SO_ContractDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_SO_ContractDetail> details)
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

