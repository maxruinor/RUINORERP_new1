﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:31
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
    /// 返厂售后单验证类
    /// </summary>
    /*public partial class tb_ReturnValidator:AbstractValidator<tb_Return>*/
    public partial class tb_ReturnValidator:BaseValidatorGeneric<tb_Return>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ReturnValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Return =>tb_Return.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage(":下拉选择值不正确。");


//***** 
 RuleFor(tb_Return =>tb_Return.TotalQty).NotNull().WithMessage("总数量:不能为空。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(18,0,true).WithMessage("总金额:小数位不能超过0。");

 RuleFor(tb_Return =>tb_Return.ReturnNo).MaximumLength(25).WithMessage("返厂单号:不能超过最大长度,25.");
 RuleFor(tb_Return =>tb_Return.ReturnNo).NotEmpty().WithMessage("返厂单号:不能为空。");

 RuleFor(tb_Return =>tb_Return.Reason).MaximumLength(250).WithMessage("返厂原因:不能超过最大长度,250.");



//***** 
 RuleFor(tb_Return =>tb_Return.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_Return =>tb_Return.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");

 RuleFor(tb_Return =>tb_Return.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_Return =>tb_Return.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(x => x.ShipCost).PrecisionScale(19,4,true).WithMessage("已付运费:小数位不能超过4。");


 RuleFor(tb_Return =>tb_Return.ShippingAddress).MaximumLength(127).WithMessage("发货地址:不能超过最大长度,127.");

 RuleFor(tb_Return =>tb_Return.ShippingWay).MaximumLength(25).WithMessage("发货方式:不能超过最大长度,25.");



 RuleFor(tb_Return =>tb_Return.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_Return =>tb_Return.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_Return =>tb_Return.Notes).MaximumLength(250).WithMessage("备注:不能超过最大长度,250.");

           	                //long?
                //MainID
                //tb_ReturnDetail
                //RuleFor(x => x.tb_ReturnDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ReturnDetails).NotNull();
                //RuleForEach(x => x.tb_ReturnDetails).NotNull();
                //RuleFor(x => x.tb_ReturnDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_ReturnDetail> details)
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

