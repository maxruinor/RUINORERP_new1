
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:25
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
    /// 字段信息表验证类
    /// </summary>
    /*public partial class tb_ButtonInfoValidator:AbstractValidator<tb_ButtonInfo>*/
    public partial class tb_ButtonInfoValidator:BaseValidatorGeneric<tb_ButtonInfo>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ButtonInfoValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.MenuID).Must(CheckForeignKeyValueCanNull).WithMessage("所属菜单:下拉选择值不正确。");
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.MenuID).NotEmpty().When(x => x.MenuID.HasValue);

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.BtnName).MaximumLength(127).WithMessage("按钮名称:不能超过最大长度,127.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.BtnText).MaximumLength(125).WithMessage("按钮文本:不能超过最大长度,125.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.HotKey).MaximumLength(25).WithMessage("热键:不能超过最大长度,25.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.FormName).MaximumLength(127).WithMessage("窗体名称:不能超过最大长度,127.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.ClassPath).MaximumLength(250).WithMessage("类路径:不能超过最大长度,250.");


//有默认值

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");

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

