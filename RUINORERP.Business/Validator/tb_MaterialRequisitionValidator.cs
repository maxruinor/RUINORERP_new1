
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:28
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
    /// 领料单(包括生产和托工)验证类
    /// </summary>
    /*public partial class tb_MaterialRequisitionValidator:AbstractValidator<tb_MaterialRequisition>*/
    public partial class tb_MaterialRequisitionValidator:BaseValidatorGeneric<tb_MaterialRequisition>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_MaterialRequisitionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.MaterialRequisitionNO).MaximumLength(25).WithMessage("领料单号:不能超过最大长度,25.");
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.MaterialRequisitionNO).NotEmpty().WithMessage("领料单号:不能为空。");

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.MONO).MaximumLength(50).WithMessage("制令单号:不能超过最大长度,50.");
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.MONO).NotEmpty().WithMessage("制令单号:不能为空。");


 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("生产部门:下拉选择值不正确。");
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("外发厂商:下拉选择值不正确。");
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.MOID).Must(CheckForeignKeyValue).WithMessage("制令单:下拉选择值不正确。");


 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.ShippingAddress).MaximumLength(127).WithMessage("发货地址:不能超过最大长度,127.");

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.shippingWay).MaximumLength(25).WithMessage("发货方式:不能超过最大长度,25.");

 RuleFor(x => x.TotalPrice).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.ExpectedQuantity).NotEmpty().When(x => x.ExpectedQuantity.HasValue);

//***** 
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.TotalSendQty).NotNull().WithMessage("实发总数:不能为空。");

//***** 
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.TotalReQty).NotNull().WithMessage("退回总数:不能为空。");


 RuleFor(x => x.ShipCost).PrecisionScale(19,4,true).WithMessage("运费:小数位不能超过4。");




 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");

 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.DataStatus).NotNull().WithMessage("数据状态:不能为空。");



//***** 
 RuleFor(tb_MaterialRequisition =>tb_MaterialRequisition.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //MR_ID
                //tb_MaterialRequisitionDetail
                //RuleFor(x => x.tb_MaterialRequisitionDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_MaterialRequisitionDetails).NotNull();
                //RuleForEach(x => x.tb_MaterialRequisitionDetails).NotNull();
                //RuleFor(x => x.tb_MaterialRequisitionDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_MaterialRequisitionDetail> details)
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

