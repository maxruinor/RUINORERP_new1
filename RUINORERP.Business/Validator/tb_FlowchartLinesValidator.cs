
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/03/2023 23:30:57
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
namespace RUINORERP.Business
{
    /// <summary>
    /// 流程图线验证类
    /// </summary>
    public partial class tb_FlowchartLinesValidator:AbstractValidator<tb_FlowchartLines>
    {
     public tb_FlowchartLinesValidator() 
     {
     RuleFor(tb_FlowchartLines =>tb_FlowchartLines.FlowchartNo).MaximumLength(50).WithMessage("流程图编号:不能超过最大长度,50.");
RuleFor(tb_FlowchartLines =>tb_FlowchartLines.FlowchartNo).NotEmpty().WithMessage("流程图编号:不能为空。");
RuleFor(tb_FlowchartLines =>tb_FlowchartLines.PointToString1).MaximumLength(100).WithMessage("大小:不能超过最大长度,100.");
RuleFor(tb_FlowchartLines =>tb_FlowchartLines.PointToString2).MaximumLength(100).WithMessage("位置:不能超过最大长度,100.");


  }

    }
}


