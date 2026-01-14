
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:09
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
    /// 合同模板表验证类
    /// </summary>
    /*public partial class tb_ContractTemplateValidator:AbstractValidator<tb_ContractTemplate>*/
    public partial class tb_ContractTemplateValidator:BaseValidatorGeneric<tb_ContractTemplate>
    {
     

     public tb_ContractTemplateValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ContractTemplate =>tb_ContractTemplate.TemplateName).NotEmpty().When(x => x.TemplateName.HasValue);



 RuleFor(tb_ContractTemplate =>tb_ContractTemplate.Remarks).MaximumMixedLength(500).WithMessage("备注:不能超过最大长度,500.");


 RuleFor(tb_ContractTemplate =>tb_ContractTemplate.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ContractTemplate =>tb_ContractTemplate.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

           	  
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

