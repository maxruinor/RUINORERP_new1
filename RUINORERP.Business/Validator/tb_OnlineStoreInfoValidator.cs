
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:15
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
    /// 网店信息表验证类
    /// </summary>
    /*public partial class tb_OnlineStoreInfoValidator:AbstractValidator<tb_OnlineStoreInfo>*/
    public partial class tb_OnlineStoreInfoValidator:BaseValidatorGeneric<tb_OnlineStoreInfo>
    {
     

     public tb_OnlineStoreInfoValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.StoreCode).MaximumMixedLength(50).WithMessage("项目代码:不能超过最大长度,50.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.StoreName).MaximumMixedLength(50).WithMessage("项目名称:不能超过最大长度,50.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.PlatformName).MaximumMixedLength(100).WithMessage("平台名称:不能超过最大长度,100.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Contact).MaximumMixedLength(50).WithMessage("联系人:不能超过最大长度,50.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Phone).MaximumMixedLength(255).WithMessage("电话:不能超过最大长度,255.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Address).MaximumMixedLength(255).WithMessage("地址:不能超过最大长度,255.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Website).MaximumMixedLength(255).WithMessage("网址:不能超过最大长度,255.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.ResponsiblePerson).MaximumMixedLength(50).WithMessage("负责人:不能超过最大长度,50.");

 RuleFor(tb_OnlineStoreInfo =>tb_OnlineStoreInfo.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


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

