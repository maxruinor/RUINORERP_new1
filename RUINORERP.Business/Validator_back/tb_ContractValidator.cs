
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:34:43
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
    /// 先销售合同再订单,条款内容后面补充验证类
    /// </summary>
    public partial class tb_ContractValidator:AbstractValidator<tb_Contract>
    {
     public tb_ContractValidator() 
     {
      RuleFor(tb_Contract =>tb_Contract.InvoiceInfo_ID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_Contract =>tb_Contract.InvoiceInfo_ID).NotEmpty().When(x => x.InvoiceInfo_ID.HasValue);
 RuleFor(tb_Contract =>tb_Contract.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_Contract =>tb_Contract.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_Contract =>tb_Contract.ContractNo).MaximumLength(50).WithMessage("合同编号:不能超过最大长度,50.");
 RuleFor(tb_Contract =>tb_Contract.TotalQty).NotEmpty().When(x => x.TotalQty.HasValue);
 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");
 RuleFor(tb_Contract =>tb_Contract.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Contract =>tb_Contract.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_Contract =>tb_Contract.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_Contract =>tb_Contract.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
//***** 
 RuleFor(tb_Contract =>tb_Contract.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
 RuleFor(tb_Contract =>tb_Contract.Buyer).NotEmpty().When(x => x.Buyer.HasValue);
 RuleFor(tb_Contract =>tb_Contract.Seller).NotEmpty().When(x => x.Seller.HasValue);
       	
           	                //long?
                //ContractID
                //tb_ContractDetail
                RuleFor(c => c.tb_ContractDetails).NotNull();
                RuleForEach(x => x.tb_ContractDetails).NotNull();
                //RuleFor(x => x.tb_ContractDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_ContractDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
     }




        private bool DetailedRecordsNotEmpty(List<tb_ContractDetail> details)
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

