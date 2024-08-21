
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:18
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
    /// 领料单(包括生产和托工)验证类
    /// </summary>
    public partial class tb_MaterialRequisitionsValidator:AbstractValidator<tb_MaterialRequisitions>
    {
     public tb_MaterialRequisitionsValidator() 
     {
      RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("生产部门:下拉选择值不正确。");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("外发厂商:下拉选择值不正确。");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.MaterialRequisitionNO).MaximumLength(50).WithMessage("领料单号:不能超过最大长度,50.");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.MaterialRequisitionNO).NotEmpty().WithMessage("领料单号:不能为空。");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.ShippingAddress).MaximumLength(255).WithMessage("发货地址:不能超过最大长度,255.");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.shippingWay).MaximumLength(50).WithMessage("发货方式:不能超过最大长度,50.");
 RuleFor(x => x.TotalPrice).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");
 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.TrackNo).MaximumLength(50).WithMessage("物流单号:不能超过最大长度,50.");
 RuleFor(x => x.ShipCost).PrecisionScale(19,4,true).WithMessage("运费:小数位不能超过4。");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.ApprovalOpinions).MaximumLength(200).WithMessage("审批意见:不能超过最大长度,200.");
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
//***** 
 RuleFor(tb_MaterialRequisitions =>tb_MaterialRequisitions.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
           	                //long
                //MainID
                //tb_MaterialRequisitionsDetail
                RuleFor(c => c.tb_MaterialRequisitionsDetails).NotNull();
                RuleForEach(x => x.tb_MaterialRequisitionsDetails).NotNull();
                //RuleFor(x => x.tb_MaterialRequisitionsDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_MaterialRequisitionsDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
     }




        private bool DetailedRecordsNotEmpty(List<tb_MaterialRequisitionsDetail> details)
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

