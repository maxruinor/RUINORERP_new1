
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/16/2024 16:57:12
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
    /// 协作人记录表-记录内部人员介绍客户的情况验证类
    /// </summary>
    /*public partial class tb_CRM_CollaboratorValidator:AbstractValidator<tb_CRM_Collaborator>*/
    public partial class tb_CRM_CollaboratorValidator:BaseValidatorGeneric<tb_CRM_Collaborator>
    {
     public tb_CRM_CollaboratorValidator() 
     {
      RuleFor(tb_CRM_Collaborator =>tb_CRM_Collaborator.Employee_ID).Must(CheckForeignKeyValue).WithMessage("协作人:下拉选择值不正确。");
 RuleFor(tb_CRM_Collaborator =>tb_CRM_Collaborator.Customer_id).Must(CheckForeignKeyValue).WithMessage("目标客户:下拉选择值不正确。");
 RuleFor(tb_CRM_Collaborator =>tb_CRM_Collaborator.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
//***** 
 RuleFor(tb_CRM_Collaborator =>tb_CRM_Collaborator.Created_by).NotNull().WithMessage("创建人:不能为空。");
 RuleFor(tb_CRM_Collaborator =>tb_CRM_Collaborator.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

