
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:24
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
    /// 地区表验证类
    /// </summary>
    /*public partial class tb_RegionValidator:AbstractValidator<tb_Region>*/
    public partial class tb_RegionValidator:BaseValidatorGeneric<tb_Region>
    {
     public tb_RegionValidator() 
     {
      RuleFor(tb_Region =>tb_Region.Region_Name).MaximumLength(25).WithMessage("地区名称:不能超过最大长度,25.");
 RuleFor(tb_Region =>tb_Region.Region_code).MaximumLength(10).WithMessage("地区代码:不能超过最大长度,10.");
 RuleFor(tb_Region =>tb_Region.Parent_region_id).NotEmpty().When(x => x.Parent_region_id.HasValue);
 RuleFor(tb_Region =>tb_Region.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage("意向客户:下拉选择值不正确。");
 RuleFor(tb_Region =>tb_Region.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
       	
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

