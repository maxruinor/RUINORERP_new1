
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// UI表格列设置验证类
    /// </summary>
    /*public partial class tb_UIGridColsSettingValidator:AbstractValidator<tb_UIGridColsSetting>*/
    public partial class tb_UIGridColsSettingValidator:BaseValidatorGeneric<tb_UIGridColsSetting>
    {
     public tb_UIGridColsSettingValidator() 
     {
      RuleFor(tb_UIGridColsSetting =>tb_UIGridColsSetting.FieldInfo_ID).NotEmpty().When(x => x.FieldInfo_ID.HasValue);
//***** 
 RuleFor(tb_UIGridColsSetting =>tb_UIGridColsSetting.ColDisplayIndex).NotNull().WithMessage("显示排序:不能为空。");
 RuleFor(tb_UIGridColsSetting =>tb_UIGridColsSetting.Sort).NotEmpty().When(x => x.Sort.HasValue);
 RuleFor(tb_UIGridColsSetting =>tb_UIGridColsSetting.ColWith).NotEmpty().When(x => x.ColWith.HasValue);
       	
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

