﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:33
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
    /// 工作台配置表验证类
    /// </summary>
    /*public partial class tb_WorkCenterConfigValidator:AbstractValidator<tb_WorkCenterConfig>*/
    public partial class tb_WorkCenterConfigValidator:BaseValidatorGeneric<tb_WorkCenterConfig>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_WorkCenterConfigValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_WorkCenterConfig =>tb_WorkCenterConfig.RoleID).NotNull().WithMessage("角色:不能为空。");

 RuleFor(tb_WorkCenterConfig =>tb_WorkCenterConfig.User_ID).NotEmpty().When(x => x.User_ID.HasValue);



 RuleFor(tb_WorkCenterConfig =>tb_WorkCenterConfig.ToDoList).MaximumLength(500).WithMessage("待办事项:不能超过最大长度,500.");

 RuleFor(tb_WorkCenterConfig =>tb_WorkCenterConfig.FrequentlyMenus).MaximumLength(200).WithMessage("常用菜单:不能超过最大长度,200.");

 RuleFor(tb_WorkCenterConfig =>tb_WorkCenterConfig.DataOverview).MaximumLength(500).WithMessage("数据概览:不能超过最大长度,500.");

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

