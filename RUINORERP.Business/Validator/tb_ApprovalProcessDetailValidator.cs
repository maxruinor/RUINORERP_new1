﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:20
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
    /// 审核流程明细表验证类
    /// </summary>
    /*public partial class tb_ApprovalProcessDetailValidator:AbstractValidator<tb_ApprovalProcessDetail>*/
    public partial class tb_ApprovalProcessDetailValidator:BaseValidatorGeneric<tb_ApprovalProcessDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ApprovalProcessDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
      RuleFor(tb_ApprovalProcessDetail =>tb_ApprovalProcessDetail.ApprovalID).NotEmpty().When(x => x.ApprovalID.HasValue);


 RuleFor(tb_ApprovalProcessDetail =>tb_ApprovalProcessDetail.ApprovalResults).NotEmpty().When(x => x.ApprovalResults.HasValue);

 RuleFor(tb_ApprovalProcessDetail =>tb_ApprovalProcessDetail.ApprovalOrder).NotEmpty().When(x => x.ApprovalOrder.HasValue);

           	        Initialize();
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

