
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
    /*public partial class LogsValidator:AbstractValidator<Logs>*/
    public partial class LogsValidator:BaseValidatorGeneric<Logs>
    {
     

     public LogsValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     

 RuleFor(Logs =>Logs.Level).MaximumMixedLength(10).WithMessage("级别:不能超过最大长度,10.");

 RuleFor(Logs =>Logs.Logger).MaximumMixedLength(500).WithMessage("记录器:不能超过最大长度,500.");



 RuleFor(Logs =>Logs.Operator).MaximumMixedLength(200).WithMessage("操作者:不能超过最大长度,200.");

 RuleFor(Logs =>Logs.ModName).MaximumMixedLength(50).WithMessage("模块名:不能超过最大长度,50.");

 RuleFor(Logs =>Logs.Path).MaximumMixedLength(100).WithMessage("路径:不能超过最大长度,100.");

 RuleFor(Logs =>Logs.ActionName).MaximumMixedLength(50).WithMessage("动作:不能超过最大长度,50.");

 RuleFor(Logs =>Logs.IP).MaximumMixedLength(20).WithMessage("网络地址:不能超过最大长度,20.");

 RuleFor(Logs =>Logs.MAC).MaximumMixedLength(30).WithMessage("物理地址:不能超过最大长度,30.");

 RuleFor(Logs =>Logs.MachineName).MaximumMixedLength(50).WithMessage("电脑名:不能超过最大长度,50.");

             
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

