
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:08
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。验证类
    /// </summary>
    /*public partial class tb_BOM_SValidator:AbstractValidator<tb_BOM_S>*/
    public partial class tb_BOM_SValidator:BaseValidatorGeneric<tb_BOM_S>
    {
     

     public tb_BOM_SValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_No).MaximumMixedLength(50).WithMessage("配方编号:不能超过最大长度,50.");
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_No).NotEmpty().WithMessage("配方编号:不能为空。");

 RuleFor(tb_BOM_S =>tb_BOM_S.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_Name).MaximumMixedLength(100).WithMessage("配方名称:不能超过最大长度,100.");
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_Name).NotEmpty().WithMessage("配方名称:不能为空。");

 RuleFor(tb_BOM_S =>tb_BOM_S.SKU).MaximumMixedLength(80).WithMessage("SKU码:不能超过最大长度,80.");

 RuleFor(tb_BOM_S =>tb_BOM_S.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("母件:下拉选择值不正确。");

 RuleFor(tb_BOM_S =>tb_BOM_S.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("制造部门:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_BOM_S =>tb_BOM_S.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("BOM工程师:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_BOM_S =>tb_BOM_S.Doc_ID).Must(CheckForeignKeyValueCanNull).WithMessage("工艺文件:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.Doc_ID).NotEmpty().When(x => x.Doc_ID.HasValue);

 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_S_VERID).Must(CheckForeignKeyValueCanNull).WithMessage("版本号:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_S_VERID).NotEmpty().When(x => x.BOM_S_VERID.HasValue);


//有默认值

//有默认值

 RuleFor(x => x.OutApportionedCost).PrecisionScale(19,4,true).WithMessage("外发分摊费用:小数位不能超过4。");

 RuleFor(x => x.SelfApportionedCost).PrecisionScale(19,4,true).WithMessage("自制分摊费用:小数位不能超过4。");

 RuleFor(x => x.TotalSelfManuCost).PrecisionScale(19,4,true).WithMessage("自制费用:小数位不能超过4。");

 RuleFor(x => x.TotalOutManuCost).PrecisionScale(19,4,true).WithMessage("外发费用:小数位不能超过4。");

 RuleFor(x => x.TotalMaterialCost).PrecisionScale(19,4,true).WithMessage("总物料费用:小数位不能超过4。");

 RuleFor(x => x.TotalMaterialQty).PrecisionScale(15,4,true).WithMessage("用料总量:小数位不能超过4。");

 RuleFor(x => x.OutputQty).PrecisionScale(15,4,true).WithMessage("产出量:小数位不能超过4。");

 RuleFor(x => x.PeopleQty).PrecisionScale(15,5,true).WithMessage("人数:小数位不能超过5。");

 RuleFor(x => x.WorkingHour).PrecisionScale(15,5,true).WithMessage("工时:小数位不能超过5。");

 RuleFor(x => x.MachineHour).PrecisionScale(15,5,true).WithMessage("机时:小数位不能超过5。");


 RuleFor(x => x.DailyQty).PrecisionScale(18,0,true).WithMessage("日产量:小数位不能超过0。");

 RuleFor(x => x.SelfProductionAllCosts).PrecisionScale(19,4,true).WithMessage("自产总成本:小数位不能超过4。");

 RuleFor(x => x.OutProductionAllCosts).PrecisionScale(19,4,true).WithMessage("外发总成本:小数位不能超过4。");


 RuleFor(tb_BOM_S =>tb_BOM_S.Notes).MaximumMixedLength(500).WithMessage("备注说明:不能超过最大长度,500.");


 RuleFor(tb_BOM_S =>tb_BOM_S.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_BOM_S =>tb_BOM_S.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


//***** 
 RuleFor(tb_BOM_S =>tb_BOM_S.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_BOM_S =>tb_BOM_S.ApprovalOpinions).MaximumMixedLength(500).WithMessage("审批意见:不能超过最大长度,500.");

 RuleFor(tb_BOM_S =>tb_BOM_S.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




           	                //long?
                //BOM_ID
                //tb_BOM_SDetailSecondary
                //RuleFor(x => x.tb_BOM_SDetailSecondarys).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_BOM_SDetailSecondarys).NotNull();
                //RuleForEach(x => x.tb_BOM_SDetailSecondarys).NotNull();
                //RuleFor(x => x.tb_BOM_SDetailSecondaries).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                            //long
                //BOM_ID
                //tb_BOM_SDetail
                //RuleFor(x => x.tb_BOM_SDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_BOM_SDetails).NotNull();
                //RuleForEach(x => x.tb_BOM_SDetails).NotNull();
                //RuleFor(x => x.tb_BOM_SDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProduceGoodsRecommendDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_ProdDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_ProductionDemandDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_ProductionDemandTargetDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_BOM_SDetailSecondary> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_BOM_SDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_ProductionPlanDetail> details)
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

