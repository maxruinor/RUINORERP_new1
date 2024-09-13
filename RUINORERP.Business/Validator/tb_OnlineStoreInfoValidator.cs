
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:57
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
    /// 网店信息表验证类
    /// </summary>
    /*public partial class tb_OnlineStoreInfoValidator:AbstractValidator<tb_OnlineStoreInfo>*/
    public partial class tb_OnlineStoreInfoValidator:BaseValidatorGeneric<tb_OnlineStoreInfo>
    {
     public tb_OnlineStoreInfoValidator() 
     {
      RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.StoreCode).MaximumLength(25).WithMessage("项目代码:不能超过最大长度,25.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.StoreName).MaximumLength(25).WithMessage("项目名称:不能超过最大长度,25.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.PlatformName).MaximumLength(50).WithMessage("平台名称:不能超过最大长度,50.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Contact).MaximumLength(25).WithMessage("联系人:不能超过最大长度,25.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Phone).MaximumLength(127).WithMessage("电话:不能超过最大长度,127.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Address).MaximumLength(127).WithMessage("地址:不能超过最大长度,127.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Website).MaximumLength(127).WithMessage("网址:不能超过最大长度,127.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.ResponsiblePerson).MaximumLength(25).WithMessage("负责人:不能超过最大长度,25.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

