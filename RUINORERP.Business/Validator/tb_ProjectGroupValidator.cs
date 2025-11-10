
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:19
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
    /// 项目组信息 用于业务分组小团队验证类
    /// </summary>
    /*public partial class tb_ProjectGroupValidator:AbstractValidator<tb_ProjectGroup>*/
    public partial class tb_ProjectGroupValidator:BaseValidatorGeneric<tb_ProjectGroup>
    {
     

     public tb_ProjectGroupValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.ProjectGroupCode).MaximumMixedLength(50).WithMessage("项目组代号:不能超过最大长度,50.");

 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.ProjectGroupName).MaximumMixedLength(50).WithMessage("项目组名称:不能超过最大长度,50.");

 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.ResponsiblePerson).MaximumMixedLength(50).WithMessage("负责人:不能超过最大长度,50.");

 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.Phone).MaximumMixedLength(255).WithMessage("电话:不能超过最大长度,255.");

 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ProjectGroup =>tb_ProjectGroup.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


//有默认值


           	        Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PaymentRecordDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_OtherExpenseDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_ExpenseClaimDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

