
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:05
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
    /// 审核配置表 对于所有单据审核，并且提供明细，每个明细通过则主表通过主表中对应一个业务单据的主ID https://www.likecs.com/show-747870.html 验证类
    /// </summary>
    /*public partial class tb_ApprovalValidator:AbstractValidator<tb_Approval>*/
    public partial class tb_ApprovalValidator:BaseValidatorGeneric<tb_Approval>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ApprovalValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Approval =>tb_Approval.BillType).MaximumMixedLength(50).WithMessage("单据类型:不能超过最大长度,50.");

 RuleFor(tb_Approval =>tb_Approval.BillName).MaximumMixedLength(100).WithMessage("单据名称:不能超过最大长度,100.");

 RuleFor(tb_Approval =>tb_Approval.BillEntityClassName).MaximumMixedLength(50).WithMessage(":不能超过最大长度,50.");

 RuleFor(tb_Approval =>tb_Approval.ApprovalResults).NotEmpty().When(x => x.ApprovalResults.HasValue);


 RuleFor(tb_Approval =>tb_Approval.Module).NotEmpty().When(x => x.Module.HasValue);

           	                //long?
                //ApprovalID
                //tb_ApprovalProcessDetail
                //RuleFor(x => x.tb_ApprovalProcessDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ApprovalProcessDetails).NotNull();
                //RuleForEach(x => x.tb_ApprovalProcessDetails).NotNull();
                //RuleFor(x => x.tb_ApprovalProcessDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_ApprovalProcessDetail> details)
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

