﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/29/2025 20:39:10
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
    /// 行级权限规则-用户关联表验证类
    /// </summary>
    /*public partial class tb_P4RowAuthPolicyByUserValidator:AbstractValidator<tb_P4RowAuthPolicyByUser>*/
    public partial class tb_P4RowAuthPolicyByUserValidator:BaseValidatorGeneric<tb_P4RowAuthPolicyByUser>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_P4RowAuthPolicyByUserValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_P4RowAuthPolicyByUser =>tb_P4RowAuthPolicyByUser.PolicyId).Must(CheckForeignKeyValue).WithMessage("数据权限规则:下拉选择值不正确。");

 RuleFor(tb_P4RowAuthPolicyByUser =>tb_P4RowAuthPolicyByUser.MenuID).Must(CheckForeignKeyValue).WithMessage("菜单:下拉选择值不正确。");

 RuleFor(tb_P4RowAuthPolicyByUser =>tb_P4RowAuthPolicyByUser.User_ID).Must(CheckForeignKeyValue).WithMessage("用户:下拉选择值不正确。");

//有默认值


 RuleFor(tb_P4RowAuthPolicyByUser =>tb_P4RowAuthPolicyByUser.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_P4RowAuthPolicyByUser =>tb_P4RowAuthPolicyByUser.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

