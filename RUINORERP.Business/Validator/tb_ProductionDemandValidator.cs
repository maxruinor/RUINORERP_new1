
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:14
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
    /// 生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购商品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作验证类
    /// </summary>
    /*public partial class tb_ProductionDemandValidator:AbstractValidator<tb_ProductionDemand>*/
    public partial class tb_ProductionDemandValidator:BaseValidatorGeneric<tb_ProductionDemand>
    {
     public tb_ProductionDemandValidator() 
     {
      RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PDNo).MaximumLength(50).WithMessage("需要分析单号:不能超过最大长度,50.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PDNo).NotEmpty().WithMessage("需要分析单号:不能为空。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PPNo).MaximumLength(50).WithMessage("计划单号:不能超过最大长度,50.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PPNo).NotEmpty().WithMessage("计划单号:不能为空。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PPID).Must(CheckForeignKeyValue).WithMessage("计划单号:下拉选择值不正确。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");
//***** 
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.DataStatus).NotNull().WithMessage("单据状态:不能为空。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
           	                //long
                //PDID
                //tb_ProductionDemandDetail
                //RuleFor(x => x.tb_ProductionDemandDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProductionDemandDetails).NotNull();
                //RuleForEach(x => x.tb_ProductionDemandDetails).NotNull();
                //RuleFor(x => x.tb_ProductionDemandDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
                        //long
                //PDID
                //tb_ProductionDemandTargetDetail
                //RuleFor(x => x.tb_ProductionDemandTargetDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProductionDemandTargetDetails).NotNull();
                //RuleForEach(x => x.tb_ProductionDemandTargetDetails).NotNull();
                //RuleFor(x => x.tb_ProductionDemandTargetDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
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
        

        private bool DetailedRecordsNotEmpty(List<tb_PurGoodsRecommendDetail> details)
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

