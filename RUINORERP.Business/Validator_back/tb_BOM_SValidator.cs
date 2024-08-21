
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:34:38
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。验证类
    /// </summary>
    public partial class tb_BOM_SValidator:AbstractValidator<tb_BOM_S>
    {
     public tb_BOM_SValidator() 
     {
      RuleFor(tb_BOM_S =>tb_BOM_S.BOM_No).MaximumLength(50).WithMessage("配方编号:不能超过最大长度,50.");
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_No).NotEmpty().WithMessage("配方编号:不能为空。");
 RuleFor(tb_BOM_S =>tb_BOM_S.property).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_Name).MaximumLength(100).WithMessage("配方名称:不能超过最大长度,100.");
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_Name).NotEmpty().WithMessage("配方名称:不能为空。");
 RuleFor(tb_BOM_S =>tb_BOM_S.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("母件:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("制造部门:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(tb_BOM_S =>tb_BOM_S.Doc_ID).Must(CheckForeignKeyValueCanNull).WithMessage("工艺文件:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.Doc_ID).NotEmpty().When(x => x.Doc_ID.HasValue);
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_S_VERID).Must(CheckForeignKeyValueCanNull).WithMessage("版本号:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.BOM_S_VERID).NotEmpty().When(x => x.BOM_S_VERID.HasValue);
 RuleFor(tb_BOM_S =>tb_BOM_S.Specifications).MaximumLength(200).WithMessage("规格:不能超过最大长度,200.");
 RuleFor(tb_BOM_S =>tb_BOM_S.Type_ID).Must(CheckForeignKeyValueCanNull).WithMessage("产品类型:下拉选择值不正确。");
 RuleFor(tb_BOM_S =>tb_BOM_S.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
 RuleFor(x => x.OtherCost).PrecisionScale(19,4,true).WithMessage(" 其它费用:小数位不能超过4。");
 RuleFor(x => x.OutManuCost).PrecisionScale(19,4,true).WithMessage(" 托工费用:小数位不能超过4。");
 RuleFor(x => x.TotalMaterialCost).PrecisionScale(19,4,true).WithMessage("总物料费用:小数位不能超过4。");
 RuleFor(x => x.LaborCost).PrecisionScale(19,4,true).WithMessage("人工费:小数位不能超过4。");
 RuleFor(x => x.TotalMaterialQty).PrecisionScale(5,4,true).WithMessage("产出量:小数位不能超过4。");
 RuleFor(x => x.OutputQty).PrecisionScale(5,4,true).WithMessage("产出量:小数位不能超过4。");
 RuleFor(x => x.PeopleQty).PrecisionScale(5,5,true).WithMessage("人数:小数位不能超过5。");
 RuleFor(x => x.WorkingHour).PrecisionScale(5,5,true).WithMessage("工时:小数位不能超过5。");
 RuleFor(x => x.MachineHour).PrecisionScale(5,5,true).WithMessage("机时:小数位不能超过5。");
 RuleFor(x => x.DailyQty).PrecisionScale(18,0,true).WithMessage("日产量:小数位不能超过0。");
 RuleFor(x => x.SelfProductionAllCosts).PrecisionScale(19,4,true).WithMessage("生产总费用:小数位不能超过4。");
 RuleFor(x => x.OutProductionAllCosts).PrecisionScale(19,4,true).WithMessage("生产总费用:小数位不能超过4。");
 RuleFor(tb_BOM_S =>tb_BOM_S.Notes).MaximumLength(500).WithMessage("备注说明:不能超过最大长度,500.");
 RuleFor(tb_BOM_S =>tb_BOM_S.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_BOM_S =>tb_BOM_S.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
//***** 
 RuleFor(tb_BOM_S =>tb_BOM_S.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(tb_BOM_S =>tb_BOM_S.ApprovalOpinions).MaximumLength(500).WithMessage("审批意见:不能超过最大长度,500.");
 RuleFor(tb_BOM_S =>tb_BOM_S.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
       	
           	                //long
                //MainID
                //tb_BOM_SDetail
                RuleFor(c => c.tb_BOM_SDetails).NotNull();
                RuleForEach(x => x.tb_BOM_SDetails).NotNull();
                //RuleFor(x => x.tb_BOM_SDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_BOM_SDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
                        //long?
                //MainID
                //tb_BOM_SDetailSecondary
                RuleFor(c => c.tb_BOM_SDetailSecondarys).NotNull();
                RuleForEach(x => x.tb_BOM_SDetailSecondarys).NotNull();
                //RuleFor(x => x.tb_BOM_SDetailSecondarys).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_BOM_SDetailSecondaries).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
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
        

        private bool DetailedRecordsNotEmpty(List<tb_BOM_SDetail> details)
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
        

        private bool DetailedRecordsNotEmpty(List<tb_ProduceGoodsRecommendDetail> details)
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

