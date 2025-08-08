
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:42
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
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本验证类
    /// </summary>
    /*public partial class tb_MaterialReturnValidator:AbstractValidator<tb_MaterialReturn>*/
    public partial class tb_MaterialReturnValidator:BaseValidatorGeneric<tb_MaterialReturn>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_MaterialReturnValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.BillNo).MaximumMixedLength(50).WithMessage("退料单号:不能超过最大长度,50.");

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.BillType).NotEmpty().When(x => x.BillType.HasValue);

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("生产部门:下拉选择值不正确。");
 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);


 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("加工厂商:下拉选择值不正确。");
 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(x => x.TotalQty).PrecisionScale(18,0,true).WithMessage("总数量:小数位不能超过0。");

 RuleFor(x => x.TotalCostAmount).PrecisionScale(18,0,true).WithMessage("总成本:小数位不能超过0。");

 RuleFor(x => x.TotalAmount).PrecisionScale(18,0,true).WithMessage("总金额:小数位不能超过0。");



 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.MR_ID).Must(CheckForeignKeyValue).WithMessage("领料单:下拉选择值不正确。");

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.MaterialRequisitionNO).MaximumMixedLength(50).WithMessage("领料单号:不能超过最大长度,50.");
 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.MaterialRequisitionNO).NotEmpty().WithMessage("领料单号:不能为空。");


//***** 
 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.ApprovalOpinions).MaximumMixedLength(500).WithMessage("审批意见:不能超过最大长度,500.");

 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);





//***** 
 RuleFor(tb_MaterialReturn =>tb_MaterialReturn.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //MRE_ID
                //tb_MaterialReturnDetail
                //RuleFor(x => x.tb_MaterialReturnDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_MaterialReturnDetails).NotNull();
                //RuleForEach(x => x.tb_MaterialReturnDetails).NotNull();
                //RuleFor(x => x.tb_MaterialReturnDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_MaterialReturnDetail> details)
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

