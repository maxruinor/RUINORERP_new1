
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:05
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
    /// 损益费用单验证类
    /// </summary>
    /*public partial class tb_FM_ProfitLossValidator:AbstractValidator<tb_FM_ProfitLoss>*/
    public partial class tb_FM_ProfitLossValidator:BaseValidatorGeneric<tb_FM_ProfitLoss>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_ProfitLossValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.ProfitLossNo).MaximumMixedLength(30).WithMessage("单据编号:不能超过最大长度,30.");

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.SourceBizType).NotEmpty().When(x => x.SourceBizType.HasValue);

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.SourceBillId).NotEmpty().When(x => x.SourceBillId.HasValue);

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.SourceBillNo).MaximumMixedLength(30).WithMessage("来源单号:不能超过最大长度,30.");




//***** 
 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.ProfitLossType).NotNull().WithMessage("损溢类型:不能为空。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额本币:小数位不能超过4。");

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);


 RuleFor(x => x.TaxTotalAmount).PrecisionScale(19,4,true).WithMessage("税额总计:小数位不能超过4。");

 RuleFor(x => x.UntaxedTotalAmont).PrecisionScale(19,4,true).WithMessage("未税总计:小数位不能超过4。");

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.Remark).MaximumMixedLength(300).WithMessage("备注:不能超过最大长度,300.");


 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.ApprovalOpinions).MaximumMixedLength(255).WithMessage("审批意见:不能超过最大长度,255.");

 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_ProfitLoss =>tb_FM_ProfitLoss.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //ProfitLossId
                //tb_FM_ProfitLossDetail
                //RuleFor(x => x.tb_FM_ProfitLossDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_ProfitLossDetails).NotNull();
                //RuleForEach(x => x.tb_FM_ProfitLossDetails).NotNull();
                //RuleFor(x => x.tb_FM_ProfitLossDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_ProfitLossDetail> details)
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

