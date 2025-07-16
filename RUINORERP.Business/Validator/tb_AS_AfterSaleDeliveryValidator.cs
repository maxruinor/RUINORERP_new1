
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 10:05:07
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
    /// 售后交付单验证类
    /// </summary>
    /*public partial class tb_AS_AfterSaleDeliveryValidator:AbstractValidator<tb_AS_AfterSaleDelivery>*/
    public partial class tb_AS_AfterSaleDeliveryValidator:BaseValidatorGeneric<tb_AS_AfterSaleDelivery>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_AfterSaleDeliveryValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ASDeliveryNo).MaximumLength(25).WithMessage("交付单号:不能超过最大长度,25.");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ASApplyID).Must(CheckForeignKeyValueCanNull).WithMessage("售后申请单:下拉选择值不正确。");
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ASApplyID).NotEmpty().When(x => x.ASApplyID.HasValue);

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ASApplyNo).MaximumLength(25).WithMessage("申请编号:不能超过最大长度,25.");
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ASApplyNo).NotEmpty().WithMessage("申请编号:不能为空。");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("客户:下拉选择值不正确。");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目小组:下拉选择值不正确。");
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

//***** 
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.TotalDeliveryQty).NotNull().WithMessage("总交付数量:不能为空。");


 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ShippingAddress).MaximumLength(250).WithMessage("收货地址:不能超过最大长度,250.");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ShippingWay).MaximumLength(25).WithMessage("发货方式:不能超过最大长度,25.");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.TrackNo).MaximumLength(25).WithMessage("物流单号:不能超过最大长度,25.");



 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

//***** 
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(tb_AS_AfterSaleDelivery =>tb_AS_AfterSaleDelivery.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");

           	                //long?
                //ASDeliveryID
                //tb_AS_AfterSaleDeliveryDetail
                //RuleFor(x => x.tb_AS_AfterSaleDeliveryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_AS_AfterSaleDeliveryDetails).NotNull();
                //RuleForEach(x => x.tb_AS_AfterSaleDeliveryDetails).NotNull();
                //RuleFor(x => x.tb_AS_AfterSaleDeliveryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_AS_AfterSaleDeliveryDetail> details)
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

