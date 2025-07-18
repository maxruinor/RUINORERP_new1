
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/18/2025 10:33:40
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 蓄水登记表验证类
    /// </summary>
    /*public partial class tb_EOP_WaterStorageValidator:AbstractValidator<tb_EOP_WaterStorage>*/
    public partial class tb_EOP_WaterStorageValidator:BaseValidatorGeneric<tb_EOP_WaterStorage>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_EOP_WaterStorageValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.WSRNo).MaximumLength(25).WithMessage("蓄水编号:不能超过最大长度,25.");
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.WSRNo).NotEmpty().WithMessage("蓄水编号:不能为空。");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.PlatformOrderNo).MaximumLength(50).WithMessage("平台单号:不能超过最大长度,50.");
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.PlatformOrderNo).NotEmpty().WithMessage("平台单号:不能为空。");

//***** 
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.PlatformType).NotNull().WithMessage("平台类型:不能为空。");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Employee_ID).Must(CheckForeignKeyValue).WithMessage("业务员:下拉选择值不正确。");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");

 RuleFor(x => x.PlatformFeeAmount).PrecisionScale(19,4,true).WithMessage("平台费用:小数位不能超过4。");


 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ShippingAddress).MaximumLength(250).WithMessage("收货地址:不能超过最大长度,250.");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ShippingWay).MaximumLength(25).WithMessage("发货方式:不能超过最大长度,25.");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.TrackNo).MaximumLength(25).WithMessage("物流单号:不能超过最大长度,25.");


 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");


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

