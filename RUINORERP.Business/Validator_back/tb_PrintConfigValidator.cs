
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:32
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
    /// 报表打印配置表验证类
    /// </summary>
    public partial class tb_PrintConfigValidator:AbstractValidator<tb_PrintConfig>
    {
     public tb_PrintConfigValidator() 
     {
      RuleFor(tb_PrintConfig =>tb_PrintConfig.Model_NO).MaximumLength(20).WithMessage("模板编号:不能超过最大长度,20.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Model_NO).NotEmpty().WithMessage("模板编号:不能为空。");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Model_Name).MaximumLength(100).WithMessage("模板名称:不能超过最大长度,100.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Model_Name).NotEmpty().WithMessage("模板名称:不能为空。");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Report_NO).MaximumLength(20).WithMessage("报表编号:不能超过最大长度,20.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Report_Name).MaximumLength(100).WithMessage("报表名称:不能超过最大长度,100.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Report_Path).MaximumLength(100).WithMessage("报表路径:不能超过最大长度,100.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Report_DataSource).MaximumLength(100).WithMessage("报表数据源:不能超过最大长度,100.");
       	
           	
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

