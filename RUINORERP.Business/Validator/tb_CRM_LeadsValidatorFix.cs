
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:15:49
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
    /// 线索机会-询盘验证类
    /// </summary>
    /*public partial class tb_CRM_LeadsValidator:AbstractValidator<tb_CRM_Leads>*/
    public partial class tb_CRM_LeadsValidator:BaseValidatorGeneric<tb_CRM_Leads>
    {
        public override void Initialize()
        {
            RuleFor(x => x.CustomerName).NotNull().WithMessage("客户名/线索名:不能为空。");
            RuleFor(x => x.CustomerName).MinimumLength(3).WithMessage("客户名/线索名:长度要大于2。");
        }
    }
 

}

