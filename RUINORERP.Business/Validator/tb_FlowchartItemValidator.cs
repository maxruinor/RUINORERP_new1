
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:11
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
    /// 流程图子项验证类
    /// </summary>
    /*public partial class tb_FlowchartItemValidator:AbstractValidator<tb_FlowchartItem>*/
    public partial class tb_FlowchartItemValidator:BaseValidatorGeneric<tb_FlowchartItem>
    {
     

     public tb_FlowchartItemValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FlowchartItem =>tb_FlowchartItem.IconFile_Path).MaximumMixedLength(500).WithMessage(":不能超过最大长度,500.");

 RuleFor(tb_FlowchartItem =>tb_FlowchartItem.Title).MaximumMixedLength(100).WithMessage("标题:不能超过最大长度,100.");

 RuleFor(tb_FlowchartItem =>tb_FlowchartItem.SizeString).MaximumMixedLength(100).WithMessage("大小:不能超过最大长度,100.");

 RuleFor(tb_FlowchartItem =>tb_FlowchartItem.PointToString).MaximumMixedLength(100).WithMessage("位置:不能超过最大长度,100.");

           	  
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

