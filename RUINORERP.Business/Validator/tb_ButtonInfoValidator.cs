
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
    /// 字段信息表验证类
    /// </summary>
    /*public partial class tb_ButtonInfoValidator:AbstractValidator<tb_ButtonInfo>*/
    public partial class tb_ButtonInfoValidator:BaseValidatorGeneric<tb_ButtonInfo>
    {
     

     public tb_ButtonInfoValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.MenuID).Must(CheckForeignKeyValueCanNull).WithMessage("所属菜单:下拉选择值不正确。");
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.MenuID).NotEmpty().When(x => x.MenuID.HasValue);

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.BtnName).MaximumMixedLength(255).WithMessage("按钮名称:不能超过最大长度,255.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.BtnText).MaximumMixedLength(250).WithMessage("按钮文本:不能超过最大长度,250.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.HotKey).MaximumMixedLength(50).WithMessage("热键:不能超过最大长度,50.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.ButtonType).MaximumMixedLength(100).WithMessage("按钮类型:不能超过最大长度,100.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.FormName).MaximumMixedLength(255).WithMessage("窗体名称:不能超过最大长度,255.");

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.ClassPath).MaximumMixedLength(500).WithMessage("类路径:不能超过最大长度,500.");


//有默认值

 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.Notes).MaximumMixedLength(200).WithMessage("备注:不能超过最大长度,200.");


 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

