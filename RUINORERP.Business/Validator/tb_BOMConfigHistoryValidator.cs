
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:36
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
    /// BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识验证类
    /// </summary>
    /*public partial class tb_BOMConfigHistoryValidator:AbstractValidator<tb_BOMConfigHistory>*/
    public partial class tb_BOMConfigHistoryValidator:BaseValidatorGeneric<tb_BOMConfigHistory>
    {
     public tb_BOMConfigHistoryValidator() 
     {
      RuleFor(tb_BOMConfigHistory =>tb_BOMConfigHistory.VerNo).NotEmpty().WithMessage("版本号:不能为空。");
 RuleFor(tb_BOMConfigHistory =>tb_BOMConfigHistory.Notes).MaximumLength(250).WithMessage("备注说明:不能超过最大长度,250.");
 RuleFor(tb_BOMConfigHistory =>tb_BOMConfigHistory.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_BOMConfigHistory =>tb_BOMConfigHistory.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

