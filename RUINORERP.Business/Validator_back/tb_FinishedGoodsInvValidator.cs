
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:34:55
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
    /// 成品入库单 要进一步完善验证类
    /// </summary>
    public partial class tb_FinishedGoodsInvValidator:AbstractValidator<tb_FinishedGoodsInv>
    {
     public tb_FinishedGoodsInvValidator() 
     {
      RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DeliveryBillNo).MaximumLength(50).WithMessage("缴库单号:不能超过最大长度,50.");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人员:下拉选择值不正确。");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("生产部门:下拉选择值不正确。");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("生产商:下拉选择值不正确。");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.ShippingWay).MaximumLength(50).WithMessage("发货方式:不能超过最大长度,50.");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.TrackNo).MaximumLength(50).WithMessage("物流单号:不能超过最大长度,50.");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Ref_BillNo).MaximumLength(50).WithMessage("引用单号:不能超过最大长度,50.");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Ref_BizType).NotEmpty().When(x => x.Ref_BizType.HasValue);
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Ref_ID).NotEmpty().When(x => x.Ref_ID.HasValue);
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.ApprovalOpinions).MaximumLength(500).WithMessage("审批意见:不能超过最大长度,500.");
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.TotalNetWorkingHours).NotNull().WithMessage("总工时:不能为空。");
 RuleFor(x => x.TotalApportionedCost).PrecisionScale(19,4,true).WithMessage("总分摊成本:小数位不能超过4。");
 RuleFor(x => x.TotalTollFees).PrecisionScale(19,4,true).WithMessage("总托工费用:小数位不能超过4。");
 RuleFor(x => x.TotalLaborCost).PrecisionScale(19,4,true).WithMessage("人工成本:小数位不能超过4。");
 RuleFor(x => x.TotalProductionCost).PrecisionScale(19,4,true).WithMessage("生产总成本:小数位不能超过4。");
 RuleFor(x => x.TotalMaterialCost).PrecisionScale(19,4,true).WithMessage("总材料成本:小数位不能超过4。");
//***** 
 RuleFor(tb_FinishedGoodsInv =>tb_FinishedGoodsInv.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
           	                //long?
                //MainID
                //tb_FinishedGoodsInvDetail
                RuleFor(c => c.tb_FinishedGoodsInvDetails).NotNull();
                RuleForEach(x => x.tb_FinishedGoodsInvDetails).NotNull();
                //RuleFor(x => x.tb_FinishedGoodsInvDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_FinishedGoodsInvDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
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

