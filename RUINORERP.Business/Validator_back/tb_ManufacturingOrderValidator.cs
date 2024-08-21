
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:16
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
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960验证类
    /// </summary>
    public partial class tb_ManufacturingOrderValidator:AbstractValidator<tb_ManufacturingOrder>
    {
     public tb_ManufacturingOrderValidator() 
     {
      RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.MONO).MaximumLength(100).WithMessage("制令单号:不能超过最大长度,100.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.MONO).NotEmpty().WithMessage("制令单号:不能为空。");
//***** 
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.ManufacturingQty).NotNull().WithMessage("生产数量:不能为空。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.BOM_No).MaximumLength(100).WithMessage("配方号:不能超过最大长度,100.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.RefBillNO).MaximumLength(100).WithMessage("来源单号:不能超过最大长度,100.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.RefBillNO).NotEmpty().WithMessage("来源单号:不能为空。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.RefBillType).NotEmpty().When(x => x.RefBillType.HasValue);
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.RequirementCustomers).MaximumLength(100).WithMessage("需求客户:不能超过最大长度,100.");
 RuleFor(x => x.LaborCost).PrecisionScale(19,4,true).WithMessage("人工费:小数位不能超过4。");
 RuleFor(x => x.PeopleQty).PrecisionScale(5,3,true).WithMessage("人数:小数位不能超过3。");
 RuleFor(x => x.WorkingHour).PrecisionScale(5,3,true).WithMessage("工时:小数位不能超过3。");
 RuleFor(x => x.MachineHour).PrecisionScale(5,3,true).WithMessage("机时:小数位不能超过3。");
 RuleFor(x => x.ExternalProduceFee).PrecisionScale(19,4,true).WithMessage("托工费用:小数位不能超过4。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.ApprovalOpinions).MaximumLength(200).WithMessage("审批意见:不能超过最大长度,200.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
//***** 
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
           	                //long
                //MOID
                //tb_ManufacturingOrderDetail
                RuleFor(c => c.tb_ManufacturingOrderDetails).NotNull();
                RuleForEach(x => x.tb_ManufacturingOrderDetails).NotNull();
                //RuleFor(x => x.tb_ManufacturingOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_ManufacturingOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
     }




        private bool DetailedRecordsNotEmpty(List<tb_ManufacturingOrderDetail> details)
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

