
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 20:57:16
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
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960验证类
    /// </summary>
    /*public partial class tb_ManufacturingOrderValidator:AbstractValidator<tb_ManufacturingOrder>*/
    public partial class tb_ManufacturingOrderValidator:BaseValidatorGeneric<tb_ManufacturingOrder>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ManufacturingOrderValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.MONO).MaximumLength(50).WithMessage("制令单号:不能超过最大长度,50.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.MONO).NotEmpty().WithMessage("制令单号:不能为空。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.PDNO).MaximumLength(50).WithMessage("需求单号:不能超过最大长度,50.");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.PDCID).Must(CheckForeignKeyValueCanNull).WithMessage("自制品:下拉选择值不正确。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.PDCID).NotEmpty().When(x => x.PDCID.HasValue);

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.PDID).Must(CheckForeignKeyValueCanNull).WithMessage("需求单据:下拉选择值不正确。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.PDID).NotEmpty().When(x => x.PDID.HasValue);

//***** 
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.QuantityDelivered).NotNull().WithMessage("已交付量:不能为空。");

//***** 
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.ManufacturingQty).NotNull().WithMessage("生产数量:不能为空。");

//***** 
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Priority).NotNull().WithMessage("紧急程度:不能为空。");



 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.SKU).MaximumLength(40).WithMessage("母件SKU码:不能超过最大长度,40.");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.CNName).MaximumLength(127).WithMessage("母件品名:不能超过最大长度,127.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.CNName).NotEmpty().WithMessage("母件品名:不能为空。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.BOM_ID).Must(CheckForeignKeyValue).WithMessage("配方名称:下拉选择值不正确。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.BOM_No).MaximumLength(50).WithMessage("配方号:不能超过最大长度,50.");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.BOM_No).NotEmpty().WithMessage("配方号:不能为空。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Type_ID).Must(CheckForeignKeyValueCanNull).WithMessage("母件类型:下拉选择值不正确。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Unit_ID).Must(CheckForeignKeyValueCanNull).WithMessage("单位:下拉选择值不正确。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.CustomerPartNo).MaximumLength(50).WithMessage("客户料号:不能超过最大长度,50.");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Specifications).MaximumLength(500).WithMessage("母件规格:不能超过最大长度,500.");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.property).MaximumLength(127).WithMessage("母件属性:不能超过最大长度,127.");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Employee_ID).Must(CheckForeignKeyValue).WithMessage("制单人:下拉选择值不正确。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Location_ID).Must(CheckForeignKeyValue).WithMessage("预入库位:下拉选择值不正确。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("需求部门:下拉选择值不正确。");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.CustomerVendor_ID_Out).NotEmpty().When(x => x.CustomerVendor_ID_Out.HasValue);

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("需求客户:下拉选择值不正确。");
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);


 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.CloseCaseOpinions).MaximumLength(100).WithMessage("结案情况:不能超过最大长度,100.");

 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");

 RuleFor(x => x.ApportionedCost).PrecisionScale(19,4,true).WithMessage("分摊成本:小数位不能超过4。");

 RuleFor(x => x.TotalManuFee).PrecisionScale(19,4,true).WithMessage("总制造费用:小数位不能超过4。");

 RuleFor(x => x.TotalMaterialCost).PrecisionScale(19,4,true).WithMessage("总材料成本:小数位不能超过4。");

 RuleFor(x => x.TotalProductionCost).PrecisionScale(19,4,true).WithMessage("生产总成本:小数位不能超过4。");

 RuleFor(x => x.PeopleQty).PrecisionScale(15,5,true).WithMessage("人数:小数位不能超过5。");

 RuleFor(x => x.WorkingHour).PrecisionScale(15,5,true).WithMessage("工时:小数位不能超过5。");

 RuleFor(x => x.MachineHour).PrecisionScale(15,5,true).WithMessage("机时:小数位不能超过5。");




 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");



 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);


//***** 
 RuleFor(tb_ManufacturingOrder =>tb_ManufacturingOrder.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //MOID
                //tb_ManufacturingOrderDetail
                //RuleFor(x => x.tb_ManufacturingOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ManufacturingOrderDetails).NotNull();
                //RuleForEach(x => x.tb_ManufacturingOrderDetails).NotNull();
                //RuleFor(x => x.tb_ManufacturingOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
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

