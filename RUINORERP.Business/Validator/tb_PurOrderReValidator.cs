
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:21
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
    /// 采购退回单 当采购发生退回时，您可以将它记录在本系统的采购退回作业中。在实际业务中虽然发生了采购但实际货物却还未入库，采购退回作业可退回订金、退回数量处理。采购退回单可以由采购订单转入，也可以手动录入新增单据,一般没有金额变化的，可以直接作废采购单。有订单等才需要做退回验证类
    /// </summary>
    /*public partial class tb_PurOrderReValidator:AbstractValidator<tb_PurOrderRe>*/
    public partial class tb_PurOrderReValidator:BaseValidatorGeneric<tb_PurOrderRe>
    {
     

     public tb_PurOrderReValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.PurOrder_ID).NotEmpty().When(x => x.PurOrder_ID.HasValue);

 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.PurOrderNo).MaximumMixedLength(100).WithMessage("采购单号:不能超过最大长度,100.");
 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.PurOrderNo).NotEmpty().WithMessage("采购单号:不能为空。");


 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.PurReturnNo).MaximumMixedLength(50).WithMessage("退回单号:不能超过最大长度,50.");

 RuleFor(x => x.GetPayment).PrecisionScale(19,4,true).WithMessage("实际收回订金货款:小数位不能超过4。");

 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("总计税额:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总计金额:小数位不能超过4。");


 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.ReturnAddress).MaximumMixedLength(255).WithMessage("退回地址:不能超过最大长度,255.");

 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.ShippingWay).MaximumMixedLength(50).WithMessage("发货方式:不能超过最大长度,50.");

 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.TrackNo).MaximumMixedLength(50).WithMessage("物流单号:不能超过最大长度,50.");



 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");

 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");



//***** 
 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


//***** 
 RuleFor(tb_PurOrderRe =>tb_PurOrderRe.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");


 RuleFor(x => x.TotalQty).PrecisionScale(19,4,true).WithMessage("合计数量:小数位不能超过4。");

           	                //long
                //PurRetrunID
                //tb_PurOrderReDetail
                //RuleFor(x => x.tb_PurOrderReDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_PurOrderReDetails).NotNull();
                //RuleForEach(x => x.tb_PurOrderReDetails).NotNull();
                //RuleFor(x => x.tb_PurOrderReDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_PurOrderReDetail> details)
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

