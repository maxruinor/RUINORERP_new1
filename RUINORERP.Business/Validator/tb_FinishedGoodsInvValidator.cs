
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:24
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
    /// 成品入库单 要进一步完善验证类
    /// </summary>
    /*public partial class tb_FinishedGoodsInvValidator:AbstractValidator<tb_FinishedGoodsInv>*/
    public partial class tb_FinishedGoodsInvValidator:BaseValidatorGeneric<tb_FinishedGoodsInv>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FinishedGoodsInvValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DeliveryBillNo).MaximumMixedLength(50).WithMessage("缴库单号:不能超过最大长度,50.");

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人员:下拉选择值不正确。");

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("生产部门:下拉选择值不正确。");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("外发工厂:下拉选择值不正确。");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);



 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.ShippingWay).MaximumMixedLength(50).WithMessage("发货方式:不能超过最大长度,50.");

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.TrackNo).MaximumMixedLength(50).WithMessage("物流单号:不能超过最大长度,50.");

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.MONo).MaximumMixedLength(50).WithMessage("制令单号:不能超过最大长度,50.");

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.MOID).Must(CheckForeignKeyValueCanNull).WithMessage("制令单:下拉选择值不正确。");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.MOID).NotEmpty().When(x => x.MOID.HasValue);

//***** 
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.TotalQty).NotNull().WithMessage("实缴数量:不能为空。");


 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");

 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);





 RuleFor(x => x.TotalNetMachineHours).PrecisionScale(15,5,true).WithMessage("总机时:小数位不能超过5。");

 RuleFor(x => x.TotalNetWorkingHours).PrecisionScale(15,5,true).WithMessage("总工时:小数位不能超过5。");

 RuleFor(x => x.TotalApportionedCost).PrecisionScale(19,4,true).WithMessage("总分摊成本:小数位不能超过4。");

 RuleFor(x => x.TotalManuFee).PrecisionScale(19,4,true).WithMessage("总制造费用:小数位不能超过4。");

 RuleFor(x => x.TotalProductionCost).PrecisionScale(19,4,true).WithMessage("生产总成本:小数位不能超过4。");

 RuleFor(x => x.TotalMaterialCost).PrecisionScale(19,4,true).WithMessage("总材料成本:小数位不能超过4。");

//***** 
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //FG_ID
                //tb_FinishedGoodsInvDetail
                //RuleFor(x => x.tb_FinishedGoodsInvDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FinishedGoodsInvDetails).NotNull();
                //RuleForEach(x => x.tb_FinishedGoodsInvDetails).NotNull();
                //RuleFor(x => x.tb_FinishedGoodsInvDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FinishedGoodsInvDetail> details)
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

