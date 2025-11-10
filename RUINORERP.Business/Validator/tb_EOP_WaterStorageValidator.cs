
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:10
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
    /// 蓄水登记表验证类
    /// </summary>
    /*public partial class tb_EOP_WaterStorageValidator:AbstractValidator<tb_EOP_WaterStorage>*/
    public partial class tb_EOP_WaterStorageValidator:BaseValidatorGeneric<tb_EOP_WaterStorage>
    {
     

     public tb_EOP_WaterStorageValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.WSRNo).MaximumMixedLength(50).WithMessage("蓄水编号:不能超过最大长度,50.");
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.WSRNo).NotEmpty().WithMessage("蓄水编号:不能为空。");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.PlatformOrderNo).MaximumMixedLength(100).WithMessage("平台单号:不能超过最大长度,100.");
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.PlatformOrderNo).NotEmpty().WithMessage("平台单号:不能为空。");

//***** 
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.PlatformType).NotNull().WithMessage("平台类型:不能为空。");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Employee_ID).Must(CheckForeignKeyValue).WithMessage("业务员:下拉选择值不正确。");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");

 RuleFor(x => x.PlatformFeeAmount).PrecisionScale(19,4,true).WithMessage("平台费用:小数位不能超过4。");


 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ShippingAddress).MaximumMixedLength(500).WithMessage("收货地址:不能超过最大长度,500.");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ShippingWay).MaximumMixedLength(50).WithMessage("发货方式:不能超过最大长度,50.");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.TrackNo).MaximumMixedLength(50).WithMessage("物流单号:不能超过最大长度,50.");


 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");


//***** 
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");

 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_EOP_WaterStorage =>tb_EOP_WaterStorage.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

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

