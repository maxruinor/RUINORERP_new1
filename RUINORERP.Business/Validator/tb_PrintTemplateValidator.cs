
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/22/2024 16:08:33
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
    /// 打印模板验证类
    /// </summary>
    /*public partial class tb_PrintTemplateValidator:AbstractValidator<tb_PrintTemplate>*/
    public partial class tb_PrintTemplateValidator:BaseValidatorGeneric<tb_PrintTemplate>
    {
     public tb_PrintTemplateValidator() 
     {
      RuleFor(tb_PrintTemplate =>tb_PrintTemplate.PrintConfigID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.PrintConfigID).NotEmpty().When(x => x.PrintConfigID.HasValue);
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Template_NO).MaximumLength(10).WithMessage("模板编号:不能超过最大长度,10.");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Template_Name).MaximumLength(50).WithMessage("模板名称:不能超过最大长度,50.");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.BizType).NotEmpty().When(x => x.BizType.HasValue);
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.BizName).MaximumLength(15).WithMessage("业务名称:不能超过最大长度,15.");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Templatet_Path).MaximumLength(100).WithMessage("模板路径:不能超过最大长度,100.");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Template_DataSource).MaximumLength(100).WithMessage("模板数据源:不能超过最大长度,100.");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.TemplateFileData).NotEmpty().WithMessage("模板文件数据:不能为空。");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

