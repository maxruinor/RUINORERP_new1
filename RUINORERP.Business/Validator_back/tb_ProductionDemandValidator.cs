
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:44
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
    public partial class tb_ProductionDemandValidator:AbstractValidator<tb_ProductionDemand>
    {
     public tb_ProductionDemandValidator() 
     {
      RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Location_ID).Must(CheckForeignKeyValue).WithMessage("指定仓库:下拉选择值不正确。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PDNo).MaximumLength(100).WithMessage("计划单号:不能超过最大长度,100.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.PDNo).NotEmpty().WithMessage("计划单号:不能为空。");
//***** 
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.TotalQuantity).NotNull().WithMessage("总数量:不能为空。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.DataStatus).MaximumLength(10).WithMessage("单据状态:不能超过最大长度,10.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.DataStatus).NotEmpty().WithMessage("单据状态:不能为空。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.ApprovalOpinions).MaximumLength(200).WithMessage("审批意见:不能超过最大长度,200.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.RefBillNO).MaximumLength(100).WithMessage("来源单号:不能超过最大长度,100.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.RefBillNO).NotEmpty().WithMessage("来源单号:不能为空。");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.RefBillType).NotEmpty().When(x => x.RefBillType.HasValue);
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ProductionDemand =>tb_ProductionDemand.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
           	                //long
                //PDID
                //tb_ProductionDemandDetail
                RuleFor(c => c.tb_ProductionDemandDetails).NotNull();
                RuleForEach(x => x.tb_ProductionDemandDetails).NotNull();
                //RuleFor(x => x.tb_ProductionDemandDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_ProductionDemandDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
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
        

        private bool DetailedRecordsNotEmpty(List<tb_PurGoodsRecommendDetail> details)
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

