
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
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
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度验证类
    /// </summary>
    /*public partial class tb_gl_CommentValidator:AbstractValidator<tb_gl_Comment>*/
    public partial class tb_gl_CommentValidator:BaseValidatorGeneric<tb_gl_Comment>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_gl_CommentValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_gl_Comment =>tb_gl_Comment.Employee_ID).Must(CheckForeignKeyValue).WithMessage("批注人:下拉选择值不正确。");

//***** 
 RuleFor(tb_gl_Comment =>tb_gl_Comment.BizTypeID).NotNull().WithMessage("业务类型:不能为空。");

//***** 
 RuleFor(tb_gl_Comment =>tb_gl_Comment.BusinessID).NotNull().WithMessage("关联业务:不能为空。");

 RuleFor(tb_gl_Comment =>tb_gl_Comment.DbTableName).MaximumMixedLength(100).WithMessage("关联表名:不能超过最大长度,100.");
 RuleFor(tb_gl_Comment =>tb_gl_Comment.DbTableName).NotEmpty().WithMessage("关联表名:不能为空。");

 RuleFor(tb_gl_Comment =>tb_gl_Comment.CommentContent).MaximumMixedLength(200).WithMessage("批注内容:不能超过最大长度,200.");
 RuleFor(tb_gl_Comment =>tb_gl_Comment.CommentContent).NotEmpty().WithMessage("批注内容:不能为空。");


 RuleFor(tb_gl_Comment =>tb_gl_Comment.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_gl_Comment =>tb_gl_Comment.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

