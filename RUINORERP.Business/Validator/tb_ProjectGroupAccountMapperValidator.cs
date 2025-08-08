
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:03
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
    /// 项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面验证类
    /// </summary>
    /*public partial class tb_ProjectGroupAccountMapperValidator:AbstractValidator<tb_ProjectGroupAccountMapper>*/
    public partial class tb_ProjectGroupAccountMapperValidator:BaseValidatorGeneric<tb_ProjectGroupAccountMapper>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ProjectGroupAccountMapperValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_ProjectGroupAccountMapper =>tb_ProjectGroupAccountMapper.ProjectGroup_ID).NotNull().WithMessage("项目组:不能为空。");

 RuleFor(tb_ProjectGroupAccountMapper =>tb_ProjectGroupAccountMapper.Account_id).Must(CheckForeignKeyValue).WithMessage("公司账户:下拉选择值不正确。");

 RuleFor(tb_ProjectGroupAccountMapper =>tb_ProjectGroupAccountMapper.Description).MaximumMixedLength(50).WithMessage("描述:不能超过最大长度,50.");



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

