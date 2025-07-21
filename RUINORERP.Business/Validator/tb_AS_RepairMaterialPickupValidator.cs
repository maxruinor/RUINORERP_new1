
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:41
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
    /// 维修领料单验证类
    /// </summary>
    /*public partial class tb_AS_RepairMaterialPickupValidator:AbstractValidator<tb_AS_RepairMaterialPickup>*/
    public partial class tb_AS_RepairMaterialPickupValidator:BaseValidatorGeneric<tb_AS_RepairMaterialPickup>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_RepairMaterialPickupValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.RepairOrderID).Must(CheckForeignKeyValueCanNull).WithMessage("维修工单:下拉选择值不正确。");
 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.RepairOrderID).NotEmpty().When(x => x.RepairOrderID.HasValue);

 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.MaterialPickupNO).MaximumLength(25).WithMessage("领料单号:不能超过最大长度,25.");
 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.MaterialPickupNO).NotEmpty().WithMessage("领料单号:不能为空。");


 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.RepairOrderNo).MaximumLength(50).WithMessage("维修工单:不能超过最大长度,50.");
 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.RepairOrderNo).NotEmpty().WithMessage("维修工单:不能为空。");

 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(x => x.TotalPrice).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");

//***** 
 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.TotalReQty).NotNull().WithMessage("总退回数:不能为空。");

 RuleFor(x => x.TotalSendQty).PrecisionScale(10,3,true).WithMessage("总发数量:小数位不能超过3。");




 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");

 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");

 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.DataStatus).NotNull().WithMessage("数据状态:不能为空。");


//***** 
 RuleFor(tb_AS_RepairMaterialPickup =>tb_AS_RepairMaterialPickup.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //RMRID
                //tb_AS_RepairMaterialPickupDetail
                //RuleFor(x => x.tb_AS_RepairMaterialPickupDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_AS_RepairMaterialPickupDetails).NotNull();
                //RuleForEach(x => x.tb_AS_RepairMaterialPickupDetails).NotNull();
                //RuleFor(x => x.tb_AS_RepairMaterialPickupDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_AS_RepairMaterialPickupDetail> details)
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

