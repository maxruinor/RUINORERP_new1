
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:07
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 验证类
    /// </summary>
    /*public partial class SequenceNumbersValidator:AbstractValidator<SequenceNumbers>*/
    public partial class SequenceNumbersValidator:BaseValidatorGeneric<SequenceNumbers>
    {
     

     public SequenceNumbersValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(SequenceNumbers =>SequenceNumbers.SequenceKey).MaximumMixedLength(255).WithMessage("序号键，唯一标识一个序号序列:不能超过最大长度,255.");
 RuleFor(SequenceNumbers =>SequenceNumbers.SequenceKey).NotEmpty().WithMessage("序号键，唯一标识一个序号序列:不能为空。");

//***** 
 RuleFor(SequenceNumbers =>SequenceNumbers.CurrentValue).NotNull().WithMessage("当前序号值:不能为空。");



 RuleFor(SequenceNumbers =>SequenceNumbers.ResetType).MaximumMixedLength(20).WithMessage("重置类型: None, Daily, Monthly, Yearly:不能超过最大长度,20.");

 RuleFor(SequenceNumbers =>SequenceNumbers.FormatMask).MaximumMixedLength(50).WithMessage("格式化掩码，如 000:不能超过最大长度,50.");

 RuleFor(SequenceNumbers =>SequenceNumbers.Description).MaximumMixedLength(255).WithMessage("序列描述:不能超过最大长度,255.");

 RuleFor(SequenceNumbers =>SequenceNumbers.BusinessType).MaximumMixedLength(100).WithMessage("业务类型:不能超过最大长度,100.");

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

